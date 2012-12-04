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
using System.Diagnostics;
using System.Reflection;

namespace csUnit.Core.Visitors { 
   // Don't use csUnit.Core.Loader as a namespace as there is already a type 
   // with the name csUnit.Core.Loader. [06sep09, ml]

   public class AssemblyLoader : MarshalByRefObject, IDisposable {
      public static AssemblyLoader CreateAssemblyLoader(AppDomain appDomain) {
         try {
            var assemblyLoader = appDomain.CreateInstanceFromAndUnwrap(
               typeof(AssemblyLoader).Assembly.CodeBase,
               typeof(AssemblyLoader).FullName);
            return assemblyLoader as AssemblyLoader;
         }
         catch (Exception ex) {
            Debug.WriteLine(ex);
            return null;
         }
      }

      static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
         return null;
      }
      
      public AssemblyLoader() {
         AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
      }

      public void Load(AssemblyName assemblyName) {
         _assembly = Assembly.Load(assemblyName);
      }

      public Type[] ExportedTypes {
         get {
            return _assembly.GetExportedTypes();
         }
      }

      public AssemblyName[] GetReferencedAssemblies() {
         return _assembly.GetReferencedAssemblies();
      }

      #region Implementation of IDisposable

      public void Dispose() {
         _assembly = null;
         GC.Collect();
         GC.WaitForPendingFinalizers();
         GC.Collect(0); 
      }

      #endregion

      private Assembly _assembly;
   }
}
