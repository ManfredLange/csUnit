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
using System.Reflection;
using csUnit.Common;
using csUnit.Interfaces;

namespace csUnit.Core {
   public class RecipeFactory {
      /// <summary>
      /// Creates an instance of the RecipeFactory.
      /// </summary>
      private RecipeFactory() {
      }

      #region Properties
      /// <summary>
      /// Gets the current recipe. Only one recipe can be loaded at any given
      /// time. Each time a new instance is created a reference to it is
      /// available through this static property.
      /// </summary>
      public static IRecipe Current {
         get {
            return _current;
         }
         private set {
            if(_current != null) {
               if(Closing != null) {
                  Closing(null, new RecipeEventArgs());
               }
            }
            _current = value;
            if(Loaded != null) {
               Loaded(null, new RecipeEventArgs());
            }
         }
      }
      
      /// <summary>
      /// Gets the default type to be instantiated by the RecipeFactory.
      /// </summary>
      internal static Type Default {
         get {
            return typeof(Recipe);
         }
      }
      
      /// <summary>
      /// Sets the type to be instantiated by the RecipeFactory. If the type
      /// doesn't implement csUnit.Interfaces.IRecipe this setter throws an
      /// ArgumentException.
      /// </summary>
      internal static Type Type {
         set {
            if(value.GetInterface(typeof(IRecipe).FullName) == null) {
               throw new ArgumentException(value.FullName + " does not implement "
                  + typeof(IRecipe).FullName);
            }
            _recipeType = value;
            RegisteredFilters.Clear();
         }
      }
      #endregion // Properties

      #region Methods
      /// <summary>
      /// Creates a new instance of a recipe.
      /// </summary>
      /// <returns>An object implementing the IRecipe interface.</returns>
      public static IRecipe NewRecipe(string pathName) {
         var recipe = CreateInstance(pathName);
         Current = recipe;
         return recipe;
      }

      /// <summary>
      /// Creates an instance of a recipe.
      /// </summary>
      /// <param name="pathName"></param>
      /// <returns>An object implementing the IRecipe interface.</returns>
      /// <remarks>Calling this method doesn't set Recipe.Current and hence the
      /// Recipe.Loaded event will not be fired.</remarks>
      private static IRecipe CreateInstance(string pathName) {
         var ci = _recipeType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
            CallingConventions.HasThis, new Type[0], null);
         var recipe = ci.Invoke(Type.EmptyTypes) as IRecipe;
         if( recipe != null ) {
            foreach(var filter in RegisteredFilters) {
               recipe.RegisterSelector(filter);
            }
            recipe.PathName = pathName;
         }
         return recipe;
      }

      /// <summary>
      /// Load and construct a recipe from a file.
      /// </summary>
      /// <param name="pathName">Path and name of the recipe file.</param>
      /// <returns>An object implementing the IRecipe interface.</returns>
      /// <remarks>If any of the assemblies cannot be found the recipe will 
      /// still be loaded, but without the assembly that couldn't be found.
      /// If the recipe couldn't be found, the current recipe will stay
      /// open.
      /// For any file that couldn't be found a LoadFailed event is fired.
      /// </remarks>
      public static IRecipe Load(string pathName) {
         var doc = XmlDocumentFactory.CreateInstance();
         if( doc.Exists(pathName) ) {

            doc.Load(pathName);

            var retrieved = CreateInstance(pathName);
            var fileNotFounds = retrieved.LoadFromXml(doc);

            if(LoadFailed != null) {
               foreach(var ex in fileNotFounds) {
                  LoadFailed(null, new RecipeEventArgs(ex.Message + " " + ex.FileName));
               }
            }
            Current = retrieved;
            return retrieved;
         }
         if(LoadFailed != null) {
            LoadFailed(null, new RecipeEventArgs("Couldn't load recipe '" + pathName + "'."));
         }
         return null;
      }

      /// <summary>
      /// Register an object implementing csUnit.Interfaces.ISelector with the
      /// factory. Each time a new recipe is instantiated this set of objects
      /// is in turn registered with that recipe object.
      /// </summary>
      /// <param name="filter">An object implementing ISelector.</param>
      /// <remarks>Please see <see cref="csUnit.Interfaces.ISelector"/> for more
      /// information about the selector concept.</remarks>
      internal static void RegisterSelector(ISelector filter) {
         RegisteredFilters.Add(filter);
      }
      #endregion // Methods

      #region Events
      /// <summary>
      /// Raised upon successful load of a recipe.
      /// </summary>
      public static event RecipeEventHandler Loaded;

      /// <summary>
      /// Raised when either the recipe or one of the test assemblies within the
      /// recipe could not be found during load.
      /// </summary>
      public static event RecipeEventHandler LoadFailed;

      /// <summary>
      /// Raised just before the recipe is finally closed.
      /// </summary>
      /// <remarks>The static member RecipeFactory.Current and all members on the 
      /// recipe can still be used while handling the event. After the event has
      /// been handled, a new Recipe instance will be created either through 
      /// load or creation, and the Loaded event will be fired.
      /// </remarks>
      public static event RecipeEventHandler Closing;
      #endregion // Events

      #region Fields
      private static readonly Set<ISelector> RegisteredFilters = new Set<ISelector>();
      private static Type _recipeType = Default;
      private static IRecipe _current;
      #endregion // Fields
   }
}
