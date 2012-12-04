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
   using System.Collections.Generic;
   using System.Drawing;
   using System.IO;
   using System.Reflection;
   using System.Windows.Forms;

   using csUnit.Ui.Controls;
   using csUnit.Core;

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
      public static void CreateCommands(ICommandTarget commandTarget) {
         FindCommandsFor(commandTarget);
         FillMainMenu(commandTarget.Menu);
      }

      /// <summary>
      /// Fills the context menu.
      /// </summary>
      /// <param name="menu">The context menu to be filled.</param>
      /// <param name="type">The type the commands should operate on.</param>
      /// <remarks>The menu items for a context menu have to be created each
      /// time when a context menu is about to be displayed. When the context
      /// menu is destroyed all menu items it it are destroyed with it. 
      /// Therefore it's not possible to reuse the menu item that is created
      /// during startup for the main menu. [20Mar2006, ml]</remarks>
      public static void FillContextMenu(ContextMenu menu, UiElementInfo info) {
         // Phase 1: collect all commands interested in 'info'
         List<Command> contextCommands = new List<Command>();
         foreach(Command command in _commands) {
            command.RegisterForContextMenu(contextCommands, info);
         }

         contextCommands.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1.ContextMenuPosition.CompareTo(cmd2.ContextMenuPosition);
         });

         // Phase 2: create menu items for each interested command
         foreach(Command command in contextCommands) {
            if(command._insertLeadingSeparator) {
               menu.MenuItems.Add(new MenuItem("-"));
            }
            menu.MenuItems.Add(CreateMenuItemFor(command));
            menu.Popup += new EventHandler(command.OnUiUpdate);
         }
      }

      private static List<Command> FindCommandsWithButton() {
         List<Command> commandsWithButton = new List<Command>();
         foreach(Command command in _commands) {
            if(command.ToolBarPosition >= 0) {
               commandsWithButton.Add(command);
            }
         }
         return commandsWithButton;
      }

      /// <summary>
      /// Fills the toolbar.
      /// </summary>
      /// <param name="bar">The tool bar to be filled.</param>
      public static void FillToolBar(ToolBar bar) {
         List<Command> commandsWithButton = FindCommandsWithButton();
         int imageIndex = 0;

         commandsWithButton.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1.ToolBarPosition.CompareTo(cmd2.ToolBarPosition);
         });

         InitializeToolBar(bar);

         foreach(Command command in commandsWithButton) {
            if(command._insertLeadingSeparator) {
               ToolBarButton separator = new ToolBarButton("-");
               separator.Style = ToolBarButtonStyle.Separator;
               bar.Buttons.Add(separator);
            }

            bar.ImageList.Images.Add(command.Image);
            bar.Buttons.Add(CreateToolBarButton(imageIndex, command));
            imageIndex++;
         }
      }

      /// <summary>
      /// Gets a list of all existing commands.
      /// </summary>
      /// <remarks>The list is built during startup using reflection and 
      /// contains an instance of each non-abstract class that derives from 
      /// Command.</remarks>
      public static List<Command> Commands {
         get {
            return _commands;
         }
      }
      
      /// <summary>
      /// Gets the full name of the command.
      /// </summary>
      public string FullName {
         get {
            return _fullName;
         }
      }

      /// <summary>
      /// Executes the command.
      /// </summary>
      /// <param name="sender">The sender of the command.</param>
      /// <param name="args">The command arguments</param>
      public abstract void Execute(object sender, EventArgs args);
      
      #endregion // Public Members

      #region Protected Members
      /// <summary>
      /// Constructs a Command object. As Command is abstract only derived 
      /// non-abstract classes can be instantiated. They internally should 
      /// either call this constructor or the overloaded version which also
      /// supports the <code>insertLeadingSeparator</code> parameter.
      /// </summary>
      /// <param name="commandTarget">The commandTarget this command operates on.</param>
      /// <param name="menuName">The name of the menu to which the item is 
      /// added.</param>
      /// <param name="menuItemName">The name of the menu item.</param>
      /// <param name="position">The position where to add the menu item.</param>
      protected Command(ICommandTarget commandTarget, string menuName,
         string menuItemName, int position)
         : this(commandTarget, menuName, menuItemName, position, false) {
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
      protected Command(ICommandTarget commandTarget, string menuName,
         string menuItemName, int position, bool insertLeadingSeparator) {
         _commandTarget = commandTarget;
         _menuItemName = menuItemName;
         _menuName = menuName;
         _position = position;
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
      protected virtual Shortcut Shortcut {
         get {
            return Shortcut.None;
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
            bool sendEvent = _enabled != value;
            _enabled = value;
            if(sendEvent) {
               OnUiUpdate(null, null);
            }
         }
      }

      /// <summary>
      /// Gets/sets a value indicating whether a menuitem is checked.
      /// </summary>
      protected bool Checked {
         get {
            return _checked;
         }
         set {
            _checked = value;
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
      /// Gets the menu item the command has been associated with.
      /// </summary>
      protected MenuItem MenuItem {
         get {
            return _menuItem;
         }
      }

      /// <summary>
      /// Gets the toolbar button the command has been associated with.
      /// </summary>
      protected ToolBarButton ToolBarButton {
         get {
            return _toolBarButton;
         }
      }

      /// <summary>
      /// Gets the requested position of the command button in the toolbar. If
      /// the command does not have a button, the value -1 is returned.
      /// </summary>
      /// <remarks>Derived classes must override this property if they have a
      /// button.</remarks>
      protected virtual int ToolBarPosition {
         get {
            return -1;
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
            return "";
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
         if(_menuItem != null) {
            _menuItem.Enabled = _enabled;
         }
         if(_toolBarButton != null) {
            bool invokeRequired = _toolBarButton.Parent.InvokeRequired;
            if(invokeRequired) {
               _toolBarButton.Parent.Invoke(new PropertySetterDelegate(EnabledSetter),
                  new object[] { _toolBarButton, _enabled });
            }
            else {
               EnabledSetter(_toolBarButton, _enabled);
            }
         }
      }
      #endregion // Protected Members

      #region Private Members
      /// <summary>
      /// Iterates over all menus in the menu bar and dynamically creates all
      /// menu items.
      /// </summary>
      /// <param name="mainMenu">The MainMenu for which to create the menu items.</param>
      private static void FillMainMenu(MainMenu mainMenu) {
         foreach(MenuItem subMenu in mainMenu.MenuItems) {
            // Phase 1: collect all commands for submenu
            List<Command> orderedCommandList = OrderedCommandListFor(subMenu.Text);

            // Phase 2: create a menu item for each command in the submenu
            foreach(Command command in orderedCommandList) {
               if(command._insertLeadingSeparator) {
                  subMenu.MenuItems.Add(new MenuItem("-"));
               }
               command._menuItem = CreateMenuItemFor(command);
               subMenu.MenuItems.Add(command._menuItem);
               subMenu.Popup += new EventHandler(command.OnUiUpdate);
            }
         }
      }

      /// <summary>
      /// Creates a menu item for a command. This method can be used for both
      /// the main menu and a context menu.
      /// </summary>
      /// <param name="command">The command for which to create menu item.</param>
      private static MenuItem CreateMenuItemFor(Command command) {
         MenuItem menuItem = new MenuItem(command._menuItemName, 
            new EventHandler(command.Execute));
         menuItem.Shortcut = command.Shortcut;
         menuItem.Enabled = command.Enabled;
         menuItem.Checked = command.Checked;
         return menuItem;
      }

      /// <summary>
      /// Creates a toolbar button for a command.
      /// </summary>
      /// <param name="imageIndex"></param>
      /// <param name="command"></param>
      /// <returns>A toolbar button.</returns>
      private static ToolBarButton CreateToolBarButton(int imageIndex, Command command) {
         ToolBarButton button = new ToolBarButton();
         // Don't set button.Text as this will create text labels just below
         // each button. [22Mar2006, ml]
         button.ToolTipText = command.ToolTipText;
         button.ImageIndex = imageIndex;
         button.Tag = command;
         button.Enabled = command.Enabled;
         command._toolBarButton = button;
         return button;
      }

      /// <summary>
      /// Sets the default stuff, e.g. image list, for the toolbar.
      /// </summary>
      /// <param name="bar">The toolbar to initialize.</param>
      private static void InitializeToolBar(ToolBar bar) {
         bar.ImageList = new ImageList();
         bar.ButtonClick += new ToolBarButtonClickEventHandler(ToolBarHandler);
         bar.ImageList.TransparentColor = Color.FromArgb(255, 0, 255);
         bar.ImageList.ImageSize = new Size(16, 16);
      }
      
      /// <summary>
      /// This method walks through all registered commands and determines
      /// whether the command wants to be displayed in a menu with a given
      /// name, e.g. for the "File" menu or for the "View" menu. The commands
      /// are collected in a list and before returning the method sorts the
      /// list by ascending _position.
      /// </summary>
      /// <param name="menuName">The name of the menu.</param>
      /// <returns>An ordered list of commands for the menu.</returns>
      private static List<Command> OrderedCommandListFor(string menuName) {
         List<Command> commands = new List<Command>();
         foreach(Command command in _commands) {
            if(command._menuName == menuName) {
               commands.Add(command);
            }
         }
         commands.Sort(delegate(Command cmd1, Command cmd2) {
            return cmd1._position.CompareTo(cmd2._position);
         });
         return commands;
      }

      /// <summary>
      /// Scan assembly for all classes that derive from class Command.
      /// </summary>
      /// <param name="commandTarget">The target the commands operate on.</param>
      private static void FindCommandsFor(ICommandTarget commandTarget) {
         Assembly a = Assembly.GetAssembly(typeof(csUnit.Runner.Commands.Command));

         foreach(Type t in a.GetTypes()) {
            if(t.IsSubclassOf(typeof(csUnit.Runner.Commands.Command))
               && !t.IsAbstract) {
               ConstructorInfo ci = t.GetConstructor(new Type[] { typeof(ICommandTarget) });
               try {
                  _commands.Add((Command)ci.Invoke(new object[] { commandTarget }));
               }
               catch(Exception ex) {
                  Console.WriteLine("Invocation exception occured while constructing command: " 
                     + t.ToString());
                  Console.WriteLine(ex.ToString());
               }
            }
         }
      }

      /// <summary>
      /// An event handler for toolbar buttons, when they are clicked. The event
      /// is simply forwarded to the associated command.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private static void ToolBarHandler(Object sender, ToolBarButtonClickEventArgs e) {
         Command cmd = e.Button.Tag as Command;
         if(cmd != null) {
            cmd.Execute(sender, e);
         }
      }

      private void CreateCommandFullName(string menuName, string menuItemName) {
         _fullName = menuName + "." + menuItemName;
         _fullName = _fullName.Replace("&", "");
         _fullName = _fullName.Replace("...", "");
         _fullName = _fullName.Replace(" ", "");
      }

      /// <summary>
      /// Used when InvokeRequired for a toolbar button.
      /// </summary>
      /// <param name="button"></param>
      /// <param name="value"></param>
      private void EnabledSetter(ToolBarButton button, bool value) {
         button.Enabled = value;
      }

      private delegate void PropertySetterDelegate(ToolBarButton target, bool value);

      /// <summary>
      /// Typed list of all commands filled during startup using reflection.
      /// </summary>
      private static List<Command> _commands = new List<Command>();

      /// <summary>
      /// A flag indicating whether a separator should be added immediately
      /// before the menu item associated with the command.
      /// </summary>
      private bool _insertLeadingSeparator = false;

      /// <summary>
      /// A flag indicating whether the menu item for the command should be
      /// checked.
      /// </summary>
      /// <remarks>This looks like a duplication of the flag that the associated
      /// MenuItem object provides. However, the MenuItem object is created later
      /// in the case of context menus much, much later, and also multiple times.
      /// </remarks>
      private bool _checked = false;

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
      private ICommandTarget _commandTarget = null;

      /// <summary>
      /// The text to be used in the menu item. For cues an ampersand may be
      /// included.
      /// </summary>
      private string _menuItemName = string.Empty;

      /// <summary>
      /// The name of the menu in which the menu item for the command should
      /// be added.
      /// </summary>
      private string _menuName;

      /// <summary>
      /// The desired position where the menu item should appear in the menu.
      /// If this is a duplicate of a different command, then the sequence is
      /// not defined and may vary depending on the reflection implementation.
      /// </summary>
      private int _position;

      /// <summary>
      /// The full name of the command consisting of namespace name and class
      /// name plus the command name.
      /// </summary>
      private string _fullName = string.Empty;

      /// <summary>
      /// The menu item for the command if any. To be used in the main menu
      /// only.
      /// </summary>
      /// <remarks>MenuItem objects for context menus are created in addition
      /// to this and on request only.</remarks>
      private MenuItem _menuItem = null;

      /// <summary>
      /// The toolbar button for the command if any. To be used in the toolbar.
      /// </summary>
      private ToolBarButton _toolBarButton = null;
      #endregion // Private Members
   }
}
