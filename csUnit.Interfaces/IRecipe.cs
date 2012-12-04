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
using System.IO;
using csUnit.Common;

namespace csUnit.Interfaces {
   /// <summary>Delegate for handling events sent by recipies.</summary>
   /// <param name="sender">The sender of the message.</param>
   /// <param name="args">Arguments for the message.</param>
   public delegate void RecipeEventHandler(object sender, RecipeEventArgs args);
   
   public interface IRecipe : IDisposable {
      #region Events
      /// <summary>
      /// Raised just after the running any tests has been aborted by invoking
      /// the Abort method on this recipe.
      /// </summary>
      event RecipeEventHandler Aborted;

      /// <summary>
      /// Raised immediately before execution of tests in the recipe starts.
      /// </summary>
      event RecipeEventHandler Started;

      /// <summary>
      /// Raised after all tests in the recipe have been executed.
      /// </summary>
      event RecipeEventHandler Finished;

      /// <summary>
      /// Raised after the recipe has been successfully saved.
      /// </summary>
      event RecipeEventHandler Saved;

      /// <summary>
      /// Raised when the settings on a selector have been modified.
      /// </summary>
      event RecipeEventHandler SelectorModified;

      /// <summary>
      /// Raised when an assembly was added to the recipe.
      /// </summary>
      event AssemblyEventHandler AssemblyAdded;

      /// <summary>
      /// Raised when an assembly is about to be removed from the recipe.
      /// </summary>
      event AssemblyEventHandler AssemblyRemoving;

      /// <summary>
      /// Raised when an assembly has been removed from the recipe.
      /// </summary>
      event AssemblyEventHandler AssemblyRemoved;
      #endregion // Events

      #region Properties

      /// <summary>
      /// Gets the test assembly object given a full name or a path file name.
      /// Returns null if not found.
      /// </summary>
      ITestAssembly this[string assemblyFullOrPathFileName] { get; }

      /// <summary>
      /// Gets an array of test assemblies in the recipe.
      /// </summary>
      ITestAssembly[] Assemblies { get; }

      /// <summary>
      /// Gets a Categories object with all categories in all test assemblies.
      /// </summary>
      Categories Categories { get; }

      /// <summary>
      /// Gets a set of Selectors registered for the recipe.
      /// </summary>
      Set<ISelector> Selectors { get; }

      /// <summary>
      /// Sets the TextWriter to which any console output is being written.
      /// </summary>
      void SetConsoleOutputTo(TextWriter value);

      /// <summary>
      /// Gets the number of assemblies in the recipe.
      /// </summary>
      int AssemblyCount { get; }
      
      /// <summary>
      /// Gets a displayable name for the recipe.
      /// </summary>
      string DisplayName { get; }

      /// <summary>
      /// Indicates, whether the recipe has been saved before.
      /// </summary>
      bool IsNew { get; }
      
      /// <summary>
      /// Is false, when the recipe has not been modified since the last save
      /// operation. True otherwise. Used to determine, whether the user should
      /// be asked whether or not to save the modified recipe.
      /// </summary>
      bool Modified { get; }

      /// <summary>
      /// Gets the full path and name of the recipe.
      /// </summary>
      string PathName { get; set; }
      
      /// <summary>
      /// Gets a value indicating whether tests are currently running.
      /// </summary>
      bool TestsRunning { get; }

      /// <summary>
      /// When invoked waits until the tests have completed, then returns.
      /// </summary>
      void Join();

      #endregion // Properties

      #region Methods
      
      /// <summary>
      /// Aborts all running tests. Calling when no test is running has no 
      /// effect.
      /// </summary>
      void Abort();

      /// <summary>
      /// Adds an assembly to the recipe.
      /// </summary>
      /// <param name="assemblyPathName">Absolute path and name of the assembly
      /// to be added.</param>
      void AddAssembly(string assemblyPathName);
      
      /// <summary>
      /// Remove all assemblies from the recipe.
      /// </summary>
      void Clear();

      /// <summary>
      /// Closes the recipe by unloading all assemblies.
      /// </summary>
      void Close();
      
      /// <summary>
      /// Returns the count of tests. If a selector has been registered, it
      /// will be considered for determining the number of tests.
      /// </summary>
      /// <returns>Number of tests.</returns>
      int CountTests();

      /// <summary>
      /// Registers a selector with the recipe.
      /// </summary>
      /// <param name="selector">The selector.</param>
      void RegisterSelector(ISelector selector);

      /// <summary>
      /// Removes an assembly from the recipe.
      /// </summary>
      /// <param name="assemblyFullName">The path and name or the full name of 
      /// the assembly to be removed.</param>
      /// <remarks>If the assembly is not in the recipe no error is returned.
      /// </remarks>
      void RemoveAssembly(string assemblyFullName);

      /// <summary>
      /// Runs selected tests in a recipe.
      /// </summary>
      /// <param name="testRun">TestRun describing which tests to run and how.</param>
      void RunTests(ITestRun testRun);

      /// <summary>
      /// Saves the recipe. This requires, that the object was created either
      /// using RecipeFactory.Load(string) or Recipe(string).
      /// </summary>
      void Save();
      
      /// <summary>
      /// Saves the recipe to a file.
      /// </summary>
      /// <param name="pathName">Path and name of the recipe file.</param>
      void Save(string pathName);

      /// <summary>
      /// Loads a recipe from an XML document.
      /// </summary>
      /// <param name="doc">The XML document to load from.</param>
      /// <returns>Returns a list of FileNotFoundException with an entry for
      /// each assembly that couldn't be loaded.</returns>
      List<FileNotFoundException> LoadFromXml(IXmlDocument doc);
      
      /// <summary>
      /// Returns an enumerator for the assemblies in the recipe.
      /// </summary>
      /// <returns>An enumerator.</returns>
      IEnumerator<ITestAssembly> GetEnumerator();
      #endregion // Methods
   }
}
