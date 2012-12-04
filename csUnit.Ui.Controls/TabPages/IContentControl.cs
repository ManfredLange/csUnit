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

using System.Collections.Generic;
using System.Windows.Forms;

namespace csUnit.Ui.Controls.TabPages {
   internal delegate void ContentControlEventHandler(IContentControl sender, ContentControlEventArgs args);

   internal interface IContentControl {
      /// <summary>
      /// Fired when the user has changed the selected item in the control, e.g.
      /// by clicking on an item.
      /// </summary>
      event TreeViewEventHandler AfterSelect;
      // TODO: This should be a more generic type of event. [24Feb07, ml]

      /// <summary>
      /// Fired when a checkmark is set or cleared for an item.
      /// </summary>
      event ContentControlEventHandler ItemCheckedStateChanged;

      /// <summary>
      /// Raised when the user right-clicks into the control and needs a context
      /// menu.
      /// </summary>
      event CsUnitControlEventHandler FillContextMenu;

      /// <summary>
      /// Gets the human readable name of the control. Used for the tab in the
      /// tab control.
      /// </summary>
      string Name {
         get;
      }

      /// <summary>
      /// Gets the text to be displayed upon mouse over on the tab.
      /// </summary>
      string ToolTipText {
         get;
      }

      /// <summary>
      /// Gets an integer value indicating the desired position on the tab
      /// control. The lower the higher.
      /// </summary>
      /// <remarks>All tabs will be initially sorted by this number. If two tabs
      /// request the same position the order depends on the order in which the
      /// reflection interface returns the types implementing the 
      /// IContentControl interface.</remarks>
      int DesiredTabPosition {
         get;
      }

      /// <summary>
      /// Gets a list of locations where the searchTerm was found.
      /// </summary>
      /// <param name="searchTerm">The sequence to search for.</param>
      List<FindLocation> Find(string searchTerm);

      /// <summary>
      /// Called when the csUnitControl's collection of check items has changed.
      /// </summary>
      /// <param name="checkedItems"></param>
      void UpdateCheckedItems(UiElementInfoCollection checkedItems);

      /// <summary>
      /// Navigate to the given findLocation.
      /// </summary>
      /// <param name="findLocation"></param>
      void NavigateTo(FindLocation findLocation);
   }
}
