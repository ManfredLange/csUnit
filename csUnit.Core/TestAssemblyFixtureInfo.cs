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

namespace csUnit.Core {
   /// <summary>
   /// Summary description for TestAssemblyFixtureInfo.
   /// </summary>
   internal abstract class TestAssemblyFixtureInfo {
      #region Constructing
      /// <summary>
      /// Scans the assembly for a class with the TestAssemblyFixtureAttribute
      /// set. Takes the first one it finds. If the assembly doesn't contain
      /// such a class a default will be created (null object).
      /// </summary>
      /// <param name="assembly">Assembly to scan.</param>
      /// <returns>TestAssemblyFixtureInfo object.</returns>
      internal static TestAssemblyFixtureInfo Scan(IAssembly assembly) {
         foreach(Type t in assembly.GetExportedTypes()) {
            if(   ! t.IsAbstract
               && IsTestAssemblyFixture(t) ) {
               return new CustomAssemblyFixture(t);
            }
         }
         return new NullFixture();
      }
      #endregion

      #region Public Methods
      /// <summary>
      /// Invokes the setup method on the TestAssemblyFixture.
      /// </summary>
      public abstract void SetUp();

      /// <summary>
      /// Invokes the teardown method of the TestAssemblyFixture.
      /// </summary>
      public abstract void TearDown();
      #endregion

      #region Private Methods
      private static bool IsTestAssemblyFixture(ICustomAttributeProvider type) {
         if( type.GetCustomAttributes(typeof(TestAssemblyFixtureAttribute), true).Length > 0 ) {
            return true;
         }
         return false;
      }
      #endregion

      /// <summary>
      /// This class will be used if the test assembly provides a class with the
      /// TestAssemblyFixtureAttribute set.
      /// </summary>
      private class CustomAssemblyFixture : TestAssemblyFixtureInfo {
         #region Constructing
         public CustomAssemblyFixture(Type testAssemblyFixture) {
            _assemblyFixture = CreateAssemblyFixtureInstance(testAssemblyFixture);

            if( _assemblyFixture != null ) {
               foreach(MethodInfo mi in testAssemblyFixture.GetMethods()) {
                  if( mi.GetCustomAttributes(typeof(SetUpAttribute), true).Length > 0) {
                     _setUpMethod = mi;
                  }
                  else if( mi.GetCustomAttributes(typeof(TearDownAttribute), true).Length > 0 ) {
                     _tearDownMethod = mi;
                  }
               }
            }
         }
         #endregion

         #region Public Methods
         public override void SetUp() {
            if( null != _setUpMethod ) {
               try {
                  _setUpMethod.Invoke(_assemblyFixture, new object[0]);
               }
               catch(Exception ex) {
                  Console.WriteLine("Error: Exception thrown in SetUp method of TestAssemblyFixture:");
                  Console.WriteLine("       Tests depending on it might fail. Error details follow.");
                  Console.WriteLine(ex.InnerException.Message);
               }
            }
         }

         public override void TearDown() {
            if( null != _tearDownMethod  ) {
               try {
                  _tearDownMethod.Invoke(_assemblyFixture, new object[0]);
               }
               catch(Exception ex) {
                  Console.WriteLine("Error: Exception thrown in TearDown method of TestAssemblyFixture:");
                  Console.WriteLine("       Assembly might not properly tear down used resources. Error details follow.");
                  Console.WriteLine(ex.InnerException.Message);
               }
            }
         }
         #endregion

         #region Private Methods
         private static object CreateAssemblyFixtureInstance(Type type) {
            var ci = type.GetConstructor(Type.EmptyTypes);
            if( ci != null ) {
               return ci.Invoke(new object[0]);
            }
            Console.WriteLine("Warning: no public parameterless constructor found for class:");
            Console.WriteLine("         " + type.FullName);
            Console.WriteLine("         TestAssemblyFixture will be ignored");
            return null;
         }
         #endregion
      
         #region Fields
         private readonly object _assemblyFixture;
         private readonly MethodInfo _setUpMethod;
         private readonly MethodInfo _tearDownMethod;
         #endregion
      }

      
      /// <summary>
      /// This class will be used if assembly does not supply its own assembly
      /// fixture. Doing this will avoid tests for null in the test execution
      /// engine at the price of two invocations of methods with an empty body.
      /// [05Mar2006, ml]
      /// </summary>
      private class NullFixture : TestAssemblyFixtureInfo {
         #region Public Methods
         override public void SetUp() {
            // Intentionally left empty. [05Mar2006, ml]
         }

         override public void TearDown() {
            // Intentionally left empty. [05Mar2006, ml]
         }
         #endregion
      }
   }
}
