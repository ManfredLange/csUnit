#region Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, and Piers Lawson. All rights reserved.
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
#endregion

using csUnit.Interfaces;
using csUnit.Interfaces.Criteria;

namespace csUnit.Core.Criteria {
   internal class CategoryCriterion : ICriterion {
      public CategoryCriterion(string categoryName) {
         _categoryName = categoryName;
      }

      #region Implementation of ICriterion

      public bool Contains(ITestMethod testMethod) {
         return testMethod.InheritedCategories.Contains(_categoryName)
                || testMethod.Categories.Contains(_categoryName);
      }

      public bool Contains(ITestFixture testFixture) {
         return testFixture.InheritedCategories.Contains(_categoryName)
                || testFixture.Categories.Contains(_categoryName);
      }

      #endregion

      private readonly string _categoryName;
   }
}
