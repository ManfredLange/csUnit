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
using System.IO;
using System.Reflection;
using System.Threading;
using csUnit.Core.Adapters;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Summary description for TestClass.
   /// </summary>
   internal class TestFixture : MarshalByRefObject, ITestFixture {
      /// <summary>
      /// Constructs a TestClass object for the given test class.
      /// </summary>
      /// <param name="testClassType">The Type object of the test fixture.</param>
      public TestFixture(Type testClassType) {
         _fixtureType = testClassType;
         _framework = FrameworkAdapter.CreateInstance(_fixtureType.Assembly.GetName().Name);
         var attribs = (TestFixtureAttribute[])_fixtureType.GetCustomAttributes(typeof(TestFixtureAttribute), true);
         if(attribs != null && attribs.Length > 0) {
            _categories.Add(attribs[0]._Categories);
         }
         ScanForIgnoreAttribute(_fixtureType);
         ScanForTestMethods();
      }

      /// <summary>
      /// Returns a reference to the fixture instance.
      /// </summary>
      public object FixtureInstance {
         get {
            if(_fixtureInstance == null) {
               CreateObject();
            }
            return _fixtureInstance;
         }
      }

      /// <summary>
      /// Gets a boolean value indicating whether the fixture should be ignored
      /// during a test run.
      /// </summary>
      public bool IsIgnored {
         get {
            if(Parent != null
               && Parent.IsIgnored) {
               return true;
            }
            return _bIgnore;
         }
      }

      /// <summary>
      /// Gets a string with the reason for ignoring the test fixture.
      /// </summary>
      public string IgnoreReason {
         get {
            return _bIgnoreReason;
         }
      }

      /// <summary>
      /// Creates an instance of a fixture.
      /// </summary>
      /// <returns></returns>
      private bool CreateObject() {
         var ci = _fixtureType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public, null,
            CallingConventions.HasThis, new Type[0], null);

         if(ci != null) {
            return InvokeMethod(ci);
         }
         
         var trea = new TestResultEventArgs(AssemblyName,
                                            Name, string.Empty, "No public default constructor found.", 0)
                       {
                          AssertCount = _framework.AssertionCount
                       };
         _testListener.OnTestError(new TestFixtureInfo(this), trea);
         return false;
      }

      public void Execute(ITestRun testRun, ITestListener listener) {
         Use(listener);
         try {
            SafeCurrentDirectory();
            if( CreateObject()
               && FixtureSetUp() ) {
               foreach( var testMethod in _testMethods ) {
                  if( !testMethod.Ignore ) {
                     if( testRun.Contains(testMethod)
                        && SetUp(testMethod) ) {
                        testMethod.Execute(_testListener);
                        TearDown(testMethod);
                     }
                  }
                  else {
                     _testListener.OnTestSkipped(new TestFixtureInfo(this),
                                                 new TestResultEventArgs(
                                                    AssemblyName,
                                                    FullName,
                                                    testMethod.Name,
                                                    testMethod.IgnoreReason,
                                                    0){TestResult = TestResultCategory.Skipped});
                  }
               }
               FixtureTearDown();
            }
         }
         finally {
            RestoreCurrentDirectory();
         }
      }

      #region Current Directory Handling
      private string _storedCurrentDirectory = string.Empty;
      private void SafeCurrentDirectory() {
         _storedCurrentDirectory = Directory.GetCurrentDirectory();
         Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
      }
      private void RestoreCurrentDirectory() {
         Directory.SetCurrentDirectory(_storedCurrentDirectory);
      }
      #endregion Current Directory Handling

      private void Use(ITestListener listener) {
         _testListener = listener;
      }

      private bool InvokeMethod(MethodEntry testMethod) {
         if(testMethod != null) {
            testMethod.Listener = _testListener;
            try {
               _framework.InvokeMethod(FixtureInstance, testMethod);
            }
            catch(ThreadAbortException) {
               throw;
            }
            catch(Exception e) {
               ReportException(e, FullName, testMethod.Name);
               return false;
            }
         }
         return true;
      }

      private bool InvokeMethod(MethodBase mi) {
         if(mi != null) {
            try {
               var ci = mi as ConstructorInfo;
               if(ci != null) {
                  _fixtureInstance = ci.Invoke(new object[] { });
               }
               else {
                  mi.Invoke(FixtureInstance, new object[0]);
               }
            }
            catch(ThreadAbortException) {
               throw;
            }
            catch(Exception e) {
               ReportException(e, FullName, mi.Name);
               return false;
            }
         }
         return true;
      }

      /// <summary>
      /// Gets a set containing all categories this fixture is assigned to.
      /// </summary>
      /// <remarks>Never returns null, but the returned set can potentially be
      /// empty.</remarks>
      public Categories Categories {
         get {
            return _categories;
         }
      }

      /// <summary>
      /// Gets all categories that the fixture inherited.
      /// </summary>
      /// <remarks>If a fixture is declared within a different test fixture,
      /// it inherits the categories of the declaring type.</remarks>
      public Categories InheritedCategories {
         get {
            return Parent != null ? Parent.Categories : Categories.Empty;
         }
      }

      private TestFixture Parent {
         get {
            if( _parentChecked == false ) {
               if(_fixtureType.DeclaringType != null) {
                  var type = _fixtureType.DeclaringType;
                  var attr = (TestFixtureAttribute[])type.GetCustomAttributes(typeof(TestFixtureAttribute), true);
                  if(attr != null && attr.Length > 0) {
                     _parent = new TestFixture(type);
                  }
               }
               _parentChecked = true;
            }
            return _parent;
         }
      }

      /// <summary>
      /// Gets an array with the TestMethod of this test class.  If the category of this
      /// fixture is declared, then only those methods that match the category are
      /// returned.
      /// </summary>
      public List<ITestMethodInfo> TestMethods {
         get {
            var methods = new List<ITestMethodInfo>();

            foreach(var tm in _testMethods) {
               methods.Add(new TestMethodInfo(tm));
            }

            return methods;
         }
      }

      private void ScanForIgnoreAttribute(Type aType) {
         var attribs = (IgnoreAttribute[])aType.GetCustomAttributes(typeof(IgnoreAttribute), true);
         if(attribs != null && attribs.Length > 0) {
            _bIgnore = true;
            _bIgnoreReason = attribs[0].Reason;
         }
         else {
            var type = aType.DeclaringType;
            if(null != type) {
               ScanForIgnoreAttribute(type);
               if(_bIgnore) {
                  _bIgnoreReason = "See parent for reason.";
               }
            }
         }
      }

      /// <summary>
      /// Analyzes all of the "public void" methods in the fixture and assigns them
      /// to the appropriate member variables according to their method names or
      /// else their attributes.
      /// </summary>
      private void ScanForTestMethods() {
         foreach(var mi in _fixtureType.GetMethods(
            BindingFlags.Instance|BindingFlags.Public|BindingFlags.Static
            |BindingFlags.NonPublic)) { // NonPublic required for NUnit support. [31mar08, ml]
            var methodEntry = _framework.CreateMethodEntryFrom(this, mi);
            if( methodEntry != null ) {
               // TODO: Replace switch with better code. E.g. 'methodEntry.MethodType' could be key into dictionary. [17mar09, ml]
               switch(methodEntry.MethodType) {
                  case MethodType.FixtureSetUpMethod:
                     _fixtureSetUpMethod = methodEntry;
                     break;
                  case MethodType.FixtureTearDownMethod:
                     _fixtureTearDownMethod = methodEntry;
                     break;
                  case MethodType.SetUpMethod:
                     AddSetUpMethod(methodEntry);
                     break;
                  case MethodType.TearDownMethod:
                     AddTearDownMethod(methodEntry);
                     break;
                  case MethodType.TestMethod:
                     _testMethods.Add(methodEntry);
                     break;
               }
            }
         }
      }

      /// <summary>
      /// Adds the given method as a setup method for this fixture.  If the 
      /// method is marked with a category, then it is stored in the setup
      /// method table.  Without a category, the method is used as the default
      /// setup method.
      /// </summary>
      /// <param name="mi"></param>
      private void AddSetUpMethod(MethodEntry mi) {
         if( mi.Categories.IsEmpty ) {
            if( _defaultSetupMethod == null ) {
               _defaultSetupMethod = mi;
            }
            else if(FullName.Equals(mi.DeclaringTypeFullName)) {
               _defaultSetupMethod.Duplicates.Add(mi);
            }
         }
         else {
            foreach(var category in mi.Categories) {
               if( !_setupMethods.ContainsKey(category)) {
                  _setupMethods.Add(category, mi);
               }
               else {
                  _setupMethods[category].Duplicates.Add(mi);
               }
            }
         }
      }

      /// <summary>
      /// Adds the given method as a tear down method for this fixture.  If the
      /// method is marked with a category, then it is stored in the tear down 
      /// method table.  Without a category, the method is used as the default
      /// tear down method.
      /// </summary>
      /// <param name="method"></param>
      private void AddTearDownMethod(MethodEntry method) {
         if (method.Categories.IsEmpty) {
            if (_defaultTearDownMethod == null) {
               _defaultTearDownMethod = method;
            }
            else if (FullName.Equals(method.DeclaringTypeFullName)) {
               _defaultTearDownMethod.Duplicates.Add(method);
            }
         }
         else {
            foreach (var category in method.Categories) {
               if (!_tearDownMethods.ContainsKey(category)) {
                  _tearDownMethods.Add(category, method);
               }
               else {
                  _tearDownMethods[category].Duplicates.Add(method);
               }
            }
         }
      }

      /// <summary>
      /// Returns the display name of the assembly.
      /// </summary>
      public string AssemblyName {
         get {
            return _fixtureType.Assembly.FullName != null ? 
               _fixtureType.Assembly.FullName.Split(",".ToCharArray())[0] : null;
         }
      }

      /// <summary>
      /// Gets the class name of the test fixture.
      /// </summary>
      private String Name {
         get {
            return _fixtureType.Name;
         }
      }

      /// <summary>
      /// Gets the full class name of the test fixture.
      /// </summary>
      public String FullName {
         get {
            return _fixtureType.FullName;
         }
      }

      /// <summary>
      /// Reports an exception.
      /// </summary>
      /// <param name="ex">The exception</param>
      /// <param name="fixtureName">Name of the fixture.</param>
      /// <param name="testMethodName">Name of the test method.</param>
      private void ReportException(Exception ex, String fixtureName, String testMethodName) {
         var realException = ex;

         if( realException is TargetInvocationException ) {
            realException = realException.InnerException;
         }

         var trea = new TestResultEventArgs(AssemblyName, fixtureName, testMethodName,
                                            realException.ToString(), 0) {
                          AssertCount = _framework.AssertionCount
                       };
         Util.ExtractLocation(realException, trea);
         if( realException.GetType() == typeof(TestFailed) ) {
            _testListener.OnTestFailed(new TestFixtureInfo(this), trea);
         }
         else if( realException is ThreadAbortException ) {
            throw realException;
         }
         else {
            _testListener.OnTestError(new TestFixtureInfo(this), trea);
         }
      }

      /// <summary>
      /// Called once for each test immediately before the test is executed.
      /// </summary>
      /// <returns>True if all called setup method returned success, false 
      /// otherwise.</returns>
      /// <remarks>If a default setup method exists (one without any categories)
      /// it will be executed in all cases if it is the only one. If more than
      /// one setup method exists it will be executed only if included by any
      /// of the filters. The order in which the setup methods are executed is
      /// not predetermined. The runtime environment verifies that no two setup
      /// method exist with the same set of categories.</remarks>
      private bool SetUp(ITestMethod method) {
         var bDefaultExecuted = false;
         if(   _setupMethods.Count > 0
            && !method.Categories.IsEmpty) {
            foreach(var category in method.Categories) {
               if(_setupMethods.ContainsKey(category)) {
                  if(!InvokeMethod(_setupMethods[category])) {
                     return false;
                  }
               }
               else if(_defaultSetupMethod != null
                  && bDefaultExecuted == false) {
                  if(!InvokeMethod(_defaultSetupMethod)) {
                     return false;
                  }
                  bDefaultExecuted = true;
               }
            }
         }
         else if( _defaultSetupMethod != null ) {
            if(!InvokeMethod(_defaultSetupMethod)) {
               return false;
            }
         }
         return true;
      }

      /// <summary>
      /// Called once after all tests within this test case have run.
      /// </summary>
      private void TearDown(ITestMethod method) {
         if(   _tearDownMethods.Count > 0
            && !method.Categories.IsEmpty) {
            foreach(var category in method.Categories) {
               if(_tearDownMethods.ContainsKey(category)) {
                  InvokeMethod(_tearDownMethods[category]);
               }
            }
         }
         else if(_defaultTearDownMethod != null) {
            InvokeMethod(_defaultTearDownMethod);
         }
      }

      /// <summary>
      /// Called once for each test fixture after the constructor has been
      /// called and just before the tests in the fixture are about to be
      /// executed.
      /// </summary>
      /// <returns>'true', if successful, 'false' otherwise.</returns>
      private bool FixtureSetUp() {
         // TODO: Here we should also support the use of categories. [22Aug2006, ml]
         return InvokeMethod(_fixtureSetUpMethod);
      }

      /// <summary>
      /// Called once for each test fixture after all tests in the fixture have
      /// been executed.
      /// </summary>
      private void FixtureTearDown() {
         // TODO: Here we should also support the use of categories. [22Aug2006, ml]
         InvokeMethod(_fixtureTearDownMethod);
      }


      private TestFixture _parent;
      private bool _parentChecked;

      private bool _bIgnore;
      private string _bIgnoreReason = string.Empty;
      private ITestListener      _testListener;
      private readonly Type               _fixtureType;

      private MethodEntry _fixtureSetUpMethod;
      private MethodEntry _fixtureTearDownMethod;

      private readonly Dictionary<String, MethodEntry> _setupMethods = new Dictionary<string, MethodEntry>();
      private MethodEntry _defaultSetupMethod;

      private readonly Dictionary<String, MethodEntry> _tearDownMethods = new Dictionary<String, MethodEntry>();
      private MethodEntry _defaultTearDownMethod;

      private readonly List<MethodEntry> _testMethods = new List<MethodEntry>();

      private readonly Categories _categories = new Categories();

      private object _fixtureInstance;
      private readonly FrameworkAdapter _framework;
   }
}
