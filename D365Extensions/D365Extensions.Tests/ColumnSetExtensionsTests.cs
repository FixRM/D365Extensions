using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class ColumnSetExtensionsTests
    {
        [TestMethod()]
        public void AddColumnTest()
        {
            // Setup
            string expectedColumn = nameof(TestEntity.ReferenceTypeProperty).ToLower();

            // Act
            ColumnSet columnSet = new ColumnSet();
            columnSet.AddColumn<TestEntity>(t => t.ReferenceTypeProperty);

            // Assert
            Assert.AreEqual(1, columnSet.Columns.Count);
            Assert.AreEqual(expectedColumn, columnSet.Columns[0]);
        }

        [TestMethod()]
        public void AddColumnsTest()
        {
            // Setup
            string expectedColumn1 = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            string expectedColumn2 = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            ColumnSet columnSet = new ColumnSet();
            columnSet.AddColumns<TestEntity>(t => t.ReferenceTypeProperty, t=> t.ValueTypeProperty);

            // Assert
            Assert.AreEqual(2, columnSet.Columns.Count);
            Assert.AreEqual(expectedColumn1, columnSet.Columns[0]);
            Assert.AreEqual(expectedColumn2, columnSet.Columns[1]);
        }
    }
}