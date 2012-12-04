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

// The following command line opens the devenv, opens the source file and
// positions the cursor at the given line:
//
// devenv <sourceFilePathName> /command "Edit.Goto 29"
// 
// where <sourceFilePathName> specifies the source file name and full path
// of the source file. This name and also the line number (here 29) can
// be retrieved from the information given in the exception message.
// ml.

using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using csUnit.Core;
using csUnit.Core.Criteria;
using csUnit.Interfaces;
using csUnit.Runner.Commands;
using csUnit.Ui.Controls;
using csUnit.Ui.Controls.Commands;

namespace csUnit.Runner {
   /// <summary>
   /// Summary description for MainForm.
   /// </summary>
   public class MainForm : Form, ICommandTarget {
      /// <summary>
      /// Default constructor.
      /// </summary>
// ReSharper disable MemberCanBeInternal
      public MainForm(CmdLineHandler clh) {
// ReSharper restore MemberCanBeInternal
         // Required for Windows Form Designer support. Must be first call in
         // this method.
         InitializeComponent();

         // Other intialization
         //

         // Create event handlers that are used to monitor testing results
         //
         _testStarted = OnTestStarted;
         _testFailed = OnTestFailed;
         _testError = OnTestError;
         _testSkipped = OnTestSkipped;
         _testPassed = OnTestPassed;
         _recipeStarted = OnRecipeStarted;
         _recipeFinished = OnRecipeFinished;
         _recipeSaved = OnRecipeSaved;
         _recipeAborted = OnRecipeAborted;
         _assemblyAdded = OnAssemblyAdded;
         _assemblyRemoving = OnAssemblyRemoving;
         _assemblyChanged = OnAssemblyChanged;

         RecipeFactory.Loaded += OnRecipeLoaded;
         RecipeFactory.LoadFailed += OnRecipeLoadFailed;
         RecipeFactory.Closing += OnRecipeClosing;

         // Create a default recipe, which will trigger the RecipeLoaded event, and set
         // the current recipe to the instance that is created here. [ml]
         RecipeFactory.NewRecipe(string.Empty);

         _clh = clh;

         Application.Idle += ApplicationIdle;
      }

      #region Recipe Event Handlers
      private void OnRecipeLoaded(object sender, RecipeEventArgs args) {
         HookupRecipe();
         Reset();
      }

