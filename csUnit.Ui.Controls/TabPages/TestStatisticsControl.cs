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
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.TabPages {
   /// <summary>
   /// Displays basic statistics for the test results of a recipe.
   /// </summary>
   /// <remarks>Although this class is a UserControl, it will still be 
   /// displayed within a tab page. The reason for this construct is that on 
   /// one hand the control should remain editable in the graphical designer of 
   /// VS, but on the other hand the tab control within the CsUnitControl 
   /// should remain generic using reflection to populate the tab pages.
   /// </remarks>
   internal partial class TestStatisticsControl : UserControl, IContentControl {
      #region Constructors

      public TestStatisticsControl() {
         InitializeComponent();
         Name = "Statistics";

         _averageTestTimeFormat = _averageTimeLabel.Text;
         _estimatedOverheadFormat = _overheadTimeLabel.Text;

         _averageTimeLabel.Text = _avgTimePerTestUnknown;
         _overheadTimeLabel.Text = _estimatedOverheadUnknown;

         _onRecipeLoaded = OnRecipeLoaded;
         _onRecipeStarted = OnRecipeStarted;
         _onRecipeFinished = OnRecipeFinished;

         _onTestAssemblyAdded = OnTestAssemblyAdded;
         _onTestAssemblyRemoving = OnTestAssemblyRemoving;

         _onTestEvent = OnTestEvent;

         _lvStatistics.ListViewItemSorter = new StatisticsListSorter();

         RecipeFactory.Loaded += _onRecipeLoaded;
         if(RecipeFactory.Current != null) {
            HookupRecipe();
         }
      }

      #endregion

      #region IContentControl Implementation
#pragma warning disable 67
      public event TreeViewEventHandler AfterSelect;
      public event CsUnitControlEventHandler FillContextMenu;
      public event ContentControlEventHandler ItemCheckedStateChanged;
#pragma warning restore 67

      public int DesiredTabPosition {
         get {
            return 3;
         }
      }

      public List<FindLocation> Find(string searchTerm) {
         List<FindLocation> entries = new List<FindLocation>();
         foreach(ListViewItem lvi in _lvStatistics.Items) {
            if( CreateKeyTermFormListViewItem(lvi).ToUpper().Contains(searchTerm) ) {
               entries.Add(new StatisticsFindLocation(lvi, this));
            }
         }
         return entries;
      }

      public string ToolTipText {
         get {
            return "Displays simple statistics for the last test run.";
         }
      }

      public void NavigateTo(FindLocation findLocation) {
         StatisticsFindLocation location = findLocation as StatisticsFindLocation;
         if( location != null ) {
            _lvStatistics.SelectedItems.Clear();
            location.Item.Selected = true;
            _lvStatistics.EnsureVisible(location.Item.Index);
         }
      }

      private static string CreateKeyTermFormListViewItem(ListViewItem lvi) {
         return lvi.Text + "." + lvi.SubItems[1].Text;
      }

      public void UpdateCheckedItems(UiElementInfoCollection checkedItems) {
      }

      private class StatisticsFindLocation : FindLocation {
         public StatisticsFindLocation(ListViewItem lvi, IContentControl ctrl) 
            : base(CreateKeyTermFormListViewItem(lvi), ctrl) {
            _listViewItem = lvi;
         }
         public ListViewItem Item {
            get {
               return _listViewItem;
            }
         }
         private readonly ListViewItem _listViewItem = null;
      }

      #endregion

      #region Event Registration and Handling
      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         Reset(_avgTimePerTestUnknown, _estimatedOverheadUnknown);
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         _startTime = DateTime.Now;
         Reset(_waitingForTestRunToFinish, _waitingForTestRunToFinish);
      }

      private void Reset(string avgTimeText, string overheadTimeText) {
         _lvStatistics.Items.Clear();
         _totalTestTime = 0;
         _testCount = 0;
         _overheadTimeLabel.Text = avgTimeText;
         _averageTimeLabel.Text = overheadTimeText;
      }

      private void OnRecipeFinished(object sender, RecipeEventArgs args) {
         if(InvokeRequired) {
            Invoke(new RecipeEventHandler(this.OnRecipeFinished),
               new object[] { sender, args });
         }
         else {
            UpdatePercentages();
            TimeSpan overallDuration = DateTime.Now - _startTime;
            double estimatedOverhead = overallDuration.TotalMilliseconds - _totalTestTime / (1000 * 1000);
            double percentage = 100 * estimatedOverhead / overallDuration.TotalMilliseconds;
            _overheadTimeLabel.Text = String.Format(
               _estimatedOverheadFormat, estimatedOverhead.ToString("N2"),
               percentage.ToString("N2"));
            double averageTimePerTest = ((double)_totalTestTime) / _testCount / (1000 * 1000);
            _averageTimeLabel.Text = String.Format(
               _averageTestTimeFormat, averageTimePerTest.ToString("N2"),
               (1000.0d / averageTimePerTest).ToString("N2"));
         }
      }

      private void HookupRecipe() {
         RecipeFactory.Current.Started += _onRecipeStarted;
         RecipeFactory.Current.Finished += _onRecipeFinished;
         RecipeFactory.Current.AssemblyAdded += _onTestAssemblyAdded;
         RecipeFactory.Current.AssemblyRemoving += _onTestAssemblyRemoving;
         foreach(ITestAssembly ta in RecipeFactory.Current.Assemblies) {
            HookupTestAssembly(ta);
         }
      }

      private void HookupTestAssembly(ITestAssembly ta) {
         ta.TestError += _onTestEvent;
         ta.TestFailed += _onTestEvent;
         ta.TestPassed += _onTestEvent;
      }

      private void UnhookTestAssembly(ITestAssembly ta) {
         ta.TestError -= _onTestEvent;
         ta.TestFailed -= _onTestEvent;
         ta.TestPassed -= _onTestEvent;
      }

      void OnTestAssemblyAdded(object sender, AssemblyEventArgs args) {
         HookupTestAssembly(RecipeFactory.Current[args.PathFileName]);
      }

      void OnTestAssemblyRemoving(object sender, AssemblyEventArgs args) {
         UnhookTestAssembly(RecipeFactory.Current[args.PathFileName]);
      }

      void OnTestEvent(object sender, TestResultEventArgs args) {
         UpdateStatistics(args.ClassName, args.MethodName, args.Duration, args.AssertCount);
      }
      #endregion // Event Registration and Handling

      private void UpdatePercentages() {
         if( InvokeRequired ) {
            Invoke(new MethodInvoker(UpdatePercentages));
         }
         else {
            int columnIndex = _PercStatColumn.Index;
            for( int i = 0; i < _lvStatistics.Items.Count; i++ ) {
               ulong duration = (ulong) _lvStatistics.Items[i].Tag;
               double percentage = _totalTestTime > 0.0 ? 100.0d * duration / _totalTestTime : 0.0;
               _lvStatistics.Items[i].SubItems[columnIndex].Text = percentage.ToString("0.00");
            }
         }
      }

      private void UpdateStatistics(string className, string methodName, ulong nanoSeconds, int assertCount) {
         if( InvokeRequired ) {
            Invoke(new UpdateStatisticsMethodDelegate(UpdateStatistics), new object[] { className, methodName, nanoSeconds, assertCount });
         }
         else {
            ListViewItem lvi = new ListViewItem();

            lvi.Text = className;
            lvi.SubItems.Add(methodName);
            double duration = nanoSeconds;
            lvi.SubItems.Add(( duration / 1000000 ).ToString("0.000"));
            lvi.SubItems.Add("1.00");
            lvi.SubItems.Add(assertCount.ToString());

            lvi.Tag = nanoSeconds;

            _lvStatistics.Items.Add(lvi);

            _totalTestTime += nanoSeconds;
            _testCount++;
         }
      }

      private delegate void UpdateStatisticsMethodDelegate(string className, string methodName, ulong milliSeconds, int numAsserts);

      private void _lvStatistics_ColumnClick(object sender, ColumnClickEventArgs e) {
         ((StatisticsListSorter)_lvStatistics.ListViewItemSorter).SortColumn = e.Column;
         _lvStatistics.Sort();
      }

      #region Private Fields

      private const string _avgTimePerTestUnknown = "Average time per test unknown.";
      private const string _estimatedOverheadUnknown = "Estimated overhead unknown.";

      private readonly string _waitingForTestRunToFinish = "Waiting for test run to finish...";
      private readonly string _averageTestTimeFormat;
      private readonly string _estimatedOverheadFormat;

      private ulong _totalTestTime = 0; // nanoseconds
      private int _testCount = 0;
      private DateTime _startTime;

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeStarted;
      private readonly RecipeEventHandler _onRecipeFinished;

      private readonly AssemblyEventHandler _onTestAssemblyAdded;
      private readonly AssemblyEventHandler _onTestAssemblyRemoving;

      private readonly TestEventHandler _onTestEvent;

      #endregion
   }
}
