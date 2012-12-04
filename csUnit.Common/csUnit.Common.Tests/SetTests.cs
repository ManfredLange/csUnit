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

namespace csUnit.Common.Tests {
   [TestFixture]
   public class SetTests {
      [Test]
      public void EmptySet() {
         var set = new Set<int>();
         Assert.Equals(0, set.Count);
      }

      [Test]
      public void AddOneEntry() {
         var set = new Set<int>();
         set.Add(5);
         Assert.Equals(1, set.Count);
      }

      [Test]
      public void AddEmptyArray() {
         var set = new Set<int>();
         set.Add(new int[] { });
         Assert.Equals(0, set.Count);
      }

      [Test]
      public void AddArrayWithOneElement() {
         var set = new Set<int>();
         set.Add(new [] { 11 });
         Assert.Equals(1, set.Count);
      }

      [Test]
      public void AddArrayWithMultipleElements() {
         var set = new Set<int>();
         set.Add(new [] { 11, 22, 33 });
         Assert.Equals(3, set.Count);
      }

      [Test]
      public void AddArrayToNonEmptySet() {
         var set = new Set<int>();
         set.Add(11);
         set.Add(22);
         set.Add(new [] { 33, 44, 55 });
         Assert.Equals(5, set.Count);
         Assert.True(set.Contains(11));
         Assert.True(set.Contains(44));
      }

      [Test]
      public void CannotAddDuplicate() {
         var set = new Set<int>();
         set.Add(5);
         set.Add(5);
         Assert.Equals(1, set.Count);
      }

      [Test]
      public void ContainsWithEmptySet() {
         var set = new Set<int>();
         Assert.False(set.Contains(5));
      }

      [Test]
      public void ContainsReturnsTrue() {
         var set = new Set<int>();
         set.Add(5);
         Assert.True(set.Contains(5));
      }

      [Test]
      public void ContainsReturnsFalse() {
         var set = new Set<int>();
         set.Add(5);
         Assert.False(set.Contains(8));
      }

      [Test]
      public void RemoveItem() {
         var set = new Set<int>();
         set.Add(5);
         set.Add(3);
         set.Add(7);
         Assert.Equals(3, set.Count);
         Assert.True(set.Contains(5));
         set.Remove(5);
         Assert.Equals(2, set.Count);
         Assert.True(set.Contains(3));
         Assert.True(set.Contains(7));
         Assert.False(set.Contains(5));
      }

      [Test]
      public void RemovingNonExistingItemDoesNotFail() {
         var set = new Set<int>();
         set.Add(5);
         set.Remove(3);
         Assert.True(set.Contains(5));
         Assert.False(set.Contains(3));
         Assert.Equals(1, set.Count);
      }

      [Test]
      public void ClearOnEmptySet() {
         var set = new Set<int>();
         set.Clear();
         Assert.Equals(0, set.Count);
      }

      [Test]
      public void ClearOnNonEmptySet() {
         var set = new Set<int>();
         set.Add(5);
         set.Add(3);
         Assert.Equals(2, set.Count);
         set.Clear();
         Assert.Equals(0, set.Count);
      }

      [Test]
      public void IsEmptyOnEmptySet() {
         var set = new Set<int>();
         Assert.True(set.IsEmpty);
      }

      [Test]
      public void IsEmptyOnNonEmptySet() {
         var set = new Set<int>();
         set.Add(5);
         Assert.False(set.IsEmpty);
      }

      [Test]
      public void EnumerateEmptySet() {
         var set = new Set<int>();
         var count = 0;
         foreach (var i in set) {
            count++;
         }
         Assert.Equals(0, count);
      }

      [Test]
      public void EnumerateNonEmptySet() {
         var set = new Set<int>();
         var count = 0;
         var sum = 0;
         set.Add(4);
         set.Add(3);
         set.Add(8);
         foreach (var i in set) {
            count++;
            sum += i;
         }
         Assert.Equals(3, count);
         Assert.Equals(15, sum);
      }

      [Test]
      public void TestStaticEmptySet() {
         var emptySet = Set<int>.EmptySet;
         Assert.True(emptySet.IsEmpty);
         Assert.Equals(0, emptySet.Count);
      }

      [Test]
      public void CannotAddNull() {
         var set = new Set<TestAttribute>();
         set.Add(new TestAttribute());
         set.Add((TestAttribute)null);
         Assert.Equals(1, set.Count);
      }

      [Test]
      public void AddSetToSet() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(3);
         set1.Add(5);
         set2.Add(4);
         set2.Add(5);
         set1.Add(set2);
         Assert.Equals(3, set1.Count);
         Assert.True(set1.Contains(3));
         Assert.True(set1.Contains(4));
         Assert.True(set1.Contains(5));
      }

