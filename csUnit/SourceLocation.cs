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

namespace csUnit {
   /// <summary>
   /// SourceLocation describes a location in a source file by means of file
   /// name and line number. csUnit currently doesn't required the column 
   /// number.
   /// </summary>
   [Serializable]
   public class SourceLocation {
      /// <summary>
      /// Creates an instance of a SourceLocation.
      /// </summary>
      /// <param name="fileName">Name of the source file.</param>
      /// <param name="lineNumber">Line number within the source file.</param>
      public SourceLocation(string fileName, int lineNumber) {
         if( fileName == null ) {
            throw new ArgumentNullException("fileName");
         }
         if( lineNumber < 0 ) {
            throw new ArgumentOutOfRangeException("lineNumber", "Line number must be greater than 0.");
         }
         _fileName = fileName;
         _lineNumber = lineNumber;
      }

      /// <summary>
      /// Compares another object to this SourceLocation.
      /// </summary>
      /// <param name="obj">An object to compare.</param>
      /// <returns>true if equal, false otherwise.</returns>
      public override bool Equals(object obj) {
         if(   obj != null 
            && obj.GetType() == GetType()) {
               var otherLoc = (SourceLocation) obj;
               if(_fileName.Equals(otherLoc.FileName)
                  && _lineNumber.Equals(otherLoc.LineNumber)) {
                  return true;
               }
            }
         return false;
      }

      /// <summary>
      /// Calculates a hash value used for collections.
      /// </summary>
      /// <returns>A hash code for the SourceLocation object.</returns>
      public override int GetHashCode() {
         return (_fileName + _lineNumber).GetHashCode();
      }

      /// <summary>
      /// Gets the file name of the SourceLocation.
      /// </summary>
      public string FileName {
         get {
            return _fileName;
         }
      }

      /// <summary>
      /// Gets the line number of the SourceLocation.
      /// </summary>
      public int LineNumber {
         get {
            return _lineNumber;
         }
      }

      /// <summary>
      /// Returns a string representation of the object.
      /// </summary>
      /// <returns>A string.</returns>
      public override string ToString() {
         return _fileName + ", line " + _lineNumber;
      }

      private readonly string _fileName = string.Empty;
      private readonly int _lineNumber;
   }
}
