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

#if DEBUG

using System;
using System.Reflection;

using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   /// <summary>
   /// Summary description for ExpectedExceptionTests.
   /// </summary>
   [TestFixture]
   public class ExpectedExceptionTests {
      //----------------------------------------------
      private class MyListener : NullListener {
         public override void OnTestError(object sender, TestResultEventArgs args) {
            ErrorCount++;
            _reason = args.Reason;
         }
         
         public override void OnTestFailed(object sender, TestResultEventArgs args) {
            FailureCount++;
            _reason = args.Reason;
         }

         public override void OnTestPassed(object sender, TestResultEventArgs args) {
            _nameOfExecutedTest = args.MethodName;
         }
         
         public string NameOfExecutedTest {
            get {
               return _nameOfExecutedTest;
            }
         }

         public int ErrorCount { get; private set; }

         public int FailureCount { get; private set; }

         public string Reason {
            get {
               return _reason;
            }
         }

         private string _nameOfExecutedTest = string.Empty;
         private string _reason = string.Empty;
      }

      //----------------------------------------------
      // The test fixture for the subsequent test
      [TestFixture]
      private class ThrowsExpectedException {
         [Test, ExpectedException(typeof(Exception))]
         public void ThrowsException() {
            throw new Exception("Message of expected exception.");
         }
      }

      [Test]
      public void ExpectedExceptionThrown() {
         var myListener = new MyListener();
         var tc = new TestFixture(typeof(ThrowsExpectedException));

         tc.Execute(new TestRun(new AllTestsCriterion()), myListener);
         
         Assert.Equals(0, myListener.ErrorCount);
         Assert.Equals("ThrowsException", myListener.NameOfExecutedTest);
      }

      //----------------------------------------------
      // The test fixture for the subsequent test
      [TestFixture]
      private class ThrowsExpectedArgumentException {
         [Test, ExpectedException(typeof(ArgumentException))]
         public void ThrowsException() {
            throw new ArgumentException("Message of expected target invocation exception.", new Exception("inner"));
         }
      }

      [Test]
      public void ExpectedTargetInvocationExceptionThrown() {
         var myListener = new MyListener();
         var tc = new TestFixture(typeof(ThrowsExpectedArgumentException));
         tc.Execute(new TestRun(new AllTestsCriterion()), myListener);
         Assert.Equals(0, myListener.ErrorCount);
         Assert.Equals("ThrowsException", myListener.NameOfExecutedTest);
      }

      //----------------------------------------------
      // The test fixture for the subsequent test
      [TestFixture]
      private class ThrowsUnExpectedException1 {
         [Test, ExpectedException(typeof(ArgumentException))]
         public void ThrowsException() {
            throw new ArithmeticException("Message of actual exception.");
         }
      }

      [Test]
      public void UnexpectedExceptionThrown1() {
         var myListener = new MyListener();
         var tc = new TestFixture(typeof(ThrowsUnExpectedException1));
         tc.Execute(new TestRun(new AllTestsCriterion()), myListener);
         Assert.Equals(1, myListener.ErrorCount);
         Assert.StartsWith("Unexpected System.ArithmeticException: Message of actual exception",
            myListener.Reason);
      }

      //----------------------------------------------
      // The test fixture for the subsequent test
      [TestFixture]
      private class ThrowsUnExpectedException2 {
         private class SpecialException : Exception {
         }

         [Test, ExpectedException(typeof(Exception))]
         public void ThrowsException() {
            throw new TargetInvocationException("Message of expected target invocation exception.", new SpecialException());
         }
      }

      [Test]
      public void UnexpectedExceptionThrown2() {
         var myListener = new MyListener();
         var tc = new TestFixture(typeof(ThrowsUnExpectedException2));
         tc.Execute(new TestRun(new AllTestsCriterion()), myListener);
         Assert.Equals(1, myListener.ErrorCount);
      }

      //----------------------------------------------
      // The test fixture for the subsequent test
      [TestFixture]
      private class ExpectedExceptionNotThrown {
         [Test, ExpectedException(typeof(DivideByZeroException))]
         public void DoesNotThrowExpectedException() {
         }
      }

      [Test]
      public void ExpectedExceptionIsNotThrown() {
         var myListener = new MyListener();
         var tc = new TestFixture(typeof(ExpectedExceptionNotThrown));
         tc.Execute(new TestRun(new AllTestsCriterion()), myListener);
         Assert.Equals(1, myListener.FailureCount);
      }
   }
}

#endif
