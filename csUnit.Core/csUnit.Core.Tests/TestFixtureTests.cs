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
using System.Reflection;

using csUnit.Core.Criteria;
using csUnit.Interfaces;
using csUnit.Interfaces.Criteria;

namespace csUnit.Core.Tests {
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local
   
   /// <summary>
   /// Summary description for TestClassTest.
   /// </summary>
   [TestFixture(Categories="CORE")] 
   public class TestFixtureTests {
      [TearDown]
      public void TearDown() {
         AssemblyFactory.Type = AssemblyFactory.Default;
      }

      private class ClassUnderTest {
      }

      private class ClassUnderTestWithSetup {
         [SetUp]
         public void TheSetupMethod() {
            _sequence += " TheSetupMethod";
         }

         [Test]
         public void ATestMethod() {
            _sequence += " ATestMethod";
         }
      }

      private class ClassWithExceptionInSetup {
         [SetUp]
         public void ThrowsException() {
            _sequence += " ThrowsException()";
#line 66 // Needed to report following exception on a specific line
            throw new NullReferenceException();
#line default
         }

         [Test]
         public void AMethod() {
         }
      }

      private class ClassWithExceptionInTearDown {
         [TearDown]
         public void ThrowsException() {
            _sequence += " ThrowsException()";
#line 81 // Needed to report following exception on a specific line
            throw new NullReferenceException();
#line default
         }

         [Test]
         public void AMethod() {
            _sequence += " AMethod";
         }
      }

      [SetUp]
      public void CreateCommonObjects() {
         _sequence = string.Empty;
         _spec = new TestRun(new AllTestsCriterion());
         _listener = new SimpleTestListener();
         _methodSequence = string.Empty;
         _tc = new TestFixture(typeof(ClassWithAttributes));
      }
      
      [Test]
      public void InstanceSetToFixtureInstance() {
         var tf = new TestFixture(typeof(ClassUnderTest));
         Assert.Equals(typeof(ClassUnderTest), tf.FixtureInstance.GetType());
      }

      [Test]
      public void MethodWithSetup() {
         var fixture = new TestFixture(typeof(ClassUnderTestWithSetup));
         fixture.Execute(_spec, _listener);
         Assert.Equals(" TheSetupMethod ATestMethod", _sequence);
         Assert.Equals(" TS:ATestMethod TP:ATestMethod", SimpleTestListener.Messages);
      }

      [Test]
      public void SetUpThrowsException() {
         var fixture = new TestFixture(typeof(ClassWithExceptionInSetup));
         fixture.Execute(_spec, _listener);
         Assert.Equals(" ThrowsException()", _sequence);
         Assert.Contains("NullReferenceException", SimpleTestListener.Messages);
         Assert.Contains("TestFixtureTests.cs:line 66", SimpleTestListener.Messages);
         Assert.DoesNotContain(" TS:AMethod TP:AMethod", SimpleTestListener.Messages);
      }

      [Test]
      public void TearDownThrowsException() {
         var fixture = new TestFixture(typeof(ClassWithExceptionInTearDown));
         fixture.Execute(_spec, _listener);
         Assert.Equals(" AMethod ThrowsException()", _sequence);
         Assert.Contains(" TS:AMethod TP:AMethod TE:ThrowsException System.NullReferenceException", SimpleTestListener.Messages);
         Assert.Contains("TestFixtureTests.cs:line 81", SimpleTestListener.Messages);
      }

      private class FixtureWithIgnoredMethod {
         [Test]
         public void ATestMethod() {
         }

         [Test]
         [Ignore("TheReason")]
         public void IgnoredMethodWithReason() {
         }
      }

