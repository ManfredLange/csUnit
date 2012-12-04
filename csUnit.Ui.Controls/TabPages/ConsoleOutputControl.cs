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
using System.Diagnostics;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.TabPages {
   /// <summary>
   /// Displays the contents of Console.Error and Console.Out created by tests.
   /// </summary>
   /// <remarks>Please note that although this class is a UserControl, it will
   /// still be displayed within a tab page. The reason for this construct is
   /// that on one hand the control should remain editable in the graphical
   /// designer of VS, but on the other hand the tab control within the
   /// CsUnitControl should remain generic using reflection to populate the
   /// tab pages.</remarks>
   internal partial class ConsoleOutputControl : UserControl, IContentControl {
      public ConsoleOutputControl() {
         InitializeComponent();
         Name = "Output";
         _onRecipeLoaded = Recipe_Loaded;
         _onRecipeStarted = Recipe_Started;
         _onRecipeClosing = Recipe_Closing;

         _swConsole = new ConsoleWriter(_textOutput);

         Debug.Listeners.Add(new TextWriterTraceListener(_swConsole, "csUnitDebug"));
         Trace.Listeners.Add(new TextWriterTraceListener(_swConsole, "csUnitTrace"));

         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
         if(RecipeFactory.Current != null) {
            RecipeFactory.Current.Started += _onRecipeStarted;
         }
         _copyToClipboardButton.Enabled = _textOutput.Text != string.Empty;
      }

      #region IContentControl implementation
#pragma warning disable 67
      public event TreeViewEventHandler AfterSelect;
      public event CsUnitControlEventHandler FillContextMenu;
      public event ContentControlEventHandler ItemCheckedStateChanged;
#pragma warning restore 67

      public int DesiredTabPosition {
         get {
            return 2;
         }
      }

      private class ConsoleOutputFindLocation : FindLocation {
         public ConsoleOutputFindLocation(int index, string keyTerm, string searchTerm, IContentControl ctrl)
            : base(keyTerm, ctrl) {
            _index = index;
            _searchTerm = searchTerm;
         }
         public int Index {
            get {
               return _index;
            }
         }
         public string SearchTerm {
            get {
               return _searchTerm;
            }
         }
         private readonly int _index = 0;
         private readonly string _searchTerm = string.Empty;
      }

      public List<FindLocation> Find(string searchTerm) {
         List<FindLocation> entries = new List<FindLocation>();
         int index = _textOutput.Find(searchTerm, 0, RichTextBoxFinds.NoHighlight);
         while(index >= 0) {
            string contextTerm = _textOutput.Text.Substring(index, searchTerm.Length + 20) + "...";
            entries.Add(new ConsoleOutputFindLocation(index, contextTerm, searchTerm, this));
            index = _textOutput.Find(searchTerm, index + 1, RichTextBoxFinds.NoHighlight);
         }
         return entries;
      }

      public string ToolTipText {
         get {
            return "Displays console output of the test run.";
         }
      }

      public void NavigateTo(FindLocation findLocation) {
         ConsoleOutputFindLocation location = findLocation as ConsoleOutputFindLocation;
         if( location != null ) {
            _textOutput.Find(location.SearchTerm, location.Index, RichTextBoxFinds.None);
         }
      }

      public void UpdateCheckedItems(UiElementInfoCollection checkedItems) {
      }

      #endregion

      void Recipe_Loaded(object sender, RecipeEventArgs args) {
         RecipeFactory.Current.Started += _onRecipeStarted;
      }

      void Recipe_Started(object sender, RecipeEventArgs args) {
         RecipeFactory.Current.SetConsoleOutputTo(_swConsole);
         _swConsole.Clear();
      }

      void Recipe_Closing(object sender, RecipeEventArgs args) {
         RecipeFactory.Current.Started -= _onRecipeStarted;
      }

      private void _copyToClipboardButton_Click(object sender, EventArgs e) {
         try {
            if(_textOutput.Text != string.Empty) {
               Clipboard.SetText(_textOutput.Text);
            }
         }
         catch(Exception ex) {
            Debug.WriteLine(ex.Message);
         }
      }

      private void _textOutput_TextChanged(object sender, EventArgs e) {
         _copyToClipboardButton.Enabled = _textOutput.Text != string.Empty;
      }

      private readonly ConsoleWriter _swConsole = null;
      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeStarted;
      private readonly RecipeEventHandler _onRecipeClosing;
   }
}
