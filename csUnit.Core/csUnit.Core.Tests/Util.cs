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
using System.IO;

namespace csUnit.Core.Tests {
   /// <summary>
   /// Some helper methods for this project.
   /// </summary>
   internal abstract class Util {
      /// <summary>
      /// Returns the path to the where the solution file is located.
      /// </summary>
      /// <returns></returns>
      public static string SolutionCodeBase {
         get {
            Uri      url   = new Uri(typeof(Util).Assembly.CodeBase);
            FileInfo fi    = new FileInfo(url.AbsolutePath);
            string path = Path.Combine(fi.DirectoryName, "../../");
            return NormalizePath(path);
         }
      }

      /// <summary>
      /// Removes useless ../ parts in the path
      /// </summary>
      /// <param name="source">A path created using Path.Combine()</param>
      /// <returns>A path without ../ elements</returns>
      public static string NormalizePath(string source) {
         FileInfo temp = new FileInfo(source);
         return Path.Combine(temp.DirectoryName, temp.Name);
      }
   }
}
