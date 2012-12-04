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
   /// Summary description for TestFixtureInfo.
   /// </summary>
   [Serializable]
   public class TestFixtureInfo : ITestFixtureInfo {
      internal TestFixtureInfo(TestFixture tf) {
         // Don't store a reference to the TestFixture object in order to ensure
         // the test assembly can be unloaded. [11Feb07, ml]
         // No longer sure about this. [16aug09, ml]
         _fullName = tf.FullName;
         _testMethods = tf.TestMethods;
         _categories = tf.Categories;
      }

      public string FullName {
         get {
            return _fullName;
         }
      }

      internal Categories Categories {
         get {
            return _categories;
         }
      }

      public List<ITestMethodInfo> TestMethods {
         get {
            return _testMethods;
         }
      }

      private readonly string             _fullName;
      private readonly List<ITestMethodInfo>   _testMethods;
      private readonly Categories _categories;
   }
}
