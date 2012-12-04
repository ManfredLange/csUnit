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

namespace csUnit.Interfaces {
   public interface ITestRun {
      /// <summary>
      /// Checks whether a test method should be executed or not. The method
      /// will be executed if the implementation of this method returns true.
      /// If it returns false, the test method will not be executed.
      /// </summary>
      /// <param name="testMethod">A test method.</param>
      /// <returns>A boolean value.</returns>
      bool Contains(ITestMethod testMethod);

      /// <summary>
      /// Checks whether the test fixture should be executed or not. Only if it
      /// is included the runtime will attempt to executed any of the test 
      /// methods defined in the fixture.
      /// </summary>
      /// <param name="testFixture">A test fixtures.</param>
      /// <returns>A boolean value indicating whether the tests in the test 
      /// fixture should be executed.</returns>
      bool Contains(ITestFixture testFixture);
   }
}
