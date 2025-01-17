﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class ColumnSetTests
    {
        [TestMethod()]
        public void Defaut_Constructor_Test()
        {
            // Act
            ColumnSet columnSet = new ColumnSet<TestEntity>();

            var query = new QueryExpression();

            // Assert
            var actual = columnSet.Columns;

            Assert.AreEqual(0, actual.Count);
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

        [TestMethod()]
        public void ColumnSet_Test()
        {
            // Setup
            Expression<Func<TestEntity, object>> expected1 = (t) => t.ReferenceTypeProperty;
            Expression<Func<TestEntity, object>> expected2 = (t) => t.ValueTypeProperty;

            // Act
            ColumnSet<TestEntity> columnSet = new ColumnSet<TestEntity>(
                expected1,
                expected2);

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
            Expression<Func<TestEntity, object>> expected = (t) => t.ReferenceTypeProperty;

            // Act
            columnSet.AddColumn(t => t.ReferenceTypeProperty);
            var actual = columnSet.Columns;

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(LogicalName.GetName(expected), LogicalName.GetName(actual[0]));
        }

        [TestMethod()]
        public void AddColumns_Test()
        {
            // Setup
            ColumnSet<TestEntity> columnSet = new ColumnSet<TestEntity>();
            Expression<Func<TestEntity, object>> expected1 = (t) => t.ReferenceTypeProperty;
            Expression<Func<TestEntity, object>> expected2 = (t) => t.ValueTypeProperty;

            // Act
            columnSet.AddColumns(t => t.ReferenceTypeProperty, t => t.ValueTypeProperty);
            var actual = columnSet.Columns;

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(LogicalName.GetName(expected1), LogicalName.GetName(actual[0]));
            Assert.AreEqual(LogicalName.GetName(expected2), LogicalName.GetName(actual[1]));
        }

        [TestMethod()]
        public void Null_Test()
        {
            // Setup
            ColumnSet<TestEntity> columnSetT = null;

            // Act
            ColumnSet columnSet = columnSetT;

            // Assert
            Assert.IsNull(columnSet);
        }
    }
}