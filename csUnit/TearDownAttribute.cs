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
   /// A method tagged with the TearDownAttribute will be called immediately
   /// after the execution of each single test. The method must have the type
   /// 'public void' and must not take any parameter.
   /// </summary>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class TearDownAttribute : MethodAttribute {
      /// <summary>
      /// Constructs a TearDownAttribute object.
      /// </summary>
      public TearDownAttribute() {
      }

      /// <summary>
      /// Constructs with a specific category name
      /// </summary>
      /// <param name="cat"></param>
      public TearDownAttribute(string cat) {
         _category = cat;
      }

      /// <summary>
      /// Gets the human readable name of the attribute.
      /// </summary>
      public override string AttributeName {
         get {
            return "TearDown";
         }
      }

      /// <summary>
      /// Gets/sets the test context category for this attribute.
      /// </summary>
      [Obsolete("Use 'Categories' property instead which accepts a comma separated list of category names.", false)]
      public override string Category {
         get { 
            return _category; 
         }
         set { 
            _category = value; 
         }
      }
      // TODO: Should be made obsolete and replaced by Categories, which supports
      // multiple categories. [04Feb07, ml]

      private string _category;
   }
}
