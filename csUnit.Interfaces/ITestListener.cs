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

namespace csUnit.Interfaces {
   /// <summary>
   /// Classes, which implement this interface will receive ouput from the
   /// tests.
   /// </summary>
   public interface ITestListener {
      /// <summary>
      /// Called when an assembly has been successfully loaded.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnAssemblyLoaded(object sender, AssemblyEventArgs args);

      /// <summary>
      /// Called when the execution of tests in an assembly has started.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnAssemblyStarted(object sender, AssemblyEventArgs args);

      /// <summary>
      /// Called when execution of all tests within a test suite has been
      /// completed.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnAssemblyFinished(object sender, AssemblyEventArgs args);

      /// <summary>
      /// Called when the execution of the tests has been aborted.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestsAborted(object sender, AssemblyEventArgs args);

      /// <summary>
      /// Called just before a test is started.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestStarted(object sender, TestResultEventArgs args);

      /// <summary>
      /// Called when a test has successfully passed.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestPassed(object sender, TestResultEventArgs args);
      
      /// <summary>
      /// Called when an error has occured during test execution.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestError(object sender, TestResultEventArgs args);

      /// <summary>
      /// Called when a test fails.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestFailed(object sender, TestResultEventArgs args);

      /// <summary>
      /// Called when a test was skipped.
      /// </summary>
      /// <param name="sender">The sender of the message.</param>
      /// <param name="args">Arguments for the message.</param>
      void OnTestSkipped(object sender, TestResultEventArgs args);
   }
}
