#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class ITracingServiceExtensionsTests
    {
        [TestMethod()]
        public void TracePluginContextTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            var pluginContext = new XrmFakedPluginExecutionContext()
            {
                MessageName = "Create",
                Stage = 10,
                PrimaryEntityId = Guid.NewGuid(),
                PrimaryEntityName = "account",
                UserId = Guid.NewGuid(),
                InitiatingUserId = Guid.NewGuid(),
                Depth = 1,
                Mode = 1
            };

            string expectedTraceString =
$@"MessageName: ""{pluginContext.MessageName}""
Stage: {pluginContext.Stage}
PrimaryEntityId: ""{{{pluginContext.PrimaryEntityId}}}""
PrimaryEntityName: ""{pluginContext.PrimaryEntityName}""
UserId: ""{{{pluginContext.UserId}}}""
InitiatingUserId: ""{{{pluginContext.InitiatingUserId}}}""
Depth: {pluginContext.Depth}
Mode: {pluginContext.Mode}
";

            //Act
            traceService.TracePluginContext(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TraceInputParametersTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            const string targetKey = "Target";
            Entity target = new Entity("account");

            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.InputParameters.Add(targetKey, target);

            string expectedTraceString =
$@"InputParameters: ParameterCollection {{ Count = 1 }}
""{targetKey}"": Entity {{ LogicalName = ""account"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 0 }}
";

            //Act
            traceService.TraceInputParameters(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TraceOutputParametersTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            const string idKey = "Id";
            var id = new Guid("00000000-0000-0000-0000-000000000000");

            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.OutputParameters.Add(idKey, id);

            string expectedTraceString =
$@"OutputParameters: ParameterCollection {{ Count = 1 }}
""{idKey}"": ""{{00000000-0000-0000-0000-000000000000}}""
";

            //Act
            traceService.TraceOutputParameters(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TraceSharedVariablesTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            const string variableKey = "TraceEnabled";
            const string variableValue = "true";

            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.SharedVariables.Add(variableKey, variableValue);

            string expectedTraceString =
$@"SharedVariables: ParameterCollection {{ Count = 1 }}
""{variableKey}"": ""{variableValue}""
";

            //Act
            traceService.TraceSharedVariables(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TracePreEntityImagesTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            const string imageName = "PreImage";
            Entity image = new Entity("account");

            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.PreEntityImages.Add(imageName, image);

            string expectedTraceString =
$@"PreEntityImages: EntityImageCollection {{ Count = 1 }}
""{imageName}"": Entity {{ LogicalName = ""account"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 0 }}
";

            //Act
            traceService.TracePreEntityImages(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TracePostEntityImagesTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var traceService = context.GetFakeTracingService();

            const string imageName = "PostImage";
            Entity image = new Entity("account");

            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.PostEntityImages.Add(imageName, image);

            string expectedTraceString =
$@"PostEntityImages: EntityImageCollection {{ Count = 1 }}
""{imageName}"": Entity {{ LogicalName = ""account"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 0 }}
";

            //Act
            traceService.TracePostEntityImages(pluginContext);

            //Assert
            string actualTraceString = traceService.DumpTrace();

            Assert.AreEqual(expectedTraceString, actualTraceString);
        }
    }
}