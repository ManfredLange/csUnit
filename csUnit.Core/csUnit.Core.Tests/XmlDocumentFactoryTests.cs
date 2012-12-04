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
using System.Xml;
using csUnit.Common;

namespace csUnit.Core.Tests {
   [TestFixture]
   public class XmlDocumentFactoryTests {
      [SetUp]
      public void Setup() {
         XmlDocumentFactory.Type = XmlDocumentFactory.Default;
      }

      [Test]
      public void CreateInstanceOfDefaultType() {
         IXmlDocument doc = XmlDocumentFactory.CreateInstance();
         Assert.Equals(typeof(XmlDocumentFacade), doc.GetType());
      }

      private class XmlDocumentMock : IXmlDocument {
         #region IXmlDocument Members

         /// <summary>
         /// Gets the first child element with the specified Name.
         /// </summary>
         /// <param name="name">The qualified name of the element to retrieve.</param>
         /// <value>The first XmlElement that matches the specified name.</value>
         public XmlElement this[string name] {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public bool Exists(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }
  
         public void Load(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlNodeList SelectNodes(string p) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlElement CreateElement(string p) {
            throw new Exception("The method or operation is not implemented.");
         }
         
         public XmlComment CreateComment(string data) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlDeclaration CreateXmlDeclaration(string version, string encoding,
            string standalone) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlNode AppendChild(XmlNode newChild) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlAttribute CreateAttribute(string p) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Save(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [Test]
      public void CreateCustomXmlDocument() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         IXmlDocument doc = XmlDocumentFactory.CreateInstance();
         Assert.Equals(typeof(XmlDocumentMock), doc.GetType());
      }

      private class HasConstructorWithParameter : IXmlDocument {
#pragma warning disable 168
         public HasConstructorWithParameter(string param) {
#pragma warning restore 168
         }
         #region IXmlDocument Members

         /// <summary>
         /// Gets the first child element with the specified Name.
         /// </summary>
         /// <param name="name">The qualified name of the element to retrieve.</param>
         /// <value>The first XmlElement that matches the specified name.</value>
         public XmlElement this[string name] {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public bool Exists(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Load(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlNodeList SelectNodes(string p) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlElement CreateElement(string p) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlComment CreateComment(string data) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlDeclaration CreateXmlDeclaration(string version, string encoding,
            string standalone) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlNode AppendChild(XmlNode newChild) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlAttribute CreateAttribute(string p) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Save(string pathName) {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [Test]
      public void ConstructorTakingParameters() {
         XmlDocumentFactory.Type = typeof(HasConstructorWithParameter);
         IXmlDocument doc = XmlDocumentFactory.CreateInstance();
         Assert.Null(doc);
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void NonIXmlDocumentThrows() {
         XmlDocumentFactory.Type = typeof(string);
      }

      [TearDown]
      public void CleanUp() {
         XmlDocumentFactory.Type = typeof(XmlDocumentFacade);
      }
   }
}
