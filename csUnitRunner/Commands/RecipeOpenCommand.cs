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
   /// Summary description for RecipeOpenCommand.
   /// </summary>
   public class RecipeOpenCommand : Command {

      private const int RequestedMenuPosition = 2;
      private const int RequestedToolBarPosition = 1;

      public RecipeOpenCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "&Open Recipe...", RequestedMenuPosition) {
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.Control | Keys.O;
         }
      }

      protected override int ToolBarPosition {
         get {
            return RequestedToolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Open Recipe";
         }
      }

      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.OpenRecipe.png");
         }
      }

      protected override void Execute(object sender, EventArgs args) {
         if( CommandTarget.AskSaveModifiedRecipe() ) {
            var dlg = new OpenFileDialog {  Filter = Constants.RECIPE_FILE_FILTER,
                                            Title = "Open Recipe"
                                         };

            if( DialogResult.OK == dlg.ShowDialog(FindMainWindow()) ) {
               CommandTarget.Status = "Loading " + dlg.FileName + "...";
               CommandTarget.Cursor = Cursors.WaitCursor;
               RecipeFactory.Load(dlg.FileName);
               CommandTarget.Cursor = Cursors.Default;
               CommandTarget.Status = "Ready";
            }
         }
      }
   }
}
