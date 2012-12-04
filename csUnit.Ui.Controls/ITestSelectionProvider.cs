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

using System;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// A test selection provider is an object which can be used to produce a
   /// set of tests to be run. Often this will be a graphical element that 
   /// displays tests in some way and a user can then use the graphical element
   /// to determine which tests to run. These tests are selected.
   /// ITestSelectionProvider can also be implemented by other non-UI means, for
   /// instance a command line parameter handler might be an option.
   /// </summary>
   /// <remarks>This interface starts its existence in the csUnit.Ui.Controls
   /// namespace, but I'm not yet sure whether it will stay there long term. 
   /// [25Mar2006, ml]</remarks>
   interface ITestSelectionProvider {
      /// <summary>
      /// This event is fired when the selection in the TestSelectionProvider
      /// has changed. The event does not provide further details about the kind
      /// of change.
      /// </summary>
      event EventHandler TestSelectionChanged;

      /// <summary>
      /// Called by the hosting control when a selection has been changed by a
      /// different TestSelectionProvider.
      /// </summary>
      /// <example>When the test hierarchy tab is used to modify the selected
      /// tests other tabs that provide capabilities to select tests need to be
      /// notified and updated. E.g. the simple test hierarchy list needs to
      /// update the selected (checkbox marked/unmarked) tests.</example>
      void OnSelectionChanged();
   }
}
