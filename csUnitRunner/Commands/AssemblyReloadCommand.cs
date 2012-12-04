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
using csUnit.Interfaces;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// Summary description for AssemblyUnloadCommand.
   /// </summary>
// ReSharper disable UnusedMember.Global
   public class AssemblyReloadCommand : Command {
// ReSharper restore UnusedMember.Global

      private const int MenuPosition    = 3;
      private const int ContextPosition = 7;
      private const int DesiredToolBarPosition = 3;

      public AssemblyReloadCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Assembly", "R&eload", MenuPosition) {
         Enabled = false;
         CsUnitControl.SelectedItems.CollectionChanged += OnSelectedItemsChanged;
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.F4;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return ContextPosition;
         }
      }

      protected override void RegisterForContextMenu(
         List<Command> contextCommands, UiElementInfo info) {
         if( info.IsAssemblyItem ) {
            contextCommands.Add(this);
         }
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.RefreshAssembly.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return DesiredToolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Reload the selected assembly";
         }
      }
      #endregion // ToolBarButton Properties

      protected override void Execute(object sender, EventArgs args) {
         var assembliesToBeRefreshed = new List<ITestAssembly>();
         foreach(var info in CsUnitControl.SelectedItems) {
            if(info.IsAssemblyItem) {
               assembliesToBeRefreshed.Add(RecipeFactory.Current[info.AssemblyPathName]);
            }
         }
         foreach(var ta in assembliesToBeRefreshed) {
            ta.Refresh();
         }
      }

      private void OnSelectedItemsChanged(object sender, EventArgs e) {
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
