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

namespace csUnit.Core.Tests {
   /// <summary>
   /// Summary description for CmdLineHandlerTests.
   /// </summary>
   [TestFixture]
   public class CmdLineHandlerTests {
      [SetUp]
      public void CreateCmdLineHandler() {
         _clh = new CmdLineHandler();
         _clh.AcceptOption("assembly", true);
         _clh.AcceptOption("recipe", true);
         _clh.AcceptOption("autorun", false);
         _clh.AcceptOption("xml", true);
         _clh.AcceptOption("optionalValue", false);
         _clh.AcceptOption("?", false);
      }

      [TearDown]
      public void Cleanup() {
         _clh = null;
      }

      [Test]
      public void TestAssemblyOption() {
         _clh.Evaluate(new string[]{"/assembly:mock.dll"});

         Assert.Equals("mock.dll", _clh.GetOptionValueFor("assembly"));
         Assert.True(_clh.IsValid());
      }

      [Test]
      public void RejectsAssemblyWithoutFileName() {
         _clh.Evaluate(new string[]{"/assembly"});

         Assert.False(_clh.IsValid());
      }

      [Test]
      public void RejectsNonsense() {
         _clh.Evaluate(new string[]{"/nonsense"});

         Assert.False(_clh.IsValid());
      }

      [Test]
      public void AcceptsAutorun() {
         _clh.Evaluate(new string[]{"/autorun"});

         Assert.True(_clh.IsValid());
      }

      [Test]
      public void ValidButNotSpecifiedReturnsEmpty() {
         _clh.Evaluate(new string[0]);

         Assert.Equals(null, _clh.GetOptionValueFor("assembly"));
         Assert.Equals(0, _clh.Count);
      }

      [Test]
      public void AcceptsHelp() {
         _clh.Evaluate(new string[]{"/?"});

         Assert.Equals(string.Empty, _clh.GetOptionValueFor("?"));
         Assert.Equals(1, _clh.Count);
      }

      [Test]
      public void CountOnMultipleOptions() {
         _clh.Evaluate(new string[]{"/assembly:mock.dll", "/xml:results.xml"});
         
         Assert.Equals(2, _clh.Count);
         Assert.True(_clh.IsValid());
      }

      [Test]
      public void ColonTreatedAsMissingValue() {
         _clh.Evaluate(new string[] { "/recipe:" });
         Assert.False(_clh.IsValid());
      }

      [Test]
      public void EmptyStringReturnedIfNoOptionParameter() {
         _clh.Evaluate(new string[] { "/optionalValue", "/recipe:bla.recipe"});
         Assert.True(_clh.IsValid());
         Assert.Equals(string.Empty, _clh.GetOptionValueFor("optionalValue"));
      }

      [Test]
      public void AcceptPathWithSpaceAsEnclosedParameter() {
         _clh.Evaluate(new string[] { "/xml:a path with spaces\\deleteme.xml" });
         Assert.True(_clh.IsValid());
         Assert.Equals("a path with spaces\\deleteme.xml", _clh.GetOptionValueFor("xml"));
      }

      [Test]
      public void NullReturnedForNonExistingOption() {
         _clh.Evaluate(new string[] { });
         Assert.Null(_clh.GetOptionValueFor("whatever"));
      }

      private CmdLineHandler _clh = null;
   }
}
