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
using System.Reflection;

using csUnit.Core.Adapters;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// The class TestAssembly represents an assembly containing tests.
   /// </summary>
   /// <remarks>TestAssembly serves as a facade towards Loader/RemoteLoader to
   /// isolate the cross appdomain communication from the client.
   /// </remarks>
   internal class TestAssembly : MarshalByRefObject {
      public TestAssembly(AssemblyName assemblyName) {
         _assembly = AssemblyFactory.CreateInstance(assemblyName);
         _frameworkAdapter = FrameworkAdapter.CreateInstance(assemblyName.Name);
      }
      
      private List<TestFixture> TestFixtures {
         get {
            if(_fixtures == null) {
               var fixtures = new List<TestFixture>();
               foreach(var t in _assembly.GetExportedTypes()) {
                  if(!t.IsAbstract
                     && _frameworkAdapter.IsTestFixture(t)) {
                     fixtures.Add(new TestFixture(t));
                  }
               }
               _fixtures = fixtures;
            }
            return _fixtures;
         }
      }

      public string CodeBase {
         get {
            return _assembly.CodeBase;
         }
      }

      public string FullName {
         get {
            return _assembly.FullName;
         }
      }

      public DateTime ModifiedTimeStamp {
         get {
            return _assembly.ModifiedTimeStamp;
         }
      }

      public TestFixtureInfoCollection TestFixtureInfos {
         get {
            var tfic = new TestFixtureInfoCollection();
            foreach(var tf in TestFixtures) {
               tfic.Add(new TestFixtureInfo(tf));
            }
            return tfic;
         }
      }

      public void RunTests(ITestRun testSelection, ITestListener listener) {
         SetUp();
         foreach( var tc in TestFixtures ) {
            if( testSelection.Contains(tc) ) {
               if(!tc.IsIgnored) {
                  tc.Execute(testSelection, listener);
               }
               else {
                  listener.OnTestSkipped(this, new TestResultEventArgs(
                                                  FullName, tc.FullName, string.Empty, tc.IgnoreReason, 0));
               }
            }
         }
         TearDown();
      }

      private void SetUp() {
         _assemblyFixtureInfo = TestAssemblyFixtureInfo.Scan(_assembly);
         _assemblyFixtureInfo.SetUp();
      }

      private void TearDown() {
         _assemblyFixtureInfo.TearDown();
      }

      private TestAssemblyFixtureInfo _assemblyFixtureInfo;
      private List<TestFixture> _fixtures;
      private readonly IAssembly _assembly;
      private readonly FrameworkAdapter _frameworkAdapter;
   }
}
