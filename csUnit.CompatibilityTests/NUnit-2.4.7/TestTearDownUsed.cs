using System;
using NUnit.Framework;

namespace NUnit_2._4._7 {
   [TestFixture]
   public class TestTearDownUsed {
      [SetUp]
      public void SetUpObjects() {
         if( _calculator != null ) {
            throw new ApplicationException("TearDown wasn't called.");
         }
            _calculator = new Calculator();
         }

      [TearDown]
      public void TearDownObjects() {
         _calculator = null;
      }

      [Test]
      public void Test1() {
         _calculator.Push(1);
         // no assertion on purpose.
      }

      [Test]
      public void Test2() {
         _calculator.Push(2);
         // no assrtion on purpose.
      }

      private Calculator _calculator;
   }
}
