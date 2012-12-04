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
using csUnit.Core;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   internal sealed class FileLoader {
      /// <summary>
      /// Default constructor is private. Class contains only static members.
      /// </summary>
      private FileLoader() {
      }

      /// <summary>
      /// Called, when an assembly has been chosen from the recent assemblies
      /// list to be loaded.
      /// </summary>
      /// <param name="assembly">Path and name of the assembly to be loaded.</param>
      static public void LoadFile(String assembly) {
         Cursor toBeRestored = Cursor.Current;
         Cursor.Current = Cursors.WaitCursor;
         try {
            if(assembly.ToUpper().EndsWith(".RECIPE")) {
               RecipeFactory.Load(assembly);
            }
            else {
               RecipeFactory.Current.AddAssembly(assembly);
            }
         }
         finally {
            Cursor.Current = toBeRestored;
         }
      }

      /// <summary>
      /// Called, when an assembly from the recent assemblies list couldn't be
      /// loaded.
      /// </summary>
      /// <param name="assemblyPathName">Path and name of the assembly.</param>
      static public void FileNotFound(String assemblyPathName) {
         MessageBox.Show(Command.FindMainWindow(), "Assembly " + assemblyPathName + " not found." +
            " Will be removed from the menu.");
      }
   }
}
