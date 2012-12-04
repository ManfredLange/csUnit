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

namespace csUnit {
   /// <summary>
   /// A public class with the TestFixtureAttribute set will be identified as
   /// a test fixture.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
   public class TestFixtureAttribute : CsUnitAttribute {
      /// <summary>
      /// The categories for this test fixture.
      /// </summary>
      private readonly Categories _categories = new Categories();

      /// <summary>
      /// Constructs a TestFixtureAttribute object.
      /// </summary>
      public TestFixtureAttribute()
         : this(string.Empty) {}

      /// <summary>
      /// Constructs a TestFixtureAttribute object with a specific category.
      /// </summary>
      public TestFixtureAttribute(string category) {
         Categories = category;
      }

      /// <summary>
      /// Get/set the category for this fixture.
      /// </summary>
      [Obsolete("Obsolete. Use property 'Categories' instead.", false)]
      public string Category {
         get { return Categories; }
         set { Categories = value; }
      }

      /// <summary>
      /// Get/set the categories for this fixture. Multiple categories can be
      /// specified comma-separated.
      /// </summary>
      public string Categories {
         set { _categories.Add(value.Split(',')); }
         get { return string.Join(",", _categories.ToArray()); }
      }

      /// <summary>
      /// Don't use. This property is for csUnit's internal use only.
      /// </summary>
      public Categories _Categories {
         get { return _categories; }
      }
   }
}