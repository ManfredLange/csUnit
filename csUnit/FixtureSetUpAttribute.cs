////////////////////////////////////////////////////////////////////////////////
// Copyright � 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, 
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
   /// A method in a test fixture tagged with the FixtureSetUpAttribute
   /// will be called once before running all the tests in the test fixture.
   /// </summary>
   /// <remarks>If you need a set up immediately before each test, use the
   /// attribute SetUpAttribute instead.</remarks>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class FixtureSetUpAttribute : CsUnitAttribute {
   }
}