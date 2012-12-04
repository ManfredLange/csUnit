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
using System.IO;
using System.Reflection;

using csUnit.Core.Criteria;

namespace csUnit.Core.Tests {
   /// <summary>
   /// TestAssemblyFixtureAttributeTests validates that a class with the 
   /// TestAssemblyFixtureAttribute is instantiated once, then its SetUp is
   /// called, all tests are executed, and finally the TearDown method is called
   /// once. The instance of the class is kept while the tests are run.
   /// </summary>
   [TestFixture]
   public class TestAssemblyFixtureAttributeTests {
      private class ATestAssembly : IAssembly {
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
            return null == AssemblyFixtureType ? new[] { typeof(ATestClass) } 
               : new [] { typeof(ATestClass), AssemblyFixtureType };
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         #endregion

         public static Type AssemblyFixtureType;

         [TestFixture]
         private class ATestClass {
            [Test]
            public void TheOnlyTestInThisAssembly() {
               Console.WriteLine("Executing " +
                  typeof(ATestClass).FullName + ".TheOnlyTestInThisAssembly()");
            }
         }
      }

      [TestAssemblyFixture]
      private class StandardAssemblyFixture {
         [SetUp]
         public void ExecutedBeforeAnyTest() {
            SimpleTestListener.Messages +=
               "Executing " + GetType().FullName + ".ExecutedBeforeAnyTest()";
         }

         [TearDown]
         public void ExecutedOnceAfterAllTests() {
            SimpleTestListener.Messages += 
               "Executing " + GetType().FullName + ".ExecutedOnceAfterAllTests()";
         }
      }


      [Test]
      public void AssemblyHasNoTestAssemblyFixture() {
         AssemblyFactory.Type = typeof(ATestAssembly);
         var ta = new TestAssembly(new AssemblyName("ATestAssembly"));
         ta.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Contains("TheOnlyTestInThisAssembly", SimpleTestListener.Messages);
      }

      [Test]
      public void AssemblyTestFixtureWithSetupAndTearDown() {
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(StandardAssemblyFixture);
         (new TestAssembly(new AssemblyName("ATestAssembly"))).RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected = "Executing " + ATestAssembly.AssemblyFixtureType.FullName + ".ExecutedBeforeAnyTest()"
            + " TS:TheOnlyTestInThisAssembly TP:TheOnlyTestInThisAssembly"
            + "Executing " + ATestAssembly.AssemblyFixtureType.FullName + ".ExecutedOnceAfterAllTests()";
         Assert.Equals(expected, SimpleTestListener.Messages);
      }

      [TestAssemblyFixture]
      private class AssemblyFixtureWithSetUpOnly {
         [SetUp]
         public void ExecutedBeforeAnyTest() {
            SimpleTestListener.Messages += 
               "Executing " + GetType().FullName + ".ExecutedBeforeAnyTest()";
         }
      }

      [Test]
      public void AssemblyFixtureHasSetupOnly() {
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(AssemblyFixtureWithSetUpOnly);
         (new TestAssembly(new AssemblyName("ATestAssembly"))).RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected = "Executing " + ATestAssembly.AssemblyFixtureType.FullName + ".ExecutedBeforeAnyTest()"
            + " TS:TheOnlyTestInThisAssembly TP:TheOnlyTestInThisAssembly";
         Assert.Equals(expected, SimpleTestListener.Messages);
      }

      [TestAssemblyFixture]
      private class AssemblyFixtureWithTearDownOnly {
         [TearDown]
         public void TearThisDown() {
            SimpleTestListener.Messages +=
               "Executing " + GetType().FullName + ".TearThisDown()";
         }
      }

      [Test]
      public void AssemblyFixtureHasTearDownOnly() {
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(AssemblyFixtureWithTearDownOnly);
         (new TestAssembly(new AssemblyName("ATestAssembly"))).RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected =
            " TS:TheOnlyTestInThisAssembly TP:TheOnlyTestInThisAssembly" +
            "Executing " + ATestAssembly.AssemblyFixtureType.FullName + ".TearThisDown()";
         Assert.Equals(expected, SimpleTestListener.Messages);
      }

      [TestAssemblyFixture]
      private class AssemblyFixtureWithPrivateConstructor {
         private AssemblyFixtureWithPrivateConstructor() {
         }

         [SetUp]
         public void SetUp() {
         }

         [TearDown]
         public void TearDown() {
         }
      }

      [Test]
      public void HasPrivateDefaultConstructorOnly() {
         var consoleOutput = new StringWriter();
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(AssemblyFixtureWithPrivateConstructor);
         var ta = new TestAssembly(new AssemblyName("ATestAssembly"));
         Console.SetOut(consoleOutput);
         ta.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected =
            "Warning: no public parameterless constructor found for class:"
            + LineFeed +
            "         " + ATestAssembly.AssemblyFixtureType.FullName + LineFeed +
            "         TestAssemblyFixture will be ignored";
         Assert.Contains(expected, consoleOutput.ToString());
      }

      [TestAssemblyFixture]
      private class AssemblyFixtureThrowsInSetUp {
         [SetUp]
         public void SetUp() {
            throw new Exception("Exception thrown in " + GetType().FullName +
               ".SetUp()");
         }
      }

      [Test]
      public void SetUpThrowsException() {
         var consoleOutput = new StringWriter();
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(AssemblyFixtureThrowsInSetUp);
         var ta = new TestAssembly(new AssemblyName("ATestAssembly"));
         Console.SetOut(consoleOutput);
         ta.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected =
            "Error: Exception thrown in SetUp method of TestAssemblyFixture:"
            + LineFeed +
            "       Tests depending on it might fail. Error details follow."
            + LineFeed +
            "Exception thrown in " + ATestAssembly.AssemblyFixtureType.FullName
            + @".SetUp()";
         Assert.Contains(expected, consoleOutput.ToString());
      }

      [TestAssemblyFixture]
      private class AssemblyFixtureThrowsInTearDown {
         [TearDown]
         public void TearDown() {
            throw new Exception("Exception thrown in " + GetType().FullName +
               ".TearDown()");
         }
      }
      
      [Test]
      public void TearDownThrowsException() {
         var consoleOutput = new StringWriter();
         AssemblyFactory.Type = typeof(ATestAssembly);
         ATestAssembly.AssemblyFixtureType = typeof(AssemblyFixtureThrowsInTearDown);
         var ta = new TestAssembly(new AssemblyName("ATestAssembly"));
         Console.SetOut(consoleOutput);
         ta.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         var expected =
            "Executing " + GetType().FullName + @"+ATestAssembly+ATestClass.TheOnlyTestInThisAssembly()" + LineFeed +
            "Error: Exception thrown in TearDown method of TestAssemblyFixture:" + LineFeed +
            "       Assembly might not properly tear down used resources. Error details follow." + LineFeed +
            "Exception thrown in " + ATestAssembly.AssemblyFixtureType.FullName + @".TearDown()" + LineFeed;
         Assert.Contains(expected, consoleOutput.ToString());
      }

      #region Fields
      private readonly string LineFeed = "\r\n";
      #endregion
   }
}
