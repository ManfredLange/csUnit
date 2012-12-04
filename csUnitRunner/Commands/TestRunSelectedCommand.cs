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

using System.Drawing;

namespace csUnit.Runner.Commands {
   using System;
   using System.Collections.Generic;
   using System.Windows.Forms;
   using csUnit.Core;
   using csUnit.Interfaces;
   using csUnit.Ui.Controls;

   /// <summary>
   /// Summary description for TestRunCheckedCommand.
   /// </summary>
   public class TestRunSelectedCommand : Command {

      private const int _menuPosition    = 2;
      private const int _contextPosition = 2;
      private const int _toolBarPosition = 8;

      public TestRunSelectedCommand(ICommandTarget commandTarget)
         : base(commandTarget, "&Test", "Run Selected", _menuPosition) {
         Enabled = false;
         CommandTarget.SelectedItems.CollectionChanged += new EventHandler(SelectedItems_CollectionChanged);
      }

      protected override Shortcut Shortcut {
         get {
            return Shortcut.F7;
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
            return new Bitmap(this.GetType(), "Images.TestRunSelectedCommand.gif");
         }
      }

      protected override int ToolBarPosition {
         get {
            return _toolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Run Selected Tests";
         }
      }
      #endregion

      public override void Execute(object sender, EventArgs args) {
         if(Recipe.Current != null) {
            TestSpec ts = new TestSpec();
            foreach(object item in CommandTarget.SelectedItems) {
               UiElementInfo uiElemInfo = item as UiElementInfo;
               if( uiElemInfo != null ) {
                  ts.AddTest(uiElemInfo.TestSpecName);
               }
            }
            if( ts.FixtureCount > 0 ) {
               Recipe.Current.RunTests(ts);
            }
         }
      }

      private void SelectedItems_CollectionChanged(object sender, EventArgs e) {
         UpdateEnabledStatus();
      }

      private void UpdateEnabledStatus() {
         if(Recipe.Current.TestsRunning) {
            Enabled = false;
         }
         else {
            if(CommandTarget.SelectedItems.Count > 0) {
               Enabled = true;
            }
            else {
               Enabled = false;
            }
         }
      }
   }
}
