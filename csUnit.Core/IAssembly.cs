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
using System.Reflection;

namespace csUnit.Core {
   /// <summary>
   /// This interface represents the set of properties, events, and methods that
   /// csUnit uses from class System.Reflection.Assembly.
   /// </summary>
   internal interface IAssembly {
      /// <summary>
      /// Gets the date and time of when the assembly was written last.
      /// </summary>
      DateTime ModifiedTimeStamp {
         get;
      }
      
      /// <summary>
      /// Loads an assembly given an AssemblyName instance.
      /// </summary>
      /// <param name="assemblyName"></param>
      void Load(AssemblyName assemblyName);

      /// <summary>
      /// Gets the display name of the assembly.
      /// </summary>
      string FullName {
         get;
      }

      /// <summary>
      /// Gets the code base as a URL.
      /// </summary>
      string CodeBase { get; }
      
      /// <summary>
      /// Gets the exported types defined in the assembly that are visible
      /// outside the assembly.
      /// </summary>
      /// <returns>An array of <b>Type</b> objects that represent the types
      /// defined in the assembly that are visible outside the assembly.
      /// </returns>
      Type[] GetExportedTypes();
   }
}
