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
   public interface IXmlDocument {
      bool Exists(string pathName);

      void Load(string pathName);

      XmlNodeList SelectNodes(string p);

      XmlElement CreateElement(string p);

      /// <summary>
      /// Adds the specified node to the end of the list of child nodes, of this node.
      /// </summary>
      /// <param name="newChild">The node to add. If it is a XmlDocumentFragment, the entire contents of the document fragment are moved into the child list of this node.</param>
      /// <returns>The node added.</returns>
      XmlNode AppendChild(XmlNode newChild);

      XmlAttribute CreateAttribute(string p);

      /// <summary>
      /// Creates an XmlComment containing the specified data.
      /// </summary>
      /// <param name="data">The content of the new XmlComment.</param>
      /// <returns>The new XmlComment.</returns>
      XmlComment CreateComment(string data);

      /// <summary>
      /// Creates an XmlDeclaration node with the specified values.
      /// </summary>
      /// <param name="version">The version must be "1.0".</param>
      /// <param name="encoding">The value of the encoding attribute.</param>
      /// <param name="standalone">The value must be either "yes" or "no". If 
      /// this is a null reference (Nothing in Visual Basic) or String.Empty, 
      /// the Save method does not write a standalone attribute on the XML 
      /// declaration.</param>
      /// <returns>The new XmlDeclaration node.</returns>
      XmlDeclaration CreateXmlDeclaration(string version, string encoding,
         string standalone);

      /// <summary>
      /// Gets the first child element with the specified Name.
      /// </summary>
      /// <param name="name">The qualified name of the element to retrieve.</param>
      /// <value>The first XmlElement that matches the specified name.</value>
      XmlElement this[string name] { get; }

      void Save(string pathName);
   }
}
