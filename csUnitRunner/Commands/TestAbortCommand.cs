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
   using csUnit.Ui.Controls;

   /// <summary>
   /// Summary description for TestRunAllCommand.
   /// </summary>
   public class TestAbortCommand : Command {

      private const int _menuPosition    = 4;
      private const int _contextPosition = 4;
      private const int _toolBarPosition = 10;

      public TestAbortCommand(ICommandTarget commandTarget)
         : base(commandTarget, "&Test", "&Abort", _menuPosition, true) {
         _onRecipeStarted = new RecipeEventHandler(OnRecipeStarted);
         _onRecipeFinishedOrAborted = new RecipeEventHandler(OnRecipeFinishedOrAborted);

         Recipe.Loaded += new RecipeEventHandler(this.OnRecipeLoaded);
         Recipe.Closing += new RecipeEventHandler(OnRecipeClosing);
         HookupRecipe();
         Enabled = false;
      }

      protected override Shortcut Shortcut {
         get {
            return Shortcut.CtrlA;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return _contextPosition;
         }
      }

      protected override void RegisterForContextMenu(
         List<Command> contextCommands, UiElementInfo info) {
         contextCommands.Add(this);
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(this.GetType(), "Images.TestAbortCommand.gif");
         }
      }

      protected override int ToolBarPosition {
         get {
            return _toolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Abort the current test run";
         }
      }
      #endregion

      public override void Execute(object sender, EventArgs args) {
         Recipe.Current.Abort();
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
      }

      void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
      }

      private void HookupRecipe() {
         Recipe.Current.Started += _onRecipeStarted;
         Recipe.Current.Finished += _onRecipeFinishedOrAborted;
         Recipe.Current.Aborted += _onRecipeFinishedOrAborted;
      }

      private void UnhookRecipe() {
         Recipe.Current.Started -= _onRecipeStarted;
         Recipe.Current.Finished -= _onRecipeFinishedOrAborted;
         Recipe.Current.Aborted -= _onRecipeFinishedOrAborted;
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         Enabled = true;
      }

      private void OnRecipeFinishedOrAborted(object sender, RecipeEventArgs args) {
         Enabled = false;
      }

      private RecipeEventHandler _onRecipeStarted;
      private RecipeEventHandler _onRecipeFinishedOrAborted;
   }
}

