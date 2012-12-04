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

using System;
using csUnit;
using System.Runtime.Remoting;

namespace csUnit.Core.Tests {
   /// <summary>
   /// Tests to ensure dynamic loading of assemblies from within tests is possible.
   /// </summary>
   [TestFixture]
   public class DynamicallyLoadAssemblyTests {
      /// <summary>
      /// Class to be loaded dynamically
      /// </summary>
      public class DynamicallyLoadedMock {
         public bool ReturnTrue() {
            return true;
         }
      }

      /// <summary>
      /// Check that a partially referenced assembly can be loaded dynamically.
      /// This is useful for being able to swap components used in a system
      /// (e.g. using a Mock component instead of a real one)
      /// </summary>
      [Test]
      public void LoadPartiallyReferencedAssembly() {
         ObjectHandle Handle = Activator.CreateInstanceFrom("csUnit.Core.Tests.dll", "csUnit.Core.Tests.DynamicallyLoadAssemblyTests+DynamicallyLoadedMock");
         DynamicallyLoadedMock MockObject = (DynamicallyLoadedMock)Handle.Unwrap();
         Assert.True(MockObject.ReturnTrue(), "Mock object in partially referenced assembly was not loaded");
      }
   }
}
