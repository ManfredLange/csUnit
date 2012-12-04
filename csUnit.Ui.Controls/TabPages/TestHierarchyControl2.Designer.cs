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

#if USE_NEW_TEST_TREE

namespace csUnit.Ui.Controls.TabPages {
   partial class TestHierarchyControl2 {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestHierarchyControl2));
         this._imageList = new System.Windows.Forms.ImageList(this.components);
         this._toolTips = new System.Windows.Forms.ToolTip(this.components);
         this._btnGo = new System.Windows.Forms.Button();
         this._treeTestHierarchy = new csUnit.Ui.Controls.SmartTree();
         this._txtSearchString = new System.Windows.Forms.TextBox();
         this._searchPanel = new System.Windows.Forms.Panel();
         this._resultsList = new System.Windows.Forms.ListBox();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._resetButton = new System.Windows.Forms.ToolStripButton();
         this._searchButton = new System.Windows.Forms.ToolStripButton();
         this._searchPanel.SuspendLayout();
         this._toolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _imageList
         // 
         this._imageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject("_imageList.ImageStream") ) );
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
         // 
         // _btnGo
         // 
         this._btnGo.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._btnGo.FlatAppearance.BorderSize = 0;
         this._btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this._btnGo.Image = global::csUnit.Ui.Controls.Properties.Resources.GoLtrHS;
         this._btnGo.Location = new System.Drawing.Point(65, 2);
         this._btnGo.Name = "_btnGo";
         this._btnGo.Size = new System.Drawing.Size(16, 16);
         this._btnGo.TabIndex = 4;
         this._toolTips.SetToolTip(this._btnGo, "Search");
         this._btnGo.UseVisualStyleBackColor = true;
         this._btnGo.Click += new System.EventHandler(this._btnGo_Click);
         // 
         // _treeTestHierarchy
         // 
         this._treeTestHierarchy.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._treeTestHierarchy.AutoScroll = true;
         this._treeTestHierarchy.BackColor = System.Drawing.SystemColors.Window;
         this._treeTestHierarchy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._treeTestHierarchy.DefaultItemWidth = 2147483647;
         this._treeTestHierarchy.ImageList = this._imageList;
         this._treeTestHierarchy.Location = new System.Drawing.Point(0, 28);
         this._treeTestHierarchy.Name = "_treeTestHierarchy";
         this._treeTestHierarchy.ShowNodeToolTips = true;
         this._treeTestHierarchy.Size = new System.Drawing.Size(398, 315);
         this._treeTestHierarchy.TabIndex = 1;
         this._toolTips.SetToolTip(this._treeTestHierarchy, "When one or more assemblies are loaded\r\nthe tests within their hierarchy are disp" +
                 "layed.\r\nUse checkboxes for selecting tests.\r\nUse context menu for more commands." +
                 "");
         // 
         // _txtSearchString
         // 
         this._txtSearchString.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._txtSearchString.Location = new System.Drawing.Point(4, 1);
         this._txtSearchString.Name = "_txtSearchString";
         this._txtSearchString.Size = new System.Drawing.Size(58, 20);
         this._txtSearchString.TabIndex = 3;
         // 
         // _searchPanel
         // 
         this._searchPanel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._searchPanel.Controls.Add(this._resultsList);
         this._searchPanel.Controls.Add(this._txtSearchString);
         this._searchPanel.Controls.Add(this._btnGo);
         this._searchPanel.Location = new System.Drawing.Point(404, 231);
         this._searchPanel.Name = "_searchPanel";
         this._searchPanel.Size = new System.Drawing.Size(88, 91);
         this._searchPanel.TabIndex = 6;
         // 
         // _resultsList
         // 
         this._resultsList.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._resultsList.FormattingEnabled = true;
         this._resultsList.HorizontalScrollbar = true;
         this._resultsList.IntegralHeight = false;
         this._resultsList.Location = new System.Drawing.Point(4, 26);
         this._resultsList.Name = "_resultsList";
         this._resultsList.Size = new System.Drawing.Size(81, 65);
         this._resultsList.TabIndex = 5;
         this._resultsList.DoubleClick += new System.EventHandler(this._resultsList_DoubleClick);
         // 
         // _toolStrip
         // 
         this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
         this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._resetButton,
            this._searchButton});
         this._toolStrip.Location = new System.Drawing.Point(0, 0);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(427, 25);
         this._toolStrip.Stretch = true;
         this._toolStrip.TabIndex = 7;
         this._toolStrip.Text = "_toolStrip";
         // 
         // _resetButton
         // 
         this._resetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this._resetButton.Image = ( (System.Drawing.Image) ( resources.GetObject("_resetButton.Image") ) );
         this._resetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._resetButton.Name = "_resetButton";
         this._resetButton.Size = new System.Drawing.Size(39, 22);
         this._resetButton.Text = "Reset";
         this._resetButton.Click += new System.EventHandler(this._resetButton_Click);
         // 
         // _searchButton
         // 
         this._searchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this._searchButton.Image = ( (System.Drawing.Image) ( resources.GetObject("_searchButton.Image") ) );
         this._searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._searchButton.Name = "_searchButton";
         this._searchButton.Size = new System.Drawing.Size(46, 22);
         this._searchButton.Text = "Search";
         this._searchButton.Click += new System.EventHandler(this._searchButton_Click);
         // 
         // TestHierarchyControl2
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._treeTestHierarchy);
         this.Controls.Add(this._toolStrip);
         this.Controls.Add(this._searchPanel);
         this.Name = "TestHierarchyControl2";
         this.Size = new System.Drawing.Size(427, 357);
         this._toolTips.SetToolTip(this, "When one or more assemblies are loaded\r\nthe tests within their hierarchy are disp" +
                 "layed.\r\nUse checkboxes for selecting tests.\r\nUse context menu for more commands." +
                 "");
         this._searchPanel.ResumeLayout(false);
         this._searchPanel.PerformLayout();
         this._toolStrip.ResumeLayout(false);
         this._toolStrip.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ImageList _imageList;
      private System.Windows.Forms.ToolTip _toolTips;
      private System.Windows.Forms.TextBox _txtSearchString;
      private System.Windows.Forms.Button _btnGo;
      private System.Windows.Forms.Panel _searchPanel;
      private System.Windows.Forms.ListBox _resultsList;
      private System.Windows.Forms.ToolStrip _toolStrip;
      private System.Windows.Forms.ToolStripButton _searchButton;
      private System.Windows.Forms.ToolStripButton _resetButton;
   }
}

#endif // DEBUG
