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

#if DEBUG

using System;
using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   /// <summary>
   /// Runs tests to verify the workings of the Ignored attribute.
   /// </summary>
   [TestFixture(Categories="CORE")]
   public class IgnoreTests {
      //----------------------------------------------
      internal class MyListener : NullListener {
         public override void OnTestSkipped(object sender, TestResultEventArgs args) {
            _skipCount++;
         }
         public override void OnTestStarted(object sender, TestResultEventArgs args) {
            _testCount++;
         }
         public int SkipCount {
            get { return(_skipCount); }
         }
         public int TestCount {
            get { return(_testCount); }
         }
         private int _skipCount = 0;
         private int _testCount = 0;
      }

      //----------------------------------------------
      [TestFixture]
         internal class HasIgnoredMethod {
         [Test, Ignore("Intentionally ignored")]
         public void IsIgnored() {
            throw new Exception("Should not be thrown.");
         }
         [Test]
         public void NotIgnored() {
         }
      }

      [Test]
      public void IgnoredMethod() {
         MyListener myListener = new MyListener();
         TestFixture tc = new TestFixture(typeof(HasIgnoredMethod));
         TestSpec ts = new TestSpec();
         ts.AddTest("csUnit.Core.Tests", typeof(HasIgnoredMethod).FullName, string.Empty);
         tc.Execute(ts, myListener);
         Assert.Equals(1, myListener.SkipCount);
         // Skipped tests do not 'start'
         Assert.Equals(1, myListener.TestCount);
      }
   }
}

#endif
