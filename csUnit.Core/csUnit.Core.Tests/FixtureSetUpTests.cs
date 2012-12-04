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

#if DEBUG

using System;

using csUnit.Core.Criteria;

namespace csUnit.Core.Tests {
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local

   /// <summary>
   /// Summary description for FixtureSetUpTests.
   /// </summary>
   [TestFixture]
   public class FixtureSetUpTests {
      [TestFixture]
      private class ClassWithFixtureSetUp {
         [Test]
         public void Test1() {
            _methodSequence += "Test1";
         }
         [Test]
         public void Test2() {
            _methodSequence += "Test2";
         }
         [FixtureSetUp]
         public void FixtureSetUp() {
            _methodSequence += "FixtureSetUp";
         }
      }

      [SetUp]
      public void SetUp() {
         _methodSequence = string.Empty;
         _tc = new TestFixture(typeof(ClassWithFixtureSetUp));
      }

      [Test]
      public void FixtureSetUpCalledOnlyOnce() {
         _tc.Execute(new TestRun(new AllTestsCriterion()), new NullListener());
         Assert.Equals("FixtureSetUpTest1Test2", _methodSequence);
      }

      [TestFixture]
      private class ExceptionInFixtureSetup {
         [Test]
         public void Test1() {
         }

         [FixtureSetUp]
         public void FixtureInit() {
            throw new Exception("just for testing");
         }
      }

      [Test]
      public void FixtureSetupThrowsException() {
         var tc = new TestFixture(typeof(ExceptionInFixtureSetup));
         tc.Execute(new TestSpec(), new NullListener());
      }

      [TestFixture]
      private class ClassWithSetupAndAttribute {
         [FixtureSetUp]
         public void Setup() {
            _methodSequence += "FixtureSetUp";
         }

         [FixtureTearDown]
         public void TearDown() {
            _methodSequence += "FixtureTearDown";
         }

         [Test]
         public void Foo1() {
            _methodSequence += "Foo1";
         }

         [Test]
         public void Foo2() {
            _methodSequence += "Foo2";
         }
      }

      [Test]
      public void TestNameIsIgnoredWhenAttributePresent() {
         _tc = new TestFixture(typeof(ClassWithSetupAndAttribute));
         _methodSequence = string.Empty;
         _tc.Execute(new TestRun(new AllTestsCriterion()), new NullListener());
         Assert.Equals("FixtureSetUpFoo1Foo2FixtureTearDown", _methodSequence);
      }
      
      private TestFixture _tc;
      private static string _methodSequence = string.Empty;
   }

   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}

#endif
