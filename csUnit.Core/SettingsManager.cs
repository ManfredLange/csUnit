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

using csUnit.Interfaces;

namespace csUnit.Core {
   public class SettingsManager {
      static SettingsManager() {
         foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach(var type in assembly.GetExportedTypes()) {
               var itf = type.GetInterface(typeof(ISettingsProvider).Name);
               if(itf != null) {
                  var provider = CreateProvider(type);
                  if(null != provider) {
                     Providers.Add(provider);
                  }
               }
            }
         }
      }

      public object this[string settingsName] {
         get {
            foreach(var provider in Providers) {
               if(provider.Settings.Contains(settingsName)) {
                  return provider[settingsName];
               }
            }
            throw new System.Configuration.SettingsPropertyNotFoundException();
         }
         set {
            foreach(var provider in Providers) {
               if(provider.Settings.Contains(settingsName)) {
                  provider[settingsName] = value;
                  return;
               }
            }
            throw new System.Configuration.SettingsPropertyNotFoundException();
         }
      }

      public static List<string> Settings {
         get {
            var names = new List<string>();
            foreach(var provider in Providers) {
               names.AddRange(provider.Settings);
            }
            return names;
         }
      }

      private static ISettingsProvider CreateProvider(Type type) {
         var ci = type.GetConstructor(Type.EmptyTypes);
         if(ci != null) {
            return ci.Invoke(new object[] { }) as ISettingsProvider;
         }
         return null;
      }

      private static readonly List<ISettingsProvider> Providers = new List<ISettingsProvider>();
   }
}
