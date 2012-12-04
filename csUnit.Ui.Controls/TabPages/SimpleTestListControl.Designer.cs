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
   partial class SimpleTestListControl {
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
         this._testListView = new System.Windows.Forms.DataGridView();
         this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this._testMethodInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
         ( (System.ComponentModel.ISupportInitialize) ( this._testListView ) ).BeginInit();
         ( (System.ComponentModel.ISupportInitialize) ( this._testMethodInfoBindingSource ) ).BeginInit();
         this.SuspendLayout();
         // 
         // _testListView
         // 
         this._testListView.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._testListView.AutoGenerateColumns = false;
         this._testListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this._testListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fullNameDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
         this._testListView.DataSource = this._testMethodInfoBindingSource;
         this._testListView.Location = new System.Drawing.Point(3, 3);
         this._testListView.Name = "_testListView";
         this._testListView.Size = new System.Drawing.Size(353, 348);
         this._testListView.TabIndex = 0;
         // 
         // fullNameDataGridViewTextBoxColumn
         // 
         this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
         this.fullNameDataGridViewTextBoxColumn.HeaderText = "FullName";
         this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
         this.fullNameDataGridViewTextBoxColumn.ReadOnly = true;
         // 
         // nameDataGridViewTextBoxColumn
         // 
         this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
         this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
         this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
         this.nameDataGridViewTextBoxColumn.ReadOnly = true;
         // 
         // _testMethodInfoBindingSource
         // 
         this._testMethodInfoBindingSource.DataSource = typeof(csUnit.Core.TestMethodInfo);
         // 
         // SimpleTestListControl
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._testListView);
         this.Name = "SimpleTestListControl";
         this.Size = new System.Drawing.Size(359, 351);
         ( (System.ComponentModel.ISupportInitialize) ( this._testListView ) ).EndInit();
         ( (System.ComponentModel.ISupportInitialize) ( this._testMethodInfoBindingSource ) ).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView _testListView;
      private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
      private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
      private System.Windows.Forms.BindingSource _testMethodInfoBindingSource;
   }
}
