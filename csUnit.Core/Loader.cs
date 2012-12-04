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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

using csUnit.Interfaces;

namespace csUnit.Core {
   internal class Loader : ITestAssembly {
      public Loader(string assemblyPathName) {
         _assemblyName = AssemblyName.GetAssemblyName(assemblyPathName);
         _listener = new TestEventSink(this);
         var appDomain = CreateRemoteDomain();

         try {
            using (var remoteLoader = LoadAssembly(appDomain)) {
               TestFixtureInfos = remoteLoader.TestFixtureInfos;
               _modifiedTimeStamp = remoteLoader.ModifiedTimeStamp;
            }
         }
         catch(Exception ex) {
            Debug.WriteLine(ex);
            var directory = appDomain.BaseDirectory;
            Debug.WriteLine(string.Format("Remote AppDomain's base directory: {0}", directory));
         }
         finally {
            TearDownAppDomain(appDomain);
         }
      }

      #region IDisposable Members

      public void Dispose() {
         AbortExistingWorkerThread();
      }

      #endregion

      #region Properties

      public TestFixtureInfoCollection TestFixtureInfos { get; private set; }

      /// <summary>
      /// Gets/Sets the TextWriter to which any console output is being written.
      /// </summary>
      public void SetConsoleOutputTo(TextWriter value) {
         _twConsole = value;
      }

      #endregion // Properties

      #region Public Methods
      public void RunTests(ITestRun testRun) {
         _testRun = testRun;
         StartWorkerThread();
      }

      public void Abort() {
         AbortExistingWorkerThread();
      }
      #endregion

      #region Private Methods

      private void AbortExistingWorkerThread() {
         if (_workerThread != null
            && _workerThread.IsAlive ) {
            var worker = new Thread(FooWorker) {
               Name = "Worker for aborting worker thread",
               IsBackground = true
            };
            worker.Start();
         }
      }

      private void FooWorker() {
         _listener.SetThreadAbortFlag();
         lock (_workerThread) {
            if(_workerThread != null) {
               if((_workerThread.ThreadState
                   & System.Threading.ThreadState.Unstarted) == 0) {
                  var terminated = _workerThread.Join(60000);
                  if(!terminated) {
                     // not nice but necessary [28mar07, ml]
                     _workerThread.Abort();
                  }
                  else {
                     Debug.WriteLine("Worker thread was properly aborted.");
                  }
               }
               if(_appDomain != null) {
                  TearDownAppDomain(_appDomain);
                  _appDomain = null;
               }
               _workerThread = null;
            }
            if(TestsAborted != null) {
               TestsAborted(null,
                            new AssemblyEventArgs(_assemblyName.FullName,
                                                  _assemblyName.CodeBase));
            }
         }
      }

      private void StartWorkerThread() {
         AbortExistingWorkerThread();
         _workerThread = new Thread(ThreadProc);
         _workerThread.Name = "Worker for " + _assemblyName.FullName;
         _workerThread.IsBackground = true;
         _workerThread.SetApartmentState(ApartmentState.STA);
         _listener.ClearThreadAbortFlag();
         _workerThread.Start();
      }

