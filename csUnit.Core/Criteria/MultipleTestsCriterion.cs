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
using System.Collections.Generic;

using csUnit.Interfaces;
using csUnit.Interfaces.Criteria;

namespace csUnit.Core.Criteria {
   [Serializable]
   public class MultipleTestsCriterion : ICriterion {
      public void Add(string assemblyNameOrFullName, string fixtureFullName, string methodName) {
         var assemblyName = assemblyNameOrFullName.Split(',')[0];
         var newItem = new Element(assemblyName, fixtureFullName, methodName);
         foreach (var element in _elements) {
            if (element.Equals(newItem)) {
               return; // new item is duplicate
            }
         }
         _elements.Add(newItem);
      }

      public bool IsEmpty {
         get {
            return _elements.Count == 0;
         }
      }

      #region Implementation of ICriterion

      public bool Contains(ITestMethod testMethod) {
         var assemblyName = testMethod.AssemblyName;
         var fixtureFullName = testMethod.DeclaringTypeFullName;
         var methodName = testMethod.Name;

         foreach (var element in _elements) {
            if (element.Matches(assemblyName, fixtureFullName, methodName)) {
               return true;
            }
         }
         return false;
      }

      public bool Contains(ITestFixture testFixture) {
         var assemblyName = testFixture.AssemblyName;
         var fixtureFullName = testFixture.FullName;

         foreach (var element in _elements) {
            if (element.Matches(assemblyName, fixtureFullName, string.Empty)) {
               return true;
            }
         }
         return false;
      }

      #endregion

      [Serializable]
      private class Element {
         public Element(string assemblyName, string fixtureFullName, string methodName) {
            _elementParts = GatherParts(assemblyName, fixtureFullName, methodName);
         }

         public bool Matches(string assemblyName, string fixtureFullName, string methodName) {
            var requestedParts = GatherParts(assemblyName, fixtureFullName, methodName);
            for (var i = 0; i < requestedParts.Count && i < _elementParts.Count; i++) {
               if (requestedParts[i] != _elementParts[i]) {
                  return false;
               }
            }
            return true;
         }

         private static List<string> GatherParts(string assemblyName,
            string fixtureFullName, string methodName) {
            var parts = new List<string>();
            parts.AddRange(assemblyName.Split("#.+".ToCharArray()));
            if (!fixtureFullName.Equals(string.Empty)) {
               parts.AddRange(fixtureFullName.Split("#.+".ToCharArray()));
               if (!methodName.Equals(string.Empty)) {
                  parts.AddRange(methodName.Split("#.+".ToCharArray()));
               }
            }
            return parts;
         }

         public override bool Equals(object obj) {
            var otherItem = obj as Element;
            if (otherItem != null) {
               if (_elementParts.Count != otherItem._elementParts.Count) {
                  return false;
               }
               for (var i = 0; i < _elementParts.Count; i++) {
                  if (_elementParts[i] != otherItem._elementParts[i]) {
                     return false;
                  }
               }
               return true;
            }
            return base.Equals(obj);
         }

         public override int GetHashCode() {
            return _elementParts.GetHashCode();
         }

         private readonly List<string> _elementParts = new List<string>();
      }

      private readonly List<Element> _elements = new List<Element>();
   }
}
