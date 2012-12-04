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

using System;

namespace csUnit {
   /// <summary>
   /// Abstract base class for attributes that decorate methods within a test
   /// fixture.
   /// Known derived classes as of 05apr08 [ml]:
   /// - SetUpAttribute
   /// - TearDownAttribute
   /// - TestAttribute
   /// </summary>
   public abstract class MethodAttribute : CsUnitAttribute {
      /// <summary>
      /// Gets the human readable name of the attribute.
      /// </summary>
      public abstract string AttributeName {
         get;
      }

      /// <summary>
      /// Obsolete. Use property 'Categories' instead, which accepts one or more
      /// categories.
      /// </summary>
      [Obsolete("Use 'Categories' property instead which accepts a comma separated list of category names.", false)]
      public virtual string Category {
         get {
            return Categories;
         }
         set {
            Categories = value;
         }
      }

      /// <summary>
      /// Get/set the categories for this method. Specify multiple categories
      /// separated by comma.
      /// </summary>
      public string Categories {
         get {
            return String.Join(",", _categories.ToArray());
         }
         set {
            _categories.Add(value.Split(','));
         }
      }

      /// <summary>
      /// Get/set the categories for this method.
      /// </summary>
      public Categories _Categories {
         get {
            return _categories;
         }
      }

      /// <summary>
      /// Categories for this method.
      /// </summary>
      private readonly Categories _categories = new Categories();
   }
}
