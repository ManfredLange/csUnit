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

namespace csUnit.Runner {
   /// <summary>
   /// RecentAssemblies is responsible for maintaining the list of most
   /// recently used assemblies. In addition it maintains the according menu
   /// items and provides an event handler for loading a recently used assembly.
   /// </summary>
   internal class RecentAssemblies {
      public RecentAssemblies() {
         ReadFromSettings();
      }

      /// <summary>
      /// Gets the number of entries in the recent assemblies list.
      /// </summary>
      public int Count {
         get {
            var count = 0;

            foreach(var entry in _recentAssemblies) {
               if( entry != string.Empty ) {
                  count++;
               }
            }
            
            return count;
         }
      }

      /// <summary>
      /// Adds and assembly to the list.
      /// </summary>
      /// <param name="location">Path and name of the assembly to be added.</param>
      public void AddAssembly(String location) {
         var uri = new Uri(location);
         var absolutePath = uri.LocalPath;

         foreach(var entry in _recentAssemblies) {
            if (entry.ToLower().Equals(absolutePath.ToLower())) {
               // move to front
               _recentAssemblies.Remove(entry);
               break;
            }
         }

         _recentAssemblies.Insert(0, absolutePath);
         // truncate
         while( _recentAssemblies.Count > 10 ) {
            _recentAssemblies.RemoveAt(_recentAssemblies.Count - 1);
         }
         WriteToSettings();
      }

      public void UpdateRecentAssembliesMenu(ToolStripMenuItem menuItem) {
         menuItem.DropDownItems.Clear();
         var index = 0;
         foreach(var entry in _recentAssemblies) {
            if( entry != string.Empty ) {
               var item = new ToolStripMenuItem("&" + index + " " + entry);
               item.Click += LoadRecentAssemblyClick;
               menuItem.DropDownItems.Add(item);
               index++;
            }
         }
      }

      private void LoadRecentAssemblyClick(object sender, EventArgs args) {
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
            FileLoader.LoadFile(assemblyPathName);
         }
         else {
            FileLoader.FileNotFound(assemblyPathName);
            _recentAssemblies.Remove(assemblyPathName);
            WriteToSettings();
         }
      }

      private void WriteToSettings() {
         new ConfigCurrentUser { RecentAssemblies = _recentAssemblies };
      }

      private void ReadFromSettings() {
         _recentAssemblies = new ConfigCurrentUser().RecentAssemblies;
      }

      private List<string> _recentAssemblies = new List<string>();
   }
}
