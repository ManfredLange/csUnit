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

using System.Collections;
using System.Collections.Generic;

namespace csUnit.Ui.Controls {
   public class SmartTreeNodeCollection : IList<SmartTreeNode> {
      internal SmartTreeNodeCollection(SmartTreeNode parentNode, SmartTree ownerTree) {
         _parentNode = parentNode;
         _ownerTree = ownerTree;
      }

      /// <summary>
      /// Gets the zero based index of a node in the collection.
      /// </summary>
      /// <param name="nodeToSearchFor">The node to search for in the collection.</param>
      /// <returns>The index of the node if found. The value -1 if not found.</returns>
      public int IndexOf(SmartTreeNode nodeToSearchFor) {
         return _nodes.IndexOf(nodeToSearchFor);
      }

      /// <summary>
      /// Inserts a node at the specified index. If the index is less than the
      /// maximum index then all items with the same or a larger index will be
      /// moved by one position towards the end of the collection.
      /// </summary>
      /// <param name="indexToInsertAt">The index at which to insert the item.</param>
      /// <param name="newNode">The node to insert.</param>
      public void Insert(int indexToInsertAt, SmartTreeNode newNode) {
         _nodes.Insert(indexToInsertAt, newNode);
         UpdateLinks();
         UpdateOwner();
      }

      internal void SetOwnerTree(SmartTree ownerTree) {
         _ownerTree = ownerTree;
      }

      internal void SetParent(SmartTreeNode parentNode) {
         _parentNode = parentNode;
      }

      /// <summary>
      /// Removes the node at the specified index of the collection.
      /// </summary>
      /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0
      /// - or - when <paramref name="index"/> is equal or greater than <see cref="Count"/>.</exception>
      /// <param name="index">The zero-based index of the node to remove.</param>
      public void RemoveAt(int index) {
         _nodes.RemoveAt(index);
         UpdateLinks();
      }

      /// <summary>
      /// Gets or sets the tree node at the given index.
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      /// <remarks>Trying to add null (nothing in Visual Basic) has no effect.
      /// </remarks>
      public SmartTreeNode this[int index] {
         get {
            return _nodes[index];
         }
         set {
            if( value != null ) {
               _nodes[index] = value;
               UpdateLinks();
               UpdateOwner();
            }
         }
      }

      public void Add(SmartTreeNode item) {
         if( item != null ) {
            item.SetParent(_parentNode);
            _nodes.Add(item);
            UpdateLinks();
            UpdateOwner();
         }
      }

      public void AddRange(SmartTreeNode[] nodes) {
         foreach( SmartTreeNode node in nodes ) {
            _nodes.Add(node);
         }
         UpdateLinks();
         UpdateOwner();
      }

      private void UpdateLinks() {
         if(_nodes.Count > 0 ) {
            _nodes[0].SetPreviousNode(null);
            _nodes[_nodes.Count - 1].SetNextNode(null);
         }
         for(int i = 0; i < _nodes.Count - 1; i++) {
            _nodes[i].SetNextNode(_nodes[i + 1]);
            _nodes[i + 1].SetPreviousNode(_nodes[i]);
         }
         for(int i = 0; i < _nodes.Count; i++) {
            _nodes[i].SetParent(_parentNode);
         }
      }

      private void UpdateOwner() {
         foreach(SmartTreeNode node in _nodes) {
            node.SetTreeView(_ownerTree);
         }
      }

      public void Clear() {
         _nodes.Clear();
      }

      public bool Contains(SmartTreeNode item) {
         return _nodes.Contains(item);
      }

      public void CopyTo(SmartTreeNode[] array, int arrayIndex) {
         _nodes.CopyTo(array, arrayIndex);
      }

      public bool Remove(SmartTreeNode item) {
         bool removed = _nodes.Remove(item);
         if( removed ) {
            UpdateLinks();
            item.SetParent(null);
            item.SetPreviousNode(null);
            item.SetNextNode(null);
            item.SetTreeView(null);
         }
         return removed;
      }

      /// <summary>
      /// Gets the number of nodes actually contained in the Collection.
      /// </summary>
      public int Count {
         get {
            return _nodes.Count;
         }
      }

      public bool IsReadOnly {
         get {
            return false;
         }
      }

      IEnumerator<SmartTreeNode> IEnumerable<SmartTreeNode>.GetEnumerator() {
         return _nodes.GetEnumerator();
      }

      public IEnumerator GetEnumerator() {
         return ((IEnumerable<SmartTreeNode>) this).GetEnumerator();
      }

      private readonly List<SmartTreeNode> _nodes = new List<SmartTreeNode>();
      private SmartTreeNode _parentNode = null;
      private SmartTree _ownerTree = null;
   }
}
