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
using System.Xml;
using csUnit.Common;
using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   [TestFixture]
   public class CategorySelectorTests {
      [SetUp]
      public void Setup() {
         Foo._messages = string.Empty;
         FooWithCategory._messages = string.Empty;
         XmlDocumentMock.Reset();
         RecipeFactory.Type = RecipeFactory.Default;
      }

      [TestFixture]
      private class Foo {
         [Test]
         public void Bar() {
            _messages += "#Foo.Bar";
         }
         [Test(Categories = "Green")]
         public void GreenFunc() {
            _messages += "#Foo.GreenFunc";
         }
         public static string _messages = string.Empty;
      }

      [TestFixture(Categories="Red")]
      private class FooWithCategory {
         [Test]
         public void Bar() {
            _messages += "#Foo.Bar";
         }
         [Test(Categories="Red")]
         public void GreenFunc() {
            _messages += "#Foo.GreenFunc";
         }
         public static string _messages = string.Empty;
      }
      
      [Test]
      public void SettingNoCategorySelectsAll() {
         var fixture = new TestFixture(typeof(Foo));
         var theTests = new MultipleTestsCriterion();
         theTests.Add(typeof(Foo).Assembly.FullName, typeof(Foo).FullName, string.Empty);
         fixture.Execute(new TestRun(theTests), new NullListener());
         Assert.Equals("#Foo.Bar#Foo.GreenFunc", Foo._messages);
      }

      [Test]
      public void ExecuteSelectedCategoryOnly() {
         var fixture = new TestFixture(typeof(Foo));
         var theTests = new MultipleTestsCriterion();
         theTests.Add(typeof(Foo).Assembly.FullName, typeof(Foo).FullName, string.Empty);
         var testRun = TestRun.Where(theTests).And(new CategoryCriterion("Green"));
         fixture.Execute(testRun, new NullListener());
         Assert.Equals("#Foo.GreenFunc", Foo._messages);
      }

      [Test]
      public void MethodInheritsCategoryFromFixture() {
         var fixture = new TestFixture(typeof(FooWithCategory));
         var theTests = new MultipleTestsCriterion();
         theTests.Add(typeof(FooWithCategory).Assembly.FullName, typeof(FooWithCategory).FullName, string.Empty);
         var testRun = TestRun.Where(theTests).And(new CategoryCriterion("Red"));
         fixture.Execute(testRun, new NullListener());
         Assert.Equals("#Foo.Bar#Foo.GreenFunc", FooWithCategory._messages);
      }

      private class ModifiedListener {
         public void OnModified(object sender, EventArgs args) {
            ModifiedEventFired = true;
         }

         public bool ModifiedEventFired;
      }

      [Test]
      public void FiresChangedEventWhenCategoryAdded() {
         CategorySelector selector = new CategorySelector();
         ModifiedListener eventListener = new ModifiedListener();
         selector.Modified += eventListener.OnModified;
         selector.IncludedCategories.Add("bla");
         Assert.True(eventListener.ModifiedEventFired, "Modified event wasn't fired.");
      }

      [Test]
      public void ChangingFilterMarksRecipeModified() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         CategorySelector selector = new CategorySelector();
         IRecipe recipe = RecipeFactory.NewRecipe(string.Empty);
         recipe.RegisterSelector(selector);
         recipe.Save(@"c:\nirvana.recipe");
         Assert.False(recipe.Modified, "Expected recipe not being modified.");
         selector.IncludedCategories.Add("bla");
         Assert.True(recipe.Modified, "Expected recipe being modified.");
      }

      [TestFixture]
      private class FixtureWithCategorizedTest {
         [Test(Categories="grey")]
         public void Foo() {
         }
      }

      [Test]
      public void CategorizedTestInUncategorizedFixture() {
         CategorySelector selector = new CategorySelector();
         selector.IncludedCategories.Add("grey");
         Assert.True(selector.Includes(new TestFixture(typeof(FixtureWithCategorizedTest))));
      }

      #region ExcludedCategories
      private class FixtureWithSeveralCategories {
         [Test]
         public void NoCategory() {
            _messages += "#NoCategory";
         }

         [Test(Categories = "green")]
         public void GreenTest() {
            _messages += "#GreenTest";
         }

         [Test(Categories = "blue")]
         public void BlueTest() {
            _messages += "#BlueTest";
         }

         [Test(Categories = "green,blue")]
         public void GreenBlueTest() {
            _messages += "#GreenBlueTest";
         }

         public static string _messages = string.Empty;
      }

      [Test]
      public void ExcludeCategory() {
         var fixture = new TestFixture(typeof(FixtureWithSeveralCategories));
         var testRun =
            TestRun.Where(new AllTestsCriterion()).And(
               new NotCriterion(new CategoryCriterion("blue")));
         fixture.Execute(testRun, new NullListener());
         Assert.Equals("#NoCategory#GreenTest", FixtureWithSeveralCategories._messages);
      }

      [Test]
      public void ModifyingExcludedCategoriesFiresEvent() {
         CategorySelector selector = new CategorySelector();
         ModifiedListener eventListener = new ModifiedListener();
         selector.Modified += eventListener.OnModified;
         selector.ExcludedCategories.Add("bla");
         Assert.True(eventListener.ModifiedEventFired, "Modified event wasn't fired.");
      }

      #endregion // ExcludedCategories

      #region Serialization
      // Note: Tests for whether the recipe triggers serializing selectors can
      // be found in SelectorTests. [23Mar07, ml]

      [Test]
      public void SerializeIncludedCategories() {
         CategorySelector selector = new CategorySelector();
         selector.IncludedCategories.Add("green");
         XmlElement serialized = selector.Serialize();
         Assert.Equals(SerializedContent1, serialized.OwnerDocument.InnerXml);
      }

      private const string SerializedContent1 = "<categorySelector><includedCategories><category>green</category></includedCategories></categorySelector>";

      [Test]
      public void DeserializeIncludedCategories() {
         CategorySelector selector = new CategorySelector();
         selector.Deserialize(SerializedContent1);
         Assert.Contains("green", selector.IncludedCategories.ToArray());
         Assert.Equals(1, selector.IncludedCategories.Count);
      }

      private const string SerializedContent2 = "<categorySelector><excludedCategories><category>blue</category></excludedCategories></categorySelector>";

      [Test]
      public void SerializeExcludedCategories() {
         CategorySelector selector = new CategorySelector();
         selector.ExcludedCategories.Add("blue");
         XmlElement serialized = selector.Serialize();
         Assert.Equals(SerializedContent2, serialized.OwnerDocument.InnerXml);
      }

      [Test]
      public void DeserializeExcludedCategories() {
         CategorySelector selector = new CategorySelector();
         selector.Deserialize(SerializedContent2);
         Assert.Contains("blue", selector.ExcludedCategories.ToArray());
         Assert.Equals(1, selector.ExcludedCategories.Count);
      }

      #endregion // Serialization
   }
}
