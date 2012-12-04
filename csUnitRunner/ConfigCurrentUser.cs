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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using csUnit.Interfaces;
using csUnit.Runner.Properties;

namespace csUnit.Runner {
   /// <summary>
   /// The ConfigCurrentUser class encapsulates access to the user specific
   /// settings, stored in the csUnit.exe.config file. Note that there are
   /// additional user specific settings that may be stored in other config
   /// files, e.g. csUnit.Ui.Controls.dll.config for user specific settings
   /// for some of csUnit's user interface controls.
   /// </summary>
   public class ConfigCurrentUser : ISettingsProvider {
      public ConfigCurrentUser() {
         _screenBounds = DetermineScreenSize();
      }

      #region ISettingsProvider Members

      public List<string> Settings {
         get {
            List<string> names = new List<string>();
            Settings settings = new Settings();
            System.Configuration.SettingsPropertyCollection spc = settings.Properties;
            foreach(System.Configuration.SettingsProperty property in spc) {
               names.Add(property.Name);
            }
            return names;
         }
      }

      public object this[string settingsName] {
         get {
            Settings settings = new Settings();
            return settings[settingsName];
         }
         set {
            Settings settings = new Settings();
            settings[settingsName] = value;
            settings.Save();
         }
      }

      #endregion

      public Size MainFormSize {
         get {
            Settings settings = new Settings();
            int width = settings.MainFormSize.Width;
            int height = settings.MainFormSize.Height;
            if(width < 200)
               width = 432;
            if(height < 100)
               height = 192;
            return new Size(width, height);
         }
         set {
            Settings settings = new Settings();
            settings.MainFormSize = value;
            settings.Save();
         }
      }

      public Point MainFormLocation {
         get {
            Settings settings = new Settings();
            int posX = settings.MainFormLocation.X;
            int posY = settings.MainFormLocation.Y;
            posX = posX < _screenBounds.Left ? _screenBounds.Left : posX;
            posX = posX > _screenBounds.Right ? _screenBounds.Right : posX;
            posY = posY < _screenBounds.Top ? _screenBounds.Top : posY;
            posY = posY > _screenBounds.Bottom ? _screenBounds.Bottom : posY;
            return new Point(posX, posY);
         }
         set {
            Settings settings = new Settings();
            settings.MainFormLocation = value;
            settings.Save();
         }
      }
    
      public bool AskForSafeOnModifiedUntitled {
         get {
            Settings settings = new Settings();
            return settings.AskForSafeOnModifiedUntitled;
         }
         set {
            Settings settings = new Settings();
            settings.AskForSafeOnModifiedUntitled = value;
            settings.Save();
         }
      }

      public string StartupLoadItem {
         get {
            Settings settings = new Settings();
            return settings.StartupLoadItem;
         }
         set {
            Settings settings = new Settings();
            settings.StartupLoadItem = value;
            settings.Save();
         }
      }

      public bool StatusBarVisible {
         get {
            Settings settings = new Settings();
            return settings.StatusBarVisible;
         }
         set {
            Settings settings = new Settings();
            settings.StatusBarVisible = value;
            settings.Save();
         }
      }

      public bool ToolBarVisible {
         get {
            Settings settings = new Settings();
            return settings.ToolBarVisible;
         }
         set {
            Settings settings = new Settings();
            settings.ToolBarVisible = value;
            settings.Save();
         }
      }

      public List<string> RecentRecipies {
         get {
            Settings settings = new Settings();
            List<string> recentRecipies = new List<string>();
            if(settings.RecentRecipies != null) {
               foreach(string recipe in settings.RecentRecipies) {
                  recentRecipies.Add(recipe);
               }
            }
            return recentRecipies;
         }
         set {
            Settings settings = new Settings();
            StringCollection recentRecipies = new StringCollection();
            recentRecipies.AddRange(value.ToArray());
            settings.RecentRecipies = recentRecipies;
            settings.Save();
         }
      }

      public List<string> RecentAssemblies {
         get {
            Settings settings = new Settings();
            List<string> recentAssemblies = new List<string>();
            if(settings.RecentAssemblies != null) {
               foreach(string assembly in settings.RecentAssemblies) {
                  recentAssemblies.Add(assembly);
               }
            }
            return recentAssemblies;
         }
         set {
            Settings settings = new Settings();
            StringCollection recentAssemblies = new StringCollection();
            recentAssemblies.AddRange(value.ToArray());
            settings.RecentAssemblies = recentAssemblies;
            settings.Save();
         }
      }

      private static Rectangle DetermineScreenSize() {
         return Screen.PrimaryScreen.WorkingArea;
      }

      private Rectangle    _screenBounds;
   }
}
