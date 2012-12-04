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

using csUnit.Core;

namespace csUnit.CommandLine.Tests {
   // ReSharper disable UnusedMember.Global
   [TestFixture]
   public class AssemblyOptionTests : CsUnitCmdTests {
      [Test]
      public void WithOutFileName() {
         var result = CsUnitApp.Main(new[] { "/assembly" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Option 'assembly' requires value.", Output);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void WithColonOnly() {
         var result = CsUnitApp.Main(new[] { "/assembly:" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Option 'assembly' requires value.", Output);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void WithNonExistingFile() {
         var result = CsUnitApp.Main(new[] { "/assembly:nirwana.dll" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Couldn't read assembly at location 'nirwana.dll'.", Output);
         Assert.Contains("Starts a new instance of csUnitCmd. Usage:", Output);
      }

      [Test]
      public void IgnoredIfRecipeOptionPresent() {
         RecipeFactory.Type = typeof(RecipeMock);
         RecipeMock.RecipePathName = string.Empty;
         const string recipePathName = @"..\..\coreassemblies.recipe";
         var result = CsUnitApp.Main(new[] { "/assembly:nirwana.dll", 
                           "/recipe:" + recipePathName });
         Assert.Equals(0, result);
         Assert.Contains("Option 'recipe' present, ignoring option 'assembly'.", Output);
         Assert.Equals(recipePathName, RecipeMock.RecipePathName);
      }

      [Test]
      public void TryRecipeAsAssembly() {
         RecipeFactory.Type = RecipeFactory.Default;
         const string recipePathName = @"..\..\coreassemblies.recipe";
         var result = CsUnitApp.Main(new[] { "/assembly:" + recipePathName });
         Assert.Equals(1, result);
         Assert.Contains("File for option /assembly does not refer to an assembly. Must be DLL or EXE.", Output);
      }
   }
   // ReSharper restore UnusedMember.Global
}
