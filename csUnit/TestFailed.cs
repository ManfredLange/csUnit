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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace csUnit {
   /// <summary>
   /// TestFailed is an exception that will be thrown when a test has failed, that is when an
   /// assertion has failed.
   /// </summary>
   [Serializable]
   public class TestFailed : Exception {
      /// <summary>
      /// Constructor capturing expected and actual separately.
      /// </summary>
      /// <param name="expected">The string representing what was expected.</param>
      /// <param name="actual">The string representing the actual value.</param>
// ReSharper disable MemberCanBeInternal
      public TestFailed(string expected, string actual)
// ReSharper restore MemberCanBeInternal
         : this(expected, actual, string.Empty) {
      }

      /// <summary>
      /// Constructor capturing expected and actual separately and also taking
      /// a custom message.
      /// </summary>
      /// <param name="expected">The string representing what was expected.</param>
      /// <param name="actual">The string representing the actual value.</param>
      /// <param name="customMessage">Custom message to be displayed instead of the standard message.</param>
// ReSharper disable MemberCanBeInternal
      public TestFailed(string expected, string actual, string customMessage) {
// ReSharper restore MemberCanBeInternal
         Tip = string.Empty;
         var st = new StackTrace(true);
         StackFrame frame = null;
         var currentModuleName = GetType().Assembly.ManifestModule.Name;
         for(var i = 0; i < st.FrameCount; i++) {
            frame = st.GetFrame(i);
            if(frame.GetFileName() != null
               && frame.GetMethod().DeclaringType.Assembly.ManifestModule.Name != currentModuleName ) {
               break;
            }
         }

         if(   frame != null 
            && frame.GetFileName() != null ) {
            var index = frame.GetFileName().LastIndexOf("\\") + 1;
            var fileName = index >= 0 ? frame.GetFileName().Substring(index) : frame.GetFileName();
            if( customMessage != string.Empty ) {
               _message = string.Format("{0} (file {1}, line {2})",
                                           customMessage,
                                           fileName,
                                           frame.GetFileLineNumber());
            }
            else {
               _message = string.Format("Failure in file {0}, line {1}",
                                           fileName,
                                           frame.GetFileLineNumber());
            }
         }
         else {
            _message = "Failure at unknown source location.";
         }
         Expected = expected;
         Actual = actual;
      }

      /// <summary>
      /// Gets the message that gives further details about the reasons for the 
      /// failed tests.
      /// </summary>
      public override string Message {
         get {
            return _message == string.Empty ? base.Message : _message;
         }
      }

      /// <summary>
      /// Gets string describing the expected outcome.
      /// </summary>
      public string Expected { get; private set; }

      /// <summary>
      /// Gets a string describing the actual outcome.
      /// </summary>
      public string Actual { get; private set; }

      /// <summary>
      /// Gets a string containing a suggestion for avoiding the failure. This
      /// property is an empty string if no such suggestion can be derived from
      /// the available information.
      /// </summary>
      public string Tip { get; internal set; }

      ///<summary>Returns true if a tip can be provided for the failure.
      ///</summary>
      public bool HasTip {
         get {
            return Tip != string.Empty;
         }
      }

      #region ISerializable implementation
      /// <summary>
      /// Constructor required for deserialization.
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Context</param>
      protected TestFailed(SerializationInfo info, StreamingContext context)
         : base(info, context) {
         Actual = info.GetString("_actual");
         Expected = info.GetString("_expected");
         _message = info.GetString("_message");
         Tip = info.GetString("_tip");
      }

      /// <summary>
      /// Method is invoked during serialization. Users of csUnit should not
      /// have the need to use this method.
      /// </summary>
      /// <param name="info">Serialization info</param>
      /// <param name="context">Streaming context</param>
      [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
      public override void GetObjectData(SerializationInfo info, StreamingContext context) {
         base.GetObjectData(info, context);
         info.AddValue("_actual", Actual);
         info.AddValue("_expected", Expected);
         info.AddValue("_message", _message);
         info.AddValue("_tip", Tip);
      }
      #endregion //ISerializable implementation

      /// <summary>
      /// Compares two TestFailed instances.
      /// </summary>
      /// <param name="obj">Object to compare to.</param>
      /// <returns>'true' if equal, 'false' otherwise.</returns>
      public override bool Equals(object obj) {
         var theOtherObject = obj as TestFailed;
         if (theOtherObject != null) {
            if( _message == theOtherObject._message
               && Expected == theOtherObject.Expected
               && Actual == theOtherObject.Actual
               && Tip == theOtherObject.Tip ) {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Hash function for this type.
      /// </summary>
      /// <returns>A hashvalue.</returns>
      public override int GetHashCode() {
         return (_message + Expected + Actual + Tip).GetHashCode();
      }

      private readonly string _message = string.Empty;
   }
}