      [Test]
      public void NameReportedForSkippedMethod() {
         var fixture = new TestFixture(typeof(FixtureWithIgnoredMethod));

         var spec = new TestSpec();

         spec.AddTest(new TestMethod(fixture,
            typeof(FixtureWithIgnoredMethod).GetMethod("ATestMethod")));
         spec.AddTest(new TestMethod(fixture,
            typeof(FixtureWithIgnoredMethod).GetMethod("IgnoredMethodWithReason")));

         fixture.Execute(spec, new SimpleTestListener());
         Assert.Contains("csUnit.Core.Tests#"
            + typeof(FixtureWithIgnoredMethod).FullName + "#"
            + "IgnoredMethodWithReason#TheReason#", 
            SimpleTestListener.IgnoredItems);
      }

      private ITestRun _spec;
      private ITestListener _listener = new SimpleTestListener();
      private static string _sequence = string.Empty;

      #region Attribute Test
      [TestFixture]
      private class ClassWithAttributes {
         [SetUp]
         public void SetUpMethod() {
            _methodSequence += "SetUpMethod";
         }

         [TearDown]
         public void CleanupMethod() {
            _methodSequence += "CleanupMethod";
         }

         [Test]
         public void FirstTestMethod() {
            _methodSequence += "FirstTestMethod";
         }

         [Test]
         public void SecondTestMethod() {
            _methodSequence += "SecondTestMethod";
         }

         [Test]
         [Ignore("Test is incomplete yet")]
         public void IgnoredTestMethod() {
            _methodSequence += "IgnoredTestMethod";
         }

         public void NonTestMethod() {
            _methodSequence += "NonTestMethod";
         }
      }

      [Test]
      public void TestRunTests() {
         _tc.Execute(new TestRun(new AllTestsCriterion()), new NullListener());
         Assert.StartsWith("SetUpMethod", _methodSequence);
      }

      [Test]
      public void RunOneTestOnly() {
         var testSpec = new TestSpec();
         var t = typeof(ClassWithAttributes);
         testSpec.AddTest(t.Assembly.FullName, t.FullName, "FirstTestMethod");
         _tc.Execute(testSpec, new NullListener());
         Assert.Equals("SetUpMethodFirstTestMethodCleanupMethod", _methodSequence);
      }

      #endregion

      #region Mock1
      //------------------------------------
      [TestFixture]
      private class Mock1 {
         [SetUp]
         public void SetUp() {
            _methodSequence += "SetUp";
         }

         [TearDown]
         public void Cleanup() {
            _methodSequence += "Cleanup";
         }

         [Test]
         public void Test1() {
            _methodSequence += "Test1";
            throw new Exception("test1 has an error");
         }
      }

      [Test]
      public void TearDownIsExecutedAfterExceptionInTest() {
         var tc = new TestFixture(typeof(Mock1));
         tc.Execute(new TestRun(new AllTestsCriterion()), new NullListener());
         Assert.Equals("SetUpTest1Cleanup", _methodSequence);
      }

      #endregion

      #region Mock2
      //------------------------------------
      [TestFixture]
      private class Mock2 {
         [FixtureSetUp]
         public void FixtureSetUp() {
            _methodSequence += "FixtureSetUp";
         }

         [FixtureTearDown]
         public void FixtureTearDown() {
            _methodSequence += "FixtureTearDown";
         }

         [SetUp]
         public void SetUp() {
            _methodSequence += "SetUp";
         }

         [TearDown]
         public void Cleanup() {
            _methodSequence += "Cleanup";
         }

         [Test]
         public void Test1() {
            _methodSequence += "Test1";
         }

         [Test]
         public void Test2() {
            _methodSequence += "Test2";
         }

         [Test]
         public void Test3() {
            _methodSequence += "Test2";
         }

         [Test, Ignore("don't run")]
         public void IgnoredTest() {
            _methodSequence += "IgnoredTest";
         }
      }

      [Test]
      public void RunOneTestOnlyWithFixtureSetUpTearDown() {
         var tc = new TestFixture(typeof(Mock2));
         var testSpec = new TestSpec();
         var t = typeof(Mock2);
         testSpec.AddTest(t.Assembly.FullName, t.FullName, "Test2");
         tc.Execute(testSpec, new NullListener());
         Assert.Equals("FixtureSetUpSetUpTest2CleanupFixtureTearDown", _methodSequence);
      }

