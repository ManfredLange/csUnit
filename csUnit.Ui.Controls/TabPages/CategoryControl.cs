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
   internal partial class CategoryControl : UserControl, IContentControl {
      #region Constructors

      public CategoryControl() {
         InitializeComponent();
         Name = "Categories";

         _categorySelector.Modified += SelectorModified;

         RecipeFactory.Loaded += RecipeLoaded;
         if(RecipeFactory.Current != null) {
            RefreshCategoriesList();
         }
         else {
            if(!DesignMode) {
               _categoriesListView.Items.Clear();
            }
         }
      }

      #endregion

      #region IContentControl Members

      public event TreeViewEventHandler AfterSelect;

#pragma warning disable 67
      public event CsUnitControlEventHandler FillContextMenu;

      public event ContentControlEventHandler ItemCheckedStateChanged;
#pragma warning restore 67

      public int DesiredTabPosition {
         get {
            return 1;
         }
      }

      private class CategoryFindLocation : FindLocation {
         public CategoryFindLocation(ListViewItem lvi, IContentControl ctrl)
            : base(lvi.Text, ctrl) {
            _listViewItem = lvi;
         }
         public ListViewItem Item {
            get {
               return _listViewItem;
            }
         }
         private readonly ListViewItem _listViewItem;
      }

      public List<FindLocation> Find(string searchTerm) {
         var entries = new List<FindLocation>();
         foreach(ListViewItem lvi in _categoriesListView.Items) {
            if( lvi.Text.ToUpper().Contains(searchTerm) ) {
               entries.Add(new CategoryFindLocation(lvi, this));
            }
         }
         return entries;
      }

      public string ToolTipText {
         get {
            return "Choose from test categories to include/exclude from running";
         }
      }

      public void NavigateTo(FindLocation findLocation) {
         var location = findLocation as CategoryFindLocation;
         if( location != null ) {
            _categoriesListView.SelectedItems.Clear();
            location.Item.Selected = true;
            _categoriesListView.EnsureVisible(location.Item.Index);
         }
      }

      public void UpdateCheckedItems(UiElementInfoCollection checkedItems) {
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Gets a IncludedCategories object with the selected categories. Can be empty.
      /// </summary>
      public Categories IncludedCategories {
         get {
            var cats = new Categories();
            for(var i = 0; i < _categoriesListView.Items.Count; i++) {
               if(_categoriesListView.Items[i].SubItems[1].Text == "Include") {
                  cats.Add(_categoriesListView.Items[i].Text);
               }
            }
            return cats;
         }
      }

      public Categories ExcludedCategories {
         get {
            var cats = new Categories();
            for(var i = 0; i < _categoriesListView.Items.Count; i++) {
               if(_categoriesListView.Items[i].SubItems[1].Text == "Exclude") {
                  cats.Add(_categoriesListView.Items[i].Text);
               }
            }
            return cats;
         }
      }

      /// <summary>
      /// Gets an object with the actual selector implementation for this object.
      /// </summary>
      /// <remarks>The returned object is not necessarily equal to the 
      /// FilterControl object.</remarks>
      public ISelector Filter {
         get {
            return _categorySelector;
         }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Refreshes the list of categories. Selections survive this operation.
      /// </summary>
      /// <remarks>This method is called when the model has changed and the view
      /// needs updating.</remarks>
      public void RefreshCategoriesList() {
         if( _categoriesListView.InvokeRequired) {
            _categoriesListView.Invoke(new MethodInvoker(RefreshCategoriesList));
         }
         else {
            _categoriesListView.Items.Clear();
            foreach(var category in RecipeFactory.Current.Categories) {
               var lvi = new ListViewItem(category);

               if(_categorySelector.IncludedCategories.Contains(category)) {
                  lvi.SubItems.Add("Include");
               }
               else if(_categorySelector.ExcludedCategories.Contains(category)) {
                  lvi.SubItems.Add("Exclude");
               }
               else {
                  lvi.SubItems.Add("Don't care");
               }
               _categoriesListView.Items.Add(lvi);
            }
         }
      }

      /// <summary>
      /// Clears both collections, included and excluded categories.
      /// </summary>
      public void ResetAllCategories() {
         for(var index = 0; index < _categoriesListView.Items.Count; index++) {
            _resetButton_Click(this, new EventArgs());
            FireSelectionChanged();
         }
      }

      /// <summary>
      /// Selects one category. Does not unselect other categories. Calling this
      /// method a second time for the same category has no effect.
      /// </summary>
      /// <param name="category">Name of the category.</param>
      public void IncludeCategory(string category) {
         var index = FindListBoxItemIndexForCategory(category);
         if(index > -1) {
            _categorySelector.IncludedCategories.Add(_categoriesListView.Items[index].Text);
            _categorySelector.ExcludedCategories.Remove(_categoriesListView.Items[index].Text);
            FireSelectionChanged();
         }
      }

      public void ExcludeCategory(string category) {
         var index = FindListBoxItemIndexForCategory(category);
         if(index > -1) {
            _categorySelector.ExcludedCategories.Add(_categoriesListView.Items[index].Text);
            _categorySelector.IncludedCategories.Remove(_categoriesListView.Items[index].Text);
            FireSelectionChanged();
         }
      }

      #endregion

      #region Private Methods

      private void HookupRecipe() {
         RecipeFactory.Current.RegisterSelector(_categorySelector);
         RecipeFactory.Current.Started += CurrentStarted;
         RecipeFactory.Current.AssemblyAdded += CurrentAssemblyAdded;
         RecipeFactory.Current.AssemblyRemoved += CurrentAssemblyRemoved;
         foreach(var ta in RecipeFactory.Current.Assemblies) {
            ta.AssemblyLoaded += TestAssemblyLoaded;
         }
      }

      private static void CurrentStarted(object sender, RecipeEventArgs args) {
      }

      private void CurrentAssemblyRemoved(object sender, AssemblyEventArgs args) {
         RefreshCategoriesList();
      }

      private void CurrentAssemblyAdded(object sender, AssemblyEventArgs args) {
         RefreshCategoriesList();
      }

      private void TestAssemblyLoaded(object sender, AssemblyEventArgs args) {
         RefreshCategoriesList();
      }

      private void RecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         RefreshCategoriesList();
      }

      private void SelectorModified(object sender, EventArgs e) {
         if(!_bHandlingSelectorModifiedEvent) {
            try {
               _bHandlingSelectorModifiedEvent = true;
               foreach(ListViewItem lvi in _categoriesListView.Items) {
                  if(_categorySelector.IncludedCategories.Contains(lvi.Text)) {
                     lvi.SubItems[WhatColumnIndex].Text = "Include";
                  }
                  else if(_categorySelector.ExcludedCategories.Contains(lvi.Text)) {
                     lvi.SubItems[WhatColumnIndex].Text = "Exclude";
                  }
                  else {
                     lvi.SubItems[WhatColumnIndex].Text = "Don't care";
                  }
               }
               FireSelectionChanged();
            }
            finally {
               _bHandlingSelectorModifiedEvent = false;
            }
         }
      }

      private int FindListBoxItemIndexForCategory(string category) {
         for(var i = 0; i < _categoriesListView.Items.Count; i++) {
            if(_categoriesListView.Items[i].Text == category) {
               return i;
            }
         }
         return -1;
      }

      private void FireSelectionChanged() {
         if(AfterSelect != null) {
            AfterSelect(this, null);
         }
      }

      private void _includeCategoryButton_Click(object sender, EventArgs e) {
         foreach(ListViewItem item in _categoriesListView.SelectedItems) {
            if(item.Selected) {
               _categorySelector.IncludedCategories.Add(item.Text);
               _categorySelector.ExcludedCategories.Remove(item.Text);
            }
         }
      }

      private void _dontCareButton_Click(object sender, EventArgs e) {
         foreach(ListViewItem item in _categoriesListView.SelectedItems) {
            if(item.Selected) {
               _categorySelector.IncludedCategories.Remove(item.Text);
               _categorySelector.ExcludedCategories.Remove(item.Text);
            }
         }
      }

      private void _excludeCategoryButton_Click(object sender, EventArgs e) {
         foreach(ListViewItem item in _categoriesListView.SelectedItems) {
            if(item.Selected) {
               _categorySelector.ExcludedCategories.Add(item.Text);
               _categorySelector.IncludedCategories.Remove(item.Text);
            }
         }
      }

      private void _resetButton_Click(object sender, EventArgs e) {
         _categorySelector.IncludedCategories.Clear();
         _categorySelector.ExcludedCategories.Clear();
      }

      private void _categoriesListView_MouseUp(object sender, MouseEventArgs e) {
         var enabled = _includeCategoryButton.Enabled;
         if(enabled != (_categoriesListView.SelectedItems.Count > 0)) {
            _includeCategoryButton.Enabled = _categoriesListView.SelectedItems.Count > 0;
            _excludeCategoryButton.Enabled = _categoriesListView.SelectedItems.Count > 0;
            _dontCareButton.Enabled = _categoriesListView.SelectedItems.Count > 0;
         }
      }

      #endregion

      #region Private Fields

      private const int WhatColumnIndex = 1;

      private bool _bHandlingSelectorModifiedEvent;

      /// <summary>
      /// The object implementing the ISelector interface.
      /// </summary>
      private readonly CategorySelector _categorySelector = new CategorySelector();

      #endregion
   }
}
