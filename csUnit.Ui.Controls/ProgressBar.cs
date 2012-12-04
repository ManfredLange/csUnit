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
using System.Drawing;
using System.Windows.Forms;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// A smooth progress bar with overlaid text
   /// </summary>
   public class ProgressBar : UserControl {
      /// <summary>
      /// Creates a ProgressBar object.
      /// </summary>
      public ProgressBar() {
         // This call is required by the Windows.Forms Form Designer.
         InitializeComponent();
         SetStyle(ControlStyles.DoubleBuffer         |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint            |
            ControlStyles.ResizeRedraw, true);
      }

      /// <summary> 
      /// Gets or sets the maximum value of the progress bar (must be at least 
      /// 0 and less than the Maximum value)
      /// </summary>
      public int Minimum {
         get {
            return Convert.ToInt32(_min);
         }

         set {
            if( value != _min ) {
               // Prevent a negative value.
               if (_value < 0) {
                  _min = 0;
               }
		
               // Make sure that the minimum value is no greater than the 
               // maximum
               if (_min > _max) {
                  _min = _max;
               }
		
               // Ensure value is still in range
               if (_value < _min) {
                  _value = _min;
               }

               Invalidate();
            }
         }
      }

      /// <summary>
      /// Gets or sets the tool tip text.
      /// </summary>
      public string ToolTipText {
         get {
            return _toolTips.GetToolTip(this);
         }
         set {
            if(value == null) {
               _toolTips.SetToolTip(this, string.Empty);
            }
            else {
               _toolTips.SetToolTip(this, value);
            }
         }
      }

      /// <summary> 
      /// Gets or sets the maximum value of the progress bar (must be at least 0)
      /// </summary>
      public int Maximum {
         get {
            return Convert.ToInt32(_max);
         }

         set {
            if (value != _max) {
               // Enforce lowest max value as 0
               _max = value >= 0 ? value : 0;

               // Enforce min is less than or equal to max
               if (_min > _max) {
                  _min = _max;
               }

               // Make sure that value is still in range.
               if (_value < _min) {
                  _value = _min;
               }
               if (_value > _max) {
                  _value = _max;
               }

               Invalidate();
            }
         }
      }

      /// <summary> 
      /// Gets or sets the current value of the progress bar
      /// </summary>
      public int Value {
         get {
            return Convert.ToInt32(_value);
         }

         set {
            if( value != _value ) {
               // Make sure that the value does not stray outside the valid range.
               if (value < _min) {
                  _value = _min;
               }
               else _value = value > _max ? _max : value;

               // Invalidate the entire control to ensure the text in the centre
               // is being updated
               Invalidate();
            }
         }
      }

      /// <summary> 
      /// Gets or sets the color of the progress bar.
      /// </summary>
      public Color BarColor {
         get {
            return _barBrush.Color;
         }

         set {
            if( value != _barBrush.Color ) {
               _barBrush.Color = value;
               Invalidate();
            }
         }
      }
      
      /// <summary> 
      /// Gets or sets the color used for drawing text on the colored part of
      /// the progress bar.
      /// </summary>
      public Color TextContrastColor {
         get {
            return _contrastTextBrush.Color;
         }

         set {
            if( value != _contrastTextBrush.Color ) {
               _contrastTextBrush.Color = value;
               Invalidate();
            }
         }
      }

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose( bool disposing ) {
         if( disposing ) {
            if(components != null) {
               components.Dispose();
            }
            _barBrush.Dispose();
            _textBrush.Dispose();
            _contrastTextBrush.Dispose();
         }
         base.Dispose( disposing );
      }

      #region Component Designer generated code
      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         this._toolTips = new System.Windows.Forms.ToolTip(this.components);
         this.SuspendLayout();
         // 
         // _toolTips
         // 
         this._toolTips.AutomaticDelay = 0;
         this._toolTips.AutoPopDelay = 5000;
         this._toolTips.InitialDelay = 0;
         this._toolTips.ReshowDelay = 100;
         // 
         // ProgressBar
         // 
         this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "ProgressBar";
         this.Size = new System.Drawing.Size(376, 120);
         this._toolTips.SetToolTip(this, "Shows test progress. When tests are executed\r\nalso shows last executed test.");
         this.ResumeLayout(false);

      }
      #endregion

      /// <summary> 
      /// Handles the paint event.
      /// </summary>
      protected override void OnPaint(PaintEventArgs e) {
         base.OnPaint(e);

         var surface = e.Graphics;

         // Calculate area for drawing the progress bar
         var progressRect = ClientRectangle;
         float percent = 0;
         if(_max > _min) {
            percent = (_value - _min) / (_max - _min);
         }
         progressRect.Width = (int)(progressRect.Width * percent);

         // Calculate the area for outputing text
         var progressString = String.Format("{0} of {1} ({2} %)", _value, _max, (percent * 100).ToString("#0"));
         var textSize = surface.MeasureString(progressString, Font, 1024, StringFormat.GenericTypographic);
         var xPos = ((float) ClientSize.Width / 2) - (textSize.Width / 2);
         var yPos = ((float) ClientSize.Height / 2) - (textSize.Height / 2);

         // Draw the progress bar with overlaid text
         surface.DrawString(progressString, Font, _textBrush, xPos, yPos, StringFormat.GenericTypographic);
         surface.FillRectangle(_barBrush, progressRect);
         surface.SetClip(progressRect);
         surface.DrawString(progressString, Font, _contrastTextBrush, xPos, yPos, StringFormat.GenericTypographic);

         // Draw a three-dimensional border around the control.
         surface.ResetClip();
         Draw3DBorder(surface);
      }

      /// <summary> 
      /// Add a 3D border to the control
      /// </summary>
      private void Draw3DBorder(Graphics g) {
         var penWidth = (int)Pens.White.Width;
         g.DrawLine(Pens.DarkGray, new Point(ClientRectangle.Left, ClientRectangle.Top), new Point(ClientRectangle.Width - penWidth, ClientRectangle.Top));
         g.DrawLine(Pens.DarkGray, new Point(ClientRectangle.Left, ClientRectangle.Top), new Point(ClientRectangle.Left, ClientRectangle.Height - penWidth));
         g.DrawLine(Pens.White, new Point(ClientRectangle.Left, ClientRectangle.Height - penWidth), new Point(ClientRectangle.Width - penWidth, ClientRectangle.Height - penWidth));
         g.DrawLine(Pens.White, new Point(ClientRectangle.Width - penWidth, ClientRectangle.Top), new Point(ClientRectangle.Width - penWidth, ClientRectangle.Height - penWidth));
      }

      private ToolTip _toolTips;
      private System.ComponentModel.IContainer components;

      private float        _min;
      private float        _max           = 100;
      private float        _value;

      private readonly SolidBrush _barBrush = new SolidBrush(Color.LightGreen);
      private readonly SolidBrush   _textBrush        = new SolidBrush(Color.Black);
      private readonly SolidBrush   _contrastTextBrush = new SolidBrush(Color.Black);
   }
}
