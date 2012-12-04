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

namespace csUnit.Tests {
   [TestFixture]
   public class ExpectedExceptionAttributeTests {
      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void NonExceptionTypeThrowsException() {
         new ExpectedExceptionAttribute(typeof(string));
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void NonExceptionTypeThrowsException_WithParameters() {
         new ExpectedExceptionAttribute(typeof(string), new object[] {});
      }

      [Test]
      public void ReturnsCorrectExeptionFullName() {
         ExpectedExceptionAttribute attr = new ExpectedExceptionAttribute(typeof(FileNotFoundException));
         Assert.Equals("System.IO.FileNotFoundException", attr.ExceptionTypeFullName);
      }

      [Test]
      public void InstantiateWithEmptyParameterList() {
         ExpectedExceptionAttribute attr = new ExpectedExceptionAttribute(typeof(FileNotFoundException), new object[] {} );
         Assert.Equals("System.IO.FileNotFoundException", attr.ExceptionTypeFullName);
      }

      [Test]
      public void InstantiateWithParameterList() {
         ExpectedExceptionAttribute attr = new ExpectedExceptionAttribute(typeof(FileNotFoundException), "No such file.");
         FileNotFoundException exception = new FileNotFoundException("No such file.");
         Assert.True(attr.Expects(exception));
      }

      [Test]
      public void ExpectsFailsIfParameterValuesDontMatch() {
         ExpectedExceptionAttribute attr = new ExpectedExceptionAttribute(typeof(FileNotFoundException), "No such file.");
         FileNotFoundException exception = new FileNotFoundException("A totally different message.");
         Assert.False(attr.Expects(exception));
      }

      [Test]
      [ExpectedException(typeof(ApplicationException), "Whatever")]
      [Ignore("Not really testing this at the moment! [ml]")]
      public void ExpectedExceptionAttributeWithWrongParameters() {
         throw new ApplicationException("Whatever");
         //new ExpectedExceptionAttribute(typeof(FileNotFoundException), 5);
      }

      [Test]
      public void ExpectedArgumentNullException() {
         ExpectedExceptionAttribute attr = new ExpectedExceptionAttribute(typeof(ArgumentNullException), "fileName");
         Assert.True(attr.Expects(new ArgumentNullException("fileName")));
      }
   }
}
