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
using System.Windows.Forms;

namespace csUnit.Runner {
   /// <summary>
   /// Class for some utility methods
   /// </summary>
   public abstract class Utils {
      /// <summary>
      /// Verifies, whether a given file exists.
      /// </summary>
      /// <param name="filePathName">Path and name of file to verify.</param>
      /// <param name="bShowMessage">If true a message will be displayed on the UI.</param>
      /// <returns>'True', if file exists, else 'False'.</returns>
      public static bool FileExists(String filePathName, bool bShowMessage) {
         bool bExists = csUnit.Core.Util.FileExists(filePathName);
         if(   !bExists
            && bShowMessage ) {
            MessageBox.Show(Form.ActiveForm, "Could not find file '" + filePathName + "'", "csUnitRunner",
               MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
         return bExists;
      }
   }
}