      private void OnRecipeLoadFailed(object sender, RecipeEventArgs args) {
         MessageBox.Show(this, args.Message, "csUnitRunner - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      private void OnRecipeClosing(object sender, RecipeEventArgs args) {
         UnhookRecipe();
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         _startTime = DateTime.Now;
         Reset();
      }

      private void OnRecipeFinished(object sender, RecipeEventArgs args) {
         TimeSpan duration = DateTime.Now.Subtract(_startTime);
         Status = "Duration: " + duration.TotalSeconds.ToString("0.000") 
            + " seconds. Finished at: " + DateTime.Now.ToShortTimeString();
         UpdateFormTitle();
#if DEBUG
         _bIdle = true;
#endif
         if(_clh.HasOption("autoexit")) {
            if(_xmlWriter != null) {
               _xmlWriter.Save();
            }
            // Close the current recipe
            RecipeFactory.Current.Close();
            Application.Exit();
         }
      }

      private void OnRecipeSaved(object sender, RecipeEventArgs args) {
         UpdateFormTitle();
      }

      private void OnRecipeAborted(object sender, RecipeEventArgs args) {
         Status = "Tests aborted.";
         if(_clh.HasOption("autoexit")) {
            if(_xmlWriter != null) {
               _xmlWriter.Save();
            }
            // Close the current recipe
            RecipeFactory.Current.Close();
            Application.Exit();
         }
      }

      private void OnSelectorModified(object sender, RecipeEventArgs args) {
         UpdateFormTitle();
      }

      #endregion

      private void Reset() {
         if(InvokeRequired) {
            Invoke(new MethodInvoker(Reset));
         }
         else {
            _testCounters.Reset();
            UpdateFormTitle();
            _statusLabel.Text = "Ready";
         }
      }

      private void HookupRecipe() {
         RecipeFactory.Current.Started  += _recipeStarted;
         RecipeFactory.Current.Finished += _recipeFinished;
         RecipeFactory.Current.Saved    += _recipeSaved;
         RecipeFactory.Current.Aborted  += _recipeAborted;
         RecipeFactory.Current.SelectorModified += OnSelectorModified;

         RecipeFactory.Current.AssemblyAdded += _assemblyAdded;
         RecipeFactory.Current.AssemblyRemoving += _assemblyRemoving;

         foreach(var ta in RecipeFactory.Current.Assemblies) {
            HookupAssembly(ta);
         }
      }

      private void UnhookRecipe() {
         foreach(var ta in RecipeFactory.Current.Assemblies) {
            UnhookAssembly(ta);
         }

         RecipeFactory.Current.AssemblyAdded -= _assemblyAdded;
         RecipeFactory.Current.AssemblyRemoving -= _assemblyRemoving;

         RecipeFactory.Current.Started -= _recipeStarted;
         RecipeFactory.Current.Finished -= _recipeFinished;
         RecipeFactory.Current.Saved -= _recipeSaved;
         RecipeFactory.Current.Aborted -= _recipeAborted;
      }

      private void HookupAssembly(ITestAssembly ta) {
         ta.TestStarted += _testStarted;
         ta.TestFailed  += _testFailed;
         ta.TestError   += _testError;
         ta.TestSkipped += _testSkipped;
         ta.TestPassed  += _testPassed;
         ta.AssemblyChanged     += _assemblyChanged;
      }

      private void UnhookAssembly(ITestAssembly ta) {
         ta.TestStarted -= _testStarted;
         ta.TestFailed  -= _testFailed;
         ta.TestError   -= _testError;
         ta.TestSkipped -= _testSkipped;
         ta.TestPassed  -= _testPassed;
         ta.AssemblyChanged     -= _assemblyChanged;
      }

      #region Assembly Event Handlers
      private static void OnAssemblyChanged(object sender, AssemblyEventArgs args) {
      }

      private void OnAssemblyAdded(object sender, AssemblyEventArgs args) {
         ITestAssembly ta = RecipeFactory.Current[args.AssemblyFullName];
         HookupAssembly(ta);
         UpdateFormTitle();
      }

      private void OnAssemblyRemoving(object sender, AssemblyEventArgs args) {
         ITestAssembly ta = RecipeFactory.Current[args.AssemblyFullName];
         UnhookAssembly(ta);
         UpdateFormTitle();
      }
      #endregion

      #region Test Event Handlers
      private void OnTestPassed(object sender, TestResultEventArgs args) {
         _testCounters._assertCount += args.AssertCount;
      }

      private void OnTestStarted(object sender, TestResultEventArgs args) {
         Status = "Executing "+ args.ClassName + "." + args.MethodName + "()";
      }

      private void OnTestFailed(object sender, TestResultEventArgs args) {
         _testCounters._failedTestCount++;
         _testCounters._assertCount += args.AssertCount;
      }

      private void OnTestError(object sender, TestResultEventArgs args) {
         _testCounters._errorCount++;
         _testCounters._assertCount += args.AssertCount;
      }

      private void OnTestSkipped(object sender, TestResultEventArgs args) {
         _testCounters._skipCount++;
      }
      #endregion

      #region Windows Form Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent(){
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
         this._toolTip = new System.Windows.Forms.ToolTip(this.components);
         this._pMainPanel = new System.Windows.Forms.Panel();
         this._toolStrip = new System.Windows.Forms.ToolStrip();
         this._menuStrip = new System.Windows.Forms.MenuStrip();
         this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._assemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this._csUnitControl = new csUnit.Ui.Controls.CsUnitControl();
         this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
         this._statusStrip = new System.Windows.Forms.StatusStrip();
         this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
         this._statusErrors = new System.Windows.Forms.ToolStripStatusLabel();
         this._statusFailures = new System.Windows.Forms.ToolStripStatusLabel();
         this._statusSkipped = new System.Windows.Forms.ToolStripStatusLabel();
         this._statusAssertions = new System.Windows.Forms.ToolStripStatusLabel();
         this._menuStrip.SuspendLayout();
         this._toolStripContainer.BottomToolStripPanel.SuspendLayout();
         this._toolStripContainer.ContentPanel.SuspendLayout();
         this._toolStripContainer.TopToolStripPanel.SuspendLayout();
         this._toolStripContainer.SuspendLayout();
         this._statusStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // _toolTip
         // 
         this._toolTip.AutoPopDelay = 5000;
         this._toolTip.InitialDelay = 350;
         this._toolTip.ReshowDelay = 100;
         this._toolTip.ShowAlways = true;
         // 
         // _pMainPanel
         // 
         this._pMainPanel.Location = new System.Drawing.Point(842, 17);
         this._pMainPanel.Name = "_pMainPanel";
         this._pMainPanel.Size = new System.Drawing.Size(76, 56);
         this._pMainPanel.TabIndex = 15;
         // 
         // _toolStrip
         // 
         this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
         this._toolStrip.Location = new System.Drawing.Point(3, 24);
         this._toolStrip.Name = "_toolStrip";
         this._toolStrip.Size = new System.Drawing.Size(111, 25);
         this._toolStrip.TabIndex = 17;
         this._toolStrip.Text = "toolStrip1";
         // 
         // _menuStrip
         // 
         this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
         this._menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
         this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._viewToolStripMenuItem,
            this._assemblyToolStripMenuItem,
            this._testToolStripMenuItem,
            this._helpToolStripMenuItem});
         this._menuStrip.Location = new System.Drawing.Point(0, 0);
         this._menuStrip.Name = "_menuStrip";
         this._menuStrip.Size = new System.Drawing.Size(408, 24);
         this._menuStrip.TabIndex = 18;
         this._menuStrip.Text = "menuStrip1";
         // 
         // _fileToolStripMenuItem
         // 
         this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
         this._fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this._fileToolStripMenuItem.Text = "&File";
         // 
         // _viewToolStripMenuItem
         // 
         this._viewToolStripMenuItem.Name = "_viewToolStripMenuItem";
         this._viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
         this._viewToolStripMenuItem.Text = "&View";
         // 
         // _assemblyToolStripMenuItem
         // 
         this._assemblyToolStripMenuItem.Name = "_assemblyToolStripMenuItem";
         this._assemblyToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
         this._assemblyToolStripMenuItem.Text = "&Assembly";
         // 
         // _testToolStripMenuItem
         // 
         this._testToolStripMenuItem.Name = "_testToolStripMenuItem";
         this._testToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
         this._testToolStripMenuItem.Text = "&Test";
         // 
         // _helpToolStripMenuItem
         // 
         this._helpToolStripMenuItem.Name = "_helpToolStripMenuItem";
         this._helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
         this._helpToolStripMenuItem.Text = "&Help";
         // 
         // _csUnitControl
         // 
         this._csUnitControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._csUnitControl.Location = new System.Drawing.Point(0, 0);
         this._csUnitControl.Name = "_csUnitControl";
         this._csUnitControl.Size = new System.Drawing.Size(408, 329);
         this._csUnitControl.TabIndex = 16;
         this._csUnitControl.FillContextMenu += new csUnit.Ui.Controls.CsUnitControlEventHandler(this.OnFillContextMenu);
         // 
         // _toolStripContainer
         // 
         // 
         // _toolStripContainer.BottomToolStripPanel
         // 
         this._toolStripContainer.BottomToolStripPanel.Controls.Add(this._statusStrip);
         // 
         // _toolStripContainer.ContentPanel
         // 
         this._toolStripContainer.ContentPanel.Controls.Add(this._csUnitControl);
         this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(408, 329);
         this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
         this._toolStripContainer.Name = "_toolStripContainer";
         this._toolStripContainer.Size = new System.Drawing.Size(408, 400);
         this._toolStripContainer.TabIndex = 19;
         this._toolStripContainer.Text = "toolStripContainer1";
         // 
         // _toolStripContainer.TopToolStripPanel
         // 
         this._toolStripContainer.TopToolStripPanel.Controls.Add(this._menuStrip);
         this._toolStripContainer.TopToolStripPanel.Controls.Add(this._toolStrip);
         // 
         // _statusStrip
         // 
         this._statusStrip.Dock = System.Windows.Forms.DockStyle.None;
         this._statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
         this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._statusErrors,
            this._statusFailures,
            this._statusSkipped,
            this._statusAssertions});
         this._statusStrip.Location = new System.Drawing.Point(0, 0);
         this._statusStrip.Name = "_statusStrip";
         this._statusStrip.ShowItemToolTips = true;
         this._statusStrip.Size = new System.Drawing.Size(408, 22);
         this._statusStrip.TabIndex = 0;
         // 
         // _statusLabel
         // 
         this._statusLabel.Name = "_statusLabel";
         this._statusLabel.Size = new System.Drawing.Size(291, 17);
         this._statusLabel.Spring = true;
         this._statusLabel.Text = "Ready";
         this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this._statusLabel.ToolTipText = "Status of csUnitRunner";
         // 
         // _statusErrors
         // 
         this._statusErrors.Name = "_statusErrors";
         this._statusErrors.Size = new System.Drawing.Size(25, 17);
         this._statusErrors.Text = "E: 0";
         this._statusErrors.ToolTipText = "Number of tests that have caused errors";
         // 
         // _statusFailures
         // 
         this._statusFailures.Name = "_statusFailures";
         this._statusFailures.Size = new System.Drawing.Size(25, 17);
         this._statusFailures.Text = "F: 0";
         this._statusFailures.ToolTipText = "Number of tests that have failed";
         // 
         // _statusSkipped
         // 
         this._statusSkipped.Name = "_statusSkipped";
         this._statusSkipped.Size = new System.Drawing.Size(25, 17);
         this._statusSkipped.Text = "S: 0";
         this._statusSkipped.ToolTipText = "Number of skipped tests";
         // 
         // _statusAssertions
         // 
         this._statusAssertions.Name = "_statusAssertions";
         this._statusAssertions.Size = new System.Drawing.Size(27, 17);
         this._statusAssertions.Text = "A: 0";
         this._statusAssertions.ToolTipText = "Number of assertions that were executed";
         // 
         // MainForm
         // 
         this.AllowDrop = true;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
         this.ClientSize = new System.Drawing.Size(408, 400);
         this.Controls.Add(this._toolStripContainer);
         this.Controls.Add(this._pMainPanel);
         this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 0 ) ));
         this.Icon = ( (System.Drawing.Icon) ( resources.GetObject("$this.Icon") ) );
         this.MainMenuStrip = this._menuStrip;
         this.MinimumSize = new System.Drawing.Size(263, 247);
         this.Name = "MainForm";
         this.Text = "csUnit";
         this.Load += new System.EventHandler(this.MainForm_Load);
         this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
         this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
         this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
         this._menuStrip.ResumeLayout(false);
         this._menuStrip.PerformLayout();
         this._toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
         this._toolStripContainer.BottomToolStripPanel.PerformLayout();
         this._toolStripContainer.ContentPanel.ResumeLayout(false);
         this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
         this._toolStripContainer.TopToolStripPanel.PerformLayout();
         this._toolStripContainer.ResumeLayout(false);
         this._toolStripContainer.PerformLayout();
         this._statusStrip.ResumeLayout(false);
         this._statusStrip.PerformLayout();
         this.ResumeLayout(false);

      }
      #endregion

      private void OnFillContextMenu(object sender, CsUnitControlEventArgs args) {
         Command.FillContextMenu(args.Menu, args.ElementInfo);
      }

      private void UpdateFormTitle() {
         if( RecipeFactory.Current != null ) {
            _windowTitle = string.Format(
               "{0}{1} ({2}) - csUnit",
               RecipeFactory.Current.DisplayName,
               RecipeFactory.Current.Modified ? "*" : string.Empty,
               RecipeFactory.Current.TestsRunning ? "Running" : "Idle");
         }
      }

      private string _windowTitle = string.Empty;

      #region Drag'n'Drop Handling
      /// <summary>
      /// Called when the mouse drags an item and the user releases the mouse
      /// indicating that the item should be 'dropped' into this form.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Event arguments.</param>
      private void MainForm_DragDrop(object sender, DragEventArgs args) {
         foreach(var fileName in DroppedFileNames(args)) {
            RecipeFactory.Current.AddAssembly(fileName);
         }
      }

      /// <summary>
      /// Called when the mouse drags an item into the client area for this form.
      /// </summary>
      /// <param name="sender">Sender of the event.</param>
      /// <param name="args">Event arguments.</param>
      private void MainForm_DragEnter(object sender, DragEventArgs args) {
         if( args.Data.GetDataPresent(DataFormats.FileDrop)
            && DroppedFileNames(args).Length > 0 ) {
            args.Effect = DragDropEffects.Copy;
         }
         else {
            args.Effect = DragDropEffects.None;
         }
      }

      private static string[] DroppedFileNames(DragEventArgs args) {
         // TODO: The following should use a List<String>() or similar generic. [18aug09, ml]
         var supportedFiles = new ArrayList();
         foreach(var fileName in (string[])args.Data.GetData(DataFormats.FileDrop)) {
            if( fileName.ToLower().EndsWith(".dll")
               || fileName.ToLower().EndsWith(".exe")) {
               supportedFiles.Add(fileName);
            }
         }
         return (string[]) supportedFiles.ToArray(typeof(string));
      }

      #endregion

      #region Status Bar Handling
      public bool StatusBarVisible {
         get {
            return _statusStrip.Visible;
         }
         set {
            _statusStrip.Visible = value;
         }
      }

      private void ApplicationIdle(object sender, EventArgs e) {
         _statusLabel.Text = _statusLabelText;
         _statusErrors.Text = "E: " + _testCounters._errorCount;
         _statusFailures.Text = "F: " + _testCounters._failedTestCount;
         _statusSkipped.Text = "S: " + _testCounters._skipCount;
         _statusAssertions.Text = "A: " + _testCounters._assertCount;
         Text = _windowTitle;
      }

      private string _statusLabelText = "Ready";

      /// <summary>
      /// Sets the status string.
      /// </summary>
      public String Status {
         set {
            _statusLabelText = value;
         }
      }

      #endregion

      #region Tool Bar Handling
      public bool ToolBarVisible {
         get {
            return _toolStrip.Visible;
         }
         set {
            _toolStrip.Visible = value;
         }
      }
      #endregion

      /// <summary>
      /// Asks the user whether to save an unsave modified recipe.
      /// </summary>
      /// <returns>'true', if processing can continue, 'false' otherwise.</returns>
      /// <remarks>Processing can continue, when the user either has saved the
      /// recipe file or when the use has selected 'No' thus explicitly throwing
      /// away the modifications. If the user presses cancel at any time,
      /// processing cannot continue and the method returns 'false'.</remarks>
      public bool AskSaveModifiedRecipe() {
         if(RecipeFactory.Current.DisplayName.Equals("Untitled")
            && ! (new ConfigCurrentUser()).AskForSafeOnModifiedUntitled ) {
            return true;
         }

         if(RecipeFactory.Current.Modified) {
            if(RecipeFactory.Current.AssemblyCount == 0) {
               MessageBox.Show(this, "Current recipe is empty. It will not be saved.", 
                  "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               return true;
            }

            var result = MessageBox.Show(this, "Recipe has been modified. Do"
               + " you want to save it?", "Warning", MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Stop,
               MessageBoxDefaultButton.Button1);
            switch(result) {
               case DialogResult.Yes:
                  Command.ExecuteCommand(typeof(RecipeSaveCommand), null, null);
                  if(RecipeFactory.Current.Modified) {
                     return false;
                  }
                  break;
               case DialogResult.Cancel:
                  return false;
            }
         }
         return true;
      }

      #region Form Event Handling
      private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
         if( AskSaveModifiedRecipe() == false ) {
            e.Cancel = true;
         }
         else {
            new ConfigCurrentUser { MainFormLocation = Location,
                                    MainFormSize = Size
            };
            RecipeFactory.Current.Close();
         }
      }

      private void MainForm_Load(object sender, EventArgs e) {
         var config = new ConfigCurrentUser();
         Location = config.MainFormLocation;
         Size = config.MainFormSize;

         Command.CreateCommands(this, _csUnitControl);
         Command.FillToolStrip(_toolStrip);

         if( _clh.HasOption("recipe") ) {
            if( Utils.FileExists(_clh.GetOptionValueFor("recipe"), true)) {
               RecipeFactory.Load(_clh.GetOptionValueFor("recipe"));
            }
         }
         else if( _clh.HasOption("assembly") ) {
            if( Utils.FileExists(_clh.GetOptionValueFor("assembly"), true)) {
               var assemblyPathName = _clh.GetOptionValueFor("assembly");
               if( !Path.IsPathRooted(assemblyPathName) ) {
                  assemblyPathName = Path.Combine(Environment.CurrentDirectory, assemblyPathName);
               }
               RecipeFactory.Current.AddAssembly(assemblyPathName);
            }
         }
         else switch(config.StartupLoadItem) {
            case "Recipe":
               if(   config.RecentRecipies.Count > 0
                     && Utils.FileExists(config.RecentRecipies[0], true) ) {
                  RecipeFactory.Load(config.RecentRecipies[0]);
               }
               break;
            case "Assembly":
               if(   config.RecentAssemblies.Count > 0
                     && Utils.FileExists(config.RecentAssemblies[0], true)) {
                  RecipeFactory.Current.AddAssembly(config.RecentAssemblies[0]);
               }
               break;
         }
         
         // Setup the xml handler
         if( _clh.HasOption("xml") ) {
            _xmlWriter = new DefaultXmlWriter(RecipeFactory.Current, _clh.GetOptionValueFor("xml"));
         }

         // Automatically start the recipe
         if( _clh.HasOption("autorun") ) {
            if(RecipeFactory.Current != null) {
               var testRun = new TestRun(new AllTestsCriterion());
               RecipeFactory.Current.RunTests(testRun);
            }
         }
      }

      #endregion

      private DateTime _startTime;
