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
using System.IO;
using System.Reflection;

namespace csUnit.Interfaces {
   /// <summary>
   /// This interface is implemented by classes that have direct or indirect
   /// access to assemblies containing tests. Potential implementors include 
   /// visitors, loaders, etc.
   /// </summary>
   public interface ITestAssembly : IDisposable {
      #region Events
      /// <summary>
      /// Fired when an assembly containing tests has changed.
      /// </summary>
      event AssemblyEventHandler AssemblyChanged;

      /// <summary>
      /// Fired when the assembly has been loaded.
      /// </summary>
      event AssemblyEventHandler AssemblyLoaded;

      /// <summary>
      /// Fired when execution of tests within the assembly are about to start.
      /// </summary>
      event AssemblyEventHandler AssemblyStarted;

      /// <summary>
      /// Fired when execution of tests within the assembly has finished.
      /// </summary>
      event AssemblyEventHandler AssemblyFinished;

      /// <summary>
      /// Fired when execution of tests has been aborted.
      /// </summary>
      event AssemblyEventHandler TestsAborted;

      /// <summary>
      /// Fired immediately before a test is executed.
      /// </summary>
      event TestEventHandler TestStarted;

      /// <summary>
      /// Fired when a test has successfully been executed.
      /// </summary>
      event TestEventHandler TestPassed;

      /// <summary>
      /// Fired when an failure was detected during the execution of a test.
      /// </summary>
      event TestEventHandler TestFailed;

      /// <summary>
      /// Fired when an error was detected during the execution of a test.
      /// </summary>
      event TestEventHandler TestError;

      /// <summary>
      /// Fired when a test was skipped.
      /// </summary>
      event TestEventHandler TestSkipped;
      #endregion // Events

      #region Properties
      /// <summary>
      /// Gets the AssemblyName for the test assembly.
      /// </summary>
      AssemblyName Name { get; }

      /// <summary>
      /// Gets time stamp of when the assembly was last modified.
      /// </summary>
      DateTime ModifiedTimeStamp { get; }

      /// <summary>
      /// Gets a collection with all test fixture infos in the assembly.
      /// </summary>
      TestFixtureInfoCollection TestFixtureInfos { get; }

      /// <summary>
      /// Sets the TextWriter to which any console output is being written.
      /// </summary>
      void SetConsoleOutputTo(TextWriter value);

      #endregion // Properties

      #region Methods
      /// <summary>
      /// Reloads the associated assembly.
      /// </summary>
      void Refresh();

      /// <summary>
      /// Executes the test run for this assembly.
      /// </summary>
      /// <param name="testRun">Test run describing which tests to run and how.</param>
      void RunTests(ITestRun testRun);

      /// <summary>
      /// Abort the test run.
      /// </summary>
      void Abort();
      #endregion // Methods

      #region IDisposable Members
      new void Dispose();
      #endregion // IDisposable Members
   }
}
