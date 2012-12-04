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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Summary description for Util.
   /// </summary>
   public abstract class Util {
      private static readonly char[] PathSeparator = new[] {'\\'};
      
      /// <summary>
      /// Returns the absolute path and name of a file given a base directory and its
      /// path relative to that base directory.
      /// </summary>
      /// <param name="baseDirName">The name of the base directory.</param>
      /// <param name="relativeFileName">Relative path and name of a file.</param>
      /// <returns>An absolute path and file name.</returns>
      internal static string GetAbsoluteFilename(string baseDirName, string relativeFileName) {
         var baseDirParts = baseDirName.Split(PathSeparator);
         var fileNameParts = relativeFileName.Split(PathSeparator);
         var upCount = 0;

         if( Path.IsPathRooted(relativeFileName) ) {
            return relativeFileName;
         }

         foreach(var part in fileNameParts) {
            if( part.Equals("..") ) {
               upCount++;
            }
            else {
               break;
            }
         }

         var absolutePath = string.Empty;
         for(var i = 0; i < baseDirParts.Length - upCount; i++) {
            absolutePath += baseDirParts[i] + @"\";
         }
         foreach(var part in fileNameParts) {
            if( !part.Equals("..") ) {
               absolutePath += part + @"\";
            }
         }
         if( absolutePath.EndsWith(@"\") ) {
            absolutePath = absolutePath.Substring(0, absolutePath.Length - 1);
         }

         return absolutePath;
      }

      /// <summary>
      /// The method returns the path of a file relative to a directory.
      /// </summary>
      /// <param name="baseDirName">The base directory.</param>
      /// <param name="fileName">Absolute path and name of a file.</param>
      /// <returns>Path and name of the file relative to the base directory.</returns>
      internal static string GetRelativeFilename(string baseDirName, string fileName) {
         if( !Path.IsPathRooted(baseDirName)
            || !Path.IsPathRooted(fileName) ) {
            throw new ArgumentException("Arguments must be rooted.");
         }
         var baseDirInfo = new DirectoryInfo(baseDirName);
         var absoluteFileInfo = new FileInfo(fileName);
         var relativeName = string.Empty;

         if( (baseDirInfo.FullName.ToUpper())[0] != (absoluteFileInfo.FullName.ToUpper())[0] ) {
            // only absolute filename will work, when located on different drives.
            relativeName = absoluteFileInfo.FullName;
         }
         else if( absoluteFileInfo.FullName.ToUpper().StartsWith(baseDirInfo.FullName.ToUpper()) ) {
            relativeName = absoluteFileInfo.FullName.Substring(baseDirInfo.FullName.Length);
            if( relativeName.StartsWith("\\") ) {
               relativeName = relativeName.Substring(1);
            }
         }
         else {
            var baseDirParts = baseDirInfo.FullName.Split(PathSeparator);
            var dirFullName = absoluteFileInfo.Directory.FullName;
            if( dirFullName.EndsWith("\\") ) {
               dirFullName = dirFullName.Substring(0, dirFullName.Length - 1);
            }
            var absoluteFileParts = dirFullName.Split(PathSeparator);

            var matches = 0;
            for(var i = 0; /* intentionally left empty */ ; i++) {
               if(   i >= baseDirParts.Length
                  || i >= absoluteFileParts.Length
                  || baseDirParts[i].ToUpper() != absoluteFileParts[i].ToUpper() ) {
                  break;
               }
               matches++;
            }
            for(var i = matches; i < baseDirParts.Length; i++ ) {
               relativeName += @"..\";
            }
            for(var i = matches; i < absoluteFileParts.Length; i++ ) {
               relativeName += absoluteFileParts[i] + @"\";
            }
            relativeName += absoluteFileInfo.Name;
         }

         return relativeName;
      }

      /// <summary>
      /// Verifies, whether a given file exists.
      /// </summary>
      /// <param name="filePathName">Path and name of file to verify.</param>
      /// <returns>'True', if file exists, else 'False'.</returns>
      public static bool FileExists(String filePathName) {
         FileInfo fi;
         try {
            fi = new FileInfo(filePathName);
         }
         catch(Exception e) {
            Debug.WriteLine(e.Message);
            return false;
         }
         return fi.Exists;
      }

      /// <summary>
      /// Creates a string, fully qualifying a method. It does not contain the
      /// full name of an assembly. Used for test spec.
      /// </summary>
      /// <param name="mi">A method info.</param>
      /// <returns>A fully qualifying name for a method.</returns>
      internal static string MakeVersionAgnosticMethodName(MethodInfo mi) {
         var parts = mi.DeclaringType.Assembly.FullName.Split(",".ToCharArray());
         return MakeCanonicalName(parts[0], mi.DeclaringType.FullName, mi.Name);
      }

      /// <summary>
      /// Creates a string, fully qualifying a method. It does not contain the
      /// full name of an assembly. Used for test spec.
      /// </summary>
      /// <param name="assemblyNameOrFullName">Name of assembly.</param>
      /// <param name="fixtureFullName">Full name of fixture.</param>
      /// <param name="methodName">A test method.</param>
      /// <returns>A fully qualifying name for a method.</returns>
      private static string MakeCanonicalName(string assemblyNameOrFullName, string fixtureFullName, string methodName) {
         var assemblyName = assemblyNameOrFullName.Split(',')[0];
         return string.Format("{0}#{1}#{2}#", assemblyName, fixtureFullName, methodName);
      }

      /// <summary>
      /// Takes an exception and extracts relevant stack information for
      /// reporting a test result.
      /// </summary>
      /// <param name="e">An exception that was thrown.</param>
      /// <param name="trea">The TestResultEventArgs.</param>
      internal static void ExtractLocation(Exception e, TestResultEventArgs trea) {
         if ((e as TargetInvocationException) != null
            && e.InnerException != null) {
            ExtractLocation(e.InnerException, trea);
         }
         else {
            trea.StackInfo = new List<StackFrameInfo>();
            if (e.StackTrace != null) {
// ReSharper disable PossibleNullReferenceException
               foreach (var sf in (new StackTrace(e, true)).GetFrames()) {
// ReSharper restore PossibleNullReferenceException
                  // Search for first non-csUnit location [26mar07, ml]
                  var fullName = sf.GetMethod().DeclaringType.FullName;
                  if (fullName.StartsWith("csUnit")
                     && !fullName.Contains(".Tests.")) {
                     continue;
                  }
                  trea.StackInfo.Add(new StackFrameInfo(sf));
               }
            }
         }
      }

      /// <summary>
      /// Extracts the file name and line number from the given exception's stack trace.  If there
      /// is an inner exception, then that exception's stack trace is explored.
      /// </summary>
      /// <param name="ex">The exception to analyze</param>
      /// <param name="fileName">The returned file name where the exception occured</param>
      /// <param name="line">The line number in the returned file there the exception occured</param>
      //internal static void TryExtractFileLocation(Exception ex, ref string fileName, ref int line) {
      //   try {
      //      fileName = null;
      //      line = -1;
      //      if(ex.InnerException != null) {
      //         TryExtractFileLocation(ex.InnerException, ref fileName, ref line);
      //      }
      //      else {
      //         string trace = ex.StackTrace;
      //         string methodName = ex.TargetSite.Name;
      //         string className = ex.TargetSite.ReflectedType.FullName;
      //         methodName = string.Format("at {0}.{1}(", className, methodName);
      //         int idx1 = trace.IndexOf(methodName);
      //         if(idx1 > -1) {
      //            int idx2 = trace.IndexOf(" in ", idx1 + methodName.Length);
      //            if(idx2 > -1) {
      //               var sb = new System.Text.StringBuilder();
      //               idx1 = idx2 + 4;
      //               idx2 = trace.IndexOf(':', idx2);
      //               if(idx2 > -1) {
      //                  fileName = trace.Substring(idx1, idx2 - idx1);
      //                  for(int i = idx2 + 1; i < trace.Length; i++) {
      //                     char cc = trace[i];
      //                     if(char.IsNumber(cc)) {
      //                        sb.Append(cc);
      //                     }
      //                     else {
      //                        break;
      //                     }
      //                  }
      //                  if(sb.Length > 0) {
      //                     line = int.Parse(sb.ToString());
      //                  }
      //               }
      //               else {
      //                  // No line number
      //                  for(int i = idx1; i < trace.Length; i++) {
      //                     char cc = trace[i];
      //                     if(char.IsWhiteSpace(cc)) {
      //                        break;
      //                     }
      //                     sb.Append(cc);
      //                  }
      //                  fileName = sb.ToString();
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch {
      //      // Ignore
      //   }
      //}
   }
}
