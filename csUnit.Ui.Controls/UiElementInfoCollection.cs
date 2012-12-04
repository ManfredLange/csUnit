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

namespace csUnit.Ui.Controls {
   public class UiElementInfoCollection {
      public event EventHandler CollectionChanged;

      /// <summary>
      /// Determines if the given item exists in this collection or not.
      /// </summary>
      /// <param name="item"></param>
      /// <returns></returns>
      public bool Contains(UiElementInfo item) {
         return _items.Contains(item);
      }

      /// <summary>
      /// Returns an enumerator on the items in the collection.
      /// </summary>
      /// <returns>An enumerator.</returns>
      public IEnumerator<UiElementInfo> GetEnumerator() {
         return _items.GetEnumerator();
      }

      /// <summary>
      /// Adds an item to the selected items collection.
      /// </summary>
      /// <param name="item">The item to add.</param>
      public void Add(UiElementInfo item) {
         if(   item != null
            && !_items.Contains(item) ) {
            _items.Add(item);
            if(CollectionChanged != null) {
               CollectionChanged(this, new EventArgs());
            }
         }
      }

      /// <summary>
      /// Adds the contents of a different selected items collection to this
      /// collection.
      /// </summary>
      /// <param name="items">Items collection to be added.</param>
      public void Add(UiElementInfoCollection items) {
         foreach(UiElementInfo item in items) {
            Add(item);
         }
      }

      /// <summary>
      /// Removes an item from the selected items collection.
      /// </summary>
      /// <param name="item">The item to be removed.</param>
      public void Remove(UiElementInfo item) {
         if(_items.Contains(item)) {
            _items.Remove(item);
            if(CollectionChanged != null) {
               CollectionChanged(this, new EventArgs());
            }
         }
      }

      /// <summary>
      /// Removes all items from the collection.
      /// </summary>
      public void Clear() {
         if(_items.Count > 0) {
            _items.Clear();
            if(CollectionChanged != null) {
               CollectionChanged(this, new EventArgs());
            }
         }
      }

      /// <summary>
      /// Returns an array of the selected items.
      /// </summary>
      /// <returns></returns>
      public UiElementInfo[] ToArray() {
         return _items.ToArray();
      }

      /// <summary>
      /// Gets the number of selected items.
      /// </summary>
      public int Count {
         get {
            return _items.Count;
         }
      }

      private readonly List<UiElementInfo> _items = new List<UiElementInfo>();
   }
}
