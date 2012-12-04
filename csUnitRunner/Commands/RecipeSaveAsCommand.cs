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
using System.Windows.Forms;

using csUnit.Core;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// RecipeSaveAsCommand handler class.
   /// </summary>
   public class RecipeSaveAsCommand : Command {
      private const int DesiredMenuPosition    = 4;
      private const int DesiredContextMenuPosition = 9;
      
      /// <summary>
      /// Instantiates the command.
      /// </summary>
      /// <param name="commandTarget">The form to act upon.</param>
      /// <param name="csUnitCtrl">The control on which to operate</param>
      public RecipeSaveAsCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "Save Recipe &As...", DesiredMenuPosition) {
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
         if( info.IsRecipeItem ) {
            contextCommands.Add(this);
         }
      }

      protected override int ContextMenuPosition {
         get {
            return DesiredContextMenuPosition;
         }
      }

      /// <summary>
      /// Executes the command.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Event parameters.</param>
      protected override void Execute(object sender, EventArgs args) {
         var dlg = new SaveFileDialog {  Filter = Constants.RECIPE_FILE_FILTER,
                                         Title = "Save Recipe As",
                                         AddExtension = true
                                      };

         if(DialogResult.OK == dlg.ShowDialog(FindMainWindow())) {
            RecipeFactory.Current.Save(dlg.FileName);
         }
      }

      /// <summary>
      /// Called when the user interface elements for the command need to be
      /// updated.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Event parameters.</param>
      protected override void OnUiUpdate(object sender, EventArgs args) {
         Enabled = RecipeFactory.Current.AssemblyCount > 0;
      }
   }
}
