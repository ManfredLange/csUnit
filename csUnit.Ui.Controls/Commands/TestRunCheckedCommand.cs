#region Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, and Piers Lawson. All rights reserved.
////////////////////////////////////////////////////////////////////////////////
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
using System.Drawing;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.Commands {
   /// <summary>
   /// Summary description for TestRunCheckedCommand.
   /// </summary>
// ReSharper disable UnusedMember.Global
   public class TestRunCheckedCommand : Command {
// ReSharper restore UnusedMember.Global

      private const int MenuPosition = 3;
      private const int ContextPosition = 2;
      private const int DesiredToolBarPosition = 8;

      public TestRunCheckedCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Test", "Run Checked", MenuPosition) {
         Enabled = false;
         _onRecipeLoaded += OnRecipeLoaded;
         _onRecipeClosing += OnRecipeClosing;
         _onRecipeStartedOrFinished += OnRecipeStartedOrFinished;

         CsUnitControl.CheckedItems.CollectionChanged += OnCheckedItemsChanged;
         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
         HookupRecipe();
      }

      void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
      }

      void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
      }

      private void HookupRecipe() {
         if( RecipeFactory.Current != null ) {
            RecipeFactory.Current.Started += _onRecipeStartedOrFinished;
            RecipeFactory.Current.Finished += _onRecipeStartedOrFinished;
         }
      }

      private void UnhookRecipe() {
         RecipeFactory.Current.Started -= _onRecipeStartedOrFinished;
         RecipeFactory.Current.Finished -= _onRecipeStartedOrFinished;
      }

      private void OnRecipeStartedOrFinished(object sender, RecipeEventArgs args) {
         UpdateEnabledStatus();
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.F8;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return ContextPosition;
         }
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
         if( info.IsAssemblyItem
            || info.IsFixtureItem
            || info.IsMethodItem ) {
            contextCommands.Add(this);
         }
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.TestRunCheckedCommand.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return DesiredToolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Run Checked Tests";
         }
      }
      #endregion

      protected override void Execute(object sender, EventArgs args) {
         if( RecipeFactory.Current != null ) {
            var multipleTestsCriterion = new MultipleTestsCriterion();
            foreach(var item in CsUnitControl.CheckedItems) {
               if( item.MethodFullName != string.Empty ) {
                  multipleTestsCriterion.Add(item.AssemblyName, item.FixtureName, item.MethodName);
               }
               else if( item.FixtureName != string.Empty ) {
                  multipleTestsCriterion.Add(item.AssemblyName, item.FixtureName, item.MethodName);
               }
            }
            RecipeFactory.Current.RunTests(new TestRun(multipleTestsCriterion));
         }
      }

      private void OnCheckedItemsChanged(object sender, EventArgs args) {
         UpdateEnabledStatus();
      }

      private void UpdateEnabledStatus() {
         if( RecipeFactory.Current.TestsRunning ) {
            Enabled = false;
         }
         else {
            Enabled = CsUnitControl.CheckedItems.Count > 0;
         }
      }

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeClosing;
      private readonly RecipeEventHandler _onRecipeStartedOrFinished;
   }
}
