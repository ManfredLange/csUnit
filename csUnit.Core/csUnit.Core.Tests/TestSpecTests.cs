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
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local
   
   /// <summary>
   /// Summary description for TestSpecTests.
   /// </summary>
   [TestFixture(Categories="CORE")]
   public class TestSpecTests {
      [TestFixture]
      private class Foo {
         [Test]
         public void Bar() {
         }

         [Test]
         public void Nope() {
         }

         [Test]
         public void Nope2() {
         }

         [Test]
         public void Test1() {
            Sequence += "Test1()";
         }
         
         [Test]
         public void Test1AsPrefix() {
            Sequence += "Test1AsPrefix()";
         }

         public static string Sequence = string.Empty;
      }

      [Test]
      public void NoTestConfiguredWhenEmpty() {
         var testSpec = new TestSpec();
         var testMethod = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Bar"));
         Assert.False(testSpec.Contains(testMethod));
      }

      [Test]
      public void OneTestConfigured() {
         var testSpec = new TestSpec();
         var testMethod1 = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Bar"));
         var testMethod2 = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Nope"));
         testSpec.AddTest(testMethod1);
         Assert.True(testSpec.Contains(testMethod1));
         Assert.False(testSpec.Contains(testMethod2));
      }

      [Test]
      public void AddSameTestTwice() {
         var testSpec = new TestSpec();
         var testMethod = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Bar"));
         testSpec.AddTest(testMethod);
         testSpec.AddTest(testMethod);
         Assert.Equals(1, testSpec.FixtureCount);
      }

      [Test]
      public void EmptyTestSpecHasCountZero() {
         Assert.Equals(0, new TestSpec().FixtureCount);
      }

      [Test]
      public void AssemblyIsConfiguredWithAtLeastOneTest() {
         var testSpec = new TestSpec();
         var testFixture = new TestFixture(typeof(Foo));
         var testMethod = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Bar"));
         testSpec.AddTest(testMethod.AssemblyName, testMethod.DeclaringTypeFullName, testMethod.Name);
         Assert.True(testSpec.Contains(testFixture));
      }

      [Test]
      public void TestNameIsPrefixOfSecondTestName() {
         var testSpec = new TestSpec();
         var testMethod1 = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Test1"));
         var testMethod2 = new TestMethod(new TestFixture(typeof(Foo)), typeof(Foo).GetMethod("Test1AsPrefix"));
         testSpec.AddTest(testMethod1);
         Assert.True(testSpec.Contains(testMethod1));
         Assert.False(testSpec.Contains(testMethod2));
      }

      private class MockTestMethod : ITestMethod {
         public MockTestMethod(string methodName) {
            _methodName = methodName;
         }

         #region ITestMethod Members
         public string AttributeName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public Categories Categories {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public Categories InheritedCategories {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public string AssemblyName {
            get {
               return GetType().Assembly.GetName().Name;
            }
         }

         public string DeclaringTypeFullName {
            get {
               return typeof(MockTestMethod).FullName;
            }
         }

         public string Name {
            get {
               return _methodName;
            }
         }

         public string FullName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public void Invoke(object obj, object[] args) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Execute(ITestListener listener) {
            throw new NotImplementedException();
         }

         public bool Ignore {
            get { throw new NotImplementedException(); }
         }

         public string IgnoreReason {
            get { throw new NotImplementedException(); }
         }

         #endregion

         private readonly string _methodName = string.Empty;
      }

      [Test]
      public void TestNameIsPrefixOfSecondTestNameViaITestMethod() {
         var testSpec = new TestSpec();
         testSpec.AddTest(GetType().Assembly.GetName().Name,
            typeof(MockTestMethod).FullName, "Prefix");
         Assert.True(testSpec.Contains(new MockTestMethod("Prefix")));
         Assert.False(testSpec.Contains(new MockTestMethod("PrefixTestName")));
      }

      [Test]
      public void CanSelectTestFixture() {
         var testSpec = new TestSpec();
         testSpec.AddTest(GetType().Assembly.GetName().Name,
            typeof(MockTestMethod).FullName, string.Empty);
         Assert.True(testSpec.Contains(new MockTestMethod("Foo")));
      }

      [Test]
      public void CanSelectNameSpace() {
         var testSpec = new TestSpec();
         var nameSpaceName = typeof(MockTestMethod).FullName;
         var indexOfFirstDot = nameSpaceName.IndexOf('.');
         nameSpaceName = nameSpaceName.Substring(0, indexOfFirstDot);
         testSpec.AddTest(GetType().Assembly.GetName().Name,
            nameSpaceName, string.Empty);
         Assert.True(testSpec.Contains(new MockTestMethod("Foo")));
      }

      [SetUp]
      public void SetUp() {
         Foo.Sequence = string.Empty;
      }
   }
   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}
