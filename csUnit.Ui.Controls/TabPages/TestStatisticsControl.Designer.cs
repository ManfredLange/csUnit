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
   partial class TestStatisticsControl {
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
         this._lvStatistics = new System.Windows.Forms.ListView();
         this._ClassStatColumn = new System.Windows.Forms.ColumnHeader();
         this._MethodStatColumn = new System.Windows.Forms.ColumnHeader();
         this._DurationStatName = new System.Windows.Forms.ColumnHeader();
         this._PercStatColumn = new System.Windows.Forms.ColumnHeader();
         this._NumAssertsColumn = new System.Windows.Forms.ColumnHeader();
         this._toolTips = new System.Windows.Forms.ToolTip(this.components);
         this._averageTimeLabel = new System.Windows.Forms.Label();
         this._overheadTimeLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // _lvStatistics
         // 
         this._lvStatistics.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._lvStatistics.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._ClassStatColumn,
            this._MethodStatColumn,
            this._DurationStatName,
            this._PercStatColumn,
            this._NumAssertsColumn});
         this._lvStatistics.FullRowSelect = true;
         this._lvStatistics.HideSelection = false;
         this._lvStatistics.Location = new System.Drawing.Point(0, 0);
         this._lvStatistics.Name = "_lvStatistics";
         this._lvStatistics.Size = new System.Drawing.Size(382, 308);
         this._lvStatistics.TabIndex = 1;
         this._toolTips.SetToolTip(this._lvStatistics, "Absolute and proportional execution time for each test.");
         this._lvStatistics.UseCompatibleStateImageBehavior = false;
         this._lvStatistics.View = System.Windows.Forms.View.Details;
         this._lvStatistics.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this._lvStatistics_ColumnClick);
         // 
         // _ClassStatColumn
         // 
         this._ClassStatColumn.Text = "Class Name";
         this._ClassStatColumn.Width = 136;
         // 
         // _MethodStatColumn
         // 
         this._MethodStatColumn.Text = "Method";
         this._MethodStatColumn.Width = 101;
         // 
         // _DurationStatName
         // 
         this._DurationStatName.Text = "ms";
         this._DurationStatName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // _PercStatColumn
         // 
         this._PercStatColumn.Text = "%";
         this._PercStatColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this._PercStatColumn.Width = 40;
         // 
         // _NumAssertsColumn
         // 
         this._NumAssertsColumn.Text = "Assertions";
         this._NumAssertsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // _averageTimeLabel
         // 
         this._averageTimeLabel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
         this._averageTimeLabel.AutoSize = true;
         this._averageTimeLabel.Location = new System.Drawing.Point(3, 311);
         this._averageTimeLabel.Name = "_averageTimeLabel";
         this._averageTimeLabel.Size = new System.Drawing.Size(179, 13);
         this._averageTimeLabel.TabIndex = 2;
         this._averageTimeLabel.Text = "{0} ms per test. {1} tests per second.";
         this._toolTips.SetToolTip(this._averageTimeLabel, "Average time per test without overhead.");
         // 
         // _overheadTimeLabel
         // 
         this._overheadTimeLabel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
         this._overheadTimeLabel.AutoSize = true;
         this._overheadTimeLabel.Location = new System.Drawing.Point(3, 324);
         this._overheadTimeLabel.Name = "_overheadTimeLabel";
         this._overheadTimeLabel.Size = new System.Drawing.Size(183, 13);
         this._overheadTimeLabel.TabIndex = 3;
         this._overheadTimeLabel.Text = "Estimated overhead of {0} ms ({1} %).";
         this._toolTips.SetToolTip(this._overheadTimeLabel, "Execution time not spent in tests.\r\nThis is an estimated only and is\r\ninfluenced " +
                 "by many different factors\r\nincluding processor load.");
         // 
         // TestStatisticsControl
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._overheadTimeLabel);
         this.Controls.Add(this._averageTimeLabel);
         this.Controls.Add(this._lvStatistics);
         this.Name = "TestStatisticsControl";
         this.Size = new System.Drawing.Size(382, 339);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ColumnHeader _ClassStatColumn;
      private System.Windows.Forms.ColumnHeader _MethodStatColumn;
      private System.Windows.Forms.ColumnHeader _DurationStatName;
      private System.Windows.Forms.ColumnHeader _PercStatColumn;
      private System.Windows.Forms.ColumnHeader _NumAssertsColumn;
      private System.Windows.Forms.ListView _lvStatistics;
      private System.Windows.Forms.ToolTip _toolTips;
      private System.Windows.Forms.Label _averageTimeLabel;
      private System.Windows.Forms.Label _overheadTimeLabel;
   }
}
