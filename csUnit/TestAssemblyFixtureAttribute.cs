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

namespace csUnit {
   /// <summary>
   /// Use the TestAssemblyFixtureAttribute to decorate a class which contains
   /// setup and teardown methods for an assembly. Apply it to only one class,
   /// as the runtime will pick up only the first one it finds.
   /// The setup method needs to be marked with the <seealso cref="csUnit.SetUpAttribute"/>.
   /// The teardown method requires the <seealso cref="csUnit.TearDownAttribute"/>.
   /// </summary>
   /// <remarks>The setup method will be executed before any test in the 
   /// assembly is executed. The teardown method will be executed after all 
   /// tests in the assembly have been executed, regardless of the outcome of
   /// those tests.
   /// </remarks>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
   public class TestAssemblyFixtureAttribute : Attribute {
      /// <summary>
      /// Constructs a TestAssemblyAttribute instance. No further parameters
      /// are needed.
      /// </summary>
      /// <remarks>While you can mark several classes in a test assembly with
      /// this attribute, the csUnit runtime will use only the first it finds.
      /// The order to classes is not guaranteed. So make sure you have at most
      /// one TestAssemblyFixture per test assembly.</remarks>
// ReSharper disable EmptyConstructor
      public TestAssemblyFixtureAttribute() {}
// ReSharper restore EmptyConstructor
   }
}
