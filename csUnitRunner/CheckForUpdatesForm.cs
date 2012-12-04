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

using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace csUnit.Runner {
   /// <summary>
   /// Summary description for CheckForUpdatesForm.
   /// </summary>
   public class CheckForUpdatesForm : Form {
      public CheckForUpdatesForm() {
         //
         // Required for Windows Form Designer support
         //
         InitializeComponent();

         //
         // TODO: Add any constructor code after InitializeComponent call
         //
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckForUpdatesForm));
         this._image = new System.Windows.Forms.PictureBox();
         this._message = new System.Windows.Forms.Label();
         this._btnCancel = new System.Windows.Forms.Button();
         this._timer = new System.Windows.Forms.Timer(this.components);
         this._imageList = new System.Windows.Forms.ImageList(this.components);
         this._lnkDownload = new System.Windows.Forms.LinkLabel();
         ( (System.ComponentModel.ISupportInitialize) ( this._image ) ).BeginInit();
         this.SuspendLayout();
         // 
         // _image
         // 
         this._image.Location = new System.Drawing.Point(16, 16);
         this._image.Name = "_image";
         this._image.Size = new System.Drawing.Size(32, 32);
         this._image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this._image.TabIndex = 0;
         this._image.TabStop = false;
         // 
         // _message
         // 
         this._message.Location = new System.Drawing.Point(56, 16);
         this._message.Name = "_message";
         this._message.Size = new System.Drawing.Size(320, 128);
         this._message.TabIndex = 1;
         this._message.Text = "Please wait while csUnitRunner checks for newer versions...";
         // 
         // _btnCancel
         // 
         this._btnCancel.Location = new System.Drawing.Point(296, 184);
         this._btnCancel.Name = "_btnCancel";
         this._btnCancel.Size = new System.Drawing.Size(75, 23);
         this._btnCancel.TabIndex = 2;
         this._btnCancel.Text = "Cancel";
         this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
         // 
         // _timer
         // 
         this._timer.Tick += new System.EventHandler(this._timer_Tick);
         // 
         // _imageList
         // 
         this._imageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject("_imageList.ImageStream") ) );
         this._imageList.TransparentColor = System.Drawing.Color.Transparent;
         this._imageList.Images.SetKeyName(0, "");
         this._imageList.Images.SetKeyName(1, "");
         // 
         // _lnkDownload
         // 
         this._lnkDownload.Location = new System.Drawing.Point(48, 152);
         this._lnkDownload.Name = "_lnkDownload";
         this._lnkDownload.Size = new System.Drawing.Size(328, 16);
         this._lnkDownload.TabIndex = 3;
         this._lnkDownload.TabStop = true;
         this._lnkDownload.Text = "Download Now...";
         this._lnkDownload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this._lnkDownload.Visible = false;
         this._lnkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkDownload_LinkClicked);
         // 
         // CheckForUpdatesForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
         this.ClientSize = new System.Drawing.Size(384, 221);
         this.Controls.Add(this._lnkDownload);
         this.Controls.Add(this._btnCancel);
         this.Controls.Add(this._message);
         this.Controls.Add(this._image);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ( (System.Drawing.Icon) ( resources.GetObject("$this.Icon") ) );
         this.MaximizeBox = false;
         this.Name = "CheckForUpdatesForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "csUnitRunner";
         this.Load += new System.EventHandler(this.CheckForUpdatesForm_Load);
         ( (System.ComponentModel.ISupportInitialize) ( this._image ) ).EndInit();
         this.ResumeLayout(false);

      }
      #endregion

      private void _btnCancel_Click(object sender, EventArgs e) {
         if( _workerThread.IsAlive ) {
            _timer.Enabled = false;
            _workerThread.Abort();
            _workerThread.Join(3000);
            _workerThread = null;
         }
         Close();
      }

      private void CheckForUpdatesForm_Load(object sender, EventArgs e) {
         _image.Image = _imageList.Images[0];
         _workerThread = new Thread(this.CheckerProc);
         _workerThread.Name = "Update checker thread";
         _workerThread.Start();
         _timer.Enabled = true;
      }

      private void CheckerProc() {
         try {
            string temp = string.Empty;
            Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            temp += "\n\nLocal version is: " + localVersion.ToString(2);
            temp += "\n\nRetrieving most recent version from csUnit web site...";
            Version recentVersion = RetrieveMostRecentVersion();
            temp += "\nMost recent version is: " + recentVersion.ToString(recentVersion.ToString().Split('.').Length);

            _image.Image = _imageList.Images[1];
            if( localVersion.CompareTo(recentVersion) < 0 ) {
               temp += "\n\nA newer release is available. "
                  + "If you want to download it now, click the link"
                  + " below.";
               _bNewerVersionAvailable = true;
            }
            else {
               temp += "\n\nNo newer service release is available. "
                  + "Your installation of csUnit is up-to-date.";
               SetControlText(_btnCancel, "Close");
            }
            SetControlText(_message, temp);
         }
         catch(ThreadAbortException) {
         }
         catch(Exception) {
            SetControlText(_message, "Could not download version info.");
         }
      }

      private static void SetControlText(Control label, string newText) {
         if(label.InvokeRequired) {
            label.Invoke(new ControlTextSetterDelegate(SetControlText),
               new object[] { label, newText });
         }
         else {
            label.Text = newText;
         }
      }

      private delegate void ControlTextSetterDelegate(Control label, string text);

      private Version RetrieveMostRecentVersion() {
         XmlDocument doc = LoadVersionInformation();
         XmlNodeList productNodes = doc.SelectNodes("/products/product");
         
         foreach(XmlNode productNode in productNodes) {
            if(   productNode.Attributes.GetNamedItem("Name") != null
               && productNode.Attributes["Name"].Value == "csUnit" ) {
               string strVersion = productNode.Attributes["Version"].Value;
               _url = productNode.Attributes["URL"].Value;
               return new Version(strVersion);
            }
         }
         
         return new Version(0,0,0);
      }

      private XmlDocument LoadVersionInformation() {
         XmlDocument doc = new XmlDocument();
         bool downloadOK = false;
         while(!downloadOK) {
            try {  
               doc.Load(PRODUCTS_URL);
               downloadOK = true;
               break;
            }
            catch(WebException ex) {
               if( ex.Response is HttpWebResponse ) {
                  HttpWebResponse response = (HttpWebResponse) ex.Response;
                  if( response.StatusCode.Equals( HttpStatusCode.ProxyAuthenticationRequired ) ) {
                     if( Authenticate().Equals(DialogResult.Cancel) ) {
                        break;
                     }
                     continue;
                  }
               }
               throw;
            }
         }

         if( !downloadOK ) {
            throw new Exception("Version information download failed.");
         }

         return doc;
      }

      private DialogResult Authenticate() {
         CheckForUpdatesProxyAuthenticationForm dlg = new CheckForUpdatesProxyAuthenticationForm();
         dlg.ShowDialog(this);
         return dlg.DialogResult;
      }

      private void _timer_Tick(object sender, EventArgs e) {
         if(   _workerThread == null
            || !_workerThread.IsAlive ) {
            if( _bNewerVersionAvailable ) {
               _lnkDownload.Visible = true;
            }
         }
      }

      private void _lnkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
         try {
            System.Diagnostics.Process.Start(_url);
         }
         catch {
         }
         Close();
      }

      private const string PRODUCTS_URL = @"http://www.csunit.org/download/products.xml";
      private Thread _workerThread = null;
      private bool   _bNewerVersionAvailable = false;
      private string _url = string.Empty;
      private Button _btnCancel;
      private System.Windows.Forms.Timer _timer;
      private ImageList _imageList;
      private PictureBox _image;
      private Label _message;
      private LinkLabel _lnkDownload;
      private System.ComponentModel.IContainer components;

   }
}
