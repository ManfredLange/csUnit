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
using System.Reflection;

namespace csUnit.Core.Tests {
   /// <summary>
   /// Summary description for UtilTests.
   /// </summary>
   public class UtilTests {
      [TestFixture]
         public class AbsoluteToRelativeTests {
         [Test]
         public void AbsoluteToRelativePath1() {
            string baseDir = @"c:\foo\bar";
            string fileName = @"c:\foo\bar\myfile.txt";

            Assert.Equals("myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath2() {
            string baseDir = @"c:\foo\bar";
            string fileName = @"c:\foo\myfile.txt";

            Assert.Equals(@"..\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath3() {
            string baseDir = @"c:\foo\bar";
            string fileName = @"c:\goo\geek\myfile.txt";

            Assert.Equals(@"..\..\goo\geek\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath4() {
            string baseDir = @"d:\projects\csUnit\csUnitRunner\bin\Debug";
            string fileName = @"d:\projects\csUnit\csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll";

            Assert.Equals(@"..\..\..\csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath5() {
            string baseDir = @"c:\foo\bar";
            string fileName = @"c:\myfile.txt";

            Assert.Equals(@"..\..\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath6() {
            string baseDir = @"c:\foo";
            string fileName = @"c:\bar\goo\geek\myfile.txt";

            Assert.Equals(@"..\bar\goo\geek\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath7() {
            string baseDir = @"d:\projects\csUnit";
            string fileName = @"d:\projects\csUnit\csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll";

            Assert.Equals(@"csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll",
               csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void AbsoluteToRelativePath8() {
            string baseDir = @"D:\";
            string fileName = @"d:\projects\csunit\csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll";

            Assert.Equals(@"projects\csunit\csUnitCore\csUnitCoreTest\bin\Debug\csUnitCoreTest.dll",
               csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void FileOnDifferentDrive() {
            string baseDir = @"c:\temp";
            string fileName = @"d:\sample.txt";

            Assert.Equals(@"d:\sample.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void ShouldIgnoreCasing1() {
            string baseDir = @"C:\FOO\BAR";
            string fileName = @"d:\foo\bar\myfile.txt";

            Assert.Equals(@"d:\foo\bar\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void ShouldIgnoreCasing2() {
            string baseDir = @"C:\FOO\BAR";
            string fileName = @"c:\foo\bar\myfile.txt";

            Assert.Equals("myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }

         [Test]
         public void ShouldIgnoreCasing3() {
            string baseDir = @"C:\FOO\BAR";
            string fileName = @"c:\goo\geek\myfile.txt";

            Assert.Equals(@"..\..\goo\geek\myfile.txt", csUnit.Core.Util.GetRelativeFilename(baseDir, fileName));
         }
      }

      [TestFixture]
         public class RelativeToAbsoluteTests {
         [Test]
         public void RelativeToAbsolutePath1() {
            string baseDir = @"d:\projects\csUnit";
            string relativeName = @"test.recipe";

            Assert.Equals(@"d:\projects\csUnit\test.recipe", csUnit.Core.Util.GetAbsoluteFilename(baseDir, relativeName));
         }

         [Test]
         public void RelativeToAbsolutePath2() {
            string baseDir = @"d:\projects\csUnit";
            string relativeName = @"..\test.recipe";

            Assert.Equals(@"d:\projects\test.recipe", csUnit.Core.Util.GetAbsoluteFilename(baseDir, relativeName));
         }

         [Test]
         public void RelativeToAbsolutePath3() {
            string baseDir = @"d:\projects\csUnit";
            string relativeName = @"..\..\test.recipe";

            Assert.Equals(@"d:\test.recipe", csUnit.Core.Util.GetAbsoluteFilename(baseDir, relativeName));
         }

         [Test]
         public void RelativeToAbsolutePath4() {
            string baseDir = @"d:\projects\csUnit";
            string relativeName = @"csUnitRunner\bin\Debug\test.recipe";

            Assert.Equals(@"d:\projects\csUnit\csUnitRunner\bin\Debug\test.recipe", csUnit.Core.Util.GetAbsoluteFilename(baseDir, relativeName));
         }

         [Test]
         public void FileOnDifferentDrive() {
            string baseDir = @"c:\projects\csUnit";
            string relativeName = @"d:\projects\csUnit\csUnitRunner\bin\Debug\csUnitRunner.exe";
            Assert.Equals(@"d:\projects\csUnit\csUnitRunner\bin\Debug\csUnitRunner.exe", csUnit.Core.Util.GetAbsoluteFilename(baseDir, relativeName));
         }
      }

      [TestFixture]
         public class MakeVersionAgnosticMethodNameTests {
         internal class Foo {
            public void Bar() {
            }
         }
         
         [Test]
         public void MakeVersionAgnosticMethodName() {
            Type t = typeof(Foo);
            MethodInfo mi = t.GetMethod("Bar");
            Assert.Equals("csUnit.Core.Tests#csUnit.Core.Tests.UtilTests+MakeVersionAgnosticMethodNameTests+Foo#Bar#",
               csUnit.Core.Util.MakeVersionAgnosticMethodName(mi));
         }
      }
   }
}
