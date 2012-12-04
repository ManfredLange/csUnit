////////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2006 by Manfred Lange, Markus Renschler, Jake Anderson, 
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
//    product, the following acknowledgment must be included in the product 
//    documentation:
// 
//       Portions Copyright © 2002-2006 by Manfred Lange, Markus Renschler, 
//       Jake Anderson, and Piers Lawson. All rights reserved.
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////

namespace csUnit.Runner.Commands {
   using System;
   using System.Collections.Generic;
   using System.Windows.Forms;
   using csUnit.Core;
   using csUnit.Interfaces;
   using csUnit.Ui.Controls;

   /// <summary>
   /// Runs the tests using the failed TestSpec from the MainForm.  If the failed TestSpec
   /// is undefined, then a default test spec is used and this command will have the
   /// same affect as RunAll.
   /// </summary>
   public class TestRunFailedCommand : Command {

      private const int _menuPosition    = 3;
      private const int _contextPosition = 3;

      public TestRunFailedCommand(ICommandTarget commandTarget)
         : base(commandTarget, "&Test", "Run &Failed", _menuPosition, true) {
         Enabled = ! _testSpec.Empty;
         _onRecipeLoaded = new RecipeEventHandler(OnRecipeLoaded);
         _onRecipeClosing = new RecipeEventHandler(OnRecipeClosing);
         _onTestErrorOrFail = new TestEventHandler(OnTestErrorOrFail);

         Recipe.Loaded += _onRecipeLoaded;
         Recipe.Closing += _onRecipeClosing;
      }

      protected override Shortcut Shortcut {
         get {
            return Shortcut.F7;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return _contextPosition;
         }
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
         contextCommands.Add(this);
      }

      public override void Execute(object sender, EventArgs args) {
         if(_testSpec.FixtureCount > 0) {
            _testSpec.FixtureCategory = CommandTarget.SelectedFixtureCategory;
            _testSpec.TestCategory = CommandTarget.SelectedTestCategory;
            Recipe.Current.RunTests(_testSpec);
         }
      }

      protected override void OnUiUpdate(object sender, System.EventArgs args) {
         if( _testSpec.FixtureCount > 0 ) {
            MenuItem.Enabled = true;
         }
         else {
            MenuItem.Enabled = false;
         }
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         _testSpec = new TestSpec();
         Recipe.Current.AssemblyRemoving += _onAssemblyRemoving;
         foreach(TestAssembly ass in Recipe.Current.Assemblies) {
            ass.TestError += _onTestErrorOrFail;
            ass.TestFailed += _onTestErrorOrFail;
         }
         OnUiUpdate(sender, args);
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         foreach(TestAssembly ass in Recipe.Current.Assemblies) {
            ass.TestError -= _onTestErrorOrFail;
            ass.TestFailed -= _onTestErrorOrFail;
         }
      }

      private void OnAssemblyRemoving(object sender, AssemblyEventArgs args) {
         TestAssembly ta = Recipe.Current[args.AssemblyFullName];
         ta.TestError -= _onTestErrorOrFail;
         ta.TestFailed -= _onTestErrorOrFail;
      }

      private void OnTestErrorOrFail(object sender, TestResultEventArgs args) {
         _testSpec.AddTest(args.AssemblyName, args.ClassName, args.MethodName);
         Enabled = true;
         OnUiUpdate(sender, args);
      }

      private RecipeEventHandler _onRecipeLoaded;
      private RecipeEventHandler _onRecipeClosing;
      private AssemblyEventHandler _onAssemblyRemoving;

      private TestEventHandler _onTestErrorOrFail;

      private TestSpec _testSpec = new TestSpec();
   }
}
