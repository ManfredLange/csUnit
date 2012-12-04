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
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   [TestFixture]
   public class SelectorTests {
      private class FilterMock : ISelector {
         #region ISelector Members

         /// <summary>
         /// Fired when the selector has changed it's contents, that is when the
         /// selection has changed.
         /// </summary>
#pragma warning disable 67
         public event EventHandler Modified;
#pragma warning restore 67

         public bool Includes(ITestMethodInfo testMethod) {
            return true;
         }

         public bool Includes(ITestFixture testFixture) {
            throw new Exception("The method or operation is not implemented.");
         }

         public XmlElement Serialize() {
            var doc = new XmlDocument();
            var element = doc.CreateElement("filterMock");

            doc.AppendChild(element);

            _serializedContent = doc.InnerXml;
            return element;
         }

         public void Deserialize(string content) {
            _deserializedContent = content;
         }

         #endregion // ISelector Members
         public string _serializedContent = string.Empty;
         public string _deserializedContent = string.Empty;
      }

      [/*SetUp,*/ TearDown] // TODO: should work but doesn't. Picks up only TearDown but not SetUp. [26nov2011, ml]
      public void TearDown() {
         XmlDocumentMock.Reset();
         RecipeFactory.Type = RecipeFactory.Default;
         LoaderFactory.Type = LoaderFactory.Default;
      }

      [SetUp]
      public void SetUp() {
         XmlDocumentMock.Reset();
         RecipeFactory.Type = RecipeFactory.Default;
         LoaderFactory.Type = LoaderFactory.Default;
      }

      [Test]
      public void StoresInRecipe() {
         XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         LoaderFactory.Type = typeof(LoaderMock);
         var recipe = RecipeFactory.NewRecipe(string.Empty);
         var filter = new FilterMock();
         recipe.RegisterSelector(filter);
         recipe.Save(@"c:\nirvana.recipe");
         var retrievedRecipe = RecipeFactory.NewRecipe(string.Empty);
         var xmlDocument = new XmlDocumentMock(XmlDocumentMock.RawContent);
         retrievedRecipe.LoadFromXml(xmlDocument);
         Assert.Contains(filter, retrievedRecipe.Selectors);
         Assert.Equals("<filterMock />", filter._serializedContent);
         Assert.Equals(filter._serializedContent, filter._deserializedContent);
      }
   }
}
