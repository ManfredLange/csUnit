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

namespace csUnit.Ui.Controls.TabPages {
   partial class CategoryControl {
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
         System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "API",
            "Include"}, -1);
         System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Default",
            "Include"}, -1);
         System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Invoicing module",
            "Exclude"}, -1);
         System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "John\'s stuff",
            "Don\'t care"}, -1);
         this._toolTip = new System.Windows.Forms.ToolTip(this.components);
         this._resetButton = new System.Windows.Forms.Button();
         this._categoriesListView = new System.Windows.Forms.ListView();
         this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
         this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
         this._includeCategoryButton = new System.Windows.Forms.Button();
         this._excludeCategoryButton = new System.Windows.Forms.Button();
         this._dontCareButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // _resetButton
         // 
         this._resetButton.Location = new System.Drawing.Point(90, 3);
         this._resetButton.Name = "_resetButton";
         this._resetButton.Size = new System.Drawing.Size(45, 23);
         this._resetButton.TabIndex = 8;
         this._resetButton.Text = "Reset";
         this._toolTip.SetToolTip(this._resetButton, "Reset Selections");
         this._resetButton.UseVisualStyleBackColor = true;
         this._resetButton.Click += new System.EventHandler(this._resetButton_Click);
         // 
         // _categoriesListView
         // 
         this._categoriesListView.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._categoriesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
         this._categoriesListView.FullRowSelect = true;
         this._categoriesListView.HideSelection = false;
         this._categoriesListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
         this._categoriesListView.Location = new System.Drawing.Point(3, 29);
         this._categoriesListView.Name = "_categoriesListView";
         this._categoriesListView.Size = new System.Drawing.Size(237, 318);
         this._categoriesListView.TabIndex = 7;
         this._toolTip.SetToolTip(this._categoriesListView, "Select categories, then use +/-/0 to include/exclude/don\'t care");
         this._categoriesListView.UseCompatibleStateImageBehavior = false;
         this._categoriesListView.View = System.Windows.Forms.View.Details;
         this._categoriesListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this._categoriesListView_MouseUp);
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "Category";
         this.columnHeader1.Width = 120;
         // 
         // columnHeader2
         // 
         this.columnHeader2.Text = "What";
         this.columnHeader2.Width = 70;
         // 
         // _includeCategoryButton
         // 
         this._includeCategoryButton.Enabled = false;
         this._includeCategoryButton.Location = new System.Drawing.Point(3, 3);
         this._includeCategoryButton.Name = "_includeCategoryButton";
         this._includeCategoryButton.Size = new System.Drawing.Size(23, 23);
         this._includeCategoryButton.TabIndex = 9;
         this._includeCategoryButton.Text = "+";
         this._toolTip.SetToolTip(this._includeCategoryButton, "Include selected categories");
         this._includeCategoryButton.UseVisualStyleBackColor = true;
         this._includeCategoryButton.Click += new System.EventHandler(this._includeCategoryButton_Click);
         // 
         // _excludeCategoryButton
         // 
         this._excludeCategoryButton.Enabled = false;
         this._excludeCategoryButton.Location = new System.Drawing.Point(32, 3);
         this._excludeCategoryButton.Name = "_excludeCategoryButton";
         this._excludeCategoryButton.Size = new System.Drawing.Size(23, 23);
         this._excludeCategoryButton.TabIndex = 10;
         this._excludeCategoryButton.Text = "-";
         this._toolTip.SetToolTip(this._excludeCategoryButton, "Exclude selected categories");
         this._excludeCategoryButton.UseVisualStyleBackColor = true;
         this._excludeCategoryButton.Click += new System.EventHandler(this._excludeCategoryButton_Click);
         // 
         // _dontCareButton
         // 
         this._dontCareButton.Enabled = false;
         this._dontCareButton.Location = new System.Drawing.Point(61, 3);
         this._dontCareButton.Name = "_dontCareButton";
         this._dontCareButton.Size = new System.Drawing.Size(23, 23);
         this._dontCareButton.TabIndex = 11;
         this._dontCareButton.Text = "0";
         this._toolTip.SetToolTip(this._dontCareButton, "Don\'t care about selected categories");
         this._dontCareButton.UseVisualStyleBackColor = true;
         this._dontCareButton.Click += new System.EventHandler(this._dontCareButton_Click);
         // 
         // CategoryControl
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._dontCareButton);
         this.Controls.Add(this._excludeCategoryButton);
         this.Controls.Add(this._includeCategoryButton);
         this.Controls.Add(this._resetButton);
         this.Controls.Add(this._categoriesListView);
         this.Name = "CategoryControl";
         this.Size = new System.Drawing.Size(243, 350);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ToolTip _toolTip;
      private System.Windows.Forms.ListView _categoriesListView;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.Button _resetButton;
      private System.Windows.Forms.Button _includeCategoryButton;
      private System.Windows.Forms.Button _excludeCategoryButton;
      private System.Windows.Forms.Button _dontCareButton;
   }
}
