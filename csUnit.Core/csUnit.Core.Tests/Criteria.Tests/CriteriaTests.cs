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

using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Core.Tests.Criteria.Tests {
   [TestFixture]
   public class CriteriaTests {
      [TestFixture]
      private class SampleTestFixture {
         [Test]
         public void Foo() {
         }

         [Test]
         public void Bar() {
         }
      }

      [Test]
      public void OneNameCriterion() {
         ITestMethod tm =
            new TestMethod(new TestFixture(typeof(SampleTestFixture)),
                           typeof(SampleTestFixture).GetMethod("Foo"));
         var nameCriterion = new NameCriterion(typeof(SampleTestFixture).FullName + ".Foo");
         var selection = new TestRun(nameCriterion);
         Assert.True(selection.Contains(tm));
      }

      [Test]
      public void TwoNameCriteria() {
         ITestMethod tm =
            new TestMethod(new TestFixture(typeof(SampleTestFixture)),
                           typeof(SampleTestFixture).GetMethod("Foo"));
         var addCriterion = new OrCriterion();
         addCriterion.Add(new NameCriterion(typeof(SampleTestFixture).FullName + ".Foo"));
         addCriterion.Add(new NameCriterion("bar"));
         var testRun = new TestRun(addCriterion);
         Assert.True(testRun.Contains(tm));
      }

      [Test]
      public void NamespaceCriterion() {
         ITestMethod tm1 = 
            new TestMethod(new TestFixture(typeof(SampleTestFixture)),
                           typeof(SampleTestFixture).GetMethod("Foo"));
         ITestMethod tm2 =
            new TestMethod(new TestFixture(typeof(CriteriaTests)),
                           typeof(CriteriaTests).GetMethod("NamespaceCriterion"));
         var selection = new TestRun(new NamespaceCriterion(tm1.DeclaringTypeFullName));
         Assert.True(selection.Contains(tm1));
         Assert.False(selection.Contains(tm2));
      }
   }
}