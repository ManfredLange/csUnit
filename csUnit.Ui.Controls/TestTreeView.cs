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

#if BLUBB

using System.Drawing;
using System.Windows.Forms;

namespace csUnit.Ui.Controls {
   public partial class TestTreeView : TreeView {
      public TestTreeView() {
         //         DrawMode = TreeViewDrawMode.OwnerDrawText;
         DrawMode = TreeViewDrawMode.OwnerDrawAll;
         InitializeComponent();
         DrawNode += OnDrawNode;
         MouseDown += OnMouseDown;
      }

      private void OnMouseDown(object sender, MouseEventArgs e) {
         TreeNode clickedNode = GetNodeAt(e.X, e.Y);
         if ( clickedNode != null 
            /*&& NodeBounds(clickedNode).Contains(e.X, e.Y)*/) {
            SelectedNode = clickedNode;
         }
      }

      private void OnDrawNode(object sender, DrawTreeNodeEventArgs e) {
         TestTreeNode node = e.Node as TestTreeNode;
         if( node != null ) {
            node.OnDrawNode(sender, e);
         }
         else {
            e.DrawDefault = true;
         }
         return;
         //e.DrawDefault = true;
         //return;

         // Draw the background and node text for a selected node.
         if ((e.State & TreeNodeStates.Selected) != 0) {
            // Draw the background of the selected node. The NodeBounds
            // method makes the highlight rectangle large enough to
            // include the text of a node tag, if one is present.
            Rectangle bounds = NodeBounds(e.Node);
            e.Graphics.FillRectangle(Brushes.Green, bounds);

            // Retrieve the node font. If the node font has not been set,
            // use the TreeView font.
            Font nodeFont = e.Node.NodeFont;
            if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

            // Draw the node text.
            //e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, 2, 2);
            e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, bounds.Left, bounds.Top);
                //Rectangle.Inflate(e.Bounds, 2, 0));
         }

        // Use the default background and node text.
         else {
            e.DrawDefault = true;
         }

         // If a node tag is present, draw its string representation 
         // to the right of the label text.
         if (e.Node.Tag != null) {
            e.Graphics.DrawString(e.Node.Tag.ToString(), _tagFont,
                Brushes.Red, e.Bounds.Right + 2, e.Bounds.Top);
         }

         // If the node has focus, draw the focus rectangle large, making
         // it large enough to include the text of the node tag, if present.
         if ((e.State & TreeNodeStates.Focused) != 0) {
            using (Pen focusPen = new Pen(Color.Black)) {
               focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
               Rectangle focusBounds = NodeBounds(e.Node);
               focusBounds.Size = new Size(focusBounds.Width - 1,
               focusBounds.Height - 1);
               e.Graphics.DrawRectangle(focusPen, focusBounds);
            }
         }

      }
      
      // Returns the bounds of the specified node, including the region 
      // occupied by the node label and any node tag displayed.
      private Rectangle NodeBounds(TreeNode node) {
         // Set the return value to the normal node bounds.
         Rectangle bounds = node.Bounds;
         if (node.Tag != null) {
            // Retrieve a Graphics object from the TreeView handle
            // and use it to calculate the display width of the tag.
            Graphics g = CreateGraphics();
            int tagWidth = (int)g.MeasureString
                (node.Tag.ToString(), _tagFont).Width + 6;

            // Adjust the node bounds using the calculated value.
            bounds.Offset(tagWidth / 2, 0);
            bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
            g.Dispose();
         }

         return bounds;

      }

      Font _tagFont = new Font("Helvetica", 8, FontStyle.Bold);
   }
}

#endif