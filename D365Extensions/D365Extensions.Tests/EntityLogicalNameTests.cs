using Microsoft.VisualStudio.TestTools.UnitTesting;
using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D365Extensions.Tests.Entities;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class EntityLogicalNameTests
    {
        [TestMethod()]
        public void GetNameTest()
        {
            // Setup
            string expectedName = Account.EntityLogicalName;
            string expectedName2 = CustomEntity.EnityLogicalName;

            // Act
            string actualName = EntityLogicalName.GetName<Account>();
            string actualName2 = EntityLogicalName.GetName<CustomEntity>();

            // Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedName2, actualName2);
        }

        [TestMethod()]
        public void Not_Decorated_Entity_Test()
        {
            // Setup
            string expectedEntityName = nameof(NotDecoratedEntity).ToLowerInvariant();

            // Act
            string actuaEntityName = EntityLogicalName.GetName<NotDecoratedEntity>();

            // Assert
            Assert.AreEqual(expectedEntityName, actuaEntityName);
        }
    }
}