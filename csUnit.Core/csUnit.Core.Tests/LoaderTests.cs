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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

using csUnit.Core.Criteria;
using csUnit.Interfaces;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace csUnit.Core.Tests {
   /// <summary>
   /// Implements ITestListener. Used for testing Loader/RemoteLoader 
   /// communiation.
   /// </summary>
   internal class MyTestListener : MarshalByRefObject, ITestListener {
      public MyTestListener() {
      }

      public MyTestListener(Loader loader) {
         _loader = loader;
         RegisterEventHandlers();
      }

      public string[] ExecutedTests {
         get {
            return (string[]) _executedTests.ToArray(typeof(string));
         }
      }

      public int FailedCount {
         get {
            return _failedCount;
         }
      }

      public bool TestsRunning {
         get {
            return _testsRunning;
         }
      }

      public long Duration {
         get {
            return _duration;
         }
      }

      public int AssembliesStartedCount {
         get {
            return _assembliesStartedCount;
         }
      }

      public int AssembliesFinishedCount {
         get {
            return _assembliesFinishedCount;
         }
      }

      public int AssembliesLoadedCount {
         get {
            return _assembliesLoadedCount;
         }
      }

      private void RegisterEventHandlers() {
         _loader.AssemblyFinished   += LoaderAssemblyFinished;
         _loader.AssemblyLoaded     += LoaderAssemblyLoaded;
         _loader.AssemblyStarted    += LoaderAssemblyStarted;
         _loader.TestError          += LoaderTestError;
         _loader.TestFailed         += LoaderTestFailed;
         _loader.TestPassed         += LoaderTestPassed;
         _loader.TestsAborted       += LoaderTestsAborted;
         _loader.TestSkipped        += LoaderTestSkipped;
         _loader.TestStarted        += LoaderTestStarted;
      }

      private static string MakeCanonicalName(TestResultEventArgs args) {
         return args.ClassName + "." + args.MethodName;
      }

      #region Event Handlers

      private void LoaderAssemblyFinished(object sender, AssemblyEventArgs args) {
         _assembliesFinishedCount++;
         _testsRunning = false;
      }

      private void LoaderAssemblyLoaded(object sender, AssemblyEventArgs args) {
         _assembliesLoadedCount++;
      }

      private void LoaderAssemblyStarted(object sender, AssemblyEventArgs args) {
        _assembliesStartedCount++;
      }

      private void LoaderTestError(object sender, TestResultEventArgs args) {
         RegisterTestResult(args);
      }

      private void RegisterTestResult(TestResultEventArgs args) {
         _executedTests.Add(MakeCanonicalName(args));
         _duration += args.Duration > 0 ? 1 : 0;
      }

      private void LoaderTestFailed(object sender, TestResultEventArgs args) {
         RegisterTestResult(args);
         _failedCount++;
      }

      private void LoaderTestPassed(object sender, TestResultEventArgs args) {
         RegisterTestResult(args);
      }

      private static void LoaderTestsAborted(object sender, AssemblyEventArgs args) {
      }

      private static void LoaderTestSkipped(object sender, TestResultEventArgs args) {
      }

      private static void LoaderTestStarted(object sender, TestResultEventArgs args) {
      }

      #endregion

      private readonly Loader    _loader;
      private int       _failedCount;
      private readonly ArrayList _executedTests = new ArrayList();
      private long _duration = 1;
      private int _assembliesStartedCount;
      private int _assembliesFinishedCount;
      private int _assembliesLoadedCount;
      private bool _testsRunning = true;

      #region ITestListener Members

      public void OnAssemblyLoaded(object sender, AssemblyEventArgs args) {
      }

      public void OnAssemblyStarted(object sender, AssemblyEventArgs args) {
      }

      public void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
      }

      public void OnTestsAborted(object sender, AssemblyEventArgs args) {
      }

      public void OnTestStarted(object sender, TestResultEventArgs args) {
      }

      public void OnTestPassed(object sender, TestResultEventArgs args) {
      }

      public void OnTestError(object sender, TestResultEventArgs args) {
      }

      public void OnTestFailed(object sender, TestResultEventArgs args) {
      }

      public void OnTestSkipped(object sender, TestResultEventArgs args) {
      }

      #endregion
   }

   /// <summary>
   /// The LoaderTests class contains test for csUnit.Core.Loader
   /// </summary>
   [TestFixture]
   public class LoaderTests {
      private readonly string _assemblyPathName = Util.NormalizePath(Path.Combine(Util.SolutionCodeBase, "TestDll/bin/debug/TestDll.dll"));
      
      [Test]
      public void TestAssemblyUnloads() {
         var initialModuleCount = GetModuleCount();

#if DEBUG
         Debug.WriteLine(string.Format("Current domain = '{0}' (LoaderTests.cs, line 219)", 
            AppDomain.CurrentDomain.FriendlyName));
#endif
         var listener = ExecuteLoaderAndListen(new TestRun(new AllTestsCriterion()));

         Assert.Equals(initialModuleCount, GetModuleCount(), 
            "Loaded module count mismatch. AppDomain has not been unloaded.");
         Assert.Equals(3, listener.ExecutedTests.Length, "ExecutedCount mismatch.");
         Assert.Equals(1, listener.FailedCount, "FailedCount mismatch.");
      }

      [Test]
      public void UnloadsAfterExecutionOfSubset() {
         var initialModuleCount = GetModuleCount();

         var setCriterion = new MultipleTestsCriterion();
         setCriterion.Add("TestDll", "TestDll.ClassWithTests", "ASucceedingTest");
         var listener = ExecuteLoaderAndListen(new TestRun(setCriterion));

         Assert.Equals(1, listener.ExecutedTests.Length, "ExecutedCount mismatch.");
         Assert.Contains("TestDll.ClassWithTests.ASucceedingTest", listener.ExecutedTests);
         Assert.Equals(initialModuleCount, GetModuleCount(), 
            "Loaded module count mismatch. AppDomain has not been unloaded.");
      }

      [TestFixture]
      private class SampleTestFixture {
         [Test]
         public void Foo() {
            Thread.Sleep(1);
         }
      }

      private class AssemblyMock : IAssembly {
         #region IAssembly Members
         public void Load(AssemblyName assemblyName) {
         }

         public string FullName {
            get {
               return "TheFullName";
            }
         }

         public string CodeBase {
            get { return "c:\\TheFullName.dll"; }
         }

         public Type[] GetExportedTypes() {
            return new[] {
               typeof(SampleTestFixture)
               };
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         #endregion
      }

      // TODO: The following test should be in RemoteLoaderTests.cs
      // [29jun08, ml]
      [Test]
      public void TestDurationReported() {
         AssemblyFactory.Type = typeof(AssemblyMock);
         var remoteLoader = new RemoteLoader();
         var listener = new MyTestListener();
         remoteLoader.Listener = listener;
         remoteLoader.LoadAssembly(new AssemblyName{CodeBase = _assemblyPathName});
         remoteLoader.RunTests(new TestRun(new AllTestsCriterion()), new StringWriter());
         Assert.Greater(listener.Duration, 0);
      }

      [Test]
      public void MultipleEventListeners() {
         var loader = new Loader(_assemblyPathName);
         var listener1 = new MyTestListener(loader);
         var listener2 = new MyTestListener(loader);
         loader.RunTests(new TestRun(new AllTestsCriterion()));
         do {
            Thread.Sleep(100);
         } while( listener1.TestsRunning );
         loader.Dispose();
         Assert.Equals(1, listener1.AssembliesLoadedCount);
         Assert.Equals(1, listener2.AssembliesLoadedCount);
         Assert.Equals(1, listener1.AssembliesStartedCount);
         Assert.Equals(1, listener2.AssembliesStartedCount);
         Assert.Equals(1, listener1.AssembliesFinishedCount);
         Assert.Equals(1, listener2.AssembliesFinishedCount);
      }

      private static int GetModuleCount() {
         var currentProcess = Process.GetCurrentProcess();
         try {
            return currentProcess.Modules.Count;
         }
         catch(Win32Exception) {
            // Wait until input message loop is empty and retry.
            currentProcess.WaitForInputIdle();
            return currentProcess.Modules.Count;
         }
      }

      private MyTestListener ExecuteLoaderAndListen(ITestRun testRun) {
         var loader = new Loader(_assemblyPathName);
         var listener = new MyTestListener(loader);
         loader.RunTests(testRun);
         do {
            Thread.Sleep(200);
         } while (listener.TestsRunning);
         loader.Dispose();
         Thread.SpinWait(20);
         return listener;
      }
   }
}
// ReSharper restore UnusedMember.Global
// ReSharper restore UnusedMember.Local
