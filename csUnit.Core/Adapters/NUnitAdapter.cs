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
using System.Reflection;

using csUnit.Core.Visitors;

namespace csUnit.Core.Adapters {
   internal class NUnitAdapter : FrameworkAdapter {
      public static FrameworkAdapter Try(AssemblyLoader assemblyLoader) {
         foreach (var assemblyName in assemblyLoader.GetReferencedAssemblies()) {
            if (assemblyName.FullName.ToLower().Contains("nunit.framework,")) {
               return new NUnitAdapter();
            }
         }
         return null;
      }

      internal static NUnitAdapter Create(string assemblyName) {
         foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if (assembly.GetName().FullName.Contains(assemblyName)) {
               foreach (var reference in assembly.GetReferencedAssemblies()) {
                  if (reference.FullName.ToLower().Contains("nunit.framework,")) {
                     return new NUnitAdapter();
                  }
               }
            }
         }
         return null;
      }

      private NUnitAdapter() {
      }

      public static Type assertType {
         get { return _assertType; }
      }

      public override int AssertionCount {
         get {
            var counter = 0;
            if(_counterFieldInfo != null) {
               counter = (int) _counterFieldInfo.GetValue(null);
            }
            return counter;
         }
      }

      public override MethodEntry CreateMethodEntryFrom(TestFixture fixture, MethodInfo mi) {
         if (HasValidSignature(mi)) {
            var attrs = mi.GetCustomAttributes(true);
            foreach (Attribute attr in attrs) {
               if (attr.GetType().FullName.Equals("NUnit.Framework.TestAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TestMethod);
               }
               if (attr.GetType().FullName.Equals("NUnit.Framework.SetUpAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.SetUpMethod);
               }
               if (attr.GetType().FullName.Equals("NUnit.Framework.TearDownAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TearDownMethod);
               }
               if (attr.GetType().FullName.Equals("NUnit.Framework.TestFixtureTearDownAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureTearDownMethod);
               }
               if (attr.GetType().FullName.Equals("NUnit.Framework.TestFixtureSetUpAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureSetUpMethod);
               }
            }
         }
         return null;
      }

      private static bool HasValidSignature(MethodInfo mi) {
         return mi.ReturnType == typeof(void)
                && mi.GetParameters().Length == 0;
      }

      public override void InvokeMethod(object fixture, MethodEntry testMethod) {
         testMethod.Invoke(fixture);
      }

      public override bool IsTestFixture(Type t) {
         var bIsTest = false;
         var attribs = t.GetCustomAttributes(true);
         for (var i = 0; i < attribs.Length && !bIsTest; i++) {
            if (attribs[i].ToString().Equals("NUnit.Framework.TestFixtureAttribute")) {
               bIsTest = true;
            }
         }
         return bIsTest;
      }

      public override void ResetAssertionCounter() {
         if(_counterFieldInfo != null) {
            _counterFieldInfo.SetValue(null, 0);
         }
      }


      public override bool IsFailure(Exception e) {
         return (e.GetType().FullName == "NUnit.Framework.AssertionException") ;
      }

      public override CsUnitAttribute[] FindAttributesOn(ICustomAttributeProvider codeElement) {
         var attrs = new List<CsUnitAttribute>();
         foreach(Attribute attr in codeElement.GetCustomAttributes(true)) {
            if(attr.GetType().FullName == "NUnit.Framework.IgnoreAttribute") {
               attrs.Add(new IgnoreAttribute("NUnit knows why."));
            }
            else if(attr.GetType().FullName == "NUnit.Framework.ExpectedExceptionAttribute") {
               attrs.Add(CreateExpectedExceptionAttribute(attr));
            }
         }
         return attrs.ToArray();
      }

      private static ExpectedExceptionAttribute CreateExpectedExceptionAttribute(Attribute attr) {
         foreach(var propertyInfo in attr.GetType().GetProperties(FlagsAll)) {
            if( propertyInfo.Name == "ExceptionType") {
               var value = propertyInfo.GetValue(attr, null);
               return new ExpectedExceptionAttribute(
                  (Type) value);
            }
         }
         return new ExpectedExceptionAttribute(typeof(Exception));
      }

      private static Type _assertType;
      private static FieldInfo _counterFieldInfo;
      private const BindingFlags FlagsAll = BindingFlags.Instance | BindingFlags.Static
                                           | BindingFlags.Public | BindingFlags.NonPublic;
   }
}
