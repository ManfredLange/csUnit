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

#if DEBUG

using System;

using csUnit.Core.Criteria;

namespace csUnit.Core.Tests {
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local

   /// <summary>
   /// Summary description for FixtureTearDownTests.
   /// </summary>
   [TestFixture]
   public class FixtureTearDownTests {
      [TestFixture]
      private class ClassWithFixtureTearDown {
         [Test]
         public void Test1() {
            _methodSequence += "Test1";
         }
         [Test]
         public void Test2() {
            _methodSequence += "Test2";
         }
         [FixtureTearDown]
         public void FixtureTearDown() {
            _methodSequence += "FixtureTearDown";
         }
      }

      [SetUp]
      public void SetUp() {
         _methodSequence = string.Empty;
         _tc = new TestFixture(typeof(ClassWithFixtureTearDown));
      }

      [Test]
      public void FixtureTearDownCalledOnlyOnce() {
         _tc.Execute(new TestRun(new AllTestsCriterion()), new NullListener());
         Assert.Equals("Test1Test2FixtureTearDown", _methodSequence);
      }

      [TestFixture]
      private class ExceptionInFixtureTearDown {
         [Test]
         public void Test1() {
         }

         [FixtureTearDown]
         public void CleanUpFixture() {
            throw new Exception("just for testing");
         }
      }

      [Test]
      public void FixtureTearDownThrowsException() {
         var tc = new TestFixture(typeof(ExceptionInFixtureTearDown));
         tc.Execute(new TestSpec(), new NullListener());
      }
      
      private TestFixture   _tc;
      private static string _methodSequence = string.Empty;
   }
   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}

#endif
