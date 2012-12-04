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

using System.Drawing;
using System.Windows.Forms;
using csUnit.Ui.Controls;

namespace csUnit.Runner.Forms {
   public partial class TestForm : Form {
      public TestForm() {
         InitializeComponent();
         _smartTree.ImageList = _imageList;

         _smartTree.Nodes.Clear();
         SmartTreeNode node0 = new SmartTreeNode("Root0:\nRoot0 Root0 ");
         SmartTreeNode node00 = new SmartTreeNode("Node00:\nNode00 Node00 Node00 Node00");
         node0.ImageIndex = 0;
         node0.HasCheckBox = true;
         node0.Nodes.Add(node00);
         SmartTreeNode node00a = new SmartTreeNode("Node00a:\nNode00a Node00a Node00a Node00a Node00a Node00a Node00a");
         node00a.HasCheckBox = true;
         node00a.Checked = true;
         node00.Nodes.Add(node00a);
         node0.Nodes.Add(new SmartTreeNode("Node01:\nNode01 Node01 Node01 Node01 Node01"));
         _smartTree.Nodes.Add(node0);
         SmartTreeNode node1 = new SmartTreeNode("Root1:\nRoot1 Root1 Root1");
         _smartTree.Nodes.Add(node1);
      }

      private void OnFontSizeTrackBarScroll(object sender, System.EventArgs e) {
         Font newFont = new Font(_smartTree.Font.FontFamily, _fontSizeTrackBar.Value);
         _smartTree.Font = newFont;
         _fontSizeTrackBar.Text = _fontSizeTrackBar.Value.ToString();
      }

      private void OnItemWidthTrackBarScroll(object sender, System.EventArgs e) {
         _smartTree.DefaultItemWidth = _itemWidthTrackBar.Value;
      }

      private void OnResetButtonClick(object sender, System.EventArgs e) {
         _smartTree.SuspendLayout();
         float defaultFontSize = 9.25f;
         Font newFont = new Font(_smartTree.Font.FontFamily, defaultFontSize);
         _smartTree.Font = newFont;
         _fontSizeTrackBar.Value = (int) defaultFontSize;
         
         _smartTree.DefaultItemWidth = 200;
         _itemWidthTrackBar.Value = 200;
         _smartTree.ResumeLayout(true);
      }
   }
}
