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
using System.Collections.Generic;
using System.Windows.Forms;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   /// <summary>
   /// Summary description for FileExitCommand.
   /// </summary>
   public class FileExitCommand : Command {
      
      private const int _menuPosition = 9;

      public FileExitCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "E&xit", _menuPosition, true) {
      }

      protected override void Execute(object sender, EventArgs args) {
         // Can't iterate on Application.OpenForms and use the iterator for
         // closing the forms, as closing a form will modify the collection 
         // Application.OpenForms and the iterator would become useless. 
         // Therefore we create a copy first.
         List<Form> forms = new List<Form>();
         foreach(Form form in Application.OpenForms) {
            forms.Add(form);
         }
         foreach(Form form in forms) {
            form.Close();
         }
      }
   }
}
