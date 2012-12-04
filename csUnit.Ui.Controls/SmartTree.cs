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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace csUnit.Ui.Controls {
   public partial class SmartTree : UserControl {
      public bool ShowNodeToolTips {
         get {
            return _showNodeToolTips;
         }
         set {
            _showNodeToolTips = value;
         }
      }
      
      public SmartTree() {
         InitializeComponent();

         _contentPanel.SetOwnerTree(this);

         _rootNodes = new SmartTreeNodeCollection(null, this);
         _selectedNodes = new List<SmartTreeNode>();
         // TODO: need to sign up for change event on this collection, e.g.
         // when a node is added/removed. Then those nodes need to be 
         // invalidated. [17apr08, ml]
      }

      public event SmartTreeEventHandler AfterSelect;

      public void BeginUpdate() {
      }

      public void EndUpdate() {
      }

      /// <summary>
      /// Gets a collection of the root nodes of this tree.
      /// </summary>
      public SmartTreeNodeCollection Nodes {
         get {
            return _rootNodes;
         }
      }

      /// <summary>
      /// Gets or sets the default with of the nodes. Value is in pixel.
      /// </summary>
      public int DefaultItemWidth {
         get {
            return _defaultItemWidth;
         }
         set {
            // update content pane only if changed to reduce flicker [ml]
            if( _defaultItemWidth != value ) { 
               _defaultItemWidth = value;
               _contentPanel.Invalidate();
            }
         }
      }

      /// <summary>
      /// Gets or sets the <see cref="ImageList"/> that contains the <see cref="Image"/>
      /// objects used by the tree nodes.
      /// </summary>
      /// <remarks>If the ImageList property value is anything other than a 
      /// null reference (Nothing in Visual Basic), all the tree nodes display 
      /// the first Image stored in the ImageList.</remarks>
      public ImageList ImageList {
         get {
            return _imageList;
         }
         set {
            _imageList = value;
         }
      }

      internal void RepaintNode(SmartTreeNode node) {
         _contentPanel.Invalidate(node.GetHighlightArea());
      }

      /// <summary>
      /// Gets the color of the lines.
      /// </summary>
      public Color LineColor {
         get {
            return Color.Gray;
         }
      }

      public SmartTreeNode GetNodeAt(int locationX, int locationY) {
         if( _rootNodes.Count > 0 ) {
            SmartTreeNode current = _rootNodes[0];
            while( current != null
               && !current.Bounds.Contains(locationX, locationY) ) {
               if( current.IsExpanded
                  && current.Nodes.Count > 0 ) {
                  current = current.Nodes[0];
               }
               else if( current.NextNode != null ) {
                  current = current.NextNode;
               }
               else {
                  current = current.Parent;
                  while( current != null ) {
                     if( current.NextNode != null ) {
                        current = current.NextNode;
                        break;
                     }
                     else {
                        current = current.Parent;
                     }
                  }
               }
            }
            return current;
         }
         else {
            return null;
         }
      }

      /// <summary>
      /// Gets the horizontal space between two elements in pixel.
      /// </summary>
      public int HorizontalSpace {
         get {
            return 8;
         }
      }

      /// <summary>
      /// Gets the vertical space between two elements in pixel.
      /// </summary>
      public int VerticalSpace {
         get {
            return 1;
         }
      }

      /// <summary>
      /// Gets the dash style used for drawing the lines.
      /// </summary>
      public DashStyle DashStyle {
         get {
            return DashStyle.Dot;
         }
      }

      protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
         if( keyData == Keys.Down ) {
            ProcessKeyDown();
            return true;
         }
         else {
            return base.ProcessCmdKey(ref msg, keyData);
         }
      }

      private void ProcessKeyDown() {
         SmartTreeNode currentFocus = FocusNode;
         if( currentFocus == null ) {
            if( _rootNodes.Count > 0 ) {
               FocusNode = _rootNodes[0];
            }
         }
         else {
            if( currentFocus.Nodes.Count > 0 ) {
               FocusNode = currentFocus.Nodes[0];
            }
            else if( currentFocus.NextNode != null ) {
               FocusNode = currentFocus.NextNode;
            }
            else {
               while(currentFocus != null) {
                  currentFocus = currentFocus.Parent;
                  if(currentFocus.NextNode != null) {
                     currentFocus = currentFocus.NextNode;
                     break;
                  }
               }
               if(currentFocus != null) {
                  FocusNode = currentFocus;
               }
            }
         }
      }

      internal SmartTreeNode FocusNode {
         get {
            return _focusNode;
         }
         set {
            if(value != _focusNode) {
               if( _focusNode != null ) {
                  RepaintNode(_focusNode);
               }
               _focusNode = value;
               if( _focusNode != null ) {
                  RepaintNode(_focusNode);
               }
            }
         }
      }

      private void OnContentPanelPaint(object sender, PaintEventArgs e) {
         // Draw the nodes
         int x = HorizontalSpace;
         int y = VerticalSpace;
         int horizontalOffset = _expanderWidth + HorizontalSpace;

         if( _rootNodes.Count > 0 ) {
            Rectangle treeBounds = new Rectangle(0, 0, HorizontalSpace, VerticalSpace);
            SmartTreeNode current = _rootNodes[0];
            Rectangle smartTreeRect = RectangleToScreen(ClientRectangle);
            smartTreeRect = _contentPanel.RectangleToClient(smartTreeRect);

            while( current != null ) {
               current.SetTreeView(this);
               Rectangle contentArea = CalculateContentArea(x, y, current.GetContentSize());
               if( contentArea.IntersectsWith(smartTreeRect) ) {
                  PaintNode(current, e.Graphics, x, y);
               }
               treeBounds = Rectangle.Union(treeBounds, contentArea);
               y += contentArea.Height + VerticalSpace;
               if( current.IsExpanded
                  && current.Nodes.Count > 0 ) {
                  current = current.Nodes[0];
                  x += horizontalOffset;
               }
               else if( current.NextNode != null ) {
                  current = current.NextNode;
               }
               else {
                  current = current.Parent;
                  x -= horizontalOffset;
                  while(current != null) {
                     if( current.NextNode != null ) {
                        current = current.NextNode;
                        break;
                     }
                     else {
                        current = current.Parent;
                        x -= horizontalOffset;
                     }
                  }
               }
            }
            _contentPanel.Size = new Size(treeBounds.Width + 2 * HorizontalSpace, treeBounds.Height + VerticalSpace);
         }
         else {
            _contentPanel.Size = Size;
         }
      }

      private void PaintNode(SmartTreeNode theNode, Graphics g, int locationX, int locationY) {
         // Icon dimensions
         int imageMidpointX = locationX + _expanderWidth / 2;
         int imageMidpointY = locationY + (int) ( Font.SizeInPoints * 2 ) / 2;

         // Node dimensions
         Rectangle contentArea = CalculateContentArea(locationX, locationY, theNode.GetContentSize());
         int nodeHeight = contentArea.Height;
         int nodeBottom = locationY + nodeHeight;

         // Update node bounds
         theNode.Bounds = new Rectangle(locationX, locationY,
            _expanderWidth + HorizontalSpace + contentArea.Width,
            contentArea.Height);

         // Draw lines
         using( Pen pen = new Pen(LineColor) ) {
            pen.DashStyle = DashStyle;

            // Horizontal Line
            g.DrawLine(pen, imageMidpointX,
                       imageMidpointY,
                       contentArea.Left, imageMidpointY);

            Point verticalLineTop = new Point(imageMidpointX, imageMidpointY);
            Point verticalLineBottom = verticalLineTop;

            // Vertical line upwards
            if( theNode.Parent != null || theNode.PreviousNode != null ) {
               verticalLineTop = new Point(imageMidpointX, locationY - VerticalSpace);
            }

            // Vertical line downwards
            if( theNode.NextNode != null ) {
               verticalLineBottom = new Point(imageMidpointX, nodeBottom);
            }

            if( verticalLineTop != verticalLineBottom ) {
               g.DrawLine(pen, verticalLineTop, verticalLineBottom);
            }
         }

         // Draw plus and minus
         if( theNode.Nodes.Count > 0 ) {
            int imageLocationY = locationY + ( (int) ( Font.SizeInPoints * 2 ) - _expanderHeight ) / 2;
            if( theNode.IsExpanded ) {
               g.DrawImage(_expanderExpandedImage, locationX, imageLocationY);
            }
            else {
               g.DrawImage(_expanderCollapsedImage, locationX, imageLocationY);
            }
         }

         // draw the vertical dot line for the parent nodes if necessary
         using( Pen pen = new Pen(LineColor) ) {
            pen.DashStyle = DashStyle;
            SmartTreeNode parentNode = theNode.Parent;
            while( parentNode != null ) {
               if( parentNode.NextNode != null ) {
                  int parentImageLocationX = parentNode.Bounds.X + _expanderWidth/2;
                  g.DrawLine(pen, parentImageLocationX,
                             locationY - VerticalSpace,
                             parentImageLocationX,
                             locationY + nodeHeight);
               }
               parentNode = parentNode.Parent;
            }
         }

         // Draw checkbox:
         if(theNode.HasCheckBox) {
            int checkBoxLocationY = locationY + ( (int) ( Font.SizeInPoints * 2 ) - CheckBoxHeight ) / 2;
            if( theNode.Checked ) {
               CheckBoxRenderer.DrawCheckBox(g, new Point(contentArea.Left, checkBoxLocationY), CheckBoxState.CheckedNormal);
            }
            else {
               CheckBoxRenderer.DrawCheckBox(g, new Point(contentArea.Left, checkBoxLocationY), CheckBoxState.UncheckedNormal);
            }
            // Update text area to account for the space needed by the checkbox:
            contentArea = new Rectangle(contentArea.Left + CheckBoxWidth, contentArea.Top, contentArea.Width, contentArea.Height);
         }

         theNode.DrawContent(g, contentArea);
      }

      private Rectangle CalculateContentArea(int locationX, int locationY, Size contentSize) {
         int textAreaLeft = locationX + _expanderWidth + HorizontalSpace;
         return new Rectangle(new Point(textAreaLeft, locationY), contentSize);
      }

      private void SmartTree_FontChanged(object sender, EventArgs e) {
         _contentPanel.Invalidate();
         // This is specific enough since the content panel is supposed to be
         // just big enough so that all nodes fit inside of it. [ml]
      }

      private void OnContentPanelMouseDown(object sender, MouseEventArgs e) {
         SmartTreeNode node = GetNodeAt(e.X, e.Y);
         if( node != null ) {
            FocusNode = node;
            switch(node.HitTest(node, e.Location)) {
               case HitTestResult.NodeContent:
                  if( ( ModifierKeys & Keys.Control ) != 0 ) {
                     if( node.IsSelected ) {
                        RemovedFromSelectedNodesCollection(node);
                     }
                     else {
                        AddToSelectedNodesCollection(node);
                     }
                  }
                  else if( ( ModifierKeys & Keys.Shift ) != 0 ) { // Extend selection
                     SelectNodesBetween(FindRecentSelectedNode(), node);
                  }
                  else {
                     ClearSelectedNodesCollection();
                     AddToSelectedNodesCollection(node);
                  }
                  break;
               case HitTestResult.Expander:
                  if( node.IsExpanded ) {
                     node.Collapse();
                  }
                  else {
                     node.Expand();
                  }
                  _contentPanel.Invalidate(new Rectangle(0, node.Bounds.Top, _contentPanel.Right, _contentPanel.Bottom));
                  break;
               case HitTestResult.CheckBox:
                  node.Checked = !node.Checked;
                  _contentPanel.Invalidate(new Rectangle(
                     node.Bounds.Left + Properties.Resources.Plus.Width + HorizontalSpace,
                     node.Bounds.Top,
                     CheckBoxWidth,
                     node.Bounds.Height));
                  break;
            }
         }
      }

      private void SelectNodesBetween(SmartTreeNode node1, SmartTreeNode node2) {
         bool forward = node2.Bounds.Top > node1.Bounds.Top;
         if( forward ) {
            SmartTreeNode current = node1;
            while(current != null
               && current != node2) {
               AddToSelectedNodesCollection(current);
               if( current.Nodes.Count > 0 ) {
                  current = current.Nodes[0];
               }
               else if( current.NextNode != null ) {
                  current = current.NextNode;
               }
               else {
                  if( current.Parent != null ) {
                     current = current.Parent.NextNode;
                  }
                  else {
                     current = null;  // done
                  }
               }
            }
         }
         else {
            SmartTreeNode current = node1;
            while( current != null
               && current != node2 ) {
               AddToSelectedNodesCollection(current);
               if(current.PreviousNode != null ) {
                  current = current.PreviousNode;
                  while( current.Nodes.Count > 0 ) {
                     current = current.Nodes[current.Nodes.Count - 1];
                  }
               }
               else if(current.Parent != null ) {
                  current = current.Parent;
               }
            }
         }
         AddToSelectedNodesCollection(node2);
      }

      private SmartTreeNode FindRecentSelectedNode() {
         SmartTreeNode recentSelectedNode = null;
         if( _selectedNodes.Count > 0 ) {
            recentSelectedNode = _selectedNodes[_selectedNodes.Count - 1];
         }
         return recentSelectedNode;
      }

      private void RemovedFromSelectedNodesCollection(SmartTreeNode node) {
         _selectedNodes.Remove(node);
         node.IsSelected = false;
         RepaintNode(node);
      }

      /// <summary>
      /// Gets the collection of nodes in the tree that are currently selected.
      /// </summary>
      public List<SmartTreeNode> SelectedNodes {
         get {
            return _selectedNodes;
         }
      }

      private void AddToSelectedNodesCollection(SmartTreeNode node) {
         if( !_selectedNodes.Contains(node) ) {
            _selectedNodes.Add(node);
            node.IsSelected = true;
            RepaintNode(node);
            if(AfterSelect != null) {
               AfterSelect(this, new SmartTreeEventHandlerArgs(node));
            }
         }
      }

      private void ClearSelectedNodesCollection() {
         foreach(SmartTreeNode selectedNode in _selectedNodes) {
            selectedNode.IsSelected = false;
            RepaintNode(selectedNode);
         }
         _selectedNodes.Clear();
      }

      private int _defaultItemWidth = int.MaxValue;
      // This results in nodes with a single line unless the text on the node 
      // contains line feeds. [ml]

      private ImageList _imageList = null;
      private readonly SmartTreeNodeCollection _rootNodes = null;
      private readonly List<SmartTreeNode> _selectedNodes = null;
      internal const int CheckBoxWidth = 13;
      internal const int CheckBoxHeight = 13;
      private bool _showNodeToolTips = true;
      private readonly int _expanderHeight = Properties.Resources.Plus.Height;
      private readonly int _expanderWidth = Properties.Resources.Plus.Width;
      private readonly Bitmap _expanderExpandedImage = Properties.Resources.Minus;
      private readonly Bitmap _expanderCollapsedImage = Properties.Resources.Plus;
      private SmartTreeNode _focusNode = null;

      private class SmartTreeContentPanel : Panel {
         public void SetOwnerTree(SmartTree owner) {
            _smartTree = owner;
         }
         
         protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if( _smartTree != null ) {
               return _smartTree.ProcessCmdKey(ref msg, keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
         }

         private SmartTree _smartTree = null;
      }
   }
}
