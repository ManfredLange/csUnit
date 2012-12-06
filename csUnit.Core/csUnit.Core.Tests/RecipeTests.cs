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
using System.IO;
using System.Reflection;
using System.Xml;
using csUnit.Common;
using csUnit.Core.Criteria;
using csUnit.Interfaces;
using csUnit.Interfaces.Criteria;

namespace csUnit.Core.Tests {
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local

   /// <summary>
   /// RecipeTest. Just Recipe tests. For UI tests, see class RecipeUITest.
   /// </summary>
   [TestFixture]
   public class RecipeTests {
      [FixtureSetUp]
      public void Initialize() {
         RecipeFactory.Type = RecipeFactory.Default;
         var url = new Uri(GetType().Assembly.CodeBase);
         var fi = new FileInfo(url.AbsolutePath);
         _path = fi.DirectoryName;
// ReSharper disable AssignNullToNotNullAttribute
         _csUnitTestExe = Path.Combine(_path, "csUnitTest.exe");
// ReSharper restore AssignNullToNotNullAttribute

         _absoluteRecipeFileName = Core.Util.GetAbsoluteFilename(Environment.CurrentDirectory, "test.recipe");
      }

      [TearDown]
      public void TearDown() {
         XmlDocumentFactory.Type = XmlDocumentFactory.Default;
         LoaderFactory.Type = LoaderFactory.Default;
         XmlDocumentMock.Reset();
      }

      [Test]
      public void DefaultRecipeEmpty() {
         var r = RecipeFactory.NewRecipe(string.Empty);
         Assert.Equals(0, r.AssemblyCount);
      }

      [Test]
      public void AddAssembly() {
         LoaderFactory.Type = typeof(LoaderMock);
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         Assert.Equals(1, r.AssemblyCount);
      }

      [Test]
      public void AddSameAssemblyTwice() {
         LoaderFactory.Type = typeof(LoaderMock);
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         r.AddAssembly(_csUnitTestExe);
         Assert.Equals(1, r.AssemblyCount);
      }

      [Test]
      public void RemoveAssembly() {
         LoaderFactory.Type = typeof(LoaderMock);
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         r.RemoveAssembly(_csUnitTestExe);
         Assert.Equals(0, r.AssemblyCount);
      }

      [Test]
      public void NonContainedAssembly() {
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.RemoveAssembly("abc");
      }

      [Test]
      public void SaveRetrieve() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         LoaderFactory.Type = typeof(LoaderMock);

         var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         r.Save(path + "\\SaveRetrieve.recipe");

         var retrieved = RecipeFactory.Load(path + "\\SaveRetrieve.recipe");
         Assert.Equals(1, retrieved.AssemblyCount);
      }

      [Test]
      public void ForEach() {
         LoaderFactory.Type = typeof(LoaderMock);
         var required = new[] {_csUnitTestExe, GetType().Assembly.Location };
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(required[0]);
         r.AddAssembly(required[1]);

         foreach(var ta in r) {
            Assert.Contains(ta.Name.CodeBase, required);
         }
      }

      private class MyEventHandler {
         public void OnRecipeLoaded(object sender, RecipeEventArgs args) {
            Loaded = true;
         }

         public bool Loaded { get; private set; }
      }

      [Test]
      public void RecipeLoadedEventFired() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         XmlDocumentMock.PathName = "test.recipe";
         XmlDocumentMock.RawContent = "<recipe></recipe>";
         LoaderFactory.Type = typeof(LoaderMock);
         var handler = new MyEventHandler();
         RecipeFactory.Loaded += handler.OnRecipeLoaded;

