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
using System.Reflection;
using csUnit.Data;
using csUnit.Interfaces;

namespace csUnit.Core.Data {
   internal class ParameterizedTestMethod : TestMethod {
      public ParameterizedTestMethod(TestFixture testFixture, MethodInfo methodInfo)
         : base(testFixture, methodInfo) {
         foreach(DataRow dataRow in methodInfo.GetCustomAttributes(typeof(DataRow), true)) {
            _dataSource.Add(dataRow);
         }
         if(_dataSource.Count == 0) {
            object[] attrs = methodInfo.GetCustomAttributes(typeof(DataSourceAttribute), true);
            if( attrs.Length > 0 ) {
               _dataSource.Add(attrs[0] as DataSourceAttribute);
            }
         }
         _expectedParameterNum = MethodInfo.GetParameters().Length;
      }

      public override void Execute(ITestListener listener) {
         if(_dataSource.Count > 0) {
            foreach(var dataRow in _dataSource) {
               if (!ValidateParameters(listener, dataRow)) {
                  continue;
               }
               ExpectedException = dataRow.ExpectedException != null ? new ExpectedExceptionAttribute(dataRow.ExpectedException) : null;
               ExecuteInternal(listener, dataRow.Values);
            }
         }
         else {
            ReportError(listener, _dataSource.ErrorReason);
         }
      }

      private bool ValidateParameters(ITestListener listener, DataRow dataRow) {
         if(_expectedParameterNum == dataRow.Values.Length) {
            var parameters = MethodInfo.GetParameters();
            var paramValues = new object[parameters.Length];
            for(var i = 0; i < parameters.Length; i++) {
               try {
                  paramValues[i] = Convert.ChangeType(dataRow.Values[i], parameters[i].ParameterType);
               }
               catch(InvalidCastException) {
                  ReportError(listener, string.Format(
                     "Parameter types don't match for DataRow({0}).",
                     dataRow));
                  return false;
               }
               catch(FormatException) {
                  ReportError(listener, string.Format(
                     "Parameter types don't match for DataRow({0}).",
                     dataRow));
                  return false;
               }
               catch(Exception ex) {
                  ReportError(listener, "Failed to convert parameter." + ex.Message);
                  return false;
               }
            }
            dataRow.Values = paramValues;
            return true;
         }
         ReportError(listener, string.Format("Each data row for {0} must have {1} values.",
                                             MethodInfo.Name, _expectedParameterNum));
         return false;
      }

      private void ReportError(ITestListener listener, string errorReason) {
         var trea = new TestResultEventArgs(AssemblyName,
            Fixture.FullName, Name, errorReason, 0);
         listener.OnTestError(new TestMethodInfo(this), trea);
      }

      private readonly TestDataSource _dataSource = new TestDataSource();
      private readonly int _expectedParameterNum;
   }
}
