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

using System.Xml;

using csUnit.Core.Criteria;

namespace csUnit.Core.Tests {
   [TestFixture]
   public class RegexSelectorTests {
      [SetUp]
      public void SetUp() {
         Foo._messages = string.Empty;
      }

      private class Foo {
         [Test]
         public void Cool() {
            _messages += "#Foo.Cool";
         }

         [Test]
         public void CoolStuff() {
            _messages += "#Foo.CoolStuff";
         }

         [Test]
         public void MoreCoolStuff() {
            _messages += "#Foo.MoreCoolStuff";
         }

         public static string _messages = string.Empty;
      }

      [Test]
      public void RunCoolStuffUsingRegEx() {
         var fixture = new TestFixture(typeof(Foo));
         var criterion = new RegexCriterion(@".*Cool.*");
         fixture.Execute(new TestRun(criterion), new NullListener());
         Assert.Equals("#Foo.Cool#Foo.CoolStuff#Foo.MoreCoolStuff", Foo._messages);
      }

      [Test]
      public void RunJustMoreCoolStuff() {
         var fixture = new TestFixture(typeof(Foo));
         var criterion = new RegexCriterion(@".*\.MoreCool.*");
         fixture.Execute(new TestRun(criterion), new NullListener());
         Assert.Equals("#Foo.MoreCoolStuff", Foo._messages);
      }

      [Test]
      public void RunFixture() {
         RegexSelector selector = new RegexSelector();
         selector.Pattern = @".*Foo.*";
         Assert.True(selector.Includes(new TestFixture(typeof(Foo))));
         Assert.False(selector.Includes(new TestFixture(GetType())));
      }

      private const string SerializedContent = "<regexSelector><pattern>.*Foo.*</pattern></regexSelector>";

      [Test]
      public void Serialize() {
         RegexSelector selector = new RegexSelector();
         selector.Pattern = @".*Foo.*";
         XmlElement serialized = selector.Serialize();
         Assert.Equals(SerializedContent, serialized.OwnerDocument.InnerXml);
      }

      [Test]
      public void Deserialize() {
         RegexSelector selector = new RegexSelector();
         selector.Deserialize(SerializedContent);
         Assert.Equals(@".*Foo.*", selector.Pattern);
      }

      private const string SerializedEmptyPattern = "<regexSelector />";

      [Test]
      public void SerializeWithEmptyPattern() {
         RegexSelector selector = new RegexSelector();
         XmlElement serialized = selector.Serialize();
         Assert.Equals(SerializedEmptyPattern, serialized.OwnerDocument.InnerXml);
      }

      [Test]
      public void DeserializeWithEmptyPattern() {
         RegexSelector selector = new RegexSelector();
         selector.Deserialize(SerializedEmptyPattern);
         Assert.Equals(string.Empty, selector.Pattern);
      }

      private const string SerializedSpecialCharacters = "<regexSelector><pattern>.*&lt;.*</pattern></regexSelector>";

      [Test]
      public void SerializeWithSpecialCharacters() {
         RegexSelector selector = new RegexSelector();
         selector.Pattern = ".*<.*";
         XmlElement serialized = selector.Serialize();
         Assert.Equals(SerializedSpecialCharacters, serialized.OwnerDocument.InnerXml);
      }

      [Test]
      public void DeserializeWithSpecialCharacters() {
         RegexSelector selector = new RegexSelector();
         selector.Deserialize(SerializedSpecialCharacters);
         Assert.Equals(".*<.*", selector.Pattern);
      }

      [Test]
      public void IncludesTestWithEmptyPattern() {
         var fixture = new TestFixture(typeof(Foo));
         var criterion = new RegexCriterion("");
         fixture.Execute(new TestRun(criterion), new NullListener());
         Assert.Equals("#Foo.Cool#Foo.CoolStuff#Foo.MoreCoolStuff", Foo._messages);
      }

      [Test]
      public void IncludesFixtureWithEmptyPattern() {
         RegexSelector selector = new RegexSelector();
         Assert.True(selector.Includes(new TestFixture(typeof(Foo))));
      }

      [Test]
      public void CanAssignEmptyPatternString() {
         RegexSelector selector = new RegexSelector();
         selector.Pattern = string.Empty;
         Assert.Equals(string.Empty, selector.Pattern);
      }
   }
}
