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

namespace csUnit.Core.Tests {
   [TestFixture]
   public class SimpleTestListenerTests {
      [Test]
      public void ConstructorResetsStatics() {
         new SimpleTestListener();
         Assert.Equals(0, SimpleTestListener.ErrorItems.Count);
         Assert.Equals(0, SimpleTestListener.FailureItems.Count);
         Assert.Equals(0, SimpleTestListener.IgnoredItems.Length);
         Assert.Equals(string.Empty, SimpleTestListener.Messages);
         Assert.Equals(0, SimpleTestListener.PassedItems.Count);
      }
   }
}
