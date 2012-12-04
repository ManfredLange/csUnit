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

using System;
using System.Xml;
using csUnit.Interfaces;

namespace csUnit.Core {
   public class CategorySelector : MarshalByRefObject, ISelector {
      public event EventHandler Modified;

      public CategorySelector() {
         _categoriesModifiedHandler = OnCategoriesModified;
         _includedCategories.Modified += _categoriesModifiedHandler;
         _excludedCategories.Modified += _categoriesModifiedHandler;
      }

      private void OnCategoriesModified(object sender, EventArgs e) {
         if(null != Modified) {
            Modified(this, new EventArgs());
         }
      }

      public Categories IncludedCategories {
         get {
            return _includedCategories;
         }
      }

      public Categories ExcludedCategories {
         get {
            return _excludedCategories;
         }
      }

      #region ISelector Members

      public bool Includes(ITestMethodInfo testMethod) {
         return IsIncluded(testMethod) && !Excluded(testMethod);
      }

      private bool IsIncluded(ITestMethodInfo testMethod) {
         if(_includedCategories.IsEmpty) {
            return true;
         }
         var local = new Categories();
         local.Add(testMethod.Categories);
         local.Add(testMethod.InheritedCategories);
         return local.Intersect(_includedCategories).Count > 0;
      }

      private bool Excluded(ITestMethodInfo testMethod) {
         if(_excludedCategories.IsEmpty) {
            return false;
         }
         var local = new Categories();
         local.Add(testMethod.Categories);
         local.Add(testMethod.InheritedCategories);
         return local.Intersect(_excludedCategories).Count > 0;
      }

      public bool Includes(ITestFixture testFixture) {
         return IsIncluded(testFixture) && !Excluded(testFixture);
      }

      private bool IsIncluded(ITestFixture testFixture) {
         if(   _includedCategories.IsEmpty
            || (testFixture.Categories.IsEmpty
            && testFixture.InheritedCategories.IsEmpty) ) {
            return true;
         }
         var local = new Categories();
         local.Add(testFixture.Categories);
         local.Add(testFixture.InheritedCategories);
         return local.Intersect(_includedCategories).Count > 0;
      }

      private bool Excluded(ITestFixture testFixture) {
         if(_excludedCategories.IsEmpty) {
            return false;
         }
         var local = new Categories();
         local.Add(testFixture.Categories);
         local.Add(testFixture.InheritedCategories);
         return local.Intersect(_excludedCategories).Count > 0;
      }

      // Example of how category selector settings are stored:
      //
      // <categorySelector>
      //    <includedCategories>
      //       <category>Alpha</category>
      //       <category>CORE</category>
      //    </includedCategories>
      //    <excludedCategories>
      //       <category>Green</category>
      //       <category>Blue</category>
      //    </excludedCategories>
      // </categorySelector>
      //
      // [24Mar07, ml]

      public XmlElement Serialize() {
         var doc = new XmlDocument();
         
         var root = doc.CreateElement("categorySelector");
         doc.AppendChild(root);

         SerializeCategories(root, "includedCategories", _includedCategories);
         SerializeCategories(root, "excludedCategories", _excludedCategories);
         
         return root;
      }

      private static void SerializeCategories(XmlNode root, string categoriesGroupName,
         Categories categoriesCollection) {
         if(categoriesCollection.Count > 0) {
            var categoriesElement = root.OwnerDocument.CreateElement(categoriesGroupName);
            root.AppendChild(categoriesElement);
            foreach(var category in categoriesCollection) {
               var elem = root.OwnerDocument.CreateElement("category");
               elem.InnerText = category;
               categoriesElement.AppendChild(elem);
            }
         }
      }

      public void Deserialize(string content) {
         var doc = new XmlDocument();
         doc.LoadXml(content);

         DeserializeCategories(doc, "includedCategories", _includedCategories);

         var excludedCatNodes = doc.SelectNodes("categorySelector/excludedCategories/category");
         foreach(XmlElement elem in excludedCatNodes) {
            _excludedCategories.Add(elem.InnerText);
         }
      }

      private static void DeserializeCategories(XmlNode doc, string categoriesGroupName,
         Categories categoriesCollection) {
         var includedCatNodes = doc.SelectNodes("categorySelector/" + categoriesGroupName + "/category");
         foreach(XmlElement elem in includedCatNodes) {
            categoriesCollection.Add(elem.InnerText);
         }
      }

      #endregion

      private readonly Categories _includedCategories = new Categories();
      private readonly Categories _excludedCategories = new Categories();
      private readonly EventHandler _categoriesModifiedHandler;
   }
}
