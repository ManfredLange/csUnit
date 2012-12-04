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
   internal class MsUnitTestAdapter : FrameworkAdapter {
      public static FrameworkAdapter Try(AssemblyLoader assemblyLoader) {
         foreach (var assemblyName in assemblyLoader.GetReferencedAssemblies()) {
            if (assemblyName.FullName.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework,")) {
               return new MsUnitTestAdapter();
            }
         }
         return null;
      }

      private MsUnitTestAdapter() {
      }

      public static Type assertType {
         get { return _assertType; }
      }

      internal static MsUnitTestAdapter Create(string assemblyName) {
         foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if( assembly.GetName().FullName.Contains(assemblyName) ) {
               foreach(var reference in assembly.GetReferencedAssemblies()) {
                  if( reference.FullName.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework,")) {
                     return new MsUnitTestAdapter();
                  }
               }
            }
         }
         return null;
      }

      public override MethodEntry CreateMethodEntryFrom(TestFixture fixture, MethodInfo mi) {
         if (HasValidSignature(mi)) {
            var attrs = mi.GetCustomAttributes(true);
            foreach (Attribute attr in attrs) {
               if (attr.GetType().FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TestMethod);
               }
               if (attr.GetType().FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.SetUpMethod);
               }
               if (attr.GetType().FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.TearDownMethod);
               }
               if (attr.GetType().FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureSetUpMethod);
               }
               if (attr.GetType().FullName.Equals("Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute")) {
                  return new MethodEntry(new TestMethod(fixture, mi), MethodType.FixtureTearDownMethod);
               }
            }
         }
         return null;
      }

      private static bool HasValidSignature(MethodInfo mi) {
         if (mi.ReturnType == typeof(void)
                && mi.GetParameters().Length == 0) {
            return true;
         }
         if (mi.ReturnType == typeof(void)
            && mi.GetParameters()[0].ParameterType.FullName == "Microsoft.VisualStudio.TestTools.UnitTesting.TestContext") {
            return true;
         }
         return false;
      }

      public override void InvokeMethod(object fixture, MethodEntry testMethod) {
         if (testMethod.MethodType == MethodType.FixtureSetUpMethod) {
            // TODO: Need to get test context here from instance then pass it to setup method. [18mar09, ml]
            // "Using the TestContext class": http://msdn.microsoft.com/en-us/library/ms404699.aspx
            testMethod.Invoke(fixture, new object[]{null});
         }
         else {
            testMethod.Invoke(fixture);
         }
      }

      public override bool IsTestFixture(Type t) {
         return _IsTestFixture(t);
      }

      private static bool _IsTestFixture(ICustomAttributeProvider t) {
         var bIsTest = false;
         var attribs = t.GetCustomAttributes(true);
         for (var i = 0; i < attribs.Length && !bIsTest; i++) {
            if (attribs[i].ToString().Equals("Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute")) {
               bIsTest = true;
            }
         }
         return bIsTest;
      }

      public override void ResetAssertionCounter() {
      }

      public override int AssertionCount {
         get {
            return _assertionCount;
         }
      }

      public override bool IsFailure(Exception e) {
         if (e.GetType().FullName == "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException") {
            return true;
         }
         return false;
      }

      public override CsUnitAttribute[] FindAttributesOn(ICustomAttributeProvider codeElement) {
         var csUnitAttributes = new List<CsUnitAttribute>();
         foreach(var attribute in codeElement.GetCustomAttributes(true)) {
            if(attribute.GetType().FullName == "Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedExceptionAttribute") {
               Type exceptionType = null;
               string exceptionMessage = "";
               var propertyInfo = attribute.GetType().GetProperty("ExceptionType");
               if (propertyInfo != null) {
                  exceptionType = propertyInfo.GetValue(attribute, null) as Type;
               }
               propertyInfo = attribute.GetType().GetProperty("Message");
               if( propertyInfo != null ) {
                  exceptionMessage = propertyInfo.GetValue(attribute, null) as string;
               }
               if (exceptionType != null) {
                  csUnitAttributes.Add(
                     new ExpectedExceptionAttribute(exceptionType,
                                                    new object[]
                                                       {exceptionMessage}));
               }
            }
         }
         return csUnitAttributes.ToArray();
      }
   
      private readonly int _assertionCount;
      private static Type _assertType;
   }
}
