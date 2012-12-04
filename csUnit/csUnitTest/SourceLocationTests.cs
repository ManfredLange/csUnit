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

namespace csUnit.Tests {
   [TestFixture]
   public class SourceLocationTests {
      [Test]
      public void DefaultValues() {
         SourceLocation loc = new SourceLocation(string.Empty, 0);
         Assert.Equals(string.Empty, loc.FileName);
         Assert.Equals(0, loc.LineNumber);
      }

      [Test]
      [ExpectedException(typeof(ArgumentNullException), "fileName")]
      public void NullAsFileNameThrows() {
         new SourceLocation(null, 0);
      }

      [Test]
      [ExpectedException(typeof(ArgumentOutOfRangeException), "lineNumber", "Line number must be greater than 0.")]
      public void NegativeLineNumberThrows() {
         new SourceLocation("foo.cs", -5);
      }

      [Test]
      public void GetFileName() {
         SourceLocation loc = new SourceLocation("foo.cs", 0);
         Assert.Equals("foo.cs", loc.FileName);
      }

      [Test]
      public void GetLineNumber() {
         SourceLocation loc = new SourceLocation("foo.cs", 47);
         Assert.Equals(47, loc.LineNumber);
      }

      [Test]
      public void Equals() {
         SourceLocation loc1 = new SourceLocation("bar.cs", 58);
         SourceLocation loc2 = new SourceLocation("bar.cs", 58);
         Assert.Equals(loc1, loc2);
      }

      [Test]
      public void EqualsWithDifferentLineNumber() {
         SourceLocation loc1 = new SourceLocation("bar.cs", 58);
         SourceLocation loc2 = new SourceLocation("bar.cs", 77);
         Assert.NotEquals(loc1, loc2);
      }

      [Test]
      public void EqualsWithDifferentFileName() {
         SourceLocation loc1 = new SourceLocation("bar.cs", 58);
         SourceLocation loc2 = new SourceLocation("for.cs", 58);
         Assert.NotEquals(loc1, loc2);
      }

      [Test]
      public void EqualsWithNull() {
         SourceLocation loc = new SourceLocation("bar.cs", 47);
         Assert.False(loc.Equals(null));
      }

      [Test]
      public new void ToString() {
         SourceLocation loc = new SourceLocation("bar.cs", 47);
         Assert.Equals("bar.cs, line 47", loc.ToString());
      }
   }
}
