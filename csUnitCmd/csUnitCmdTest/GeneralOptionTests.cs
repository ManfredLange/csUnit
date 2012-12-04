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
using System.Collections.Generic;
using System.IO;
using System.Text;

using csUnit;
using csUnit.CommandLine;
using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.CommandLine.Tests {
   [TestFixture]
   public class GeneralOptionTests : CsUnitCmdTests {
      [FixtureTearDown]
      public override void FixtureTearDown() {
         base.FixtureTearDown();
         RecipeFactory.Type = RecipeFactory.Default;
      }

      [Test]
      public void NoOptionDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { });
         Assert.Equals(0, result);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void SlashQuestionMarkDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { "/?" });
         Assert.Equals(0, result);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void MinusQuestionMarkDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { "-?" });
         Assert.Equals(0, result);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void SlashHelpDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { "/help" });
         Assert.Equals(0, result);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void MinusHelpDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { "-help" });
         Assert.Equals(0, result);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void UnknownOptionDisplaysUsage() {
         int result = CsUnitApp.Main(new string[] { "/foo" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Invalid option 'foo'.", Output);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }
   }
}
