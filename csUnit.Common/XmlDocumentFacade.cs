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

using System.IO;
using System.Xml;

namespace csUnit.Common {
   public sealed class XmlDocumentFacade : IXmlDocument {
      #region IXmlDocument Members

      /// <summary>
      /// Gets the first child element with the specified Name.
      /// </summary>
      /// <param name="name">The qualified name of the element to retrieve.</param>
      /// <value>The first XmlElement that matches the specified name.</value>
      public XmlElement this[string name] {
         get {
            return _document[name];
         }
      }

      public XmlNode AppendChild(XmlNode newChild) {
         return _document.AppendChild(newChild);
      }

      public XmlAttribute CreateAttribute(string p) {
         return _document.CreateAttribute(p);
      }

      public XmlComment CreateComment(string data) {
         return _document.CreateComment(data);
      }

      public XmlElement CreateElement(string p) {
         return _document.CreateElement(p);
      }

      public XmlDeclaration CreateXmlDeclaration(string version, 
                                                 string encoding, string standalone) {
         return _document.CreateXmlDeclaration(version, encoding, standalone);
      }

      public bool Exists(string pathName) {
         return File.Exists(pathName);
      }

      public void Load(string pathName) {
         _document.Load(pathName);
      }

      public void Save(string pathName) {
         _document.Save(pathName);
      }

      public XmlNodeList SelectNodes(string p) {
         return _document.SelectNodes(p);
      }

      #endregion

      private readonly XmlDocument _document = new XmlDocument();
   }
}
