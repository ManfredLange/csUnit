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
using System.Xml;

namespace csUnit.Interfaces {
   /// <summary>
   /// ISelector is obsolete. It's way too complicated and overengineered. It
   /// will be replaced by ITestSelection, ICriterion and its respective
   /// implementations. [24mar09, ml]
   /// </summary>
   public interface ISelector {
      /// <summary>
      /// Fired when the selector has changed it's contents, that is when the
      /// selection has changed.
      /// </summary>
      event EventHandler Modified;

      /// <summary>
      /// Called during a test run just before a testMethod is executed. If the 
      /// implementation returns 'true' the testMethod will be executed. If the 
      /// method returns 'false' the testMethod will not be executed.
      /// </summary>
      /// <param name="testMethod">An object implementing the ITestMethod 
      /// interface.</param>
      /// <returns>A boolean value indicating whether or not to execute the 
      /// testMethod.</returns>
      bool Includes(ITestMethodInfo testMethod);

      /// <summary>
      /// Called during a test run just before anything inside a test fixture is
      /// executed. If the implementation returns 'true', an attempt will be
      /// made to executed everything within the fixture. If 'false' is returned
      /// the test fixture and everything nested in it is not considered for the
      /// test run.
      /// </summary>
      /// <param name="testFixture">An object implementing the ITestFixture
      /// interface.</param>
      /// <returns>A boolean value indicating whether or not to execute anything
      /// inside the test fixture.</returns>
      bool Includes(ITestFixture testFixture);

      /// <summary>
      /// Serializes selector settings into a string.
      /// </summary>
      /// <returns>A string containing the serialized selector settings.</returns>
      /// <remarks>The returned XmlElement will be imported into the recipe.</remarks>
      XmlElement Serialize();

      /// <summary>
      /// Reads selector settings from a string.
      /// </summary>
      /// <param name="content">The string as contained in the recipe.</param>
      void Deserialize(string content);
   }
}
