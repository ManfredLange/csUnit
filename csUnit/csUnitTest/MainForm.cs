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

namespace csUnit.Tests {
   /// <summary>
   /// Summary description for MainForm.
   /// </summary>
   internal class MainForm : System.Windows.Forms.Form {
      /// <summary>
      /// Button for closing the form.
      /// </summary>
      private System.Windows.Forms.Button _btnClose;
      /// <summary>
      /// The textbox containing the message.
      /// </summary>
      private System.Windows.Forms.TextBox _tbMessage;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private readonly System.ComponentModel.Container components = null;

      public MainForm() {
         // Required for Windows Form Designer support
         InitializeComponent();

         _btnClose.Select();
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
         this._tbMessage = new System.Windows.Forms.TextBox();
         this._btnClose = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // _tbMessage
         // 
         this._tbMessage.Location = new System.Drawing.Point(8, 8);
         this._tbMessage.Multiline = true;
         this._tbMessage.Name = "_tbMessage";
         this._tbMessage.ReadOnly = true;
         this._tbMessage.Size = new System.Drawing.Size(176, 48);
         this._tbMessage.TabIndex = 0;
         this._tbMessage.Text = "This assembly contains test cases. In order to run them, use csunitRunner (instal" +
            "led separately).";
         // 
         // _btnClose
         // 
         this._btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
         this._btnClose.Location = new System.Drawing.Point(8, 64);
         this._btnClose.Name = "_btnClose";
         this._btnClose.Size = new System.Drawing.Size(176, 23);
         this._btnClose.TabIndex = 1;
         this._btnClose.Text = "Close";
         this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(192, 93);
         this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                      this._btnClose,
                                                                      this._tbMessage});
         this.Name = "MainForm";
         this.Text = "MainForm";
         this.ResumeLayout(false);

      }
      #endregion

      private void _btnClose_Click(object sender, System.EventArgs e) {
         Close();
      }
   }
}
