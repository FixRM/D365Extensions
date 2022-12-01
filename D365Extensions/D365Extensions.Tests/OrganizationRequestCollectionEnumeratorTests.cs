using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class OrganizationRequestCollectionEnumeratorTests
    {
        [TestMethod()]
        public void Should_Throw_If_Incorrect_Size_Test()
        {
            //Setup
            var collection = new List<OrganizationRequest>();
            var size = -10;

            //Act + Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=> collection.Chunk(size));
        }

        [TestMethod()]
        public void Should_Retun_Zero_Length_Test()
        {
            //Setup
            var collection = new List<OrganizationRequest>();
            var size = 10;

            //Act
            var result = collection.Chunk(size).ToList();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod()]
        public void Should_Return_Whole_Collection_Of_Same_SizeTest()
        {
            //Setup
            var collection = new List<OrganizationRequest>()
            {
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
            };

            var size = collection.Count;

            //Act
            var result = collection.Chunk(size).ToList();
            
            //Assert
            Assert.AreEqual(1, result.Count);
            CollectionAssert.AreEqual(collection, result[0]);
        }


        [TestMethod()]
        public void Should_Return_Whole_Collection_Test()
        {
            //Setup
            var collection = new List<OrganizationRequest>()
            {
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
            };

            var size = collection.Count + 1;

            //Act
            var result = collection.Chunk(size).ToList();

            //Assert
            Assert.AreEqual(1, result.Count);
            CollectionAssert.AreEqual(collection, result[0]);
        }

        [TestMethod()]
        public void Should_Split_Collection_On_Two_Equal_Parts()
        {
            //Setup
            var collection = new List<OrganizationRequest>()
            {
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
            };

            var size = collection.Count / 2;

            //We dont use Skip+Take in real method as they iterate over collection from the beginning for each attempt
            List<OrganizationRequest> expectedChunk1 = collection.Take(size).ToList();
            List<OrganizationRequest> expectedChunk2 = collection.Skip(size).Take(size).ToList();

            //Act
            var result = collection.Chunk(size).ToList();
            var actualChunk1 = result[0];
            var actualChunk2 = result[1];

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(expectedChunk1, actualChunk1);
            CollectionAssert.AreEqual(expectedChunk2, actualChunk2);
        }

        [TestMethod()]
        public void Should_Split_Collection_On_Two_Parts()
        {
            //Setup
            var collection = new List<OrganizationRequest>()
            {
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
            };

            var size = collection.Count / 2 + 1;

            //We dont use Skip+Take in real method as they iterate over collection from the beginning for each attempt
            List<OrganizationRequest> expectedChunk1 = collection.Take(size).ToList();
            List<OrganizationRequest> expectedChunk2 = collection.Skip(size).Take(size).ToList();

            //Act
            var result = collection.Chunk(size).ToList();
            var actualChunk1 = result[0];
            var actualChunk2 = result[1];

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEqual(expectedChunk1, actualChunk1);
            CollectionAssert.AreEqual(expectedChunk2, actualChunk2);
        }
    }
}