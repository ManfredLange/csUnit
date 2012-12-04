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
   using System.Drawing;
   using System.Windows.Forms;

   using csUnit.Core;
   using csUnit.Interfaces;
   using csUnit.Ui.Controls;

   /// <summary>
   /// Summary description for TestRunAllCommand.
   /// </summary>
   public class TestRunAllCommand : Command {

      private const int _menuPosition    = 1;
      private const int _contextPosition = 1;
      private const int _toolBarPosition = 6;

      public TestRunAllCommand(ICommandTarget commandTarget)
         : base(commandTarget, "&Test", "&Run All", _menuPosition) {
         _onRecipeLoaded = new RecipeEventHandler(OnRecipeLoaded);
         _onRecipeClosing = new RecipeEventHandler(OnRecipeClosing);
         _onOtherRecipeEvent = new RecipeEventHandler(OnOtherRecipeEvent);
         _onAssemblyEvent = new AssemblyEventHandler(OnAssemblyEvent);
         
         Recipe.Loaded += _onRecipeLoaded;
         Recipe.Closing += _onRecipeClosing;
         HookupRecipe();
         UpdateEnabledStatus();
      }

      protected override Shortcut Shortcut {
         get {
            return Shortcut.F5;
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

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(this.GetType(), "Images.TestRunAllCommand.gif");
         }
      }

      protected override int ToolBarPosition {
         get {
            return _toolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Run all tests";
         }
      }
      #endregion // ToolBarButton Properties

      public override void Execute(object sender, EventArgs args) {
         if(Recipe.Current != null) {
            if(!Recipe.Current.TestsRunning) {
               Recipe.Current.RunTests(CommandTarget.SelectedTestCategory,
                  CommandTarget.SelectedFixtureCategory);
            }
         }
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         UpdateEnabledStatus();
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
         UpdateEnabledStatus();
      }

      private void HookupRecipe() {
         Recipe.Current.AssemblyAdded += _onAssemblyEvent;
         Recipe.Current.AssemblyRemoved += _onAssemblyEvent;

         Recipe.Current.Started += _onOtherRecipeEvent;
         Recipe.Current.Finished += _onOtherRecipeEvent;
         Recipe.Current.Aborted += _onOtherRecipeEvent;
      }

      private void UnhookRecipe() {
         Recipe.Current.AssemblyAdded -= _onAssemblyEvent;
         Recipe.Current.AssemblyRemoved -= _onAssemblyEvent;

         Recipe.Current.Started -= _onOtherRecipeEvent;
         Recipe.Current.Finished -= _onOtherRecipeEvent;
         Recipe.Current.Aborted -= _onOtherRecipeEvent;
      }

      private void OnOtherRecipeEvent(object sender, RecipeEventArgs args) {
         UpdateEnabledStatus();
      }

      private void OnAssemblyEvent(object sender, AssemblyEventArgs args) {
         UpdateEnabledStatus();
      }

      private void UpdateEnabledStatus() {
         if(Recipe.Current.TestsRunning) {
            Enabled = false;
         }
         else {
            if(Recipe.Current.Count > 0) {
               Enabled = true;
            }
            else {
               Enabled = false;
            }
         }
      }

      private RecipeEventHandler _onRecipeLoaded;
      private RecipeEventHandler _onRecipeClosing;
      private RecipeEventHandler _onOtherRecipeEvent;

      private AssemblyEventHandler _onAssemblyEvent;
   }
}