// ReSharper disable InconsistentNaming
      private System.ComponentModel.IContainer components;
// ReSharper restore InconsistentNaming
      private ToolTip _toolTip;
      private readonly CmdLineHandler _clh;
      private Panel _pMainPanel;
      private CsUnitControl _csUnitControl;

      private readonly TestEventHandler _testStarted;
      private readonly TestEventHandler _testFailed;
      private readonly TestEventHandler _testError;
      private readonly TestEventHandler _testSkipped;
      private readonly TestEventHandler _testPassed;
      private readonly RecipeEventHandler _recipeStarted;
      private readonly RecipeEventHandler _recipeFinished;
      private readonly RecipeEventHandler _recipeSaved;
      private readonly RecipeEventHandler _recipeAborted;
      private readonly AssemblyEventHandler _assemblyAdded;
      private readonly AssemblyEventHandler _assemblyRemoving;
      private readonly AssemblyEventHandler _assemblyChanged;

      private DefaultXmlWriter _xmlWriter;
      private readonly TestCounters _testCounters = new TestCounters();
      private ToolStrip _toolStrip;
      private MenuStrip _menuStrip;
      private ToolStripMenuItem _fileToolStripMenuItem;
      private ToolStripMenuItem _viewToolStripMenuItem;
      private ToolStripMenuItem _assemblyToolStripMenuItem;
      private ToolStripMenuItem _testToolStripMenuItem;
      private ToolStripMenuItem _helpToolStripMenuItem;
      private ToolStripContainer _toolStripContainer;
      private StatusStrip _statusStrip;
      private ToolStripStatusLabel _statusLabel;
      private ToolStripStatusLabel _statusErrors;
      private ToolStripStatusLabel _statusFailures;
      private ToolStripStatusLabel _statusSkipped;
      private ToolStripStatusLabel _statusAssertions;

      #region Diagnostics
#if DEBUG
      private bool _bIdle = true;

      internal void Join() {
         while( !_bIdle ) {
            Application.DoEvents();
            Thread.Sleep(100);
         }
      }

#endif

      #endregion
   }
}
