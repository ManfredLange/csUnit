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
using csUnit.Data;

namespace csUnit.Core.Tests.Data {
   // ReSharper disable UnusedMember.Local
   [TestFixture]
   public class DataRowAttributeTests {
      private class ClassWithListFeed {
         [Test]
         [DataRow(0, 0)]
         [DataRow(1, 1)]
         [DataRow(2, 4)]
         [DataRow(3, 9)]
         public void SimpleTest(int oper, int result) {
            Assert.Equals(result, oper * oper);
         }
      }

      [Test]
      public void TrySimpleListFeed() {
         var tf = new TestFixture(typeof(ClassWithListFeed));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(4, SimpleTestListener.PassedItems.Count);
         Assert.Contains(" TS:SimpleTest TP:SimpleTest", SimpleTestListener.Messages);
      }

      private class ClassWithEmptyList {
         [Test]
         public void SimpleTest(int oper, int result) {
            Assert.Equals(result, oper * oper);
         }
      }

      [Test]
      public void EmptySimpleList() {
         var tf = new TestFixture(typeof(ClassWithEmptyList));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(0, SimpleTestListener.PassedItems.Count);
         Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
         Assert.Contains(" TE:SimpleTest No data rows specified for parameterized test.", SimpleTestListener.Messages);
      }

      private class FixtureWithErrorInList {
         [Test]
         [DataRow(3, 9)]
         [DataRow]
         public void SimpleTest(int oper, int result) {
            Assert.Equals(result, oper * oper);
         }
      }

      [Test]
      public void ASimpleListWithError() {
         var tf = new TestFixture(typeof(FixtureWithErrorInList));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.PassedItems.Count);
         Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
         Assert.Contains(" TS:SimpleTest TP:SimpleTest", SimpleTestListener.Messages);
         Assert.Contains(" TE:SimpleTest Each data row for SimpleTest must have 2 values.", SimpleTestListener.Messages);
      }

      private class ParameterizedTestExpectedException {
         [Test]
         [DataRow(2, 1, 2)]
         [DataRow(6, 2, 3)]
         [DataRow(4, 0, 0, ExpectedException = typeof(DivideByZeroException))]
         public void DivisionTest(int numerator, int denominator, int quotient) {
            Assert.Equals(quotient, numerator / denominator);
         }
      }

      [Test]
      public void SimpleListWithExpectedException() {
         var tf = new TestFixture(typeof(ParameterizedTestExpectedException));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(3, SimpleTestListener.PassedItems.Count);
         Assert.Contains(" TS:DivisionTest TP:DivisionTest", SimpleTestListener.Messages);
      }

      private class DataRowWithNonMatchingParameterType {
         [Test]
         [DataRow(2, "oops", 2)]
         public void DivisionTest(int numerator, int denominator, int quotient) {
            Assert.Equals(quotient, numerator / denominator);
         }
      }

      [Test]
      public void NonMatchingParameterTypeInRow() {
         var tf = new TestFixture(typeof(DataRowWithNonMatchingParameterType));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
         Assert.Contains(" TE:DivisionTest Parameter types don't match for DataRow(2, oops, 2).", SimpleTestListener.Messages);
      }

      [Test]
      public void NullAsConstructorParameter() {
         var dataRow = new DataRow(null);
         Assert.Equals(new object[] { }, dataRow.Values);
      }

      private class FixtureWithVaryingParameterType {
         [Test]
         [DataRow(1, 1, 1)]
         [DataRow(2.0, 2.0, 4.0)]
         [DataRow(3.5, 1, 3.5)]
         public void SomeMultiplication(float param1, float param2, float result) {
            Assert.Equals(result, param1 * param2);
         }
      }

      [Test]
      public void ConvertsParameterType() {
         var tf = new TestFixture(typeof(FixtureWithVaryingParameterType));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(3, SimpleTestListener.PassedItems.Count);
         Assert.Contains(" TS:SomeMultiplication TP:SomeMultiplication", SimpleTestListener.Messages);
      }
   }
   // ReSharper restore UnusedMember.Local
}
