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

using csUnit.Core;

namespace csUnit.CommandLine.Tests {
   // ReSharper disable UnusedMember.Global
   [TestFixture]
   public class RecipeOptionTests : CsUnitCmdTests {
      [TearDown]
      public void TearDown() {
         RecipeFactory.Type = RecipeFactory.Default;
      }

      [Test]
      public void NonExistentRecipe() {
         int result = CsUnitApp.Main(new[] { "/recipe:deleteMe.recipe" });
         Assert.Equals(1, result);
         Assert.Contains("Couldn't read recipe at location 'deleteMe.recipe'.", Output);
      }

      [Test]
      public void NoRecipeNameSpecified() {
         int result = CsUnitApp.Main(new[] { "/recipe" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Option 'recipe' requires value.", Output);
      }

      [Test]
      public void NoRecipeNameSpecifiedIncludingColon() {
         var result = CsUnitApp.Main(new[] { "/recipe:" });
         Assert.Equals(1, result);
         Assert.Contains("Error: Option 'recipe' requires value.", Output);
      }

      [Test]
      public void SuccessfulRecipeRead() {
         RecipeFactory.Type = typeof(RecipeMock);
         int result = CsUnitApp.Main(new[] { @"/recipe:..\..\coreassemblies.recipe" });
         Assert.Equals(0, result);
      }
   }
   // ReSharper restore UnusedMember.Global
}