      /// <summary>
      /// Runs the tests using the remote loader constructed in the LoadAssembly
      /// method.
      /// </summary>
      private void ThreadProc() {
         AppDomain appDomain = null;
         RuntimeHelpers.PrepareConstrainedRegions();
         try {
            _appDomain = null;
            appDomain = CreateRemoteDomain();
#if DEBUG
            Debug.WriteLine(string.Format("Worker thread {0} executing in appdomain '{1}'",
               Thread.CurrentThread.ManagedThreadId, appDomain.FriendlyName));
#endif
            var currentFullName = _assemblyName.FullName;
            var currentModifiedTimeStamp = ModifiedTimeStamp;

            using (var remoteLoader = LoadAssembly(appDomain)) {
               TestFixtureInfos = remoteLoader.TestFixtureInfos;
               _modifiedTimeStamp = remoteLoader.ModifiedTimeStamp;

               if (_assemblyName.FullName != currentFullName
                  || ModifiedTimeStamp != currentModifiedTimeStamp) {
                  if (AssemblyChanged != null) {
                     AssemblyChanged(this, new AssemblyEventArgs(_assemblyName.FullName, _assemblyName.CodeBase));
                  }
               }
               
               remoteLoader.Listener = _listener;
               remoteLoader.RunTests(_testRun, _twConsole);
               remoteLoader.Dispose();
            }
         }
         catch (ThreadAbortException ex) {
            _appDomain = appDomain;
            Debug.WriteLine("Worker thread has aborted because: " + ex.Message);
            Debug.WriteLine(ex.ToString());
         }
         catch (Exception ex) {
            _appDomain = appDomain;
            if (TestsAborted != null) {
               TestsAborted(null, new AssemblyEventArgs(_assemblyName.FullName, _assemblyName.CodeBase));
            }
            Debug.WriteLine("Worker thread has terminated because: " + ex.Message);
            Debug.WriteLine(ex.ToString());
         }
         finally {
            if (appDomain != null && _appDomain == null) {
               TearDownAppDomain(appDomain);
            }
//            _workerThread = null;
            _testRun = null;
         }
      }

      private RemoteLoader LoadAssembly(AppDomain appDomain) {
         var lastSlash = typeof(Loader).Assembly.CodeBase.LastIndexOf("/");
         var csUnitRoot = typeof(Loader).Assembly.CodeBase.Substring(0, lastSlash + 1);
         var remoteLoader = RemoteLoader.CreateInstance(appDomain, csUnitRoot);
         if (remoteLoader != null) {
            try {
               remoteLoader.Listener = _listener;
               remoteLoader.LoadAssembly(_assemblyName);
            }
            catch (Exception ex) {
               Debug.WriteLine(ex);
            }
         }
         else {
            throw (new Exception("Could not create RemoteLoader instance."));
         }
         return remoteLoader;
      }

