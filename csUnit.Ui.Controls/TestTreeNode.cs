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

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace csUnit.Ui.Controls {
   public class TestTreeNode : TreeNode {
      public TestTreeNode() {
      }
      
      public TestTreeNode(string text) 
         : base(text) {
      }

      public void OnDrawNode(object sender, DrawTreeNodeEventArgs e) {
         TreeView treeview = e.Node.TreeView;
         Font font = e.Node.NodeFont;
         if( font == null ) {
            font = treeview.Font;
         }

         int top = e.Bounds.Y;
         int height = e.Bounds.Height;

         int nodeleft = e.Node.Bounds.X;
         int nodetop = e.Node.Bounds.Y;
         int nodeheight = e.Node.Bounds.Height;
         int nodewidth = e.Node.Bounds.Width;

         int linelength = 10;
         int checkboxwidth = 13;
         int checknodespace = 2;

         if( !treeview.CheckBoxes ) {
            checkboxwidth = checknodespace = 0;
         }

         using (SolidBrush brush = new SolidBrush(BackColor)) {

            e.Graphics.FillRectangle(brush, e.Node.Bounds);

         }
         
         using(Pen p = new Pen(Color.Gray)) {
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            int lineleft = nodeleft - checknodespace - checkboxwidth - linelength;
            //draw horizontal dot line
            e.Graphics.DrawLine(p, lineleft, top + height/2, lineleft +
                                                             linelength, top + height/2);
            // draw the up half vertical dot line
            if(e.Node.PrevNode != null || e.Node.Parent != null) {
               e.Graphics.DrawLine(p, lineleft, top, lineleft, top + height/2);
            }
            // draw the down half vertical dot line
            if(e.Node.NextNode != null) {
               e.Graphics.DrawLine(p, lineleft, top + height/2,
                                   lineleft, e.Node.NextNode.Bounds.Top);
            }
            // draw plus/minus image
            if(e.Node.Nodes.Count > 0) {
               if(!e.Node.IsExpanded) {
                  e.Graphics.DrawImage(Properties.Resources.Plus,
                                       lineleft - Properties.Resources.Plus.Width/2, top +
                                                                                     (height -
                                                                                      Properties.Resources.Plus.Height)/
                                                                                     2);
               }
               else {
                  e.Graphics.DrawImage(Properties.Resources.Minus,
                                       lineleft - Properties.Resources.Minus.Width/2, top +
                                                                                      (height -
                                                                                       Properties.Resources.Minus.Height)/
                                                                                      2);
               }
            }
            // draw the vertical dot line for the parent nodes if necessary
            TreeNode parentNode = e.Node.Parent;
            int parentNodeLeftDistance = checknodespace + checkboxwidth +
                                         linelength;
            while(parentNode != null) {
               if(parentNode.NextNode != null) {
                  e.Graphics.DrawLine(p, parentNode.Bounds.X -
                                         parentNodeLeftDistance,
                                      top, parentNode.Bounds.X - parentNodeLeftDistance, top +
                                                                                         height);
               }
               parentNode = parentNode.Parent;
            }
         }
         if (treeview.CheckBoxes) {
            // draw checkbox
            if(e.Node.Checked) {
               CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(nodeleft -
                                                                   checkboxwidth - checknodespace,
                                                                   top + (height - checkboxwidth)/2), CheckBoxState.CheckedNormal);
            }
            else {
               CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(nodeleft -
                                                                   checkboxwidth - checknodespace,
                                                                   top + (height - checkboxwidth) / 2), CheckBoxState.UncheckedNormal);
            }
         }
         // erase the previous node text
         using (Brush b = new SolidBrush(treeview.BackColor)) {
            e.Graphics.DrawString(e.Node.Text, font, b, nodeleft, nodetop);
         }
         // draw node text and highlight rectangle
         if(e.Node.IsSelected) {
            ControlPaint.DrawFocusRectangle(e.Graphics, e.Node.Bounds);

            e.Graphics.FillRectangle(Brushes.LightSkyBlue, new
                                                              Rectangle(nodeleft + 1, nodetop + 1, nodewidth - 2,
                                                                        nodeheight - 2));
            using(Brush b = new SolidBrush(Color.White)) {
               e.Graphics.DrawString(e.Node.Text, font, b, nodeleft, nodetop);
            }
         }
         else {
            using(Brush b = new SolidBrush(treeview.ForeColor)) {
               e.Graphics.DrawString(e.Node.Text, font, b, nodeleft, nodetop);
            }
         }
      }
   }
}
