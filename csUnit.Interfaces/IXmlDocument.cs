////////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2007 by Manfred Lange, Markus Renschler, Jake Anderson, 
//                          and Piers Lawson. All rights reserved.
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
//    product, the following acknowledgement must be included in the product 
//    documentation:
// 
//       Portions Copyright © 2002-2007 by Manfred Lange, Markus Renschler, 
//       Jake Anderson, and Piers Lawson. All rights reserved.
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace csUnit.Interfaces {
   public interface IXmlDocument {
      void Load(string pathName);

      System.Xml.XmlNodeList SelectNodes(string p);

      System.Xml.XmlElement CreateElement(string p);

      /// <summary>
      /// Adds the specified node to the end of the list of child nodes, of this node.
      /// </summary>
      /// <param name="newChild">The node to add. If it is a XmlDocumentFragment, the entire contents of the document fragment are moved into the child list of this node.</param>
      /// <returns>The node added.</returns>
      XmlNode AppendChild(System.Xml.XmlNode newChild);

      System.Xml.XmlAttribute CreateAttribute(string p);

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

      void Save(string pathName);
   }
}
