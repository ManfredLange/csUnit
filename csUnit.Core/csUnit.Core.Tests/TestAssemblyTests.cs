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
   // ReSharper disable UnusedMember.Global
   // ReSharper disable UnusedMember.Local
   /// <summary>
   /// Summary description for TestAssemblyTests.
   /// </summary>
   [TestFixture]
   public class TestAssemblyTests {
      [TestFixture]
      public class ConsoleTests {
         [SetUp]
         public void Setup() {
            RecipeFactory.Type = RecipeFactory.Default;
         }

         [TearDown]
         public void TearDown() {
            RecipeFactory.Type = RecipeFactory.Default;
         }

         [Test]
         public void ConsoleIsRedirected() {
            var assemblyPathName = Environment.CurrentDirectory + "/ConsoleRedirection.dll";

            using(var recipe = RecipeFactory.NewRecipe(string.Empty)) {
               var temporarilyStoredConsoleOut = Console.Out;
               Console.SetOut(MyConsole);
               recipe.SetConsoleOutputTo(MyConsole);
               recipe.AddAssembly(assemblyPathName);
               recipe.RunTests(new TestRun(new AllTestsCriterion()));
               recipe.Join();
               Console.SetOut(temporarilyStoredConsoleOut);
            }

            Assert.Contains("Show me the list:" + MyConsole.NewLine + "A" + MyConsole.NewLine
               + "B" + MyConsole.NewLine + "C" + MyConsole.NewLine + "# end of list #" + MyConsole.NewLine, 
               MyConsole.GetStringBuilder().ToString());
         }

         public void AMethod() {
            Console.WriteLine("This should be ignored");
         }

         private class MyStringWriter : StringWriter {
         }

         private static readonly MyStringWriter MyConsole = new MyStringWriter();
      }

      [TestFixture(Categories="Blue")]
      [Ignore("Ignored for testing reasons.")]
      private class IgnoredFixture {
         [Test]
         public void Foo() {
         }

         [TestFixture]
         public class NestedFixture {
            [Test]
            public void Bar() {
            }
         }
      }

      private class ASpecialAssembly : IAssembly {

         #region IAssembly Members

         public void Load(AssemblyName assemblyName) {
         }

         public string FullName {
            get {
               return GetType().Assembly.FullName;
            }
         }

         public string CodeBase {
            get { throw new NotImplementedException(); }
         }

         public Type[] GetExportedTypes() {
            return new[] { 
               typeof(IgnoredFixture), 
               typeof(IgnoredFixture.NestedFixture)
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
      public void IgnoreAttributeOnTestFixture() {
         AssemblyFactory.Type = typeof(ASpecialAssembly);
         var assembly = new TestAssembly(new AssemblyName("bla"));
         assembly.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.DoesNotContain("Foo", SimpleTestListener.Messages);
      }

      [Test]
      public void IgnoredTestFixtureIsReported() {
         AssemblyFactory.Type = typeof(ASpecialAssembly);
         var assembly = new TestAssembly(new AssemblyName("bla"));
         assembly.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.Contains(GetType().Assembly.FullName +
            "#csUnit.Core.Tests.TestAssemblyTests+IgnoredFixture##Ignored for testing reasons.#",
            SimpleTestListener.IgnoredItems);
      }

      [Test]
      public void TestFixtureNestedInIgnoredTestFixture() {
         AssemblyFactory.Type = typeof(ASpecialAssembly);
         var assembly = new TestAssembly(new AssemblyName("bla"));
         assembly.RunTests(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
         Assert.DoesNotContain("Bar", SimpleTestListener.Messages);
      }

      [Test]
      public void IgnoreAppliedOnlyAfterTestSpec() {
         AssemblyFactory.Type = typeof(ASpecialAssembly);
         var assembly = new TestAssembly(new AssemblyName("bla"));

         // The fixture is actually in category "Blue"!
         assembly.RunTests(new TestRun(new CategoryCriterion("Green")), new SimpleTestListener());
         
         Assert.Equals(0, SimpleTestListener.IgnoredItems.Length,
            "Ignored tests that were not specified.");
      }

      [TearDown]
      public void CleanUp() {
         AssemblyFactory.Type = AssemblyFactory.Default;
         RecipeFactory.Type = RecipeFactory.Default;
      }
   }
   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}
