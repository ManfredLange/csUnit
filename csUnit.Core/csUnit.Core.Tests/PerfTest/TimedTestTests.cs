///////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2003 by Manfred Lange. All rights reserved.
// Usage of this software only according to the terms of the GNU GPL.
///////////////////////////////////////////////////////////////////////////////

using System;
using csUnit;
using csUnit.Core;
using csUnit.Perf;

namespace csUnit.Core.Test.PerfTest {
   /// <summary>
   /// Summary description for TimedTestTests.
   /// </summary>
   [TestFixture]
   public class TimedTestTests {
      [TestFixture]
      internal class ClassWithTimedTests {
         [TimedTest(100)]
         public void DoesNotTimeOut() {
            _sequence += "DoesNotTimeOut";
         }

         [TimedTest(100)]
         public void TimesOut() {
            _sequence += "TimesOut";
            System.Threading.Thread.Sleep(150);
         }
      }

      internal class MyListener : NullListener {
         public override void OnTestFailed(object sender, TestResultEventArgs args) {
            _failureMessage = args.Reason;
         }
         public string FailureMessage {
            get {
               return _failureMessage;
            }
         }
         private string _failureMessage = string.Empty;
      }

      [SetUp]
      public void ResetVariables() {
         _listener = new MyListener();
         _sequence = string.Empty;
      }

      [Test]
      public void DoesNotTimeOut() {
         TestSpec ts = new TestSpec();
         ts.Add(typeof(ClassWithTimedTests).FullName, "DoesNotTimeOut");
         TestFixture tf = new TestFixture(typeof(ClassWithTimedTests), _listener);
         tf.RunTests(ts);
         Assert.Equals("DoesNotTimeOut", _sequence);
      }

      [Test]
      public void TimesOut() {
         TestSpec ts = new TestSpec();
         ts.Add(typeof(ClassWithTimedTests).FullName, "TimesOut");
         TestFixture tf = new TestFixture(typeof(ClassWithTimedTests), _listener);
         tf.RunTests(ts);
         Assert.Equals("TimesOut", _sequence);
         Assert.Equals("Test was expected to finish within 100 milliseconds.", _listener.FailureMessage);
      }

      //-----------------------------------------------------------------------
      [TestFixture]
      internal class ContainsLongTimedTest {
         [TimedTest(100, false)]
         public void TakesLong() {
            System.Threading.Thread.Sleep(2000);
         }
      }

      [TimedTest(1000), Ignore("First we need to move the duration measurement to TestAttribute.ExecuteTest()")]
      public void DontWaitForCompletion() {
         TestFixture tf = new TestFixture(typeof(ContainsLongTimedTest), _listener);
         tf.RunTests(new TestSpec());
      }

      [Test, Ignore("Implementation required.")]
      public void AbortLongTimedTest() {
      }

      private static string _sequence = string.Empty;
      private MyListener   _listener = null;
   }
}
