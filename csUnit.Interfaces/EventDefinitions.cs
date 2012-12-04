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

namespace csUnit.Interfaces {

   // TODO: Instead of two strings the constructor should simply take an AssemblyName object. [15aug09, ml]

   /// <summary>
   /// Arguments for assembly events.
   /// </summary>
   [Serializable]
   public class AssemblyEventArgs : EventArgs {
      public AssemblyEventArgs(string assemblyFullName, string assemblyPathFileName) {
         _assemblyFullName = assemblyFullName;
         _assemblyPathFileName = assemblyPathFileName;
         if( _assemblyFullName == _assemblyPathFileName ) {
            throw new ArgumentException("Assembly Name cannot be identical to filePathName.");
         }
      }

      /// <summary>
      /// Gets the full name of the affected assembly.
      /// </summary>
      public string AssemblyFullName {
         get {
            return _assemblyFullName;
         }
      }

      public string PathFileName {
         get {
            return _assemblyPathFileName;
         }
      }

      private readonly string _assemblyFullName = string.Empty;
      private readonly string _assemblyPathFileName = string.Empty;
   }

   /// <summary>Delegate for handling events sent by assemblies.</summary>
   /// <param name="sender">The sender of the message.</param>
   /// <param name="args">Arguments for the message.</param>
   public delegate void AssemblyEventHandler(object sender, AssemblyEventArgs args);
}
