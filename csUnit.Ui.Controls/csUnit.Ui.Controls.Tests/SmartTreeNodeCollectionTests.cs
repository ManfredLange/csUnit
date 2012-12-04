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

namespace csUnit.Ui.Controls.Tests {
   [TestFixture]
   public class SmartTreeNodeCollectionTests {
      [SetUp]
      public void Setup() {
         _ownerTree = new SmartTree();
         _rootNode = new SmartTreeNode("Root Node");
      }

      [Test]
      public void HasOwner() {
         _ownerTree.Nodes.Add(_rootNode);
         Assert.Equals(_ownerTree, _rootNode.TreeView);
      }

      [Test]
      public void ParentProperlySet() {
         SmartTreeNode child = new SmartTreeNode("Child node");
         _rootNode.Nodes.Add(child);
         Assert.Equals(_rootNode, child.Parent);
         Assert.Equals(1, _rootNode.Nodes.Count);
      }

      [Test]
      public void LinksSetProperly() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         Assert.Equals(last, first.NextNode);
         Assert.Equals(first, last.PreviousNode);
      }

      [Test]
      public void AddRange() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         SmartTreeNode[] nodes = new SmartTreeNode[]{first, last};
         _rootNode.Nodes.AddRange(nodes);
         Assert.Equals(2, _rootNode.Nodes.Count);
         Assert.Equals(last, first.NextNode);
         Assert.Equals(first, last.PreviousNode);
      }

      [Test]
      public void RemoveUpdatesLinks() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         _rootNode.Nodes.Remove(last);
         Assert.Null(first.NextNode);
         Assert.Null(first.PreviousNode);
      }

      [Test]
      public void LastNodeIsUnlinkedWhenRemoved() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         _rootNode.Nodes.Remove(last);
         Assert.Null(last.NextNode);
         Assert.Null(last.PreviousNode);
      }

      [Test]
      public void FirstNodeIsUnlinkedWhenRemoved() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         _rootNode.Nodes.Remove(first);
         Assert.Null(first.NextNode);
         Assert.Null(first.PreviousNode);
      }

      [Test]
      public void RemovedNodeHasNoOwnerTree() {
         _ownerTree.Nodes.Add(_rootNode);
         _ownerTree.Nodes.Remove(_rootNode);
         Assert.Null(_rootNode.TreeView);
      }

      [Test]
      public void RemovedNodeHasNoParent() {
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         _rootNode.Nodes.Remove(first);
         Assert.Null(first.Parent);
      }

      [Test]
      public void TryRemoveNull() {
         Assert.False(_ownerTree.Nodes.Remove(null));
      }

      [Test]
      public void TryAddingNull() {
         _ownerTree.Nodes.Add(null);
         Assert.Equals(0, _ownerTree.Nodes.Count);
      }

      [Test]
      public void AddNodeViaIndexer() {
         _ownerTree.Nodes.Add(_rootNode);
         SmartTreeNode replacement = new SmartTreeNode("Replacement");
         _ownerTree.Nodes[0] = replacement;
         Assert.Equals(_ownerTree, replacement.TreeView);
      }

      [Test]
      public void ReplaceWithNullViaIndexer() {
         _ownerTree.Nodes.Add(_rootNode);
         _ownerTree.Nodes[0] = null;
         Assert.Equals(_rootNode, _ownerTree.Nodes[0]);
      }

      [Test]
      public void ReplacingViaIndexerSetsParentAndOwner() {
         _ownerTree.Nodes.Add(_rootNode);
         Assert.Equals(_ownerTree, _rootNode.TreeView);
         SmartTreeNode first = new SmartTreeNode("First child");
         SmartTreeNode last = new SmartTreeNode("Last child");
         SmartTreeNode replacement = new SmartTreeNode("Replacement");
         _rootNode.Nodes.Add(first);
         _rootNode.Nodes.Add(last);
         _rootNode.Nodes[1] = replacement;
         Assert.Equals(_rootNode, replacement.Parent);
         Assert.Equals(_ownerTree, replacement.TreeView);
      }

      private SmartTree _ownerTree = null;
      private SmartTreeNode _rootNode = null;
   }
}
