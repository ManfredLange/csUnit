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

using System.IO;

namespace csUnit.Common.Tests {
   [TestFixture]
   public class XmlDocumentMockTests {
      [Test]
      [ExpectedException(typeof(FileNotFoundException))]
      public void InvalidContentThrowsOnLoad() {
         // The idea is that we don't want the parsing happen when we set the
         // content but at the same time as it would happen if we would read
         // from the file.
         XmlDocumentMock.RawContent = "bla"; // Must not throw!
         IXmlDocument doc = XmlDocumentFactory.CreateInstance();
         doc.Load("whatever.xml");
      }
   }
}
