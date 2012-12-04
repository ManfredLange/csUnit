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
using csUnit.Common;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.CommandLine.Tests {
   // ReSharper disable UnusedMember.Global
   [TestFixture]
   public class NonOptionRelatedTests : CsUnitCmdTests {
      [TearDown]
      public void TearDown() {
         RecipeFactory.Type = RecipeFactory.Default;
      }

      private class RecipeMockThrowingException : IRecipe {
         public RecipeMockThrowingException() {
            throw new Exception("Thrown from within RecipeMockThrowingException.");
         }

         #region IRecipe Members
#pragma warning disable 67
         public event RecipeEventHandler Aborted;

         public event RecipeEventHandler Started;

         public event RecipeEventHandler Finished;

         public event RecipeEventHandler Saved;

         public event RecipeEventHandler SelectorModified;

         public event AssemblyEventHandler AssemblyAdded;

         public event AssemblyEventHandler AssemblyRemoving;

         public event AssemblyEventHandler AssemblyRemoved;
#pragma warning restore 67
         public ITestAssembly this[string assemblyFullOrPathFileName] {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public ITestAssembly[] Assemblies {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public Categories Categories {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public Set<ISelector> Selectors {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public void SetConsoleOutputTo(TextWriter value) {
            throw new Exception("The method or operation is not implemented.");
         }

         public int AssemblyCount {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public string DisplayName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public bool IsNew {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public bool Modified {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public string PathName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
            set {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public bool TestsRunning {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public void Join() { }

         public void Abort() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void AddAssembly(string assemblyPathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Clear() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Close() {
            throw new Exception("The method or operation is not implemented.");
         }

         public int CountTests() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RegisterSelector(ISelector filter) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RemoveAssembly(string assemblyFullName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RunTests(ITestRun selection) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Save() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Save(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public List<FileNotFoundException> LoadFromXml(IXmlDocument doc) {
            throw new Exception("The method or operation is not implemented.");
         }

         public IEnumerator<ITestAssembly> GetEnumerator() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion

         #region IDisposable Members

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [Test]
      public void ExceptionThrownDoesntReturnSuccess() {
         RecipeFactory.Type = typeof(RecipeMockThrowingException);
         var result = CsUnitApp.Main(new[] { @"/recipe:..\..\coreassemblies.recipe" });
         Assert.Equals(1, result);
         Assert.Contains("Internal error: ", Output);
         Assert.Contains("Thrown from within RecipeMockThrowingException.", Output);
      }


      private class AbortingRecipe : IRecipe {
         #region Implementation of IDisposable

         public void Dispose() {
         }

         #endregion

         #region Implementation of IRecipe

         public event RecipeEventHandler Aborted;
         public event RecipeEventHandler Started;
         public event RecipeEventHandler Finished;
         public event RecipeEventHandler Saved;
         public event RecipeEventHandler SelectorModified;
         public event AssemblyEventHandler AssemblyAdded;
         public event AssemblyEventHandler AssemblyRemoving;
         public event AssemblyEventHandler AssemblyRemoved;
         public ITestAssembly this[string assemblyFullOrPathFileName] {
            get { return null; }
         }

// ReSharper disable UnusedAutoPropertyAccessor.Local
         public ITestAssembly[] Assemblies { get; private set; }
         public Categories Categories { get; private set; }
         public Set<ISelector> Selectors { get; private set; }
         public void SetConsoleOutputTo(TextWriter value) {}
         public int AssemblyCount { get; private set; }
         public string DisplayName { get; private set; }
         public bool IsNew { get; private set; }
         public bool Modified { get; private set; }
         public string PathName { get; set; }
         public bool TestsRunning { get; private set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
         public void Abort() {
         }

         public void AddAssembly(string assemblyPathName) {
         }

         public void Clear() {
         }

         public void Close() {
         }

         public int CountTests() {
            return 0;
         }

         public void Join() { }

         public void RegisterSelector(ISelector selector) {
         }

         public void RemoveAssembly(string assemblyFullName) {
         }

         public void RunTests(ITestRun testSelection) {
            RaiseAbortedEvent();
         }

         private void RaiseAbortedEvent() {
            if (Aborted != null) {
               Aborted(this, new RecipeEventArgs());
            }
         }

         public void Save() {
         }

         public void Save(string pathName) {
         }

         public List<FileNotFoundException> LoadFromXml(IXmlDocument doc) {
            return new List<FileNotFoundException>();
         }

         public IEnumerator<ITestAssembly> GetEnumerator() {
            return null;
         }

         #endregion
      }

      [Test]
      public void ReturnsTwoWhenTestsAbort() {
         var app = new CsUnitApp();
         var result = app.ExecuteValidCommandLine(new CmdLineHandler(), new AbortingRecipe());
         Assert.Equals(2, result);
      }
   }
   // ReSharper restore UnusedMember.Global
}
