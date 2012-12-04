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

using System;

namespace csUnit {
   /// <summary>
   /// The IgnoreAttribute can be used to mark a test fixture or a test. When
   /// marked this way, the test or the entire test fixture will not be excuted
   /// when the tests are run.
   /// </summary>
   /// <remarks>The advantage compared to commenting out tests is that ignored
   /// tests will still be visible in the csUnitRunner interface. This way they
   /// will not be forgotten as could be the case when code is commented out.
   /// However, they will not be displayed as passed or failed. Therefore they 
   /// do not influence the test result.</remarks>
   [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = false)]
   public class IgnoreAttribute : CsUnitAttribute {
      /// <summary>
      /// Constructs an IgnoreAttribute object. When set on a test, the test
      /// will be ignored by csUnit during test execution.
      /// </summary>
      /// <param name="reason">Why the test or the test fixture should be
      /// ignored.</param>
      public IgnoreAttribute(string reason) {
         _reason = reason;
      }

      /// <summary>
      /// Gets the reason why a particular test was ignored.
      /// </summary>
      public string Reason {
         get {
            return _reason;
         }
      }

      private readonly string _reason = string.Empty;
   }
}