      [Test]
      public void RunTwoOutOfThree() {
         var tc = new TestFixture(typeof(Mock2));
         var testSpec = new TestSpec();
         var t = typeof(Mock2);
         testSpec.AddTest(t.Assembly.FullName, t.FullName, "Test2");
         testSpec.AddTest(t.Assembly.FullName, t.FullName, "Test1");
         tc.Execute(testSpec, new NullListener());
         Assert.Equals("FixtureSetUpSetUpTest1CleanupSetUpTest2CleanupFixtureTearDown", _methodSequence);
      }

      [Test]
      public void IgnoredTestIsNotRun() {
         var tc = new TestFixture(typeof(Mock2));
         var testSpec = new TestSpec();
         var t = typeof(Mock2);
         testSpec.AddTest(t.Assembly.FullName, t.FullName,  "IgnoredTest");
         tc.Execute(testSpec, new NullListener());
         Assert.Equals("FixtureSetUpFixtureTearDown", _methodSequence);
      }

      #endregion

      #region Inheritance Tests
      //----------------------------------------------------
      [TestFixture]
      private class BaseFixture {
         [Test]
         public virtual void ATest() {
         }
      }

      private class DerivedFixture : BaseFixture {
         public override void ATest() {
         }
      }

      private class AssemblyMock : IAssembly {
         #region IAssembly Members

         public void Load(AssemblyName assemblyName) {
         }

         public string FullName {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public string CodeBase {
            get { throw new NotImplementedException(); }
         }

         public Type[] GetExportedTypes() {
            return new [] {
               typeof(DerivedFixture),
               typeof(TestFixtureTests)
               };
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         #endregion
      }

      [Test]
      public void TestAttributeInherited() {
         AssemblyFactory.Type = typeof(AssemblyMock);
         var assembly = new TestAssembly(GetType().Assembly.GetName());//   new AssemblyName(GetType().Assembly.CodeBase));
         foreach (TestFixtureInfo tfi in assembly.TestFixtureInfos) {
            if (tfi.FullName.Equals("csUnit.Core.Tests.TestFixtureTests+DerivedFixture")) {
               return;
            }
         }
         Assert.Fail("TestFixture didn't inherit TestFixtureAttribute.");
      }
      #endregion

      private class Foo {
         [Test]
         public void Test1() {
            _methodSequence += "Test1()";
         }

         [Test]
         public void Test1AsPrefix() {
            _methodSequence += "Test1AsPrefix()";
         }
      }

      [Test]
      public void TestNameIsPrefixOfSecondTestName() {
         var testSpec = new TestSpec();
         var testFixture = new TestFixture(typeof(Foo));
         var testMethod = new TestMethod(testFixture, typeof(Foo).GetMethod("Test1"));
         testSpec.AddTest(testMethod);
         testFixture.Execute(testSpec, new NullListener());
         Assert.Equals("Test1()", _methodSequence);
      }

      private class MyCriterion : ICriterion {
         public MyCriterion() {
            HasBeenCalled = false;
         }
         #region Implementation of ICriterion

         public bool Contains(ITestMethod testMethod) {
            HasBeenCalled = true;
            return true;
         }

         public bool Contains(ITestFixture testFixture) {
            HasBeenCalled = true;
            return true;
         }

         #endregion

         public bool HasBeenCalled { get; private set; }
      }

      [Test]
      public void FilterGetsCalled() {
         var fixture = new TestFixture(typeof(Foo));
         var criterion = new MyCriterion();
         fixture.Execute(new TestRun(criterion), new NullListener());
         Assert.True(criterion.HasBeenCalled, "Filter wasn't called.");
      }

      private class FixtureWithDuplicateDefaultSetUpMethods {
         [SetUp]
         public void SetUp1() {
            _messages += " SetUp1()";
         }

         [SetUp]
         public void SetUp2() {
            _messages += " SetUp2()";
         }

         [Test]
         public void Foo() {
            _messages += " Foo()";
         }

         private static string _messages = string.Empty;
      }

      [Test]
      public void DuplicateDefaultSetUpMethodsFails() {
         var fixture = new TestFixture(typeof(FixtureWithDuplicateDefaultSetUpMethods));
         fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.FailureItems.Count);
         Assert.Contains("Invalid configuration: More than one applicable SetUp method:" + 
            " {" + typeof(FixtureWithDuplicateDefaultSetUpMethods).FullName + ".SetUp1," +
            " " + typeof(FixtureWithDuplicateDefaultSetUpMethods).FullName + ".SetUp2}",
            SimpleTestListener.Messages);
      }

