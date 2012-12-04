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

using System.Text;

namespace csUnit.Core {
   /// <summary>
   /// TestSuite is a collection of test fixtures within an assembly. In the
   /// current implementation a test suite is equal to a namespace in an
   /// assembly. Namespaces can be nested; therefore TestSuites can be nested.
   /// Currently the test suite does not implement any members. Instead its
   /// type-object is used for some client side functionality. [ml]
   /// </summary>
   public class TestSuite {
      public TestSuite(string[] classFullNameParts, int index) {
         var sb = new StringBuilder();
         for(int i = 0; i <= index; i++ ) {
            sb.Append(classFullNameParts[i]);
            if( i < index ) {
                sb.Append('.');
            }
         }
         _suiteFullName = sb.ToString();
     }

      /// <summary>
      /// Gets the full name of the suite.
      /// </summary>
      public string FullName {
         get {
            return _suiteFullName;
         }
      }

      private readonly string _suiteFullName = string.Empty;
   }
}
