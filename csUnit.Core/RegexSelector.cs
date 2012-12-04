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
using System.Text.RegularExpressions;
using System.Xml;
using csUnit.Interfaces;

namespace csUnit.Core {
   public class RegexSelector : MarshalByRefObject, ISelector {
      // Gets/sets the regular expression pattern for the selector.
      public string Pattern {
         internal get {
            return null != _regex ? _regex.ToString() : string.Empty;
         }
         set {
            _regex = new Regex(value);
            if(Modified != null) {
               Modified(this, new EventArgs());
            }
         }
      }

      #region ISelector Members

      public event EventHandler Modified;

      public bool Includes(ITestMethodInfo testMethod) {
         return Matches(testMethod.FullName);
      }

      public bool Includes(ITestFixture testFixture) {
         return Matches(testFixture.FullName);
      }

      private bool Matches(string theString) {
         if(null != _regex) {
            var match = _regex.Match(theString);
            return match.Success;
         }
         return true;
      }

      public XmlElement Serialize() {
         XmlDocument doc = new XmlDocument();

         XmlElement root = doc.CreateElement("regexSelector");
         doc.AppendChild(root);
         
         if( _regex != null ) {
            XmlElement patternElement = doc.CreateElement("pattern");
            patternElement.InnerText = _regex.ToString();
            root.AppendChild(patternElement);
         }

         return root;
      }

      public void Deserialize(string content) {
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(content);

         XmlNodeList patternNodes = doc.SelectNodes("regexSelector/pattern");
         if(patternNodes.Count == 1) {
            XmlElement patternElement = patternNodes[0] as XmlElement;
            if(patternElement != null) {
               _regex = new Regex(patternElement.InnerText);
            }
         }
      }

      #endregion
      private Regex _regex;
   }
}
