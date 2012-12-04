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
using System.IO;

namespace csUnit.CommandLine.Tests {
   /// <summary>
   /// Base class for other fixtures containing tests for csUnitCmd.
   /// </summary>
   /// <remarks>This base class does not contain any tests itself. If you do add
   /// a test here you may need to improve how csUnit handles tests inherited
   /// from base classes. Otherwise they may appear in multiple places, which
   /// may or may not be intended!</remarks>
   [TestFixture]
   public class CsUnitCmdTests {
      [FixtureSetUp]
      public void FixtureSetUp() {
         _savedConsole = Console.Out;
      }

      [SetUp]
      public virtual void SetUp() {
         _console = new StringWriter();
         Console.SetOut(_console);
      }

      [FixtureTearDown]
      public virtual void FixtureTearDown() {
         Console.SetOut(_savedConsole);
      }

      protected string Output {
         get {
            return _console.GetStringBuilder().ToString();
         }
      }

      private TextWriter _savedConsole;
      private StringWriter _console = new StringWriter();
   }
}
