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
using csUnit.Interfaces;

namespace csUnit.Core {
   internal static class LoaderFactory {
      /// <summary>
      /// Creates a loader for the provided assembly.
      /// </summary>
      /// <param name="assemblyPathName">Path and name of the assembly to load</param>
      /// <returns>A test assembly</returns>
      /// <exception cref="ArgumentException">Thrown when 'assemblyPathName' 
      /// refers to a file other than dll or exe.</exception>
      public static ITestAssembly CreateInstance(string assemblyPathName) {
         ValidatePathName(assemblyPathName);
         var url = new Uri(assemblyPathName);
         var ci = _type.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
            CallingConventions.HasThis, new[] { typeof(string) }, null);
         if(null != ci) {
            var loader = ci.Invoke(new object[] { url.LocalPath }) as ITestAssembly;
            return loader;
         }
         throw new Exception("Type '" + _type.FullName +
                             "' doesn't have a constructor of type 'string' or it is " +
                             "private or internal.");
      }

      private static void ValidatePathName(string pathName) {
         if( !File.Exists(pathName) ) {
            throw new FileNotFoundException("Couldn't find file.", pathName);
         }
         var info = new FileInfo(pathName);
         if(info.Extension.ToUpper() != ".DLL"
            && info.Extension.ToUpper() != ".EXE") {
            throw new ArgumentException("File for option /assembly does not refer to an assembly. Must be DLL or EXE.", "pathName");
         }
      }

      public static Type Default {
         get {
            return typeof(Loader);
         }
      }

      public static Type Type {
         set {
            if(null == value.GetInterface(typeof(ITestAssembly).Name)) {
               throw new ArgumentException(
                  string.Format("Type {0} doesn't implement interface {1}", 
                  value.FullName, typeof(ITestAssembly).Name));
            }
            _type = value;
         }
      }

      private static Type _type = Default;
   }
}
