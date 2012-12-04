////////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2006 by Manfred Lange, Markus Renschler, Jake Anderson, 
//                          and Piers Lawson. All rights reserved.
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
//    product, the following acknowledgment must be included in the product 
//    documentation:
// 
//       Portions Copyright © 2002-2006 by Manfred Lange, Markus Renschler, 
//       Jake Anderson, and Piers Lawson. All rights reserved.
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////

namespace csUnit.Runner.Commands {
   using System;
   using System.IO;
   using System.Windows.Forms;
   
   using csUnit.Ui.Controls;
   using csUnit.Core;
   
   /// <summary>
   /// ICommandTarget is implemented by objects on which commands can operate.
   /// The interface provides access to properties, methods, and events, which
   /// are required by the commands.
   /// </summary>
   /// <remarks>Known implementors:
   /// <list type="bullet">
   /// <item><see cref="csUnit.Runner.MainForm">csUnit.Runner.MainForm</see></item>
   /// </list>
   /// </remarks>
   public interface ICommandTarget {
      /// <summary>
      /// Returns the currently displayed cursor of the command target.
      /// </summary>
      Cursor Cursor {
         set;
      }

      /// <summary>
      /// Gets/sets a value indicating whether to display the toolbar.
      /// </summary>
      bool ToolBarVisible {
         get;
         set;
      }

      /// <summary>
      /// Gets the FileLoader object for the command target.
      /// </summary>
      IFileLoader FileLoader {
         get;
      }

      /// <summary>
      /// Gets the MainMenu for the command target.
      /// </summary>
      MainMenu Menu {
         get;
      }

      /// <summary>
      /// Gets/sets a value indicating whether the status bar is visible.
      /// </summary>
      bool StatusBarVisible {
         get;
         set;
      }

      /// <summary>
      /// A window to use for the owner of the command target.
      /// </summary>
      IWin32Window OwnerWindow {
         get;
      }

      /// <summary>
      /// Gets a string with the selected test category.
      /// </summary>
      string SelectedTestCategory {
         get;
      }

      /// <summary>
      /// Gets a string with the selected fixture category.
      /// </summary>
      string SelectedFixtureCategory {
         get;
      }

      /// <summary>
      /// Gets a collection of selected items.
      /// </summary>
      SelectedItemsCollection SelectedItems {
         get;
      }

      /// <summary>
      /// Gets a collection containing all checked items. If the UI does not
      /// provide the ability to check/uncheck items, then the collection is
      /// empty.
      /// </summary>
      /// <remarks>The collection can be empty if the tests are displayed in
      /// an ongoing list instead of a hierarchy tree.</remarks>
      CheckedItemsCollection CheckedItems {
         get;
      }

      /// <summary>
      /// Sets the text of the status bar of the command target.
      /// </summary>
      String Status {
         set;
      }

      /// <summary>
      /// Determines whether the current recipe has been modified and whether
      /// the user wants to save it.
      /// </summary>
      /// <returns>True, if the recipe has been saved, or if the user decided
      /// to discard the modifications. False otherwise.</returns>
      bool AskSaveModifiedRecipe();

      /// <summary>
      /// Executes a command.
      /// </summary>
      /// <param name="commandName"></param>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      /// <remarks>This method seems to be odd. I would assume that the
      /// implementation would be found in the Command base class. However,
      /// this might be related to the fact that the csUnit control might
      /// contribute commands as well.</remarks>
      void ExecuteCommand(string commandName, object sender, EventArgs args);
   }
}
