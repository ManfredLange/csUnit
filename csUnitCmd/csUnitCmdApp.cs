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

using csUnit.Core;
using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.CommandLine {
   /// <summary>
   /// Summary description for CsUnitApp.
   /// </summary>
   class CsUnitApp {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      internal static int Main(string[] args) {
         var app = new CsUnitApp();
         return app.Execute(args);
      }

      private int Execute(string[] args) {
         var result = 0;
         try {
            var clh = new CmdLineHandler();
            SetUpCommandLineOptions(clh);
            clh.Evaluate(args);

            if( ! clh.IsValid() ) {
               PrintUsage();
               result = 1;
            }
            else if(HelpNeeded(clh)) {
               PrintUsage();
               result = 0;
            }
            else {
               IRecipe recipe = null;
               if(clh.HasOption("recipe")) {
                  if( clh.HasOption("assembly")) {
                     Console.WriteLine("Option 'recipe' present, ignoring option 'assembly'.");
                  }
                  recipe = RecipeFactory.Load(clh.GetOptionValueFor("recipe"));
                  if( recipe == null ) {
                     Console.WriteLine(String.Format(
                        "Couldn't read recipe at location '{0}'.",
                        clh.GetOptionValueFor("recipe")));
                     result = 1;
                  }
               }
               else if(clh.HasOption("assembly")) {
                  var filePathName = clh.GetOptionValueFor("assembly");
                  if(File.Exists(filePathName)) {
                     recipe = RecipeFactory.NewRecipe(string.Empty);
                     recipe.AddAssembly(filePathName);
                  }
                  else {
                     Console.WriteLine("Error: Couldn't read assembly at location '{0}'.", filePathName);
                     PrintUsage();
                     result = 1;
                  }
               }
               if(   recipe != null
                  && result == 0 ) {
                  result = ExecuteValidCommandLine(clh, recipe);
               }
            }
         }
         catch(Exception ex) {
            while(ex.InnerException != null) {
               ex = ex.InnerException;
            }
            Console.WriteLine("Internal error: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            result = 1;
         }
         Console.WriteLine("Done.");
         return result;
      }

      internal int ExecuteValidCommandLine(CmdLineHandler clh, IRecipe recipe) {
         var result = 0;
         SetUpCategorySelector("testCategory", clh, recipe);
         SetUpCategorySelector("fixtureCategory", clh, recipe);
         SetUpRegexSelector(clh, recipe);

         DefaultXmlWriter resultWriter = null;
         if(clh.HasOption("xml")) {
            var resultPathName = clh.GetOptionValueFor("xml");
            resultPathName = resultPathName == string.Empty ? "csUnit.results.xml" : resultPathName;
            resultWriter = new DefaultXmlWriter(recipe, resultPathName);
         }

         recipe.Aborted += RecipeAborted;
         recipe.RunTests(new TestRun(new AllTestsCriterion()));

         recipe.Join();

         if(resultWriter != null) {
            resultWriter.Save();
            result = resultWriter.Result;
         }
         if (result == 0 && _recipeAborted) {
            Console.Error.WriteLine("Tests Aborted: " + _recipeAbortMessage);
            result = 2;
         }
         return result;
      }

      private void RecipeAborted(object sender, RecipeEventArgs args) {
         _recipeAbortMessage = args.Message;
         _recipeAborted = true;
      }

      private static void SetUpCategorySelector(string optionName, CmdLineHandler clh, IRecipe recipe) {
         if(clh.HasOption(optionName)) {
            var selector = new CategorySelector();
            selector.IncludedCategories.Add(clh.GetOptionValueFor(optionName));
            recipe.RegisterSelector(selector);
         }
      }

      private static void SetUpRegexSelector(CmdLineHandler clh, IRecipe recipe) {
         if(clh.HasOption("pattern")) {
            var selector = new RegexSelector {  
               Pattern = clh.GetOptionValueFor( "pattern" )
            };
            recipe.RegisterSelector(selector);
         }
      }

      private static bool HelpNeeded(CmdLineHandler clh) {
         return clh.Count == 0 || clh.HasOption("?") || clh.HasOption("help");
      }

      private static void SetUpCommandLineOptions(CmdLineHandler clh) {
         clh.AcceptOption("testCategory", true);
         clh.AcceptOption("tc", true);

         clh.AcceptOption("fixtureCategory", true);
         clh.AcceptOption("fc", true);
         
         clh.AcceptOption("pattern", true);
         clh.AcceptOption("assembly", true);
         clh.AcceptOption("recipe", true);
         clh.AcceptOption("xml", false);
         clh.AcceptOption("?", false);
         clh.AcceptOption("help", false);
         clh.AcceptOption("waitfordebugger", false);
      }

      private static void PrintUsage() {
         Console.WriteLine("csUnitCmd, Version " + RetrieveAssemblyVersion());
         Console.WriteLine("Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson,");
         Console.WriteLine("                         and Piers Lawson. All rights reserved.");
         Console.WriteLine("Use of this software only upon acceptance the license conditions");
         Console.WriteLine("See file license.txt in the installation folder for further details.");
         Console.WriteLine();
         Console.WriteLine("Starts a new instance of csUnitCmd. Usage:");
         Console.WriteLine();
         Console.WriteLine("   csUnitCmd /<option>");
         Console.WriteLine();
         Console.WriteLine("Where <option> can be one or more of the following:");
         Console.WriteLine();
         Console.WriteLine("   /assembly:<assemblyName>");
         Console.WriteLine("      Loads the specified assembly.");
         Console.WriteLine("      Will be ignored, if /recipe is specified.");
         Console.WriteLine();
         Console.WriteLine("   /recipe:<recipeName>");
         Console.WriteLine("      Loads the specified recipe.");
         Console.WriteLine();
         Console.WriteLine("   /xml");
         Console.WriteLine("      Writes the results to csUnit.results.xml");
         Console.WriteLine();
         Console.WriteLine("   /xml:[<xmlName>]");
         Console.WriteLine("      Writes results to the specified XML file.");
         Console.WriteLine("      The file is overwritten each time the execution of");
         Console.WriteLine("      a recipe has been started.");
         Console.WriteLine();
         Console.WriteLine("   /testCategory:<name> or /tc:<name>");
         Console.WriteLine("      Specifies the name of the category that identifies which");
         Console.WriteLine("      test methods will be run.");
         Console.WriteLine();
         Console.WriteLine("   /fixtureCategory:<name> or /fc:<name>");
         Console.WriteLine("      Specifies the name of the category that identifies which");
         Console.WriteLine("      fixtures will be run.");
         Console.WriteLine();
         Console.WriteLine("   /pattern:<value>");
         Console.WriteLine("      Specifies a regular expression pattern used to select tests by");
         Console.WriteLine("      their fully qualified names.");
         Console.WriteLine();
         Console.WriteLine("   /autorun");
         Console.WriteLine("      ");
         Console.WriteLine("      ");
         Console.WriteLine();
         Console.WriteLine("");
         Console.WriteLine("   /?");
         Console.WriteLine("   /help");
         Console.WriteLine("      Displays this help message.");
         Console.WriteLine("");
         Console.WriteLine("Examples:");
         Console.WriteLine("   csUnitRunner /assembly:\"Program Files\\test.dll\"");
         Console.WriteLine("   csUnitRunner /recipe:\'Test Files\\mock.recipe\'");
         Console.WriteLine("   csUnitRunner /out:results.xml");
         Console.WriteLine("   csUnitRunner myRecipe.recipe");
         Console.WriteLine("   csUnitRunner myAssembly.dll");
         Console.WriteLine("");
         Console.WriteLine("Note: Options can also preceeded by a minus, e.g");
         Console.WriteLine("      -autorun");
         Console.WriteLine("");
         Console.WriteLine("When no options are specified and only one argument is given,");
         Console.WriteLine("then the argument is interpreted as either a recipe, assembly,");
         Console.WriteLine("or executable. If the sole argument does not have a file extension,");
         Console.WriteLine("then it is interpreted as a recipe file.");
         Console.WriteLine("");
         Console.WriteLine("For support options, bug reports, and feature requests, and");
         Console.WriteLine("contact options, please see the ReleastNotes.txt. Thank you!");
      }

      private static String RetrieveAssemblyVersion() {
         var ass = Assembly.GetExecutingAssembly();
         return ass.GetName().Version.ToString(3);
      }

      private bool _recipeAborted;
      private string _recipeAbortMessage;
   }
}
