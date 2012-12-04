#region Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, and Piers Lawson. All rights reserved.
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
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;
using csUnit.Ui.Controls.Commands;
using csUnit.Ui.Controls.Properties;
using csUnit.Ui.Controls.TabPages;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// This is the core user interface control for all graphical user interfaces
   /// of csUnit. It is used e.g. by csUnitRunner and by the Visual Studio add-in.
   /// </summary>
   public partial class CsUnitControl : UserControl {

      public event CsUnitControlEventHandler FillContextMenu;

      /// <summary>
      /// Instantiates a CsUnitControl object.
      /// </summary>
      public CsUnitControl() {
         InitializeComponent();

         InitControls();

         RecipeFactory.Loaded += OnRecipeLoaded;
         _recipeStarted = OnRecipeStarted;
         
         _assemblyAdded = OnAssemblyAdded;
         _onAssemblyChanged = OnAssemblyChanged;
         _assemblyRemoving = OnAssemblyRemoving;

         _testPassed = OnTestPassed;
         _testFailed = OnTestFailed;
         _testError = OnTestError;
         _testSkipped = OnTestSkipped;

         Application.Idle += ApplicationIdle;
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         _selectedItems.Clear();
         UnhookRecipe();
         Reset();
         HookupRecipe();
      }

      /// <summary>
      /// Gets the a collection of UiElementInfo objects representing the
      /// currently selected items.
      /// </summary>
      public UiElementInfoCollection SelectedItems {
         get {
            return _selectedItems;
         }
      }

      internal UiElementInfoCollection CheckedItems {
         get {
            return _checkedItems;
         }
      }

      /// <summary>
      /// Resets the controls to the state required just before any tests were
      /// run.
      /// </summary>
      private void Reset() {
         if(InvokeRequired) {
            Invoke(new MethodInvoker(Reset));
         }
         else {
            _progressBar.BarColor = ConfigCurrentUser.SuccessColor;
            _progressBar.TextContrastColor = Constants.ColorPassedContrast;
            _progressBar.Value = 0;
            _progressBar.Maximum = RecipeFactory.Current != null ? RecipeFactory.Current.CountTests() : 0; 
            _clearSearchButton_Click(this, new EventArgs());
            ResetCounters();
         }
      }

      private void HookupRecipe() {
         RecipeFactory.Current.Started += _recipeStarted;

         RecipeFactory.Current.AssemblyAdded += _assemblyAdded;
         RecipeFactory.Current.AssemblyRemoving += _assemblyRemoving;

         foreach(ITestAssembly ta in RecipeFactory.Current.Assemblies) {
            HookupAssembly(ta);
         }
      }

      private void UnhookRecipe() {
         if(RecipeFactory.Current != null) {
            foreach(ITestAssembly ta in RecipeFactory.Current.Assemblies) {
               UnhookAssembly(ta);
            }

            RecipeFactory.Current.AssemblyAdded -= _assemblyAdded;
            RecipeFactory.Current.AssemblyRemoving -= _assemblyRemoving;

            RecipeFactory.Current.Started -= _recipeStarted;
         }
      }

      private void HookupAssembly(ITestAssembly ta) {
         ta.AssemblyChanged += _onAssemblyChanged;
         ta.TestPassed  += _testPassed;
         ta.TestFailed  += _testFailed;
         ta.TestError   += _testError;
         ta.TestSkipped += _testSkipped;
      }

      private void UnhookAssembly(ITestAssembly ta) {
         ta.AssemblyChanged -= _onAssemblyChanged;
         ta.TestPassed -= _testPassed;
         ta.TestFailed  -= _testFailed;
         ta.TestError   -= _testError;
         ta.TestSkipped -= _testSkipped;
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         Reset();
         if( FindPanel.Visible ) {
            Command.ExecuteCommand(typeof(ShowFindPanelCommand), this, new EventArgs());
         }
      }

      private void OnAssemblyAdded(object sender, AssemblyEventArgs args) {
         ITestAssembly ta = RecipeFactory.Current[args.AssemblyFullName];
         HookupAssembly(ta);
         Reset();
         ResetCounters();
      }

      private void OnAssemblyChanged(object sender, AssemblyEventArgs args) {
         ClearAssemblySelections(RecipeFactory.Current[args.AssemblyFullName]);
         ResetCounters();
      }

      private void OnAssemblyRemoving(object sender, AssemblyEventArgs args) {
         ITestAssembly ta = RecipeFactory.Current[args.AssemblyFullName];
         UnhookAssembly(ta);
         ClearAssemblySelections(ta);
         Reset();
         ResetCounters();
      }

      private void ClearAssemblySelections(ITestAssembly ta) {
         // Remove the selected items that match this assembly
         var elementInfos = new List<UiElementInfo>();
         foreach(var info in _selectedItems) {
            if(info.AssemblyPathName == ta.Name.CodeBase) {
               elementInfos.Add(info);
            }
         }
         foreach(var info in elementInfos) {
            _selectedItems.Remove(info);
         }
      }

      private void OnTestPassed(object sender, TestResultEventArgs args) {
         _executedTestCount++;
      }

      private void OnTestError(object sender, TestResultEventArgs args) {
         if(InvokeRequired) {
            Invoke(new TestResultMethodDelegate(OnTestError), new[] { sender, args });
         }
         else {
            _progressBar.BarColor = ConfigCurrentUser.FailureColor;
            _progressBar.TextContrastColor = Constants.ColorErrorContrast;
            _executedTestCount++;
         }
      }

      private void OnTestFailed(object sender, TestResultEventArgs args) {
         if(InvokeRequired) {
            Invoke(new TestResultMethodDelegate(OnTestError), new[] { sender, args });
         }
         else {
            _progressBar.BarColor = ConfigCurrentUser.FailureColor;
            _progressBar.TextContrastColor = Constants.ColorFailedContrast;
            _executedTestCount++;
         }
      }

      private void OnTestSkipped(object sender, TestResultEventArgs args) {
         _executedTestCount++;
      }

      void ApplicationIdle(object sender, EventArgs e) {
         if( _executedTestCount != _progressBar.Value ) {
            _progressBar.Maximum = RecipeFactory.Current != null ? RecipeFactory.Current.CountTests() : 0;
            _progressBar.Value = _executedTestCount;
         }
      }
      
      private void ResetCounters() {
         if(InvokeRequired) {
            Invoke(new MethodInvoker(ResetCounters));
         }
         else {
            _executedTestCount = 0;
         }
      }

      private void OnItemCheckedStateChanged(IContentControl sender, ContentControlEventArgs args) {
         if (args.UiElementInfo != null) {
            if(args.Checked) {
               _checkedItems.Add(args.UiElementInfo);
            }
            else {
               _checkedItems.Remove(args.UiElementInfo);
            }
         }
      }

      /// <summary>
      /// Callback executed when a user right-clicks into the control and in the
      /// context menu should be shown.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      private void OnFillContextMenu(object sender, CsUnitControlEventArgs args) {
         // Just forward this event
         if( FillContextMenu != null ) {
            FillContextMenu(sender, args);
         }
      }

      private void OnAfterSelect(object sender, TreeViewEventArgs args) {
         _selectedItems.Clear();
         if(   null != args
            && args.Node != null) {
            _selectedItems.Add(args.Node.Tag as UiElementInfo);
         }
      }

      #region Tab Page Creation and Initialization
      /// <summary>
      /// Initializes controls. Called when the form is created.
      /// </summary>
      private void InitControls() {
         _tabControl.Controls.AddRange(CreateTabPagesFromContentControls());
         if(_tabControl.TabPages.Count > 0) {
            _tabControl.SelectedTab = _tabControl.TabPages[0];
         }
         _tabControl.Invalidate();
      }

      /// <summary>
      /// Scanning the TabPages namespace this method creates a list of tab
      /// pages to be added to the tab control in the csUnitControl.
      /// This way the tab pages can be dynamically added/removed to the tab 
      /// control during runtime, while at the same time remaining editable 
      /// in the graphical editor.
      /// </summary>
      /// <returns>A list of TabPage objects.</returns>
      private TabPage[] CreateTabPagesFromContentControls() {
         var contentControls = FindAllContentControls();
         var tabPages = new List<TabPage>(contentControls.Count);

         contentControls.Sort(
            (ctrl1, ctrl2) =>
            ctrl1.DesiredTabPosition.CompareTo(ctrl2.DesiredTabPosition));

         foreach(var cc in contentControls) {
            tabPages.Add(CreateTabPageFor(cc));
         }

         return tabPages.ToArray();
      }

      private List<IContentControl> FindAllContentControls() {
         var contentControls = new List<IContentControl>();

         foreach(var t in GetType().Assembly.GetTypes()) {
            if(t.GetInterface(typeof(IContentControl).Name) != null) {
               var ci = t.GetConstructor(Type.EmptyTypes);
               if(ci != null) {
                  var cc = ci.Invoke(new object[] { }) as IContentControl;
                  if(cc != null) {
                     contentControls.Add(cc);
                  }
               }
            }
         }

         return contentControls;
      }

      /// <summary>
      /// Takes a user control and embeds it in a tab page.
      /// </summary>
      /// <param name="contentControl">A single control that represents the content of a the tab page.</param>
      /// <returns>A tab page with the content control embedded.</returns>
      private TabPage CreateTabPageFor(IContentControl contentControl) {
         var tabPage = new TabPage(contentControl.Name) { ToolTipText = contentControl.ToolTipText };
         contentControl.FillContextMenu += OnFillContextMenu;
         contentControl.AfterSelect += OnAfterSelect;
         contentControl.ItemCheckedStateChanged += OnItemCheckedStateChanged;
         
         var control = contentControl as Control;
         if(control != null) {
            tabPage.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            
         }
         else {
            var label = new Label {  Top = 10,
                                     Left = 10,
                                     Text = ("Couldn't load content control '" + contentControl.Name 
                                     + "'. It's not derived from class Control.")
                                  };
            tabPage.Controls.Add(label);
         }
         return tabPage;
      }
      #endregion

      #region Private Fields
      private int _executedTestCount;

      private readonly RecipeEventHandler _recipeStarted;

      private readonly AssemblyEventHandler _assemblyAdded;
      private readonly AssemblyEventHandler _onAssemblyChanged;
      private readonly AssemblyEventHandler _assemblyRemoving;

      private readonly TestEventHandler _testPassed;
      private readonly TestEventHandler _testFailed;
      private readonly TestEventHandler _testError;
      private readonly TestEventHandler _testSkipped;

      private readonly UiElementInfoCollection _selectedItems = new UiElementInfoCollection();
      private readonly UiElementInfoCollection _checkedItems = new UiElementInfoCollection();
      #endregion // Private Fields

      #region UI Delegates
      private delegate void TestResultMethodDelegate(object sender, TestResultEventArgs args);
      #endregion // UI Delegates

      #region Find Panel

      public Panel FindPanel { get; private set; }

      private void _findButton_Click(object sender, EventArgs e) {
         _findResultsListView.Items.Clear();
         AddToRecentSearchTerms();
         foreach( TabPage page in _tabControl.Controls ) {
            var ctrl = page.Controls[0] as IContentControl;
            if( ctrl != null ) {
               foreach( var element in ctrl.Find(_findTermDropDown.Text.ToUpper()) ) {
                  var lvi = new ListViewItem(element.KeyTerm) { 
                     ToolTipText = "Double-click to navigate to this item.",
                     Tag = element
                  };
                  lvi.SubItems.Add(element.ContentControl.Name);
                  _findResultsListView.Items.Add(lvi);
               }
            }
         }
         _clearSearchButton.Enabled = _findResultsListView.Items.Count > 0;
      }

      private void AddToRecentSearchTerms() {
         if (Settings.Default.RecentSearchTerms == null ) {
            Settings.Default.RecentSearchTerms = new StringCollection();
         }
         while( Settings.Default.RecentSearchTerms.Contains(_findTermDropDown.Text) ) {
            Settings.Default.RecentSearchTerms.Remove(_findTermDropDown.Text);
         }
         Settings.Default.RecentSearchTerms.Insert(0, _findTermDropDown.Text);
         while(Settings.Default.RecentSearchTerms.Count > Settings.Default.RecentSearchTermsMaxItems) {
            Settings.Default.RecentSearchTerms.RemoveAt(Settings.Default.RecentSearchTerms.Count - 1);
         }
         Settings.Default.Save();
      }

      private void CsUnitControl_SizeChanged(object sender, EventArgs e) {
         var oldSize = _findTermDropDown.Size;
         const int leftPadding = 6;
         const int experimentalValue = 5;
         _findTermDropDown.Size = new Size(_findPanelToolStrip.Width - 2 * _findButton.Width - experimentalValue - leftPadding,
                                           oldSize.Height);
      }

      private void _clearSearchButton_Click(object sender, EventArgs e) {
         _findResultsListView.Items.Clear();
         _findTermDropDown.Text = string.Empty;
         _findButton.Enabled = _findTermDropDown.Text != string.Empty;
         _clearSearchButton.Enabled = _findResultsListView.Items.Count > 0;
      }

      private void _findResultsListView_VisibleChanged(object sender, EventArgs e) {
         if( FindPanel.Visible ) {
            _findTermDropDown.Focus();
         }
      }

      private void _findTermDropDown_KeyDown(object sender, KeyEventArgs e) {
         if( e.KeyCode == Keys.Enter ) {
            _findButton_Click(this, new EventArgs());
            e.Handled = true;
         }
      }

      private void _findTermDropDown_KeyUp(object sender, KeyEventArgs e) {
         _findButton.Enabled = _findTermDropDown.Text != string.Empty;
      }

      private void _findResultsListView_DoubleClick(object sender, EventArgs e) {
         if( _findResultsListView.SelectedItems.Count > 0 ) {
            var selectedItem = _findResultsListView.SelectedItems[0];
            var findLocation = selectedItem.Tag as FindLocation;
            if( findLocation != null ) {
               _tabControl.SelectedTab = FindTabPageWith(findLocation.ContentControl);
               findLocation.ContentControl.NavigateTo(findLocation);
               if(FindPanel.Visible) {
                  Command.ExecuteCommand(typeof(ShowFindPanelCommand), this, new EventArgs());
               }
            }
         }
      }

      private TabPage FindTabPageWith(IContentControl contentControl) {
         foreach(TabPage page in _tabControl.TabPages) {
            var ctrl = page.Controls[0];
            if( ctrl.Equals(contentControl) ) {
               return page;
            }
         }
         return null;
      }

      private void _findTermDropDown_DropDown(object sender, EventArgs e) {
         if( Settings.Default.RecentSearchTerms == null ) {
            Settings.Default.RecentSearchTerms = new StringCollection();
         }
         _findTermDropDown.Items.Clear();
         foreach(var term in Ui.Controls.Properties.Settings.Default.RecentSearchTerms) {
            _findTermDropDown.Items.Add(term);
         }
      }

      #endregion
   }
}
