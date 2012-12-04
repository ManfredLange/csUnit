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
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// Summary description for RecipeSaveCommand.
   /// </summary>
   public class RecipeSaveCommand : Command {

      private const int _menuPosition    = 3;
      private const int _contextPosition = 8;
      private const int _toolBarPosition = 2;

      public RecipeSaveCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "&Save Recipe", _menuPosition) {
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.Control | Keys.S;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return _contextPosition;
         }
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
         if( info.IsRecipeItem ) {
            contextCommands.Add(this);
         }
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.SaveRecipe.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return _toolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Save Recipe";
         }
      }

      #endregion // ToolBarButton Properties

      protected override void Execute(object sender, EventArgs args) {
         if(RecipeFactory.Current.IsNew) {
            ExecuteCommand(typeof(RecipeSaveAsCommand), sender, args);
         }
         else {
            RecipeFactory.Current.Save();
         }
      }
   }
}
