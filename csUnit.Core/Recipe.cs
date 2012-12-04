#region Copyright © 2012 by Agile Utilities New Zealand Ltd. All rights reserved.
////////////////////////////////////////////////////////////////////////////////
//
// Copyright © 2012 by Agile Utilities New Zealand Ltd. All rights reserved.
//                  http://www.agileutilities.com
//
// Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, 
//                       and Piers Lawson. All rights reserved.
//
// This software is provided 'as-is', without any express or implied warranty. 
// In no event will the authors be held liable for any damages arising from the
// use of this software.
//
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim 
//    that you wrote the original software. If you use this software in a 
//    product, an acknowledgment in the product documentation would be 
//    appreciated but is not required.
//
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using csUnit.Common;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Recipe describes a series of assemblies, which contain tests. When a 
   /// recipe is loaded, the tests in the assemblies can be executed in the same
   /// way as a recipe.
   /// </summary>
   /// <remarks>The recipe can be viewed as the document type, csUnitRunner works
   /// with.</remarks>
   internal sealed class Recipe : IRecipe {
      #region Events
      /// <summary>
      /// Raised just after the running any tests has been aborted by invoking
      /// the Abort method on this recipe.
      /// </summary>
      public event RecipeEventHandler        Aborted;

      /// <summary>
      /// Raised immediately before execution of tests in the recipe starts.
      /// </summary>
      public event RecipeEventHandler        Started;
      
      /// <summary>
      /// Raised after all tests in the recipe have been executed.
      /// </summary>
      public event RecipeEventHandler        Finished;

      /// <summary>
      /// Raised after the recipe has been successfully saved.
      /// </summary>
      public event RecipeEventHandler        Saved;

      /// <summary>
      /// Raised when the settings on a selector have been modified.
      /// </summary>
      public event RecipeEventHandler        SelectorModified;

      /// <summary>
      /// Raised when an assembly was added to the recipe.
      /// </summary>
      public event AssemblyEventHandler AssemblyAdded;

      /// <summary>
      /// Raised when an assembly has been removed from the recipe.
      /// </summary>
      public event AssemblyEventHandler AssemblyRemoved;

      /// <summary>
      /// Raised when an assembly is about to be removed from the recipe.
      /// </summary>
      public event AssemblyEventHandler      AssemblyRemoving;
      #endregion // Events

      private Recipe() {
         _onAssemblyFinished = OnAssemblyFinished;
         _onTestsAborted = OnTestsAborted;
      }

      /// <summary>
      /// Loads the recipe from an XML document.
      /// </summary>
      /// <param name="doc">The XML document from which to read the recipe.</param>
      /// <returns>A List of FileNotFoundExceptions. The list is empty if all
      /// assemblies were found.</returns>
      public List<FileNotFoundException> LoadFromXml(IXmlDocument doc) {
         var fileNotFounds = new List<FileNotFoundException>();

         var nodes = doc.SelectNodes("recipe/assembly");
         var pathAttributeName = "path";
         if(nodes.Count == 0) { // backwards compatibility for reading
            nodes = doc.SelectNodes("Recipe/Assembly");
            pathAttributeName = "Path";
         }

         foreach(XmlNode node in nodes) {
            var assemblyPathName = node.Attributes[pathAttributeName].Value;
            try {
               AddAssembly(assemblyPathName);
            }
            catch(FileNotFoundException ex) {
               fileNotFounds.Add(ex);
            }
         }
         RetrieveFilterData(doc.SelectNodes("recipe/selector"));
         MarkModified(false);

         return fileNotFounds;
      }

      /// <summary>
      /// Gets/Sets the TextWriter to which any console output is being written.
      /// </summary>
      public void SetConsoleOutputTo(TextWriter value) {
         _twConsole = value;
         foreach(var testAssembly in Assemblies) {
            testAssembly.SetConsoleOutputTo(_twConsole);
         }
      }

      /// <summary>
      /// Add an assembly. Calling the method twice for the same assembly does
      /// not cause a duplicate.
      /// </summary>
      /// <param name="assemblyPathName">Full path and name of the assembly.</param>
      /// <exception cref="FileNotFoundException">Thrown when the file could not
      /// be found.</exception>
      /// <exception cref="ArgumentException">Thrown when 'assemblyPathName'
      /// doesn't refer to a DLL or EXE.</exception>
      public void AddAssembly(string assemblyPathName) {
         var absolutePath = GetAbsolutePathFor(assemblyPathName);
         if(!Path.IsPathRooted(absolutePath)) {
            throw new ArgumentException("Path must be absolute.", "assemblyPathName");
         }

         foreach(var testAssembly in Assemblies) {
            if(testAssembly.Name.CodeBase == absolutePath) {
               return;
            }
         }

         var addedTestAssembly = LoaderFactory.CreateInstance(absolutePath);
         AddAssembly(addedTestAssembly);
      }

      private string GetAbsolutePathFor(string assemblyPathName) {
         var absolutePath = assemblyPathName;
         if(PathName != string.Empty) {
            var fileInfo = new FileInfo(PathName);
            if (fileInfo.Directory != null) {
               absolutePath = Util.GetAbsoluteFilename(fileInfo.Directory.FullName, assemblyPathName);
            }
         }
         else if(!absolutePath.ToLower().StartsWith("file:///")) {
            absolutePath = (new FileInfo(assemblyPathName)).FullName;
         }
         var uri = new Uri(absolutePath);
         absolutePath = uri.LocalPath;
         return absolutePath;
      }

      internal void AddAssembly(ITestAssembly addedTestAssembly) {
         addedTestAssembly.SetConsoleOutputTo(_twConsole);

         HookupAssembly(addedTestAssembly);

         _testAssemblies.Add(addedTestAssembly);
         IsModified = true;

         if(addedTestAssembly.Name.FullName != string.Empty) {
            if(AssemblyAdded != null) {
               AssemblyAdded(this,
                  new AssemblyEventArgs(addedTestAssembly.Name.FullName,
                     addedTestAssembly.Name.CodeBase));
            }
         }
      }

      /// <summary>
      /// Gets the set of filters registered for the recipe.
      /// </summary>
      public Set<ISelector> Selectors {
         get {
            return _filters;
         }
      }

      private void HookupAssembly(ITestAssembly addedTestAssembly) {
         addedTestAssembly.AssemblyFinished += _onAssemblyFinished;
         addedTestAssembly.TestsAborted += _onTestsAborted;
         addedTestAssembly.AssemblyChanged += AddedTestAssemblyAssemblyChanged;
         InvalidateTestCount();
      }

      private void AddedTestAssemblyAssemblyChanged(object sender, AssemblyEventArgs args) {
         InvalidateTestCount();
      }

      private void UnhookAssembly(ITestAssembly testAssembly) {
         testAssembly.AssemblyFinished -= _onAssemblyFinished;
         testAssembly.TestsAborted -= _onTestsAborted;
         InvalidateTestCount();
      }

      private void InvalidateTestCount() {
         _testCount = -1;
      }

      /// <summary>
      /// Gets an array with the test assemblies in the recipe.
      /// </summary>
      public ITestAssembly[] Assemblies {
         get {
            return _testAssemblies.ToArray();
         }
      }

      /// <summary>
      /// Gets the test assembly object given a full name or a path file name.
      /// Returns null if not found.
      /// </summary>
      public ITestAssembly this[string assemblyFullOrPathFileName] {
         get {
            foreach(var testAssembly in Assemblies) {
               if(testAssembly.Name.FullName == assemblyFullOrPathFileName
                  || testAssembly.Name.CodeBase == assemblyFullOrPathFileName) {
                  return testAssembly;
               }
            }
            return null;
         }
      }

      /// <summary>
      /// Remove all assemblies from the recipe.
      /// </summary>
      public void Clear() {
         var pathNames = new List<String>();
         foreach(var testAssembly in Assemblies) {
            pathNames.Add(testAssembly.Name.CodeBase);
         }
         foreach(var pathName in pathNames) {
            RemoveAssembly(pathName);
         }
      }

      /// <summary>
      /// Gets/sets the path and name of the recipe.
      /// </summary>
      private string PathName {
         get {
            return _recipePathName;
         }
      }

      /// <summary>
      /// Gets the number of assemblies in the recipe.
      /// </summary>
      public int AssemblyCount {
         get {
            return _testAssemblies.Count;
         }
      }

      /// <summary>
      /// Gets a displayable name for the recipe.
      /// </summary>
      public string DisplayName {
         get {
            if( IsNew ) {
               return "Untitled";
            }
            var lastSlash = _recipePathName.LastIndexOfAny(new[] {'/', '\\'});
            return lastSlash != -1 ? _recipePathName.Substring(lastSlash + 1) : _recipePathName;
         }
      }

      /// <summary>
      /// Returns an enumerator for the assemblies in the recipe.
      /// </summary>
      /// <returns>An enumerator.</returns>
      public IEnumerator<ITestAssembly> GetEnumerator() {
         return _testAssemblies.GetEnumerator();
      }

      /// <summary>
      /// Indicates, whether the recipe has been saved before.
      /// </summary>
      public bool IsNew {
         get {
            return _recipePathName == string.Empty;
         }
      }

      /// <summary>
      /// Is false, when the recipe has not been modified since the last save
      /// operation. True otherwise. Used to determine, whether the user should
      /// be asked whether or not to save the modified recipe.
      /// </summary>
      public bool Modified {
         get {
            return IsModified;
         }
      }

      /// <summary>
      /// Register a selector to be used by this recipe.
      /// </summary>
      /// <param name="selector">The selector.</param>
      public void RegisterSelector(ISelector selector) {
         if (null == selector) {
            return;
         }
         InvalidateTestCount();
         _filters.Add(selector);
         selector.Modified += OnSelectorModified;
         RecipeFactory.RegisterSelector(selector);
      }

      private void OnSelectorModified(object sender, EventArgs e) {
         IsModified = true;
         InvalidateTestCount();
         if(null != SelectorModified) {
            SelectorModified(this, new RecipeEventArgs());
         }
      }

      /// <summary>
      /// Removes an assembly from the recipe.
      /// </summary>
      /// <param name="assemblyFullName">The path and name or the full name of 
      /// the assembly to be removed.</param>
      /// <remarks>If the assembly is not in the recipe no error is returned.
      /// </remarks>
      public void RemoveAssembly(string assemblyFullName) {
         foreach(var testAssembly in _testAssemblies) {
            if(   testAssembly.Name.CodeBase == assemblyFullName
               || testAssembly.Name.FullName == assemblyFullName ) {
               UnhookAssembly(testAssembly);
               IsModified = true;

               if(AssemblyRemoving != null) {
                  AssemblyRemoving(this, new AssemblyEventArgs(testAssembly.Name.FullName, testAssembly.Name.CodeBase));
               }
               _testAssemblies.Remove(testAssembly);
               if(AssemblyRemoved != null) {
                  AssemblyRemoved(this, new AssemblyEventArgs(testAssembly.Name.FullName, testAssembly.Name.CodeBase));
               }
               break;
            }
         }
      }

      /// <summary>
      /// Called when all configured tests of an assembly have been executed.
      /// </summary>
      /// <param name="sender">The sender of the event.</param>
      /// <param name="args">Arguments</param>
      private void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
         if( _queuedAssemblies.Count > 0 ) {
            RunTests(_queuedAssemblies.Dequeue(), _testRun);
         }
         else {
            TestsRunning = false;
            if( Finished != null ) {
               Finished(this, new RecipeEventArgs());
            }
         }
      }

      private void OnTestsAborted(object sender, AssemblyEventArgs args) {
         TestsRunning = false;
         if( Aborted != null ) {
            Aborted(this, new RecipeEventArgs());
         }
      }

      /// <summary>
      /// Saves the recipe. This requires, that the object was created either
      /// using RecipeFactory.Load(string) or Recipe(string).
      /// </summary>
      public void Save() {
         if( _recipePathName == string.Empty) {
            // TODO: We shouldn't throw this exception here since it is
            // surprising. This method doesn't have a parameter! [07may08, ml]
            throw new ArgumentNullException("_filePathName", 
               "Cannot save recipe without file name");
         }
         Save(_recipePathName);
      }

      /// <summary>
      /// Saves the recipe to a file.
      /// </summary>
      /// <param name="pathName">Path and name of the recipe file.</param>
      public void Save(string pathName) {
         if( !Path.IsPathRooted(pathName) ) {
            throw new ArgumentException("Path must be rooted.", "pathName");
         }

         var doc = XmlDocumentFactory.CreateInstance();
         var recipeElem = doc.CreateElement("recipe");
         doc.AppendChild(recipeElem);

         FileInfo fi;
         try {
            fi = new FileInfo(pathName);

            foreach( var testAssembly in Assemblies ) {
               var assemblyElem = doc.CreateElement("assembly");
               var attr = doc.CreateAttribute("path");
               if (fi.Directory != null) {
                  var url = new Uri(testAssembly.Name.CodeBase);
                  attr.Value = Util.GetRelativeFilename(fi.Directory.FullName, url.AbsolutePath);
               }
               assemblyElem.Attributes.Append(attr);
               recipeElem.AppendChild(assemblyElem);
            }

            SaveFilters(recipeElem);

            doc.Save(pathName);
            _recipePathName = pathName;
            IsModified = false;
            if( Saved != null ) {
               Saved(this, new RecipeEventArgs());
            }
         }
         catch( Exception ex ) {
            Console.WriteLine(ex.Message);
         }
      }

      private void SaveFilters(XmlNode parent) {
         foreach(var filter in _filters) {
            var filterElement = parent.OwnerDocument.CreateElement("selector");
            var filterData = filter.Serialize();
            if(null != filterData) {
               parent.AppendChild(filterElement);
               filterElement.SetAttribute("selectorClass", filter.GetType().FullName);
               var imported = parent.OwnerDocument.ImportNode(filterData, true);
               filterElement.AppendChild(imported);
            }
         }
      }

      private void RetrieveFilterData(XmlNodeList filterNodes) {
         foreach(XmlElement element in filterNodes) {
            var typeName = element.Attributes["selectorClass"].Value;
            var data = element.InnerXml;
            _filterData.Add(typeName, data);
         }
         foreach(var filter in _filters) {
            if(_filterData.ContainsKey(filter.GetType().FullName)) {
               filter.Deserialize(_filterData[filter.GetType().FullName]);
            }
         }
      }

      /// <summary>
      /// Closes the recipe by unloading all assemblies.
      /// </summary>
      public void Close() {
         foreach(var testAssembly in _testAssemblies) {
            UnhookAssembly(testAssembly);
         }
         _testAssemblies.Clear();
      }

      #region Test Running Methods
      /// <summary>
      /// Executes tests specified as a test run. The TestRun object contains
      /// information about what tests to execute and how.
      /// </summary>
      /// <param name="testRun">Information about the test run.</param>
      public void RunTests(ITestRun testRun) {
         if (_testAssemblies.Count > 0) {
            _testRun = testRun;
            PrepareTestRun();
            RunTests(_queuedAssemblies.Dequeue(), _testRun);
         }
      }

      private void PrepareTestRun() {
         TestsRunning = true;
         if( Started != null ) {
            Started(this, new RecipeEventArgs());
         }

         _queuedAssemblies.Clear();
         foreach(var testAssembly in _testAssemblies) {
            _queuedAssemblies.Enqueue(testAssembly);
         }
      }

      private readonly Queue<ITestAssembly> _queuedAssemblies = new Queue<ITestAssembly>();

      private void RunTests(ITestAssembly testAssembly, ITestRun testRun) {
         if( testAssembly != null ) {
            try {
               //testAssembly.Refresh();
               testAssembly.RunTests(testRun);
            }
            catch( Exception ex ) {
               // Invoke the abort event b/c an exception that goes unhandled 
               // here is the equivalent of an aborted recipe.
               var recipeEventArgs = new RecipeEventArgs(ex.Message);
               TestsRunning = false;
               if( Aborted != null ) {
                  Aborted(this, recipeEventArgs);
               }
            }
         }
      }
      #endregion

      /// <summary>
      /// Aborts all running tests. Calling when no test is running has no 
      /// effect.
      /// </summary>
      public void Abort() {
         foreach(var testAssembly in _testAssemblies) {
            testAssembly.Abort();
         }
         TestsRunning = false;
         if( Aborted != null ) {
            Aborted(this, new RecipeEventArgs());
         }
      }

      /// <summary>
      /// Gets a value indicating whether tests are currently running.
      /// </summary>
      public bool TestsRunning { get; private set; }


      /// <summary>
      /// When invoked waits until the tests have completed, then returns.
      /// </summary>
      public void Join() {
         while (TestsRunning) {
            Thread.SpinWait(10);
            Thread.Sleep(200);
         }
      }

      /// <summary>
      /// Returns the count of tests.
      /// </summary>
      /// <returns>Number of tests.</returns>
      public int CountTests() {
         if( _testCount == -1 ) {
            _testCount = 0;
            foreach(var testAssembly in Assemblies) {
               foreach(var testFixtureInfo in testAssembly.TestFixtureInfos) {
                  foreach(var testMethodInfo in testFixtureInfo.TestMethods) {
                     if(IsIncluded(testMethodInfo)) {
                        _testCount++;
                     }
                  }
               }
            }
         }
         return _testCount;
      }

      private bool IsIncluded(ITestMethodInfo testMethodInfo) {
         foreach(var filter in _filters) {
            if(!filter.Includes(testMethodInfo)) {
               return false;
            }
         }
         return true;
      }

      /// <summary>
      /// Gets a set of all categories found in the recipe.
      /// </summary>
      public Categories Categories {
         get {
            lock(this) {
               _categories.Clear();
               foreach(var testAssembly in Assemblies) {
                  foreach(TestFixtureInfo tfi in testAssembly.TestFixtureInfos) {
                     _categories.Add(tfi.Categories);
                     foreach(TestMethodInfo tmi in tfi.TestMethods) {
                        _categories.Add(tmi.Categories);
                     }
                  }
               }
            }
            return _categories;
         }
      }

      private void MarkModified(bool modified) {
         IsModified = modified;
      }

      #region IDisposable Members

      public void Dispose() {
         foreach(ITestAssembly ta in _testAssemblies) {
            ta.Dispose();
         }
      }

      #endregion

      #region IRecipe Members

      string IRecipe.PathName {
         get {
            return _recipePathName;
         }
         set {
            _recipePathName = value;
         }
      }

      #endregion

      #region Fields
      private bool IsModified { get; set; }

      private readonly Dictionary<string, string> _filterData = new Dictionary<string, string>();
      private readonly Set<ISelector> _filters = new Set<ISelector>();
      private readonly Categories _categories = new Categories();
      private readonly List<ITestAssembly> _testAssemblies = new List<ITestAssembly>();
      private ITestRun     _testRun;
      private String       _recipePathName = string.Empty;
      private TextWriter   _twConsole;
      private readonly AssemblyEventHandler _onAssemblyFinished;
      private readonly AssemblyEventHandler _onTestsAborted;
      private int _testCount = -1;
      #endregion // Fields
   }
}
