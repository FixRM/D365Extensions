using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class RelatedEntityCollectionExtensionsTests
    {

        [TestMethod()]
        public void GetShouldReturnCollectionTest()
        {
            //Setup
            var schemaName = "relationship_schema";
            var relationship = new Relationship(schemaName);

            var expectedCollection = new EntityCollection();
            expectedCollection.EntityName = Account.EntityLogicalName;

            var reCollection = new RelatedEntityCollection();
            reCollection.Add(relationship, expectedCollection);

            //Act
            var actualCollection = reCollection.Get(schemaName);

            //Assert
            Assert.AreEqual(expectedCollection, actualCollection);
        }

        [TestMethod()]
        public void GetShouldReturnNullTest()
        {
            //Setup
            var schemaName = "relationship_schema";
            var relationship = new Relationship()
            {
                SchemaName = schemaName,
                PrimaryEntityRole = EntityRole.Referencing
            };

            var expectedCollection = new EntityCollection();
            expectedCollection.EntityName = Account.EntityLogicalName;

            var reCollection = new RelatedEntityCollection();
            reCollection.Add(relationship, expectedCollection);

            //Act
            var actualCollection = reCollection.Get(schemaName);

            //Assert
            Assert.IsNull(actualCollection);
        }

        [TestMethod()]
        public void GetOrAddShouldReturnExistingCollectionTest()
        {
            //Setup
            var relationship = new Relationship()
            {
                SchemaName = "relationship_schema",
                PrimaryEntityRole = EntityRole.Referencing
            };

            var expectedCollection = new EntityCollection();
            expectedCollection.EntityName = Account.EntityLogicalName;

            var reCollection = new RelatedEntityCollection();
            reCollection.Add(relationship, expectedCollection);

            //Act
            var actualCollection = reCollection.GetOrAdd(relationship);

            //Assert
            Assert.AreEqual(expectedCollection, actualCollection);
        }

        [TestMethod()]
        public void GetOrAddShouldAddNewCollectionTest()
        {
            //Setup
            var relationship = new Relationship("relationship_schema");

            var reCollection = new RelatedEntityCollection();

            //Act
            var actualCollection = reCollection.GetOrAdd(relationship);

            //Assert
            Assert.IsNotNull(actualCollection);
            Assert.AreEqual(reCollection[relationship], actualCollection);
        }


        [TestMethod()]
        public void AddToShouldAddToExistingCollectionTest()
        {
            //Setup
            var relationship = new Relationship()
            {
                SchemaName = "relationship_schema",
                PrimaryEntityRole = EntityRole.Referencing
            };

            var expectedCollection = new EntityCollection();
            expectedCollection.EntityName = Account.EntityLogicalName;

            var reCollection = new RelatedEntityCollection();
            reCollection.Add(relationship, expectedCollection);

            var entity = new Entity();

            //Act
            reCollection.AddTo(relationship, entity);

            //Assert
            Assert.AreEqual(1, expectedCollection.Entities.Count);
            Assert.AreEqual(entity, expectedCollection.Entities[0]);
        }

        [TestMethod()]
        public void AddToShouldAddToNewCollectionTest()
        {
            //Setup
            var relationship = new Relationship()
            {
                SchemaName = "relationship_schema",
                PrimaryEntityRole = EntityRole.Referencing
            };

            var reCollection = new RelatedEntityCollection();

            var entity = new Entity();

            //Act
            reCollection.AddTo(relationship, entity);

            //Assert
            Assert.AreEqual(1, reCollection.Count);
            Assert.AreEqual(entity, reCollection[relationship].Entities[0]);
        }

        [TestMethod()]
        public void AddToShouldAddRangeToExistingCollectionTest()
        {
            //Setup
            var relationship = new Relationship()
            {
                SchemaName = "relationship_schema",
                PrimaryEntityRole = EntityRole.Referencing
            };

            var expectedCollection = new EntityCollection();
            expectedCollection.EntityName = Account.EntityLogicalName;

            var reCollection = new RelatedEntityCollection();
            reCollection.Add(relationship, expectedCollection);

            var entity1 = new Entity();
            var entity2 = new Entity();

            //Act
            reCollection.AddTo(relationship, [entity1, entity2]);

            //Assert
            Assert.AreEqual(2, expectedCollection.Entities.Count);
            Assert.AreEqual(entity1, expectedCollection.Entities[0]);
            Assert.AreEqual(entity2, expectedCollection.Entities[1]);
        }

        [TestMethod()]
        public void AddToShouldAddRangeToNewCollectionTest()
        {
            //Setup
            var relationship = new Relationship()
            {
                SchemaName = "relationship_schema",
                PrimaryEntityRole = EntityRole.Referencing
            };

            var reCollection = new RelatedEntityCollection();

            var entity1 = new Entity();
            var entity2 = new Entity();

            //Act
            reCollection.AddTo(relationship, [entity1, entity2]);

            //Assert
            Assert.AreEqual(1, reCollection.Count);
            Assert.AreEqual(entity1, reCollection[relationship].Entities[0]);
            Assert.AreEqual(entity2, reCollection[relationship].Entities[1]);
        }
    }
}