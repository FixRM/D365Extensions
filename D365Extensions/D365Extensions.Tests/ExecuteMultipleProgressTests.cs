using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class ExecuteMultipleProgressTests
    {
        [DataTestMethod]
        [DataRow(100.0F, 0, 1000, 10000, DisplayName = "Progress should show 100% if 0 is queried test")]
        [DataRow(25.0F, 100, 25, 0, DisplayName = "Progress should show 25% if 100 is queried and 25 processed test")]
        [DataRow(50.0F, 100, 25, 25, DisplayName = "Progress should show 50% if 100 is queried and 25 processed and 25 skipped test")]
        [DataRow(100.0F, 100, 100, 0, DisplayName = "Progress should show 100% if 100 is queried and 100 processed test")]
        [DataRow(100.0F, 100, 0, 100, DisplayName = "Progress should show 100% if 100 is queried and 100 skipped test")]
        public void ProgressTest(float expectedProgress, int queried, int processed, int skipped)
        {
            // Setup
            const uint errors = 200000000;

            // Act
            var emProgress = new ExecuteMultipleProgress((uint)queried, (uint)processed, (uint)skipped, errors);

            // Assert
            Assert.AreEqual<float>(expectedProgress, emProgress.Progress);
        }
    }
}
