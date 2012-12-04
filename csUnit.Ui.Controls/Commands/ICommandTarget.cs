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
using System.Windows.Forms;

namespace csUnit.Ui.Controls.Commands {
   public interface IFileLoader {
   }
   
   /// <summary>
   /// ICommandTarget is implemented by objects on which commands can operate.
   /// The interface provides access to properties, methods, and events, which
   /// are required by the commands.
   /// </summary>
   /// <remarks>Known implementors:
   /// <list type="bullet">
#pragma warning disable 1574
   /// <item><see cref="csUnit.Runner.MainForm">csUnit.Runner.MainForm</see></item>
#pragma warning restore 1574
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
      /// Gets the MainMenuStrip for the command target.
      /// </summary>
      MenuStrip MainMenuStrip {
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
   }
}
