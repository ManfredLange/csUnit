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

namespace csUnit.Runner.Forms {
   partial class TestForm {

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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
         this._fontSizeTrackBar = new System.Windows.Forms.TrackBar();
         this._itemWidthTrackBar = new System.Windows.Forms.TrackBar();
         this._fontLabel = new System.Windows.Forms.Label();
         this._itemWidthLabel = new System.Windows.Forms.Label();
         this._resetButton = new System.Windows.Forms.Button();
         this._smartTree = new csUnit.Ui.Controls.SmartTree();
         this._imageList = new System.Windows.Forms.ImageList(this.components);
         ( (System.ComponentModel.ISupportInitialize) ( this._fontSizeTrackBar ) ).BeginInit();
         ( (System.ComponentModel.ISupportInitialize) ( this._itemWidthTrackBar ) ).BeginInit();
         this.SuspendLayout();
         // 
         // _fontSizeTrackBar
         // 
         this._fontSizeTrackBar.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._fontSizeTrackBar.Location = new System.Drawing.Point(394, 47);
         this._fontSizeTrackBar.Maximum = 100;
         this._fontSizeTrackBar.Minimum = 5;
         this._fontSizeTrackBar.Name = "_fontSizeTrackBar";
         this._fontSizeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
         this._fontSizeTrackBar.Size = new System.Drawing.Size(45, 228);
         this._fontSizeTrackBar.TabIndex = 5;
         this._fontSizeTrackBar.TickFrequency = 100;
         this._fontSizeTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
         this._fontSizeTrackBar.Value = 9;
         this._fontSizeTrackBar.Scroll += new System.EventHandler(this.OnFontSizeTrackBarScroll);
         // 
         // _itemWidthTrackBar
         // 
         this._itemWidthTrackBar.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._itemWidthTrackBar.Location = new System.Drawing.Point(448, 50);
         this._itemWidthTrackBar.Maximum = 1000;
         this._itemWidthTrackBar.Minimum = 50;
         this._itemWidthTrackBar.Name = "_itemWidthTrackBar";
         this._itemWidthTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
         this._itemWidthTrackBar.Size = new System.Drawing.Size(45, 225);
         this._itemWidthTrackBar.TabIndex = 6;
         this._itemWidthTrackBar.TickFrequency = 100;
         this._itemWidthTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
         this._itemWidthTrackBar.Value = 200;
         this._itemWidthTrackBar.Scroll += new System.EventHandler(this.OnItemWidthTrackBarScroll);
         // 
         // _fontLabel
         // 
         this._fontLabel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._fontLabel.Location = new System.Drawing.Point(394, 9);
         this._fontLabel.Name = "_fontLabel";
         this._fontLabel.Size = new System.Drawing.Size(45, 35);
         this._fontLabel.TabIndex = 7;
         this._fontLabel.Text = "Font size:";
         // 
         // _itemWidthLabel
         // 
         this._itemWidthLabel.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._itemWidthLabel.Location = new System.Drawing.Point(445, 9);
         this._itemWidthLabel.Name = "_itemWidthLabel";
         this._itemWidthLabel.Size = new System.Drawing.Size(45, 35);
         this._itemWidthLabel.TabIndex = 8;
         this._itemWidthLabel.Text = "Item width:";
         // 
         // _resetButton
         // 
         this._resetButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._resetButton.Location = new System.Drawing.Point(397, 281);
         this._resetButton.Name = "_resetButton";
         this._resetButton.Size = new System.Drawing.Size(96, 23);
         this._resetButton.TabIndex = 9;
         this._resetButton.Text = "Reset";
         this._resetButton.UseVisualStyleBackColor = true;
         this._resetButton.Click += new System.EventHandler(this.OnResetButtonClick);
         // 
         // _smartTree
         // 
         this._smartTree.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                     | System.Windows.Forms.AnchorStyles.Left )
                     | System.Windows.Forms.AnchorStyles.Right ) ) );
         this._smartTree.AutoScroll = true;
         this._smartTree.BackColor = System.Drawing.SystemColors.Window;
         this._smartTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this._smartTree.DefaultItemWidth = 200;
         this._smartTree.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this._smartTree.ImageList = null;
         this._smartTree.Location = new System.Drawing.Point(12, 12);
         this._smartTree.Name = "_smartTree";
         this._smartTree.ShowNodeToolTips = true;
         this._smartTree.Size = new System.Drawing.Size(376, 292);
         this._smartTree.TabIndex = 4;
         // 
         // _imageList
         // 
         this._imageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject("_imageList.ImageStream") ) );
         this._imageList.TransparentColor = System.Drawing.Color.Transparent;
         this._imageList.Images.SetKeyName(0, "TestAssembly.gif");
         this._imageList.Images.SetKeyName(1, "TestSuite.gif");
         this._imageList.Images.SetKeyName(2, "TestFixture.gif");
         this._imageList.Images.SetKeyName(3, "TestMethod.gif");
         this._imageList.Images.SetKeyName(4, "Comment.gif");
         // 
         // TestForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(505, 325);
         this.Controls.Add(this._resetButton);
         this.Controls.Add(this._itemWidthLabel);
         this.Controls.Add(this._fontLabel);
         this.Controls.Add(this._itemWidthTrackBar);
         this.Controls.Add(this._fontSizeTrackBar);
         this.Controls.Add(this._smartTree);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this.Name = "TestForm";
         this.Text = "TestForm";
         ( (System.ComponentModel.ISupportInitialize) ( this._fontSizeTrackBar ) ).EndInit();
         ( (System.ComponentModel.ISupportInitialize) ( this._itemWidthTrackBar ) ).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Ui.Controls.SmartTree _smartTree;
      private System.Windows.Forms.TrackBar _fontSizeTrackBar;
      private System.Windows.Forms.TrackBar _itemWidthTrackBar;
      private System.Windows.Forms.Label _fontLabel;
      private System.Windows.Forms.Label _itemWidthLabel;
      private System.Windows.Forms.Button _resetButton;
      private System.Windows.Forms.ImageList _imageList;
      private System.ComponentModel.IContainer components;

   }
}