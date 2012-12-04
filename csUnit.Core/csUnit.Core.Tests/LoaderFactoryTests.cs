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
using System.IO;
using System.Reflection;

using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   [TestFixture]
   public class LoaderFactoryTests {
      [Test]
      public void CreatesDefault() {
         var loader = LoaderFactory.CreateInstance(GetType().Assembly.Location);
         Assert.Equals(typeof(Loader), loader.GetType());
      }

      private class CustomLoader : ITestAssembly {
         public CustomLoader(string pathName) {
         }
         #region ITestAssembly Members
#pragma warning disable 67
         public event AssemblyEventHandler AssemblyChanged;

         public event AssemblyEventHandler AssemblyLoaded;

         public event AssemblyEventHandler AssemblyStarted;

         public event AssemblyEventHandler AssemblyFinished;

         public event AssemblyEventHandler TestsAborted;

         public event TestEventHandler TestStarted;

         public event TestEventHandler TestPassed;

         public event TestEventHandler TestFailed;

         public event TestEventHandler TestError;

         public event TestEventHandler TestSkipped;
#pragma warning restore 67
         public AssemblyName Name {
            get { throw new NotImplementedException(); }
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         public TestFixtureInfoCollection TestFixtureInfos {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public void Refresh() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RunTests(ITestRun testRun) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Abort() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void SetConsoleOutputTo(TextWriter value) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [Test]
      public void CreatesCustomLoader() {
         LoaderFactory.Type = typeof(CustomLoader);
         var loader = LoaderFactory.CreateInstance(GetType().Assembly.Location);
         Assert.Equals(typeof(CustomLoader), loader.GetType());
      }

      private class LoaderMissingConstructor : ITestAssembly {
         #region ITestAssembly Members
#pragma warning disable 67
         public event AssemblyEventHandler AssemblyChanged;

         public event AssemblyEventHandler AssemblyLoaded;

         public event AssemblyEventHandler AssemblyStarted;

         public event AssemblyEventHandler AssemblyFinished;

         public event AssemblyEventHandler TestsAborted;

         public event TestEventHandler TestStarted;

         public event TestEventHandler TestPassed;

         public event TestEventHandler TestFailed;

         public event TestEventHandler TestError;

         public event TestEventHandler TestSkipped;
#pragma warning restore 67
         public AssemblyName Name {
            get { throw new NotImplementedException(); }
         }

         public DateTime ModifiedTimeStamp {
            get {
               return DateTime.Now;
            }
         }

         public TestFixtureInfoCollection TestFixtureInfos {
            get {
               throw new Exception("The method or operation is not implemented.");
            }
         }

         public void Refresh() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void RunTests(ITestRun testRun) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Abort() {
            throw new Exception("The method or operation is not implemented.");
         }

         public void SetConsoleOutputTo(TextWriter value) {
            throw new Exception("The method or operation is not implemented.");
         }

         public void Dispose() {
            throw new Exception("The method or operation is not implemented.");
         }

         #endregion
      }

      [Test]
      [ExpectedException(typeof(Exception))]
      public void MissingConstructorThrowsException() {
         LoaderFactory.Type = typeof(LoaderMissingConstructor);
         LoaderFactory.CreateInstance(GetType().Assembly.Location);
      }

      [Test]
      [ExpectedException(typeof(ArgumentException))]
      public void NonITestAssemblyThrows() {
         LoaderFactory.Type = typeof(string);
      }

      [FixtureTearDown]
      public void FixtureTearDown() {
         LoaderFactory.Type = LoaderFactory.Default;
      }

      [FixtureSetUp]
      public void FixtureSetup() {
         LoaderFactory.Type = LoaderFactory.Default;
      }
   }
}
