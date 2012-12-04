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
using System.Reflection;

using csUnit.Core.Visitors;

namespace csUnit.Core.Adapters {
   internal abstract class FrameworkAdapter : FrameworkAdapterBase {
      public static FrameworkAdapter CreateInstanceFor(AssemblyLoader assemblyLoader) {
         if (_adapter != null) {
            return _adapter;
         }
         if (_adapter == null) {
            _adapter = MsUnitTestAdapter.Try(assemblyLoader);
         }
         if( _adapter == null ) {
            _adapter = CsUnitAdapter.Try(assemblyLoader);
         }
         if( _adapter == null ) {
            _adapter = NUnitAdapter.Try(assemblyLoader);
         }
         return _adapter;
      }

      public static FrameworkAdapter CreateInstance(string assemblyName) {
         if( _adapter != null ) {
            return _adapter;
         }
         // TODO: Replace following by virtual constructor pattern. [01mar09, ml]
         if (_adapter == null) {
            _adapter = MsUnitTestAdapter.Create(assemblyName);
         }
         if( _adapter == null ) {
            _adapter = NUnitAdapter.Create(assemblyName);
         }
         if(_adapter == null) { 
            // All others were not found. Create default. [01mar09, ml]
            _adapter = new CsUnitAdapter();
         }
         
         return _adapter;
      }

      public abstract MethodEntry CreateMethodEntryFrom(TestFixture fixture, MethodInfo mi);
      public abstract CsUnitAttribute[] FindAttributesOn(ICustomAttributeProvider codeElement);
      public abstract void InvokeMethod(object fixture, MethodEntry testMethod);
      public abstract bool IsFailure(Exception e);
      public abstract bool IsTestFixture(Type t);
      public abstract void ResetAssertionCounter();

      private static FrameworkAdapter _adapter;
   }
}
