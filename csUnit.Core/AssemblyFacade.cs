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
using System.Reflection;

namespace csUnit.Core {
   /// <summary>
   /// This class serves as a facade to System.Reflection.Assembly. It simply
   /// forwards all calls to System.Reflection.Assembly.
   /// </summary>
   internal class AssemblyFacade : IAssembly {
      public void Load(AssemblyName assemblyName) {
         _assembly = Assembly.Load(assemblyName);
      }

      /// <summary>
      /// Gets the exported types defined in the assembly that are visible
      /// outside the assembly.
      /// </summary>
      /// <returns>An array of <b>Type</b> objects that represent the types
      /// defined in the assembly that are visible outside the assembly.
      /// </returns>
      public Type[] GetExportedTypes() {
         return _assembly.GetExportedTypes();
      }

      /// <summary>
      /// Gets the code base of the assembly as a URL.
      /// </summary>
      public string CodeBase {
         get {
            return _assembly.CodeBase;
         }
      }

      /// <summary>
      /// Gets the display name of the assembly.
      /// </summary>
      public string FullName {
         get {
            return _assembly.FullName;
         }
      }

      /// <summary>
      /// Gets the time when the assembly was last written to.
      /// </summary>
      public DateTime ModifiedTimeStamp {
         get {
            var uri = new Uri(_assembly.GetName(false).CodeBase);
            var fi = new FileInfo(uri.AbsolutePath);
            return fi.LastWriteTime;
         }
      }

      private Assembly _assembly;
   }
}
