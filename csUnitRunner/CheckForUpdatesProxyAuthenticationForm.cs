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

using System.Net;

namespace csUnit.Runner {
   /// <summary>
   /// Dialog to ask the user for his http proxy credentials.
   /// </summary>
   public class CheckForUpdatesProxyAuthenticationForm : System.Windows.Forms.Form {
      public CheckForUpdatesProxyAuthenticationForm() {
         InitializeComponent();
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckForUpdatesProxyAuthenticationForm));
         this._lblDescription1 = new System.Windows.Forms.Label();
         this._lblDescription2 = new System.Windows.Forms.Label();
         this._tbLogin = new System.Windows.Forms.TextBox();
         this._tbPassword = new System.Windows.Forms.TextBox();
         this._lblLogin = new System.Windows.Forms.Label();
         this._lblPassword = new System.Windows.Forms.Label();
         this._btnCancel = new System.Windows.Forms.Button();
         this._btnOK = new System.Windows.Forms.Button();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).BeginInit();
         this.SuspendLayout();
         // 
         // _lblDescription1
         // 
         this._lblDescription1.AutoSize = true;
         this._lblDescription1.Location = new System.Drawing.Point(64, 16);
         this._lblDescription1.Name = "_lblDescription1";
         this._lblDescription1.Size = new System.Drawing.Size(247, 13);
         this._lblDescription1.TabIndex = 0;
         this._lblDescription1.Text = "Your Http Proxy Server requires authentication.";
         // 
         // _lblDescription2
         // 
         this._lblDescription2.AutoSize = true;
         this._lblDescription2.Location = new System.Drawing.Point(64, 32);
         this._lblDescription2.Name = "_lblDescription2";
         this._lblDescription2.Size = new System.Drawing.Size(183, 13);
         this._lblDescription2.TabIndex = 1;
         this._lblDescription2.Text = "Please enter your credentials here:";
         // 
         // _tbLogin
         // 
         this._tbLogin.Location = new System.Drawing.Point(128, 56);
         this._tbLogin.Name = "_tbLogin";
         this._tbLogin.Size = new System.Drawing.Size(232, 22);
         this._tbLogin.TabIndex = 0;
         // 
         // _tbPassword
         // 
         this._tbPassword.Location = new System.Drawing.Point(128, 80);
         this._tbPassword.Name = "_tbPassword";
         this._tbPassword.PasswordChar = '*';
         this._tbPassword.Size = new System.Drawing.Size(232, 22);
         this._tbPassword.TabIndex = 1;
         // 
         // _lblLogin
         // 
         this._lblLogin.AutoSize = true;
         this._lblLogin.Location = new System.Drawing.Point(64, 56);
         this._lblLogin.Name = "_lblLogin";
         this._lblLogin.Size = new System.Drawing.Size(36, 13);
         this._lblLogin.TabIndex = 4;
         this._lblLogin.Text = "Login";
         // 
         // _lblPassword
         // 
         this._lblPassword.AutoSize = true;
         this._lblPassword.Location = new System.Drawing.Point(64, 80);
         this._lblPassword.Name = "_lblPassword";
         this._lblPassword.Size = new System.Drawing.Size(56, 13);
         this._lblPassword.TabIndex = 5;
         this._lblPassword.Text = "Password";
         // 
         // _btnCancel
         // 
         this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this._btnCancel.Location = new System.Drawing.Point(288, 112);
         this._btnCancel.Name = "_btnCancel";
         this._btnCancel.Size = new System.Drawing.Size(72, 24);
         this._btnCancel.TabIndex = 3;
         this._btnCancel.Text = "&Cancel";
         this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
         // 
         // _btnOK
         // 
         this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this._btnOK.Location = new System.Drawing.Point(208, 112);
         this._btnOK.Name = "_btnOK";
         this._btnOK.Size = new System.Drawing.Size(72, 24);
         this._btnOK.TabIndex = 2;
         this._btnOK.Text = "&OK";
         this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
         // 
         // pictureBox1
         // 
         this.pictureBox1.Image = ( (System.Drawing.Image) ( resources.GetObject("pictureBox1.Image") ) );
         this.pictureBox1.Location = new System.Drawing.Point(16, 16);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(32, 32);
         this.pictureBox1.TabIndex = 6;
         this.pictureBox1.TabStop = false;
         // 
         // CheckForUpdatesProxyAuthenticationForm
         // 
         this.AcceptButton = this._btnOK;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
         this.ClientSize = new System.Drawing.Size(378, 149);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(this._btnOK);
         this.Controls.Add(this._btnCancel);
         this.Controls.Add(this._lblPassword);
         this.Controls.Add(this._lblLogin);
         this.Controls.Add(this._tbPassword);
         this.Controls.Add(this._tbLogin);
         this.Controls.Add(this._lblDescription2);
         this.Controls.Add(this._lblDescription1);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ( (System.Drawing.Icon) ( resources.GetObject("$this.Icon") ) );
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "CheckForUpdatesProxyAuthenticationForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "csUnitRunner";
         ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private void _btnOK_Click(object sender, System.EventArgs e) {
         // GlobalProxySelection has been deprecated in .NET 2.0. Using
         // WebRequest.DefaultWebProxy instead. [13Mar2005, ml]
         // System.Net.GlobalProxySelection.Select.Credentials =
         WebRequest.DefaultWebProxy.Credentials =
            new NetworkCredential(_tbLogin.Text, _tbPassword.Text);
         Close();
      }

      private void _btnCancel_Click(object sender, System.EventArgs e) {
         Close();
      }

      private System.Windows.Forms.Label _lblDescription1;
      private System.Windows.Forms.Label _lblDescription2;
      private System.Windows.Forms.TextBox _tbLogin;
      private System.Windows.Forms.TextBox _tbPassword;
      private System.Windows.Forms.Label _lblLogin;
      private System.Windows.Forms.Label _lblPassword;
      private System.Windows.Forms.Button _btnCancel;
      private System.Windows.Forms.Button _btnOK;
      private System.Windows.Forms.PictureBox pictureBox1;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private readonly System.ComponentModel.Container components = null;
   }
}
