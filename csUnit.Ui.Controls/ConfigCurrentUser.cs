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

using System.Collections.Generic;
using System.Drawing;
using csUnit.Interfaces;
using csUnit.Ui.Controls.Properties;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// The ConfigCurrentUser class encapsulates access to the config file.
   /// </summary>
// ReSharper disable MemberCanBeInternal
   public class ConfigCurrentUser : ISettingsProvider {
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

      /// <summary>
      /// Gets/sets a value indicating whether to expand comment nodes in the
      /// test hierarchy tree.
      /// </summary>
      public static bool ExpandCommentNodes {
         get {
            return new Settings().ExpandCommentNodes;
         }
         set {
            new Settings { ExpandCommentNodes = value }.Save();
         }
      }

      public static bool ExpandExecutedTestNodes {
         get {
            return new Settings().ExpandExecutedTestNodes;
         }
         set {
            new Settings {ExpandExecutedTestNodes = value}.Save();
         }
      }

      /// <summary>
      /// Gets/sets a value indicating whether to show line numbers in the
      /// output tab.
      /// </summary>
      public static bool ShowLineNumbersInOutput {
         get {
            return new Settings().ShowLineNumbersInOutput;
         }
         set {
            new Settings { ShowLineNumbersInOutput = value }.Save();
         }
      }

      /// <summary>
      /// Gets/sets a value indicating whether to automatically expand the test
      /// hierarchy tree when a recipe or an assembly is loaded.
      /// </summary>
      public static bool AutoExpandTestHierarchy {
          get {
             return new Settings().AutoExpandTestHierarchy;
         }
         set {
            new Settings { AutoExpandTestHierarchy = value }.Save();
         }
      }

      /// <summary>
      /// Gets/sets the color of the progress bar to use as long as all tests
      /// were successful.
      /// </summary>
      public static Color SuccessColor {
         get {
            return new Settings().SuccessColor;
         }
         set {
            new Settings {SuccessColor = value}.Save();
         }
      }

      /// <summary>
      /// Gets/sets the color of the progress bar to use as long as all tests
      /// were successful.
      /// </summary>
      public static Color FailureColor {
         get {
            var settings = new Settings();
            return settings.FailureColor;
         }
         set {
            new Settings {FailureColor = value}.Save();
         }
      }
   }
// ReSharper restore MemberCanBeInternal
}
