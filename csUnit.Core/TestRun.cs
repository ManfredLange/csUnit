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

using csUnit.Core.Criteria;
using csUnit.Interfaces;
using csUnit.Interfaces.Criteria;

namespace csUnit.Core {
   /// <summary>
   /// A TestRun describes which tests are to be run and how. It may contain one
   /// or more criteria that selects just a subset of the tests in a recipe.
   /// </summary>
   [Serializable]
   public class TestRun : ITestRun {
      public TestRun(ICriterion criterion) {
         if( criterion == null ) {
            throw new NullReferenceException( "Parameter 'criterion' cannot be null." );
         }
         _criterion = criterion;
      }

      public bool Contains(ITestMethod testMethod) {
         return _criterion.Contains(testMethod);
      }

      public bool Contains(ITestFixture testFixture) {
         return _criterion.Contains(testFixture);
      }

      internal static TestRun Where(ICriterion criterion) {
         return new TestRun(criterion);
      }

      internal TestRun And(ICriterion criterion) {
         var andCriterion = new AndCriterion();
         andCriterion.Add(criterion);
         andCriterion.Add(_criterion);
         return new TestRun(andCriterion);
      }

      private readonly ICriterion _criterion;
   }
}
