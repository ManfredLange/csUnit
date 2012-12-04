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
using System.Collections;
using System.Collections.Generic;

using csUnit.Common;

namespace csUnit.Tests {
   namespace AssertTests {
      [TestFixture]
      public class AreInstancesOfType {
         [Test]
         public void SimpleAreInstancesOfType() {
            var list1 = new List<int> {5, 2};
            Assert.AreInstancesOfType(typeof(int), list1);
         }

         private class Base {
         }

         private class Derived : Base {
         }

         [Test]
         public void AreInstanceOfTypeWhenDerivedPasses() {
            var list1 = new List<Base> { new Base(), new Derived() };
            Assert.AreInstancesOfType(typeof(Base), list1);
         }

         [Test]
         public void AreInstanceOfTypeWhenDerivedFails() {
            var list1 = new List<Base> { new Base(), new Derived() };
            try {
#line 63
               Assert.AreInstancesOfType(typeof(Derived), list1);
#line default
               Assert.Fail();
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 63", ex.Message);
               Assert.Equals("All elements of collection are of type <csUnit.Tests.AssertTests.AreInstancesOfType+Base>", ex.Expected);
               Assert.Equals("Found element of type <csUnit.Tests.AssertTests.AreInstancesOfType+Base>", ex.Actual);
               Assert.Equals("Change type parameter of collection to <csUnit.Tests.AssertTests.AreInstancesOfType+Base>", ex.Tip);
            }
         }
      }


      #region TestData
      public class UserClass {
         public int m_i;
         public String m_s;
         public float m_f;

         public override bool Equals(Object obj) {
            var uc = (UserClass) obj;
            if(uc == null) {
               return false;
            }
            return (m_i == uc.m_i && m_s.Equals(uc.m_s)) && m_f == uc.m_f;
         }

         public override int GetHashCode() {
            return m_i.GetHashCode() ^ m_s.GetHashCode() ^ m_f.GetHashCode();
         }
      }

#pragma warning disable 659
      public class BaseClass {
#pragma warning restore 659
         public override bool Equals(object obj) {
            return obj is BaseClass;
         }
      }

      public class DerivedClass : BaseClass {
      }

#pragma warning disable 659
      public class Customer1 {
#pragma warning restore 659
         public override bool Equals(object obj) {
            if(obj is Customer1
               || obj is Customer2)
               return true;
            return false;
         }
      }

#pragma warning disable 659
      public class Customer2 {
#pragma warning restore 659
         public override bool Equals(object obj) {
            return obj is Customer1 || obj is Customer2;
         }
      }
      #endregion

      #region Assert Fail
      [TestFixture]
      public class AssertFailTests {
         [Test]
         public void StandardMessage() {
            try {
#line 141
               Assert.Fail();
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Test failed for unknown reason. (file AssertTest.cs, line 141)", ex.Message);
            }
         }

         [Test]
         public void CustomMessage() {
            try {
#line 153
               Assert.Fail("My message");
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("My message (file AssertTest.cs, line 153)", e.Message);
            }
         }
      }
      #endregion

      #region Contains
      [TestFixture]
      public class ContainsTests {
         [Test]
         public void Contains() {
            String[] strings = new String[] { "first", "second" };

            Assert.Contains("first", strings);

            try {
#line 171
               Assert.Contains("xxx", strings);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 171", e.Message);
               Assert.Equals("Array <first, second> contains <xxx>.", e.Expected);
               Assert.Equals("<xxx> not found.", e.Actual);

            }
         }

         [Test]
         public void StringContainedInOtherString() {
            string s = "The brown fox.";
            Assert.Contains("brown", s);
         }

         [Test]
         public void StringContainedInOtherStringFails() {
            string s = "The brown fox.";
            try {
#line 191
               Assert.Contains("cow", s);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 191", e.Message);
               Assert.Equals("String contains <cow>", e.Expected);
               Assert.Equals("Not found in <The brown fox.>", e.Actual);
            }
         }

         [Test]
         public void ContainsWithArrayWithMessage() {
            try {
#line 206
               Assert.Contains("John", new string[] { "Frank", "James" }, "John's not in the set!");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 206", e.Message);
               Assert.Equals("Array <Frank, James> contains <John>.", e.Expected);
               Assert.Equals("<John> not found.", e.Actual);
            }
         }

         [Test]
         public void ContainsInStringWithMessage() {
            try {
#line 220
               Assert.Contains("John", "First=James Last=Smith", "John's not the user!");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("John's not the user! (file AssertTest.cs, line 220)", e.Message);
               Assert.Equals("String contains <John>", e.Expected);
               Assert.Equals("Not found in <First=James Last=Smith>", e.Actual);
            }
         }
         
