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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using csUnit.Core.Adapters;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Summary description for TestMethod.
   /// </summary>
   internal class TestMethod : MarshalByRefObject, ITestMethod {
      /// <summary>
      /// Creates an instance of the TestMethod class.
      /// </summary>
      /// <param name="testFixture">TestFixture instance the TestMethod belongs to.</param>
      /// <param name="methodInfo">MethodInfo the test method.</param>
      public TestMethod(TestFixture testFixture, MethodInfo methodInfo) {
         _fixture = testFixture;
         _methodInfo = methodInfo;
         _framework = FrameworkAdapter.CreateInstance(_fixture.AssemblyName);
         ScanForAttributes();
      }

      public string AttributeName {
         get {
            return _attributeName;
         }
      }

      /// <summary>
      /// Executes the test method.
      /// </summary>
      /// <param name="listener"></param>
      public virtual void Execute(ITestListener listener) {
         ExecuteInternal(listener, new object[] { });
      }

      // TODO: Cleanup the remains of this in this file. [16aug09, ml]
      //RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(trycode, cleanupcode, this);
      
      protected void ExecuteInternal(ITestListener listener, params object[] args) {
         var testResultEventArgs = Prepare(listener);
         var ud = new UserData {Listener = listener, Args = args, Trea = testResultEventArgs};
         TheTryCode(ud);
      }

      private class UserData {
         public ITestListener Listener;
         public object[] Args;
         public TestResultEventArgs Trea;
      }

      private void TheTryCode(object data) {
         var ud = data as UserData;
         if( ud != null ) {
            try {
               if(_expectedExceptionAttribute != null) {
                  _expectedExceptionAttribute.Before();
               }

#if DEBUG
               Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
#endif

               _methodInfo.Invoke(_fixture.FixtureInstance, ud.Args);

               if(VerifyExpectedExceptionThrown() == false) {
                  ud.Trea.TestResult = TestResultCategory.ExpectedExceptionNotThrown;
               }
            }
            catch(ThreadAbortException) {
               throw;
            }
            catch(Exception e) {
#if DEBUG
               Debug.WriteLine(string.Format("Current domain = '{0}' (TestMethod.cs, line 96)",
                  AppDomain.CurrentDomain.FriendlyName));
#endif
               if (!IsExpectedException(e)) {
                  Util.ExtractLocation(e, ud.Trea);
                  HandleException(e, ud.Trea);
                  if(e.InnerException != null
                     && e.GetType().Equals(typeof(TargetInvocationException))) {
                     ud.Trea.Failure = e.InnerException as TestFailed;
                  }
               }
            }
            finally {
               StopTimer(ud.Trea);
               if(_timeoutInMilliseconds > 0) {
                  VerifyTimeout(ud.Trea.Duration);
               }

               ud.Trea.Reason = _failureReason;
               if(_bFailure) {
                  ud.Listener.OnTestFailed(new TestMethodInfo(this), ud.Trea);
               }
               else if(_bError) {
                  ud.Listener.OnTestError(new TestMethodInfo(this), ud.Trea);
               }
               else {
                  ud.Listener.OnTestPassed(new TestMethodInfo(this), ud.Trea);
               }
            }
         }
      }

      protected TestFixture Fixture {
         get {
            return _fixture;
         }
      }

      protected MethodInfo MethodInfo {
         get {
            return _methodInfo;
         }
      }

      protected ExpectedExceptionAttribute ExpectedException {
         set {
            _expectedExceptionAttribute = value;
         }
      }

      private TestResultEventArgs Prepare(ITestListener listener) {
         StartTimer();
         var trea = new TestResultEventArgs(
            _methodInfo.DeclaringType.Assembly.FullName,
            _fixture.FullName, _methodInfo.Name, string.Empty, 0);
         _framework.ResetAssertionCounter();
         listener.OnTestStarted(new TestMethodInfo(this), trea);
         _bFailure = false;
         _bError = false;
         return trea;
      }

      private void HandleException(Exception e, TestResultEventArgs trea) {
         if(e.InnerException != null
            && e.GetType().Equals(typeof(TargetInvocationException))) {
            HandleException(e.InnerException, trea);
            return;
         }
         if(_framework.IsFailure(e)) { // Filter out failed assertions. [13jan07, ml]
            HasFailed(e.Message);
            trea.TestResult = TestResultCategory.Failure;
         }
         else {
            HasError("Unexpected " + e.GetType().FullName + ": " + e.Message);
            trea.TestResult = TestResultCategory.Error;
         }
      }

      public Categories InheritedCategories {
         get {
            return _fixture.Categories;
         }
      }

      private bool IsExpectedException(Exception e) {
         if( (e as TargetInvocationException) != null ) {
            return IsExpectedException(e.InnerException);
         }
         if(_expectedExceptionAttribute != null) {
            _bExpectedExceptionThrown = _expectedExceptionAttribute.Expects(e);
            return _bExpectedExceptionThrown;
         }
         return false;
      }

      private void StopTimer(TestResultEventArgs trea) {
         _timer.Stop();
         trea.Duration = ((ulong)_timer.ElapsedTicks) * _nanosecsPerTick;
         trea.AssertCount = _framework.AssertionCount;
         if( _bExpectedExceptionThrown ) {
            trea.AssertCount++;
         }
      }

      private void StartTimer() {
         _timer = Stopwatch.StartNew();
      }

      private Stopwatch _timer;

      public bool Ignore {
         get {
            return _bIgnore;
         }
      }

      public string IgnoreReason {
         get {
            return _ignoreReason;
         }
      }

      private void ScanForAttributes() {
         var csUnitAttributes = _framework.FindAttributesOn(_methodInfo);
         foreach(var csUnitAttribute in csUnitAttributes) {
            if( (csUnitAttribute as MethodAttribute) != null ) {
               var ma = csUnitAttribute as MethodAttribute;
               _categories.Add(ma.Categories.Split(','));
               _attributeName = ma.AttributeName;
               if((ma as TestAttribute) != null) {
                  _timeoutInMilliseconds = (ma as TestAttribute).Timeout;
               }
            }
            else if((csUnitAttribute as IgnoreAttribute) != null) {
               var ia = csUnitAttribute as IgnoreAttribute;
               _bIgnore = true;
               _ignoreReason = ia.Reason;
            }
            else if((csUnitAttribute as ExpectedExceptionAttribute) != null) {
               _expectedExceptionAttribute = csUnitAttribute as ExpectedExceptionAttribute;
            }
         }
      }

      #region Some Helpers

      private bool VerifyExpectedExceptionThrown() {
         if(   _expectedExceptionAttribute != null
            && !_bExpectedExceptionThrown) {
            HasFailed("Expected exception of type "
               + _expectedExceptionAttribute.ExceptionTypeFullName
               + " was not thrown.");
            return false;
         }
         return true;
      }

      private void VerifyTimeout(ulong durationNanoSeconds) {
         var durationMilliSeconds = durationNanoSeconds / 1000000;
         if(_timeoutInMilliseconds > 0
            && durationMilliSeconds > _timeoutInMilliseconds) {
            HasFailed(string.Format("Time out. Expected {0} ms, but took {1}.",
               _timeoutInMilliseconds, durationMilliSeconds));
         }
      }

      private void HasFailed(string reason) {
         // Don't reduce severity [08Apr2006, ml]:
         if(!_bError) {
            _failureReason = reason;
            _bFailure = true;
         }
      }

      private void HasError(string reason) {
         _failureReason = reason;
         _bError = true;
      }

      #endregion Some Helpers

      //------------------------------------------------

      /// <summary>
      /// Gets the full name of the method that is the full class plus the
      /// method name separated by '.' 
      /// </summary>
      /// <example>Example: 'MyNameSpace.MyFixture.Method1'</example>
      public string FullName {
         get {
            return _methodInfo.DeclaringType + "." + Name;
         }
      }

      #region ITestMethod implementation
      /// <summary>
      /// Gets the name of the assembly which contains the test method.
      /// </summary>
      public string AssemblyName {
         get {
            if (_methodInfo.DeclaringType.Assembly.FullName != null) {
               if (_methodInfo != null) {
                  return _methodInfo.DeclaringType.Assembly.FullName.Split(',')[0];
               }
            }
            return null;
         }
      }

      /// <summary>
      /// Gets the full name of the declaring type.
      /// </summary>
      public string DeclaringTypeFullName {
         get {
            return _methodInfo.DeclaringType.FullName;
         }
      }

      /// <summary>
      /// Gets the name of the method.
      /// </summary>
      public string Name {
         get {
            return _methodInfo.Name;
         }
      }

      /// <summary>
      /// Gets all categories for the test method.
      /// </summary>
      public Categories Categories {
         get {
            return _categories;
         }
      }

      /// <summary>
      /// Invokes the test method.
      /// </summary>
      /// <param name="obj">Object on which to execute the test method.</param>
      /// <param name="args">Arguments to pass to the method.</param>
      public void Invoke(object obj, object[] args) {
         if(obj != null) {
            _methodInfo.Invoke(obj, args);
         }
      }
      #endregion // ITestMethod implementation

      private readonly TestFixture _fixture;
      private readonly MethodInfo _methodInfo;
      private readonly Categories _categories = new Categories();
      private string _attributeName = string.Empty;
      private ulong _timeoutInMilliseconds;

      private bool _bIgnore;
      private string _ignoreReason = String.Empty;
      private bool _bFailure;
      private bool _bError;
      private bool _bExpectedExceptionThrown;
      private ExpectedExceptionAttribute _expectedExceptionAttribute;
      private readonly ulong _nanosecsPerTick = (1000UL * 1000UL * 1000UL) / ((ulong)Stopwatch.Frequency);
      private string _failureReason = string.Empty;
      private readonly FrameworkAdapter _framework;
   }
}
