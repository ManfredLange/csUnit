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
using System.Xml;
using csUnit.Common;
using csUnit.Interfaces;

namespace csUnit.Core {
   /// <summary>
   /// Summary description for DefaultXmlWriter.
   /// </summary>
   public class DefaultXmlWriter {
      public DefaultXmlWriter(IRecipe recipe, string resultsPathFileName) {
         if( resultsPathFileName != string.Empty ) {
            _resultsFileName = resultsPathFileName;
         }
         _recipeStarted = OnRecipeStarted;
         _recipeFinished = OnRecipeFinished;
         _assemblyStarted = OnAssemblyStarted;
         _assemblyFinished = OnAssemblyFinished;

         _testPassed = OnTestPassed;
         _testFailed = OnTestFailed;
         _testError = OnTestError;
         _testSkipped = OnTestSkipped;
         Hookup(recipe);
      }

      /// <summary>
      /// Gets the result of the test run. Returns 0, if all tests have been
      /// passed successfully. If an error or a failure has been detected or
      /// if a test was skipped, the return value is greater than 0.
      /// </summary>
      public int Result {
         get {
            return _errorCount + _failureCount + _skippedCount;
         }
      }

      public void Save() {
         Save(_resultsFileName);
      }

      public void Save(string fileName) {
         _document.Save(fileName);
      }

      private void Hookup(IRecipe recipe) {
         _document = XmlDocumentFactory.CreateInstance();

         WriteDocumentHeader(recipe.DisplayName);

         recipe.Started  += _recipeStarted;
         recipe.Finished += _recipeFinished;
         recipe.Aborted += _recipeFinished;

         foreach(var ta in recipe.Assemblies) {
            ta.AssemblyStarted  += _assemblyStarted;
            ta.AssemblyFinished += _assemblyFinished;

            ta.TestPassed  += _testPassed;
            ta.TestFailed  += _testFailed;
            ta.TestError   += _testError;
            ta.TestSkipped += _testSkipped;
         }
      }

      private void WriteDocumentHeader(string recipeName) {
         _document.AppendChild(_document.CreateXmlDeclaration("1.0", string.Empty, "no"));
         _document.AppendChild(_document.CreateComment("This file contains the results of a csUnit test drive."));
         _results = _document.CreateElement("test-results");
         _document.AppendChild(_results);

         var name = _document.CreateAttribute("name");
         name.Value = recipeName;
         _results.Attributes.Append(name);
      }

      private void OnRecipeStarted(object sender, RecipeEventArgs args) {
         _startTime = DateTime.Now;
      }

      private void OnRecipeFinished(object sender, RecipeEventArgs args) {
         var total = _document.CreateAttribute("total");
         total.Value = (_passedCount + _failureCount + _errorCount + _skippedCount).ToString();
         _results.Attributes.Append(total);

         CreateAttributeOnResults("passed", _passedCount.ToString());
         CreateAttributeOnResults("failures", _failureCount.ToString());
         CreateAttributeOnResults("errors", _errorCount.ToString());
         CreateAttributeOnResults("skipped", _skippedCount.ToString());
         CreateAttributeOnResults("duration", (DateTime.Now - _startTime).TotalSeconds.ToString("0.000"));
         
         _startTime = DateTime.MinValue;
      }

      private void CreateAttributeOnResults(string attributeName, string attributeValue) {
         var passed = _document.CreateAttribute(attributeName);
         passed.Value = attributeValue;
         _results.Attributes.Append(passed);
      }

      private void OnAssemblyStarted(object sender, AssemblyEventArgs args) {
         _assemblyPassedCount = 0;
         _assemblyErrorCount = 0;
         _assemblyFailureCount = 0;
         _assemblySkippedCount = 0;
         _assemblyStartTime = DateTime.Now;

         _assembly = _document.CreateElement("assembly");
         var name = _document.CreateAttribute("name");
         name.Value = args.AssemblyFullName;
         _assembly.Attributes.Append(name);
         _results.AppendChild(_assembly);
      }

      private void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
         var total = _document.CreateAttribute("total");
         total.Value = (_assemblyPassedCount + _assemblyErrorCount + _assemblyFailureCount + _assemblySkippedCount).ToString();
         _assembly.Attributes.Append(total);

         CreateAndAppendAssemblyAttribute("passed", _assemblyPassedCount.ToString());
         CreateAndAppendAssemblyAttribute("failures", _assemblyFailureCount.ToString());
         CreateAndAppendAssemblyAttribute("errors", _assemblyErrorCount.ToString());
         CreateAndAppendAssemblyAttribute("skipped", _assemblySkippedCount.ToString());
         CreateAndAppendAssemblyAttribute("duration", (DateTime.Now - _assemblyStartTime).TotalSeconds.ToString("0.000"));
      }

      private void CreateAndAppendAssemblyAttribute(string attributeName, string attributeValue) {
         var passed = _document.CreateAttribute(attributeName);
         passed.Value = attributeValue;
         _assembly.Attributes.Append(passed);
      }

      private void OnTestPassed(object sender, TestResultEventArgs args) {
         _passedCount++;
         _assemblyPassedCount++;
         CreateResultNode(args);
      }

      private void OnTestFailed(object sender, TestResultEventArgs args) {
         _failureCount++;
         _assemblyFailureCount++;
         CreateResultNode(args);
      }

      private void OnTestError(object sender, TestResultEventArgs args) {
         _errorCount++;
         _assemblyErrorCount++;
         CreateResultNode(args);
      }

      private void OnTestSkipped(object sender, TestResultEventArgs args) {
         _skippedCount++;
         _assemblySkippedCount++;
         CreateResultNode(args);
      }

      private void CreateResultNode(TestResultEventArgs args) {
         var test = _document.CreateElement("test");
         var name = _document.CreateAttribute("name");
         name.Value = args.ClassName + "." + args.MethodName;
         test.Attributes.Append(name);
         _assembly.AppendChild(test);

         test.Attributes.Append(CreateAttributeOnTest("test-result", args.TestResult.ToString()));
         test.Attributes.Append(CreateAttributeOnTest("reason", args.Reason));
         test.Attributes.Append(CreateAttributeOnTest("duration", (((double) args.Duration) / 1000000).ToString("0.000")));
         test.Attributes.Append(CreateAttributeOnTest("asserts", args.AssertCount.ToString()));
      }

      private XmlAttribute CreateAttributeOnTest( string attributeName, string attributeValue) {
         var testResult = _document.CreateAttribute(attributeName);
         testResult.Value = attributeValue;
         return testResult;
      }

      private DateTime     _startTime;
      private DateTime     _assemblyStartTime;

      private int          _passedCount;
      private int          _failureCount;
      private int          _errorCount;
      private int          _skippedCount;
      
      private int          _assemblyPassedCount;
      private int          _assemblyErrorCount;
      private int          _assemblyFailureCount;
      private int          _assemblySkippedCount;

      private IXmlDocument _document;
      private XmlElement   _results;
      private XmlElement   _assembly;

      private readonly RecipeEventHandler   _recipeStarted;
      private readonly RecipeEventHandler   _recipeFinished;
      private readonly AssemblyEventHandler _assemblyStarted;
      private readonly AssemblyEventHandler _assemblyFinished;

      private readonly TestEventHandler _testPassed;
      private readonly TestEventHandler _testFailed;
      private readonly TestEventHandler _testError;
      private readonly TestEventHandler _testSkipped;

      private readonly string _resultsFileName = "csUnit.results.xml";
   }
}
