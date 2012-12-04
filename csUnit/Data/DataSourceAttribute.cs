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
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using csUnit.Common;

namespace csUnit.Data {
   /// <summary>
   /// Use DataSourceAttribute for providing information about value sets for
   /// parameterized tests.
   /// </summary>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
   public class DataSourceAttribute : Attribute {
      /// <summary>
      /// Constructs a DataSourceAttribute instance using an XML file as the
      /// data source for the parameterization.
      /// </summary>
      /// <param name="xmlFileName">Path and name of the XML file.</param>
      public DataSourceAttribute(string xmlFileName) {
         _xmlPathFileName = xmlFileName;
      }

      /// <summary>
      /// Constructs a DataSourceAttribute instance for using an ADO.NET data
      /// provider as a datasource.
      /// </summary>
      /// <param name="dataProviderInvariantName">Invariant name of the ADO.NET data provider.</param>
      /// <param name="connectionString">Connection string for the data source.</param>
      /// <param name="tableName">Table from the data source to be used.</param>
      public DataSourceAttribute(string dataProviderInvariantName,
         string connectionString, string tableName) {
         _dataProviderInvariantName = dataProviderInvariantName;
         _connectionString = connectionString;
         _tableName = tableName;
      }

      /// <summary>
      /// Creates and instance of a DataSourceAttribute.
      /// </summary>
      /// <param name="dataProvider">Type that will provide data.</param>
      public DataSourceAttribute(Type dataProvider) {
         _dataProvider = dataProvider;
      }

      /// <summary>
      /// Returns rows containing value sets for parameterized tests.
      /// </summary>
      public DataRow[] DataRows {
         get {
            if(_dataRows == null) {
               TryLoadFromDataProviderType();
            }
            if(_dataRows == null) {
               TryLoadFromXmlFile();
            }
            if(_dataRows == null) {
               TryLoadFromDatabase();
            }
            if(_dataRows == null) {
               _dataRows = new DataRow[] { };
            }
            return _dataRows;
         }
      }

      private void TryLoadFromDataProviderType() {
         if(_dataProvider != null) {
            var providerMethod = FindProviderMethod(_dataProvider);
            DataRow[] dataRows = null;
            try {
               dataRows = providerMethod.Invoke(null, null) as DataRow[];
            }
// ReSharper disable EmptyGeneralCatchClause
            catch {
// ReSharper restore EmptyGeneralCatchClause
            }

            if(dataRows != null) {
               _dataRows = dataRows;
            }
         }
      }

      private void TryLoadFromXmlFile() {
         if(!string.IsNullOrEmpty(_xmlPathFileName)) {
            var dataRows = new List<DataRow>();
            try {
               var document = XmlDocumentFactory.CreateInstance();
               document.Load(_xmlPathFileName);
               var rowNodes = document.SelectNodes("dataTable/dataRow");
               foreach(XmlNode rowNode in rowNodes) {
                  var valueNodes = rowNode.SelectNodes("value");
                  if (valueNodes != null) {
                     var values = new string[valueNodes.Count];
                     for (var i = 0; i < valueNodes.Count; i++) {
                        values[i] = valueNodes[i].InnerText;
                     }
                     var exceptionNodes = rowNode.SelectNodes("expectedException");
                     if (exceptionNodes != null
                      && exceptionNodes.Count > 0) {
                        dataRows.Add(new DataRow(exceptionNodes[0].InnerText, values));
                     }
                     else {
                        dataRows.Add(new DataRow(values));
                     }
                  }
               }
            }
            catch(XmlException ex) {
               Console.WriteLine(ex.ToString());
            }
            _dataRows = dataRows.ToArray();
         }
      }

      private void TryLoadFromDatabase() {
         if(   _dataProviderInvariantName != string.Empty
            && _connectionString != string.Empty
            && _tableName != string.Empty) {
            var providerFactory = FindSuitableProviderFactory(_dataProviderInvariantName);
            using(var connection = providerFactory.CreateConnection()) {
               connection.ConnectionString = _connectionString;

               // Create the data adapter
               var adapter = providerFactory.CreateDataAdapter();
               adapter.SelectCommand = connection.CreateCommand();
               adapter.SelectCommand.CommandText = "SELECT * FROM " + _tableName;

               // Run the query
               var table = new DataTable();
               adapter.Fill(table);

               var dataRows = new List<DataRow>();
               foreach(System.Data.DataRow row in table.Rows) {
                  dataRows.Add(new DataRow(row.ItemArray));
               }
               _dataRows = dataRows.ToArray();
            }
         }
      }

      private static DbProviderFactory FindSuitableProviderFactory(string dataProviderInvariantName) {
         var table = DbProviderFactories.GetFactoryClasses();
         foreach(System.Data.DataRow row in table.Rows) {
            if(((string)row["InvariantName"]) == dataProviderInvariantName) {
               return DbProviderFactories.GetFactory((string)row["InvariantName"]);
            }

         }
         return null;
      }

      private static MethodBase FindProviderMethod(Type dataProvider) {
         foreach(var method in dataProvider.GetMethods()) {
            if(method.ReturnType == typeof(DataRow[])
               && method.GetParameters().Length == 0
               && method.IsStatic ) {
               return method;
            }
         }
         return null;
      }

      private readonly Type _dataProvider;
      private readonly string _xmlPathFileName = string.Empty;
      private DataRow[] _dataRows;

      private readonly string _dataProviderInvariantName = string.Empty;
      private readonly string _connectionString = string.Empty;
      private readonly string _tableName = string.Empty;
   }
}
