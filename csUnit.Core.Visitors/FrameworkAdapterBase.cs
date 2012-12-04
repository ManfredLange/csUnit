﻿#region Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, and Piers Lawson. All rights reserved.
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
using System.IO;

namespace csUnit.Core.Visitors {
   public abstract class FrameworkAdapterBase : MarshalByRefObject {
      // We need generic classes for
      // - test assembly (probably identical with the AssemblyLoader?)
      // - test fixture
      // - test method
      // - ignore attribute?
      // - expected exception
      // - ...
      //
      // as a consequence csUnit.Core will only deal with a generic framework.
      // This is a similar approach to NHibernate, which uses it to level the differences
      // in SQL dialects.
      
      public abstract int AssertionCount { get; }

      //public void ExecuteTests(ITestRun testRun, TextWriter console,
      //                         ITestListener listener) {
      //   listener.AssemblyStarted();
      //   foreach(var fixture in Fixtures) {
      //      fixture.FixtureSetUp();
      //      foreach(var method in fixture.Methods) {
      //         fixture.Execute(method);
      //      }
      //      fixture.FixtureTearDown();
      //   }
      //   listener.AssemblyCompleted();
      //}
   }
}