      [Test]
      public void TwoSetsAreEqual() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(3);
         set1.Add(5);
         set2.Add(3);
         set2.Add(5);
         Assert.Equals(set1, set2);
      }

      [Test]
      public void TwoEmptySetsAreEqual() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         Assert.Equals(set1, set2);
      }

      [Test]
      public void TwoSetsWithDifferentSizeAreNotEqual() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(5);
         set2.Add(5);
         set2.Add(6);
         Assert.NotEquals(set1, set2);
      }

      [Test]
      public void TestSetsSameSizeStillDifferent() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(3);
         set1.Add(5);
         set2.Add(3);
         set2.Add(6);
         Assert.NotEquals(set1, set2);
      }

      [Test]
      public void TestIntersection() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         Assert.True(set1.Intersect(set2).IsEmpty);
      }

      [Test]
      public void TestNonIntersection() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(new [] { 1, 2 });
         set2.Add(new [] { 2, 3 });
         var intersection = set1.Intersect(set2);
         Assert.Equals(1, intersection.Count);
         Assert.True(intersection.Contains(2));
      }

      [Test]
      public void TestEmptyIntersection() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         set1.Add(new [] { 1, 2 });
         set2.Add(new [] { 3, 4 });
         var intersection = set1.Intersect(set2);
         Assert.Equals(0, intersection.Count);
      }

      [Test]
      public void TestToArray() {
         var set1 = new Set<int>();
         set1.Add(new [] { 1, 2, 3, 5 });
         var intArray = set1.ToArray();
         foreach (var i in intArray) {
            Assert.True(set1.Contains(i));
         }
         Assert.Equals(set1.Count, intArray.GetLength(0));
      }

      [Test]
      public void TestCopyConstructor() {
         var set1 = new Set<int>();
         set1.Add(new [] { 1, 2, 3, 4 });
         var set2 = new Set<int>(set1);
         Assert.Equals(set1, set2);
      }

      [Test]
      public void SimpleUnion() {
         var set1 = new Set<int>();
         var set2 = new Set<int>();
         var expectedUnion = new Set<int>();
         set1.Add(new [] { 1, 2 });
         set2.Add(new [] { 3, 4 });
         expectedUnion.Add(new [] { 1, 2, 3, 4 });
         var union = set1.Union(set2);
         Assert.Equals(expectedUnion, union);
      }

      [Test]
      public void ToStringOnEmptySet() {
         var set = new Set<int>();
         Assert.Equals("{}", set.ToString());
      }

      [Test]
      public void ToStringWithOneElement() {
         var set = new Set<int>();
         set.Add(1);
         Assert.Equals("{1}", set.ToString());
      }

      [Test]
      public void ToStringWithManyElements() {
         var set = new Set<int>();
         set.Add(new [] { 1, 2, 3, 4 });
         Assert.Equals("{1, 2, 3, 4}", set.ToString());
      }

      [TestFixture]
      public class CopyToTests {
         [FixtureSetUp]
         public void FixtureSetUp() {
            _fromSet.Add(new [] { 1, 2, 3, 4, 5 });
         }

         [Test]
         public void CopyTo() {
            var toArray = new int[5];
            _fromSet.CopyTo(toArray, 0);
            Assert.Equals(1, toArray[0]);
            Assert.Equals(2, toArray[1]);
            Assert.Equals(3, toArray[2]);
            Assert.Equals(4, toArray[3]);
            Assert.Equals(5, toArray[4]);
         }

         [Test]
         [ExpectedException(typeof(ArgumentNullException))]
         public void CopyToWithNullArray() {
            _fromSet.CopyTo(null, 0);
         }

         [Test]
         [ExpectedException(typeof(ArgumentOutOfRangeException))]
         public void ArrayIndexLessThanZero() {
            var toArray = new int[5];
            _fromSet.CopyTo(toArray, -5);
         }

         [Test]
         [ExpectedException(typeof(ArgumentException))]
         [Ignore("Cannot test in C#. [ml]")]
         public void MultiDimensionalArray() {
            var toArray = new int[5, 4];
            // Here we should be able to either pass toArray to the next
            // function call, or to do a hard cast (in C++ fashion) to
            // an array with a single dimension. I couldn't find a way to
            // test this from C# yet. I tried unsafe and fixed as well.
            // [04Mar07, ml]
            //_fromSet.CopyTo(toArrayCopied, 0);
         }

         [Test]
         [ExpectedException(typeof(ArgumentException))]
         public void ArrayIndexEqualOrGreaterThanLengthOfArray() {
            var toArray = new int[3];
            _fromSet.CopyTo(toArray, 3);
         }

         [Test]
         [ExpectedException(typeof(ArgumentException))]
         public void SetSizeGreaterThanSpaceInArray() {
            var toArray = new int[3];
            Assert.Greater(_fromSet.Count, toArray.GetLength(0));
            _fromSet.CopyTo(toArray, 0);
         }

         [Test]
         [ExpectedException(typeof(ArgumentException))]
         [Ignore("Cannot test in C#. [ml]")]
         public void TypeOfSetElementsCannotBeCastToArrayType() {
            var toArray = new string[20];
            // Again, here it should be possible to pass toArray to the CopyTo()
            // method call. However, the compiler does not allow this. Using an
            // Array variable as temporary variable gets the code past the
            // compiler, but will then throw an exception when converting the
            // Array variable to type int[]. I couldn't find a way to test this
            // using C#. I tried unsafe and fixed as well. [04Mar07, ml]
            //_fromSet.CopyTo(toArray, 0);
         }

         private readonly Set<int> _fromSet = new Set<int>();
      }

      // Other set operations left out for now. They will get added on an as
      // needed basis. [06Feb2007, ml]
   }
}