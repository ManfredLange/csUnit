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
using System.Reflection;

namespace csUnit {
   /// <summary>
   /// The ExpectedExceptionAttribute can be used to mark a test, so that when
   /// no exception of a particular type has been thrown, the test will be
   /// reported as failed. Only if the exact exception type has been thrown, 
   /// the test will pass, unless the IsRequired parameter is set to 'false'.
   /// </summary>
   /// <remarks>Use this attribute for tests that should fail if an exception
   /// of a specific type has not been thrown. E.g. a division operation
   /// should fail if the denominator is zero. Then you would write a test
   /// with the ExpectedExceptionAttribute set, in which you would try to
   /// divide by zero. If the division operation would proceed without
   /// throwing the exception, the csUnit would detect that now exception
   /// was thrown and the test would fail.</remarks>
   /// <example>
   /// <code>
   /// [Test]
   /// [ExpectedException(typeof(System.DivideByZeroException))]
   /// public void TryDivisionByZero() {
   ///    int numerator = 5;
   ///    int denominator = 0;
   ///    int result = numerator / denominator;
   /// }
   /// </code></example>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class ExpectedExceptionAttribute : CsUnitAttribute {
      /// <summary>
      /// Creates an ExpectedExceptionAttribute object. If the argument value
      /// doesn't refer to System.Exception or a type derived from it.
      /// </summary>
      /// <param name="expectedExceptionType">Type of the expected exception.
      /// </param>
      public ExpectedExceptionAttribute(Type expectedExceptionType) {
         ValidateExceptionType(expectedExceptionType);
      }

      private void ValidateExceptionType(Type expectedExceptionType) {
         if(   !expectedExceptionType.IsSubclassOf(typeof(Exception))
               && !expectedExceptionType.Equals(typeof(Exception))) {
            throw new ArgumentException("Argument value must refer to System.Exception or a type derived from it.");
         }
         _expectedExceptionType = expectedExceptionType;
      }

      /// <summary>
      /// Creates an ExpectedExceptionAttribute object. This constructor takes
      /// a array of objects to be used as parameters to instantiate an object
      /// of the exception type. That way not only the type but exception objects
      /// can be compared.
      /// </summary>
      /// <remarks>According to the description of Exception.Message in the .NET
      /// documentation, the Message propery "should completely describe the 
      /// error". The implementation of ExpectedExceptionAttribute uses the
      /// Message property to compare the actual exception with the expected
      /// exception.
      /// </remarks>
      /// <param name="expectedExceptionType"></param>
      /// <param name="parameters"></param>
      public ExpectedExceptionAttribute(Type expectedExceptionType, params object[] parameters) {
         ValidateExceptionType(expectedExceptionType);
         Type[] types = new Type[parameters.Length];
         for(int i = 0; i < parameters.Length; i++ ) {
            types[i] = parameters[i].GetType();
         }
         ConstructorInfo ci = _expectedExceptionType.GetConstructor(types);
         if( ci != null ) {
            _exception = ci.Invoke(parameters) as Exception;
         }
         else {
            _exceptionToThrow = new TestFailed(string.Empty,
               "No constructor available for this set of " 
               + "parameters.");
         }
      }

      /// <summary>
      /// Don't use this constructor anymore. Either an exception is expected 
      /// when running a test and then it must be thrown by the test code or
      /// the test code must not throw the exception at all. Making an expected
      /// exception as "not required" is in effect filtering an exception. If
      /// this is the expected behavior then it should be expressed as a
      /// try-catch block in the test itself. The ExpectedExceptionAttribute
      /// should not be used for that case.
      /// </summary>
      /// <param name="expectedExceptionType">Type of the expected exception.
      /// </param>
      /// <param name="isRequired">true if this exception must be thrown to make
      /// the test pass</param>
      [Obsolete("An ExpectedException is always required. Please drop the second parameter. It's ignored anyways.")]
#pragma warning disable 168
      public ExpectedExceptionAttribute(Type expectedExceptionType, bool isRequired)
#pragma warning restore 168
         : this(expectedExceptionType) {
      }

      /// <summary>
      /// Invoked just before a test is executed.
      /// </summary>
      public void Before() {
         // TODO: Do we still need this method? [06mar09, ml]
         if( _exceptionToThrow != null ) {
            throw _exceptionToThrow;
         }
      }

      /// <summary>
      /// Returns a boolean value indicating whether an exception was expected 
      /// or not.
      /// </summary>
      /// <param name="thrownException">The exception that was actually thrown.</param>
      /// <returns>'true' if the exception was expected, 'false' otherwise.
      /// </returns>
      public virtual bool Expects(Exception thrownException) {
         if( _exception != null ) {
            if( _exception is TestFailed
               && thrownException is TestFailed ) {
               return _exception.Equals(thrownException as TestFailed);
            }
            return _exception.Message.Equals(thrownException.Message);
         }
         return thrownException.GetType().Equals(_expectedExceptionType);
      }

      /// <summary>
      /// Gets the system type of the expected exception.
      /// </summary>
      public Type ExceptionType {
         get {
            return _expectedExceptionType;
         }
      }

      /// <summary>
      /// Gets the full name of the exception type that is expected to be
      /// thrown.
      /// </summary>
      public virtual string ExceptionTypeFullName {
         get {
            return _expectedExceptionType.FullName;
         }
      }

      private Type  _expectedExceptionType;
      private readonly Exception _exception;
      private readonly Exception _exceptionToThrow;
   }
}
