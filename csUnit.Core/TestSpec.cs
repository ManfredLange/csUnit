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

namespace csUnit.Core {
   /// <summary>
   /// A TestSpec object is a container for a test specification. By adding
   /// fixtures and/or test methods to a test spec and passing the test spec
   /// to a loader, only the tests in the test spec will be executed.
   /// </summary>
   /// <remarks>
   /// We do not want pass references to appdomains or the like across appdomain
   /// borders. One of the reasons is, that we want to be able to do a reload of
   /// an assembly without loosing the information, which tests are configured
   /// and which are not.
   /// The format for specifying a method is:
   ///    [assemblyName]#[fixtureFullName]#[MethodName]
   /// Example:
   ///    csUnitCoreTest#csUnit.Core.Test.TestSpecTests#FixtureCountWhenEmpty
   /// </remarks>
   [Serializable]
   internal class TestSpec : ITestRun {
      //public TestSpec(CmdLineHandler clh) {
      //   // TODO: This should really be in the command handler. A TestSpec
      //   // shouldn't know anything about a command line or a command line
      //   // handler. [05Sep2006, ml]
      //   string testCat = null;
      //   if(clh.HasOption("testCategory")) {
      //      testCat = clh.GetOptionValueFor("testCategory");
      //   }
      //   else if(clh.HasOption("tc")) {
      //      testCat = clh.GetOptionValueFor("tc");
      //   }
      //   string fixtureCat = null;
      //   if(clh.HasOption("fixtureCategory")) {
      //      fixtureCat = clh.GetOptionValueFor("fixtureCategory");
      //   }
      //   else if(clh.HasOption("fc")) {
      //      fixtureCat = clh.GetOptionValueFor("fc");
      //   }
      //   string pattern = null;
      //   if(clh.HasOption("pattern")) {
      //      pattern = clh.GetOptionValueFor("pattern");
      //   }
      //}

      /// <summary>
      /// Returns the number of fixtures in this specification.
      /// </summary>
      internal int FixtureCount {
         get {
            return _configuredItems.Count;
         }
      }

      public bool Contains(ITestMethod testMethod) {
         return _configuredItems.Count != 0 && IsTestConfigured(testMethod);
      }

      public bool Contains(ITestFixture testFixture) {
         return _configuredItems.Count == 0 || IsFixtureConfigured(testFixture);
      }

      private bool IsTestConfigured(ITestMethod testMethod) {
         var assemblyName = testMethod.AssemblyName;
         var fixtureFullName = testMethod.DeclaringTypeFullName;
         var methodName = testMethod.Name;

         foreach(var item in _configuredItems) {
            if(item.Matches(assemblyName, fixtureFullName, methodName)) {
               return true;
            }
         }
         return false;
      }

      private bool IsFixtureConfigured(ITestFixture testFixture) {
         return IsTestConfigured(testFixture.AssemblyName, testFixture.FullName, string.Empty);
      }

      public void AddTest(string assemblyNameOrFullName, string fixtureFullName, string methodName) {
         var assemblyName = assemblyNameOrFullName.Split(',')[0];
         var newItem = new ConfiguredItem(assemblyName, fixtureFullName, methodName);
         foreach (var item in _configuredItems) {
            if(item.Equals(newItem)) {
               return; // new item is duplicate
            }
         }
         _configuredItems.Add(newItem);
      }

      internal void AddTest(ITestMethod testMethod) {
         AddTest(testMethod.AssemblyName, testMethod.DeclaringTypeFullName, 
            testMethod.Name);
      }

      private bool IsTestConfigured(string assemblyNameOrFullName, string fixtureFullName, string methodName) {
         var assemblyName = assemblyNameOrFullName.Split(',')[0];
         foreach (var item in _configuredItems) {
            if(item.Matches(assemblyName, fixtureFullName, methodName)) {
               return true;
            }
         }
         return false;
      }

      [Serializable]
      private class ConfiguredItem {
         public ConfiguredItem(string assemblyName, string fixtureFullName, string methodName) {
            _configuredParts = GatherParts(assemblyName, fixtureFullName, methodName);
         }

         public bool Matches(string assemblyName, string fixtureFullName, string methodName) {
            var requestedParts = GatherParts(assemblyName, fixtureFullName, methodName);
            for (var i = 0; i < requestedParts.Count && i < _configuredParts.Count; i++) {
               if( requestedParts[i] != _configuredParts[i] ) {
                  return false;
               }
            }
            return true;
         }

         private static List<string> GatherParts(string assemblyName, 
            string fixtureFullName, string methodName) {
            var parts = new List<string>();
            parts.AddRange(assemblyName.Split("#.+".ToCharArray()));
            if(!fixtureFullName.Equals(string.Empty)) {
               parts.AddRange(fixtureFullName.Split("#.+".ToCharArray()));
               if(!methodName.Equals(string.Empty)) {
                  parts.AddRange(methodName.Split("#.+".ToCharArray()));
               }
            }
            return parts;
         }

         public override bool Equals(object obj) {
            var otherItem = obj as ConfiguredItem;
            if(otherItem != null) {
               if(_configuredParts.Count != otherItem._configuredParts.Count) {
                  return false;
               }
               for (var i = 0; i < _configuredParts.Count; i++) {
                  if(_configuredParts[i] != otherItem._configuredParts[i]) {
                     return false;
                  }
               }
               return true;
            }
            return base.Equals(obj);
         }

         public override int GetHashCode() {
            return _configuredParts.GetHashCode();
         }

         private readonly List<string> _configuredParts = new List<string>();
      }

      private readonly List<ConfiguredItem> _configuredItems = new List<ConfiguredItem>();
   }
}
