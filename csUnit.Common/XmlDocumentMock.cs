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

using System.Xml;

namespace csUnit.Common {
   public class XmlDocumentMock : IXmlDocument {
      public XmlDocumentMock() {
      }

      public XmlDocumentMock(string innerXml) {
         _document.InnerXml = innerXml;
      }

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

      public bool Exists(string pathName) {
         if( pathName == PathName ) {
            return true;
         }
         return false;
      }

      /// <summary>
      /// Gets/sets the raw content for the mock.
      /// </summary>
      public static string RawContent { get; set; }

      /// <summary>
      /// Resets the mock to an empty content. Typically this method is call in
      /// the SetUp or the TearDown method of a test fixture.
      /// </summary>
      public static void Reset() {
         RawContent = string.Empty;
      }

      #region IXmlDocument Members

      public void Load(string pathName) {
         _document.InnerXml = RawContent;
      }

      public XmlNodeList SelectNodes(string p) {
         return _document.SelectNodes(p);
      }

      public XmlElement CreateElement(string p) {
         return _document.CreateElement(p);
      }

      public XmlComment CreateComment(string data) {
         return _document.CreateComment(data);
      }

      public XmlDeclaration CreateXmlDeclaration(string version,
         string encoding, string standalone) {
         return _document.CreateXmlDeclaration(version, encoding, standalone);
      }

      public XmlNode AppendChild(XmlNode newChild) {
         return _document.AppendChild(newChild);
      }

      public XmlAttribute CreateAttribute(string p) {
         return _document.CreateAttribute(p);
      }

      public void Save(string pathName) {
         PathName = pathName;
         //_docContents = _document;
         RawContent = _document.InnerXml;
      }

      #endregion

      private readonly XmlDocument _document = new XmlDocument();

      public static string PathName = string.Empty;

      static XmlDocumentMock() {
         RawContent = string.Empty;
      }
   }
}
