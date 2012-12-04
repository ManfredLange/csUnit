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
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.Commands {
   /// <summary>
   /// Runs the tests using the failed TestSpec from the MainForm.  If the failed TestSpec
   /// is undefined, then a default test spec is used and this command will have the
   /// same affect as RunAll.
   /// </summary>
   public class TestRunFailedCommand : Command {

      private const int _menuPosition    = 3;
      private const int _contextPosition = 3;

      public TestRunFailedCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Test", "Run &Failed", _menuPosition, true) {
         Enabled = ! _failedTestCriterion.IsEmpty;
         _onRecipeLoaded = OnRecipeLoaded;
         _onRecipeClosing = OnRecipeClosing;
         _onTestErrorOrFail = OnTestErrorOrFail;
         _onAssemblyAdded = OnAssemblyAdded;
         _onAssemblyRemoving = OnAssemblyRemoving;

         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
         HookupRecipe();
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.F7;
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

      protected override void Execute(object sender, EventArgs args) {
         if(! _failedTestCriterion.IsEmpty ) {
            var testRun = new TestRun(_failedTestCriterion);
            RecipeFactory.Current.RunTests(testRun);
         }
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         _failedTestCriterion = new MultipleTestsCriterion();
         HookupRecipe();
      }

      private void HookupRecipe() {
         if(RecipeFactory.Current != null) {
            RecipeFactory.Current.AssemblyAdded += _onAssemblyAdded;
            RecipeFactory.Current.AssemblyRemoving += _onAssemblyRemoving;
            foreach(var ass in RecipeFactory.Current.Assemblies) {
               HookupAssembly(ass);
            }
         }
      }

      private void OnAssemblyAdded(object sender, AssemblyEventArgs args) {
         HookupAssembly(RecipeFactory.Current[args.AssemblyFullName]);
      }

      private void HookupAssembly(ITestAssembly assembly) {
         assembly.TestError += _onTestErrorOrFail;
         assembly.TestFailed += _onTestErrorOrFail;
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         foreach(var assembly in RecipeFactory.Current.Assemblies) {
            assembly.TestError -= _onTestErrorOrFail;
            assembly.TestFailed -= _onTestErrorOrFail;
         }
      }

      private void OnAssemblyRemoving(object sender, AssemblyEventArgs args) {
         var ta = RecipeFactory.Current[args.AssemblyFullName];
         ta.TestError -= _onTestErrorOrFail;
         ta.TestFailed -= _onTestErrorOrFail;
      }

      private void OnTestErrorOrFail(object sender, TestResultEventArgs args) {
         _failedTestCriterion.Add(args.AssemblyName, args.ClassName, args.MethodName);
         Enabled = true;
      }

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeClosing;
      private readonly AssemblyEventHandler _onAssemblyRemoving;
      private readonly AssemblyEventHandler _onAssemblyAdded;

      private readonly TestEventHandler _onTestErrorOrFail;

      private MultipleTestsCriterion _failedTestCriterion = new MultipleTestsCriterion();
   }
}
