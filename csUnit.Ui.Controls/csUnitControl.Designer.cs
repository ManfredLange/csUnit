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

namespace csUnit.Ui.Controls {
   partial class CsUnitControl {
      /// <summary>
      /// Required designer variable.
      /// </summary>
// ReSharper disable InconsistentNaming
      private readonly System.ComponentModel.Container components;
// ReSharper restore InconsistentNaming

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
         this._tabControl = new System.Windows.Forms.TabControl();
         this.FindPanel = new System.Windows.Forms.Panel();
         this._label1 = new System.Windows.Forms.Label();
         this._findResultsListView = new System.Windows.Forms.ListView();
         this._columnHeader1 = new System.Windows.Forms.ColumnHeader();
         this._columnHeader2 = new System.Windows.Forms.ColumnHeader();
         this._findPanelToolStrip = new System.Windows.Forms.ToolStrip();
         this._findTermDropDown = new System.Windows.Forms.ToolStripComboBox();
         this._findButton = new System.Windows.Forms.ToolStripButton();
         this._clearSearchButton = new System.Windows.Forms.ToolStripButton();
         this._progressBar = new csUnit.Ui.Controls.ProgressBar();
         this.FindPanel.SuspendLayout();
         this._findPanelToolStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _tabControl
         // 
         this._tabControl.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._tabControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this._tabControl.Location = new System.Drawing.Point(0, 26);
         this._tabControl.Name = "_tabControl";
         this._tabControl.SelectedIndex = 0;
         this._tabControl.ShowToolTips = true;
         this._tabControl.Size = new System.Drawing.Size(633, 359);
         this._tabControl.TabIndex = 2;
         // 
         // _findPanel
         // 
         this.FindPanel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this.FindPanel.Controls.Add(this._label1);
         this.FindPanel.Controls.Add(this._findResultsListView);
         this.FindPanel.Controls.Add(this._findPanelToolStrip);
         this.FindPanel.Location = new System.Drawing.Point(0, 26);
         this.FindPanel.Name = "FindPanel";
         this.FindPanel.Size = new System.Drawing.Size(633, 358);
         this.FindPanel.TabIndex = 4;
         // 
         // _label1
         // 
         this._label1.AutoSize = true;
         this._label1.Location = new System.Drawing.Point(4, 28);
         this._label1.Name = "_label1";
         this._label1.Size = new System.Drawing.Size(68, 13);
         this._label1.TabIndex = 5;
         this._label1.Text = "Find Results:";
         // 
         // _findResultsListView
         // 
         this._findResultsListView.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._findResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._columnHeader1,
            this._columnHeader2});
         this._findResultsListView.FullRowSelect = true;
         this._findResultsListView.Location = new System.Drawing.Point(7, 44);
         this._findResultsListView.MultiSelect = false;
         this._findResultsListView.Name = "_findResultsListView";
         this._findResultsListView.ShowItemToolTips = true;
         this._findResultsListView.Size = new System.Drawing.Size(619, 308);
         this._findResultsListView.TabIndex = 4;
         this._findResultsListView.UseCompatibleStateImageBehavior = false;
         this._findResultsListView.View = System.Windows.Forms.View.Details;
         this._findResultsListView.VisibleChanged += new System.EventHandler(this._findResultsListView_VisibleChanged);
         this._findResultsListView.DoubleClick += new System.EventHandler(this._findResultsListView_DoubleClick);
         // 
         // _columnHeader1
         // 
         this._columnHeader1.Text = "Item";
         this._columnHeader1.Width = 150;
         // 
         // _columnHeader2
         // 
         this._columnHeader2.Text = "Location";
         this._columnHeader2.Width = 90;
         // 
         // _findPanelToolStrip
         // 
         this._findPanelToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
         this._findPanelToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._findTermDropDown,
            this._findButton,
            this._clearSearchButton});
         this._findPanelToolStrip.Location = new System.Drawing.Point(0, 0);
         this._findPanelToolStrip.Name = "_findPanelToolStrip";
         this._findPanelToolStrip.Padding = new System.Windows.Forms.Padding(6, 0, 1, 0);
         this._findPanelToolStrip.Size = new System.Drawing.Size(633, 25);
         this._findPanelToolStrip.TabIndex = 3;
         this._findPanelToolStrip.Text = "toolStrip1";
         // 
         // _findTermDropDown
         // 
         this._findTermDropDown.Name = "_findTermDropDown";
         this._findTermDropDown.Size = new System.Drawing.Size(100, 25);
         this._findTermDropDown.ToolTipText = "Enter search term";
         this._findTermDropDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this._findTermDropDown_KeyUp);
         this._findTermDropDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this._findTermDropDown_KeyDown);
         this._findTermDropDown.DropDown += new System.EventHandler(this._findTermDropDown_DropDown);
         // 
         // _findButton
         // 
         this._findButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._findButton.Image = global::csUnit.Ui.Controls.Properties.Resources.GoLtrHS;
         this._findButton.ImageTransparentColor = System.Drawing.Color.Magenta;
         this._findButton.Name = "_findButton";
         this._findButton.Size = new System.Drawing.Size(23, 22);
         this._findButton.ToolTipText = "Search";
         this._findButton.Click += new System.EventHandler(this._findButton_Click);
         // 
         // _clearSearchButton
         // 
         this._clearSearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this._clearSearchButton.Enabled = false;
         this._clearSearchButton.Image = global::csUnit.Ui.Controls.Properties.Resources.ClearSearch;
         this._clearSearchButton.ImageTransparentColor = System.Drawing.Color.White;
         this._clearSearchButton.Name = "_clearSearchButton";
         this._clearSearchButton.Size = new System.Drawing.Size(23, 22);
         this._clearSearchButton.ToolTipText = "Clear Search";
         this._clearSearchButton.Click += new System.EventHandler(this._clearSearchButton_Click);
         // 
         // _progressBar
         // 
         this._progressBar.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._progressBar.BarColor = System.Drawing.Color.FromArgb(( (int) ( ( (byte) ( 64 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 64 ) ) ) ));
         this._progressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this._progressBar.Location = new System.Drawing.Point(0, 0);
         this._progressBar.Maximum = 100;
         this._progressBar.Minimum = 0;
         this._progressBar.Name = "_progressBar";
         this._progressBar.Size = new System.Drawing.Size(632, 20);
         this._progressBar.TabIndex = 3;
         this._progressBar.TextContrastColor = System.Drawing.Color.Black;
         this._progressBar.ToolTipText = "Shows test progress.";
         this._progressBar.Value = 0;
         // 
         // CsUnitControl
         // 
         this.Controls.Add(this.FindPanel);
         this.Controls.Add(this._progressBar);
         this.Controls.Add(this._tabControl);
         this.Name = "CsUnitControl";
         this.Size = new System.Drawing.Size(632, 384);
         this.SizeChanged += new System.EventHandler(this.CsUnitControl_SizeChanged);
         this.FindPanel.ResumeLayout(false);
         this.FindPanel.PerformLayout();
         this._findPanelToolStrip.ResumeLayout(false);
         this._findPanelToolStrip.PerformLayout();
         this.ResumeLayout(false);

      }
      #endregion

      private ProgressBar _progressBar;
      private System.Windows.Forms.TabControl _tabControl;
      private System.Windows.Forms.ToolStrip _findPanelToolStrip;
      private System.Windows.Forms.ToolStripButton _findButton;
      private System.Windows.Forms.ToolStripComboBox _findTermDropDown;
      private System.Windows.Forms.ToolStripButton _clearSearchButton;
      private System.Windows.Forms.Label _label1;
      private System.Windows.Forms.ListView _findResultsListView;
      private System.Windows.Forms.ColumnHeader _columnHeader1;
      private System.Windows.Forms.ColumnHeader _columnHeader2;
   }
}
