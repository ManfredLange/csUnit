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

using csUnit.Core;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// TreeNodeInfo objects carry the information associated with a node in the
   /// test hierarchy tree. E.g. they have a member for identifying the kind of
   /// object associated with the node. Optionally they can carry an object
   /// reference to an object with the information displayed in the node.
   /// </summary>
   /// <remarks>The class takes TestAssembly, TestClass, and TestMethod as
   /// parameters. However, no reference on them is stored in order to avoid
   /// keeping a reference to any of them. The background is to avoid having
   /// references across appdomains as any tests are instantiated in a
   /// separate appdomain. Unloading an appdomain might be impossible because
   /// of outstanding references.</remarks>
   public class UiElementInfo {
      public UiElementInfo(ITestAssembly ta, ITestFixtureInfo tf, ITestMethodInfo tm) {
         if (ta != null) {
            _assemblyName = ta.Name.FullName.Split(",".ToCharArray())[0];
            _assemblyPathName = ta.Name.CodeBase;
         }
         _fixtureFullName = string.Empty;
         _methodName = string.Empty;
         if( tf != null ) {
            _fixtureFullName = tf.FullName;
            if( tm != null ) {
               _methodName = tm.Name;
               _methodFullName = tm.FullName;
            }
         }
      }

      internal UiElementInfo(string assemblyName, string fixtureFullName, string methodName) {
         _assemblyName = assemblyName;
         _fixtureFullName = fixtureFullName;
         _methodName = methodName;
      }

      public UiElementInfo(ITestAssembly ta, TestSuite ts) {
         _assemblyName = ta.Name.FullName.Split(",".ToCharArray())[0];
         _assemblyPathName = ta.Name.CodeBase;
         _fixtureFullName = ts.FullName;
         _methodName = string.Empty;
      }

      public string AssemblyName {
         get {
            return _assemblyName;
         }
      }

      public string AssemblyPathName {
         get {
            return _assemblyPathName;
         }
      }

      public string FixtureName {
         get {
            return _fixtureFullName;
         }
      }

      public string MethodName {
         get {
            return _methodName;
         }
      }

      public string MethodFullName {
         get {
            return _methodFullName;
         }
      }

      public bool IsRecipeItem {
         get {
            return false;
         }
      }

      public bool IsAssemblyItem {
         get {
            return _fixtureFullName == string.Empty
                   && _methodName == string.Empty;
         }
      }

      public bool IsFixtureItem {
         get {
            return _fixtureFullName != string.Empty
                   && _methodName == string.Empty;
         }
      }

      public bool IsMethodItem {
         get {
            return _methodName != string.Empty;
         }
      }

   
      public override int GetHashCode() {
         return (_assemblyName + _fixtureFullName + _methodName
                 + _assemblyPathName + _methodFullName).GetHashCode();
      }

      public override bool Equals(object obj) {
         var info = obj as UiElementInfo;
         if( info != null ) {
            if(_assemblyName == info._assemblyName
               && _fixtureFullName == info._fixtureFullName
               && _methodName == info._methodName) {
               return true;
            }
         }
         return false;
      }
      
      private readonly string _assemblyName     = string.Empty;
      private readonly string _fixtureFullName  = string.Empty;
      private readonly string _methodName       = string.Empty;

      private readonly string _assemblyPathName = string.Empty;
      private readonly string _methodFullName   = string.Empty;
   }
}
