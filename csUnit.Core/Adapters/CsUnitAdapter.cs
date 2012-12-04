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
using csUnit.Core.Data;
using csUnit.Core.Visitors;

namespace csUnit.Core.Adapters {
   internal class CsUnitAdapter : FrameworkAdapter {
      public static FrameworkAdapter Try(AssemblyLoader assemblyLoader) {
         foreach(var assemblyName in assemblyLoader.GetReferencedAssemblies()) {
            if( assemblyName.FullName.Contains("csUnit,")) {
               return new CsUnitAdapter();
            }
         }
         return null;
      }

      public override int AssertionCount {
         get {
            return Assert.Count;
         }
      }

      public override MethodEntry CreateMethodEntryFrom(TestFixture fixture, MethodInfo mi) {
         if (HasValidSignature(mi)) {
            var attrs = mi.GetCustomAttributes(true);

            // First try locating framework attributes
            foreach(Attribute attr in attrs) {
               if (attr.GetType().FullName.Equals(typeof(TestAttribute).FullName)) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TestMethod);
               }
               if (attr.GetType().FullName.Equals(typeof(SetUpAttribute).FullName)) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.SetUpMethod);
               }
               if (attr.GetType().FullName.Equals(typeof(TearDownAttribute).FullName)) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TearDownMethod);
               }
               if (attr.GetType().FullName.Equals(typeof(FixtureTearDownAttribute).FullName)) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureTearDownMethod);
               }
               if (attr.GetType().FullName.Equals(typeof(FixtureSetUpAttribute).FullName)) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureSetUpMethod);
               }
            }

            // Next we try whether the method is based on naming convention
            if (mi.Name.ToUpper().StartsWith("TEST")) {
               return new MethodEntry(new TestMethod(fixture, mi), MethodType.TearDownMethod);
            }
            if (mi.Name.ToUpper().Equals("SETUP")) {
               return new MethodEntry(new TestMethod(fixture, mi), MethodType.SetUpMethod);
            }
            if (mi.Name.ToUpper().Equals("TEARDOWN")) {
               return new MethodEntry(new TestMethod(fixture, mi), MethodType.TearDownMethod);
            }
         }
         else if (mi.ReturnType == typeof(void)
            && HasTestAttribute(mi)) { // This line needed to address SF1797256 [24mar2008, ml]
            return new MethodEntry(new ParameterizedTestMethod(fixture, mi), MethodType.TestMethod);
         }
         return null;
      }

      /// <summary>
      /// Tests whether a method has an instance of the TestAttribute attached
      /// to it.
      /// </summary>
      /// <param name="methodInfo">MethodInfo object with description of the 
      /// method to check for presence of the TestAttribute.</param>
      /// <returns>true, if a TestAttribute instance is attached, or false 
      /// otherwise.</returns>
      private static bool HasTestAttribute(ICustomAttributeProvider methodInfo) {
         var attribs = methodInfo.GetCustomAttributes(true);
         for (var i = 0; i < attribs.Length; i++) {
            var strAttrib = attribs[i].ToString();
            if (strAttrib == "csUnit.TestAttribute") {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Checks whether a method has a correct signature.
      /// </summary>
      /// <param name="mi">The method info of the method to verify.</param>
      /// <returns>'true', if the method has an acceptable signature. 'false' 
      /// otherwise.</returns>
      /// <remarks>A correct signature is public void name(), where 'name' can 
      /// be any valid method name.</remarks>
      private static bool HasValidSignature(MethodInfo mi) {
         return mi.ReturnType == typeof(void)
                && mi.GetParameters().Length == 0;
      }

      public override void InvokeMethod(object fixture, MethodEntry testMethod) {
         testMethod.Invoke(fixture);
      }

      public override bool IsTestFixture(Type t) {
         var isTest = t.Name.EndsWith("Test");
         if (!isTest) {
            var attribs = t.GetCustomAttributes(true);
            for (var i = 0; i < attribs.Length && !isTest; i++) {
               if ((attribs[i] is TestFixtureAttribute) ||
                   (attribs[i].ToString().Equals("csUnit.TestFixtureAttribute"))) {
                  isTest = true;
               }
            }
         }
         return isTest;
      }

      public override void ResetAssertionCounter() {
         Assert.Count = 0;
      }

      public override bool IsFailure(Exception e) {
         return (e is TestFailed);
      }

      public override CsUnitAttribute[] FindAttributesOn(ICustomAttributeProvider codeElement) {
         return (CsUnitAttribute[])codeElement.GetCustomAttributes(typeof(CsUnitAttribute), true);
      }
   }
}
