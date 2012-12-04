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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using csUnit.Runner.Commands;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner {
   /// <summary>
   /// RecentRecipies is responsible for maintaining the list of most
   /// recently used recipies. In addition it maintains the according menu
   /// items and provides an event handler for loading a recently used recipe.
   /// </summary>
   internal class RecentRecipies {
      public RecentRecipies(ICommandTarget commandTarget) {
         _commandTarget = commandTarget;

         ReadFromSettings();
      }

      /// <summary>
      /// Gets the number of entries in the recent recipies list.
      /// </summary>
      public int Count {
         get {
            var count = 0;

            foreach(var entry in _recentRecipies) {
               if( entry != string.Empty ) {
                  count++;
               }
            }
            
            return count;
         }
      }

      /// <summary>
      /// Adds a recipe to the list of recent recipies.
      /// </summary>
      /// <param name="recipePathName">Path and name of the recipe to be added.</param>
      public void AddRecipe(string recipePathName) {
         if( recipePathName != string.Empty ) {
            foreach(var entry in _recentRecipies) {
               if(entry.ToLower().Equals(recipePathName.ToLower())) {
                  // move to front
                  _recentRecipies.Remove(entry);
                  break;
               }
            }

            _recentRecipies.Insert(0, recipePathName);
            // truncate
            while(_recentRecipies.Count > 10) {
               _recentRecipies.RemoveAt(_recentRecipies.Count - 1);
            }
            WriteToSettings();
         }
      }

      public void UpdateRecentRecipiesMenu(ToolStripMenuItem menuItem) {
         menuItem.DropDownItems.Clear();
         var index = 0;
         foreach(var entry in _recentRecipies) {
            if( entry != string.Empty ) {
               var item = new ToolStripMenuItem("&" + index + " " + entry);
               item.Click += LoadRecentRecipeClick;
               menuItem.DropDownItems.Add(item);
               index++;
            }
         }
      }

      private void LoadRecentRecipeClick(object sender, EventArgs args) {
         var itemText = string.Empty;
         var item = sender as MenuItem;
         var stripItem = sender as ToolStripMenuItem;
         if(item != null) {
            itemText = item.Text;
         }
         else if(stripItem != null) {
            itemText = stripItem.Text;
         }

         var assemblyPathName = itemText.Substring(3);

         if( File.Exists(assemblyPathName) ) {
            _commandTarget.Status = "Loading " + assemblyPathName + "...";
            FileLoader.LoadFile(assemblyPathName);
            _commandTarget.Status = "Ready";
         }
         else {
            FileLoader.FileNotFound(assemblyPathName);
            _recentRecipies.Remove(assemblyPathName);
            WriteToSettings();
         }
      }

      private void WriteToSettings() {
         new ConfigCurrentUser { RecentRecipies = _recentRecipies };
      }

      private void ReadFromSettings() {
         var config = new ConfigCurrentUser();
         _recentRecipies = config.RecentRecipies;
      }

      private List<string> _recentRecipies = new List<string>();
      private readonly ICommandTarget _commandTarget;
   }
}
