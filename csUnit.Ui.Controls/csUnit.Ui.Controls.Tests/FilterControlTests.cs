////////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2007 by Manfred Lange, Markus Renschler, Jake Anderson, 
//                          and Piers Lawson. All rights reserved.
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
//    product, the following acknowledgement must be included in the product 
//    documentation:
// 
//       Portions Copyright © 2002-2007 by Manfred Lange, Markus Renschler, 
//       Jake Anderson, and Piers Lawson. All rights reserved.
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using csUnit;
using csUnit.Core;
using csUnit.Interfaces;
using csUnit.Ui.Controls.TabPages;

namespace csUnit.Ui.Controls.Tests {
   [TestFixture]
   public class FilterControlTests {
      private class ASpecialRecipe : IRecipe {
         public Categories Categories {
            get {
               Categories cats = new Categories();
               cats.Add("Red");
               cats.Add("Green");
               cats.Add("Blue");
               return cats;
            }
         }
         
         #region IRecipe Members

         public event RecipeEventHandler Aborted;

         public event RecipeEventHandler Started;

         public event RecipeEventHandler Finished;

         public event RecipeEventHandler Saved;

         public event RecipeEventHandler Loaded;

         /// <summary>
         /// Raised when the settings on a selector have been modified.
         /// </summary>
         public event RecipeEventHandler SelectorModified;

         public event AssemblyEventHandler AssemblyAdded;

         public event AssemblyEventHandler AssemblyRemoving;

         public event AssemblyEventHandler AssemblyRemoved;

         public ITestAssembly this[string assemblyFullOrPathFileName] {
            get {
               return null;
            }
         }

         public ITestAssembly[] Assemblies {
            get {
               return new ITestAssembly[0];
            }
         }

         public System.IO.TextWriter ConsoleOutput {
            get {
               return null;
            }
            set {
            }
         }

         public int AssemblyCount {
            get {
               return 5;
            }
         }

         public string DisplayName {
            get {
               return "ASpecialRecipe";
            }
         }

         public bool IsNew {
            get {
               return false;
            }
         }

         public bool Modified {
            get {
               return false;
            }
         }

         public string PathName {
            get {
               return "";
            }
            set {
            }
         }

         public TestRunKind TestRunKind {
            get {
               return TestRunKind.None;
            }
         }

         public bool TestsRunning {
            get {
               return false;
            }
         }

         public void Abort() {
         }

         public void AddAssembly(string assemblyPathName) {
         }

         public void Clear() {
         }

         public void Close() {
         }

         public int CountTests() {
            return 55;
         }

         public List<FileNotFoundException> LoadFromXml(IXmlDocument document) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RegisterSelector(ISelector filter) {
            _filters.Add(filter);
         }

         public void RemoveAssembly(string assemblyFullName) {
         }

         public void RunTests(ITestSpec spec) {
            if(Started != null) {
               Started(this, new RecipeEventArgs());
            }
         }

         public void Save() {
         }

         public void Save(string pathName) {
         }

         public IEnumerator<ITestAssembly> GetEnumerator() {
            return null;
         }

         public Set<ISelector> Selectors {
            get {
               return _filters;
            }
         }

         #endregion

         #region IDisposable Members

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion

         private Set<ISelector> _filters = new Set<ISelector>();

         #region IRecipe Members


         public void SetConsoleOutputTo(System.IO.TextWriter textWriter) {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [FixtureSetUp]
      public void Setup() {
         _fc = new CategoryControl();
         _fc.CreateControl();
      }

      [Test]
      public void SelectionSurvivesRefresh() {
         RecipeFactory.Type = typeof(ASpecialRecipe);
         IRecipe recipe = RecipeFactory.NewRecipe("");
         _fc.ResetAllCategories();
         _fc.IncludeCategory("Blue");
         _fc.RefreshCategoriesList();
         Categories expected = new Categories();
         expected.Add("Blue");
         Assert.Equals(expected, _fc.IncludedCategories);
      }

      [Test]
      public void FilterControlRegistersFilter() {
         RecipeFactory.Type = typeof(ASpecialRecipe);
         _fc = new CategoryControl();
         IRecipe recipe = RecipeFactory.NewRecipe("");
         Assert.Greater(recipe.Selectors.Count, 0);
      }

      private class ASpecialTestMethod : ITestMethodInfo {
         public ASpecialTestMethod(string category) {
            _category = category;
         }

         #region ITestMethodInfo Members

         public string FullName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public string Name {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public Categories Categories {
            get {
               Categories cats = new Categories();
               cats.Add(_category);
               return cats;
            }
         }

         public Categories InheritedCategories {
            get {
               return Categories.Empty;
            }
         }

         #endregion

         private string _category = string.Empty;
      }

      [Test]
      public void RegisteredFilterWorksForSelectedCategory() {
         RecipeFactory.Type = typeof(ASpecialRecipe);
         IRecipe recipe = RecipeFactory.NewRecipe("");
         _fc.ResetAllCategories();
         _fc.IncludeCategory("Blue");
         ISelector registeredFilter = null;
         foreach(ISelector filter in recipe.Selectors) {
            registeredFilter = filter;
            break;
         }
         Assert.True(registeredFilter.Includes(new ASpecialTestMethod("Blue")));
         Assert.False(registeredFilter.Includes(new ASpecialTestMethod("Red")));
         Assert.False(registeredFilter.Includes(new ASpecialTestMethod("Green")));
      }

      [Test]
      public void SelectingNoneIncludesAll() {
         RecipeFactory.Type = typeof(ASpecialRecipe);
         _fc._Name = "SelectingNoneIncludesAll()";
         IRecipe recipe = RecipeFactory.NewRecipe("");
         _fc.ResetAllCategories();
         recipe.RunTests(new AllTestsTestSpec());
         ISelector registeredFilter = null;
         foreach(ISelector filter in recipe.Selectors) {
            if(filter.Equals(_fc.Filter)) {
               registeredFilter = filter;
               break;
            }
         }
         Assert.True(registeredFilter.Includes(new ASpecialTestMethod("Blue")));
         Assert.True(registeredFilter.Includes(new ASpecialTestMethod("Red")));
         Assert.True(registeredFilter.Includes(new ASpecialTestMethod("Green")));
      }

      private CategoryControl _fc = null;
   }
}
