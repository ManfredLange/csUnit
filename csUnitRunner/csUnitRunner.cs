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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using csUnit.Common;
using csUnit.Core;

namespace csUnit.Runner {
   /// <summary>
   /// Summary description for csunit.
   /// </summary>
   internal class csUnitRunner {
      /// <summary>
      /// Entry point of csUnitRunner.
      /// </summary>
      /// <remarks>args[0] may contain an assembly with tests to be loaded.</remarks>
      [STAThread]
      [LoaderOptimization(LoaderOptimization.MultiDomain)]
      public static int Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AddExplicitOptionToSoleArgumentIfOmitted(ref args);

            var clh = SetUpCommandLineHandler(args);

            if(clh.IsValid() == false
               || clh.HasOption("?")
               || clh.HasOption("help")) {
               PrintUsage(clh.HasOption("nodialogs"));
               return 1;
            }
            if(clh.NonSwitchArguments.Length > 0
               && !clh.HasOption("assembly")
               && !clh.HasOption("recipe")) {
               var xargs = clh.NonSwitchArguments;
               var fileName = string.Join(" ", xargs);
               var fi = new FileInfo(xargs[0]);
               if(!fi.Exists) {
                  Console.WriteLine("File '{0}' does not exist.", fileName);
                  return (1);
               }
               if(fi.Extension != null) {
                  switch(fi.Extension.ToUpper()) {
                     case ".EXE":
                     case ".DLL":
                        clh.SetOptionValueFor("assembly", fileName);
                        break;
                     case ".RECIPE":
                        clh.SetOptionValueFor("recipe", fileName);
                        break;
                     default:
                        Console.WriteLine("Do not know how to handle file type={1}: {0} ", fileName, fi.Extension);
                        return (1);
                  }
                  clh.SetOptionValueFor("autorun", "true");
               }
            }
            AppDomain currentDomain = AppDomain.CurrentDomain;

            currentDomain.AssemblyLoad += OnAssemblyLoaded;
            currentDomain.UnhandledException += ExceptionHandler; 

            Application.Run(new MainForm(clh));
            return 0;
      }

      public static CsTrace trace = new CsTrace("csUnitRunner");

      private static void AddExplicitOptionToSoleArgumentIfOmitted(ref string[] args) {
         if( args.Length == 1
            && File.Exists(args[0]) ) {
            var info = new FileInfo(args[0]);
            if( info.Extension.ToUpper().Equals(".RECIPE") ) {
               args[0] = string.Format("/recipe:{0}", args[0]);
            }
            else if( info.Extension.ToUpper().Equals(".EXE")
                    || info.Extension.ToUpper().Equals(".DLL") ) {
               args[0] = string.Format("/assembly:{0}", args[0]);
            }
         }
      }

      private static CmdLineHandler SetUpCommandLineHandler(string[] args) {
         var clh = new CmdLineHandler();
         clh.AcceptOption("autoexit", false);
         clh.AcceptOption("autorun", false);
         clh.AcceptOption("assembly", true);
         clh.AcceptOption("recipe", true);
         clh.AcceptOption("xml", false);
         clh.AcceptOption("?", false);
         clh.AcceptOption("help", false);

         clh.AcceptOption("nodialogs", false);
         // hidden option so we can automatically test invalid command line 
         // arguments [23aug09, ml]
         
         clh.Evaluate(args);
         return clh;
      }

      static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args) {
         var e = (Exception) args.ExceptionObject;
         Console.WriteLine("Unhandled exception caught. Message: " + e.Message);
      }
		
      static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args) {
         var a = args.LoadedAssembly;
         trace.WriteLine(TraceLevel.Info,
                         string.Format("Assembly {0} loaded from {1}", a.FullName, a.CodeBase));
      }

      private static void PrintUsage(bool noDialogs) {
         if( noDialogs == false ) {
            var sb = new StringBuilder();
            sb.Append("csUnitRunner command line usage:\n");
            sb.Append("\n");
            sb.Append("   csUnitRunner [/<option>[:<value>]]\n");
            sb.Append("\n");
            sb.Append("Where <option> can be one or more of the following:\n");
            sb.Append("\n");
            sb.Append("   /assembly:<assemblyName>\n");
            sb.Append("      Loads the specified assembly. ");
            sb.Append("Will be ignored, if /recipe is specified, too.\n");
            sb.Append("\n");
            sb.Append("   /autorun\n");
            sb.Append("      When used in combination with /assembly or /recipe ");
            sb.Append("execution of tests will start atuomatically.\n");
            sb.Append("\n");
            sb.Append("   /autoexit\n");
            sb.Append("      Used with the /autorun switch, will close csUnitRunner ");
            sb.Append("after the configured recipe has finished.\n");
            sb.Append("\n");
            sb.Append("   /recipe:<recipeName>\n");
            sb.Append("      Loads the specified recipe.");
            sb.Append("If specified option /assembly will be ignored.\n");
            sb.Append("\n");
            sb.Append("   /?\n");
            sb.Append("      Displays this help message.\n");
            sb.Append("\n");
            sb.Append("Examples:\n");
            sb.Append("   csUnitRunner /recipe:\"Test Files\\mock.recipe\"\n");
            sb.Append("   csUnitRunner /assembly:\"Program Files\\test.dll\"\n");
            sb.Append("   csUnitRunner /recipe:\"Test Files\\mock.recipe\" /autorun\n");
            sb.Append("   csUnitRunner myRecipe.recipe\n");
            sb.Append("   csUnitRunner myAssembly.dll\n");
            sb.Append("\n");
            sb.Append("When no options are specified and only one argument is given,\n");
            sb.Append("then the argument is interpreted as either a recipe or an assembly.\n");
            sb.Append("\n");
            sb.Append("Send feedback, suggestions, and comments to info@csunit.org. Thank you!\n");
            sb.Append("\n");
            sb.Append("Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson,\n");
            sb.Append("                         and Piers Lawson. All rights reserved.\n");
            sb.Append("Usage of this software only under the terms of it's license.\n");
            sb.Append("See file license.txt in the installation folder for further details.\n");

            MessageBox.Show(sb.ToString(), "Invalid Command Line Option detected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
      }
   }
}
