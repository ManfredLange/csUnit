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

using System.Drawing;
using System.Windows.Forms;

namespace csUnit.Ui.Controls {
   public class CustomProfessionalColors : ProfessionalColorTable {
      public override Color ToolStripGradientBegin {
         get {
            return _lightBrown;
         }
      }

      public override Color ToolStripGradientMiddle {
         get {
            return _middleBrown;
         }
      }

      public override Color ToolStripGradientEnd {
         get {
            return _darkBrown;
         }
      }

      public override Color ToolStripBorder {
         get {
            return _darkBrown;
         }
      }

      public override Color MenuStripGradientBegin {
         get {
            return _middleBrown;
         }
      }

      public override Color MenuStripGradientEnd {
         get {
            return _middleBrown;
         }
      }

      public override Color MenuItemSelected {
         get {
            return _lightBlue;
         }
      }

      public override Color MenuItemSelectedGradientBegin {
         get {
            return _lightBlue;
         }
      }

      public override Color MenuItemSelectedGradientEnd {
         get {
            return _lightBlue;
         }
      }

      public override Color ImageMarginGradientBegin {
         get {
            return _lightBrown;
         }
      }

      public override Color ImageMarginGradientMiddle {
         get {
            return _middleBrown;
         }
      }

      public override Color ImageMarginGradientEnd {
         get {
            return _darkBrown;
         }
      }

      public override Color ButtonSelectedBorder {
         get {
            return _darkBlue;
         }
      }

      public override Color OverflowButtonGradientBegin {
         get {
            return _lightBrown;
         }
      }

      public override Color OverflowButtonGradientMiddle {
         get {
            return _middleBrown;
         }
      }

      public override Color OverflowButtonGradientEnd {
         get {
            return _darkBrown;
         }
      }

      public override Color ButtonPressedGradientBegin {
         get {
            return _middleBlue;
         }
      }

      public override Color ButtonPressedGradientMiddle {
         get {
            return _middleBlue;
         }
      }

      public override Color ButtonPressedGradientEnd {
         get {
            return _middleBlue;
         }
      }

      public override Color ButtonSelectedGradientBegin {
         get {
            return _lightBlue;
         }
      }

      public override Color ButtonSelectedGradientMiddle {
         get {
            return _lightBlue;
         }
      }

      public override Color ButtonSelectedGradientEnd {
         get {
            return _lightBlue;
         }
      }

      public override Color MenuItemPressedGradientBegin {
         get {
            return _lightBrown;
         }
      }

      public override Color MenuItemPressedGradientMiddle {
         get {
            return _lightBrown;
         }
      }

      public override Color MenuItemPressedGradientEnd {
         get {
            return _lightBrown;
         }
      }

      public override Color SeparatorDark {
         get {
            return _separatorColor;
         }
      }

      public override Color SeparatorLight {
         get {
            return Color.Transparent;
         }
      }

      private readonly Color _lightBrown = Color.FromArgb(251, 250, 247);
      private readonly Color _middleBrown = Color.FromArgb(235, 231, 224);
      private readonly Color _darkBrown = Color.FromArgb(189, 189, 163);
      private readonly Color _lightBlue = Color.FromArgb(193, 210, 238);
      private readonly Color _middleBlue = Color.FromArgb(152, 181, 226);
      private readonly Color _darkBlue = Color.FromArgb(49, 106, 197);
      private readonly Color _separatorColor = Color.FromArgb(197, 194, 184);
   }
}
