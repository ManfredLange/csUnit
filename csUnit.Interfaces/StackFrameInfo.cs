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

namespace csUnit.Interfaces {
   [Serializable]
   public class StackFrameInfo {
      public StackFrameInfo(StackFrame frame) {
         _filePathName = frame.GetFileName() ?? "Unknown file";
         _lineNumber = frame.GetFileLineNumber();
         var methodInfo = frame.GetMethod();
         _methodFullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
      }

      private string MethodFullName {
         get {
            return _methodFullName;
         }
      }

      internal string FilePathName {
         get {
            return _filePathName;
         }
      }

      internal int LineNumber {
         get {
            return _lineNumber;
         }
      }

      public override string ToString() {
         return string.Format("{0} ({1}, line {2})", MethodFullName, FilePathName, LineNumber);
      }

      private readonly string _filePathName = string.Empty;
      private readonly int _lineNumber = -1;
      private readonly string _methodFullName = string.Empty;
   }
}
