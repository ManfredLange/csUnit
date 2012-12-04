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
   partial class ConsoleOutputControl {
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleOutputControl));
         this._textOutput = new System.Windows.Forms.RichTextBox();
         this._copyToClipboardButton = new System.Windows.Forms.Button();
         this._toolTips = new System.Windows.Forms.ToolTip(this.components);
         this.SuspendLayout();
         // 
         // _textOutput
         // 
         this._textOutput.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._textOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this._textOutput.HideSelection = false;
         this._textOutput.Location = new System.Drawing.Point(0, 32);
         this._textOutput.Name = "_textOutput";
         this._textOutput.ReadOnly = true;
         this._textOutput.Size = new System.Drawing.Size(407, 284);
         this._textOutput.TabIndex = 2;
         this._textOutput.Text = resources.GetString("_textOutput.Text");
         this._toolTips.SetToolTip(this._textOutput, "This area will contain all output written\r\nto Console.Out and Console.Error.");
         this._textOutput.WordWrap = false;
         this._textOutput.TextChanged += new System.EventHandler(this._textOutput_TextChanged);
         // 
         // _copyToClipboardButton
         // 
         this._copyToClipboardButton.Enabled = false;
         this._copyToClipboardButton.Location = new System.Drawing.Point(3, 3);
         this._copyToClipboardButton.Name = "_copyToClipboardButton";
         this._copyToClipboardButton.Size = new System.Drawing.Size(45, 23);
         this._copyToClipboardButton.TabIndex = 3;
         this._copyToClipboardButton.Text = "Copy";
         this._toolTips.SetToolTip(this._copyToClipboardButton, "Copies the entire content of the \r\nOutput tab to the clipboard.");
         this._copyToClipboardButton.UseVisualStyleBackColor = true;
         this._copyToClipboardButton.Click += new System.EventHandler(this._copyToClipboardButton_Click);
         // 
         // ConsoleOutputControl
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.Controls.Add(this._copyToClipboardButton);
         this.Controls.Add(this._textOutput);
         this.Name = "ConsoleOutputControl";
         this.Size = new System.Drawing.Size(407, 319);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.RichTextBox _textOutput;
      private System.Windows.Forms.Button _copyToClipboardButton;
      private System.Windows.Forms.ToolTip _toolTips;
   }
}
