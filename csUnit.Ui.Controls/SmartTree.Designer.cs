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

namespace csUnit.Ui.Controls {
   partial class SmartTree {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private readonly System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
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
         this._contentPanel = new csUnit.Ui.Controls.SmartTree.SmartTreeContentPanel();
         this.SuspendLayout();
         // 
         // _contentPanel
         // 
         this._contentPanel.BackColor = System.Drawing.Color.GreenYellow;
         this._contentPanel.CausesValidation = false;
         this._contentPanel.Location = new System.Drawing.Point(0, 0);
         this._contentPanel.Name = "_contentPanel";
         this._contentPanel.Size = new System.Drawing.Size(135, 135);
         this._contentPanel.TabIndex = 1;
         this._contentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnContentPanelPaint);
         this._contentPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnContentPanelMouseDown);
         // 
         // SmartTree
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
         this.AutoScroll = true;
         this.BackColor = System.Drawing.Color.FromArgb(( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 128 ) ) ) ), ( (int) ( ( (byte) ( 0 ) ) ) ));
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this._contentPanel);
         this.DoubleBuffered = true;
         this.Name = "SmartTree";
         this.Size = new System.Drawing.Size(146, 146);
         this.FontChanged += new System.EventHandler(this.SmartTree_FontChanged);
         this.ResumeLayout(false);

      }

      #endregion

      private SmartTreeContentPanel _contentPanel;

   }
}
