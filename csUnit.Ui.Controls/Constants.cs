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

using System.Drawing;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// Summary description for Constants.
   /// </summary>
   public abstract class Constants {
      public static readonly Color  ColorPassed = Color.FromArgb(64,255,64);
      public static readonly Color  ColorPassedContrast  = Color.Black;
      public static readonly Color  ColorError = Color.FromArgb(255,64,64);
      public static readonly Color  ColorErrorContrast   = Color.Black;
      public static readonly Color  ColorReset = Color.Empty;
      public static readonly Color  ColorResetContrast   = Color.Black;
      public static readonly Color  ColorFailed = Color.FromArgb(255,64,64);
      public static readonly Color  ColorFailedContrast  = Color.Black;
      public static readonly Color  ColorSkipped = Color.FromArgb(224,224,224);
      public static readonly Color  ColorSkippedContrast = Color.Black;
      public const string RECIPE_FILE_FILTER = "Recipe Files (*.recipe)|*.recipe|All files (*.*)|*.*";
   }
}

