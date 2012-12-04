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

namespace csUnit.Interfaces {
   /// <summary>
   /// Enumeration for categorizing test results.
   /// </summary>
   [Serializable]
   public enum TestResultCategory {
      /// <summary>
      /// Test(s) have passed successfully.
      /// </summary>
      Success,
      /// <summary>
      /// An error occured, typically an unexpected exception was thrown.
      /// </summary>
      Error,
      /// <summary>
      /// An assertion failed.
      /// </summary>
      Failure,
      /// <summary>
      /// An expected exception wasn't thrown.
      /// </summary>
      ExpectedExceptionNotThrown,
      /// <summary>
      /// The test(s) was/were skipped.
      /// </summary>
      Skipped
   }

   /// <summary>
   /// An instance of this class contains information about the outcome of the
   /// execution of a test method.
   /// </summary>
   [Serializable]
   public class TestResultEventArgs : EventArgs {
      /// <summary>
      /// Constructs a TestResultEventArgs object with a number parameter. The
      /// parameter fileName can be null, in which case LineNumber is ignored.
      /// </summary>
      /// <param name="assemblyName"></param>
      /// <param name="className"></param>
      /// <param name="methodName"></param>
      /// <param name="reason"></param>
      /// <param name="duration"></param>
      public TestResultEventArgs(string assemblyName, string className, 
         string methodName, string reason, ulong duration) {
         AssemblyName = assemblyName;
         ClassName = className;
         MethodName = methodName;
         Reason = reason;
         Duration = duration;
      }

      /// <summary>
      /// Returns the name of the file in which the error occured, if the error
      /// was caused by an exception.
      /// </summary>
      public string FileName {
         get {
            return StackInfo.Count > 0 ? StackInfo[0].FilePathName : "Unknown location";
         }
      }

      /// <summary>
      /// Returns the line number where the error occured, if the error was caused
      /// by an exception.
      /// </summary>
      public int LineNumber {
         get {
            return StackInfo.Count > 0 ? StackInfo[0].LineNumber : -1;
         }
      }

      /// <summary>
      /// Gets the name of the assembly in which the test resides.
      /// </summary>
      public string AssemblyName { get; private set; }

      /// <summary>
      /// Gets the name of the class, aka the test fixture.
      /// </summary>
      public string ClassName { get; private set; }

      /// <summary>
      /// Gets the name of the test, that is the method within the test fixture.
      /// </summary>
      public string MethodName { get; private set; }
      
      /// <summary>
      /// Gets/sets the reason of a failure or an error. If successfully passed, the
      /// value will be string.Empty.
      /// </summary>
      public string Reason { get; set; }

      public TestResultCategory TestResult { get; set; }

      /// <summary>
      /// Gets the number of nanoseconds a test needed to executed. If not
      /// successful, the value will be 0 (zero).
      /// </summary>
      public ulong Duration { get; set; }

      /// <summary>
      /// Gets the number of assertions invoked during the test run
      /// </summary>
      public int AssertCount { get; set; }

      public TestFailed Failure { get; set; }

      /// <summary>
      /// Gets/sets stack info, e.g. when a test has failed.
      /// </summary>
      public List<StackFrameInfo> StackInfo { get; set; }
   }

   /// <summary>
   /// Delegate for events fired by test fixtures.
   /// </summary>
   /// <param name="sender">The sender of the message.</param>
   /// <param name="args">Arguments for the message.</param>
   public delegate void TestEventHandler(object sender, TestResultEventArgs args);
}
