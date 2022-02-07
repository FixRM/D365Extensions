using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Tests
{
    [TestClass()]
    public class DataCollectionExtensionsTests
    {
        [TestMethod()]
        public void GetValueTest()
        {
            // Setup
            string expectedSring = "Artem";
            int? expectedInt = 36;
            DateTime? expectedDateTime = new DateTime(1985, 8, 8);

            ParameterCollection paramCollection = new ParameterCollection();
            paramCollection.Add("name", expectedSring);
            paramCollection.Add("age", expectedInt);
            paramCollection.Add("birthdate", expectedDateTime);

            // Act
            string actualString = paramCollection.GetValue<string>("name");
            int? actualInt = paramCollection.GetValue<int?>("age");
            DateTime? actualDateTime = paramCollection.GetValue<DateTime?>("birthdate");

            //Assert
            Assert.AreEqual(expectedSring, actualString);
            Assert.AreEqual(expectedInt, actualInt);
            Assert.AreEqual(expectedDateTime, actualDateTime);

            Assert.IsNull(paramCollection.GetValue<string>("notexisting"));
        }
    }
}