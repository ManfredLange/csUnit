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

using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.TabPages {
   /// <summary>
   /// Displays a simple list of all tests in the current recipe.
   /// </summary>
   /// <remarks>Please note that although this class is a UserControl, it will
   /// still be displayed within a tab page. The reason for this construct is
   /// that on one hand the control should remain editable in the graphical
   /// designer of VS, but on the other hand the tab control within the
   /// CsUnitControl should remain generic using reflection to populate the
   /// tab pages.</remarks>
   public partial class SimpleTestListControl : UserControl/*, IContentControl*/ {
//      public event TreeViewEventHandler AfterSelect;
//      public event TreeViewEventHandler AfterCheck;
//      public event CsUnitControlEventHandler FillContextMenu;
      
      public SimpleTestListControl() {
         InitializeComponent();
         Name = "Test List";
         _onRecipeLoaded = OnRecipeLoaded;
         _onRecipeClosing = OnRecipeClosing;
         _onAssemblyAddedOrRemoved = OnAssemblyAddedOrRemoved;

         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
      }

      public string ToolTipText {
         get {
            return "Displays a simple list of the tests.";
         }
      }

      public int DesiredTabPosition {
         get {
            return 10;
         }
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         RecipeFactory.Current.AssemblyAdded += _onAssemblyAddedOrRemoved;
         RecipeFactory.Current.AssemblyRemoved += _onAssemblyAddedOrRemoved;
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         RecipeFactory.Current.AssemblyAdded -= _onAssemblyAddedOrRemoved;
         RecipeFactory.Current.AssemblyRemoved -= _onAssemblyAddedOrRemoved;
      }

      void OnAssemblyAddedOrRemoved(object sender, AssemblyEventArgs args) {
         UpdateFromRecipe();
      }

      private void UpdateFromRecipe() {
         _testMethodInfoBindingSource.Clear();
         foreach(ITestAssembly ta in RecipeFactory.Current) {
            foreach(TestFixtureInfo tfi in ta.TestFixtureInfos) {
               foreach(TestMethodInfo tmi in tfi.TestMethods) {
                  _testMethodInfoBindingSource.Add(tmi);
               }
            }
         }
      }

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeClosing;
      private readonly AssemblyEventHandler _onAssemblyAddedOrRemoved;
   }
}
