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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace csUnit.Core {
   /// <summary>
   /// The CmdLineHandler class parses the command line and checks for its
   /// validity.
   /// </summary>
   public class CmdLineHandler {
      private static readonly Regex Splitter = new Regex(@"^-{1}|^/|=|:",
         RegexOptions.IgnoreCase|RegexOptions.Compiled);
      private static readonly Regex Replacer = new Regex(@"^['""]?(.*?)['""]?$",
         RegexOptions.IgnoreCase|RegexOptions.Compiled);


      /// <summary>Parses the command line arguments and retrieves options and
      /// their values if present.</summary>
      /// <remarks>Samples for valid syntax of options:
      /// <list type="bullet">
      /// <item><description>-option</description></item>
      /// <item><description>/option</description></item>
      /// <item><description>-option value</description></item>
      /// <item><description>/option value</description></item>
      /// <item><description>-option:value</description></item>
      /// <item><description>/option:value</description></item>
      /// <item><description>-option=value</description></item>
      /// <item><description>/option=value</description></item>
      /// <item><description>-option="a string"</description></item>
      /// <item><description>/option="a string"</description></item>
      /// <item><description>-option='a string'</description></item>
      /// <item><description>/option='a string'</description></item>
      /// </description></item>
      /// </list>
      /// </remarks>
      /// <param name="cmdLineArgs">The commandline arguments.</param>
      public bool Evaluate(string[] cmdLineArgs) {
         string option = null;
         string userSuppliedName = null;
         string[] optionParts;

         foreach(string text in cmdLineArgs) {
            // look for new parameters (- or /) and a possible enclosed 
            // value (= or :)
            optionParts = Splitter.Split(text, 3);
            switch(optionParts.Length) {
               case 1: // found a value
                  if( option != null ) {
                     optionParts[0] = Replacer.Replace(optionParts[0], "$1");
                     if( !_options.ContainsKey(option) ) {
                        _options.Add(option, new OptionInfo(userSuppliedName, optionParts[0]));
                     }
                     else {
                        OptionInfo s = _options[option];
                        s.UserSuppliedValue = String.Format("{0} {1}", s.UserSuppliedValue, optionParts[0]);
                        _options[option] = s;
                     }
                  }
                  else {
                     // Add the non-matched value to the list of extra arguments
                     _extraArguments.Add(optionParts[0]);
                  }
                  break;
               case 2: // found just a parameter
                  if( option != null ) {
                     if( !_options.ContainsKey(option) ) {
                        _options.Add(option, new OptionInfo(userSuppliedName, "true"));
                     }
                  }
                  userSuppliedName = optionParts[1];
                  option = userSuppliedName.ToLower();
                  break;
               case 3: // parameter with enclosed value
                  if( option != null ) {
                     if( !_options.ContainsKey(option) ) {
                        _options.Add(option, new OptionInfo(userSuppliedName, string.Empty));
                     }
                  }
                  userSuppliedName = optionParts[1];
                  option = userSuppliedName.ToLower();
                  // Remove possible enclosing characters (",')
                  if( !_options.ContainsKey(option) ) {
                     optionParts[2] = Replacer.Replace(optionParts[2], "$1");
                     _options.Add(option, new OptionInfo(userSuppliedName, optionParts[2]));
                  }
                  break;
            }
         }

         // in case a parameter is still waiting
         if( option != null ) {
            if( !_options.ContainsKey(option) ) {
               _options.Add(option, new OptionInfo(userSuppliedName, string.Empty/*"true"*/));
            }
         }

         return ValidateOptions();
      }

      /// <summary>
      /// Returns true if the options on the command line included the given named option.
      /// </summary>
      /// <param name="name">The name of the option to query.</param>
      /// <returns></returns>
      public bool HasOption(string name) {
         name = name.ToLower();
         return _options.ContainsKey(name);
      }

      /// <summary>
      /// Returns the value for a command line option. If no value was specified
      /// null is returned.
      /// </summary>
      /// <param name="optionName">Name of the command line option.</param>
      /// <returns>Value if found. Null otherwise.</returns>
      public string GetOptionValueFor(string optionName) {
         optionName = optionName.ToLower();
         return _options.ContainsKey(optionName) ? _options[optionName].UserSuppliedValue : null;
      }

      /// <summary>
      /// Sets the value for a command line option.
      /// </summary>
      /// <param name="optionName">Name of the option.</param>
      /// <param name="value">Value for the option.</param>
      public void SetOptionValueFor(string optionName, string value) {
         var lowerCasedName = optionName.ToLower();
         _options.Add(lowerCasedName, new OptionInfo(optionName, value));
      }

      /// <summary>
      /// Gets the number of options.
      /// </summary>
      public int Count {
         get {
            return _options.Count;
         }
      }

      /// <summary>
      /// Accepts the given option as a non-path switch.
      /// </summary>
      /// <param name="optionName">Name of the option.</param>
      /// <param name="bRequiresOptionValue">'true', if the option requires a value, 'false' otherwise.</param>
      public void AcceptOption(string optionName, bool bRequiresOptionValue) {
         optionName = optionName.ToLower();
         if( !_validOptions.ContainsKey(optionName) ) {
            _validOptions.Add(optionName, bRequiresOptionValue);
         }
      }

      /// <summary>
      /// Indicates, whether the commandline is valid. The return value is 
      /// 'false' if an error is detected. See remarks for a list of possible
      /// errors.
      /// </summary>
      /// <remarks>The command line is considered invalid, if an error from the
      /// following list is detected:
      /// <list type="bullet">
      /// <item><description>an unknown command line option</description></item>
      /// <item><description>missing parameter from an option requiring a parameter
      /// </description></item>
      /// </list>
      /// </remarks>
      /// <returns>True if the command line is valid, false otherwise.</returns>
      public bool IsValid() {
         return _valid;
      }

      /// <summary>
      /// Validates whether all given options are correct. E.g. whether an
      /// option that requires a value actually has a value. Also, unknown
      /// options are rejected.
      /// </summary>
      /// <returns>True if all options are valid, false otherwise.</returns>
      private bool ValidateOptions() {
         foreach(string key in _options.Keys) {
            if( !_validOptions.ContainsKey(key) ) {

               Console.WriteLine("Error: Invalid option '{0}'.", key);

               _options.Remove(key);
               _valid = false;
               break;
            }
            if(    _validOptions[key]
                   && _options[key].UserSuppliedValue == string.Empty ) {

               Console.WriteLine("Error: Option '{0}' requires value.", 
                                 _options[key].UserSuppliedName);

               _options.Remove(key);
               _valid = false;
               break;
            }
         }
         return _valid;
      }

      /// <summary>
      /// Returns those arguments that are not switches.
      /// </summary>
      public string[] NonSwitchArguments {
         get {
            var args = (string[])_extraArguments.ToArray(typeof(string));
            return(args);
         }
      }

      /// <summary>
      /// Represents an option as specified by the user. Although the options
      /// are case insensitive, this is interesting for the internal handling
      /// only. To improve user experience all messages displayed to the user
      /// include the option names and value the way the user specified them.
      /// </summary>
      private class OptionInfo {
         public OptionInfo(string userSuppliedName, string userSuppliedValue) {
            UserSuppliedName = userSuppliedName;
            UserSuppliedValue = userSuppliedValue;
         }

         public readonly string UserSuppliedName;
         public string UserSuppliedValue;
      }

      private readonly ArrayList _extraArguments = new ArrayList();
      private bool   _valid = true;

      private readonly Dictionary<string, bool> _validOptions = new Dictionary<string, bool>();
      private readonly Dictionary<string, OptionInfo> _options = new Dictionary<string, OptionInfo>();
   }
}
