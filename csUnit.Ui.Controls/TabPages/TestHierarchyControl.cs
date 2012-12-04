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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using csUnit.Common;
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
// ReSharper disable UnusedMember.Global
   internal partial class TestHierarchyControl : UserControl, IContentControl {
// ReSharper restore UnusedMember.Global
      /// <summary>
      /// Create a TestHierarchyControl.
      /// </summary>
      public TestHierarchyControl() {
         InitializeComponent();
         Name = "Tests";

         if(!DesignMode) {
            _treeTestHierarchy.Nodes.Clear();
         }

         _treeTestHierarchy.MouseUp += OnMouseUp;
         _treeTestHierarchy.AfterSelect += OnAfterSelect;
         _treeTestHierarchy.DoubleClick += OnDoubleClick;

         _onRecipeLoaded = OnRecipeLoaded;
         _onRecipeStarted = OnRecipeStarted;
         _onRecipeClosing = OnRecipeClosing;

         _onAssemblyAdded = OnTestAssemblyAdded;
         _onAssemblyRemoving = OnTestAssemblyRemoving;
         _onAssemblyChanged = OnAssemblyChanged;

         _onTestPassed = OnTestPassed;
         _onTestFailed = OnTestFailed;
         _onTestError = OnTestError;
         _onTestSkipped = OnTestSkipped;

         _treeTestHierarchy.ImageList = _imageList;

         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
      }

      #region ContentControl

      public event TreeViewEventHandler AfterSelect;
      public event CsUnitControlEventHandler FillContextMenu;
      public event ContentControlEventHandler ItemCheckedStateChanged;

      /// <summary>
      /// Gets the desired tab position for this control.
      /// </summary>
      public int DesiredTabPosition {
         get { return 0; }
      }

      public List<FindLocation> Find(string searchTerm) {
         var entries = new List<FindLocation>();
         foreach(var node in (new TreeViewVisitor(_treeTestHierarchy))) {
            if(node.Text.ToUpper().Contains(searchTerm)) {
               entries.Add(new TestHierarchyFindLocation(node, this));
            }
         }
         return entries;
      }

      /// <summary>
      /// Gets the text to be displayed as a tooltip for the tab page containing
      /// the TestHierarchyControl.
      /// </summary>
      public string ToolTipText {
         get {
            return
               "Displays the hierarchy of all tests and results of test runs.";
         }
      }

      public void NavigateTo(FindLocation findLocation) {
         var nie = findLocation as TestHierarchyFindLocation;
         if(nie != null) {
            _treeTestHierarchy.SelectedNode = nie.Node;
            if(nie.Node.Parent != null) {
               nie.Node.Parent.Expand();
            }
            nie.Node.EnsureVisible();
            _treeTestHierarchy.Focus();
         }
      }

      public void UpdateCheckedItems(UiElementInfoCollection checkedItems) {
         UncheckAllNodes();
         foreach(var info in checkedItems) {
            var node = FindNodeFor(info);
            if(node != null) {
               node.Checked = true;
            }
         }
      }

      private class TestHierarchyFindLocation : FindLocation {
         public TestHierarchyFindLocation(TreeNode node, IContentControl owner)
            : base(node.Text, owner) {
            _node = node;
         }

         public TreeNode Node {
            get { return _node; }
         }

         private readonly TreeNode _node;
      }

      private class TreeViewVisitor : IEnumerable<TreeNode> {
         public TreeViewVisitor(TreeView treeView) {
            CollectNodesFrom(treeView.Nodes);
         }

         private void CollectNodesFrom(TreeNodeCollection nodes) {
            foreach(TreeNode node in nodes) {
               _nodes.Add(node);
               CollectNodesFrom(node.Nodes);
            }
         }

         public IEnumerator<TreeNode> GetEnumerator() {
            return _nodes.GetEnumerator();
         }

         IEnumerator IEnumerable.GetEnumerator() {
            return _nodes.GetEnumerator();
         }

         private readonly List<TreeNode> _nodes = new List<TreeNode>();
      }

      #endregion

      private TreeNode FindNodeFor(UiElementInfo info) {
         TreeNode found = null;
         foreach(TreeNode node in _treeTestHierarchy.Nodes) {
            found = SearchIn(node, info);
            if(found != null) {
               break;
            }
         }
         return found;
      }

      private static TreeNode SearchIn(TreeNode node, UiElementInfo info) {
         var currentInfo = node.Tag as UiElementInfo;
         if(currentInfo != null
            && currentInfo == info) {
            return node;
         }
         foreach(TreeNode currentNode in node.Nodes) {
            var found = SearchIn(currentNode, info);
            if(found != null) {
               return found;
            }
         }
         return null;
      }

      private void FireCheckedStateChanged(UiElementInfo uiElementInfo,
                                           bool isChecked) {
         if(ItemCheckedStateChanged != null) {
            ItemCheckedStateChanged(this,
                                    new ContentControlEventArgs(uiElementInfo,
                                                                isChecked));
         }
      }

      private void FireSelectionChanged(TreeViewEventArgs args) {
         if(AfterSelect != null) {
            AfterSelect(this, args);
         }
      }

      /// <summary>
      /// Handler for the RecipeFactory.Loaded event.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Additional arguments.</param>
      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
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
         foreach(var ta in RecipeFactory.Current.Assemblies) {
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

         RemoveNodesFor(assembly, false);
      }

      /// <summary>
      /// Registers all event handlers for the current recipe.
      /// </summary>
      private void HookupRecipe() {
         RecipeFactory.Current.Started += _onRecipeStarted;
         RecipeFactory.Current.Finished += OnRecipeFinished;

         RecipeFactory.Current.AssemblyAdded += _onAssemblyAdded;
         RecipeFactory.Current.AssemblyRemoving += _onAssemblyRemoving;

         foreach(var ta in RecipeFactory.Current.Assemblies) {
            HookupAssembly(ta);
         }
      }

      private void OnRecipeFinished(object sender, RecipeEventArgs args) {
         Invoke(new RecipeEventHandler(OnRecipeFinishedAsync), sender, args);
      }

      private void OnRecipeFinishedAsync(object sender, RecipeEventArgs args) {
         if(_failureNodes.Count > 0) {
            EnsureTreeNodeVisible(_failureNodes[0]);
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

      private static void EnsureTreeNodeVisible(TreeNode node) {
         if(node.LastNode != null) {
            node = node.LastNode;
         }
         var parent = node.Parent;
         while(parent != null) {
            parent.Expand();
            parent = parent.Parent;
         }
         node.EnsureVisible();
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         var selectedNode = _treeTestHierarchy.SelectedNode;
         Reset();
         if(selectedNode != null) {
            _treeTestHierarchy.SelectedNode = selectedNode;
            EnsureTreeNodeVisible(selectedNode);
         }
      }

      private void Reset() {
         _failureNodes.Clear();
         _treeTestHierarchy.BeginUpdate();

         var toBeRemoved = new List<TreeNode>();
         foreach(TreeNode node in _treeTestHierarchy.Nodes) {
            node.Collapse();
            Reset(node, toBeRemoved);
            node.BackColor = Constants.ColorReset;
            node.ImageIndex = node.SelectedImageIndex = 0;
         }

         foreach(var node in toBeRemoved) {
            node.Remove();
         }

         _treeTestHierarchy.EndUpdate();
      }

      private static void Reset(TreeNode node, ICollection<TreeNode> toBeRemoved) {
         foreach(TreeNode child in node.Nodes) {
            while(child.ImageIndex > 4) {
               child.ImageIndex -= 5;
            }
            child.SelectedImageIndex = child.ImageIndex;
            child.BackColor = Constants.ColorReset;
            child.Collapse();
            if(child.Tag is string
               && ((string) child.Tag).Equals("CommentNode")) {
               toBeRemoved.Add(child);
            }
            else {
               Reset(child, toBeRemoved);
            }
         }
      }

      private void CreateNodesFor(ITestAssembly ta, int assemblyNodePosition) {
         var tests = ta.TestFixtureInfos;
         var root = CreateOrFindAssemblyNode(ta, assemblyNodePosition);
         foreach(TestFixtureInfo tf in tests) {
            var classNode = CreateOrFindClassNode(ta, root.Nodes,
                                                  tf.FullName.Split(new[] {'.', '+'}),
                                                  0);
            classNode.Tag = new UiElementInfo(ta, tf, null);
            classNode.ImageIndex = 2;
            foreach(TestMethodInfo tm in tf.TestMethods) {
               var methodNode = CreateOrFindMethodNode(classNode.Nodes, tm);
               methodNode.Tag = new UiElementInfo(ta, tf, tm);
            }
         }
         var temp = AfterSelect;
         AfterSelect = null;
         try {
            RestoreCheckState(root);
         }
         finally {
            AfterSelect = temp;
         }
         if(ConfigCurrentUser.AutoExpandTestHierarchy) {
            root.ExpandAll();
         }
      }

      private void RestoreCheckState(TreeNode root) {
         if ((root.Tag as UiElementInfo) != null) {
            var elementInfo = root.Tag as UiElementInfo;
            root.Checked = _checkedNodes.Contains(elementInfo.GetHashCode());
         }
         ApplyToAllNodes(root.Nodes, x => {
            var elementInfo = x.Tag as UiElementInfo;
            if (elementInfo != null) {
               x.Checked = _checkedNodes.Contains(elementInfo.GetHashCode());
            }
         });
      }

      private static TreeNode CreateOrFindMethodNode(TreeNodeCollection nodes,
                                                     ITestMethodInfo tm) {
         foreach(TreeNode node in nodes) {
            if(node.Text
               == tm.Name) {
               return node;
            }
         }
         var newNode = new TreeNode(tm.Name) {
            Tag = new UiElementInfo(null, null, tm),
            ImageIndex = 3
         };
         nodes.Add(newNode);
         return newNode;
      }

      private TreeNode CreateOrFindAssemblyNode(ITestAssembly ta,
                                                int assemblyNodePosition) {
         var parts = ta.Name.FullName.Split(',');
         var displayName = parts[0] + ", " + parts[1];
         var toolTipText = "Full Name: " + parts[0] + "\n"
                           + parts[1].Trim() + "\n"
                           + parts[2].Trim() + "\n"
                           + parts[3].Trim() + "\n"
                           + "Modified: " + ta.ModifiedTimeStamp;
         foreach(TreeNode node in _treeTestHierarchy.Nodes) {
            var uieleminfo = node.Tag as UiElementInfo;
            if(uieleminfo != null
               && uieleminfo.AssemblyPathName == ta.Name.CodeBase) {
               node.Text = displayName;
               node.ToolTipText = toolTipText;
               return node;
            }
         }
         var assemblyNode = new TreeNode(displayName) {
            Tag = new UiElementInfo(ta, null, null),
            ToolTipText = toolTipText,
            ImageIndex = 0
         };
         if(assemblyNodePosition != PositionAny) {
            _treeTestHierarchy.Nodes.Insert(assemblyNodePosition, assemblyNode);
         }
         else {
            _treeTestHierarchy.Nodes.Add(assemblyNode);
         }
         return assemblyNode;
      }

      private static TreeNode CreateOrFindClassNode(ITestAssembly ta,
                                                    TreeNodeCollection nodes,
                                                    string[] classFullNameParts,
                                                    int index) {
         foreach(TreeNode node in nodes) {
            if(node.Text
               == classFullNameParts[index]) {
               index++;
               if(index < classFullNameParts.Length) {
                  return CreateOrFindClassNode(ta, node.Nodes,
                                               classFullNameParts, index);
               }
               return node;
            }
         }
         var newNode = new TreeNode(classFullNameParts[index]) { 
            ImageIndex = 1, 
            Tag = new UiElementInfo(ta, new TestSuite(classFullNameParts, index))
         };
         nodes.Add(newNode);
         while(++index
               < classFullNameParts.Length) {
            var child = new TreeNode(classFullNameParts[index]) {
               ImageIndex = 1,
               Tag = new UiElementInfo(ta, new TestSuite(classFullNameParts,index))
            };
            newNode.Nodes.Add(child);
            newNode = child;
         }
         newNode.ImageIndex = 2;
         newNode.Tag = null;
         return newNode;
      }

      private delegate void NodeFunction(TreeNode node);

      private static void ApplyToAllNodes(TreeNodeCollection nodes, NodeFunction nodeFunction ) {
         foreach(TreeNode node in nodes) {
            ApplyToAllNodes(node.Nodes, nodeFunction);
            nodeFunction(node);
         }
      }

      private readonly Set<int> _checkedNodes = new Set<int>();

      private void RemoveNodesFor(ITestAssembly ta, bool saveCheckState) {
         var assemblyNode = FindAssemblyNode(ta);

         if (saveCheckState) {
            ApplyToAllNodes(assemblyNode.Nodes,
               x => {
                  var elementInfo = x.Tag as UiElementInfo;
                  if (elementInfo != null) {
                     if(x.Checked) {
                        _checkedNodes.Add(elementInfo.GetHashCode());
                     }
                     else {
                        _checkedNodes.Remove(elementInfo.GetHashCode());
                     }
                  }
               }
               );
         }
         else {
            ApplyToAllNodes(assemblyNode.Nodes,
               x => {
                  var elementInfo = x.Tag as UiElementInfo;
                  if( elementInfo != null ) {
                     _checkedNodes.Remove(elementInfo.GetHashCode());
                  }
               });
         }

         for(var i = 0; i < _treeTestHierarchy.Nodes.Count; i++) {
            if(assemblyNode.Equals(_treeTestHierarchy.Nodes[i])) {
               break;
            }
         }
         assemblyNode.Remove();

         return;
      }

      private void OnTestPassed(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(OnTestPassedAsync), new[] {sender, args});
         }
         else {
            OnTestPassedAsync(sender, args);
         }
      }

      private void OnTestPassedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            try {
               var assemblyNode = FindAssemblyNode(sender as ITestAssembly);
               var node = FindTestMethodNode(assemblyNode.Nodes,
                                             args.ClassName + "."
                                             + args.MethodName);
               SetNodeStatus(node, 5);
            }
            catch(NullReferenceException ex) {
               // TODO: Exception is thrown when a test is in herited from
               // another test fixture. [19-jul-09, ml]
               Debug.WriteLine(ex);
            }
         }
      }

      private void OnTestFailed(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(OnTestFailedAsync), new[] {sender, args});
         }
         else {
            OnTestFailedAsync(sender, args);
         }
      }

      private void OnTestFailedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            var assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            var node = FindTestMethodNode(assemblyNode.Nodes,
                                          args.ClassName + "." + args.MethodName)
                       ??
                       FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
            if( args.StackInfo != null ) {
               AddCallStack(node, args.StackInfo);
            }
            if(args.Failure != null) {
               if(args.Failure.HasTip) {
                  AddCommentNode(node, "Tip: " + args.Failure.Tip);
               }
               AddCommentNode(node, "Actual: " + args.Failure.Actual);
               AddCommentNode(node, "Expected: " + args.Failure.Expected);
               AddCommentNode(node, args.Failure.Message);
            }
            else {
               AddCommentNode(node, "Failure: " + args.Reason);
            }
            SetNodeStatus(node, 15);
            _failureNodes.Add(node);
            node.Expand();
         }
      }

      private static void InsertNode(TreeNode parent, int index, TreeNode child) {
         parent.Nodes.Insert(index, child);
         child.ImageIndex = child.SelectedImageIndex = 4;
      }

      /// <summary>
      /// Sets the status of a node, e.g. uses an icon with red background for 
      /// a failed test.
      /// </summary>
      /// <param name="node">The node to start with.</param>
      /// <param name="minimumImageIndex">Minimum image index.</param>
      private static void SetNodeStatus(TreeNode node, int minimumImageIndex) {
         try {
            while (node.ImageIndex < minimumImageIndex) {
               node.ImageIndex += 5;
            }
            node.SelectedImageIndex = node.ImageIndex;
            if (node.Parent != null) {
               var parent = node.Parent;
               SetNodeStatus(parent, minimumImageIndex);
            }

            if(ConfigCurrentUser.ExpandExecutedTestNodes) {
               EnsureTreeNodeVisible(node);
            }
         }
         catch(NullReferenceException ex) {
            // TODO: Exception is thrown when a test is inherited from
            // another test fixture. [19-jul-09, ml]
            Debug.WriteLine(ex);
         }
      }

      private TreeNode FindAssemblyNode(ITestAssembly ta) {
         foreach(TreeNode rootNode in _treeTestHierarchy.Nodes) {
            var info = rootNode.Tag as UiElementInfo;
            if(info != null
               && info.AssemblyPathName == ta.Name.CodeBase) {
               return rootNode;
            }
         }
         return null;
      }

      private void OnTestError(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(OnTestErrorAsync), new[] {sender, args});
         }
         else {
            OnTestErrorAsync(sender, args);
         }
      }

      private void OnTestErrorAsync(object sender, TestResultEventArgs args) {
         lock (this) {
            var assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            var node = FindTestMethodNode(assemblyNode.Nodes,
                                          args.ClassName + "." + args.MethodName)
                       ??
                       FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
            AddCallStack(node, args.StackInfo);
            AddCommentNode(node, "Error: " + args.Reason);
            SetNodeStatus(node, 20);
            _failureNodes.Add(node);
            node.Expand();
         }
      }

      private static TreeNode FindTestMethodNode(TreeNodeCollection nodes,
                                                 string methodFullName) {
         foreach(TreeNode node in nodes) {
            if(node.Tag is UiElementInfo) {
               var info = (UiElementInfo) node.Tag;
               if(info.IsMethodItem
                  && info.MethodFullName == methodFullName) {
                  return node;
               }
               var child = FindTestMethodNode(node.Nodes, methodFullName);
               if(child != null) {
                  return child;
               }
            }
         }
         return null;
      }

      private static TreeNode FindTestFixtureNode(TreeNodeCollection nodes,
                                                  string fixtureName) {
         foreach(TreeNode node in nodes) {
            if(node.Tag is UiElementInfo) {
               var info = (UiElementInfo) node.Tag;
               if(info.IsFixtureItem
                  && info.FixtureName == fixtureName) {
                  return node;
               }
               var child = FindTestFixtureNode(node.Nodes, fixtureName);
               if(child != null) {
                  return child;
               }
            }
         }
         return null;
      }

      private void OnTestSkipped(object sender, TestResultEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new TestEventHandler(OnTestSkippedAsync),
                   new[] {sender, args});
         }
         else {
            OnTestSkippedAsync(sender, args);
         }
      }

      private void OnTestSkippedAsync(object sender, TestResultEventArgs args) {
         lock(this) {
            var assemblyNode = FindAssemblyNode(sender as ITestAssembly);
            var node = FindTestMethodNode(assemblyNode.Nodes,
                                          args.ClassName + "." + args.MethodName)
                       ??
                       FindTestFixtureNode(assemblyNode.Nodes, args.ClassName);
            SetNodeStatus(node, 10);
            var commentNode = AddCommentNode(node, "Skip reason: " + args.Reason);
            if(ConfigCurrentUser.ExpandCommentNodes) {
               EnsureTreeNodeVisible(commentNode);
            }
         }
      }

      private void OnTestAssemblyAdded(object sender, AssemblyEventArgs args) {
         HookupAssembly(RecipeFactory.Current[args.PathFileName]);
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
      private void OnAfterSelect(object sender, TreeViewEventArgs args) {
         var node = _treeTestHierarchy.SelectedNode;
         node.SelectedImageIndex = node.ImageIndex;
         FireSelectionChanged(args);
      }

      private void OnMouseUp(object sender, MouseEventArgs args) {
         if(args.Button == MouseButtons.Right) {
            _treeTestHierarchy.SelectedNode = _treeTestHierarchy.GetNodeAt(args.X, args.Y);
            if(   _treeTestHierarchy.SelectedNode != null
               && _treeTestHierarchy.SelectedNode.Tag != null
               && _treeTestHierarchy.SelectedNode.Tag.GetType() == typeof(UiElementInfo)) {
               var uiElemInfo = (UiElementInfo) _treeTestHierarchy.SelectedNode.Tag;

               // Convert from tree coordinates to screen coordinates, and then back
               // to form coordinates so that the context menu pops up at the right
               // position
               var spot = PointToClient( _treeTestHierarchy.PointToScreen(new Point(args.X, args.Y)) );

               _contextMenuStrip = new ContextMenuStrip();

               if(FillContextMenu != null) {
                  FillContextMenu(this,
                                  new CsUnitControlEventArgs(_contextMenuStrip,
                                                             uiElemInfo));
               }

               AppendMenuItemsForExpandCollapse();

               _contextMenuStrip.Show(this, spot);
               _contextMenuStrip = null;
            }
         }
      }

      private void AppendMenuItemsForExpandCollapse() {
         _contextMenuStrip.Items.Add("-");
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Expand", null,
                                                           new EventHandler(
                                                              OnExpand)));
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Collapse", null,
                                                           new EventHandler(
                                                              OnCollapse)));
         _contextMenuStrip.Items.Add(new ToolStripMenuItem("Expand All", null,
                                                           new EventHandler(
                                                              OnExpandAll)));
      }

      private void OnExpand(object sender, EventArgs args) {
         _treeTestHierarchy.SelectedNode.Expand();
      }

      private void OnCollapse(object sender, EventArgs args) {
         _treeTestHierarchy.SelectedNode.Collapse();
      }

      private void OnExpandAll(object sender, EventArgs args) {
         _treeTestHierarchy.SelectedNode.ExpandAll();
      }

      private void OnDoubleClick(object sender, EventArgs e) {
         if(_treeTestHierarchy.SelectedNode != null) {
            var parts = _treeTestHierarchy.SelectedNode.Text.Split(new[] {' '});
            var filePathName = string.Empty;
            for(var i = 1; i < parts.Length; i++) {
               filePathName += parts[i];
            }
            var lastColon = filePathName.LastIndexOf(":");
            if(lastColon > 0) {
// ReSharper disable RedundantAssignment
               filePathName = filePathName.Substring(0, lastColon);
// ReSharper restore RedundantAssignment
               // TODO: the host should provide a mechanism for displaying the 
               // file in a window. [24mar09, ml]
            }
         }
      }

      private static TreeNode AddCommentNode(TreeNode node, string comment) {
         var commentNode = new TreeNode { Tag = "CommentNode", Text = comment.Trim() };
         InsertNode(node, 0, commentNode);
         return commentNode;
      }

      private static void AddCallStack(TreeNode node, ICollection<StackFrameInfo> stackFrameInfos) {
         if (stackFrameInfos != null) {
            var stackTreeNode =
               new TreeNode(string.Format("Call Stack ({0} frame{1})",
                                          stackFrameInfos.Count,
                                          stackFrameInfos.Count > 1 ? "s" : ""))
               {Tag = "CommentNode"};
            InsertNode(node, 0, stackTreeNode);
            stackTreeNode.ImageIndex = 25;
            foreach(var stackFrame in stackFrameInfos) {
               var stackFrameTreeNode = new TreeNode(stackFrame.ToString())
                                        {Tag = "CommentNode"};
               InsertNode(stackTreeNode, 0, stackFrameTreeNode);
            }
         }
         else {
            AddCommentNode(node, "No stack information available.");
         }
      }

      private void OnAssemblyChanged(object sender, AssemblyEventArgs args) {
         if(_treeTestHierarchy.InvokeRequired) {
            Invoke(new AssemblyEventHandler(OnAssemblyChangedAsync),
                   new[] { sender, args });
         }
         else {
            OnAssemblyChangedAsync(sender, args);
         }
      }

      private void OnAssemblyChangedAsync(object sender, AssemblyEventArgs args) {
         var ta = RecipeFactory.Current[args.PathFileName];
         var nodePosition =
            _treeTestHierarchy.Nodes.IndexOf(FindAssemblyNode(ta));
         RemoveNodesFor(ta, true);
         CreateNodesFor(ta, nodePosition);
      }

      private void OnAfterCheck(object sender, TreeViewEventArgs args) {
         if (args.Action == TreeViewAction.ByKeyboard
            || args.Action == TreeViewAction.ByMouse) {
            var elementInfo = args.Node.Tag as UiElementInfo;
            if(!_propagating) {
               try {
                  _propagating = true;
                  PropagateUp(args.Node.Parent);
                  PropagateDown(args.Node);
               }
               finally {
                  _propagating = false;
               }
            }
            FireCheckedStateChanged(elementInfo, args.Node.Checked);
         }
      }

      private bool _propagating;

      private static void PropagateUp(TreeNode node) {
         if(node != null) {
            foreach(TreeNode child in node.Nodes) {
               if(!child.Checked) {
                  node.Checked = false;
                  PropagateUp(node.Parent);
                  return;
               }
            }
            node.Checked = true;
            PropagateUp(node.Parent);
         }
      }

      private static void PropagateDown(TreeNode node) {
         foreach(TreeNode child in node.Nodes) {
            child.Checked = node.Checked;
            PropagateDown(child);
         }
      }

      private void UncheckAllNodes() {
         foreach(TreeNode node in _treeTestHierarchy.Nodes) {
            UncheckNode(node);
         }
      }

      private static void UncheckNode(TreeNode node) {
         node.Checked = false;
         foreach(TreeNode child in node.Nodes) {
            UncheckNode(child);
         }
      }

      private const int PositionAny = -1;
      private TreeView _treeTestHierarchy;

      private ContextMenuStrip _contextMenuStrip;

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

      private readonly List<TreeNode> _failureNodes = new List<TreeNode>();
   }
}
