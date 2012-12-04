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
using System.Security.Permissions;
using System.Threading;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Instantiated in the AppDomain of the tested assembly, the remote loader 
   /// loads the test classes and keeps the classes in their own AppDomain. 
   /// Without this loader, the test classes could crash the csUnitRunner, which
   /// is not desirable.
   /// </summary>
   [FileIOPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
   internal class RemoteLoader : MarshalByRefObject, IDisposable {
      #region Constructing
      public static RemoteLoader CreateInstance(AppDomain remoteDomain, string csUnitRoot) {
         try {
            remoteDomain.SetData("csUnitRoot", csUnitRoot);
            var remoteLoader = (RemoteLoader) remoteDomain.CreateInstanceFromAndUnwrap(
                  Path.Combine(csUnitRoot, "csUnit.Core.dll"),
                  typeof(RemoteLoader).FullName);
            return remoteLoader;
         }
         catch (Exception ex) {
            Debug.WriteLine(string.Format("## Listing assemblies in domain '{0}'##", remoteDomain.FriendlyName));
            foreach (var assembly in remoteDomain.GetAssemblies()) {
               Debug.WriteLine(assembly.FullName);
            }
            Debug.WriteLine("## end of list ##");
            Debug.WriteLine("Could not instantiate remote loader. " + ex);
            return null;
         }
      }

      public RemoteLoader() {
         AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
      }

      private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args) {
         var assemblyName = new AssemblyName(args.Name);
         var csUnitRoot = AppDomain.CurrentDomain.GetData("csUnitRoot") as string;
         Assembly assembly = null;
         if(assemblyName.Name.Equals("csUnit.Core")) {
            assembly = Assembly.LoadFrom(csUnitRoot + "csUnit.Core.dll");
         }
         else if( assemblyName.Name.Equals("csUnit")) {
            assembly = Assembly.LoadFrom(csUnitRoot + "csUnit.dll");
         }
         else if( assemblyName.Name.Equals("csUnit.Core.Visitors")) {
            assembly = Assembly.LoadFrom(csUnitRoot + "csUnit.Core.Visitors.dll");
         }
         else if (assemblyName.Name.Equals("csUnit.Common")) {
            assembly = Assembly.LoadFrom(csUnitRoot + "csUnit.Common.dll");
         }
         else if (assemblyName.Name.Equals("csUnit.Interfaces")) {
            assembly = Assembly.LoadFrom(csUnitRoot + "csUnit.Interfaces.dll");
         }
         // Last option: Let's try to find assembly by filename and path:
         if ( assembly == null
            && File.Exists(args.Name)) {
            assembly = Assembly.LoadFrom(args.Name);
         }
         if (assembly == null) {
            Debug.WriteLine(string.Format("RemoteLoader (approx. line 90) couldn't resolve assembly '{0}'", 
               args.Name));
            // All csUnit related assemblies come from the same directory.
         }
         return assembly;
      }
      #endregion

      #region IDisposable Members

      public void Dispose() {
         _listener = null;
         _testAssembly = null;
      }

      #endregion

      #region Properties
      public string AssemblyFullName {
         get {
            return _testAssembly.FullName;
         }
      }

      public ITestListener Listener {
         set {
            if(null == value) {
               throw new ArgumentNullException("value", "Must specifiy an ITestListener implementor.");
            }
            _listener = value;
         }
      }

      public DateTime ModifiedTimeStamp {
         get {
            return _testAssembly.ModifiedTimeStamp;
         }
      }

      public TestFixtureInfoCollection TestFixtureInfos {
         get {
            try {
               return _testAssembly.TestFixtureInfos;
            }
            catch(FileNotFoundException ex) {
               Debug.WriteLine(ex.Message);
               return new TestFixtureInfoCollection();
            }
         }
      }
      #endregion

      #region Public Methods
      /// <summary>
      /// Loads an assembly given the assembly name.
      /// </summary>
      /// <param name="assemblyName">Assembly name object.</param>
      public void LoadAssembly(AssemblyName assemblyName) {
         _testAssembly =  new TestAssembly(assemblyName);
         if (_listener != null) {
            _listener.OnAssemblyLoaded(null, new AssemblyEventArgs( _testAssembly.FullName, assemblyName.CodeBase));
         }
      }

      /// <summary>
      /// Executes a test run. The ITestRun object specifies which tests to run 
      /// and how. If the tests write to the console (either Error or Out) this 
      /// output will be redirected to 'twConsole'.
      /// </summary>
      /// <param name="testRun">ITestRun object describing what tests to run and how.</param>
      /// <param name="twConsole">TextWriter to where Out and Error are redirected.</param>
      public void RunTests(ITestRun testRun, TextWriter twConsole) {
         SetUpConsoleAndWorkingDirectory(twConsole);

         if( _listener != null ) {
            _listener.OnAssemblyStarted(null, new AssemblyEventArgs(_testAssembly.FullName, _testAssembly.CodeBase));
         }

         try {
            _testAssembly.RunTests(testRun, _listener);
         }
         catch( Exception ex ) {
            if( _listener != null ) {
               _listener.OnTestsAborted(null, new AssemblyEventArgs(_testAssembly.FullName, _testAssembly.CodeBase));
            }
            CheckAndHandleCsUnitCrash(ex);
         }
         finally {
            if( _listener != null ) {
               _listener.OnAssemblyFinished(null, new AssemblyEventArgs(_testAssembly.FullName, _testAssembly.CodeBase));
            }
            RestoreConsoleAndWorkingDirectory();
         }
      }
      #endregion

      #region Private Methods

      /// <summary>
      /// Theoretically this shouldn't happen. However, if it happens, we would
      /// like to see it as prominently as possible. If we fail, we want to fail
      /// as fast as possible.
      /// </summary>
      /// <param name="ex">The exception csUnitCore didn't know how to deal with
      /// it.</param>
      private static void CheckAndHandleCsUnitCrash(Exception ex) {
         if( ! ex.GetType().Equals(typeof(ThreadAbortException)) ) {
            Console.WriteLine("### Detected in RemoteLoader.cs, line 165 ###");
            Console.WriteLine("### Runtime error in csUnitCore ###");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            Console.WriteLine(ex.StackTrace);
         }
      }
      
      /// <summary>
      /// Intercept console error and out, and set the working directory. Both
      /// will be restored after the tests have been run.
      /// </summary>
      /// <param name="twConsole">TextWriter to which to write the intercepted 
      /// console error and out.</param>
      private void SetUpConsoleAndWorkingDirectory(TextWriter twConsole) {
         InterceptConsole(twConsole);
         
         // Set the CWD to be the app domain's base directory
         _savedWorkingDirectory = Directory.GetCurrentDirectory(); 
         Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); 
      }

      /// <summary>
      /// Console and working directory might have been modified by tests or the
      /// core. This method sets both to what they were before the tests were
      /// executed.
      /// </summary>
      private void RestoreConsoleAndWorkingDirectory() {
         // Restore the CWD
         Directory.SetCurrentDirectory(_savedWorkingDirectory);

         RestoreConsole();
      }

      /// <summary>
      /// Intercept console out and error and write to both the current out
      /// and error and the TextWriter object that is passed into the method.
      /// </summary>
      /// <param name="twConsole">A TextWriter to which to write in addition 
      /// to what console out and error are currently set.</param>
      private void InterceptConsole(TextWriter twConsole) {
         _savedConsoleOut = Console.Out;
         _savedConsoleError = Console.Error;

         var consoleOutMultiplexer = new TextWriterMultiplexer();
         consoleOutMultiplexer.AddTextWriter(_savedConsoleOut);
         consoleOutMultiplexer.AddTextWriter(twConsole);
         
         var consoleErrorMultiplexer = new TextWriterMultiplexer();
         consoleErrorMultiplexer.AddTextWriter(_savedConsoleError);
         consoleErrorMultiplexer.AddTextWriter(twConsole);

         Console.SetOut(consoleOutMultiplexer);
         Console.SetError(consoleErrorMultiplexer);
      }

      /// <summary>
      /// Restores console out and error to what they were before the tests
      /// were executed. If tests changed them this will also clean that up.
      /// </summary>
      private void RestoreConsole() {
         Console.SetOut(_savedConsoleOut);
         Console.SetError(_savedConsoleError);
      }

      #endregion // Private Methods

      #region Fields
      private ITestListener   _listener;
      private TestAssembly    _testAssembly;
      private TextWriter      _savedConsoleOut;
      private TextWriter      _savedConsoleError;
      private string          _savedWorkingDirectory = string.Empty;
      #endregion // Fields
   }
}
