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
using csUnit.Common;
using csUnit.Core.Criteria;
using csUnit.Data;

namespace csUnit.Core.Tests.Data {
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
   public class DataSourceAttributeTests {
      [TestFixture]
      public class StaticDataSource {
         private class FixtureWithStaticDataProvider {
            public static DataRow[] GetTestData() {
               return new[] {
                  new DataRow(0, 0),
                  new DataRow(1, 1),
                  new DataRow(2, 4),
                  new DataRow(3, 9) 
               };
            }

            [Test]
            [DataSource(typeof(FixtureWithStaticDataProvider))]
            public void Squares(int oper, int result) {
               Assert.Equals(result, oper * oper);
            }
         }

         [Test]
         public void StaticMethodAsProvider() {
            var tf = new TestFixture(typeof(FixtureWithStaticDataProvider));
            tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(4, SimpleTestListener.PassedItems.Count);
            Assert.Equals(0, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TS:Squares TP:Squares TS:Squares TP:Squares TS:Squares TP:Squares TS:Squares TP:Squares", SimpleTestListener.Messages);
         }

         private class ErrorInDataProvider {
            public static DataRow[] GetTestData() {
               throw new Exception("Error in data provider");
            }

            [Test]
            [DataSource(typeof(ErrorInDataProvider))]
            public void Squares(int oper, int result) {
               Assert.Equals(result, oper * oper);
            }
         }

         [Test]
         public void StaticMethodWithError() {
            var tf = new TestFixture(typeof(ErrorInDataProvider));
            tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TE:Squares No data rows specified for parameterized test.", SimpleTestListener.Messages);
         }

         private class DataSourceReturningInvalidRow {
            public static DataRow[] GetTestData() {
               return new[] {
                  new DataRow(0, 0),
                  new DataRow(1, 1, 2),
                  new DataRow(2, 4),
                  new DataRow(3, 9) 
               };
            }

            [Test]
            [DataSource(typeof(DataSourceReturningInvalidRow))]
            public void Squares(int oper, int result) {
               Assert.Equals(result, oper * oper);
            }
         }

         [Test]
         public void DataSourceWithInvalidRow() {
            var tf = new TestFixture(typeof(DataSourceReturningInvalidRow));
            tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(3, SimpleTestListener.PassedItems.Count);
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TS:Squares TP:Squares TE:Squares Each data row for Squares must have 2 values. TS:Squares TP:Squares TS:Squares TP:Squares", SimpleTestListener.Messages);
         }

         private class ClassWithStaticPropertyAsDataProvider {
            public static DataRow[] TestData {
               get {
                  return new[] {
                     new DataRow(0, 0),
                     new DataRow(1, 1),
                     new DataRow(2, 4),
                     new DataRow(3, 9) 
                  };
               }
            }

            [Test]
            [DataSource(typeof(ClassWithStaticPropertyAsDataProvider))]
            public void Squares(int oper, int result) {
               Assert.Equals(result, oper * oper);
            }
         }

         [Test]
         public void StaticPropertyAsDataProvider() {
            var tf = new TestFixture(typeof(ClassWithStaticPropertyAsDataProvider));
            tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(4, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TS:Squares TP:Squares TS:Squares TP:Squares TS:Squares TP:Squares TS:Squares TP:Squares", SimpleTestListener.Messages);
         }

         [Test]
         public void TypeDataSourceWithNullValue() {
            var attr = new DataSourceAttribute((Type) null);
            Assert.Equals(new DataRow[] { }, attr.DataRows);
         }

         private class ClassWithProperty {
            public int MyProperty { get; set; }

            [Test]
            public void SomeUsefulTest() {
            }
         }

         [Test]
         public void ClassWithPropertyDoesNotCauseError() {
            // This is a test specific to bug SF1797256.
            var tf = new TestFixture(typeof(ClassWithProperty));
            tf.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(1, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TS:SomeUsefulTest TP:SomeUsefulTest", SimpleTestListener.Messages);
         }
      }

      [TestFixture]
      public class XmlDataSource {
         [SetUp]
         public void SetUp() {
            XmlDocumentMock.RawContent = string.Empty;
            XmlDocumentFactory.Type = typeof(XmlDocumentMock);
         }

         private const string SerializedContent =
            "<dataTable>" +
               "<dataRow>" +
                  "<value>1</value>" +
                  "<value>1</value>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>2</value>" +
                  "<value>4</value>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>3</value>" +
                  "<value>9</value>" +
               "</dataRow>" +
            "</dataTable>";

         private static void SetXmlDocumentContent(string content) {
            XmlDocumentMock.RawContent = content;
         }

         private class FixtureWithSimpleXmlDataSource {
            [Test]
            [DataSource("Squares.params.xml")]
            public void Squares(int oper, int result) {
               Assert.Equals(result, oper * oper);
            }
         }

         [Test]
         public void SimpleList() {
            SetXmlDocumentContent(SerializedContent);
            var fixture = new TestFixture(typeof(FixtureWithSimpleXmlDataSource));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(3, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TS:Squares TP:Squares TS:Squares TP:Squares TS:Squares TP:Squares", SimpleTestListener.Messages);
         }

         private const string SerializedContentNoDataRows =
            "<dataTable>" +
            "</dataTable>";

         [Test]
         public void ListWithoutDataRows() {
            SetXmlDocumentContent(SerializedContentNoDataRows);
            var fixture = new TestFixture(typeof(FixtureWithSimpleXmlDataSource));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TE:Squares No data rows specified for parameterized test.", SimpleTestListener.Messages);
         }

         private const string ContentOfDifferentXmlFile =
            "<content>" +
            "</content>";

         [Test]
         public void FeedWithDifferentXmlFile() {
            SetXmlDocumentContent(ContentOfDifferentXmlFile);
            var fixture = new TestFixture(typeof(FixtureWithSimpleXmlDataSource));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TE:Squares No data rows specified for parameterized test.", SimpleTestListener.Messages);
         }

         private const string CrabContent = "blah blubber flabber";

         [Test]
         public void FeedWithArbitraryTextFile() {
            SetXmlDocumentContent(CrabContent);
            var fixture = new TestFixture(typeof(FixtureWithSimpleXmlDataSource));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count);
            Assert.Equals(" TE:Squares No data rows specified for parameterized test.", SimpleTestListener.Messages);
         }

         private const string WrongValueCount =
            "<dataTable>" +
               "<dataRow>" +
                  "<value>1</value>" +
                  "<value>1</value>" +
                  "<value>1</value>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>2</value>" +
                  "<value>4</value>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>3</value>" +
                  "<value>9</value>" +
               "</dataRow>" +
            "</dataTable>";

         [Test]
         public void ListContainingDataRowWithWrongValueCount() {
            SetXmlDocumentContent(WrongValueCount);
            var fixture = new TestFixture(typeof(FixtureWithSimpleXmlDataSource));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(2, SimpleTestListener.PassedItems.Count);
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TE:Squares Each data row for Squares must have 2 values. TS:Squares TP:Squares TS:Squares TP:Squares", SimpleTestListener.Messages);
         }

         private const string ContentWithExpectedException =
            "<dataTable>" +
               "<dataRow>" +
                  "<value>1</value>" +
                  "<value>0</value>" +
                  "<value>0</value>" +
                  "<expectedException>System.DivideByZeroException</expectedException>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>4</value>" +
                  "<value>2</value>" +
                  "<value>2</value>" +
               "</dataRow>" +
            "</dataTable>";

         private class DivisionTests {
            [Test]
            [DataSource("Division.params.xml")]
            public void Division(int nominator, int denominator, int expected) {
               var actualResult = nominator / denominator;
               Assert.Equals(expected, actualResult);
            }
         }

         [Test]
         public void ListWithExpectedException() {
            SetXmlDocumentContent(ContentWithExpectedException);
            var fixture = new TestFixture(typeof(DivisionTests));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(2, SimpleTestListener.PassedItems.Count);
            Assert.Equals(0, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TS:Division TP:Division TS:Division TP:Division", SimpleTestListener.Messages);
         }

         private const string ContentWithExpectedExceptionNotThrown =
            "<dataTable>" +
               "<dataRow>" +
                  "<value>1</value>" +
                  "<value>1</value>" +
                  "<value>1</value>" +
                  "<expectedException>System.DivideByZeroException</expectedException>" +
               "</dataRow>" +
               "<dataRow>" +
                  "<value>4</value>" +
                  "<value>2</value>" +
                  "<value>2</value>" +
               "</dataRow>" +
            "</dataTable>";

         [Test]
         public void ListWithExpectedExceptionNotThrown() {
            SetXmlDocumentContent(ContentWithExpectedExceptionNotThrown);
            var fixture = new TestFixture(typeof(DivisionTests));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(1, SimpleTestListener.PassedItems.Count);
            Assert.Equals(1, SimpleTestListener.FailureItems.Count);
            Assert.Equals(" TS:Division TF:Division Expected exception of type System.DivideByZeroException was not thrown. TS:Division TP:Division", SimpleTestListener.Messages);
         }

         [Test]
         public void XmlDataSourceWithNullValue() {
            var attr = new DataSourceAttribute((string) null);
            Assert.Equals(new DataRow[] { }, attr.DataRows);
         }
      }

      [TestFixture]
      public class DbAsSource {
         [Test]
         public void JustTheAttribute() {
            var dsa = new DataSourceAttribute("System.Data.SqlClient", 
               "Data Source=.\\SQLEXPRESS;" +
               @"AttachDBFilename=|DataDirectory|\Data\csUnitTestData.mdf;" +
               "Integrated Security=True;Connect Timeout=30;User Instance=True", 
               "DivisionTests");
            var rows = dsa.DataRows;
            Assert.Contains(new DataRow(4, 2, 2), rows, "Note: SQL Server (Express) database needed to pass");
            Assert.Contains(new DataRow(10, 5, 2), rows);
            Assert.Contains(new DataRow(9, 3, 3), rows);
            Assert.Equals(3, rows.GetLength(0));
         }

         private class DivisionTests {
            [Test]
            [DataSource("System.Data.SqlClient",
               "Data Source=.\\SQLEXPRESS;" +
               @"AttachDBFilename=|DataDirectory|\Data\csUnitTestData.mdf;" +
               "Integrated Security=True;Connect Timeout=30;User Instance=True", 
               "DivisionTests")] 
            public void Division(int nominator, int denominator, int expectedResult) {
               var actualResult = nominator / denominator;
               Assert.Equals(expectedResult, actualResult);
            }
         }

         [Test(Categories = "SQLEXPRESS")]
         public void Simple() {
            var fixture = new TestFixture(typeof(DivisionTests));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(3, SimpleTestListener.PassedItems.Count, 
               "Note: SQL Server (Express) database required for this test to pass");
            Assert.Equals(0, SimpleTestListener.ErrorItems.Count);
            Assert.Equals(" TS:Division TP:Division TS:Division TP:Division TS:Division TP:Division", SimpleTestListener.Messages);
         }

         private class DataDrivenButDataSourceUnavailable {
            [Test]
            [DataSource("System.Data.SqlClient", 
               "Data Source=.\\MSSQLSERVER;" +
               @"AttachDBFilename=|DataDirectory|\csUnitTestData.mdf;" +
               "Integrated Security=True;Connect Timeout=1;User Instance=True",
               "DivisionTests")]
            public void Division(int nominator, int denominator, int expectedResult) {
               var actualResult = nominator / denominator;
               Assert.Equals(expectedResult, actualResult);
            }
         }

         [Test(Categories = "SQLEXPRESS")]
         public void DataSourceUnavailable() {
            var fixture = new TestFixture(typeof(DataDrivenButDataSourceUnavailable));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count, "Note: SQL Server (Express) database needed to pass");
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Contains(" TE:Division System.Data.SqlClient.SqlException (0x80131904): A network-related or instance-specific", SimpleTestListener.Messages);
         }

         private class InvalidFile {
            [Test]
            [DataSource("System.Data.SqlClient",
               "Data Source=.\\SQLEXPRESS;" +
               @"AttachDBFilename=|DataDirectory|\csUnitTestData.mdf;" +
               "AttachDbFilename=\"C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Data\\fooTestData.mdf\";" +
               "Integrated Security=True;Connect Timeout=10;User Instance=True",
               "DivisionTests")]
            public void Division(int nominator, int denominator, int expectedResult) {
               var actualResult = nominator / denominator;
               Assert.Equals(expectedResult, actualResult);
            }
         }

         [Test(Categories = "SQLEXPRESS")]
         public void InvalidDataBaseFile() {
            var fixture = new TestFixture(typeof(InvalidFile));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count, "Note: SQL Server (Express) database needed to pass");
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Contains(" TE:Division System.Data.SqlClient.SqlException (0x80131904): An attempt to attach", SimpleTestListener.Messages);
         }

         private class IntegratedSecuritySetFalse {
            [Test]
            [DataSource("System.Data.SqlClient",
               "Data Source=.\\SQLEXPRESS;" +
               @"AttachDBFilename=|DataDirectory|\csUnitTestData.mdf;" +
               "Integrated Security=False;Connect Timeout=30;User Instance=True",
               "DivisionTests")]
            public void Division(int nominator, int denominator, int expectedResult) {
               int actualResult = nominator / denominator;
               Assert.Equals(expectedResult, actualResult);
            }
         }

         [Test(Categories = "SQLEXPRESS")]
         public void IntegratedSecurityFalse() {
            var fixture = new TestFixture(typeof(IntegratedSecuritySetFalse));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count, "Note: SQL Server (Express) database needed to pass");
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Contains(" TE:Division System.Data.SqlClient.SqlException (0x80131904): Login failed for user ", SimpleTestListener.Messages);
         }

         private class UserInstanceSetFalse {
            [Test]
            [DataSource("System.Data.SqlClient",
               "Data Source=.\\SQLEXPRESS;" +
               @"AttachDBFilename=|DataDirectory|\csUnitTestData.mdf;" +
               "Integrated Security=True;Connect Timeout=10;User Instance=False",
               "DivisionTests")]
            public void Division(int nominator, int denominator, int expectedResult) {
               var actualResult = nominator / denominator;
               Assert.Equals(expectedResult, actualResult);
            }
         }

         [Test(Categories="SQLEXPRESS")]
         public void UserInstanceFalse() {
            var fixture = new TestFixture(typeof(UserInstanceSetFalse));
            fixture.Execute(new TestRun(new AllTestsCriterion()), new SimpleTestListener());
            Assert.Equals(0, SimpleTestListener.PassedItems.Count, "Note: SQL Server (Express) database needed to pass");
            Assert.Equals(1, SimpleTestListener.ErrorItems.Count);
            Assert.Contains(" TE:Division System.Data.SqlClient.SqlException (0x80131904):", SimpleTestListener.Messages);
         }
      }
   }
   // ReSharper restore UnusedMember.Local
   // ReSharper restore UnusedMember.Global
}
