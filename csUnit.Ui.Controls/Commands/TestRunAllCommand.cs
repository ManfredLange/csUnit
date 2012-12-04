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
using System.Drawing;
using System.Windows.Forms;

using csUnit.Core;
using csUnit.Core.Criteria;
using csUnit.Interfaces;

namespace csUnit.Ui.Controls.Commands {
   /// <summary>
   /// Summary description for TestRunAllCommand.
   /// </summary>
   public class TestRunAllCommand : Command {

      private const int MenuPosition    = 1;
      private const int ContextPosition = 1;
      private const int DesiredToolBarPosition = 6;

      public TestRunAllCommand(ICommandTarget commandTarget, CsUnitControl csUnitCtrl)
         : base(commandTarget, csUnitCtrl, "&Test", "&Run All", MenuPosition, true) {
         _onRecipeLoaded = OnRecipeLoaded;
         _onRecipeClosing = OnRecipeClosing;
         _onOtherRecipeEvent = OnOtherRecipeEvent;
         _onAssemblyEvent = OnAssemblyEvent;
         
         RecipeFactory.Loaded += _onRecipeLoaded;
         RecipeFactory.Closing += _onRecipeClosing;
         HookupRecipe();
         UpdateEnabledStatus();
      }

      protected override Keys ShortcutKeys {
         get {
            return Keys.F5;
         }
      }

      protected override int ContextMenuPosition {
         get {
            return ContextPosition;
         }
      }

      protected override void RegisterForContextMenu(List<Command> contextCommands,
         UiElementInfo info) {
         contextCommands.Add(this);
      }

      #region ToolBarButton Properties
      protected override Image Image {
         get {
            return new Bitmap(GetType(), "Images.TestRunAllCommand.png");
         }
      }

      protected override int ToolBarPosition {
         get {
            return DesiredToolBarPosition;
         }
      }

      protected override string ToolTipText {
         get {
            return "Run all tests";
         }
      }
      #endregion // ToolBarButton Properties

      protected override void Execute(object sender, EventArgs args) {
         if(RecipeFactory.Current != null) {
            if(!RecipeFactory.Current.TestsRunning) {
               ITestRun selection = new TestRun(new AllTestsCriterion());
               RecipeFactory.Current.RunTests(selection);
            }
         }
      }

      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         UpdateEnabledStatus();
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
         UpdateEnabledStatus();
      }

      private void HookupRecipe() {
         if(RecipeFactory.Current != null) {
            RecipeFactory.Current.AssemblyAdded += _onAssemblyEvent;
            RecipeFactory.Current.AssemblyRemoved += _onAssemblyEvent;

            RecipeFactory.Current.Started += _onOtherRecipeEvent;
            RecipeFactory.Current.Finished += _onOtherRecipeEvent;
            RecipeFactory.Current.Aborted += _onOtherRecipeEvent;
         }
      }

      private void UnhookRecipe() {
         RecipeFactory.Current.AssemblyAdded -= _onAssemblyEvent;
         RecipeFactory.Current.AssemblyRemoved -= _onAssemblyEvent;

         RecipeFactory.Current.Started -= _onOtherRecipeEvent;
         RecipeFactory.Current.Finished -= _onOtherRecipeEvent;
         RecipeFactory.Current.Aborted -= _onOtherRecipeEvent;
      }

      private void OnOtherRecipeEvent(object sender, RecipeEventArgs args) {
         UpdateEnabledStatus();
      }

      private void OnAssemblyEvent(object sender, AssemblyEventArgs args) {
         UpdateEnabledStatus();
      }

      private void UpdateEnabledStatus() {
         if(RecipeFactory.Current != null
            && !RecipeFactory.Current.TestsRunning
            && RecipeFactory.Current.AssemblyCount > 0) {
            Enabled = true;
         }
         else {
            Enabled = false;
         }
      }

      private readonly RecipeEventHandler _onRecipeLoaded;
      private readonly RecipeEventHandler _onRecipeClosing;
      private readonly RecipeEventHandler _onOtherRecipeEvent;

      private readonly AssemblyEventHandler _onAssemblyEvent;
   }
}