      private class FixtureWithDuplicateCategorySetUp {
         [SetUp(Categories = "green")]
         public void GreenSetUp1() {
            _messages += " GreenSetUp1()";
         }

         [SetUp(Categories = "green")]
         public void GreenSetUp2() {
            _messages += " GreenSetUp1()";
         }

         [Test(Categories="green")]
         public void Foo() {
            _messages += " Foo()";
         }

         private static string _messages = string.Empty;
      }

      [Test]
      public void DuplicateCategorySetUpMethodFails() {
         var fixture = new TestFixture(typeof(FixtureWithDuplicateCategorySetUp));
         fixture.Execute(new TestRun(new CategoryCriterion("green")),
                         new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.FailureItems.Count);
         Assert.Contains("Invalid configuration: More than one applicable SetUp method:" +
            " {" + typeof(FixtureWithDuplicateCategorySetUp).FullName + ".GreenSetUp1," +
            " " + typeof(FixtureWithDuplicateCategorySetUp).FullName + ".GreenSetUp2}",
            SimpleTestListener.Messages);
      }

      private class FixtureWithDuplicateDefaultTearDown {
         [TearDown]
         public void TearDown1() {
            _messages += " TearDown1()";
         }

         [TearDown]
         public void TearDown2() {
            _messages += " TearDown2()";
         }

         [Test]
         public void Bar() {
            _messages += " Bar()";
         }

         private static string _messages = string.Empty;
      }

      [Test]
      public void DuplicateTearDownMethodsFails() {
         var tf = new TestFixture(typeof(FixtureWithDuplicateDefaultTearDown));
         tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.FailureItems.Count);
         Assert.Contains("Invalid configuration: More than one applicable TearDown method:" +
            " {" + typeof(FixtureWithDuplicateDefaultTearDown).FullName + ".TearDown1," +
            " " + typeof(FixtureWithDuplicateDefaultTearDown).FullName + ".TearDown2}",
            SimpleTestListener.Messages);
      }

      private class FixtureWithDuplicateCategoryTearDown {
         [TearDown(Categories = "green")]
         public void GreenTearDown1() {
            _messages += " GreenTearDown1()";
         }

         [TearDown(Categories = "green")]
         public void GreenTearDown2() {
            _messages += " GreenTearDown1()";
         }

         [Test(Categories = "green")]
         public void Foo() {
            _messages += " Foo()";
         }

         private static string _messages = string.Empty;
      }

      [Test]
      public void DuplicateCategoryTearDownMethodFails() {
         var fixture = new TestFixture(typeof(FixtureWithDuplicateCategoryTearDown));
         fixture.Execute(new TestRun(new CategoryCriterion("green")),
                         new SimpleTestListener());
         Assert.Equals(1, SimpleTestListener.FailureItems.Count);
         Assert.Contains("Invalid configuration: More than one applicable TearDown method:" +
            " {" + typeof(FixtureWithDuplicateCategoryTearDown).FullName + ".GreenTearDown1," +
            " " + typeof(FixtureWithDuplicateCategoryTearDown).FullName + ".GreenTearDown2}",
            SimpleTestListener.Messages);
      }
      
      private TestFixture _tc;
      private static string _methodSequence = string.Empty;
   }
   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}

#endif // DEBUG
