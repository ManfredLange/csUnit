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
using System.Text;

namespace csUnit.Data {
   /// <summary>
   /// A data row specifies a set of parameters for a parameterized test.
   /// </summary>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
   public class DataRow : Attribute {
      /// <summary>
      /// Creates an instance of DataRow.
      /// </summary>
      /// <param name="values"></param>
      public DataRow(params object[] values) {
         _values = values ?? new object[] { };
      }

      internal DataRow(string expectedExceptionFullName, params object[] values)
         : this(values) {
         ExpectedException = FindExceptionTypeFor(expectedExceptionFullName);
      }

      /// <summary>
      /// Sets/gets the expected exception.
      /// </summary>
      public Type ExpectedException { set; get; }

      /// <summary>
      /// Gets/sets the parameter values that belong to this data row.
      /// </summary>
      public object[] Values {
         get {
            return _values;
         }
         set {
            _values = value;
         }
      }

      /// <summary>
      /// Compares the data row object with a second object.
      /// </summary>
      /// <param name="obj"></param>
      /// <returns>Returns true if both objects are equal, false otherwise.</returns>
      public override bool Equals(object obj) {
         var row = obj as DataRow;
         if(null != row) {
            if(row._values.GetLength(0) == _values.GetLength(0)) {
               for(var i = 0; i < row._values.GetLength(0); i++) {
                  if(!row._values[i].Equals(_values[i])) {
                     return false;
                  }
               }
               return true;
            }
            return false;
         }
         return false;
      }

      /// <summary>
      /// Hash function for this type.
      /// </summary>
      /// <returns>A hash value</returns>
      public override int GetHashCode() {
         return _values.GetHashCode();
      }

      /// <summary>
      /// Creates a string representatino of the data row.
      /// </summary>
      /// <returns>Data row in a string representation.</returns>
      public override string ToString() {
         var s = new StringBuilder();
         for(var i = 0; i < _values.Length; i++) {
            s.Append(_values[i]);
            if(i < _values.Length - 1) {
               s.Append(", ");
            }
         }
         return s.ToString();
      }

      private static Type FindExceptionTypeFor(string expectedExceptionFullName) {
         foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach(var type in assembly.GetExportedTypes()) {
               if(type.FullName.Equals(expectedExceptionFullName)) {
                  return type;
               }
            }
         }
         return null;
      }

      private object[] _values = new object[] { };
   }
}
