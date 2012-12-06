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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace csUnit {
   /// <summary>
   /// The Assert class is used in test methods to assert a known
   /// condition.  For instance, after running some logic, to assert
   /// that a result variable has an expected value, use the <code>Assert.Equals</code>
   /// method.
   /// </summary>
   /// <example>
   /// [TestFixture]
   /// public class MyTest {
   ///   public MyTest() {
   ///   }
   ///   [Test]
   ///   public void FirstTest() {
   ///     int result = RunSomeMethod();
   ///     // Make sure the result is 25
   ///     Assert.Equals(25, result, "The result should have been 25");
   ///   }
   /// }
   /// </example>
   public abstract class Assert {
      #region Assert.AreInstancesOf
      /// <summary>
      /// Asserts that all elements of a collection are instances of an expected
      /// type. If an element is of a type that is derived from the expected 
      /// type, the test will pass.
      /// </summary>
      /// <typeparam name="T">Type of the collection.</typeparam>
      /// <param name="expectedType">Expected type of all elements in the collection.</param>
      /// <param name="collection">Collection to test.</param>
      public static void AreInstancesOfType<T>(Type expectedType, ICollection<T> collection) {
         foreach(var item in collection) {
            if(!item.GetType().Equals(expectedType)
               && !item.GetType().IsSubclassOf(expectedType)) {
               throw new TestFailed(
                  string.Format("All elements of collection are of type <{0}>", typeof(T).FullName),
                  string.Format("Found element of type <{0}>", item.GetType().FullName)) {
                  Tip = string.Format("Change type parameter of collection to <{0}>", item.GetType().FullName)
               };
            }
         }
      }
      #endregion // Assert.AreInstancesOf

      #region Assert.Contains
      /// <summary>
      /// Assert that a string is member of a string array. The search is
      /// case sensitive.
      /// </summary>
      /// <param name="expected">String to look for.</param>
      /// <param name="strings">Array of strings</param>
      /// <param name="message">Message to display if 'expected' is not in array.</param>
      public static void Contains(string expected, string[] strings, string message) {
         Count++;
         var temp = new StringBuilder();
         temp.Append("<");
         foreach(var s in strings) {
            if( s.Equals(expected) ) {
               return;
            }
            temp.Append(s);
            temp.Append(", ");
         }
         temp.Append(">");
         var msg = temp.ToString();
         msg = msg.Replace(", >", ">");
         throw new TestFailed(
            string.Format("Array {0} contains <{1}>.", msg, expected),
            string.Format("<{0}> not found.", expected));
      }

      /// <summary>
      /// Assert that a string is member of a string array. The search is
      /// case sensitive.
      /// </summary>
      /// <param name="expected">String to look for.</param>
      /// <param name="strings">Array of strings</param>
      public static void Contains(string expected, string[] strings) {
         Contains(expected, strings, string.Empty);
      }

      /// <summary>
      /// Assert that a string is a substring of a different string. The search
      /// is case sensitive.
      /// </summary>
      /// <param name="expected">The expected string.</param>
      /// <param name="toBeSearched">The string to search.</param>
      /// <param name="message">Message to display when 'expected' is not found 
      /// in 'toBeSearched'.</param>
      public static void Contains(string expected, string toBeSearched, string message) {
         Count++;
         if(toBeSearched.IndexOf(expected) == -1) {
            throw new TestFailed("String contains <" + expected + ">",
               "Not found in <" + toBeSearched + ">", message);
         }
      }

      /// <summary>
      /// Assert that a string is a substring of a different string. The search
      /// is case sensitive.
      /// </summary>
      /// <param name="expected">The expected string.</param>
      /// <param name="toBeSearched">The string to search.</param>
      public static void Contains(string expected, string toBeSearched) {
         Contains(expected, toBeSearched, string.Empty);
      }

      /// <summary>
      /// Asserts that an element of type T is contained in a set.
      /// </summary>
      /// <typeparam name="T">The type of the elements.</typeparam>
      /// <param name="expected">The element to search for.</param>
      /// <param name="toBeSearched">The set in which to search.</param>
      public static void Contains<T>(T expected, ICollection<T> toBeSearched) {
         Contains(expected, toBeSearched, string.Empty);
      }

      /// <summary>
      /// Asserts that an element of type T is contained in a set.
      /// </summary>
      /// <typeparam name="T">Type of the elements.</typeparam>
      /// <param name="expected">Element to search for.</param>
      /// <param name="toBeSearched">Set to search in.</param>
      /// <param name="customMessage">Message to be displayed, if the element is
      /// not found.</param>
      public static void Contains<T>(T expected, ICollection<T> toBeSearched, string customMessage) {
         Count++;
         if(!toBeSearched.Contains(expected)) {
            throw new TestFailed(FormatCollectionTypeName(toBeSearched) + " contains <" + expected + ">.",
               "Element not found.", customMessage);
         }
      }

      /// <summary>
      /// Asserts that an element of type T is contained in the enumerable object.
      /// This operation can be expensive on larger containers since the search
      /// is performed linearly.
      /// </summary>
      /// <typeparam name="T">Type of the elements.</typeparam>
      /// <param name="expected">Element to search for.</param>
      /// <param name="toBeSearched">Enumerable object to search in.</param>
      public static void Contains<T>(T expected, IEnumerable<T> toBeSearched) {
         Contains(expected, toBeSearched, string.Empty);
      }

      /// <summary>
      /// Asserts that an element of type T is contained in the enumerable object.
      /// This operation can be expensive on larger containers since the search
      /// is performed linearly.
      /// </summary>
      /// <typeparam name="T">Type of the elements.</typeparam>
      /// <param name="expected">Element to search for.</param>
      /// <param name="toBeSearched">Enumerable object to search in.</param>
      /// <param name="customMessage">Message to be displayed if assertion fails.</param>
      public static void Contains<T>(T expected, IEnumerable<T> toBeSearched,
         string customMessage) {
         Count++;
         foreach (var item in toBeSearched) {
            if( item.Equals(expected) ) {
               return;
            }
         }
         throw new TestFailed(FormatCollectionTypeName(toBeSearched) + " contains <" + expected + ">.",
            "Element not found.", customMessage);
      }

      #endregion // Assert.Contains

      #region Assert.Equals
      /// <summary>
      /// Assert two objects are equal.
      /// </summary>
      /// <param name="expected">The expected value, either literal or object.</param>
      /// <param name="actual">The actual value, typically a variable.</param>
      /// <param name="message">Message to be displayed, when not equal.</param>
      public static void Equals(object expected, object actual, string message) {
         Count++;
         if( expected == null && actual == null ) {
            return;
         }
         if( expected == null ) {
            throw new TestFailed(
               "'Expected' not null.",
               "Is null reference.",
               message) {
                           Tip = "If this is intented, use Assert.IsNull() instead."
                        };
         }
         if( actual == null ) {
            throw new TestFailed(
               "'Actual' not null.",
               "Is null reference.",
               message);
         }

         if( expected is ValueType && actual is ValueType ) {
            NormalizeValueType(ref expected, ref actual);
            if( !expected.Equals(actual) ) {
               throw new TestFailed(
                  string.Format("<{0}>", expected), 
                  string.Format("<{0}>", actual), 
                  message);
            }
         }
         else if( !expected.Equals(actual) ) {
            if( expected.ToString() == actual.ToString()
               && !(expected.GetType() == actual.GetType()) ) {
               throw new ArgumentException("Parameters are not of the same type.");
            }
            throw new TestFailed(
               string.Format("<{0}>", expected), 
               string.Format("<{0}>", actual), 
               message);
         }
      }

      /// <summary>
      /// Assert the equality of two long primitives and emit the
      /// default message when they are not equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void Equals(long expected, long actual) {
         Equals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the equality of two long primitive types and issues a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void Equals(long expected, long actual, string message) {
         Equals(expected, (object) actual, message);
      }

      /// <summary>
      /// Assert the equality of two integer primitives and emit the
      /// default message when they are not equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void Equals(int expected, int actual) {
         Equals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the equality of two integer primitive types and issues a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void Equals(int expected, int actual, string message) {
         Equals(expected, (object) actual, message);
      }

      /// <summary>
      /// Assert the equality of two short primitives and emit the
      /// default message when they are not equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void Equals(short expected, short actual) {
         Equals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the equality of two short primitive types and issues a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void Equals(short expected, short actual, string message) {
         Equals(expected, (object) actual, message);
      }

      /// <summary>
      /// Assert two objects are equal.
      /// </summary>
      /// <param name="expected">The expected value, either literal or object.</param>
      /// <param name="actual">The actual value, typically a variable.</param>
      new public static void Equals(object expected, object actual) {
         Equals(expected, actual, string.Empty);
      }

      /// <summary>
      /// Assert that two arrays are equal. The instances can be different. 
      /// This assertions verifies that the content of both is the same.
      /// </summary>
      /// <param name="expected">Expected array.</param>
      /// <param name="actual">Actual array.</param>
      public static void Equals(object[] expected, object[] actual) {
         Count++;
         if(expected.Length != actual.Length) {
            throw new TestFailed( "<" + expected + ">", "<" + actual + ">");
         }
         for(var i = 0; i < expected.Length; i++) {
            if(expected[i] != null
               || !expected[i].Equals(actual[i])) {
               throw new TestFailed("<" + expected + ">", "<" + actual + ">");
            }
         }
      }

      /// <summary>
      /// Asserts that two doubles are equal.
      /// </summary>
      /// <param name="expected">The expected value.</param>
      /// <param name="actual">The actual value.</param>
      /// <remarks>For numerical operations it is in many cases necessary to 
      /// allow for a delta. If this is what you want, then please use one of 
      /// the overloads that take a permitted delta as the
      /// third parameter.</remarks>
      public static void Equals(double expected, double actual) {
         Equals(expected, actual, string.Empty);
      }

      /// <summary>
      /// Asserts that two doubles are equal. Overload with custom message.
      /// </summary>
      /// <param name="expected">The expected value.</param>
      /// <param name="actual">The actual value.</param>
      /// <param name="customMessage">Message to dispay if assertion fails.</param>
      public static void Equals(double expected, double actual, string customMessage) {
         Count++;
         if( customMessage == null ) {
            throw new TestFailed(
               "Custom message not null.",
               "Custom message is null.");
         }
         if( expected.CompareTo(actual) != 0 ) {
            throw new TestFailed(
               "<" + expected + ">",
               "<" + actual + ">",
               customMessage);
         }
      }

      /// <summary>
      /// Asserts that the difference between two doubles is not larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected double value.</param>
      /// <param name="actual">The actual double value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      /// <param name="message">Message to display if delta is larger than
      /// the permitted value.</param>
      public static void Equals(double expected, double actual, 
         double permittedDelta, string message) {
         Count++;
         if( Double.IsNaN(permittedDelta) ) {
            throw new TestFailed(
               "Permitted delta is a number.",
               "Permitted delta not a number (NaN).");
         }
         if( Double.IsNaN(expected) && Double.IsNaN(actual) ) {
            return;
         }
         if( Double.IsNaN(expected) ) {
            throw new TestFailed("'expected' to be a number.", "<NaN>");
         }
         if( Double.IsNaN(actual)) {
            throw new TestFailed("'actual' to be a number.", "<NaN>");
         }
         if( Math.Abs(expected - actual) > Math.Abs(permittedDelta) ) {
            throw new TestFailed(
               string.Format("Delta between <{0}> and <{1}> less than <{2}>.", expected, actual, permittedDelta),
               string.Format("Delta is <{0}>", Math.Abs(expected - actual)),
               message);
         }
      }

      /// <summary>
      /// Asserts that the difference between two doubles is not larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected double value.</param>
      /// <param name="actual">The actual double value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      public static void Equals(double expected, double actual, 
         double permittedDelta) {
         Equals(expected, actual, permittedDelta, string.Empty);
      }

      /// <summary>
      /// Asserts that the difference between two Singles is not larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected Single value.</param>
      /// <param name="actual">The actual Single value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      /// <param name="message">Message to display if delta is larger than
      /// the permitted value.</param>
      public static void Equals(Single expected, Single actual, Single permittedDelta,
         string message) {
         Count++;
         if( Math.Abs(expected - actual) > Math.Abs(permittedDelta) ) {
            throw new TestFailed(
               string.Format("Delta between <{0}> and <{1}> less than <{2}>.", expected, actual, permittedDelta),
               string.Format("Delta is <{0}>", Math.Abs(expected - actual)),
               message);
         }
      }

      /// <summary>
      /// Asserts that the difference between two Singles is not larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected Single value.</param>
      /// <param name="actual">The actual Single value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      public static void Equals(Single expected, Single actual, 
         Single permittedDelta) {
         Equals(expected, actual, permittedDelta, string.Empty);
      }
      #endregion

      #region Assert.EqualsRegex
      /// <summary>
      /// Compares the given string (actual) against the regular expression.
      /// If the expression does not match on the string, then an assertion is
      /// raised.
      /// </summary>
      /// <param name="actual">The string on which to test the expression</param>
      /// <param name="expression">The regular expression to test</param>
      /// <example>
      /// Assert.EqualsRegEx("thisismyname1234", "[a-zA-Z]+[0-9]*")
      /// </example>
      public static void EqualsRegex(string actual, string expression) {
         EqualsRegex(actual, expression, String.Empty);
      }

      /// <summary>
      /// Compares the given string (actual) against the regular expression.
      /// If the expression does not match on the string, then an assertion is
      /// raised.
      /// </summary>
      /// <param name="actual">The string on which to test the expression</param>
      /// <param name="expression">The regular expression to test</param>
      /// <param name="message">The user message</param>
      /// <example>
      /// Assert.EqualsRegEx("thisismyname1234", "[a-zA-Z]+[0-9]*", "Wrong Format")
      /// </example>
      public static void EqualsRegex(string actual, string expression, string message) {
         Count++;
         var r = new Regex(expression); 
         var m = r.Match(actual); 
         if (!m.Success) {
            throw new TestFailed(
               "<" + actual + "> matches regular expression <" + expression + ">.",
               "No match.",
               message);
         }
      }
      
      #endregion
      
      #region Assert.False
      /// <summary>
      /// Verify whether expression is 'false'.
      /// </summary>
      /// <param name="expression">Boolean expression checked to be 'false'.</param>
      /// <param name="message">Message to be displayed, when expression is 'true'.</param>
      public static void False(bool expression, string message) {
         Count++;
         if( expression ) {
            throw new TestFailed("'false'", "'true'", message);
         }
      }

      /// <summary>
      /// Verify whether expression is 'false'.
      /// </summary>
      /// <param name="expression">Boolean expression checked to be 'false'.</param>
      public static void False(bool expression) {
         False(expression, string.Empty);
      }
      #endregion

      #region Assert.Fail
      /// <summary>
      /// Calling this method fails a test. Generally it shouldn't be needed
      /// in your code.
      /// </summary>
      public static void Fail() {
         Fail("Test failed for unknown reason.");
      }

      /// <summary>
      /// Calling this method fails a test. Generally it shouldn't be needed
      /// in your code.
      /// </summary>
      /// <param name="message">Custom string message.</param>
      public static void Fail(string message) {
         Count++;
         throw new TestFailed(string.Empty, string.Empty, message);
      }
      #endregion
      
      #region Assert.Greater
      /// <summary>
      /// Asserts that an object is greater than a different object. Both 
      /// objects must be of the same type, and that type must implement the
      /// System.IComparable interface.
      /// </summary>
      /// <param name="expectedGreater">The object expected to be greater.</param>
      /// <param name="expectedLess">The object expected to be less.</param>
      /// <param name="message">Message to be displayed when obj1 is not greater
      ///  than obj2</param>
      public static void Greater(IComparable expectedGreater, IComparable expectedLess, string message) {
         Count++;
         if( expectedGreater.GetType() != expectedLess.GetType() ) {
            throw new TestFailed(
               "'expected' and 'actual' to be of the same time.",
               string.Format("'expected' of type <{0}>, 'actual' of type <{1}>", 
                              expectedGreater.GetType(), expectedLess.GetType()), 
               message);
         }

         if( ! (expectedGreater.CompareTo(expectedLess) > 0) ) {
            throw new TestFailed("<" + expectedGreater + "> greater than <" + expectedLess + ">.",
               "Equal or less than <" + expectedLess + ">.", message);
         }
      }

      /// <summary>
      /// Asserts that an object is greater than a different object. Both 
      /// objects must be of the same type, and that type must implement the
      /// System.IComparable interface.
      /// </summary>
      /// <param name="expectedGreater">The object expected to be greater.</param>
      /// <param name="expectedLess">The object expected to be less.</param>
      public static void Greater(IComparable expectedGreater, IComparable expectedLess) {
         Greater(expectedGreater, expectedLess, string.Empty);
      }

      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      public static void Greater(int expectedGreater, int expectedLess) {
         Greater(expectedGreater, expectedLess, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Greater(int expectedGreater, int expectedLess, string message) {
         Greater(expectedGreater, (IComparable) expectedLess, message);
      }

      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      public static void Greater(short expectedGreater, short expectedLess) {
         Greater(expectedGreater, expectedLess, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Greater(short expectedGreater, short expectedLess, string message) {
         Greater(expectedGreater, (IComparable) expectedLess, message);
      }

      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      public static void Greater(long expectedGreater, long expectedLess) {
         Greater(expectedGreater, expectedLess, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value.
      /// </summary>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Greater(long expectedGreater, long expectedLess, string message) {
         Greater(expectedGreater, (IComparable) expectedLess, message);
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <remarks>The interpretation would be one of the two:
      /// <list type=""><item>Actual &lt; (Expected ± permittedDelta)</item></list>
      /// <list type=""><item>Expected &gt; (Actual ± permittedDelta)</item></list>
      /// </remarks>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      public static void Greater(double expectedGreater, double expectedLess, double permittedDelta) {
         Greater(expectedGreater, expectedLess, permittedDelta, String.Empty);
      }

      /// <summary>
      /// Asserts the first value is greater than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <remarks>The interpretation would be one of the two:
      /// <list type=""><item>Actual &lt; (Expected ± permittedDelta)</item></list>
      /// <list type=""><item>Expected &gt; (Actual ± permittedDelta)</item></list>
      /// </remarks>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Greater(double expectedGreater, double expectedLess, double permittedDelta, string message) {
         if( permittedDelta < 0.0 ) {
            throw new TestFailed(
               string.Empty,
               "Permitted delta must not be negative.");
         }
         Count++;
         double diff = expectedGreater - expectedLess;
         if( Math.Abs(diff) <= permittedDelta ) {
            throw new TestFailed(
               string.Format("<{0}> greater than <{1}> with tolerance of <{2}>.", expectedGreater, expectedLess, permittedDelta),
               string.Format("Delta is <{0}>.", Math.Abs(diff)),
               message);
         }
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <remarks>The interpretation would be one of the two:
      /// <list type=""><item>Actual &lt; (Expected ± permittedDelta)</item></list>
      /// <list type=""><item>Expected &gt; (Actual ± permittedDelta)</item></list>
      /// </remarks>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      public static void Greater(Single expectedGreater, Single expectedLess, Single permittedDelta) {
         Greater(expectedGreater, expectedLess, permittedDelta, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is greater than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <remarks>The interpretation would be one of the two:
      /// <list type=""><item>Actual &lt; (Expected ± permittedDelta)</item></list>
      /// <list type=""><item>Expected &gt; (Actual ± permittedDelta)</item></list>
      /// </remarks>
      /// <param name="expectedGreater">The left-hand side of the inequality.</param>
      /// <param name="expectedLess">The right-hand side of the inequality.</param>
      /// <param name="permittedDelta">The equality tolerance.</param>
      /// <param name="message">An optional message to display with the default message.</param>
      public static void Greater(Single expectedGreater, Single expectedLess, Single permittedDelta, string message) {
         if( permittedDelta < 0.0 ) {
            throw new TestFailed(
               string.Empty,
               "Permitted delta must not be negative.");
         }
         Single diff = expectedGreater - expectedLess;
         Count++;
         if( Math.Abs(diff) <= permittedDelta ) {
            throw new TestFailed(
               string.Format("<{0}> greater than <{1}> with tolerance of <{2}>.", expectedGreater, expectedLess, permittedDelta),
               string.Format("Delta is <{0}>.", Math.Abs(diff)),
               message);
         }
      }
      #endregion
      
      #region Assert.Less
      /// <summary>
      /// Asserts that an object is less than a different object. Both objects
      /// must be of the same type, and that type must implement the
      /// System.IComparable interface.
      /// </summary>
      /// <param name="expectedLess">The object expected to be less.</param>
      /// <param name="expectedGreater">The object expected to be greater.</param>
      /// <param name="message">Message to be displayed when obj1 is no less.</param>
      public static void Less(IComparable expectedLess, IComparable expectedGreater, string message) {
         Count++;
         if( expectedLess.GetType() != expectedGreater.GetType() ) {
            throw new TestFailed(string.Empty,
               string.Format("Assert.Less(): Types are incompatible: {0} and {1}.", 
                              expectedLess.GetType().FullName, 
                              expectedGreater.GetType().FullName));
         }

         if( ! (expectedLess.CompareTo(expectedGreater) < 0) ) {
            throw new TestFailed(
               string.Format("<{0}> less than <{1}>.", expectedLess, expectedGreater),
               "Is greater or equal.", message);
         }
      }

      /// <summary>
      /// Asserts that an object is less than a different object. Both objects
      /// must be of the same type, and that type must implement the
      /// System.IComparable interface.
      /// </summary>
      /// <param name="expectedLess">The object expected to be less.</param>
      /// <param name="expectedGreater">The object expected to be greater.</param>
      public static void Less(IComparable expectedLess, IComparable expectedGreater) {
         Less(expectedLess, expectedGreater, string.Empty);
      }

      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      public static void Less(int expectedLess, int expectedGreater) {
         Less(expectedLess, expectedGreater, String.Empty);
      }
      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Less(int expectedLess, int expectedGreater, string message) {
         Less(expectedLess, (IComparable) expectedGreater, message);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      public static void Less(short expectedLess, short expectedGreater) {
         Less(expectedLess, expectedGreater, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Less(short expectedLess, short expectedGreater, string message) {
         Less(expectedLess, (IComparable) expectedGreater, message);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      public static void Less(long expectedLess, long expectedGreater) {
         Less(expectedLess, expectedGreater, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Less(long expectedLess, long expectedGreater, string message) {
         Less(expectedLess, (IComparable) expectedGreater, message);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      public static void Less(double expectedLess, double expectedGreater, double permittedDelta) {
         Less(expectedLess, expectedGreater, permittedDelta, String.Empty);
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Less(double expectedLess, double expectedGreater, double permittedDelta, string message) {
         double diff = expectedLess - expectedGreater;
         Count++;
         if(Math.Abs(diff) <= permittedDelta || diff > permittedDelta) {
            throw new TestFailed(
               string.Format("<{0}> less than <{1}> with tolerance of <{2}>.", expectedLess, expectedGreater, permittedDelta),
               string.Format("Delta is <{0}>.", Math.Abs(diff)),
               message);
         }
      }
      
      /// <summary>
      /// Asserts the first value is less than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      public static void Less(Single expectedLess, Single expectedGreater, Single permittedDelta) {
         Less(expectedLess, expectedGreater, permittedDelta, String.Empty);
      }

      /// <summary>
      /// Asserts the first value is less than the second value, within a specified
      /// tolerance.
      /// </summary>
      /// <param name="expectedLess">The left-hand side of the inequality</param>
      /// <param name="expectedGreater">The right-hand side of the inequality</param>
      /// <param name="permittedDelta">The equality tolerance</param>
      /// <param name="message">An optional message to display with the default message</param>
      public static void Less(Single expectedLess, Single expectedGreater, Single permittedDelta, string message) {
         Count++;
         Single diff = expectedLess - expectedGreater;
         if(Math.Abs(diff) <= permittedDelta || diff > permittedDelta) {
            throw new TestFailed(
               string.Format("<{0}> less than <{1}> with tolerance of <{2}>.", expectedLess, expectedGreater, permittedDelta),
               string.Format("Delta is <{0}>.", Math.Abs(diff)),
               message);
         }
      }
      #endregion
      
      #region Assert.NotEquals
      /// <summary>
      /// Assert two objects are NOT equal.
      /// </summary>
      /// <param name="obj1">An object to compare with.</param>
      /// <param name="obj2">Another object which is compared to the first.</param>
      /// <param name="message">Message to be display when objects are equal.</param>
      public static void NotEquals(object obj1, object obj2, string message) {
         Count++;
         if( obj1.Equals(obj2) ) {
            throw new TestFailed(
               string.Format("<{0}> and <{1}> are different.", obj1, obj2),
               "They are equal.",
               message);
         }
      }

      /// <summary>
      /// Assert two objects are NOT equal.
      /// </summary>
      /// <param name="obj1">An object to compare with.</param>
      /// <param name="obj2">Another object which is compared to the first.</param>
      public static void NotEquals(object obj1, object obj2) {
         NotEquals(obj1, obj2, string.Empty);
      }
      
      /// <summary>
      /// Assert the inequality of two long primitives and emit the
      /// default message when they are equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void NotEquals(long expected, long actual) {
         NotEquals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the equality of two long primitive types and issues a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void NotEquals(long expected, long actual, string message) {
         Count++;
         if(expected != actual) {
            return;
         }
         throw new TestFailed(
            string.Format("<{0}> and <{1}> are different.", expected, actual),
            "They are equal.", 
            message);
      }

      /// <summary>
      /// Assert the inequality of two integer primitives and emit the
      /// default message when they are equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void NotEquals(int expected, int actual) {
         NotEquals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the inequality of two integer primitive types and emits a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void NotEquals(int expected, int actual, string message) {
         Count++;
         if(expected != actual) {
            return;
         }
         throw new TestFailed(
            string.Format("<{0}> and <{1}> to be different.", expected, actual),
            "They are equal.", 
            message);
      }

      /// <summary>
      /// Assert the inequality of two short primitives and emit the
      /// default message when they are equal.
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      public static void NotEquals(short expected, short actual) {
         NotEquals(expected, actual, String.Empty);
      }

      /// <summary>
      /// Assert the inequality of two integer primitive types and emits a
      /// custom message if it is not String.Empty
      /// </summary>
      /// <param name="expected">The expected value</param>
      /// <param name="actual">The actual value</param>
      /// <param name="message">The optional message to emit</param>
      public static void NotEquals(short expected, short actual, string message) {
         Count++;
         if(expected != actual) {
            return;
         }
         throw new TestFailed(
            string.Format("<{0}> and <{1}> to be different.", expected, actual),
            "They are equal.", 
            message);
      }

      /// <summary>
      /// Asserts that the difference between two doubles is larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected double value.</param>
      /// <param name="actual">The actual double value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      /// <param name="message">Message to display if delta is not larger than
      /// the permitted value.</param>
      public static void NotEquals(double expected, double actual, 
         double permittedDelta, string message) {
         Count++;
         if( Math.Abs(expected - actual) <= Math.Abs(permittedDelta) ) {
            throw new TestFailed(
               string.Format("<{0}> and <{1}> to be different by more than <{2}>.", expected, actual, permittedDelta),
               string.Format("Difference is <{0}>.", Math.Abs(expected - actual)),
               message);
         }
      }

      /// <summary>
      /// Asserts that the difference between two doubles is larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected double value.</param>
      /// <param name="actual">The actual double value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      public static void NotEquals(double expected, double actual, 
         double permittedDelta) {
         NotEquals(expected, actual, permittedDelta, string.Empty);
      }

      /// <summary>
      /// Asserts that the difference between two Singles is larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected Single value.</param>
      /// <param name="actual">The actual Single value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      /// <param name="message">Message to display if delta is not larger than
      /// the permitted value.</param>
      public static void NotEquals(Single expected, Single actual, Single permittedDelta,
         string message) {
         Count++;
         if( Math.Abs(expected - actual) <= Math.Abs(permittedDelta) ) {
            throw new TestFailed(
               "<" + expected + "> and <" + actual + "> to be different by more than<" + permittedDelta + ">.",
               string.Format("Difference is <{0}>.", Math.Abs(expected - actual)),
               message);
         }
      }

      /// <summary>
      /// Asserts that the difference between two Singles is larger than a
      /// permitted delta.
      /// </summary>
      /// <param name="expected">The expected Single value.</param>
      /// <param name="actual">The actual Single value.</param>
      /// <param name="permittedDelta">The permitted delta between the two.</param>
      public static void NotEquals(Single expected, Single actual, 
         Single permittedDelta) {
         NotEquals(expected, actual, permittedDelta, string.Empty);
      }
      #endregion

      #region Assert.NotEqualsRegex
      /// <summary>
      /// Compares the given string (actual) against the regular expression.
      /// If the expression does match on the string, then an assertion is
      /// raised.
      /// </summary>
      /// <param name="actual">The string on which to test the expression</param>
      /// <param name="expression">The regular expression to test</param>
      /// <example>
      /// Assert.Equals("thisismyname1234", "[a-zA-Z]+[0-9]*")
      /// </example>
      public static void NotEqualsRegex(string actual, string expression) {
         NotEqualsRegex(actual, expression, String.Empty);
      }

      /// <summary>
      /// Compares the given string (actual) against the regular expression.
      /// If the expression does match on the string, then an assertion is
      /// raised.
      /// </summary>
      /// <param name="actual">The string on which to test the expression</param>
      /// <param name="expression">The regular expression to test</param>
      /// <param name="message">The user message</param>
      /// <example>
      /// Assert.Equals("thisismyname1234", "[a-zA-Z]+[0-9]*")
      /// </example>
      public static void NotEqualsRegex(string actual, string expression, string message) {
         Count++;
         var r = new Regex(expression); 
         var m = r.Match(actual); 
         if (m.Success) {
            throw new TestFailed(
               "<" + actual + "> does not match regular expression <" + expression + ">.",
               "It matches.",
               message);
         }
      }
      #endregion

      #region Assert.Null
      /// <summary>
      /// Assert a reference is 'null'.
      /// </summary>
      /// <param name="obj">The object reference to be checked.</param>
      /// <param name="message">Message to be displayed when obj is not null.</param>
      public static void Null(object obj, string message) {
         Count++;
         if( obj != null ) {
            throw new TestFailed(
               "Reference is <null>.",
               "Not <null>.",
               message);
         }
      }

      /// <summary>
      /// Assert a reference is 'null'.
      /// </summary>
      /// <param name="obj">The object reference to be checked.</param>
      public static void Null(object obj) {
         Null(obj, string.Empty);
      }
      #endregion
      
      #region Assert.NotNull
      /// <summary>
      /// Assert object is not null.
      /// </summary>
      /// <param name="obj">Reference to be checked.</param>
      /// <param name="message">Message to be displayed, when obj is null.</param>
      public static void NotNull(object obj, string message) {
         Count++;
         if( obj == null ) {
            throw new TestFailed(
               "Reference not null.",
               "It is null.", 
               message);
         }
      }

      /// <summary>
      /// Assert object is not null.
      /// </summary>
      /// <param name="obj">Object to be checked.</param>
      public static void NotNull(object obj) {
         NotNull(obj, string.Empty);
      }
      #endregion
      
      #region Assert.ReferenceEquals
      /// <summary>
      /// Assert that object reference refer to the same object.
      /// </summary>
      /// <param name="obj1">An object reference.</param>
      /// <param name="obj2">A second object reference.</param>
      new public static void ReferenceEquals(object obj1, object obj2) {
         ReferenceEquals(obj1, obj2, string.Empty);
      }

      /// <summary>
      /// Assert that two object references refer to the same object.
      /// </summary>
      /// <param name="obj1">An object reference.</param>
      /// <param name="obj2">A second object reference.</param>
      /// <param name="message">Message to display, if the both references refer to different objects.</param>
      public static void ReferenceEquals(object obj1, object obj2, string message) {
         Count++;
         if( !Object.ReferenceEquals(obj1, obj2) ) {
            throw new TestFailed(
               "References refer to same object.",
               "References refer to two different objects.",
               message);
         }
      }
      #endregion
      
      #region Assert.True
      /// <summary>
      /// Verify whether expression is 'true'.
      /// </summary>
      /// <param name="expression">Boolean expression checked to be 'true'.</param>
      /// <param name="message">Message to be displayed, when expression is 'false'.</param>
      public static void True(bool expression, string message) {
         Count++;
         if( expression == false ) {
            throw new TestFailed("'true'", "'false'", message);
         }
      }

      /// <summary>
      /// Verify whether expression is 'true'.
      /// </summary>
      /// <param name="expression">Boolean expression checked to be 'true'.</param>
      public static void True(bool expression) {
         True(expression, string.Empty);
      }
      #endregion
      
      #region Assert.StartsWith
      /// <summary>
      /// Assert that a string starts with a particular substring. The check is
      /// case sensitive.
      /// </summary>
      /// <param name="expected">Expected start of the string.</param>
      /// <param name="actual">String to check.</param>
      /// <param name="message">Message to display when 'actual' does not start 
      /// with 'expected'.</param>
      public static void StartsWith(string expected, string actual, string message) {
         Count++;
         if( actual.StartsWith(expected) == false ) {
            throw new TestFailed(
               String.Format("String to start with <{0}>.", expected),
               String.Format("String starts with <{0}>.", actual),
               message);
         }
      }

      /// <summary>
      /// Assert that a string starts with a particular substring. The check is
      /// case sensitive.
      /// </summary>
      /// <param name="expected">Expected start of the string.</param>
      /// <param name="actual">String to check.</param>
      public static void StartsWith(string expected, string actual) {
         StartsWith(expected, actual, string.Empty);
      }
      #endregion

      #region Assert.ContainsType
      /// <summary>
      /// Asserts that a collection contains an object of a specified type.
      /// </summary>
      /// <typeparam name="T">Type of the elements in the collection.</typeparam>
      /// <param name="type">Type of object to search from.</param>
      /// <param name="toBeSearched">Collection to search in.</param>
      public static void ContainsType<T>(Type type, ICollection<T> toBeSearched) {
         foreach(T obj in toBeSearched) {
            if(   obj.GetType().Equals(type)
               || obj.GetType().IsSubclassOf(type)) {
               return;
            }
         }
         throw new TestFailed(
            String.Format("{0} contains an instance of type {1}.", toBeSearched, type.FullName),
            string.Format("No instance of type {0} found.", type.FullName));
      }
      #endregion // Assert.ContainsType

      #region Assert.DoesNotContain
      /// <summary>
      /// Assert, that a string is not contained in a search string. The search
      /// is case sensitive.
      /// </summary>
      /// <param name="searchString">The string to search for.</param>
      /// <param name="toBeSearched">The string in which to search.</param>
      public static void DoesNotContain(string searchString, string toBeSearched) {
         DoesNotContain(searchString, toBeSearched, string.Empty);
      }

      /// <summary>
      /// Assert, that a string is not contained in a search string. The search
      /// is case sensitive. This version accepts a custom message.
      /// </summary>
      /// <param name="searchString">The string to search for.</param>
      /// <param name="toBeSearched">The string in which to search.</param>
      /// <param name="customMessage">Message to display when assertion fails.</param>
      public static void DoesNotContain(string searchString, string toBeSearched,
         string customMessage) {
         Count++;
         if( toBeSearched.IndexOf(searchString) >= 0 ) {
            throw new TestFailed(
               string.Format("<{0}> does not contain <{1}>.", toBeSearched, searchString),
               string.Format("<{0}> found.", searchString),
               customMessage);
         }
      }

      /// <summary>
      /// Assert that an object of type T is not contained in a collection of 
      /// type T. The search uses T.Equals to determine equivalence.
      /// </summary>
      /// <typeparam name="T">The type of the objects contained in the 
      /// collection.</typeparam>
      /// <param name="unexpected">The object to search for.</param>
      /// <param name="toBeSearched">The collection to search in.</param>
      public static void DoesNotContain<T>(T unexpected, ICollection<T> toBeSearched) {
         DoesNotContain(unexpected, toBeSearched, string.Empty);
      }


      /// <summary>
      /// Assert that an object of type T is not contained in a collection of 
      /// type T. The search uses T.Equals to determine equivalence.
      /// </summary>
      /// <typeparam name="T">The type of the objects contained in the 
      /// collection.</typeparam>
      /// <param name="unexpected">The object to search for.</param>
      /// <param name="toBeSearched">The collection to search in.</param>
      /// <param name="customMessage">Message to display if the assertion fails.
      /// </param>
      public static void DoesNotContain<T>(T unexpected, ICollection<T> toBeSearched,
         string customMessage) {
         Count++;
         if(toBeSearched.Contains(unexpected)) {
            throw new TestFailed(
               string.Format("{0} does not contain <{1}>.", FormatCollectionTypeName(toBeSearched), unexpected),
               string.Format("<{0}> found.", unexpected),
               customMessage);
         }
      }

      #endregion // DoesNotContain

      #region Private
      /// <summary>
      /// Takes the mangled generic type name and converts it into a human
      /// readable format.
      /// </summary>
      /// <param name="obj">An instance of a generic type.</param>
      /// <returns>Human readable string of a generic type.</returns>
      /// <remarks>If the object passed in as a parameter is not an instance
      /// of a generic type, then an ArgumentException is thrown.</remarks>
      private static string FormatCollectionTypeName(object obj) {
         if(obj.GetType().IsGenericType) {
            string name = obj.GetType().Name.Split("`".ToCharArray())[0];
            Type type = obj.GetType().GetGenericArguments()[0];
            return name + "<" + type.Name + ">";
         }
         if(obj.GetType().IsArray) {
            return obj.GetType().Name;
         }
         throw new ArgumentException();
      }

      private static void NormalizeValueType(ref object expected, ref object actual) {
         NormalizeValueType(ref expected);
         NormalizeValueType(ref actual);
         if( expected is Double ) {
            actual = Convert.ToDouble(actual);
         }
         else if( actual is Double ) {
            expected = Convert.ToDouble(expected);
         }
      }

      private static void NormalizeValueType(ref object valueType) {
         if(   valueType is Decimal
            || valueType is Single ) {
            valueType = Convert.ToDouble(valueType);
         }
         else if(    valueType is Byte
            || valueType is Char
            || valueType is UInt16
            || valueType is UInt32 ) {
            valueType = Convert.ToInt64(valueType);
         }
         else if( valueType is Int16
            || valueType is Int32
            || valueType is SByte ) {
            valueType = Convert.ToInt64(valueType);
         }
      }
      #endregion

      #region Assertion Counting
      /// <summary>
      /// Controls access to the assertion count
      /// </summary>
      private static readonly object Mutex = new object();
      /// <summary>
      /// The count of invocations on this class
      /// </summary>
      private static int _count;

      /// <summary>
      /// Get/set the current assertion count
      /// </summary>
      public static int Count {
         get {
            lock(Mutex) {
               return _count;
            }
         }
         set {
            lock(Mutex) {
               _count = value;
            }
         }
      }
      #endregion
   }
}
