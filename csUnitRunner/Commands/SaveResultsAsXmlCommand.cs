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
using csUnit.Interfaces;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner.Commands {
   public class SaveResultsAsXmlCommand : Command {
      private const int _menuPosition = 8;

      public SaveResultsAsXmlCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&File", "Save Result As &XML...", _menuPosition, true) {
         _results = new DefaultXmlWriter(RecipeFactory.Current, string.Empty);
         RecipeFactory.Loaded += RecipeFactory_Loaded;
         Enabled = false;
      }

      void RecipeFactory_Loaded(object sender, RecipeEventArgs args) {
         _results = new DefaultXmlWriter(RecipeFactory.Current, string.Empty);
         RecipeFactory.Current.Finished += Current_Finished;
         Enabled = false;
      }

      void Current_Finished(object sender, RecipeEventArgs args) {
         Enabled = true;
      }

      protected override void Execute(object sender, EventArgs args) {
         SaveFileDialog dlg = new SaveFileDialog();
         dlg.Filter = Constants.RESULTS_FILE_FILTER;
         dlg.Title = "Save Results";
         dlg.AddExtension = true;
         if( DialogResult.OK == dlg.ShowDialog(FindMainWindow()) ) {
            _results.Save(dlg.FileName);
         }

      }

      private DefaultXmlWriter _results;
   }
}
