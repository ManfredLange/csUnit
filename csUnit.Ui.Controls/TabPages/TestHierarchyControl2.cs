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

#if USE_NEW_TEST_TREE


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.TabPages {
   /// <summary>
   /// This is the main control for the graphical UI. It displays the contents
   /// of a recipe in a hierachical fashion. All test assemblies, their test
   /// fixtures, and all test methods are displayed in a tree view.
   /// </summary>
   /// <remarks>Please note that although this class is a UserControl, it will
   /// still be displayed within a tab page. The reason for this construct is
   /// that on one hand the control should remain editable in the graphical
   /// designer of VS, but on the other hand the tab control within the
   /// CsUnitControl should remain generic using reflection to populate the
   /// tab pages.</remarks>
   internal partial class TestHierarchyControl2 : UserControl, IContentControl {
      //public event SmartTreeEventHandler AfterSelect;
      public event TreeViewEventHandler AfterSelect;
      public event CsUnitControlEventHandler FillContextMenu;
      public event ContentControlEventHandler ItemCheckedStateChanged;
      
      /// <summary>
      /// Create a TestHierarchyControl.
      /// </summary>
      public TestHierarchyControl2() {
         InitializeComponent();
         Name = "Tests (New)";

         _checkedTestSelector.Modified += _checkedTestSelector_Modified;

         if( !DesignMode ) {
            _treeTestHierarchy.Nodes.Clear();
         }

         _treeTestHierarchy.MouseUp += this.OnMouseUp;
         _treeTestHierarchy.AfterSelect += this.OnAfterSelect;
         _treeTestHierarchy.DoubleClick += this.OnDoubleClick;

         _onRecipeLoaded = this.OnRecipeLoaded;
         _onRecipeStarted = this.OnRecipeStarted;
         _onRecipeClosing = this.OnRecipeClosing;

         _onAssemblyAdded = this.OnTestAssemblyAdded;
         _onAssemblyRemoving = this.OnTestAssemblyRemoving;
         _onAssemblyChanged = this.OnAssemblyChanged;

         _onTestPassed = this.OnTestPassed;
         _onTestFailed = this.OnTestFailed;
         _onTestError = this.OnTestError;
         _onTestSkipped = this.OnTestSkipped;

         _treeTestHierarchy.ImageList = _imageList;

         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
      }

      public void UpdateCheckedItems(UiElementInfoCollection checkedItems) {
         UncheckAllNodes();
         foreach(UiElementInfo info in checkedItems) {
            SmartTreeNode node = FindNodeFor(info);
            if( node != null ) {
               node.Checked = true;
            }
         }
      }

      private SmartTreeNode FindNodeFor(UiElementInfo info) {
         SmartTreeNode found = null;
         foreach(SmartTreeNode node in _treeTestHierarchy.Nodes) {
            found = SearchIn(node, info);
            if( found != null ) {
               break;
            }
         }
         return found;
      }

      private static SmartTreeNode SearchIn(SmartTreeNode node, UiElementInfo info) {
         UiElementInfo currentInfo = node.Tag as UiElementInfo;
         if(currentInfo != null
            && currentInfo == info ) {
            return node;
         }
         else {
            foreach(SmartTreeNode currentNode in node.Nodes) {
               SmartTreeNode found = SearchIn(currentNode, info);
               if( found != null ) {
                  return found;
               }
            }
         }
         return null;
      }

      private bool _bHandlingSelectorModifiedEvent = false;

      private void _checkedTestSelector_Modified(object sender, EventArgs e) {
         if(!_bHandlingSelectorModifiedEvent) {
            try {
               _bHandlingSelectorModifiedEvent = true;

               SetCheckedStatus(_treeTestHierarchy.Nodes);

               FireSelectionChanged();
            }
            finally {
               _bHandlingSelectorModifiedEvent = false;
            }
         }
      }

      private void SetCheckedStatus(SmartTreeNodeCollection nodes) {
         foreach(SmartTreeNode node in nodes) {
            UiElementInfo elementInfo = node.Tag as UiElementInfo;
            if(elementInfo != null) {
               node.Checked = _checkedTestSelector.Contains(elementInfo);
            }
            if(node.Nodes.Count > 0) {
               SetCheckedStatus(node.Nodes);
            }
         }
      }

      private void FireSelectionChanged() {
         if(AfterSelect != null) {
            AfterSelect(this, null);
         }
      }

      /// <summary>
      /// Gets the text to be displayed as a tooltip for the tab page containing
      /// the TestHierarchyControl.
      /// </summary>
      public string ToolTipText {
         get {
            return "Displays the hierarchy of all tests and results of test runs.";
         }
      }

      /// <summary>
      /// Gets the desired tab position for this control.
      /// </summary>
      public int DesiredTabPosition {
         get {
            return 5;
         }
      }

      /// <summary>
      /// Handler for the RecipeFactory.Loaded event.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Additional arguments.</param>
      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         HideSearchPanel();
      }

      /// <summary>
      /// Handler for the RecipeFactory.Closing event.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
         _treeTestHierarchy.Nodes.Clear();
      }

      /// <summary>
      /// Unregisters all event handlers from the current recipe.
      /// </summary>
      private void UnhookRecipe() {
         foreach(ITestAssembly ta in RecipeFactory.Current.Assemblies) {
            UnhookAssembly(ta);
         }

         RecipeFactory.Current.AssemblyAdded -= _onAssemblyAdded;
         RecipeFactory.Current.AssemblyRemoving -= _onAssemblyRemoving;

         RecipeFactory.Current.Started -= _onRecipeStarted;
      }

      /// <summary>
      /// Unregisters all event handlers from a test assembly.
      /// </summary>
      /// <param name="assembly">Test assembly from which to remove event 
      /// handlers.</param>
      private void UnhookAssembly(ITestAssembly assembly) {
         assembly.TestPassed -= _onTestPassed;
         assembly.TestError -= _onTestError;
         assembly.TestFailed -= _onTestFailed;
         assembly.TestSkipped -= _onTestSkipped;
         assembly.AssemblyChanged -= _onAssemblyChanged;
         assembly.AssemblyLoaded -= _onAssemblyChanged;

         RemoveNodesFor(assembly);
      }

      /// <summary>
      /// Registers all event handlers for the current recipe.
      /// </summary>
      private void HookupRecipe() {
         RecipeFactory.Current.RegisterSelector(_checkedTestSelector);
         RecipeFactory.Current.Started += _onRecipeStarted;

         RecipeFactory.Current.AssemblyAdded += _onAssemblyAdded;
         RecipeFactory.Current.AssemblyRemoving += _onAssemblyRemoving;

         foreach(ITestAssembly ta in RecipeFactory.Current.Assemblies) {
            HookupAssembly(ta);
         }
      }

      /// <summary>
      /// Registers all event handlers for a test assembly.
      /// </summary>
      /// <param name="assembly">Test assembly for which to register event
      /// handlers.</param>
      private void HookupAssembly(ITestAssembly assembly) {
         assembly.TestPassed += _onTestPassed;
         assembly.TestError += _onTestError;
         assembly.TestFailed += _onTestFailed;
         assembly.TestSkipped += _onTestSkipped;
         assembly.AssemblyChanged += _onAssemblyChanged;
         assembly.AssemblyLoaded += _onAssemblyChanged;

         CreateNodesFor(assembly, PositionAny);
      }

      //private static void EnsureTreeNodeVisible(TreeNode node) {
      //   TreeNode parent = node.Parent;
      //   while(parent != null) {
      //      parent.Expand();
      //      parent = parent.Parent;
      //   }
      //   node.EnsureVisible();
      //}

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         HideSearchPanel();
         Reset();
         //TreeNode selectedNode = _treeTestHierarchy.SelectedNode;
         //if( selectedNode != null ) {
         //   _treeTestHierarchy.SelectedNode = selectedNode;
         //   EnsureTreeNodeVisible(selectedNode);
         //}
      }

      private void Reset() {
         _treeTestHierarchy.BeginUpdate();

         List<SmartTreeNode> toBeRemoved = new List<SmartTreeNode>();
         foreach(SmartTreeNode node in _treeTestHierarchy.Nodes) {
            node.Collapse();
            Reset(node, toBeRemoved);
            node.BackColor = Constants.ColorReset;
            node.ImageIndex = node.SelectedImageIndex = 0;
         }

         foreach(SmartTreeNode node in toBeRemoved) {
            node.Remove();
         }

         _treeTestHierarchy.EndUpdate();
      }

      private static void Reset(SmartTreeNode node, List<SmartTreeNode> toBeRemoved) {
         foreach( SmartTreeNode child in node.Nodes ) {
            while(child.ImageIndex > 4) {
               child.ImageIndex -= 5;
            }
            child.SelectedImageIndex = child.ImageIndex;
            child.BackColor = Constants.ColorReset;
            child.Collapse();
            if(child.Tag is string
               && ((string)child.Tag).Equals("CommentNode")) {
               toBeRemoved.Add(child);
            }
            else {
               Reset(child, toBeRemoved);
            }
         }
      }

      private void CreateNodesFor(ITestAssembly ta, int assemblyNodePosition) {
         TestFixtureInfoCollection tests = ta.TestFixtureInfos;
         SmartTreeNode root = CreateOrFindAssemblyNode(ta, assemblyNodePosition);
         foreach(TestFixtureInfo tf in tests) {
            SmartTreeNode classNode = CreateOrFindClassNode(ta, root.Nodes,
               tf.FullName.Split(new char[] { '.', '+' }), 0);
            classNode.Tag = new UiElementInfo(ta, tf, null);
            classNode.ImageIndex = 2;
            foreach(TestMethodInfo tm in tf.TestMethods) {
               SmartTreeNode methodNode = CreateOrFindMethodNode(classNode.Nodes, tm);
               methodNode.Tag = new UiElementInfo(ta, tf, tm);
            }
         }
         TreeViewEventHandler temp = AfterSelect;
         AfterSelect = null;
         try {
            RestoreCheckState(root);
            SetCheckedStatus(_treeTestHierarchy.Nodes);
         }
         finally {
            AfterSelect = temp;
         }
         if(_config.AutoExpandTestHierarchy) {
            root.ExpandAll();
         }
      }

      private void RestoreCheckState(SmartTreeNode root) {
         if((root.Tag as UiElementInfo) != null) {
            UiElementInfo element = root.Tag as UiElementInfo;
            root.Checked = _checkedTestSelector.Contains(element);
            if(root.Checked) {
               root.EnsureVisible();
            }
         }
         foreach(SmartTreeNode node in root.Nodes ) {
            RestoreCheckState(node);
         }
      }

      private static SmartTreeNode CreateOrFindMethodNode(SmartTreeNodeCollection nodes, ITestMethodInfo tm) {
         foreach( SmartTreeNode node in nodes ) {
            if(node.Text == tm.Name) {
               return node;
            }
         }
         SmartTreeNode newNode = new SmartTreeNode(tm.Name);
         newNode.ImageIndex = 3;
         nodes.Add(newNode);
         return newNode;
      }

      private SmartTreeNode CreateOrFindAssemblyNode(ITestAssembly ta, int assemblyNodePosition) {
         string[] parts = ta.FullName.Split(',');
         string displayName = parts[0] + ", " + parts[1];
         string toolTipText = "Full Name: " + parts[0] + "\n"
            + parts[1].Trim() + "\n"
            + parts[2].Trim() + "\n"
            + parts[3].Trim() + "\n"
            + "Modified: " + ta.ModifiedTimeStamp;
         foreach( SmartTreeNode node in _treeTestHierarchy.Nodes ) {
            UiElementInfo uieleminfo = node.Tag as UiElementInfo;
            if(   uieleminfo != null
               && uieleminfo.AssemblyPathName == ta.PathName) {
               node.Text = displayName;
               node.ToolTipText = toolTipText;
               return node;
            }
         }
         SmartTreeNode assemblyNode = new SmartTreeNode(displayName);
         assemblyNode.Tag = new UiElementInfo(ta, null, null);
         assemblyNode.ToolTipText = toolTipText;
         assemblyNode.ImageIndex = 0;
         assemblyNode.HasCheckBox = true;
         if(assemblyNodePosition != PositionAny) {
            _treeTestHierarchy.Nodes.Insert(assemblyNodePosition, assemblyNode);
         }
         else {
            _treeTestHierarchy.Nodes.Add(assemblyNode);
         }
         return assemblyNode;
      }

      private static SmartTreeNode CreateOrFindClassNode(ITestAssembly ta, SmartTreeNodeCollection nodes,
         string[] classFullNameParts, int index) {
         foreach(SmartTreeNode node in nodes) {
            if(node.Text == classFullNameParts[index]) {
               index++;
               if(index < classFullNameParts.Length) {
                  return CreateOrFindClassNode(ta, node.Nodes, classFullNameParts, index);
               }
               else {
                  return node;
               }
            }
         }
         SmartTreeNode newNode = new SmartTreeNode(classFullNameParts[index]);
         newNode.ImageIndex = 1;
         newNode.Tag = new UiElementInfo(ta, new TestSuite(classFullNameParts, index));
         nodes.Add(newNode);
         while(++index < classFullNameParts.Length) {
            SmartTreeNode child = new SmartTreeNode(classFullNameParts[index]);
            child.ImageIndex = 1;
            child.Tag = new UiElementInfo(ta, new TestSuite(classFullNameParts, index));
            newNode.Nodes.Add(child);
            newNode = child;
         }
         newNode.ImageIndex = 2;
         newNode.Tag = null;
         return newNode;
      }

      private void RemoveNodesFor(ITestAssembly ta) {
         SmartTreeNode assemblyNode = FindAssemblyNode(ta);

         if(assemblyNode != null) {
            for(int i = 0; i < _treeTestHierarchy.Nodes.Count; i++) {
               if(assemblyNode.Equals(_treeTestHierarchy.Nodes[i])) {
                  break;
               }
            }
            assemblyNode.Remove();
         }

         return;
      }

      private void OnTestPassed(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(this.OnTestPassedAsync), new object[] { sender, args });
         }
         else {
            OnTestPassedAsync(sender, args);
         }
      }

      private void OnTestPassedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            SmartTreeNode assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            SmartTreeNode node = FindTestMethodNode(assemblyNode.Nodes, args.ClassName + "." + args.MethodName);
            SetNodeSuccess(ref node);
            if(node.IsVisible) {
               node.EnsureVisible();
            }
            else {
               SmartTreeNode parent = node.Parent;
               while(parent != null) {
                  if(parent.IsVisible) {
                     parent.EnsureVisible();
                     break;
                  }
                  else {
                     parent = parent.Parent;
                  }
               }
            }
         }
      }

      private void OnTestFailed(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(this.OnTestFailedAsync), new object[] { sender, args });
         }
         else {
            OnTestFailedAsync(sender, args);
         }
      }

      private void OnTestFailedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            SmartTreeNode assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            SmartTreeNode node = FindTestMethodNode(assemblyNode.Nodes, args.ClassName + "." + args.MethodName);
            if(node == null) {
               node = FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
            }
            AddCommentNode(ref node, string.Format("File {0}, Line {1}",
               args.FileName.Substring(args.FileName.LastIndexOf("\\") + 1),
               args.LineNumber));
            SmartTreeNode commentNode = AddCommentNode(ref node, "Failure: " + args.Reason);
            SetNodeFailed(ref node, Constants.ColorFailed);
            if(_config.ExpandCommentNodes) {
               commentNode.ExpandAll();
               commentNode.LastNode.EnsureVisible();
            }
            else {
               node.EnsureVisible();
            }
         }
      }

      private static void InsertNode(ref SmartTreeNode parent, int index, ref SmartTreeNode child) {
         parent.Nodes.Insert(index, child);
         child.ImageIndex = child.SelectedImageIndex = 4;
      }

      /// <summary>
      /// Sets the background color of a node to green. If any of the parent
      /// nodes has no background color yet, their color is set to green as
      /// well.
      /// </summary>
      /// <param name="node">The node to start with.</param>
      private static void SetNodeSuccess(ref SmartTreeNode node) {
         if(node.ImageIndex < 5) {
            node.ImageIndex += 5;
         }
         node.SelectedImageIndex = node.ImageIndex;
         if(node.Parent != null) {
            SmartTreeNode parent = node.Parent;
            SetNodeSuccess(ref parent);
         }
         if(   RecipeFactory.Current.TestRunKind == TestRunKind.RunChecked
            && node.Checked ) {
            node.EnsureVisible();
         }
      }

      /// <summary>
      /// Sets the background color of a node to 'newColor'. Then sets the
      /// background color of all parents recursively to the same 'newColor' as
      /// well.
      /// </summary>
      /// <param name="node">The node to start with.</param>
      /// <param name="newColor">The new background color.</param>
      private static void SetNodeError(ref SmartTreeNode node, Color newColor) {
         while(node.ImageIndex < 20) {
            node.ImageIndex += 5;
         }
         node.SelectedImageIndex = node.ImageIndex;
         if(node.Parent != null) {
            SmartTreeNode parent = node.Parent;
            SetNodeError(ref parent, newColor);
         }
         node.Expand();
      }

      /// <summary>
      /// Sets the background color of a node to 'newColor'. Then sets the
      /// background color of all parents recursively to the same 'newColor' as
      /// well.
      /// </summary>
      /// <param name="node">The node to start with.</param>
      /// <param name="newColor">The new background color.</param>
      private static void SetNodeFailed(ref SmartTreeNode node, Color newColor) {
         while(node.ImageIndex < 15) {
            node.ImageIndex += 5;
         }
         node.SelectedImageIndex = node.ImageIndex;
         if(node.Parent != null) {
            SmartTreeNode parent = node.Parent;
            SetNodeFailed(ref parent, newColor);
         }
         node.Expand();
      }

      /// <summary>
      /// Sets the background color of a node to blue. Then sets the background
      /// color of all parents recursively to blue, if they have not been set
      /// yet to yellow or red.
      /// </summary>
      /// <param name="node">The node to start with.</param>
      private static void SetNodeSkipped(SmartTreeNode node) {
         while(node.ImageIndex < 10) {
            node.ImageIndex += 5;
         }
         node.SelectedImageIndex = node.ImageIndex;
         if(node.Parent != null) {
            SmartTreeNode parent = node.Parent;
            SetNodeSkipped(parent);
         }
         node.Expand();
      }

      private SmartTreeNode FindAssemblyNode(ITestAssembly ta) {
         foreach(SmartTreeNode rootNode in _treeTestHierarchy.Nodes) {
            UiElementInfo info = rootNode.Tag as UiElementInfo;
            if(   info != null
               && info.AssemblyPathName == ta.PathName) {
               return rootNode;
            }
         }
         return null;
      }

      private void OnTestError(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(this.OnTestErrorAsync), new object[] { sender, args });
         }
         else {
            OnTestErrorAsync(sender, args);
         }
      }

      private void OnTestErrorAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            SmartTreeNode assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            SmartTreeNode node = FindTestMethodNode(assemblyNode.Nodes, args.ClassName + "." + args.MethodName);
            if(node == null) {
               node = FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
            }
            AddCommentNode(ref node, string.Format( "File {0}, Line {1}",
               args.FileName.Substring(args.FileName.LastIndexOf("\\") + 1),
               args.LineNumber));
            SmartTreeNode commentNode = AddCommentNode(ref node, "Error: " + args.Reason);
            SetNodeError(ref node, Constants.ColorError);
            if(_config.ExpandCommentNodes) {
               commentNode.ExpandAll();
               commentNode.LastNode.EnsureVisible();
            }
            else {
               node.EnsureVisible();
            }
         }
      }

      private static SmartTreeNode FindTestMethodNode(SmartTreeNodeCollection nodes, string methodFullName) {
         foreach(SmartTreeNode node in nodes) {
            if(node.Tag is UiElementInfo) {
               UiElementInfo info = (UiElementInfo)node.Tag;
               if(info.IsMethodItem
                  && info.MethodFullName == methodFullName) {
                  return node;
               }
               else {
                  SmartTreeNode child = FindTestMethodNode(node.Nodes, methodFullName);
                  if(child != null) {
                     return child;
                  }
               }
            }
         }
         return null;
      }

      private static SmartTreeNode FindTestFixtureNode(SmartTreeNodeCollection nodes, string fixtureName) {
         foreach(SmartTreeNode node in nodes) {
            if(node.Tag is UiElementInfo) {
               UiElementInfo info = (UiElementInfo)node.Tag;
               if(info.IsFixtureItem
                  && info.FixtureName == fixtureName) {
                  return node;
               }
               else {
                  SmartTreeNode child = FindTestFixtureNode(node.Nodes, fixtureName);
                  if(child != null) {
                     return child;
                  }
               }
            }
         }
         return null;
      }

      private void OnTestSkipped(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(this.OnTestSkippedAsync), new object[] { sender, args });
         }
         else {
            OnTestSkippedAsync(sender, args);
         }
      }

      private void OnTestSkippedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            SmartTreeNode assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            SmartTreeNode node = FindTestMethodNode(assemblyNode.Nodes, args.ClassName + "." + args.MethodName);
            if(node == null) {
               node = FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
               foreach(SmartTreeNode child in node.Nodes) {
                  if(child.Tag is UiElementInfo) {
                     SetNodeSkipped(child);
                  }
               }
            }
            SetNodeSkipped(node);
            AddCommentNode(ref node, "Skip reason: " + args.Reason).EnsureVisible();
         }
      }

      private void OnTestAssemblyAdded(object sender, AssemblyEventArgs args) {
         HookupAssembly(RecipeFactory.Current[args.PathFileName]);
         HideSearchPanel();
      }

      private void OnTestAssemblyRemoving(object sender, AssemblyEventArgs args) {
         UnhookAssembly(RecipeFactory.Current[args.PathFileName]);
      }

      /// <summary>
      /// Called, when the selection state of a node in the test hierarchy tree
      /// has changed.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      private void OnAfterSelect(object sender, SmartTreeEventHandlerArgs args) {
         foreach(SmartTreeNode node in _treeTestHierarchy.SelectedNodes) {
            node.SelectedImageIndex = node.ImageIndex;
            if(AfterSelect != null) {
               // Need to map the behavior.
               TreeViewEventArgs newArgs = new TreeViewEventArgs(null, TreeViewAction.Unknown);
               AfterSelect(sender, newArgs);
            }
         }
      }

      private void OnMouseUp(object sender, MouseEventArgs args) {
         if(args.Button == MouseButtons.Right) {
            SmartTreeNode node = _treeTestHierarchy.GetNodeAt(args.X, args.Y);
            if( node != null ) {
               _treeTestHierarchy.SelectedNodes.Add(node);
               UiElementInfo uiElemInfo = node.Tag as UiElementInfo;
               if( uiElemInfo != null ) {
                  // Convert from tree coordinates to screen coordinates, and then back
                  // to form coordinates so that the context menu pops up at the right
                  // position
                  Point spot = PointToClient(_treeTestHierarchy.PointToScreen(new Point(args.X, args.Y)));

                  _contextMenuStrip = new ContextMenuStrip();

                  if( FillContextMenu != null ) {
                     FillContextMenu(this, new CsUnitControlEventArgs(_contextMenuStrip, uiElemInfo));
                  }

                  AppendMenuItemsForExpandCollapse();

                  _contextMenuStrip.Show(this, spot);
                  _contextMenuStrip = null;
               }
            }

            //_treeTestHierarchy.SelectedNode = _treeTestHierarchy.GetNodeAt(args.X, args.Y);
            //if(_treeTestHierarchy.SelectedNode != null
            //   && _treeTestHierarchy.SelectedNode.Tag != null
            //   && _treeTestHierarchy.SelectedNode.Tag.GetType() == typeof(UiElementInfo)) {
            //   UiElementInfo uiElemInfo = (UiElementInfo)_treeTestHierarchy.SelectedNode.Tag;

            //   // Convert from tree coordinates to screen coordinates, and then back
            //   // to form coordinates so that the context menu pops up at the right
            //   // position
            //   Point spot = PointToClient(_treeTestHierarchy.PointToScreen(new Point(args.X, args.Y)));

            //   _contextMenuStrip = new ContextMenuStrip();

            //   if(FillContextMenu != null) {
            //      FillContextMenu(this, new CsUnitControlEventArgs(_contextMenuStrip, uiElemInfo));
            //   }

            //   AppendMenuItemsForExpandCollapse();

            //   _contextMenuStrip.Show(this, spot);
            //   _contextMenuStrip = null;
            //}
         }
      }

      private void AppendMenuItemsForExpandCollapse() {
         _contextMenuStrip.Items.Add("-");
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Expand", null, new EventHandler(this.OnExpand)));
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Collapse", null, new EventHandler(this.OnCollapse)));
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Expand All", null, new EventHandler(this.OnExpandAll)));
      }

      private void OnExpand(object sender, EventArgs args) {
         // TODO: implement this method [17apr08, ml]
         //_treeTestHierarchy.SelectedNode.Expand();
      }

      private void OnCollapse(object sender, EventArgs args) {
         // TODO: implement this method [17apr08, ml]
         //_treeTestHierarchy.SelectedNode.Collapse();
      }

      private void OnExpandAll(object sender, EventArgs args) {
         // TODO: implement this method [17apr08, ml]
         //_treeTestHierarchy.SelectedNode.ExpandAll();
      }

      private void OnDoubleClick(object sender, EventArgs e) {
         //if(_treeTestHierarchy.SelectedNode != null) {
         //   string[] parts = _treeTestHierarchy.SelectedNode.Text.Split(new char[] { ' ' });
         //   string filePathName = string.Empty;
         //   for(int i = 1; i < parts.Length; i++) {
         //      filePathName += parts[i];
         //   }
         //   int lastColon = filePathName.LastIndexOf(":");
         //   if(lastColon > 0) {
         //      filePathName = filePathName.Substring(0, lastColon);
         //   }

         //   // TODO: the following must be mapped onto the IDE functionality.
         //   // See also feature request SF1599487. [27mar08, ml]
         //   //         if( Util.FileExists(filePathName, false) ) {
         //   //            ProcessStartInfo psi = new ProcessStartInfo("cmd", "/C " + filePathName);
         //   //            psi.WindowStyle = ProcessWindowStyle.Hidden;
         //   //            Process.Start(psi);
         //   //         }
         //}
      }

      private static SmartTreeNode AddCommentNode(ref SmartTreeNode node, string comment) {
         SmartTreeNode commentNode = new SmartTreeNode();
         commentNode.Tag = "CommentNode";
         if(comment.IndexOf(" in ") != -1
            && comment.IndexOf(" at ") != -1) {
            commentNode = AddCallStack(ref node, comment);
         }
         else {
            commentNode.Text = comment.Trim();
            InsertNode(ref node, 0, ref commentNode);
         }
         return commentNode;
      }

      private static SmartTreeNode AddCallStack(ref SmartTreeNode node, string comment) {
         int indexOfNewLine = comment.IndexOf('\n');
         SmartTreeNode tn = new SmartTreeNode(comment.Substring(0, indexOfNewLine).Trim());
         tn.Tag = "CommentNode";
         InsertNode(ref node, 0, ref tn);

         string modified = comment.Substring(indexOfNewLine).Replace(" in ", "\nin ");
         string[] parts = modified.Split(new char[] { '\n' });

         SmartTreeNode firstNode = node.FirstNode;
         for(int i = 1; i < parts.Length; i++) {
            SmartTreeNode commentPart = new SmartTreeNode(parts[i].Trim());
            InsertNode(ref firstNode, i, ref commentPart);
         }
         return tn;
      }

      private void OnAssemblyChanged(object sender, AssemblyEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new AssemblyEventHandler(this.OnAssemblyChangedAsync), new object[] { sender, args });
         }
         else {
            OnAssemblyChangedAsync(sender, args);
         }
      }

      private void OnAssemblyChangedAsync(object sender, AssemblyEventArgs args) {
         ITestAssembly ta = RecipeFactory.Current[args.PathFileName];
         int nodePosition = _treeTestHierarchy.Nodes.IndexOf(FindAssemblyNode(ta));
         RemoveNodesFor(ta);
         CreateNodesFor(ta, nodePosition);
      }

      private void OnAfterCheck(object sender, SmartTreeEventHandlerArgs args) {
         UiElementInfo elementInfo = args.Node.Tag as UiElementInfo;

         if( args.Action != TreeViewAction.Unknown ) {
            if(args.Node.Checked) {
               _checkedTestSelector.Add(elementInfo);
            }
            else {
               _checkedTestSelector.Remove(elementInfo);
            }
         }

         if(!_propagating) {
            try{
               _propagating = true;
               PropagateUp(args.Node.Parent);
               PropagateDown(args.Node);
            }
            finally {
               _propagating = false;
            }
         }
         FireSelectionChanged();
      }

      private bool _propagating = false;

      private static void PropagateUp(SmartTreeNode node) {
         if( node != null ) {
            foreach( SmartTreeNode child in node.Nodes ) {
               if( !child.Checked ) {
                  node.Checked = false;
                  PropagateUp(node.Parent);
                  return;
               }
            }
            node.Checked = true;
            PropagateUp(node.Parent);
         }
      }

      private static void PropagateDown(SmartTreeNode node) {
         foreach( SmartTreeNode child in node.Nodes ) {
            child.Checked = node.Checked;
            PropagateDown(child);
         }
      }

      private void _resetButton_Click(object sender, EventArgs e) {
         HideSearchPanel();
         UncheckAllNodes();
      }

      private void UncheckAllNodes() {
         foreach(SmartTreeNode node in _treeTestHierarchy.Nodes) {
            UncheckNode(node);
         }
      }

      private static void UncheckNode(SmartTreeNode node) {
         node.Checked = false;
         foreach( SmartTreeNode child in node.Nodes ) {
            UncheckNode(child);
         }
      }

      private void _searchButton_Click(object sender, EventArgs e) {
         _searchButton.Checked = !_searchButton.Checked;
         _searchPanel.Visible = _searchButton.Checked;
         _searchPanel.Enabled = _searchButton.Checked;
         //if( _searchButton.Checked ) {
         //   _searchPanel.SendToBack();
         //}
         //else {
         //   _searchPanel.BringToFront();
         //}

         if(_searchButton.Checked) {
            _searchPanel.Location = _treeTestHierarchy.Location;
            _searchPanel.Size = _treeTestHierarchy.Size;
            _searchPanel.BringToFront();
         }
         else {
            HideSearchPanel();
         }
      }

      private void HideSearchPanel() {
         _searchButton.Checked = _searchPanel.Visible = false;
         _searchPanel.Enabled = _searchButton.Checked;
//         _searchPanel.SendToBack();

         _searchPanel.Location = new Point(10000,10000);
         _searchPanel.SendToBack();
      }

      private void _btnGo_Click(object sender, EventArgs e) {
         // Walk the tree recursively and collect all results that match.
         _resultsList.Items.Clear();
         string searchTextLowerCase = _txtSearchString.Text.ToLower();
         Cursor _saved = Cursor.Current;
         try {
            Cursor.Current = Cursors.WaitCursor;
            _resultsList.BeginUpdate();
            foreach(SmartTreeNode node in (new TreeViewVisitor(_treeTestHierarchy))) {
               if (node.Text.ToLower().Contains(searchTextLowerCase)) {
                  _resultsList.Items.Add(node);
               }
            }
         }
         finally {
            _resultsList.EndUpdate();
            Cursor.Current = _saved;
         }
      }

      private void _resultsList_DoubleClick(object sender, EventArgs e) {
         SmartTreeNode selectedItem = _resultsList.SelectedItem as SmartTreeNode;
         if( selectedItem != null ) {
            //_treeTestHierarchy.SelectedNode = selectedItem;
            // TODO: implement this method [17apr08, ml]
            HideSearchPanel();
         }
      }

      private class TreeViewVisitor : IEnumerable<SmartTreeNode> {
         public TreeViewVisitor(SmartTree treeView) {
            CollectNodesFrom(treeView.Nodes);
         }

         private void CollectNodesFrom(SmartTreeNodeCollection nodes) {
            foreach( SmartTreeNode node in nodes ) {
               _nodes.Add(node);
               CollectNodesFrom(node.Nodes);
            }
         }

         #region IEnumerable<SmartTreeNode> Members

         public IEnumerator<SmartTreeNode> GetEnumerator() {
            return _nodes.GetEnumerator();
         }

         #endregion

         #region IEnumerable Members

         System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _nodes.GetEnumerator();
         }

         #endregion

         private readonly List<SmartTreeNode> _nodes = new List<SmartTreeNode>();
      }

      private const int PositionAny = -1;
      private SmartTree _treeTestHierarchy = null;

      private readonly ConfigCurrentUser _config = new ConfigCurrentUser();

      private ContextMenuStrip _contextMenuStrip = null;

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeStarted;
      private readonly RecipeEventHandler _onRecipeClosing;

      private readonly AssemblyEventHandler _onAssemblyAdded;
      private readonly AssemblyEventHandler _onAssemblyRemoving;
      private readonly AssemblyEventHandler _onAssemblyChanged;

      private readonly TestEventHandler _onTestPassed;
      private readonly TestEventHandler _onTestFailed;
      private readonly TestEventHandler _onTestError;
      private readonly TestEventHandler _onTestSkipped;

      private readonly CheckedTestsSelector _checkedTestSelector = new CheckedTestsSelector();
   }
}

#endif // DEBUG