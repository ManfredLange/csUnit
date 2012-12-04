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
using System.Collections.Generic;
using csUnit.Interfaces;

namespace csUnit.Core {
   internal enum MethodType {
      FixtureSetUpMethod,
      FixtureTearDownMethod,
      SetUpMethod,
      TearDownMethod,
      TestMethod
   }

   // TODO: What is the difference of TestMethod, MethodInfo and MethodEntry? [17mar09, ml]
   // MethodInfo is the reflection information about a method.
   // TestMethod ... This is a wrapper representing a method containing a test.
   // MethodEntry ... This is a wrapper representing a test method/setup/teardown/fixture setup/fixture teardown
   // TODO: Attempt to merge TestMethod and MethodEntry, or derive one from the other and find better names. [18mar09, ml]
   internal class MethodEntry : MarshalByRefObject, ITestMethod {
      public MethodEntry(ITestMethod method, MethodType methodType) {
         _method = method;
         MethodType = methodType;
      }

      public MethodType MethodType { get; private set; }

      #region ITestMethod Members

      public void Execute(ITestListener listener) {
         _method.Execute(listener);
      }

      public bool Ignore {
         get { return _method.Ignore; }
      }

      public string IgnoreReason {
         get { return _method.IgnoreReason; }
      }

      public string AssemblyName {
         get {
            return _method.AssemblyName;
         }
      }

      public string AttributeName {
         get {
            return _method.AttributeName;
         }
      }

      public string DeclaringTypeFullName {
         get {
            return _method.DeclaringTypeFullName;
         }
      }

      public string FullName {
         get {
            return _method.FullName;
         }
      }

      public string Name {
         get {
            return _method.Name;
         }
      }

      public Categories Categories {
         get {
            return _method.Categories;
         }
      }

      public Categories InheritedCategories {
         get {
            return _method.InheritedCategories;
         }
      }

      public void Invoke(object obj) {
         Invoke(obj, new object[] {});
      }

      public void Invoke(object obj, object[] args) {
         if(   Duplicates.Count > 0
               && Listener != null) {
            var message = "Invalid configuration: More than one applicable " + 
                             _method.AttributeName + " method: {" + FullName;
            foreach(var testMethod in Duplicates) {
               message += ", " + testMethod.FullName;
            }
            message += "}";
            Assert.Fail(message);
         }
         _method.Invoke(obj, args);
      }

      #endregion // ITestMethod Members

      private readonly ITestMethod _method;
      public readonly List<ITestMethod> Duplicates = new List<ITestMethod>();
      public ITestListener Listener;
   }
}
