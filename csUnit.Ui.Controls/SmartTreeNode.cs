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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace csUnit.Ui.Controls {
   public class SmartTreeNode {
      public SmartTreeNode() {
         _nodes = new SmartTreeNodeCollection(this, _ownerTree);
         
      }

      public SmartTreeNode(string label) : this() {
         _label = label;
      }

      public SmartTreeNode(string label, SmartTreeNode[] children) 
      : this(label) {
         _nodes.AddRange(children);
      }

      public SmartTreeNode(string label, int imageIndex,
         int selectedImageIndex)
         : this(label) {
         _imageIndex = imageIndex;
         _selectedImageIndex = selectedImageIndex;
      }

      public SmartTreeNode(string label, int imageIndex, 
         int selectedImageIndex, SmartTreeNode[] children) 
         : this(label, imageIndex, selectedImageIndex) {
         _nodes.AddRange(children);
      }

      public override string ToString() {
         return _label;
      }

      /// <summary>
      /// Gets or set a value indicating whether this node is checked. This can
      /// be used with or without a checkbox. The check status will be visible
      /// only if the property 'HasCheckBox' has been set to true.
      /// </summary>
      public bool Checked {
         get {
            return _checked;
         }
         set {
            _checked = value;
         }
      }

      /// <summary>
      /// Gets or sets a value indicating whether this node has a checkbox.
      /// </summary>
      public bool HasCheckBox {
         get {
            return _hasCheckBox;
         }
         set {
            _hasCheckBox = value;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the node is expanded. This is meaningful
      /// only if the node has children.
      /// </summary>
      public bool IsExpanded {
         get {
            return _expanded;
         }
      }

      /// <summary>
      /// Collapses the node. If the node has no children then calling this
      /// method does nothing.
      /// </summary>
      public void Collapse() {
         _expanded = false;
      }

      /// <summary>
      /// Collapses the node. If the node has no children then calling this
      /// method does nothing.
      /// </summary>
      public void Expand() {
         _expanded = true;
      }

      public int ImageIndex {
         get {
            return _imageIndex;
         }
         set {
            if(_imageIndex != value
               && _ownerTree != null ) {
               _ownerTree.RepaintNode(this);
            }
            _imageIndex = value;
            _image = null;
         }
      }

      /// <summary>
      /// Gets or sets a value indicating whether this node is selected. If
      /// selected the node will be drawn highlighted.
      /// </summary>
      public bool IsSelected {
         get {
            return _selected;
         }
         set {
            _selected = value;
         }
      }

      /// <summary>
      /// Gets the parent tree node or null if the node has not been added as 
      /// a child to a different tree node. All root nodes return null.
      /// </summary>
      public SmartTreeNode Parent {
         get {
            return _parentNode;
         }
      }

      public object Tag {
         get {
            return _tag;
         }
         set {
            _tag = value;
         }
      }

      public string Text {
         get {
            return _label;
         }
         set {
            _label = value;
         }
      }

      public string ToolTipText {
         get {
            return _toolTipText;
         }
         set {
            _toolTipText = value;
         }
      }

      /// <summary>
      /// Gets the tree view that owns this node. The return value is null if
      /// the node has not been added to a tree or to a node that is already
      /// owned by a tree.
      /// </summary>
      public SmartTree TreeView {
         get {
            return _ownerTree;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the tree node is visible or partially 
      /// visible.
      /// </summary>
      public bool IsVisible {
         get {
            return true;
         }
      }

      /// <summary>
      /// Gets the last child tree node.
      /// </summary>
      public SmartTreeNode LastNode {
         get {
            if( _nodes.Count > 0 ) {
               return _nodes[_nodes.Count - 1];
            }
            else {
               return null;
            }
         }
      }

      public string Name {
         get {
            return _name;
         }
         set {
            _name = value;
         }
      }

      /// <summary>
      /// Gets the next sibling of the node. If there is none, the return value
      /// is null.
      /// </summary>
      public SmartTreeNode NextNode {
         get {
            return _nextNode;
         }
      }

      /// <summary>
      /// Gets the previous sibling of the node. If there is none, the return
      /// value is null.
      /// </summary>
      public SmartTreeNode PreviousNode {
         get {
            return _previousNode;
         }
      }

      /// <summary>
      /// Removes the node from the tree.
      /// </summary>
      public void Remove() {
         if( _parentNode != null ) {
            _parentNode._nodes.Remove(this);
         }
      }

      public int SelectedImageIndex {
         get {
            return _selectedImageIndex;
         }
         set {
            _selectedImageIndex = value;
         }
      }


      /// <summary>
      /// Gets a collection of the children of this node.
      /// </summary>
      public SmartTreeNodeCollection Nodes {
         get {
            _nodes.SetParent(this);
            _nodes.SetOwnerTree(_ownerTree);
            return _nodes;
         }
      }

      internal void SetNextNode(SmartTreeNode nextNode) {
         _nextNode = nextNode;
      }

      internal void SetPreviousNode(SmartTreeNode previousNode) {
         _previousNode = previousNode;
      }

      internal void SetParent(SmartTreeNode parentNode) {
         _parentNode = parentNode;
         foreach( SmartTreeNode node in _nodes ) {
            node.SetParent(this);
         }
      }

      internal void SetTreeView(SmartTree ownerTreeView) {
         _ownerTree = ownerTreeView;
         foreach( SmartTreeNode node in _nodes ) {
            node.SetTreeView(ownerTreeView);
         }
      }

      public SmartTreeNode FirstNode {
         get {
            if(_nodes.Count > 0) {
               return _nodes[0];
            }
            else {
               return null;
            }
         }
      }

      internal Size GetContentSize() {
         Size suggestedTextSize;
         int imageWidth = 0;
         int imageHeight = 0;
         if(ImageIndex != NoImage) {
            Bitmap image = Image;
            if(image != null) {
               imageWidth = image.Width;
            }
            suggestedTextSize = new Size(_ownerTree.DefaultItemWidth - imageWidth,
                                         (int) _ownerTree.Font.SizeInPoints);
         }
         else {
            suggestedTextSize = new Size(_ownerTree.DefaultItemWidth, (int) _ownerTree.Font.SizeInPoints);
         }
         if( _currentSize == Size.Empty 
            || _label != _currentLabel
            || _currentSuggestedTextSize != suggestedTextSize
            || _currentFont != _ownerTree.Font ) {
            Size textSize = TextRenderer.MeasureText(_label, _ownerTree.Font, suggestedTextSize, _flags);
            _currentSize = new Size(imageWidth + textSize.Width, Math.Max(imageHeight, textSize.Height));
            _currentLabel = _label;
            _currentSuggestedTextSize = suggestedTextSize;
            _currentFont = _ownerTree.Font;
         }
         return _currentSize;
      }

      // The following three are used for caching and performance in the 
      // preceeding method. [20apr08, ml]
      private String _currentLabel = string.Empty;
      private Size _currentSize = Size.Empty;
      private Size _currentSuggestedTextSize = Size.Empty;
      private Font _currentFont = null;

      internal Rectangle GetHighlightArea() {
         Size contentSize = GetContentSize();
         int x = Bounds.Right - contentSize.Width;
         if( HasCheckBox ) {
            x += SmartTree.CheckBoxWidth;
         }
         return new Rectangle(
            x,
            Bounds.Top - 1,
            contentSize.Width,
            Bounds.Height + 2);
      }

      public void EnsureVisible() {
         // TODO: implement this method [17apr08, ml]
      }

      public void ExpandAll() {
         // TODO: implement this method [17apr08, ml]
      }

      private Bitmap Image {
         get {
            if( _image == null 
               && ImageIndex != NoImage ) {
               _image = _ownerTree.ImageList.Images[ImageIndex] as Bitmap;
            }
            return _image;
         }
      }

      internal void DrawContent(Graphics g, Rectangle contentArea) {
         Rectangle textArea = contentArea;
         Bitmap image = null;
         if(ImageIndex != NoImage) {
            image = Image;
            if( image != null ) {
               textArea = new Rectangle(contentArea.Left + image.Width,
                  contentArea.Top, contentArea.Width - image.Width,
                  contentArea.Height);
            }
         }
         if( IsSelected ) {
            Rectangle highlightRectangle = GetHighlightArea();
            g.FillRectangle(Brushes.DodgerBlue, highlightRectangle);
            TextRenderer.DrawText(g, _label, _ownerTree.Font, textArea,
                                  Color.White, Color.DodgerBlue, _flags);
         }
         else {
            TextRenderer.DrawText(g, _label, _ownerTree.Font, textArea,
                                  _ownerTree.ForeColor, _ownerTree.BackColor, _flags);
         }
         if( _ownerTree.FocusNode == this ) {
            Rectangle highlightRectangle = GetHighlightArea();
            ControlPaint.DrawFocusRectangle(g, highlightRectangle);
         }
         if( image != null ) {
            ImageAttributes attr = new ImageAttributes();
            Rectangle destRect = new Rectangle(contentArea.X,
               contentArea.Y + ( (int) ( _ownerTree.Font.SizeInPoints * 2 ) - image.Height ) / 2, 
               image.Width, image.Height);
            attr.SetColorKey(image.GetPixel(0, 0), image.GetPixel(0, 0));
            g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
         }
      }

      public Color BackColor {
         get {
            return _backColor;
         }
         set {
            _backColor = value;
         }
      }

      internal Rectangle Bounds {
         get {
            return _bounds;
         }
         set {
            _bounds = value;
         }
      }

      internal HitTestResult HitTest(SmartTreeNode node, Point location) {
         if( node.Bounds.Contains(location) ) {
            if( node.GetHighlightArea().Contains(location) ) {
               return HitTestResult.NodeContent;
            }
            if( node.Nodes.Count > 0 ) {
               int iconHeight = Properties.Resources.Plus.Height;
               int imageLocationY = node.Bounds.Top + ( (int) ( _ownerTree.Font.SizeInPoints * 2 ) - iconHeight ) / 2;
               Rectangle expanderRectangle = new Rectangle(node.Bounds.Left, imageLocationY,
                                                           Properties.Resources.Plus.Width,
                                                           Properties.Resources.Plus.Height);
               if( expanderRectangle.Contains(location) ) {
                  return HitTestResult.Expander;
               }
            }
            if( node.HasCheckBox ) {
               int iconWidth = Properties.Resources.Plus.Width;
               int checkBoxLocationY = node.Bounds.Top + ( (int) ( _ownerTree.Font.SizeInPoints * 2 ) - SmartTree.CheckBoxHeight ) / 2;
               int checkBoxLocationX = node.Bounds.Left + iconWidth + _ownerTree.HorizontalSpace;
               Rectangle checkBoxRectangle = new Rectangle(checkBoxLocationX, checkBoxLocationY,
                  SmartTree.CheckBoxWidth, SmartTree.CheckBoxHeight);
               if( checkBoxRectangle.Contains(location) ) {
                  return HitTestResult.CheckBox;
               }
            }
         }
         return HitTestResult.None;
      }
      
      private const TextFormatFlags _flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak |
                                             TextFormatFlags.NoFullWidthCharacterBreak
                                             | TextFormatFlags.ExternalLeading;// | TextFormatFlags.PreserveGraphicsClipping; 

      private const int NoImage = -1;
      private Rectangle _bounds = Rectangle.Empty;
      private  string _label = string.Empty;
      private SmartTree _ownerTree = null;
      private SmartTreeNode _parentNode = null;
      private readonly SmartTreeNodeCollection _nodes;
      private SmartTreeNode _nextNode = null;
      private SmartTreeNode _previousNode = null;
      private bool _expanded = false;
      private bool _selected = false;
      private bool _hasCheckBox = false;
      private bool _checked = false;
      private int _imageIndex = NoImage;
      private int _selectedImageIndex = NoImage;
      private object _tag = null;
      private string _toolTipText = string.Empty;
      private string _name = string.Empty;
      private Color _backColor = SystemColors.Window;
      private Bitmap _image = null;
   }
}
