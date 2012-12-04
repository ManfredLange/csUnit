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
using System.Drawing;
using System.Windows.Forms;

using csUnit.Core;

namespace csUnit.Runner {
   /// <summary>
   /// Summary description for OptionsForm.
   /// </summary>
   internal class OptionsForm : Form {
      public OptionsForm() {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         tabControl1.TabPages.Remove(tabPage4);
         // TODO: remove when settings dialog is implemented generically.
      }

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose( bool disposing ) {
         if( disposing ) {
            if(components != null) {
               components.Dispose();
            }
         }
         base.Dispose( disposing );
      }

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
         this._btnOK = new System.Windows.Forms.Button();
         this._btnCancel = new System.Windows.Forms.Button();
         this._cbAskForSafeOnModifiedUntitled = new System.Windows.Forms.CheckBox();
         this._cbStartupLoadItem = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this._cbExpandCommentNodes = new System.Windows.Forms.CheckBox();
         this._cbShowLineNumbers = new System.Windows.Forms.CheckBox();
         this._cbAutoExpand = new System.Windows.Forms.CheckBox();
         this._successColorDisplay = new System.Windows.Forms.Panel();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this._successColorButton = new System.Windows.Forms.Button();
         this._failureColorButton = new System.Windows.Forms.Button();
         this._failureColorDisplay = new System.Windows.Forms.Panel();
         this._defaultColorsButton = new System.Windows.Forms.Button();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.groupBox4 = new System.Windows.Forms.GroupBox();
         this.groupBox5 = new System.Windows.Forms.GroupBox();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.tabPage3 = new System.Windows.Forms.TabPage();
         this.panel1 = new System.Windows.Forms.Panel();
         this.tabPage4 = new System.Windows.Forms.TabPage();
         this._settingsList = new System.Windows.Forms.ListBox();
         this._cbExpandExecutedTestNodes = new System.Windows.Forms.CheckBox();
         this.groupBox3.SuspendLayout();
         this.groupBox4.SuspendLayout();
         this.groupBox5.SuspendLayout();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.tabPage2.SuspendLayout();
         this.tabPage3.SuspendLayout();
         this.panel1.SuspendLayout();
         this.tabPage4.SuspendLayout();
         this.SuspendLayout();
         // 
         // _btnOK
         // 
         this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this._btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this._btnOK.Location = new System.Drawing.Point(206, 234);
         this._btnOK.Name = "_btnOK";
         this._btnOK.Size = new System.Drawing.Size(75, 23);
         this._btnOK.TabIndex = 2;
         this._btnOK.Text = "OK";
         this._btnOK.Click += new System.EventHandler(this.OnOK);
         // 
         // _btnCancel
         // 
         this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this._btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this._btnCancel.Location = new System.Drawing.Point(287, 234);
         this._btnCancel.Name = "_btnCancel";
         this._btnCancel.Size = new System.Drawing.Size(75, 23);
         this._btnCancel.TabIndex = 3;
         this._btnCancel.Text = "Cancel";
         this._btnCancel.Click += new System.EventHandler(this.OnCancel);
         // 
         // _cbAskForSafeOnModifiedUntitled
         // 
         this._cbAskForSafeOnModifiedUntitled.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbAskForSafeOnModifiedUntitled.Location = new System.Drawing.Point(6, 19);
         this._cbAskForSafeOnModifiedUntitled.Name = "_cbAskForSafeOnModifiedUntitled";
         this._cbAskForSafeOnModifiedUntitled.Size = new System.Drawing.Size(319, 32);
         this._cbAskForSafeOnModifiedUntitled.TabIndex = 1;
         this._cbAskForSafeOnModifiedUntitled.Text = "Upon closing csUnitRunner show a Save File Dialog when untitled recipe is modifie" +
             "d.";
         this._cbAskForSafeOnModifiedUntitled.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         // 
         // _cbStartupLoadItem
         // 
         this._cbStartupLoadItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this._cbStartupLoadItem.Items.AddRange(new object[] {
            "nothing.",
            "most recently used Recipe.",
            "most recently used TestAssembly."});
         this._cbStartupLoadItem.Location = new System.Drawing.Point(97, 19);
         this._cbStartupLoadItem.Name = "_cbStartupLoadItem";
         this._cbStartupLoadItem.Size = new System.Drawing.Size(228, 21);
         this._cbStartupLoadItem.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(9, 24);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(82, 16);
         this.label1.TabIndex = 4;
         this.label1.Text = "Upon start load";
         // 
         // _cbExpandCommentNodes
         // 
         this._cbExpandCommentNodes.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbExpandCommentNodes.Location = new System.Drawing.Point(6, 6);
         this._cbExpandCommentNodes.Name = "_cbExpandCommentNodes";
         this._cbExpandCommentNodes.Size = new System.Drawing.Size(331, 18);
         this._cbExpandCommentNodes.TabIndex = 5;
         this._cbExpandCommentNodes.Text = "Expand nodes skipped during test run.";
         this._cbExpandCommentNodes.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         // 
         // _cbShowLineNumbers
         // 
         this._cbShowLineNumbers.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbShowLineNumbers.Location = new System.Drawing.Point(6, 19);
         this._cbShowLineNumbers.Name = "_cbShowLineNumbers";
         this._cbShowLineNumbers.Size = new System.Drawing.Size(319, 18);
         this._cbShowLineNumbers.TabIndex = 6;
         this._cbShowLineNumbers.Text = "Show line numbers in Output.";
         this._cbShowLineNumbers.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         // 
         // _cbAutoExpand
         // 
         this._cbAutoExpand.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbAutoExpand.Location = new System.Drawing.Point(6, 54);
         this._cbAutoExpand.Name = "_cbAutoExpand";
         this._cbAutoExpand.Size = new System.Drawing.Size(331, 18);
         this._cbAutoExpand.TabIndex = 7;
         this._cbAutoExpand.Text = "Expand test hierarchy tree upon load of recipe or assembly.";
         this._cbAutoExpand.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbAutoExpand.UseVisualStyleBackColor = true;
         // 
         // _successColorDisplay
         // 
         this._successColorDisplay.BackColor = System.Drawing.Color.Lime;
         this._successColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this._successColorDisplay.Location = new System.Drawing.Point(98, 8);
         this._successColorDisplay.Name = "_successColorDisplay";
         this._successColorDisplay.Size = new System.Drawing.Size(15, 15);
         this._successColorDisplay.TabIndex = 8;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 10);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(46, 13);
         this.label2.TabIndex = 9;
         this.label2.Text = "Success";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(7, 37);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(71, 13);
         this.label3.TabIndex = 10;
         this.label3.Text = "Failure/Error";
         // 
         // _successColorButton
         // 
         this._successColorButton.Location = new System.Drawing.Point(119, 5);
         this._successColorButton.Name = "_successColorButton";
         this._successColorButton.Size = new System.Drawing.Size(24, 23);
         this._successColorButton.TabIndex = 11;
         this._successColorButton.Text = "...";
         this._successColorButton.UseVisualStyleBackColor = true;
         this._successColorButton.Click += new System.EventHandler(this._successColorButton_Click);
         // 
         // _failureColorButton
         // 
         this._failureColorButton.Location = new System.Drawing.Point(119, 32);
         this._failureColorButton.Name = "_failureColorButton";
         this._failureColorButton.Size = new System.Drawing.Size(24, 23);
         this._failureColorButton.TabIndex = 12;
         this._failureColorButton.Text = "...";
         this._failureColorButton.UseVisualStyleBackColor = true;
         this._failureColorButton.Click += new System.EventHandler(this._failureColorButton_Click);
         // 
         // _failureColorDisplay
         // 
         this._failureColorDisplay.BackColor = System.Drawing.Color.Red;
         this._failureColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this._failureColorDisplay.Location = new System.Drawing.Point(98, 35);
         this._failureColorDisplay.Name = "_failureColorDisplay";
         this._failureColorDisplay.Size = new System.Drawing.Size(15, 15);
         this._failureColorDisplay.TabIndex = 9;
         // 
         // _defaultColorsButton
         // 
         this._defaultColorsButton.Location = new System.Drawing.Point(197, 11);
         this._defaultColorsButton.Name = "_defaultColorsButton";
         this._defaultColorsButton.Size = new System.Drawing.Size(71, 39);
         this._defaultColorsButton.TabIndex = 13;
         this._defaultColorsButton.Text = "Default Colors";
         this._defaultColorsButton.UseVisualStyleBackColor = true;
         this._defaultColorsButton.Click += new System.EventHandler(this._defaultColorsButton_Click);
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this._cbStartupLoadItem);
         this.groupBox3.Controls.Add(this.label1);
         this.groupBox3.Location = new System.Drawing.Point(6, 6);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(331, 49);
         this.groupBox3.TabIndex = 16;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Start  Behavior";
         // 
         // groupBox4
         // 
         this.groupBox4.Controls.Add(this._cbAskForSafeOnModifiedUntitled);
         this.groupBox4.Location = new System.Drawing.Point(6, 61);
         this.groupBox4.Name = "groupBox4";
         this.groupBox4.Size = new System.Drawing.Size(331, 63);
         this.groupBox4.TabIndex = 17;
         this.groupBox4.TabStop = false;
         this.groupBox4.Text = "Shut Down Behavior";
         // 
         // groupBox5
         // 
         this.groupBox5.Controls.Add(this._cbShowLineNumbers);
         this.groupBox5.Location = new System.Drawing.Point(6, 130);
         this.groupBox5.Name = "groupBox5";
         this.groupBox5.Size = new System.Drawing.Size(331, 45);
         this.groupBox5.TabIndex = 18;
         this.groupBox5.TabStop = false;
         this.groupBox5.Text = "Output Tab Behavior";
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Controls.Add(this.tabPage2);
         this.tabControl1.Controls.Add(this.tabPage3);
         this.tabControl1.Controls.Add(this.tabPage4);
         this.tabControl1.Location = new System.Drawing.Point(12, 12);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(351, 211);
         this.tabControl1.TabIndex = 19;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.groupBox3);
         this.tabPage1.Controls.Add(this.groupBox5);
         this.tabPage1.Controls.Add(this.groupBox4);
         this.tabPage1.Location = new System.Drawing.Point(4, 22);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(343, 185);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "General";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // tabPage2
         // 
         this.tabPage2.Controls.Add(this._cbExpandExecutedTestNodes);
         this.tabPage2.Controls.Add(this._cbAutoExpand);
         this.tabPage2.Controls.Add(this._cbExpandCommentNodes);
         this.tabPage2.Location = new System.Drawing.Point(4, 22);
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage2.Size = new System.Drawing.Size(343, 185);
         this.tabPage2.TabIndex = 1;
         this.tabPage2.Text = "Test Hierarchy Tree";
         this.tabPage2.UseVisualStyleBackColor = true;
         // 
         // tabPage3
         // 
         this.tabPage3.Controls.Add(this.panel1);
         this.tabPage3.Location = new System.Drawing.Point(4, 22);
         this.tabPage3.Name = "tabPage3";
         this.tabPage3.Size = new System.Drawing.Size(343, 185);
         this.tabPage3.TabIndex = 2;
         this.tabPage3.Text = "Colors";
         this.tabPage3.UseVisualStyleBackColor = true;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.label2);
         this.panel1.Controls.Add(this._defaultColorsButton);
         this.panel1.Controls.Add(this.label3);
         this.panel1.Controls.Add(this._failureColorButton);
         this.panel1.Controls.Add(this._successColorButton);
         this.panel1.Controls.Add(this._successColorDisplay);
         this.panel1.Controls.Add(this._failureColorDisplay);
         this.panel1.Location = new System.Drawing.Point(3, 3);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(276, 61);
         this.panel1.TabIndex = 20;
         // 
         // tabPage4
         // 
         this.tabPage4.Controls.Add(this._settingsList);
         this.tabPage4.Location = new System.Drawing.Point(4, 22);
         this.tabPage4.Name = "tabPage4";
         this.tabPage4.Size = new System.Drawing.Size(343, 185);
         this.tabPage4.TabIndex = 3;
         this.tabPage4.Text = "Settings";
         this.tabPage4.UseVisualStyleBackColor = true;
         // 
         // _settingsList
         // 
         this._settingsList.FormattingEnabled = true;
         this._settingsList.Location = new System.Drawing.Point(3, 3);
         this._settingsList.Name = "_settingsList";
         this._settingsList.Size = new System.Drawing.Size(122, 173);
         this._settingsList.TabIndex = 0;
         // 
         // _cbExpandExecutedTestNodes
         // 
         this._cbExpandExecutedTestNodes.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbExpandExecutedTestNodes.Location = new System.Drawing.Point(6, 30);
         this._cbExpandExecutedTestNodes.Name = "_cbExpandExecutedTestNodes";
         this._cbExpandExecutedTestNodes.Size = new System.Drawing.Size(331, 18);
         this._cbExpandExecutedTestNodes.TabIndex = 8;
         this._cbExpandExecutedTestNodes.Text = "Expand nodes of executed tests during test run.";
         this._cbExpandExecutedTestNodes.TextAlign = System.Drawing.ContentAlignment.TopLeft;
         this._cbExpandExecutedTestNodes.UseVisualStyleBackColor = true;
         // 
         // OptionsForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
         this.ClientSize = new System.Drawing.Size(374, 269);
         this.Controls.Add(this.tabControl1);
         this.Controls.Add(this._btnCancel);
         this.Controls.Add(this._btnOK);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "OptionsForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Options";
         this.Load += new System.EventHandler(this.OptionsForm_Load);
         this.groupBox3.ResumeLayout(false);
         this.groupBox4.ResumeLayout(false);
         this.groupBox5.ResumeLayout(false);
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.tabPage2.ResumeLayout(false);
         this.tabPage3.ResumeLayout(false);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.tabPage4.ResumeLayout(false);
         this.ResumeLayout(false);

      }
      #endregion

      private void OnCancel(object sender, EventArgs e) {
         Close();
      }

      private void OnOK(object sender, EventArgs e) {
         Core.SettingsManager settingsMgr = new Core.SettingsManager();

         settingsMgr["SuccessColor"] = _successColorDisplay.BackColor;
         settingsMgr["FailureColor"] = _failureColorDisplay.BackColor;

         settingsMgr["AutoExpandTestHierarchy"] = _cbAutoExpand.Checked;
         settingsMgr["ExpandCommentNodes"] = _cbExpandCommentNodes.Checked;
         settingsMgr["ShowLineNumbersInOutput"] = _cbShowLineNumbers.Checked;
         settingsMgr["ExpandExecutedTestNodes"] = _cbExpandExecutedTestNodes.Checked;
         
         ConfigCurrentUser config = new ConfigCurrentUser();
         config.AskForSafeOnModifiedUntitled  = _cbAskForSafeOnModifiedUntitled.Checked;
         switch(_cbStartupLoadItem.SelectedIndex) {
            case 0:
               config.StartupLoadItem = "Nothing";
               break;
            case 1:
               config.StartupLoadItem = "Recipe";
               break;
            case 2:
               config.StartupLoadItem = "Assembly";
               break;
         }

         Close();
      }

      private void OptionsForm_Load(object sender, EventArgs e) {
         Core.SettingsManager settingsMgr = new Core.SettingsManager();

         _settingsList.Items.Clear();
         foreach(string setting in SettingsManager.Settings) {
            _settingsList.Items.Add(setting);
         }

         _successColorDisplay.BackColor = (Color)settingsMgr["SuccessColor"];
         _failureColorDisplay.BackColor = (Color)settingsMgr["FailureColor"];
         _cbAutoExpand.Checked = (bool)settingsMgr["AutoExpandTestHierarchy"];
         _cbExpandCommentNodes.Checked = (bool)settingsMgr["ExpandCommentNodes"];
         _cbShowLineNumbers.Checked = (bool)settingsMgr["ShowLineNumbersInOutput"];
         _cbExpandExecutedTestNodes.Checked = (bool) settingsMgr["ExpandExecutedTestNodes"];

         ConfigCurrentUser config = new ConfigCurrentUser();
         
         _cbAskForSafeOnModifiedUntitled.Checked = config.AskForSafeOnModifiedUntitled;

         switch(config.StartupLoadItem) {
            case "Nothing":
            default:
               _cbStartupLoadItem.SelectedItem = "nothing.";
               break;
            case "Recipe":
               _cbStartupLoadItem.SelectedItem = "most recently used Recipe.";
               break;
            case "Assembly":
               _cbStartupLoadItem.SelectedItem = "most recently used TestAssembly.";
               break;
         }
      }

      private void _defaultColorsButton_Click(object sender, EventArgs e) {
         _successColorDisplay.BackColor = csUnit.Ui.Controls.Constants.ColorPassed;
         _failureColorDisplay.BackColor = csUnit.Ui.Controls.Constants.ColorError;
      }

      private void _successColorButton_Click(object sender, EventArgs e) {
         var dlg = new ColorDialog();
         if(DialogResult.OK == dlg.ShowDialog(this)) {
            _successColorDisplay.BackColor = dlg.Color;
         }
      }

      private void _failureColorButton_Click(object sender, EventArgs e) {
         var dlg = new ColorDialog();
         if(DialogResult.OK == dlg.ShowDialog(this)) {
            _failureColorDisplay.BackColor = dlg.Color;
         }
      }

      private Button _btnOK;
      private Button _btnCancel;
      private CheckBox _cbAskForSafeOnModifiedUntitled;
      private Label label1;
      private ComboBox _cbStartupLoadItem;
      private CheckBox _cbExpandCommentNodes;
      private CheckBox _cbShowLineNumbers;
      private CheckBox _cbAutoExpand;
      private Panel _successColorDisplay;
      private Label label2;
      private Label label3;
      private Button _successColorButton;
      private Button _failureColorButton;
      private Panel _failureColorDisplay;
      private Button _defaultColorsButton;
      private GroupBox groupBox3;
      private GroupBox groupBox4;
      private GroupBox groupBox5;
      private TabControl tabControl1;
      private TabPage tabPage1;
      private TabPage tabPage2;
      private TabPage tabPage3;
      private Panel panel1;
      private TabPage tabPage4;
      private ListBox _settingsList;
      private CheckBox _cbExpandExecutedTestNodes;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private readonly System.ComponentModel.Container components = null;
   }
}
