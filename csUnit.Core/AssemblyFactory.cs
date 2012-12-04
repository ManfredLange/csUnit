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
using System.Reflection;

namespace csUnit.Core {
   internal class AssemblyFactory {
      public static IAssembly CreateInstance(AssemblyName assemblyName) {
         var ci = _assemblyType.GetConstructor(
               BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
               CallingConventions.HasThis, new Type[0], null);
         if(ci != null) {
            var assembly = ci.Invoke(Type.EmptyTypes) as IAssembly;
            if(assembly != null) {
               assembly.Load(assemblyName);
            }
            return assembly;
         }
         return null;
      }

      private AssemblyFactory() {
      }

      public static Type Type {
         set {
            if(value.GetInterface(typeof(IAssembly).FullName) == null) {
               throw new ArgumentException("Type " + value.FullName + 
                  " doesn't implement " + typeof(IAssembly).FullName);
            }
            _assemblyType = value;
         }
      }

      public static Type Default {
         get {
            return typeof(AssemblyFacade);
         }
      }

      private static Type _assemblyType = Default;
   }
}