      private AppDomain CreateRemoteDomain() {
         var friendlyName = string.Format("{0}:TestExecutor-{1}",
                                          AppDomain.CurrentDomain.FriendlyName,
                                          _assemblyName.Name);
         var domainSetup = new AppDomainSetup();
         var uri = new Uri(_assemblyName.CodeBase);
         var lastSlash = uri.OriginalString.LastIndexOf('/');
         var appBase = uri.OriginalString.Substring(0, lastSlash) + "\\";
         domainSetup.ApplicationBase = appBase;
         domainSetup.ApplicationName = _assemblyName.Name;

         domainSetup.ShadowCopyFiles = "true";
         // Uses CLR's download cache, which is the default location. [29aug09, ml]
         
         domainSetup.ShadowCopyDirectories = appBase; 
         // This might not be good enough since it would not shadow copy subfolders... [02sep09, ml]

         var configFile = appBase + "/web.config";
         if(File.Exists(configFile) ) {
            domainSetup.ConfigurationFile = configFile;
         }
         else {
            domainSetup.ConfigurationFile = _assemblyName.CodeBase + ".config";
         }

         var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
         evidence.AddHost(new Zone(SecurityZone.MyComputer));
         var permissions = new PermissionSet(PermissionState.Unrestricted);
         permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));//Execution));
         permissions.AddPermission(new UIPermission(PermissionState.Unrestricted));
         permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
         permissions.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));

         var appDomain = AppDomain.CreateDomain(friendlyName,
                                       //AppDomain.CurrentDomain.Evidence,
                                       null,//evidence,
                                       domainSetup,
                                       permissions,
                                       GetStrongName(Assembly.GetExecutingAssembly()));

         return appDomain;
      }

      private static StrongName GetStrongName(Assembly assembly) {
         if (assembly == null)
            throw new ArgumentNullException("assembly");

         AssemblyName assemblyName = assembly.GetName();
         Debug.Assert(assemblyName != null, "Could not get assembly name");

         // Get the public key blob.
         byte[] publicKey = assemblyName.GetPublicKey();
         if (publicKey == null || publicKey.Length == 0)
            throw new InvalidOperationException("Assembly is not strongly named");

         StrongNamePublicKeyBlob keyBlob = new StrongNamePublicKeyBlob(publicKey);

         // Return the strong name.
         return new StrongName(keyBlob, assemblyName.Name, assemblyName.Version);
      }


      private static void TearDownAppDomain(AppDomain appDomain) {
         if (appDomain != null) {
            var domainName = "unknown";
            try {
               domainName = appDomain.FriendlyName;
               AppDomain.Unload(appDomain);
            }
            catch (AppDomainUnloadedException ex) {
               Debug.WriteLine(string.Format("AppDomain '{0}' is already unloaded. {1}", domainName, ex));
            }
            catch (CannotUnloadAppDomainException ex) {
               Debug.WriteLine(string.Format("System cannot unload appdomain '{0}'. {1}", domainName, ex));
            }
            catch (ThreadAbortException ex) {
               Debug.WriteLine(string.Format("Cannot unload appdomain '{0}' because thread has terminated. {1}", domainName, ex));
            }
         }
      }

      #endregion // Private Methods

      #region ITestAssembly Implementation
      #region Events
      /// <summary>
      /// Fired when an assembly containing tests has changed.
      /// </summary>
      public event AssemblyEventHandler AssemblyChanged;

      /// <summary>
      /// Fired when the assembly has been loaded.
      /// </summary>
      public event AssemblyEventHandler AssemblyLoaded;

      /// <summary>
      /// Fired when execution of tests within the assembly are about to start.
      /// </summary>
      public event AssemblyEventHandler AssemblyStarted;

      /// <summary>
      /// Fired when execution of tests within the assembly has finished.
      /// </summary>
      public event AssemblyEventHandler AssemblyFinished;

      /// <summary>
      /// Fired when execution of tests has been aborted.
      /// </summary>
      public event AssemblyEventHandler TestsAborted;

      public event TestEventHandler TestSkipped;
      public event TestEventHandler TestStarted;
      public event TestEventHandler TestPassed;
      public event TestEventHandler TestFailed;
      public event TestEventHandler TestError;
      #endregion // Events

      public AssemblyName Name {
         get {
            return _assemblyName;
         }
      }

      public DateTime ModifiedTimeStamp {
         get {
            return _modifiedTimeStamp;
         }
      }

      public void Refresh() {
         var appDomain = CreateRemoteDomain();

         try {
            var currentFullName = _assemblyName.FullName;
            var currentModifiedTimeStamp = ModifiedTimeStamp;

            using (var remoteLoader = LoadAssembly(appDomain)) {
               TestFixtureInfos = remoteLoader.TestFixtureInfos;
               _modifiedTimeStamp = remoteLoader.ModifiedTimeStamp;
            }

            if (_assemblyName.FullName != currentFullName
                || ModifiedTimeStamp != currentModifiedTimeStamp) {
               if (AssemblyChanged != null) {
                  AssemblyChanged(this, new AssemblyEventArgs(_assemblyName.FullName, _assemblyName.CodeBase));
               }
            }
         }
         catch(Exception ex) {
            Debug.WriteLine(ex);
            Debug.WriteLine(string.Format("Remote AppDomain's base directory: {0}", appDomain.BaseDirectory));
         }
         finally {
            TearDownAppDomain(appDomain);
         }
      }

      #endregion // ITestAssembly Implementation


      #region TestEventSink
      private class TestEventSink : MarshalByRefObject, ITestListener {
         public TestEventSink(Loader loader) {
            _loader = loader;
         }

         public void ClearThreadAbortFlag() {
            lock (this) {
               _abortThread = false;
            }
         }

         public void SetThreadAbortFlag() {
            lock (this) {
               _abortThread = true;
            }
         }

         private void CheckAbortThread() {
            if (_abortThread) {
               Thread.CurrentThread.Abort();
            }
         }

         private bool _abortThread;

         private static void TestEventHandlerAsyncCallback(IAsyncResult ar) {
            TestEventHandler teh = ar.AsyncState as TestEventHandler;
            if (teh != null) {
               teh.EndInvoke(ar);
            }
         }

         private static void AssemblyEventHandlerAsyncCallBack(IAsyncResult ar) {
            AssemblyEventHandler aeh = ar.AsyncState as AssemblyEventHandler;
            if (aeh != null) {
               aeh.EndInvoke(ar);
            }
         }

         #region ITestListener Members

         public void OnTestSkipped(object sender, TestResultEventArgs args) {
            lock (this) {
               if (_loader.TestSkipped != null) {
                  FireAndForget(_loader.TestSkipped, args);
               }
               CheckAbortThread();
            }
         }

         public void OnTestStarted(object sender, TestResultEventArgs args) {
            lock (this) {
               if (_loader.TestStarted != null) {
                  FireAndForget(_loader.TestStarted, args);
               }
               CheckAbortThread();
            }
         }

         public void OnTestError(object sender, TestResultEventArgs args) {
            lock (this) {
               if (_loader.TestError != null) {
                  FireAndForget(_loader.TestError, args);
               }
               CheckAbortThread();
            }
         }

         public void OnTestPassed(object sender, TestResultEventArgs args) {
            lock (this) {
               if (_loader.TestPassed != null) {
                  FireAndForget(_loader.TestPassed, args);
               }
               CheckAbortThread();
            }
         }

         private void FireAndForget(Delegate handler, TestResultEventArgs args) {
            foreach (TestEventHandler teh in handler.GetInvocationList()) {
               teh.BeginInvoke(_loader, args, TestEventHandlerAsyncCallback, teh);
            }
         }

         public void OnTestFailed(object sender, TestResultEventArgs args) {
            lock (this) {
               if (_loader.TestFailed != null) {
                  FireAndForget(_loader.TestFailed, args);
               }
               CheckAbortThread();
            }
         }

         public void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
            lock (this) {
               if (_loader.AssemblyFinished != null) {
                  foreach (AssemblyEventHandler d in _loader.AssemblyFinished.GetInvocationList()) {
                     d.BeginInvoke(_loader, args, AssemblyEventHandlerAsyncCallBack, d);
                  }
               }
               CheckAbortThread();
            }
         }

         public void OnAssemblyStarted(object sender, AssemblyEventArgs args) {
            lock (this) {
               if (_loader.AssemblyStarted != null) {
                  foreach (AssemblyEventHandler d in _loader.AssemblyStarted.GetInvocationList()) {
                     d.BeginInvoke(_loader, args, AssemblyEventHandlerAsyncCallBack, d);
                  }
               }
               CheckAbortThread();
            }
         }

         public void OnTestsAborted(object sender, AssemblyEventArgs args) {
            // Don't check for aborting the thread in this case. This has 
            // already happened. The thread is about to abort. [28mar07, ml]
         }

         public void OnAssemblyLoaded(object sender, AssemblyEventArgs args) {
            lock (this) {
               if (_loader.AssemblyLoaded != null) {
                  FireAndForget(_loader.AssemblyLoaded, args);
               }
               CheckAbortThread();
            }
         }

         private void FireAndForget(Delegate handler, AssemblyEventArgs args) {
            foreach (AssemblyEventHandler aeh in handler.GetInvocationList()) {
               aeh.BeginInvoke(_loader, args, AssemblyEventHandlerAsyncCallBack, aeh);
            }
         }

         #endregion

         private readonly Loader _loader;
      }
      #endregion
      
      private readonly AssemblyName       _assemblyName;
      private DateTime                    _modifiedTimeStamp = DateTime.MinValue;
      private readonly TestEventSink      _listener;
      private Thread                      _workerThread;
      private AppDomain                   _appDomain;
      private ITestRun                    _testRun;
      private TextWriter                  _twConsole;
   }
}
