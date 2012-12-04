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

namespace csUnit.Core.Tests {
   [TestFixture]
   public class AssemblyFactoryTests {
      [Test]
      public void DefaultCreatesAssemblyFacadeInstance() {
         AssemblyFactory.Type = AssemblyFactory.Default;
         var assembly = AssemblyFactory.CreateInstance(GetType().Assembly.GetName(false));
         Assert.Equals(typeof(AssemblyFacade), assembly.GetType());
      }

      private class MySpecialAssembly : IAssembly {
         #region IAssembly Members

         public void Load(AssemblyName assemblyName) {
            // Intentionally empty for testing. [ml]
         }

         public string FullName {
            get {
               throw new NotImplementedException("The method or operation is not implemented.");
            }
         }

         public string CodeBase {
            get { throw new NotImplementedException(); }
         }

         public Type[] GetExportedTypes() {
            throw new NotImplementedException("The method or operation is not implemented.");
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         #endregion
      }

      [Test]
      public void CanCreateCustomAssembly() {
         AssemblyFactory.Type = typeof(MySpecialAssembly);
         var assembly = AssemblyFactory.CreateInstance(new AssemblyName("bla"));
         Assert.Equals(typeof(MySpecialAssembly), assembly.GetType());
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void AssemblyFactoryThrowsIfNonAssembly() {
         AssemblyFactory.Type = typeof(string);
         AssemblyFactory.CreateInstance(new AssemblyName("bla"));
      }

      [TearDown]
      public void CleanUp() {
         AssemblyFactory.Type = AssemblyFactory.Default;
      }
   }
}
