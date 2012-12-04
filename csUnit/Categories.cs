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

using csUnit.Common;

namespace csUnit {
   /// <summary>
   /// A collection of categories. If empty, it represents "all" categories.
   /// </summary>
   /// <remarks>Categories is similar to a Set of strings, but is differerent in two
   /// aspects. The latter is the reason why it doesn't directly inherit from
   /// class Set. 
   /// A set accepts empty strings as elements. A Categories object does not.
   /// A Category object that doesn't contain any elements represents "all" 
   /// categories. This is not true for a set of strings.
   /// Therefore inheritance would not have been appropriate, although the
   /// Categories class has many operations (by name) in common with a set.
   /// </remarks>
   [Serializable]
   public class Categories {
      /// <summary>
      /// Fired when a category has been added to or removed from the collection
      /// of categories.
      /// </summary>
      public event EventHandler Modified;

      ///// <summary>
      ///// Returns an empty Categories object.
      ///// </summary>
      //public static Categories Empty {
      //   get {
      //      return _empty;
      //   }
      //}

      /// <summary>
      /// Returns an empty Categories object.
      /// </summary>
      public static readonly Categories Empty = new Categories();

      /// <summary>
      /// Adds a category. Empty strings are ignored.
      /// </summary>
      /// <param name="categoryName">The name of the category.</param>
      public void Add(string categoryName) {
         int initialCount = _categories.Count;
         string trimmedCategoryName = categoryName.Trim();
         if( trimmedCategoryName != string.Empty ) {
            _categories.Add(trimmedCategoryName);
            if(initialCount != _categories.Count) {
               FireModifiedEvent();
            }
         }
      }

      private void FireModifiedEvent() {
         if(null != Modified) {
            Modified(this, new EventArgs());
         }
      }

      /// <summary>
      /// Adds multiple strings to a category. Empty strings are ignored.
      /// </summary>
      /// <param name="categoryNames">An array of category names.</param>
      public void Add(string[] categoryNames) {
         int initialCount = _categories.Count;
         foreach(string categoryName in categoryNames) {
            string trimmedCategoryName = categoryName.Trim();
            if( trimmedCategoryName != string.Empty ) {
               _categories.Add(trimmedCategoryName);

            }
         }
         if( initialCount != _categories.Count) {
            FireModifiedEvent();
         }
      }

      /// <summary>
      /// Adds a different set of categories. Duplicates are ignored.
      /// </summary>
      /// <param name="categories">Set of categories to be added.</param>
      public void Add(Categories categories) {
         int initialCount = _categories.Count;
         _categories.Add(categories._categories);
         if(initialCount != _categories.Count) {
            FireModifiedEvent();
         }
      }

      /// <summary>
      /// Clears the collection.
      /// </summary>
      public void Clear() {
         int initialCount = _categories.Count;
         _categories.Clear();
         if(initialCount > 0) {
            FireModifiedEvent();
         }
      }

      /// <summary>
      /// Tests whether a specific category is contained in the set.
      /// </summary>
      /// <param name="category">Name of category to test for.</param>
      /// <returns>True, if the category is contained, false if not.</returns>
      public bool Contains(string category) {
         return _categories.Contains(category);
      }

      /// <summary>
      /// Gets the number of categories in the set.
      /// </summary>
      public int Count {
         get {
            return _categories.Count;
         }
      }

      /// <summary>
      /// Tests whether two Categories objects contain the same categories.
      /// </summary>
      /// <param name="obj">Categories object to compare.</param>
      /// <returns>True, if both contain the same categories, false otherwise.
      /// </returns>
      public override bool Equals(object obj) {
         var otherCategories = obj as Categories;
         if(otherCategories != null) {
            return _categories.Equals(otherCategories._categories);
         }
         return false;
      }

      /// <summary>
      /// Gets an enumerator to iterate over the collection.
      /// </summary>
      /// <returns>An enumerator over strings.</returns>
      public IEnumerator<string> GetEnumerator() {
         return _categories.GetEnumerator();
      }

      /// <summary>
      /// Determines the intersection of two Categories objects.
      /// </summary>
      /// <param name="otherCats">A Cateogies object.</param>
      /// <returns>A Categories object with categories that both sets have in 
      /// common.</returns>
      public Categories Intersect(Categories otherCats) {
         var cats = new Categories {
            _categories = _categories.Intersect( otherCats._categories)
         };
         return cats;
      }

      /// <summary>
      /// Determines the union of two Categories objects.
      /// </summary>
      /// <param name="otherCats">A Categories object.</param>
      /// <returns>A Categories object with all categories that are also in at
      /// least one of the two Categories objects.</returns>
      public Categories Union(Categories otherCats) {
         var cats = new Categories {
            _categories = _categories.Union(otherCats._categories)
         };
         return cats;
      }

      /// <summary>
      /// Gets a boolean value indicating whether the Categories object is 
      /// empty. Returns true, if the Categories objects doesn't contain any
      /// category, false otherwise.
      /// </summary>
      public bool IsEmpty {
         get {
            return _categories.IsEmpty;
         }
      }

      /// <summary>
      /// Removes a specific category from the Categories object. Removing an
      /// element that does not exist does not fail.
      /// </summary>
      /// <param name="category">Category name to remove.</param>
      public void Remove(string category) {
         int initialCount = _categories.Count;
         _categories.Remove(category);
         if(initialCount != _categories.Count) {
            FireModifiedEvent();
         }
      }

      /// <summary>
      /// Converts the Categories object into an array of strings.
      /// </summary>
      /// <returns>A string array.</returns>
      public string[] ToArray() {
         return _categories.ToArray();
      }

      /// <summary>
      /// Converts the Categories collection into a string representation.
      /// </summary>
      /// <returns></returns>
      public override string ToString() {
         return _categories.ToString();
      }

      /// <summary>
      /// Returns a hash code for this object.
      /// </summary>
      /// <returns>Integer hash code.</returns>
      public override int GetHashCode() {
         return _categories.GetHashCode();
      }

      private Set<string> _categories = new Set<string>();
   }
}
