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
using System.Threading;

using csUnit.Core.Criteria;
using csUnit.Interfaces;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace csUnit.Core.Tests {
   [TestFixture]
   public class TestMethodTests {

      [TestFixture]
      private class Foo {
         [Test]
         public void AMethod() {
            _sequence += " AMethod";
         }

         [Test]
         public void MethodWithException() {
            _sequence += " MethodWithException";
            throw new NullReferenceException();
         }
      }

      //------------------------------------------
      // The actual tests

      [SetUp]
      public void CreateCommonObjects() {
         _sequence = string.Empty;
         _listener = new SimpleTestListener();
      }

      [Test]
      public void ExecuteMethod() {
         var fixture = new TestFixture(typeof(Foo));
         var tm = new TestMethod(fixture, typeof(Foo).GetMethod("AMethod"));
         tm.Execute( _listener);
         Assert.Equals(" AMethod", _sequence);
         Assert.Equals(" TS:AMethod TP:AMethod", SimpleTestListener.Messages);
      }

      [Test]
      public void ExecuteMethodThrowsException() {
         var fixture = new TestFixture(typeof(Foo));
         var tm = new TestMethod(fixture, typeof(Foo).GetMethod("MethodWithException"));
         tm.Execute(_listener);
         Assert.Equals(" MethodWithException", _sequence);
         Assert.StartsWith(" TS:MethodWithException TE:MethodWithException ", SimpleTestListener.Messages);
      }

      private class FixtureWithTimedTest {
         [Test(Timeout=50)]
         public void TimedTest() {
            Thread.Sleep(55);
         }
      }

      [Test]
      public void TestWithTimeOutFails() {
         var fixture = new TestFixture(typeof(FixtureWithTimedTest));
         fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.StartsWith(" TS:TimedTest TF:TimedTest Time out. Expected 50 ms, but took ", 
            SimpleTestListener.Messages);
         Assert.Equals(1, SimpleTestListener.FailureItems.Count);
      }

      private class FixtureWithTimedTest2 {
         [Test(Timeout=50)]
         public void TimedTest() {
            Thread.Sleep(30);
         }
      }

      [Test]
      public void TestWithTimeOutPasses() {
         var fixture = new TestFixture(typeof(FixtureWithTimedTest2));
         fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Contains("TP:TimedTest", SimpleTestListener.Messages);
         Assert.Equals(0, SimpleTestListener.FailureItems.Count);
         Assert.Equals(1, SimpleTestListener.PassedItems.Count);
      }

      [TestFixture]
      private class ClassWithPassingTest {
         [Test]
         public void PassingTest() {
            // intentionally empty [ml]
         }
      }

      [Test]
      public void TestResultEventArgsWhenPassing() {
         var listener = new CheckingTestListener();
         var fixture = new TestFixture(typeof(ClassWithPassingTest));
         fixture.Execute(new TestRun(new AllTestsCriterion()), listener);
         Assert.Equals(TestResultCategory.Success, listener.TestResult);
      }

      [TestFixture]
      private class ClassWithFailingTest {
         [Test]
         public void FailingTest() {
            Assert.Equals(1, 2);
         }
      }

      [Test]
      public void TestResultEventArgsWhenFailing() {
         var listener = new CheckingTestListener();
         var fixture = new TestFixture(typeof(ClassWithFailingTest));
         fixture.Execute(new TestRun(new AllTestsCriterion()), listener);
         Assert.Equals(TestResultCategory.Failure, listener.TestResult);
      }

      [TestFixture]
      private class ClassWithTestWithError {
         [Test]
         public void ATestWithError() {
// ReSharper disable ConvertToConstant.Local
            var i = 0;
            var j = 1;
// ReSharper restore ConvertToConstant.Local
#pragma warning disable 168
            var k = j / i;
#pragma warning restore 168
         }
      }

      [Test]
      public void TestResultEventArgsWhenError() {
         var listener = new CheckingTestListener();
         var fixture = new TestFixture(typeof(ClassWithTestWithError));
         fixture.Execute(new TestRun(new AllTestsCriterion()), listener);
         Assert.Equals(TestResultCategory.Error, listener.TestResult);
      }

      private class DontCareException : Exception {}

      [TestFixture]
      private class ClassWithMissingExpectedException {
         [Test]
         [ExpectedException(typeof(DontCareException))]
         public void NotThrowingExpectedException() {
            // Intentionally left empty. [ml]
         }
      }

      [Test]
      public void TestResultEventArgsWhenExpectedExceptionMissing() {
         var listener = new CheckingTestListener();
         var fixture = new TestFixture(typeof(ClassWithMissingExpectedException));
         fixture.Execute(new TestRun(new AllTestsCriterion()), listener);
         Assert.Equals(TestResultCategory.ExpectedExceptionNotThrown, listener.TestResult);
      }

      [TestFixture]
      private class ContainsTestsToBeSkipped {
         [Test]
         [Ignore("for good")]
         public void ToBeSkipped() {
            // intentionally left empty
         }
      }

      [Test]
      public void TestResultEventArgsWhenTestSkipped() {
         var listener = new CheckingTestListener();
         var fixture = new TestFixture(typeof(ContainsTestsToBeSkipped));
         fixture.Execute(new TestRun(new AllTestsCriterion()), listener);
         Assert.Equals(TestResultCategory.Skipped, listener.TestResult);
      }

      private class CheckingTestListener : ITestListener {
         public TestResultCategory TestResult { get; private set; }

         #region Implementation of ITestListener

         public void OnAssemblyLoaded(object sender, AssemblyEventArgs args) {
            throw new NotImplementedException();
         }

         public void OnAssemblyStarted(object sender, AssemblyEventArgs args) {
            throw new NotImplementedException();
         }

         public void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
            throw new NotImplementedException();
         }

         public void OnTestsAborted(object sender, AssemblyEventArgs args) {
            throw new NotImplementedException();
         }

         public void OnTestStarted(object sender, TestResultEventArgs args) {
            TestResult = args.TestResult;
         }

         public void OnTestPassed(object sender, TestResultEventArgs args) {
            TestResult = args.TestResult;
         }

         public void OnTestError(object sender, TestResultEventArgs args) {
            TestResult = args.TestResult;
         }

         public void OnTestFailed(object sender, TestResultEventArgs args) {
            TestResult = args.TestResult;
         }

         public void OnTestSkipped(object sender, TestResultEventArgs args) {
            TestResult = args.TestResult;
         }

         #endregion
      }

      private ITestListener _listener;
      private static string _sequence = string.Empty;
   }
}

// ReSharper restore UnusedMember.Local
// ReSharper restore UnusedMember.Global