         [Test]
         public void ForArray() {
            int[] someInts = new int[] { 1, 2, 3 };
            try {
#line 237
               Assert.Contains(4, someInts);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 237", ex.Message);
               Assert.Equals("Int32[] contains <4>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         private class SomethingToEnumerate<T> : IEnumerable<T> {
            public void Add(T item) {
               _content.Add(item);
            }
            IEnumerator<T> IEnumerable<T>.GetEnumerator() {
               return _content.GetEnumerator();
            }

            public IEnumerator GetEnumerator() {
               return ((IEnumerable<T>) this).GetEnumerator();
            }
            private readonly List<T> _content = new List<T>();
         }

         [Test]
         public void ForGenericEnumerator() {
            SomethingToEnumerate<int> target = new SomethingToEnumerate<int>();
            target.Add(5);
            Assert.Contains(5, target);
         }

         [Test]
         public void ForGenericEnumeratorFailing() {
            SomethingToEnumerate<Int32> target = new SomethingToEnumerate<int>();
            try {
#line 273
               Assert.Contains(5, target);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 273", ex.Message);
               Assert.Equals("SomethingToEnumerate<Int32> contains <5>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericEnumeratorFailingWithCustomMessage() {
            SomethingToEnumerate<Int32> target = new SomethingToEnumerate<int>();
            try {
#line 288
               Assert.Contains(5, target, "What a crap!");
#line default
            }
            catch( TestFailed ex ) {
               Assert.Equals("What a crap! (file AssertTest.cs, line 288)", ex.Message);
               Assert.Equals("SomethingToEnumerate<Int32> contains <5>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericList() {
            List<string> list = new List<string>();
            try {
#line 295
               Assert.Contains("bla", list);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 295", ex.Message);
               Assert.Equals("List<String> contains <bla>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericSet() {
            Set<string> set = new Set<string>();
            try {
#line 310
               Assert.Contains("bla", set);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 310", ex.Message);
               Assert.Equals("Set<String> contains <bla>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericSetWithIntegers() {
            Set<int> set = new Set<int>();
            try {
#line 325
               Assert.Contains(35, set);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 325", ex.Message);
               Assert.Equals("Set<Int32> contains <35>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericListCounted() {
            int startCount = Assert.Count;
            List<int> list = new List<int>();
            list.Add(4);
            Assert.Contains(4, list);
            int endCount = Assert.Count;
            Assert.Equals(startCount + 1, endCount);
         }

         [Test]
         public void ForGenericListWithCustomMessage() {
            List<int> list = new List<int>();
            try {
#line 350
               Assert.Contains(4, list, "Custom message.");
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Custom message. (file AssertTest.cs, line 350)", ex.Message);
               Assert.Equals("List<Int32> contains <4>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericSetWithCustomMessage() {
            Set<int> set = new Set<int>();
            try {
#line 365
               Assert.Contains(4, set, "Custom message.");
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Custom message. (file AssertTest.cs, line 365)", ex.Message);
               Assert.Equals("Set<Int32> contains <4>.", ex.Expected);
               Assert.Equals("Element not found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericListWithCustomMessageCounted() {
            int startCount = Assert.Count;
            List<int> list = new List<int>();
            list.Add(4);
            Assert.Contains(4, list, "Custom message");
            int endCount = Assert.Count;
            Assert.Equals(startCount + 1, endCount);
         }
      }
      #endregion // Contains

      #region ContainsType
      [TestFixture]
      public class ContainsType {
         private interface IFoo {
         }

         private class Bar : IFoo {
         }

         [Test]
         public void ObjectOfTypeInSet() {
            Set<IFoo> set = new Set<IFoo>();
            set.Add(new Bar());
            Assert.ContainsType(typeof(Bar), set);
         }

         [Test]
         [ExpectedException(typeof(TestFailed))]
         public void OnEmptyCollection() {
            List<Int32> list = new List<int>();
            Assert.ContainsType(typeof(Int32), list);
         }
      }
      #endregion // ContainsType

      #region DoesNotContain
      [TestFixture]
      public class DoesNotContainTests {
         [Test]
         public void WithStringsPasses() {
            string expected = "sprinkle";
            string toBeSearched = "The brown fox jumps over the hedge.";
            Assert.DoesNotContain(expected, toBeSearched);
         }

         [Test]
         public void WithStringsPassesCustomMessage() {
            string expected = "sprinkle";
            string toBeSearched = "The brown fox jumps over the hedge.";
            Assert.DoesNotContain(expected, toBeSearched, "Custom message");
         }

         [Test]
         public void WithStringsFails() {
            string expected = "fox";
            string toBeSearched = "The brown fox jumps over the hedge.";
            try {
#line 450
               Assert.DoesNotContain(expected, toBeSearched);
#line default
               Assert.Fail("DoesNotContain did not fail.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 450", ex.Message);
               Assert.Equals("<The brown fox jumps over the hedge.> does not contain <fox>.", ex.Expected);
               Assert.Equals("<fox> found.", ex.Actual);
            }
         }

         [Test]
         public void WithStringsFailsWithCustomMessage() {
            string expected = "fox";
            string toBeSearched = "The brown fox jumps over the hedge.";
            try {
#line 467
               Assert.DoesNotContain(expected, toBeSearched, "Content-free custom message.");
#line default
               Assert.Fail("DoesNotContain did not fail.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Content-free custom message. (file AssertTest.cs, line 467)", ex.Message);
               Assert.Equals("<The brown fox jumps over the hedge.> does not contain <fox>.", ex.Expected);
               Assert.Equals("<fox> found.", ex.Actual);
            }
         }

         [Test]
         public void StringStringCounted() {
            string searchToken = "sprinkle";
            string toBeSearched = "The brown fox jumps over the hedge.";
            int startCount = Assert.Count;
            Assert.DoesNotContain(searchToken, toBeSearched);
            int finishCount = Assert.Count;
            Assert.Equals(startCount + 1, finishCount);
         }

         [Test]
         public void StringStringCustomMessageCounted() {
            string searchToken = "sprinkle";
            string toBeSearched = "The brown fox jumps over the hedge.";
            int startCount = Assert.Count;
            Assert.DoesNotContain(searchToken, toBeSearched, "Custom message");
            int finishCount = Assert.Count;
            Assert.Equals(startCount + 1, finishCount);
         }

         [Test]
         public void ForGenericList() {
            List<int> list = new List<int>();
            list.Add(4);
            Assert.DoesNotContain(5, list);
         }

         [Test]
         public void ForGenericListFails() {
            List<int> list = new List<int>();
            list.Add(4);
            try {
#line 511
               Assert.DoesNotContain(4, list);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 511", ex.Message);
               Assert.Equals("List<Int32> does not contain <4>.", ex.Expected);
               Assert.Equals("<4> found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericListCounted() {
            List<int> list = new List<int>();
            int startCount = Assert.Count;
            Assert.DoesNotContain(5, list);
            int finishCount = Assert.Count;
            Assert.Equals(startCount + 1, finishCount);
         }

         [Test]
         public void ForGenericSetPasses() {
            Set<int> set = new Set<int>();
            Assert.DoesNotContain(4, set);
         }

         [Test]
         public void ForGenericSetFails() {
            Set<int> set = new Set<int>();
            set.Add(4);
            try {
#line 543
               Assert.DoesNotContain(4, set);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 543", ex.Message);
               Assert.Equals("Set<Int32> does not contain <4>.", ex.Expected);
               Assert.Equals("<4> found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericSetWithCustomMessagePasses() {
            Set<int> set = new Set<int>();
            Assert.DoesNotContain(4, set, "Custom message.");
         }

         [Test]
         public void ForGenericSetWithCustomMessageFails() {
            Set<int> set = new Set<int>();
            set.Add(4);
            try {
#line 566
               Assert.DoesNotContain(4, set, "Custom message.");
               Assert.Fail("Expected exception not thrown.");
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Custom message. (file AssertTest.cs, line 566)", ex.Message);
               Assert.Equals("Set<Int32> does not contain <4>.", ex.Expected);
               Assert.Equals("<4> found.", ex.Actual);
            }
         }

         [Test]
         public void ForGenericSetCounted() {
            Set<int> set = new Set<int>();
            int startCount = Assert.Count;
            Assert.DoesNotContain(5, set);
            int finishCount = Assert.Count;
            Assert.Equals(startCount + 1, finishCount);
         }

         [Test]
         public void ForGenericSetWithCustomMessageCounted() {
            Set<int> set = new Set<int>();
            int startCount = Assert.Count;
            Assert.DoesNotContain(5, set, "Custom message.");
            int finishCount = Assert.Count;
            Assert.Equals(startCount + 1, finishCount);
         }
      }
      #endregion

      #region Equals
      [TestFixture]
      public class Equals {
         [Test]
         public void CustomEquals() {
            Assert.Equals(new BaseClass(), new DerivedClass());
            Assert.Equals(new DerivedClass(), new BaseClass());
         }

         [Test]
         public void CustomerEquals() {
            Assert.Equals(new Customer1(), new Customer2());
            Assert.Equals(new Customer2(), new Customer1());
         }

         [Test]
         public void NullEqualsNull() {
            Assert.Equals(null, null, "null and null are not equal.");
         }

         [Test]
         public void ShortEqualsShort() {
            short expected = 5;
            short actual = 5;
            Assert.Equals(expected, actual);
         }

         [Test]
         public void ShortEqualsShortFails() {
            short expected = 5;
            short actual = 7;
            try {
#line 1425265
               Assert.Equals(expected, actual);
#line default
               Assert.Fail("Equals on shorts didn't fail.");
            }
            catch(TestFailed ex) {
               Assert.StartsWith("Failure in file AssertTest.cs, line 1425265", ex.Message);
            }
         }

         [Test]
         public void StringEqualsSelf() {
            String s = "abc";
            Assert.Equals(s, s);
         }

         [Test]
         public void StringEqualsInteger() {
            String expected = "200";
            int actual = 200;
            try {
               try {
                  Assert.Equals(expected, actual);
               }
               catch(TestFailed) {
               }
               Assert.Fail();
            }
            catch(ArgumentException) {
            }
         }

         [Test]
         public void StringEqualsEmpty() {
            string s1 = "abc";
            string s2 = string.Empty;
            try {
               Assert.Equals(s1, s2);
               Assert.Fail("An expected TestFailed exception was not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("<abc>", e.Expected);
               Assert.Equals("<>", e.Actual);
            }
         }

         [Test]
         public void UserClassEquals() {
            UserClass uc1 = new UserClass();
            UserClass uc2 = new UserClass();
            uc1.m_f = 1.0F;
            uc1.m_s = "abc";
            uc1.m_i = 2;
            uc2.m_f = 1.0F;
            uc2.m_s = "abc";
            uc2.m_i = 2;
            Assert.Equals(uc1, uc2);
            Assert.Equals(uc2, uc1);
         }

         [Test]
         public void TwoEmptyArrays() {
            object[] array1 = new object[] { };
            object[] array2 = new object[] { };
            Assert.Equals(array1, array2);
         }

         [Test]
         [ExpectedException(typeof(TestFailed))]
         public void DifferentSize() {
            object[] array1 = new object[2];
            object[] array2 = new object[] { };
            Assert.Equals(array1, array2);
         }

         [Test]
         [ExpectedException(typeof(TestFailed))]
         public void DifferentContent() {
            object[] array1 = new object[] { 5, "blue", 7 };
            object[] array2 = new object[] { 5, "green", 7 };
            Assert.Equals(array1, array2);
         }

         [Test]
         [ExpectedException(typeof(TestFailed))]
         public void FirstArrayWithNull() {
            object[] array1 = new object[] { 5, null, 7 };
            object[] array2 = new object[] { 5, "green", 7 };
            Assert.Equals(array1, array2);
         }

         [Test]
         [ExpectedException(typeof(TestFailed))]
         public void SecondArrayWithNull() {
            object[] array1 = new object[] { 5, "blue", 7 };
            object[] array2 = new object[] { 5, null, 7 };
            Assert.Equals(array1, array2);
         }
      
         [Test]
         public void FailureMessageHasCorrectOrder() {
            int expected = 3;
            int actual = 5;
            try {
#line 677
               Assert.Equals(expected, actual);
#line default
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 677", ex.Message);
               Assert.Equals(string.Format("<{0}>", 3), ex.Expected);
               Assert.Equals(string.Format("<{0}>", 5), ex.Actual);
            }
         }
         
         [Test]
         public void PermittedDeltaWithDoubles() {
            Double expected = 0.33333333333;
            Double actual = 1.0 / 3.0;
            Double permittedDelta = 0.0005;
            Assert.Equals(expected, actual, permittedDelta);
         }

         [Test]
         public void BeyondPermittedDeltaWithDoubles() {
            Double expected = 0.5;
            Double actual = 0.6;
            Double permittedDelta = 0.01;
            try {
#line 735
               Assert.Equals(expected, actual, permittedDelta);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 735", e.Message);
               Assert.Equals("Delta between <0.5> and <0.6> less than <0.01>.", e.Expected);
               Assert.Equals("Delta is <0.1>", e.Actual);
            }
         }

         [Test]
         public void PermittedDeltaWithMessageWithDoubles() {
            Double expected = 0.5;
            Double actual = 0.6;
            Double permittedDelta = 0.01;
            try {
#line 753
               Assert.Equals(expected, actual, permittedDelta, "Deviation of target!");
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Deviation of target! (file AssertTest.cs, line 753)", e.Message);
               Assert.Equals("Delta between <0.5> and <0.6> less than <0.01>.", e.Expected);
               Assert.Equals("Delta is <0.1>", e.Actual);
            }
         }

         [Test]
         public void ActualIsNotANumberFails() {
            Double expected = 0.5;
            Double actual = Double.NaN;
            try {
#line 765
               Assert.Equals(expected, actual);
#line default
               Assert.Fail("Excpected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 765", e.Message);
               Assert.Equals("<0.5>", e.Expected);
               Assert.Equals("<NaN>", e.Actual);
            }
         }

         [Test]
         public void ActualIsNotANumberWithPermittedDeltaFails() {
            Double expected = 0.5;
            Double actual = Double.NaN;
            Double permittedDelta = 0.01;

            try {
#line 789
               Assert.Equals(expected, actual, permittedDelta);
#line default
               Assert.Fail("Excpected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 789", e.Message);
               Assert.Equals("'actual' to be a number.", e.Expected);
               Assert.Equals("<NaN>", e.Actual);
            }
         }

         [Test]
         public void PermittedDeltaIsNotANumberFails() {
            Double expected = 0.5;
            Double actual = 0.5;
            Double permittedDelta = Double.NaN;
            try {
#line 807
               Assert.Equals(expected, actual, permittedDelta);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 807", e.Message);
               Assert.Equals("Permitted delta is a number.", e.Expected);
               Assert.Equals("Permitted delta not a number (NaN).", e.Actual);
            }
         }

         [Test]
         public void NotANumberEqualsNotANumber() {
            Double expected = Double.NaN;
            Double actual = Double.NaN;

            Assert.Equals(expected, actual);
         }

         [Test]
         public void AssertEqualsDoubleFailsWithNullMessage() {
            Double expected = 0.5;
            Double actual = 0.5;
            string nullMessage = null;
            try {
#line 821
               Assert.Equals(expected, actual, nullMessage);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 821", e.Message);
               Assert.Equals("Custom message not null.", e.Expected);
               Assert.Equals("Custom message is null.", e.Actual);
            }
         }
         
         [Test]
         public void PermittedDeltaWithSingles() {
            Single s1 = 0.333333f;
            Single s2 = 1.0f / 3.0f;
            Single permittedDelta = 0.0005f;
            Assert.Equals(s1, s2, permittedDelta);
         }

         [Test]
         public void BeyondPermittedDeltaWithSingles() {
            Single s1 = 0.5f;
            Single s2 = 0.6f;
            Single permittedDelta = 0.01f;
            try {
#line 859
               Assert.Equals(s1, s2, permittedDelta);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 859", e.Message);
               Assert.Equals("Delta between <0.5> and <0.6> less than <0.01>.", e.Expected);
               Assert.Equals("Delta is <0.1>", e.Actual);
            }
         }

         [Test]
         public void PermittedDeltaWithMessageWithSingles() {
            Single s1 = 0.5f;
            Single s2 = 0.6f;
            Single permittedDelta = 0.01f;
            try {
#line 877
               Assert.Equals(s1, s2, permittedDelta, "Deviation of target!");
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Deviation of target! (file AssertTest.cs, line 877)", e.Message);
               Assert.Equals("Delta between <0.5> and <0.6> less than <0.01>.", e.Expected);
               Assert.Equals("Delta is <0.1>", e.Actual);
            }
         }
               
         [Test]
         public void EqualsWithMessage() {
            string name1 = "John";
            string name2 = "James";

            try {
#line 837
               Assert.Equals(name1, name2, "First names are different.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("First names are different. (file AssertTest.cs, line 837)", e.Message);
               Assert.Equals("<John>", e.Expected);
               Assert.Equals("<James>", e.Actual);
            }
         }
      
         [Test]
         public void AssertEqualsWithSecondParameterNull() {
            UserClass uc = new UserClass();

            try {
#line 887
               Assert.Equals(uc, null);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 887", e.Message);
               Assert.Equals("'Actual' not null.", e.Expected);
               Assert.Equals("Is null reference.", e.Actual);
            }
         }

         [Test]
         public void AssertEqualsWithFirstParameterNull() {
            UserClass uc = new UserClass();

            try {
#line 904
               Assert.Equals(null, uc);
#line default
               Assert.Fail();
            }
            catch( TestFailed e ) {
               Assert.Equals("Failure in file AssertTest.cs, line 904", e.Message);
               Assert.Equals("'Expected' not null.", e.Expected);
               Assert.Equals("Is null reference.", e.Actual);
               Assert.Equals("If this is intented, use Assert.IsNull() instead.", e.Tip);
            }
            
         }

         [Test]
         public void StringEqualsString() {
            String expected = "abc";
            String actual = "abc";
            Assert.Equals(expected, actual);
         }
      }
      #endregion // Equals

      #region False
      [TestFixture]
      public class False {
         [Test]
         public void AssertFalse() {
            Assert.False(false);

            try {
#line 879
               Assert.False(true);
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 879", e.Message);
               Assert.Equals("'false'", e.Expected);
               Assert.Equals("'true'", e.Actual);
            }
         }

         [Test]
         public void FalseWithMessage() {
            try {
#line 894
               Assert.False(3 == 3, "That's crazy!");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("That's crazy! (file AssertTest.cs, line 894)", e.Message);
               Assert.Equals("'false'", e.Expected);
               Assert.Equals("'true'", e.Actual);
            }
         }
}
      #endregion // False

      #region Greater
      [TestFixture]
      public class GreaterTests {
         [Test]
         public void ForStrings() {
            string s1 = "aaa";
            string s2 = "bbb";

            Assert.Greater(s2, s1);
         }

         [Test]
         public void ForStringFails() {
            string s1 = "aaa";
            string s2 = "bbb";

            try {
#line 1010
               Assert.Greater(s1, s2);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1010", e.Message);
               Assert.Equals("<aaa> greater than <bbb>.", e.Expected);
               Assert.Equals("Equal or less than <bbb>.", e.Actual);
            }
         }

         [Test]
         public void WithIntegers() {
            Assert.Greater(2, 1);
         }

         [Test]
         public void WithImcompatibleTypes() {
            try {
#line 1030
               Assert.Greater(1, "aaa");

#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1030", e.Message);
               Assert.Equals("'expected' and 'actual' to be of the same time.", e.Expected);
               Assert.Equals("'expected' of type <System.Int32>, 'actual' of type <System.String>", e.Actual);
            }
         }

         [Test]
         public void WithMessage() {
            string name1 = "Frank";
            string name2 = "James";

            try {
#line 1049
               Assert.Greater(name1, name2, "Are you kiddin'?");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Are you kiddin'? (file AssertTest.cs, line 1049)", e.Message);
            }
         }

         [Test]
         public void WithDoublesPermittedDeltaAndMessage() {
            double expectedGreater = 2.1;
            double expectedLess = 2.0;
            double permittedDelta = 0.0;
            string message = "Content-free message.";

            Assert.Greater(expectedGreater, expectedLess, permittedDelta, message);
         }

         [Test]
         public void WithDoublesAndPermittedDeltaFails() {
            double expectedGreater = 2.1;
            double expectedLesser  = 2.5;
            double permittedDelta = 5.0;

            try {
#line 1076
               Assert.Greater(expectedGreater, expectedLesser, permittedDelta);
#line default
               Assert.Fail("Greater worked instead of throwing exception.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 1076", ex.Message);
               Assert.Equals("<" + expectedGreater + "> greater than <"
                  + expectedLesser + "> with tolerance of <" + permittedDelta + ">.", ex.Expected);
               Assert.Equals("Delta is <0.4>.", ex.Actual);
            }
         }

         [Test]
         public void WithDoublesRejectsNegativeDelta() {
            double expectedGreater = 2.0;
            double expectedLesser  = 1.5;
            double permittedDelta  = -0.5;
            try {
#line 1095
               Assert.Greater(expectedGreater, expectedLesser, permittedDelta);
#line default
               Assert.Fail("Greater worked instead of throwing exception.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1095", e.Message);
               Assert.Equals(string.Empty, e.Expected);
               Assert.Equals("Permitted delta must not be negative.", e.Actual);
            }
         }

         [Test]
         public void WithTwoShorts() {
            short expectedGreater = 4;
            short expectedLesser = 2;
            Assert.Greater(expectedGreater, expectedLesser);
         }

         [Test]
         public void WithTwoShortsFails() {
            short expectedGreater = 2;
            short expectedLesser = 4;
            try {
#line 1113
               Assert.Greater(expectedGreater, expectedLesser);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 1113", ex.Message);
               Assert.Equals("<" + expectedGreater + "> greater than <" + expectedLesser + ">.", ex.Expected);
               Assert.Equals("Equal or less than <" + expectedLesser + ">.", ex.Actual);
            }
         }

         [Test]
         public void WithTwoLongs() {
            long expectedGreater = 4;
            long expectedLesser = 2;
            Assert.Greater(expectedGreater, expectedLesser);
         }

         [Test]
         public void WithTwoLongsFails() {
            long expectedGreater = 2;
            long expectedLesser = 4;
            try {
#line 1139
               Assert.Greater(expectedGreater, expectedLesser);
#line default
               Assert.Fail("Greater worked instead of throwing exception.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 1139", ex.Message);
               Assert.Equals("<" + expectedGreater + "> greater than <"
                  + expectedLesser + ">.", ex.Expected);
               Assert.Equals("Equal or less than <"
                  + expectedLesser + ">.", ex.Actual);
            }
         }

         [Test]
         public void WithTwoSinglesRejectsNegativeDelta() {
            Single expectedGreater = 4;
            Single expectedLesser = 2;
            Single permittedDelta = -2;
            try {
#line 1163
               Assert.Greater(expectedGreater, expectedLesser, permittedDelta);
#line default
               Assert.Fail("Greater worked instead of throwing exception.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1163", e.Message);
               Assert.Equals(string.Empty, e.Expected);
               Assert.Equals("Permitted delta must not be negative.", e.Actual);
            }
         }

         [Test]
         public void WithTwoSinglesPermittedDeltaAndMessage() {
            Single expectedGreater = 2.1F;
            Single expectedLess = 2.0F;
            Single permittedDelta = 0.0F;
            string message = "Content-free message.";

            Assert.Greater(expectedGreater, expectedLess, permittedDelta, message);
         }

         [Test]
         public void WithTwoSinglesPermittedDeltaAndMessageFails() {
            Single expectedGreater = 2.1F;
            Single expectedLesser  = 2.5F;
            Single permittedDelta = 5.0F;

            try {
#line 1192
               Assert.Greater(expectedGreater, expectedLesser, permittedDelta);
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Failure in file AssertTest.cs, line 1192", ex.Message);
               Assert.Equals("<" + expectedGreater + "> greater than <"
                  + expectedLesser + "> with tolerance of <" + permittedDelta + ">.", ex.Expected);
               Assert.Equals("Delta is <0.4000001>.", ex.Actual);
            }
         }
      }
      #endregion

      #region Less
      [TestFixture]
      public class Less {
         [Test]
         public void TestLess() {
            string s1 = "aaa";
            string s2 = "bbb";

            Assert.Less(s1, s2);
         }

         [Test]
         public void LessFails() {
            string s1 = "aaa";
            string s2 = "bbb";

            try {
#line 1224
               Assert.Less(s2, s1);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1224", e.Message);
               Assert.Equals("<bbb> less than <aaa>.", e.Expected);
               Assert.Equals("Is greater or equal.", e.Actual);
            }
         }

         [Test]
         public void LessWithIntegers() {
            Assert.Less(1, 2);
         }

         [Test]
         public void LessWithIncompatibleTypes() {
            try {
#line 1244
               Assert.Less(1, "aaa");
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1244", e.Message);
               Assert.Equals(string.Empty, e.Expected);
               Assert.Equals("Assert.Less(): Types are incompatible: System.Int32 and System.String.", e.Actual);
            }
         }

         [Test]
         public void LessWithMessage() {
            string name1 = "Frank";
            string name2 = "James";

            try {
#line 1262
               Assert.Less(name2, name1, "What's this!?");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("What's this!? (file AssertTest.cs, line 1262)", e.Message);
               Assert.Equals("<James> less than <Frank>.", e.Expected);
               Assert.Equals("Is greater or equal.", e.Actual);
            }
         }
      }
      #endregion // Less

      #region NotEquals
      [TestFixture]
      public class NotEquals {
         [Test]
         public void StringNotEqualsInteger() {
            String expected = "200";
            int actual = 200;
            Assert.NotEquals(expected, actual);
         }

         [Test]
         public void TestNotEquals() {
            String s1 = "abc";
            String s2 = "def";
            Assert.NotEquals(s1, s2);
            Assert.NotEquals(s2, s1);

            int i = 200;
            Assert.NotEquals(s1, i);
            Assert.NotEquals(i, s1);
         }

         [Test]
         public void UserClassNotEquals() {
            UserClass uc1 = new UserClass();
            UserClass uc2 = new UserClass();
            uc1.m_f = 1.0F;
            uc1.m_s = "abc";
            uc1.m_i = 2;
            uc2.m_f = 1.5F;
            uc2.m_s = "def";
            uc2.m_i = 3;
            Assert.NotEquals(uc1, uc2);
            Assert.NotEquals(uc2, uc1);
         }

         [Test]
         public void AssertNotEqualsWithNullParameter() {
            UserClass uc = new UserClass();

            Assert.NotEquals(uc, null);
         }
      
         [Test]
         public void NotEqualsWithMessage() {
            string name1 = "John";
            string name2 = "John";

            try {
#line 1325
               Assert.NotEquals(name1, name2, "First names are equal.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("First names are equal. (file AssertTest.cs, line 1325)", e.Message);
               Assert.Equals("<John> and <John> are different.", e.Expected);
               Assert.Equals("They are equal.", e.Actual);
            }
         }
}
      #endregion // NotEquals

      #region NotNull
      [TestFixture]
      public class NotNull {
         [Test]
         public void TestNotNull() {
            Object obj = null;
            try {
#line 1346
               Assert.NotNull(obj);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1346", e.Message);
               Assert.Equals("Reference not null.", e.Expected);
               Assert.Equals("It is null.", e.Actual);
            }
         }

         [Test]
         public void NotNullWithMessage() {
            try {
#line 1361
               Assert.NotNull(null, "Should return a string.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Should return a string. (file AssertTest.cs, line 1361)", e.Message);
               Assert.Equals("Reference not null.", e.Expected);
               Assert.Equals("It is null.", e.Actual);
            }
         }
      }
      #endregion // NotNull

      #region Null
      [TestFixture]
      public class Null {
         [Test]
         public void TestNull() {
            Assert.Null(null);
         }

         [Test]
         public void TestNullFails() {
            string s = "abc";
            try {
#line 1380
               Assert.Null(s);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1380", e.Message);
               Assert.Equals("Reference is <null>.", e.Expected);
               Assert.Equals("Not <null>.", e.Actual);
            }
         }

         [Test]
         public void NullWithMessage() {
            try {
#line 1395
               Assert.Null("John", "Should have returned null.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Should have returned null. (file AssertTest.cs, line 1395)", e.Message);
               Assert.Equals("Reference is <null>.", e.Expected);
               Assert.Equals("Not <null>.", e.Actual);
            }
         }
      }
      #endregion // Null

      #region Numerics
      [TestFixture]
      public class Numerics {
         [Test]
         public void LongAndIntAreEqual() {
            int i = 5;
            long l = 5;
            Assert.Equals(i, l);
         }

         [Test]
         public void SignedAndUnsignedAreEqual() {
            UInt16 ui = 5;
            Int16 i = 5;
            Assert.Equals(ui, i);
         }

         [Test]
         public void UnsignedIntegerAndDoubleAreEqual() {
            UInt32 ui = 5;
            Double d = 5.0;
            Assert.Equals(ui, d);
         }

         [Test]
         public void SignedIntegerAndDoubleAreEqual() {
            Int32 i = -5;
            Double d = -5.0;
            Assert.Equals(i, d);
         }

         [Test, ExpectedException(typeof(TestFailed))]
         public void SignedIntegerAndDoubleAreNotEqual() {
            Int16 i = -7;
            Double d = 5.3;
            Assert.Equals(i, d);
         }
      }
      #endregion

      #region ReferenceEqualsTests
      [TestFixture]
      public class ReferenceEqualsTests {
         [Test]
         public void AreSameReference() {
            UserClass obj1 = new UserClass();
            UserClass obj2 = obj1;
            Assert.ReferenceEquals(obj1, obj2);
         }

         [Test]
         public void AreDifferentReference() {
            UserClass obj1 = new UserClass();
            UserClass obj2 = new UserClass();
            try {
#line 1471
               Assert.ReferenceEquals(obj1, obj2);
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1471", e.Message);
               Assert.Equals("References refer to same object.", e.Expected);
               Assert.Equals("References refer to two different objects.", e.Actual);
            }
         }

         [Test]
         public void AreDifferentReferenceWithMessage() {
            UserClass obj1 = new UserClass();
            UserClass obj2 = new UserClass();
            try {
#line 1488
               Assert.ReferenceEquals(obj1, obj2, "Just a message.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Just a message. (file AssertTest.cs, line 1488)", e.Message);
               Assert.Equals("References refer to same object.", e.Expected);
               Assert.Equals("References refer to two different objects.", e.Actual);
            }
         }
      }
      #endregion

      #region RegexEquals
      [TestFixture]
      public class RegexEquals {
         [Test]
         public void TestRegexEquals() {
            string strToTest = "ThisisMytest123";
            string regex = "^[a-zA-Z]+([0-9]+$)";
            Assert.EqualsRegex(strToTest, regex);
         }

         [Test]
         public void RegexEqualsWithMessage() {
            string strToTest = "ThisisMytest123";
            string regex = "^[0-9][a-zA-Z]+([0-9]+$)";
            try {
#line 1449
               Assert.EqualsRegex(strToTest, regex, "Regular expression matching is not working.");
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Regular expression matching is not working. (file AssertTest.cs, line 1449)", ex.Message);
               Assert.Equals("<" + strToTest +"> matches regular expression <" + regex + ">.", ex.Expected);
               Assert.Equals("No match.", ex.Actual);
            }
         }
      }
      #endregion

      #region RegexNotEquals
      [TestFixture]
      public class RegexNotEquals {
         [Test]
         public void SimpleRegexNotEquals() {
            string strToTest = "ThisisMytest123";
            string regex = "^[0-9][a-zA-Z]+([0-9]+$)";
            Assert.NotEqualsRegex(strToTest, regex);
         }

         [Test]
         public void RegexNotEqualsWithMessage() {
            string strToTest = "ThisisMytest123";
            string regex = "^[a-zA-Z]+([0-9]+$)";
            try {
#line 1524
               Assert.NotEqualsRegex(strToTest, regex, "Regular expression matching is not working.");
#line default
               Assert.Fail("Expected exception not thrown.");
            }
            catch(TestFailed ex) {
               Assert.Equals("Regular expression matching is not working. (file AssertTest.cs, line 1524)",ex.Message);
               Assert.Equals("<" + strToTest + "> does not match regular expression <" + regex + ">.", ex.Expected);
               Assert.Equals("It matches.", ex.Actual);
            }
         }
      }
      #endregion

      #region StartsWith
      [TestFixture]
      public class StartsWith {
         [Test]
         public void TestStartsWith() {
            String a = "abcdef";
            Assert.StartsWith("abc", a);
            try {
#line 1568
               Assert.StartsWith("xxx", a);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1568", e.Message);
               Assert.Equals("String to start with <xxx>.", e.Expected);
               Assert.Equals("String starts with <abcdef>.", e.Actual);
            }
         }

         [Test]
         public void StartsWithWithMessage() {
            try {
#line 1583
               Assert.StartsWith("John", "Smith, John", "Name must begin with first name.");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Name must begin with first name. (file AssertTest.cs, line 1583)", e.Message);
               Assert.Equals("String to start with <John>.", e.Expected);
               Assert.Equals("String starts with <Smith, John>.", e.Actual);
            }
         }
      }
      #endregion // StartsWith

      #region True
      [TestFixture]
      public class True {
         [Test]
         public void AssertTrue() {
            Assert.True(true);

            try {
#line 1443
               Assert.True(false);
#line default
               Assert.Fail();
            }
            catch(TestFailed e) {
               Assert.Equals("Failure in file AssertTest.cs, line 1443", e.Message);
               Assert.Equals("'true'", e.Expected);
               Assert.Equals("'false'", e.Actual);
            }
         }

         [Test]
         public void TrueWithMessage() {
            try {
#line 1458
               Assert.True(3 == 5, "Are u inventing new maths?");
#line default
               Assert.Fail("Exception not thrown.");
            }
            catch(TestFailed e) {
               Assert.Equals("Are u inventing new maths? (file AssertTest.cs, line 1458)", e.Message);
               Assert.Equals("'true'", e.Expected);
               Assert.Equals("'false'", e.Actual);
            }
         }
      }
      #endregion // True
   }
}
