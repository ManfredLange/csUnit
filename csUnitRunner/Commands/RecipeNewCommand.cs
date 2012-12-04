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
using System.Drawing;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// Implements the "Recipe - New" command.
   /// </summary>
   public class RecipeNewCommand : Command {

      private const int _menuPosition    = 1;
      private const int _toolBarPosition = 0;

      public RecipeNewCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "&New Recipe", _menuPosition) {
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.Control | Keys.N;
         }
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.NewRecipe.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return _toolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Create a new recipe";
         }
      }
      #endregion

      protected override void Execute(object sender, EventArgs args) {
         if( CommandTarget.AskSaveModifiedRecipe() == false ) {
            return;
         }
         RecipeFactory.NewRecipe(string.Empty);
      }
   }
}
