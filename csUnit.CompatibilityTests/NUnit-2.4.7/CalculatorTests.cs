using System;
using NUnit.Framework;

namespace NUnit_2._4._7 {
   [TestFixture]
   public class CalculatorTests {
      [SetUp]
      public void CreateObjects() {
         _calculator = new Calculator();
      }

      [Test]
      public void AddTwoNumbers() {
         _calculator.Push(2);
         _calculator.Push(3);
         _calculator.Multiply();
         Assert.AreEqual(6, _calculator.Peek());
      }

      [Test]
      [ExpectedException(typeof(ApplicationException))]
      public void PeekWhenStackEmpty() {
         _calculator.Peek();
      }

      [Test]
      [Ignore("To be ignored")]
      public void IgnoredTestMethod() {
         
      }

      private Calculator _calculator = null;
   }
}
