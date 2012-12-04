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

using System.Collections.Generic;
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   // How to use this class:
   // When you invoke RunTests() on some of the classes, one of the required
   // parameters is a ITestListener implementation. For testing purposes you
   // typically want to know which test methods were executed and which were 
   // not.
   // A test can now be written as follows:
   //    ...
   //    <obj>.RunTests(..., new SimpleTestListener());
   //    ...
   //    Assert....(..., SimpleTestListener.Messages);
   //
   // [20Feb07, ml]
   internal class SimpleTestListener : ITestListener {
      public static string Messages {
         get {
            return _messages;
         }
         set {
            _messages = value;
         }
      }

      public static string[] IgnoredItems {
         get {
            return _ignoredItems.ToArray();
         }
      }

      public static List<string> ErrorItems {
         get {
            return _errorItems;
         }
      }

      public static List<string> FailureItems {
         get {
            return _failureItems;
         }
      }

      public static List<string> PassedItems {
         get {
            return _passedItems;
         }
      }

      public SimpleTestListener() {
         _messages = string.Empty;
         _ignoredItems.Clear();
         _passedItems.Clear();
         _errorItems.Clear();
         _failureItems.Clear();
      }

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
         _messages += " TS:" + args.MethodName;
      }

      public void OnTestPassed(object sender, TestResultEventArgs args) {
         _messages += " TP:" + args.MethodName;
         _passedItems.Add(string.Format("{0}#{1}#{2}#",
            args.AssemblyName,
            args.ClassName,
            args.MethodName));
      }

      public void OnTestError(object sender, TestResultEventArgs args) {
         _messages += " TE:" + args.MethodName + " " + args.Reason;
         _errorItems.Add(string.Format("{0}#{1}#{2}#",
            args.AssemblyName,
            args.ClassName,
            args.MethodName));
      }

      public void OnTestFailed(object sender, TestResultEventArgs args) {
         _messages += " TF:" + args.MethodName + " " + args.Reason;
         _failureItems.Add(string.Format("{0}#{1}#{2}#",
            args.AssemblyName,
            args.ClassName,
            args.MethodName));
      }

      public void OnTestSkipped(object sender, TestResultEventArgs args) {
         _ignoredItems.Add(string.Format("{0}#{1}#{2}#{3}#",
            args.AssemblyName,
            args.ClassName,
            args.MethodName,
            args.Reason));
      }

      #endregion

      private static string _messages = string.Empty;
      private static readonly List<string> _ignoredItems = new List<string>();
      private static readonly List<string> _passedItems = new List<string>();
      private static readonly List<string> _failureItems = new List<string>();
      private static readonly List<string> _errorItems = new List<string>();
   }
}
