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
using System.IO;
using csUnit.Common;
using csUnit.Interfaces;

namespace csUnit.CommandLine.Tests {
   internal class RecipeMock : IRecipe {
      #region IRecipe Members
#pragma warning disable 67
      public event RecipeEventHandler Aborted;

      public event RecipeEventHandler Started;

      public event RecipeEventHandler Finished;

      public event RecipeEventHandler Saved;

      public event RecipeEventHandler SelectorModified;

      public event AssemblyEventHandler AssemblyAdded;

      public event AssemblyEventHandler AssemblyRemoving;

      public event AssemblyEventHandler AssemblyRemoved;
#pragma warning restore 67
      public ITestAssembly this[string assemblyFullOrPathFileName] {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public ITestAssembly[] Assemblies {
         get {
            return new ITestAssembly[] { };
         }
      }

      public Categories Categories {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public Set<ISelector> Selectors {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public void SetConsoleOutputTo(TextWriter value) {
         throw new NotImplementedException();
      }

      public int AssemblyCount {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public string DisplayName {
         get {
            return "Mocked Recipe";
         }
      }

      public bool IsNew {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public bool Modified {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
      }

      public string PathName {
         get {
            throw new Exception("The method or operation is not implemented.");
         }
         set {
            RecipePathName = value;
         }
      }

      public bool TestsRunning {
         get {
            return false;
         }
      }

      public void Join() {
         return;
      }

      public void Abort() {
         throw new Exception("The method or operation is not implemented.");
      }

      public void AddAssembly(string assemblyPathName) {
         throw new Exception("The method or operation is not implemented.");
      }

      public void Clear() {
         throw new Exception("The method or operation is not implemented.");
      }

      public void Close() {
         throw new Exception("The method or operation is not implemented.");
      }

      public int CountTests() {
         throw new Exception("The method or operation is not implemented.");
      }

      public void RegisterSelector(ISelector filter) {
         _selectors.Add(filter);
      }

      public void RemoveAssembly(string assemblyFullName) {
         throw new Exception("The method or operation is not implemented.");
      }

      public void RunTests(ITestRun selection) {
         FireFinishedEvent();
      }

      private void FireFinishedEvent() {
         if (Finished != null) {
            Finished(this, new RecipeEventArgs());
         }
      }

      public void Save() {
         throw new Exception("The method or operation is not implemented.");
      }

      public void Save(string pathName) {
         throw new Exception("The method or operation is not implemented.");
      }

      public List<FileNotFoundException> LoadFromXml(IXmlDocument doc) {
         return new List<FileNotFoundException>();
      }

      public IEnumerator<ITestAssembly> GetEnumerator() {
         throw new Exception("The method or operation is not implemented.");
      }

      #endregion

      #region IDisposable Members

      public void Dispose() {
         throw new Exception("The method or operation is not implemented.");
      }

      #endregion

      public static string RecipePathName = string.Empty;
      public static readonly Set<ISelector> _selectors = new Set<ISelector>();
   }

}
