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
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace csUnit.Ui.Controls.Commands {
   /// <summary>
   /// Abstract base class for an implementation of the Command design pattern.
   /// (See GOF book).
   /// </summary>
   public abstract class Command {
      #region Public Members
      /// <summary>
      /// Using reflection this static method creates all commands which can be
      /// found in the assembly in which class csUnit.Runner.Commands.Command is
      /// defined.
      /// </summary>
      /// <remarks>First this method scans the assembly in which class Command is
      /// defined. In step two, when all commands have been found, the commands
      /// are then used to fill the menus.</remarks>
      /// <param name="commandTarget">The commandTarget the command operates 
      /// on.</param>
      /// <param name="csUnitCtrl">The CsUnitControl the command operates on.
      /// </param>
      public static void CreateCommands(ICommandTarget commandTarget,
         CsUnitControl csUnitCtrl) {
         FindCommandsFor(commandTarget, csUnitCtrl);
         FillMainMenuStrip(commandTarget.MainMenuStrip);
      }

      /// <summary>
      /// Fills the context menu.
      /// </summary>
      /// <param name="menu">The context menu to be filled.</param>
      /// <param name="info">The UI element the commands should operate on.</param>
      /// <remarks>The menu items for a context menu have to be created each
      /// time when a context menu is about to be displayed. When the context
      /// menu is destroyed all menu items it it are destroyed with it. 
      /// Therefore it's not possible to reuse the menu item that is created
      /// during startup for the main menu. [20Mar2006, ml]</remarks>
      public static void FillContextMenu(ContextMenuStrip menu, UiElementInfo info) {
         // Phase 1: collect all commands interested in 'info'
         List<Command> contextCommands = new List<Command>();
         foreach(Command command in Commands) {
            command.RegisterForContextMenu(contextCommands, info);
         }

         contextCommands.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1.ContextMenuPosition.CompareTo(cmd2.ContextMenuPosition);
         });

         // Phase 2: create menu items for each interested command
         foreach(Command command in contextCommands) {
            if(   command._insertLeadingSeparator
               && menu.Items.Count > 0 ) {
               menu.Items.Add("-");
            }
            menu.Items.Add(CreateToolStripMenuItem(command));
         }
      }

      private static List<Command> FindCommandsWithButton() {
         List<Command> commandsWithButton = new List<Command>();
         foreach(Command command in Commands) {
            if(command.ToolBarPosition >= 0) {
               commandsWithButton.Add(command);
            }
         }
         return commandsWithButton;
      }

      /// <summary>
      /// Fills a toolstrip with all items for commands which support a button
      /// on a toolstrip.
      /// </summary>
      /// <param name="strip">The toolstrip to fill.</param>
      public static void FillToolStrip(ToolStrip strip) {
         List<Command> commandsWithButton = FindCommandsWithButton();

         commandsWithButton.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1.ToolBarPosition.CompareTo(cmd2.ToolBarPosition);
         });

         foreach(Command command in commandsWithButton) {
            if(command._insertLeadingSeparator
               && strip.Items.Count > 0) {
               strip.Items.Add(new ToolStripSeparator());
            }
            strip.Items.Add(CreateToolStripButton(command));
         }
      }

      /// <summary>
      /// Creates a toolstrip button for a command.
      /// </summary>
      /// <param name="cmd">The command for which to create a toolstrip button.</param>
      /// <returns>A toolstrip button.</returns>
      private static ToolStripButton CreateToolStripButton(Command cmd) {
         cmd._toolStripButton = new ToolStripButton(cmd.Name, cmd.Image);
         cmd._toolStripButton.Click += cmd.Execute;
         cmd._toolStripButton.Tag = cmd;
         cmd._toolStripButton.Image = cmd.Image;
         cmd._toolStripButton.Enabled = cmd.Enabled;
         cmd._toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
         cmd._toolStripButton.ToolTipText = cmd.ToolTipText + TranslateShortCut(cmd);
         return cmd._toolStripButton;
      }

      /// <summary>
      /// Creates a toolstrip button for a command.
      /// </summary>
      /// <param name="cmd">The command for which to create a toolstrip button.</param>
      /// <returns>A toolstrip button.</returns>
      private static ToolStripMenuItem CreateToolStripMenuItem(Command cmd) {
         ToolStripMenuItem menuItem;
         menuItem = new ToolStripMenuItem(cmd.Name, cmd.Image);
         menuItem.Click += cmd.Execute;
         menuItem.Tag = cmd;
         menuItem.Image = cmd.Image;
         menuItem.Enabled = cmd.Enabled;
         menuItem.Checked = cmd.Checked;
         menuItem.ShortcutKeys = cmd.ShortcutKeys;
         menuItem.ToolTipText = cmd.ToolTipText + TranslateShortCut(cmd);
         return menuItem;
      }

      /// <summary>
      /// Executes a named command. If the command is not found it displays a
      /// friendly message in a dialog box.
      /// </summary>
      /// <param name="commandType">The type of the command.</param>
      /// <param name="sender">The sender of the event.</param>
      /// <param name="args">The event parameters.</param>
      public static void ExecuteCommand(Type commandType, object sender, EventArgs args) {
         foreach(Command c in Commands) {
            if(c.GetType().Equals(commandType)) {
               c.Execute(sender, args);
               return;
            }
         }
         MessageBox.Show(FindMainWindow(), "Couldn't find the command \'" + commandType.FullName
            + "\'", "Warning - csUnit", MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
      }

      /// <summary>
      /// Finds the main window of the current process and returns an 
      /// IWin32Window interface reference.
      /// </summary>
      /// <returns>IWin32Window reference</returns>
      /// <remarks>The method is typically used for cases where a command needs
      /// an owner window for displaying a modal dialog box.</remarks>
      public static IWin32Window FindMainWindow() {
         IntPtr windowHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
         return Control.FromHandle(windowHandle);
      }

      /// <summary>
      /// Executes the command.
      /// </summary>
      /// <param name="sender">The sender of the command.</param>
      /// <param name="args">The command arguments</param>
      protected abstract void Execute(object sender, EventArgs args);
      
      #endregion // Public Members

      #region Protected Members
      /// <summary>
      /// Constructs a Command object. As Command is abstract only derived 
      /// non-abstract classes can be instantiated. They internally should 
      /// either call this constructor or the overloaded version which also
      /// supports the <code>insertLeadingSeparator</code> parameter.
      /// </summary>
      /// <param name="commandTarget">The commandTarget this command operates on.</param>
      /// <param name="csUnitCtrl">The CsUnitControl to operate on.</param>
      /// <param name="menuName">The name of the menu to which the item is 
      /// added.</param>
      /// <param name="menuItemName">The name of the menu item.</param>
      /// <param name="position">The position where to add the menu item.</param>
      protected Command(ICommandTarget commandTarget, CsUnitControl csUnitCtrl,
         string menuName, string menuItemName, int position)
         : this(commandTarget, csUnitCtrl, menuName, menuItemName, position, false) {
      }

      /// <summary>
      /// Constructs a Command object. As Command is abstract only derived 
      /// non-abstract classes can be instantiated. They internally should 
      /// either call this constructor or the overloaded version which does not
      /// require the <code>insertLeadingSeparator</code> parameter.
      /// <param name="commandTarget">The commandTarget this command acts upon.</param>
      /// <param name="menuName">The name of the menu to which the item is 
      /// added.</param>
      /// <param name="menuItemName">The name of the menu item.</param>
      /// <param name="position">The position where to add the menu item.</param>
      /// <param name="insertLeadingSeparator">If 'true' a separator will be added just 
      /// before the menu item.</param>
      /// <remarks>The 'position' parameter is only the requested position. If
      /// a different command requests the same position then the implementation
      /// of the reflection functionality determines the actual order of the
      /// commands.</remarks>
      protected Command(ICommandTarget commandTarget, CsUnitControl csUnitCtrl, 
         string menuName, string menuItemName, int position, bool insertLeadingSeparator) {
         _commandTarget = commandTarget;
         _csUnitCtrl = csUnitCtrl;
         _menuItemName = menuItemName;
         _menuName = menuName;
         _desiredMenuPosition = position;
         _insertLeadingSeparator = insertLeadingSeparator;

         CreateCommandFullName(menuName, menuItemName);
      }

      /// <summary>
      /// Gets the position of the command's menu item in a context menu.
      /// </summary>
      protected virtual int ContextMenuPosition {
         get {
            return 0;
         }
      }

      /// <summary>
      /// Gets the shortcut for the command.
      /// </summary>
      /// <remarks>Commands that support a shortcut should override this
      /// member. Duplicate shortcuts can be defined, but the behavior is then
      /// undefined.</remarks>
      protected virtual Keys ShortcutKeys {
         get {
            return Keys.None;
         }
      }

      /// <summary>
      /// Gets the name of the command.
      /// </summary>
      protected string Name {
         get {
            return _menuItemName;
         }
      }

      /// <summary>
      /// Gets/sets a value indicating whether the menuitem is enabled.
      /// </summary>
      protected bool Enabled {
         get {
            return _enabled;
         }
         set {
            _enabled = value;
            Update(_menuStripItem);
            Update(_toolStripButton);
         }
      }

      private void Update(ToolStripItem item) {
         if(item != null) {
            if(item.Owner.InvokeRequired) {
               item.Owner.Invoke(new UpdateDelegate(Update), new object[] { item });
            }
            else {
               item.Enabled = _enabled;
            }
         }
      }

      private delegate void UpdateDelegate(ToolStripItem item);

      /// <summary>
      /// Gets/sets a value indicating whether a menuitem is checked.
      /// </summary>
      protected bool Checked {
         get {
            return _checked;
         }
         set {
            if( _checked != value ) {
               _checked = value;
               if( _menuStripItem != null ) {
                  _menuStripItem.Checked = _checked;
               }
               if( _toolStripButton != null ) {
                  _toolStripButton.Checked = _checked;
               }
            }
         }
      }

      /// <summary>
      /// Gets the commandTarget on which this command can be applied.
      /// </summary>
      protected ICommandTarget CommandTarget {
         get {
            return _commandTarget;
         }
      }

      /// <summary>
      /// Gets the CsUnitControl on which this command can operate.
      /// </summary>
      protected CsUnitControl CsUnitControl {
         get {
            return _csUnitCtrl;
         }
      }

      /// <summary>
      /// Gets the ToolStripMenuItem the command has been associated with.
      /// </summary>
      protected ToolStripMenuItem ToolStripMenuItem {
         get {
            return _menuStripItem;
         }
      }

      /// <summary>
      /// Gets the requested position of the command button in the toolbar. The
      /// default implementation returns -1, which means the command doesn't 
      /// have a toolbar button.
      /// </summary>
      /// <remarks>Derived classes must override this property if they have a
      /// button.</remarks>
      protected virtual int ToolBarPosition {
         get {
            return NoMenuEntry;
         }
      }

      /// <summary>
      /// Returns the image for the command button in the toolbar. If the 
      /// command does not have a button <code>null</code> will be returned.
      /// </summary>
      /// <remarks>Derived classes must override this property if they have a
      /// button.</remarks>
      protected virtual Image Image {
         get {
            return null;
         }
      }

      /// <summary>
      /// Returns the text for the tooltip of a command button in the toolbar.
      /// If the command does not provide a button, an empty string is returned.
      /// </summary>
      /// <remarks>Derived classes must override this property if they have a
      /// button.</remarks>
      protected virtual string ToolTipText {
         get {
            return string.Empty;
         }
      }

      /// <summary>
      /// When implemented by a command, it can determine whether it wants its
      /// menu item to appear in a context menu depending on the UiElementInfo.
      /// </summary>
      /// <param name="contextCommands">A list to which the command can add itself.</param>
      /// <param name="info">Information about the element the user is pointing to.</param>
      /// <remarks>The default implementation does nothing.</remarks>
      protected virtual void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
      }

      /// <summary>
      /// Called, when the command should update its associated user interface
      /// elements.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      /// <remarks>An example for an action could be that a command has button
      /// associated with it. Depending on some rules the command determines,
      /// whether the button should be enabled, or not. A different command
      /// may choose to switch the text of its menu item.</remarks>
      protected virtual void OnUiUpdate(object sender, EventArgs args) {
      }

      #endregion // Protected Members

      #region Private Members
      /// <summary>
      /// Iterates over all menus in the menu bar and dynamically creates all
      /// menu items.
      /// </summary>
      /// <param name="mainMenu">The MainMenu for which to create the menu items.</param>
      private static void FillMainMenuStrip(ToolStrip mainMenu) {
         if(null != mainMenu) {
            foreach(ToolStripMenuItem subMenu in mainMenu.Items) {
               // Phase 1: collect all commands for submenu
               List<Command> orderedCommandList = OrderedCommandListFor(subMenu.Text);

               // Phase 2: create a menu item for each command in the submenu
               foreach(Command command in orderedCommandList) {
                  if(command._insertLeadingSeparator
                     && subMenu.DropDownItems.Count > 0) {
                     subMenu.DropDownItems.Add("-");
                  }
                  command._menuStripItem = CreateToolStripMenuItem(command);
                  subMenu.DropDownItems.Add(command._menuStripItem);
                  subMenu.DropDownOpening += command.OnUiUpdate;
               }
            }
         }
      }

      /// <summary>
      /// Takes a command and creates a 'translated' short cut, e.g. 'F5' will
      /// be translated to ' (F5)'. The purpose is a mere UI related one. Users
      /// should find it easier to identify what exactly the short cut for a
      /// command is.
      /// </summary>
      /// <param name="command">Command for which to create a translated shortcut.</param>
      /// <returns>A human readable version of the shortcut to be 
      /// appended to a tooltip.</returns>
      private static string TranslateShortCut(Command command) {
         string shortCut = string.Empty;
         if(command.ShortcutKeys != Keys.None) {
            KeysConverter conv = new KeysConverter();
            shortCut = conv.ConvertToString(command.ShortcutKeys);
            shortCut = String.Format(" ({0})", shortCut);
         }
         return shortCut;
      }
      
      /// <summary>
      /// This method walks through all registered commands and determines
      /// whether the command wants to be displayed in a menu with a given
      /// name, e.g. for the "File" menu or for the "View" menu. The commands
      /// are collected in a list and before returning the method sorts the
      /// list by ascending _desiredMenuPosition.
      /// </summary>
      /// <param name="menuName">The name of the menu.</param>
      /// <returns>An ordered list of commands for the menu.</returns>
      private static List<Command> OrderedCommandListFor(string menuName) {
         List<Command> commands = new List<Command>();
         foreach(Command command in Commands) {
            if(command._menuName == menuName
               && command._desiredMenuPosition != NoMenuEntry ) {
               commands.Add(command);
            }
         }
         commands.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1._desiredMenuPosition.CompareTo(cmd2._desiredMenuPosition);
         });
         return commands;
      }

      /// <summary>
      /// Scan assembly for all classes that derive from class Command.
      /// </summary>
      /// <param name="commandTarget">The target the commands operate on.</param>
      /// <param name="csUnitCtrl">The CsUnitControl the commands operate on.</param>
      private static void FindCommandsFor(ICommandTarget commandTarget,
         CsUnitControl csUnitCtrl) {
         List<Assembly> assembliesToBeSearched = CreateListOfAssembliesToBeSearched(commandTarget);

         foreach( Assembly a in assembliesToBeSearched ) {
            foreach( Type t in a.GetTypes() ) {
               if( t.IsSubclassOf(typeof(Command))
                  && !t.IsAbstract ) {
                  ConstructorInfo ci = t.GetConstructor(new[] { typeof(ICommandTarget), typeof(CsUnitControl) });
                  try {
                     Commands.Add((Command) ci.Invoke(new object[] { commandTarget, csUnitCtrl }));
                  }
                  catch( Exception ex ) {
                     System.Diagnostics.Debug.WriteLine("Invocation exception occured while constructing command: "
                        + t);
                     System.Diagnostics.Debug.WriteLine(ex.ToString());
                  }
               }
            }
         }
      }

      /// <summary>
      /// Creates a list of all assemblies which will be searched for commands
      /// to be added to the command collection.
      /// </summary>
      /// <param name="obj">An object of which the implementing assembly will be
      /// added to the search collection as well.</param>
      /// <returns>A list of assemblies to search. The list contains at least
      /// one element.</returns>
      private static List<Assembly> CreateListOfAssembliesToBeSearched(object obj) {
         List<Assembly> assemblies = new List<Assembly>();
         assemblies.Add(Assembly.GetAssembly(typeof(Command)));
         assemblies.Add(Assembly.GetAssembly(obj.GetType()));
         return assemblies;
      }

      private void CreateCommandFullName(string menuName, string menuItemName) {
         System.Text.StringBuilder fullName = new System.Text.StringBuilder(menuName);
         fullName.Append(".").Append(menuItemName);
         fullName.Replace("&", string.Empty);
         fullName.Replace("...", string.Empty);
         fullName.Replace(" ", string.Empty);
         fullName.ToString();
      }

      /// <summary>
      /// Typed list of all commands filled during startup using reflection.
      /// </summary>
      private static readonly List<Command> Commands = new List<Command>();

      /// <summary>
      /// A flag indicating whether a separator should be added immediately
      /// before the menu item associated with the command.
      /// </summary>
      private readonly bool _insertLeadingSeparator;

      /// <summary>
      /// A flag indicating whether the menu item for the command should be
      /// checked.
      /// </summary>
      /// <remarks>This looks like a duplication of the flag that the associated
      /// MenuItem object provides. However, the MenuItem object is created later
      /// in the case of context menus much, much later, and also multiple times.
      /// </remarks>
      private bool _checked;

      /// <summary>
      /// A flag indicating whether the menu item for the command should be
      /// enabled (true) or disabled (false).
      /// </summary>
      /// <remarks>This looks like a duplication of the flag that the associated
      /// MenuItem object provides. However, the MenuItem object is created later
      /// in the case of context menus much, much later, and also multiple times.
      /// </remarks>
      private bool _enabled = true;

      /// <summary>
      /// The command target this command can operate on.
      /// </summary>
      private readonly ICommandTarget _commandTarget;

      /// <summary>
      /// The CsUnitControl this command can operate on.
      /// </summary>
      private readonly CsUnitControl _csUnitCtrl;

      /// <summary>
      /// The text to be used in the menu item. For cues an ampersand may be
      /// included.
      /// </summary>
      private readonly string _menuItemName = string.Empty;

      /// <summary>
      /// The name of the menu in which the menu item for the command should
      /// be added.
      /// </summary>
      private readonly string _menuName;

      /// <summary>
      /// The desired position where the menu item should appear in the menu.
      /// If this is a duplicate of a different command, then the sequence is
      /// not defined and may vary depending on the reflection implementation.
      /// </summary>
      private readonly int _desiredMenuPosition;

      /// <summary>
      /// The menu item for the command if any. To be used in the main menu
      /// only.
      /// </summary>
      /// <remarks>ToolStripMenuItem objects for context menus are created in
      /// addition to this and on request only. The reason for this is that when
      /// the context menu goes out of scope the associated ToolStripMenuItems 
      /// are destroyed as well. Behavior would not be desirable for the main 
      /// menu.</remarks>
      private ToolStripMenuItem _menuStripItem;

      /// <summary>
      /// The toolstrip button for the command if any. To be used in the 
      /// toolbar.
      /// </summary>
      /// <remarks>If the command is not represented by a button, this member
      /// is null.</remarks>
      private ToolStripButton _toolStripButton;

      protected const int NoMenuEntry = -1;

      #endregion // Private Members
   }
}
