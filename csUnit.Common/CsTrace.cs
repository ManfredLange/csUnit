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
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace csUnit.Common {
   public class CsTrace {
      public CsTrace(string traceSwitchName) {
         _traceApplication = new TraceSwitch(traceSwitchName, "Trace switch for csUnit.");
         _traceApplication.Level = ( new CsTraceSettings() ).TraceLevelForSwitch(traceSwitchName);

         if( _traceApplication.Level != TraceLevel.Off ) {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string targetFile = Path.Combine(myDocuments, traceSwitchName + ".log.txt");
            FileStream myTraceLog = new FileStream(targetFile, FileMode.OpenOrCreate);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myTraceLog);
            Trace.Listeners.Add(myListener);
            Trace.AutoFlush = true;
         }
      }

      public void WriteLine(TraceLevel level, string message) {
         if( _traceApplication.Level >= level) {
            Trace.WriteLine(FormatTraceMessage(message));
         }
      }

      private static string FormatTraceMessage(string message) {
         return string.Format("[{0:0.000}] {1}", ( (double) Environment.TickCount ) / 1000, message);
      }

      private readonly TraceSwitch _traceApplication;

      /// <summary>
      /// Class for interfacing with an XML file containing TraceSwitch
      /// configurations.
      /// </summary>
      /// <remarks>TraceSwitch configurations are stored in a file called 
      /// "csUnit.Tracing.xml" which is located at the current users "My
      /// Documents" folder. The content is structured exactly like the
      /// same information in an app.config or web.config file.
      /// The following is an example of a csUnit.Tracing.xml file:
      /// </remarks>
      /// <example>
      /// </configuration>
      ///    <system.diagnostics>
      ///       <!--
      ///       Please use the following switches only if requested by the csUnit team to
      ///       diagnose runtime issues.
      ///       By default all switches are set to off (= 0). Other values are:
      ///          1 = Error
      ///          2 = Warning
      ///          3 = Info
      ///          4 = Verbose
      ///       Remarks: 
      ///       - By default we try to avoid the use of tracing. However, we have
      ///         encountered issues that we were unable to reproduce. The switches should
      ///         be used only in those rare cases. 
      ///       - If you use the switches in other cases you do so on your own risk. Be 
      ///         aware that log files can potentially become very large.
      ///       - If tracing is enabled log files will be writen to the "My Documents"
      ///         folder (= "Documents" on Vista). For non-English system look in the
      ///         respective place. Log files are created in this folder because this
      ///         is the most likely place to have write permissions.
      ///       -->
      ///       <switches>
      ///          <!--
      ///          The General switch is for all non-specific messages.
      ///          -->
      ///          <add name="General" value="0" />
      ///       </switches>
      ///    </system.diagnostics>
      /// </configuration>
      /// </example>
      private class CsTraceSettings {
         public CsTraceSettings() {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string targetFile = Path.Combine(myDocuments, "csUnit.Tracing.xml");
            if( File.Exists(targetFile) ) {
               XmlDocument switchSettings = new XmlDocument();
               switchSettings.Load(targetFile);
               XmlNode root = switchSettings.DocumentElement;
               if(root != null) {
// ReSharper disable PossibleNullReferenceException
                  foreach( XmlNode node in root.SelectNodes("system.diagnostics/switches/add") ) {
// ReSharper restore PossibleNullReferenceException
                     string name = node.Attributes["name"].Value;
                     TraceLevel level = (TraceLevel) Enum.Parse(typeof(TraceLevel), node.Attributes["value"].Value);
                     _traceLevels.Add(name, level);
                  }
               }
            }
         }

         public TraceLevel TraceLevelForSwitch(string switchName) {
            if( _traceLevels.ContainsKey(switchName) ) {
               return _traceLevels[switchName];
            }
            return TraceLevel.Off;
         }

         private readonly Dictionary<string, TraceLevel> _traceLevels = new Dictionary<string, TraceLevel>(); 
      }
   }
}
