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
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Summary description for TestMethodInfo.
   /// </summary>
   [Serializable]
   public class TestMethodInfo : ITestMethodInfo {
      internal TestMethodInfo(ITestMethod tm) {
         _fullName   = tm.FullName;
         _name       = tm.Name;
         _categories = tm.Categories;
         _inheritedCategories = tm.InheritedCategories;
      }

      /// <summary>
      /// Gets the fixture name for this method plus the method's name.
      /// </summary>
      public string FullName {
         get {
            return _fullName;
         }
      }

      /// <summary>
      /// Gets the method's name only.
      /// </summary>
      public string Name {
         get {
            return _name;
         }
      }

      public Categories Categories {
         get {
            return _categories;
         }
      }

      public Categories InheritedCategories {
         get {
            return _inheritedCategories;
         }
      }

      private readonly string _fullName = string.Empty;
      private readonly string _name = string.Empty;
      private readonly Categories _categories;
      private readonly Categories _inheritedCategories;
   }
}
