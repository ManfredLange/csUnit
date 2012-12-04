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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;
using csUnit.Ui.Controls;

namespace csUnit.Ui.Controls.Commands {
   /// <summary>
   /// Summary description for TestRunAllCommand.
   /// </summary>
   public class TestAbortCommand : Command {

      private const int _menuPosition    = 4;
      private const int _contextPosition = 4;
      private const int _toolBarPosition = 10;

      public TestAbortCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Test", "&Abort", _menuPosition, true) {
         _onRecipeStarted = OnRecipeStarted;
         _onRecipeFinishedOrAborted = OnRecipeFinishedOrAborted;

         RecipeFactory.Loaded += this.OnRecipeLoaded;
         RecipeFactory.Closing += OnRecipeClosing;
         HookupRecipe();
         Enabled = false;
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.Control | Keys.A;
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
            return new Bitmap(GetType(), "Images.TestAbortCommand.png");
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

      protected override void Execute(object sender, EventArgs args) {
         RecipeFactory.Current.Abort();
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
      }

      void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
      }

      private void HookupRecipe() {
         if(RecipeFactory.Current != null) {
            RecipeFactory.Current.Started += _onRecipeStarted;
            RecipeFactory.Current.Finished += _onRecipeFinishedOrAborted;
            RecipeFactory.Current.Aborted += _onRecipeFinishedOrAborted;
         }
      }

      private void UnhookRecipe() {
         RecipeFactory.Current.Started -= _onRecipeStarted;
         RecipeFactory.Current.Finished -= _onRecipeFinishedOrAborted;
         RecipeFactory.Current.Aborted -= _onRecipeFinishedOrAborted;
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         Enabled = true;
      }

      private void OnRecipeFinishedOrAborted(object sender, RecipeEventArgs args) {
         Enabled = false;
      }

      private readonly RecipeEventHandler _onRecipeStarted;
      private readonly RecipeEventHandler _onRecipeFinishedOrAborted;
   }
}

