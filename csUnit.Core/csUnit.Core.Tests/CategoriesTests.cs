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
using System.IO;
using System.Reflection;

using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   [TestFixture(Categories = "Alpha")]
   public class CategoriesTests {
      [TestFixture]
      private class ClassWithColoredTests {
         [Test(Categories = "Red" )]
         public void RedTest() {
            _sequence += " RedTest";
         }

         [Test]
         public void BlackTest() {
            _sequence += " BlackTest";
         }
      }

      [SetUp]
// ReSharper disable UnusedMember.Global
      public void SetUp() {
// ReSharper restore UnusedMember.Global
         _sequence = string.Empty;
         _listener = new SimpleTestListener();
         _methodSequence = string.Empty;
         _tc = new TestFixture(typeof(ClassWithCategories));
         RecipeFactory.Type = RecipeFactory.Default;
      }

      [Test]
      public void RunRedTestsOnly() {
         var tf = new TestFixture(typeof(ClassWithColoredTests));
         tf.Execute(new TestRun(new CategoryCriterion("Red")), _listener);
         Assert.Equals(" RedTest", _sequence);
      }

      [Test]
      public void InheritedCategoriesWithNoDeclaringType() {
         var tf = new TestFixture(typeof(CategoriesTests));
         Assert.True(tf.InheritedCategories.IsEmpty);
      }

      private ITestListener _listener = new SimpleTestListener();
      private static string _sequence = string.Empty;

      /// <summary>
      /// Class used for testing.
      /// </summary>
      [TestFixture]
      private class ClassWithCategories {
         // ReSharper disable UnusedMember.Local
         [SetUp]
         public void DefaultSetUp() {
            _methodSequence += " DefaultSetUp";
         }

         [SetUp(Categories = "Cat1")]
         public void Cat1SetUp() {
            _methodSequence += " Cat1SetUp";
         }

         [SetUp(Categories = "Cat2")]
         public void Cat2SetUp() {
            _methodSequence += " Cat2SetUp";
         }

         [Test]
         public void UncategorizedTestMethod() {
            _methodSequence += " UncategorizedTestMethod";
         }

         [Test(Categories = "Cat1")]
         public void Cat1TestMethod() {
            _methodSequence += " Cat1TestMethod";
         }

         [Test(Categories = "Cat2")]
         public void Cat2TestMethod() {
            _methodSequence += " Cat2TestMethod";
         }

         [TearDown]
         public void DefaultTearDown() {
            _methodSequence += " DefaultTearDown";
         }

         [TearDown(Categories = "Cat1")]
         public void Cat1TearDown() {
            _methodSequence += " Cat1TearDown";
         }

         [TearDown(Categories = "Cat2")]
         public void Cat2TearDown() {
            _methodSequence += " Cat2TearDown";
         }
         // ReSharper restore UnusedMember.Local
      }

      [TestFixture(Categories = "Panda")]
      private class AFixtureWithCategory {
      }

      [Test]
      public void RunAllCategories() {
         var setCriterion = new MultipleTestsCriterion();
         var t = typeof(ClassWithCategories);
         setCriterion.Add(t.Assembly.FullName, t.FullName, "Cat1TestMethod");
         setCriterion.Add(t.Assembly.FullName, t.FullName, "Cat2TestMethod");
         setCriterion.Add(t.Assembly.FullName, t.FullName, "UncategorizedTestMethod");
         _tc.Execute(new TestRun(setCriterion), new NullListener());
         Assert.Equals(" DefaultSetUp UncategorizedTestMethod DefaultTearDown" +
            " Cat1SetUp Cat1TestMethod Cat1TearDown" +
            " Cat2SetUp Cat2TestMethod Cat2TearDown",
            _methodSequence);
      }

      [Test]
      public void Cat1CategoryTestOnly() {
         var categoryCriterion = new CategoryCriterion("Cat1");
         var setCriterion = new MultipleTestsCriterion();
         var t = typeof(ClassWithCategories);
         setCriterion.Add(t.Assembly.FullName, t.FullName, "Cat1TestMethod");
         setCriterion.Add(t.Assembly.FullName, t.FullName, "Cat2TestMethod");
         setCriterion.Add(t.Assembly.FullName, t.FullName, "UncategorizedTestMethod");
         var testRun = TestRun.Where(setCriterion).And(categoryCriterion);
         _tc.Execute(testRun, new NullListener());
         Assert.Equals(" Cat1SetUp Cat1TestMethod Cat1TearDown", _methodSequence);
      }

      [Test]
      public void Cat2CategoryTestOnly() {
         var t = typeof(ClassWithCategories);
         var category = new CategoryCriterion("Cat2");
         var theTests = new MultipleTestsCriterion();
         theTests.Add(t.Assembly.FullName, t.FullName, "Cat1TestMethod");
         theTests.Add(t.Assembly.FullName, t.FullName, "Cat2TestMethod");
         theTests.Add(t.Assembly.FullName, t.FullName, "UncategorizedTestMethod");
         var testRun = TestRun.Where(theTests).And(category);
         _tc.Execute(testRun, new NullListener());
         Assert.Equals(" Cat2SetUp Cat2TestMethod Cat2TearDown", _methodSequence);
      }

      [Test]
      public void TestFixtureWithDefaultCategory() {
         var tf = new TestFixture(typeof(ClassWithCategories));
         Assert.Equals(0, tf.Categories.Count);
      }

      [Test]
      public void TestFixtureInfoWithDefaultCategory() {
         var tfi = new TestFixtureInfo(new TestFixture(typeof(ClassWithCategories)));
         Assert.Equals(0, tfi.Categories.Count);
      }

      [Test]
      public void TestFixtureReportsCategory() {
         var tf = new TestFixture(typeof(CategoriesTests));
         Assert.Equals(1, tf.Categories.Count);
         Assert.True(tf.Categories.Contains("Alpha"));
      }

      [Test]
      public void TestFixtureInfoReportsCategory() {
         var tfi = new TestFixtureInfo(new TestFixture(typeof(CategoriesTests)));
         Assert.Equals(1, tfi.Categories.Count);
         Assert.True(tfi.Categories.Contains("Alpha"));
      }

      private class AssemblyMock : ITestAssembly {
         #region ITestAssembly Members
#pragma warning disable 67
         public event AssemblyEventHandler AssemblyChanged;

         public event AssemblyEventHandler AssemblyLoaded;

         public event AssemblyEventHandler AssemblyStarted;

         public event AssemblyEventHandler AssemblyFinished;

         public event AssemblyEventHandler TestsAborted;

         public event TestEventHandler TestStarted;

         public event TestEventHandler TestPassed;

         public event TestEventHandler TestFailed;

         public event TestEventHandler TestError;

         public event TestEventHandler TestSkipped;
#pragma warning restore 67
         public AssemblyName Name {
            get {
               return _assemblyName;
            }
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         public TestFixtureInfoCollection TestFixtureInfos {
            get {
               return new TestFixtureInfoCollection {
                  new TestFixtureInfo(new TestFixture(typeof(ClassWithCategories ))),
                  new TestFixtureInfo(new TestFixture(typeof(AFixtureWithCategory)))
               };
            }
         }

         public void Refresh() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RunTests(ITestRun testRun) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Abort() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void SetConsoleOutputTo(TextWriter value) {}

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion

         private readonly AssemblyName _assemblyName = new AssemblyName {
            CodeBase = "c:\\Nirvana.dll",
            Name = "Nirvana"
         };
      }

      [Test]
      public void CategoriesFound() {
         var current = RecipeFactory.NewRecipe(string.Empty) as Recipe;
         if( current != null ) {
            current.AddAssembly(new AssemblyMock());
            Assert.True(current.Categories.Contains("Cat1"), "Doesn't contain category 'Cat1'.");
            Assert.True(current.Categories.Contains("Cat2"), "Doesn't contain category 'Cat2'.");
            Assert.True(current.Categories.Contains("Panda"), "Doesn't contain category 'Panda'.");
            current.Close();
         }
         else {
            Assert.Fail("Couldn't create new recipe.");
         }
      }

      [Test]
      public void OldCategoryMappedToCategoriesProperty() {
         var tf = new TestFixture(typeof(AFixtureWithCategory));
         Assert.True(tf.Categories.Contains("Panda"), "TestFixture doesn't contain expected category 'Panda'.");
      }

      [Test]
      public void EmptyStringIsIgnoredAsCategory() {
         var cats = new Categories();
         cats.Add(string.Empty);
         Assert.Equals(0, cats.Count);
      }

      //------------------------------------
      private TestFixture _tc;
      private static string _methodSequence = string.Empty;

      private class ModifiedListener {
         public void OnModified(object sender, EventArgs args) {
            EventCount++;
         }

         public int EventCount;
      }

      [Test]
      public void AddFiresModifiedEvent() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Add("bla");
         Assert.Equals(1, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test]
      public void AddFiresModifiedEventOnlyIfSomethingHasBeenAdded() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Add("bla");
         cats.Modified += eventListener.OnModified;
         cats.Add("bla"); // adding the same shouldn't fire
         Assert.Equals(0, eventListener.EventCount, 
            "Modified event was fired unnecessarily.");
      }

      [Test]
      public void AddStringArrayFiresOnceOnly() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Add(new[] { "red", "green", "blue" });
         Assert.Equals(1, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test]
      public void AddCategoriesFiresModifiedEvent() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         var otherCats = new Categories();
         otherCats.Add(new[] { "red", "green", "blue" });
         cats.Add(otherCats);
         Assert.Equals(1, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test]
      public void ClearOnNonEmptyFiresModifiedEvent() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Add("bla");
         cats.Clear();
         Assert.Equals(2, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test]
      public void ClearDoesntFireWhenEmpty() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Clear();
         Assert.Equals(0, eventListener.EventCount, "Modified event was fired unexpectedly.");
      }

      [Test]
      public void RemoveFiresModifiedEvent() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Add("blue");
         cats.Remove("blue");
         Assert.Equals(2, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test]
      public void RemoveFiresOnlyWhenCatRemoved() {
         var eventListener = new ModifiedListener();
         var cats = new Categories();
         cats.Modified += eventListener.OnModified;
         cats.Add("blue");
         cats.Remove("red");
         Assert.Equals(1, eventListener.EventCount, "Modified event wasn't fired.");
      }

      [Test(Categories="grey,blue")]
      public void AddedStringIsTrimmed() {
         var cats = new Categories();
         cats.Add(" blue ");
         Assert.Equals(1, cats.Count);
         Assert.Contains("blue", cats.ToArray());
         Assert.DoesNotContain(" blue ", cats.ToArray());
      }

      [Test(Categories="grey")]
      public void StringsTrimmedWhenArrayAdded() {
         var strings = new[] { " blue ", " green " };
         var cats = new Categories();
         cats.Add(strings);
         Assert.Equals(2, cats.Count);
         Assert.Contains("blue", cats.ToArray());
         Assert.DoesNotContain(" blue ", cats.ToArray());
         Assert.Contains("green", cats.ToArray());
         Assert.DoesNotContain(" green ", cats.ToArray());
      }
   }
}
#endif // DEBUG
