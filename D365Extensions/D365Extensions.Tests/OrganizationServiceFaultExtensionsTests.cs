using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class OrganizationServiceFaultExtensionsTests
    {
        [TestMethod()]
        public void GetKnownErrorCodeTest()
        {
            // Setup
            const ErrorCodes expectedErrorCode = ErrorCodes.AccessDenied;

            var fault = new OrganizationServiceFault();
            fault.ErrorCode = (int)expectedErrorCode;

            var exception = new FaultException<OrganizationServiceFault>(fault);

            // Act
            var actualErrorCode = exception.GetErrorCode();

            // Assert
            Assert.IsNotNull(actualErrorCode);
            Assert.AreEqual(expectedErrorCode, actualErrorCode);
        }

        [TestMethod()]
        public void GetUnknownErrorCodeTest()
        {
            // Setup
            var fault = new OrganizationServiceFault();
            fault.ErrorCode = 42;

            var exception = new FaultException<OrganizationServiceFault>(fault);

            // Act
            var error = exception.GetErrorCode();

            // Assert
            Assert.IsNull(error);
        }
    }
}