         RecipeFactory.Load("test.recipe");
         Assert.True(handler.Loaded);
      }

      [Test]
      public void StoresRelativePaths() {
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         LoaderFactory.Type = typeof(LoaderMock);
         var codeBase = GetType().Assembly.CodeBase;

         var url = new Uri(codeBase);
         var assemblyPathName = url.AbsolutePath;

         var fi = new FileInfo(assemblyPathName);
         var path = fi.DirectoryName;

         var src = RecipeFactory.NewRecipe(Path.Combine(path, "StoresRelativePaths.recipe"));
         src.Clear();
         src.AddAssembly(assemblyPathName);
         src.Save();

         var doc = XmlDocumentFactory.CreateInstance();
         doc.Load(Path.Combine(path, "StoresRelativePaths.recipe"));
         var elem = doc["recipe"]["assembly"];
         Assert.Equals("csUnit.Core.Tests.DLL", elem.Attributes["path"].Value);
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper restore PossibleNullReferenceException
      }

      [Test]
      public void FindAssemblyByName() {
         LoaderFactory.Type = typeof(LoaderMock);
         var r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         var ta = r[_csUnitTestExe];
         Assert.Equals(ta.Name.CodeBase, _csUnitTestExe);
      }

      [Test]
      public void FindAssemblyByNameWithUnknownName() {
         LoaderFactory.Type = typeof(LoaderMock);
         IRecipe r = RecipeFactory.NewRecipe(string.Empty);
         r.AddAssembly(_csUnitTestExe);
         var ta = r["fluffy duck"];
         Assert.Null(ta);
      }

      [Test]
      public void FindAssemblyByNameInEmptyRecipe() {
         var r = RecipeFactory.NewRecipe(string.Empty);
         var ta = r[_csUnitTestExe];
         Assert.Null(ta);
      }

      private class MyEventHandler2 {
         public void OnRecipeLoaded(object sender, RecipeEventArgs args) {
            _events += "OnRecipeLoaded";
         }
         public void OnRecipeClosing(object sender, RecipeEventArgs args) {
            _events += "OnRecipeClosing";
         }
         public void OnRecipeLoadFailed(object sender, RecipeEventArgs args) {
            _events += "OnRecipeLoadFailed";
         }

         private string _events = string.Empty;

         public string Events {
            get { return _events; }
         }
      }

      [Test]
      public void FiresClosingLoadedEventsWithNewInstance() {
         var eventHandler = new MyEventHandler2();
         RecipeFactory.Loaded += eventHandler.OnRecipeLoaded;
         RecipeFactory.Closing += eventHandler.OnRecipeClosing;
         RecipeFactory.NewRecipe(string.Empty);
         Assert.Equals("OnRecipeClosingOnRecipeLoaded", eventHandler.Events);
      }

      [Test]
      public void FiresClosingLoadedEventsWithLoad() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         XmlDocumentMock.PathName = _absoluteRecipeFileName;
         XmlDocumentMock.RawContent = "<recipe></recipe>";
         var eventHandler = new MyEventHandler2();
         RecipeFactory.Loaded += eventHandler.OnRecipeLoaded;
         RecipeFactory.Closing += eventHandler.OnRecipeClosing;
         RecipeFactory.Load(_absoluteRecipeFileName);
         Assert.Equals("OnRecipeClosingOnRecipeLoaded", eventHandler.Events);
      }

      [Test]
      public void RecipeCurrentIsUsefulForInvalidRecipePathName() {
         var eventHandler = new MyEventHandler2();
         RecipeFactory.Loaded += eventHandler.OnRecipeLoaded;
         RecipeFactory.LoadFailed += eventHandler.OnRecipeLoadFailed;
         Assert.NotNull(RecipeFactory.Current);
         RecipeFactory.Load("invalid.recipe");
         Assert.NotNull(RecipeFactory.Current);
         Assert.Equals("OnRecipeLoadFailed", eventHandler.Events);
      }

      [Test]
      public void CanAddAssemblyByUri() {
         LoaderFactory.Type = typeof(LoaderMock);
         var recipe = RecipeFactory.NewRecipe(string.Empty);
         var assembly = GetType().Assembly;
         var url = new Uri(assembly.CodeBase);
         recipe.AddAssembly(assembly.CodeBase);
         Assert.Equals(1, recipe.Assemblies.Length);
         Assert.Contains(url.LocalPath, recipe.Assemblies[0].Name.CodeBase);
      }

      [TestFixture]
      private class ATestFixture {
         [Test]
         public void Foo() {
         }

         [Test(Categories = "Red")]
         public void Bar() {
         }
      }

      private class MockAssembly : ITestAssembly {
         #region ITestAssembly Members
#pragma warning disable 67
         public event AssemblyEventHandler AssemblyLoaded;
         
         public event AssemblyEventHandler AssemblyChanged;

         public event AssemblyEventHandler AssemblyStarted;

         public event AssemblyEventHandler AssemblyFinished;

         public event AssemblyEventHandler TestsAborted;

         public event TestEventHandler TestStarted;

         public event TestEventHandler TestPassed;

         public event TestEventHandler TestFailed;

         public event TestEventHandler TestError;

         public event TestEventHandler TestSkipped;
#pragma warning disable 67
         public AssemblyName Name {
            get {
               return new AssemblyName {
                  CodeBase = "c:/MockAssembly.dll",
                  Name = "MockAssembly"
                                       };
            }
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         public TestFixtureInfoCollection TestFixtureInfos {
            get {
// ReSharper disable UseObjectOrCollectionInitializer
               var tfic = new TestFixtureInfoCollection();
// ReSharper restore UseObjectOrCollectionInitializer
               tfic.Add(new TestFixtureInfo(new TestFixture(typeof(ATestFixture))));
               return tfic;
            }
         }

         public void SetConsoleOutputTo(TextWriter value) {}

         public void Refresh() {
         }

         public void RunTests(ITestRun testRun) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Abort() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }
         
         #endregion

         public Set<ISelector> Filters {
            get {
               return _filters;
            }
         }

         private readonly Set<ISelector> _filters = new Set<ISelector>();
      }

      private class MyCriterion : MarshalByRefObject, ICriterion {
         public bool HasBeenCalled { get; private set; }

         #region Implementation of ICriterion

         public bool Contains(ITestMethod testMethod) {
            HasBeenCalled = true;
            return true;
         }

         public bool Contains(ITestFixture testFixture) {
            HasBeenCalled = true;
            return true;
         }

         #endregion
      }

      [Test]
      public void UsesCriteria() {
         var uri = new Uri(GetType().Assembly.CodeBase);
         const string targetPath = "..\\..\\TestDll\\bin\\Debug\\csUnit.Core.Tests.dll";
         if( File.Exists(targetPath) ) {
            File.Delete(targetPath);
         }
         File.Copy(uri.AbsolutePath, targetPath);
         var recipe = RecipeFactory.NewRecipe(string.Empty) as Recipe;
         var criterion = new MyCriterion();
         if (recipe != null) {
            recipe.AddAssembly(TestDll);
            recipe.RunTests(new TestRun(criterion));
            recipe.Join();
         }
         Assert.True(criterion.HasBeenCalled);
      }

      private class CategoryFilterMock : MarshalByRefObject, ISelector {
         #region ISelector Members

         /// <summary>
         /// Fired when the selector has changed it's contents, that is when the
         /// selection has changed.
         /// </summary>
         public event EventHandler Modified;

         public bool Includes(ITestMethodInfo testMethod) {
            if(testMethod.Categories.Contains("Red")) {
               return true;
            }
            return false;
         }

         public bool Includes(ITestFixture testFixture) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlElement Serialize() {
            return null;
         }

         public void Deserialize(string content) {
         }

         #endregion
      }

      [Test]
      public void TestCountConsidersFiltersOnMethods() {
         var filter = new CategoryFilterMock();
         var recipe = RecipeFactory.NewRecipe(string.Empty) as Recipe;
         var mockAssembly = new MockAssembly();
// ReSharper disable PossibleNullReferenceException
         recipe.AddAssembly(mockAssembly);
// ReSharper restore PossibleNullReferenceException
         Assert.Equals(2, recipe.CountTests());
         recipe.RegisterSelector(filter);
         Assert.Equals(1, recipe.CountTests());
      }

      private class SimpleRecipeListener {
         public SimpleRecipeListener(IRecipe recipe) {
            foreach(var assembly in recipe) {
               assembly.TestPassed += TestListenerTestPassed;
               assembly.TestFailed += SimpleRecipeListenerTestFailed;
               assembly.TestError += SimpleRecipeListenerTestError;
               assembly.TestSkipped += AssemblyTestSkipped;
            }
         }

         public int TestCount { get; private set; }
         public int PassedCount { get; private set; }
         public int FailedCount { get; private set; }
         public int ErrorCount { get; private set; }
         public int SkipCount { get; private set; }

         private void AssemblyTestSkipped(object sender, TestResultEventArgs args) {
            SkipCount++;
         }

         private void SimpleRecipeListenerTestError(object sender, TestResultEventArgs args) {
            TestCount++;
            ErrorCount++;
      }

         private void SimpleRecipeListenerTestFailed(object sender, TestResultEventArgs args) {
            TestCount++;
            FailedCount++;
         }

         private void TestListenerTestPassed(object sender, TestResultEventArgs args) {
            TestCount++;
            PassedCount++;
         }
      }

      [Test]
      public void CriterionAppliedWithMultipleAssemblies() {
         var recipe = RecipeFactory.NewRecipe("twoassemblies.deleteme.recipe");
         recipe.AddAssembly(TestDll);
         recipe.AddAssembly("..\\..\\csUnit.CompatibilityTests\\NUnit-2.4.7\\bin\\Debug\\NUnit-2.4.7.dll");
         var listener = new SimpleRecipeListener(recipe);

         recipe.RunTests(new TestRun(new AllTestsCriterion()));
         recipe.Join();
         Assert.Equals(7, listener.TestCount);
         Assert.Equals(5, listener.PassedCount);

         listener = new SimpleRecipeListener(recipe);
         var setCriterion = new MultipleTestsCriterion();
         setCriterion.Add("TestDll", "TestDll.ClassWithTests", "AFailingTest");
         setCriterion.Add("TestDll", "TestDll.ClassWithTests", "ATestWithError");
         recipe.RunTests(new TestRun(setCriterion));

         recipe.Join();
         Assert.Equals(2, listener.TestCount);
      }

      [Test]
      public void CriterionIsRemovedForSecondTestRun() {
         var recipe = RecipeFactory.NewRecipe(string.Empty);
         recipe.AddAssembly(TestDll);

         var criterion = new NameCriterion("TestDll.ClassWithTests.ASucceedingTest");
         var listener = new SimpleRecipeListener(recipe);
         recipe.RunTests(new TestRun(criterion));
         recipe.Join();
         Assert.Equals(1, listener.TestCount);

         listener = new SimpleRecipeListener(recipe);
         recipe.RunTests(new TestRun(new AllTestsCriterion()));
         recipe.Join();
         Assert.Equals(3, listener.TestCount);
      }

      [Test]
      public void CriterionIsAppliedForSecondTestRun() {
         var recipe = RecipeFactory.NewRecipe(string.Empty);
         recipe.AddAssembly(TestDll);

         var listener = new SimpleRecipeListener(recipe);
         recipe.RunTests(new TestRun(new AllTestsCriterion()));
         recipe.Join();
         Assert.Equals(3, listener.TestCount);

         var criterion = new NameCriterion("TestDll.ClassWithTests.ASucceedingTest");
         listener = new SimpleRecipeListener(recipe);
         recipe.RunTests(new TestRun(criterion));
         recipe.Join();
         Assert.Equals(1, listener.TestCount);
      }

      [Test]
      public void EmptyRecipeTestRunDoesntFail() {
         var recipe = RecipeFactory.NewRecipe(string.Empty);
         recipe.RunTests(new TestRun(new AllTestsCriterion()));
         Assert.False(recipe.TestsRunning);
      }

      private string _absoluteRecipeFileName = string.Empty;
      private string _path = string.Empty;
      private string _csUnitTestExe = string.Empty;
      private const string TestDll = "..\\..\\TestDll\\bin\\Debug\\testDll.dll";
   }

   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}
