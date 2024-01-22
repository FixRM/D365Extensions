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
    public class EntityImageCollectionExtensionsTests
    {
        [TestMethod()]
        public void Should_Return_Null_Test()
        {

            // Setup
            var images = new EntityImageCollection();

            // Act
            var nullImage = images.GetImage<Account>("PreImage");

            // Assert
            Assert.IsNull(nullImage);
        }

        [TestMethod()]
        public void Should_Return_Image_Test()
        {
            // Setup
            var image = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = Account.EntityLogicalName,
                [nameof(Account.Name).ToLower()] = "Test Account"
            };

            var expectedImageName = "PreImage";

            var images = new EntityImageCollection
            {
                { expectedImageName, image }
            };

            // Act
            var actualImage = images.GetImage<Account>(expectedImageName);

            // Assert
            Assert.IsNotNull(actualImage);

            Assert.AreEqual(image.Id, actualImage.Id);
        }
    }
}