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
    public class ColumnSetTests
    {
        [TestMethod()]
        public void ColumnSet_Test()
        {
            // Setup
            string expected1 = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            string expected2 = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            ColumnSet<TestEntity> columnSet = new ColumnSet<TestEntity>(
                t => t.ReferenceTypeProperty,
                t => t.ValueTypeProperty);

            var actual = columnSet.Columns;

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void AddColumn_Test()
        {
            // Setup
            ColumnSet<TestEntity> columnSet = new ColumnSet<TestEntity>();
            string expected = nameof(TestEntity.ReferenceTypeProperty).ToLower();

            // Act
            columnSet.AddColumn(t => t.ReferenceTypeProperty);
            var actual = columnSet.Columns;

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod()]
        public void AddColumns_Test()
        {
            // Setup
            ColumnSet<TestEntity> columnSet = new ColumnSet<TestEntity>();
            string expected1 = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            string expected2 = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            columnSet.AddColumns(t => t.ReferenceTypeProperty, t => t.ValueTypeProperty);
            var actual = columnSet.Columns;

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void Implicit_Cast_Test()
        {
            // Setup
            string expected1 = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            string expected2 = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            ColumnSet columnSet = new ColumnSet<TestEntity>(
                t => t.ReferenceTypeProperty,
                t => t.ValueTypeProperty);

            var actual = columnSet.Columns;

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }
    }
}