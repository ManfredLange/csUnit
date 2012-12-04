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

using System;
using System.Reflection;
using System.Windows.Forms;

namespace csUnit.Runner {
   /// <summary>
   /// Summary description for AboutForm.
   /// </summary>
   public class AboutForm : Form {
      public AboutForm() {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         //
         // TODO: Add any constructor code after InitializeComponent call
         //
         String labelText = "www.csunit.org";
         _lnkLblCsunitSite.Links.Add(0, labelText.Length, labelText);

         String assemblyVersion = RetrieveAssemblyVersion();
         _tbAbout.Text = String.Format(_tbAbout.Text, assemblyVersion);

         _btnOK.Focus();
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
         this._tbAbout = new System.Windows.Forms.TextBox();
         this._lnkLblCsunitSite = new System.Windows.Forms.LinkLabel();
         this._btnOK = new System.Windows.Forms.Button();
         this.textBox3 = new System.Windows.Forms.TextBox();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.textBox5 = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // _tbAbout
         // 
         this._tbAbout.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this._tbAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this._tbAbout.Location = new System.Drawing.Point(8, 8);
         this._tbAbout.Multiline = true;
         this._tbAbout.Name = "_tbAbout";
         this._tbAbout.ReadOnly = true;
         this._tbAbout.Size = new System.Drawing.Size(414, 16);
         this._tbAbout.TabIndex = 1;
         this._tbAbout.TabStop = false;
         this._tbAbout.Text = "csUnit version {0}";
         this._tbAbout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // _lnkLblCsunitSite
         // 
         this._lnkLblCsunitSite.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this._lnkLblCsunitSite.Location = new System.Drawing.Point(5, 366);
         this._lnkLblCsunitSite.Name = "_lnkLblCsunitSite";
         this._lnkLblCsunitSite.Size = new System.Drawing.Size(342, 23);
         this._lnkLblCsunitSite.TabIndex = 2;
         this._lnkLblCsunitSite.TabStop = true;
         this._lnkLblCsunitSite.Tag = "www.csunit.org";
         this._lnkLblCsunitSite.Text = "www.csunit.org";
         this._lnkLblCsunitSite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this._lnkLblCsunitSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkLblCsunitSite_LinkClicked);
         // 
         // _btnOK
         // 
         this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this._btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this._btnOK.Location = new System.Drawing.Point(351, 334);
         this._btnOK.Name = "_btnOK";
         this._btnOK.Size = new System.Drawing.Size(75, 52);
         this._btnOK.TabIndex = 3;
         this._btnOK.Text = "OK";
         // 
         // textBox3
         // 
         this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.textBox3.Location = new System.Drawing.Point(10, 334);
         this.textBox3.Multiline = true;
         this.textBox3.Name = "textBox3";
         this.textBox3.ReadOnly = true;
         this.textBox3.Size = new System.Drawing.Size(337, 29);
         this.textBox3.TabIndex = 6;
         this.textBox3.TabStop = false;
         this.textBox3.Text = "For more information about csUnit including online documentation and tutorials pl" +
             "ease visit:";
         // 
         // textBox1
         // 
         this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox1.Location = new System.Drawing.Point(12, 30);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.ReadOnly = true;
         this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.textBox1.Size = new System.Drawing.Size(414, 272);
         this.textBox1.TabIndex = 8;
         this.textBox1.Text = resources.GetString("textBox1.Text");
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(9, 308);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(76, 13);
         this.label1.TabIndex = 10;
         this.label1.Text = "Contributors:";
         // 
         // textBox5
         // 
         this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
         this.textBox5.Location = new System.Drawing.Point(96, 308);
         this.textBox5.Multiline = true;
         this.textBox5.Name = "textBox5";
         this.textBox5.ReadOnly = true;
         this.textBox5.Size = new System.Drawing.Size(330, 20);
         this.textBox5.TabIndex = 11;
         this.textBox5.Text = "Richard Lopes, Philipp Lange, Steve Schwenker. Thank you!";
         // 
         // AboutForm
         // 
         this.AcceptButton = this._btnOK;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
         this.ClientSize = new System.Drawing.Size(438, 398);
         this.Controls.Add(this.textBox5);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.textBox1);
         this.Controls.Add(this.textBox3);
         this.Controls.Add(this._tbAbout);
         this.Controls.Add(this._btnOK);
         this.Controls.Add(this._lnkLblCsunitSite);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimumSize = new System.Drawing.Size(320, 297);
         this.Name = "AboutForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "About csUnitRunner";
         this.ResumeLayout(false);
         this.PerformLayout();

      }
      #endregion

      private void _lnkLblCsunitSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         Close();
         _lnkLblCsunitSite.Links[_lnkLblCsunitSite.Links.IndexOf(e.Link)].Visited = true;

         try {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
         }
         catch(Exception exception) {
            Console.Write(exception.Message);
         }
      }

      private static String RetrieveAssemblyVersion() {
         Assembly ass = Assembly.GetExecutingAssembly();
         return ass.GetName().Version.ToString(3);
      }

      private TextBox _tbAbout;
      private LinkLabel _lnkLblCsunitSite;
      private Button _btnOK;
      private TextBox textBox3;
      private TextBox textBox1;
      private Label label1;
      private TextBox textBox5;

      /// <summary>
      /// Required designer variable.
      /// </summary>
      private readonly System.ComponentModel.Container components = null;
   }
}
