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
using System.Drawing;
using System.Windows.Forms;

using csUnit.Core;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// Implements the "Assembly - Remove" command.
   /// </summary>
// ReSharper disable UnusedMember.Global
   public class AssemblyRemoveCommand : Command {
// ReSharper restore UnusedMember.Global

      private const int MenuPosition    = 4;
      private const int ContextPosition = 6;
      private const int DesiredToolBarPosition = 4;

      public AssemblyRemoveCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Assembly", "&Remove", MenuPosition, true) {
         Enabled = false;
         CsUnitControl.SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
      }

      protected override int ContextMenuPosition {
         get {
            return ContextPosition;
         }
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.Delete;
         }
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands, UiElementInfo info) {
         if( info.IsAssemblyItem ) {
            contextCommands.Add(this);
         }
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.DeleteAssembly.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return DesiredToolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Remove the selected assembly";
         }
      }
      #endregion // ToolBarButton Properties

      protected override void Execute(object sender, EventArgs args) {
         var toBeRemoved = new List<UiElementInfo>();
         foreach( var info in CsUnitControl.SelectedItems ) {
            if(info.IsAssemblyItem) {
               toBeRemoved.Add(info);
            }
         }

         foreach(var info in toBeRemoved) {
            RecipeFactory.Current.RemoveAssembly(info.AssemblyPathName);
         }
      }

      void SelectedItemsCollectionChanged(object sender, EventArgs e) {
         UpdateEnabledStatus();
      }

      private void UpdateEnabledStatus() {
         if(RecipeFactory.Current.TestsRunning) {
            Enabled = false;
         }
         else {
            var enabled = false;
            foreach( var item in CsUnitControl.SelectedItems ) {
               if(item.IsAssemblyItem) {
                  enabled = true;
                  break;
               }
            }
            Enabled = enabled;
         }
      }
   }
}
