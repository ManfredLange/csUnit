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

namespace csUnit.Ui.Controls.TabPages {
   partial class TestHierarchyControl {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if(disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("TestMethod11", 3, 3);
         System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("TestMethod12", 3, 3);
         System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("TestFixture10", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
         System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("TestMethod21", 3, 3);
         System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("TestFixture20", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode4});
         System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("TestAssemblyA", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode5});
         System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("TestAssemblyB");
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestHierarchyControl));
         this._treeTestHierarchy = new System.Windows.Forms.TreeView();
         this._imageList = new System.Windows.Forms.ImageList(this.components);
         this._toolTips = new System.Windows.Forms.ToolTip(this.components);
         this.SuspendLayout();
         // 
         // _treeTestHierarchy
         // 
         this._treeTestHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this._treeTestHierarchy.BackColor = System.Drawing.SystemColors.Window;
         this._treeTestHierarchy.CheckBoxes = true;
         this._treeTestHierarchy.HideSelection = false;
         this._treeTestHierarchy.ImageIndex = 0;
         this._treeTestHierarchy.ImageList = this._imageList;
         this._treeTestHierarchy.Location = new System.Drawing.Point(4, 3);
         this._treeTestHierarchy.Name = "_treeTestHierarchy";
         treeNode1.ImageIndex = 3;
         treeNode1.Name = "Node2";
         treeNode1.SelectedImageIndex = 3;
         treeNode1.Text = "TestMethod11";
         treeNode2.ImageIndex = 3;
         treeNode2.Name = "Node3";
         treeNode2.SelectedImageIndex = 3;
         treeNode2.Text = "TestMethod12";
         treeNode3.ImageIndex = 2;
         treeNode3.Name = "Node0";
         treeNode3.SelectedImageIndex = 2;
         treeNode3.Text = "TestFixture10";
         treeNode4.ImageIndex = 3;
         treeNode4.Name = "Node4";
         treeNode4.SelectedImageIndex = 3;
         treeNode4.Text = "TestMethod21";
         treeNode5.ImageIndex = 2;
         treeNode5.Name = "Node1";
         treeNode5.SelectedImageIndex = 2;
         treeNode5.Text = "TestFixture20";
         treeNode6.Name = "Node0";
         treeNode6.Text = "TestAssemblyA";
         treeNode7.Name = "Node1";
         treeNode7.Text = "TestAssemblyB";
         this._treeTestHierarchy.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7});
         this._treeTestHierarchy.SelectedImageIndex = 0;
         this._treeTestHierarchy.ShowNodeToolTips = true;
         this._treeTestHierarchy.Size = new System.Drawing.Size(366, 330);
         this._treeTestHierarchy.TabIndex = 0;
         this._toolTips.SetToolTip(this._treeTestHierarchy, "When one or more assemblies are loaded\r\nthe tests within their hierarchy are disp" +
                 "layed.\r\nUse checkboxes for selecting tests.\r\nUse context menu for more commands." +
                 "");
         this._treeTestHierarchy.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterCheck);
         // 
         // _imageList
         // 
         this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("_imageList.ImageStream")));
         this._imageList.TransparentColor = System.Drawing.Color.Transparent;
         this._imageList.Images.SetKeyName(0, "");
         this._imageList.Images.SetKeyName(1, "");
         this._imageList.Images.SetKeyName(2, "");
         this._imageList.Images.SetKeyName(3, "");
         this._imageList.Images.SetKeyName(4, "");
         this._imageList.Images.SetKeyName(5, "");
         this._imageList.Images.SetKeyName(6, "");
         this._imageList.Images.SetKeyName(7, "");
         this._imageList.Images.SetKeyName(8, "");
         this._imageList.Images.SetKeyName(9, "");
         this._imageList.Images.SetKeyName(10, "");
         this._imageList.Images.SetKeyName(11, "");
         this._imageList.Images.SetKeyName(12, "");
         this._imageList.Images.SetKeyName(13, "");
         this._imageList.Images.SetKeyName(14, "");
         this._imageList.Images.SetKeyName(15, "");
         this._imageList.Images.SetKeyName(16, "");
         this._imageList.Images.SetKeyName(17, "");
         this._imageList.Images.SetKeyName(18, "");
         this._imageList.Images.SetKeyName(19, "");
         this._imageList.Images.SetKeyName(20, "");
         this._imageList.Images.SetKeyName(21, "");
         this._imageList.Images.SetKeyName(22, "");
         this._imageList.Images.SetKeyName(23, "");
         this._imageList.Images.SetKeyName(24, "");
         this._imageList.Images.SetKeyName(25, "CallStack.bmp");
         // 
         // TestHierarchyControl
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._treeTestHierarchy);
         this.Name = "TestHierarchyControl";
         this.Size = new System.Drawing.Size(373, 336);
         this._toolTips.SetToolTip(this, "When one or more assemblies are loaded\r\nthe tests within their hierarchy are disp" +
                 "layed.\r\nUse checkboxes for selecting tests.\r\nUse context menu for more commands." +
                 "");
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ImageList _imageList;
      private System.Windows.Forms.ToolTip _toolTips;
   }
}
