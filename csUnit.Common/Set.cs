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
using System.Text;

namespace csUnit.Common {
   /// <summary>
   /// The generic Set class represents a non-ordered collection with no
   /// duplicate values.</summary>
   /// <typeparam name="T">Type of item in the set.</typeparam>
   /// <remarks>
   /// Adding the same value more than once does not produce duplicates in the
   /// set.
   /// The sequence in which elements are added has to influence in which an
   /// iterator on the set returns those elements.
   /// An iterator returns each element in the set exactly one, and in an
   /// arbitrary order.
   /// Furthermore, ach instance of an iterator may return the elements in the 
   /// set in a different order.
   /// This set implementation may not support all standard set operations. 
   /// Missing ones will only be added as needed by the csUnit project. Please 
   /// see also the associated unit tests for further information about what 
   /// should work, and what is not required yet. [28Apr2006, ml]
   /// </remarks>
   [Serializable]
   public class Set<T> : ICollection<T> {
      /// <summary>
      /// Returns an empty set.
      /// </summary>
      public static Set<T> EmptySet {
         get {
            return new Set<T>();
         }
      }

      /// <summary>
      /// Default constructor
      /// </summary>
      public Set() {
      }

      /// <summary>
      /// Copy constructor
      /// </summary>
      /// <param name="source">Source set.</param>
      /// <remarks>This creates a physical copy of the set.</remarks>
      public Set(Set<T> source) {
         foreach(T key in source._items.Keys) {
            _items.Add(key, null);
         }
      }

      /// <summary>
      /// Gets the number of elements in the set.
      /// </summary>
      public int Count {
         get {
            return _items.Count;
         }
      }

      /// <summary>
      /// Gets a boolean indicating whether the set is empty.
      /// </summary>
      public bool IsEmpty {
         get {
            return _items.Count == 0;
         }
      }

      /// <summary>
      /// Adds an item to the set. If the value is already in the set, the
      /// method simply returns. Adding null (nothing in VB) has no effect.
      /// </summary>
      /// <param name="item">The item to add.</param>
      public void Add(T item) {
         if( null == item ) {
            return;
         }
         if(!_items.ContainsKey(item)) {
            _items.Add(item, null);
         }
      }

      /// <summary>
      /// Adds an array of items to the set. If any of the values is already in
      /// the set, the item will not be added. If any of the elements in the
      /// array is null (nothing in VB) then these elements will not be added.
      /// </summary>
      /// <param name="items">An array of items to add.</param>
      public void Add(T[] items) {
         foreach(T item in items) {
            Add(item);
         }
      }

      /// <summary>
      /// Adds the elements of a different set. The resulting set is the join
      /// set of both sets before the call of this method.
      /// </summary>
      /// <param name="otherSet">The set with items to add.</param>
      /// <remarks>The resulting set will not have any duplicates.
      /// </remarks>
      public void Add(Set<T> otherSet) {
         foreach(T item in otherSet) {
            if(!_items.ContainsKey(item)) {
               _items.Add(item, null);
            }
         }
      }

      /// <summary>
      /// Removes all items from the set.
      /// </summary>
      public void Clear() {
         _items.Clear();
      }

      /// <summary>
      /// Determines whether a given item is in the set.
      /// </summary>
      /// <param name="item">Item to look for.</param>
      /// <returns>True, if the item is in the set, false otherwise.</returns>
      public bool Contains(T item) {
         return _items.ContainsKey(item);
      }

      /// <summary>
      /// Copies the elements of the Set to an array, starting at a particular
      /// array index.
      /// </summary>
      /// <param name="array">The one-dimentional array that is the destination
      /// of the elements copied from the Set. The array must have zero-based
      /// indexing.</param>
      /// <param name="arrayIndex">The zero-based index in array at which
      /// copying begins.</param>
      public void CopyTo(T[] array, int arrayIndex) {
         _items.Keys.CopyTo(array, arrayIndex);
      }

      /// <summary>
      /// Determines the intersection of two sets and returns it in a new 
      /// instance. The input parameters are not modified.
      /// </summary>
      /// <param name="otherSet">Second set to intersect.</param>
      /// <returns>A set containing the intersection.</returns>
      public Set<T> Intersect(Set<T> otherSet) {
         var intersection = new Set<T>();
         foreach(T item in this) {
            if(otherSet.Contains(item)) {
               intersection.Add(item);
            }
         }
         return intersection;
      }

      /// <summary>
      /// Gets a value indicating whether the Set is read-only.
      /// </summary>
      public bool IsReadOnly {
         get {
            return false;
         }
      }

      /// <summary>
      /// Determins the union of the two sets and returns it in a new instance.
      /// The input parameters are not modified.
      /// </summary>
      /// <param name="otherSet">Second set to add to the union.</param>
      /// <returns>A set containing the union.</returns>
      public Set<T> Union(Set<T> otherSet) {
         var union = new Set<T> {otherSet};
         foreach(T item in this) {
            union.Add(item);
         }
         return union;
      }

      /// <summary>
      /// Removes an item from the set. If the set does not contain the item,
      /// the method simply returns.
      /// </summary>
      /// <param name="item"></param>
      public bool Remove(T item) {
         _items.Remove(item);
         return true;
      }

      /// <summary>
      /// Converts the set to a fixed-size array.
      /// </summary>
      /// <returns>An arry of T's.</returns>
      public T[] ToArray() {
         var array = new T[_items.Count];
         var index = 0;
         foreach(var item in this) {
            array[index] = item;
            index++;
         }
         return array;
      }

      /// <summary>
      /// Compares two sets. The order within the sets is of no importance. Two
      /// sets are considered the same if they each element in one set is also
      /// an element in the other set.
      /// </summary>
      /// <param name="obj">Set to compare with.</param>
      /// <returns>True, if the sets are equal. False otherwise.</returns>
      public override bool Equals(object obj) {
         var otherSet = obj as Set<T>;
         if(otherSet != null) {
            if(otherSet.Count != Count) {
               return false;
            }
            foreach( T item in _items.Keys ) {
               if( !otherSet.Contains(item) ) {
                  return false;
               }
            }
            return true;
         }
         return false;
      }

      /// <summary>
      /// Hash function for this type.
      /// </summary>
      /// <returns>A hash value</returns>
      public override int GetHashCode() {
         return _items.GetHashCode();
      }

      /// <summary>
      /// Converts the set to a string representation. Calls T.ToString() to 
      /// obtain a string representation of an element.
      /// </summary>
      /// <returns>A string with the contents of the set.</returns>
      /// <remarks>This operation can be very expensive for large sets.</remarks>
      public override string ToString() {
         var sb = new StringBuilder();
         var count = _items.Count;
         sb.Append("{");
         foreach(T item in _items.Keys) {
            sb.Append(item.ToString());
            if(--count > 0) {
               sb.Append(", ");
            }
         }
         sb.Append("}");
         return sb.ToString();
      }

      #region IEnumerable<T> Members
      /// <summary>
      /// Returns an enumerator for the set.
      /// </summary>
      /// <returns>An enumerator</returns>
      public IEnumerator<T> GetEnumerator() {
         return _items.Keys.GetEnumerator();
      }

      #endregion

      #region IEnumerable Members

      IEnumerator IEnumerable.GetEnumerator() {
         return _items.Keys.GetEnumerator();
      }

      #endregion

      private readonly Dictionary<T, object> _items = new Dictionary<T, object>();
   }
}