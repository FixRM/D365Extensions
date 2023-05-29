#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class PluginExecutionTraceContextTests
    {

        [TestMethod()]
        public void PluginExecutionTraceContextDefaultSettingsTest()
        {
            // Setup
            var xrmContext = new XrmFakedContext();

            Guid userId = Guid.NewGuid();
            const string entityName = "account";

            var pluginContext = xrmContext.GetDefaultPluginContext();
            pluginContext.PrimaryEntityName = entityName;
            pluginContext.UserId = userId;
            pluginContext.InitiatingUserId = userId;

            var traceService = xrmContext.GetFakeTracingService();

            const string message = "test message";

            // by default we don't trace empty collection properties of IPluginExecutionContext
            var expectedTraceString =
$@"MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: 1
Mode: 0
test message

";

            // Act
            try
            {
                using (var traceContext = new PluginExecutionTraceContext(traceService, pluginContext))
                {
                    traceContext.Trace(message);

                    //by default we will trace only if error occurs
                    throw new InvalidPluginExecutionException();
                }
            }
            catch { }

            var actualTraceString = traceService.DumpTrace();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void PluginExecutionTraceContextCustomSettingsTest()
        {
            // Setup
            var xrmContext = new XrmFakedContext();

            Guid userId = Guid.NewGuid();
            const string entityName = "account";

            var pluginContext = xrmContext.GetDefaultPluginContext();
            pluginContext.PrimaryEntityName = entityName;
            pluginContext.UserId = userId;
            pluginContext.InitiatingUserId = userId;

            var traceService = xrmContext.GetFakeTracingService();

            const string message = "test message";

            var expectedTraceString =
$@"MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: 1
Mode: 0
InputParameters: ParameterCollection {{ Count = 0 }}
PreEntityImages: EntityImageCollection {{ Count = 0 }}
SharedVariables: ParameterCollection {{ Count = 0 }}
OutputParameters: ParameterCollection {{ Count = 0 }}
PostEntityImages: EntityImageCollection {{ Count = 0 }}
test message

";

            // Act
            PluginExecutionTraceContextSettings settings = new PluginExecutionTraceContextSettings()
            {
                ErrorOnly = false,
                ShowEmptyCollections = true
            };

            using (var traceContext = new PluginExecutionTraceContext(traceService, pluginContext, settings))
            {
                traceContext.Trace(message);
            }

            var actualTraceString = traceService.DumpTrace();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void DisposeTest()
        {
            // Setup
            var xrmContext = new XrmFakedContext();
            var pluginContext = xrmContext.GetDefaultPluginContext();

            var traceService = xrmContext.GetFakeTracingService();

            const string message = "test message";

            // by default we trace only if error occurs
            var expectedTraceString = "";

            // Act
            using (var traceContext = new PluginExecutionTraceContext(traceService, pluginContext))
            {
                traceContext.Trace(message);
            }

            var actualTraceString = traceService.DumpTrace();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TraceTest()
        {
            // Setup
            var xrmContext = new XrmFakedContext();

            Guid userId = Guid.NewGuid();
            const string entityName = "account";

            var pluginContext = xrmContext.GetDefaultPluginContext();
            pluginContext.PrimaryEntityName = entityName;
            pluginContext.UserId = userId;
            pluginContext.InitiatingUserId = userId;

            var traceService = xrmContext.GetFakeTracingService();

            const string message = "test message with arg1={0} and arg2={1}";
            const string arg1 = "FixRM";
            const int arg2 = 100;

            var expectedTraceString =
$@"MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: 1
Mode: 0
test message with arg1={arg1} and arg2={arg2}
test message with arg1={arg1} and arg2={arg2}

";

            // Act
            PluginExecutionTraceContextSettings settings = new PluginExecutionTraceContextSettings()
            {
                ErrorOnly = false
            };

            using (var traceContext = new PluginExecutionTraceContext(traceService, pluginContext, settings))
            {
                traceContext.Trace(message, arg1, arg2);
                traceContext.Trace(message, arg1, arg2);
            }

            var actualTraceString = traceService.DumpTrace();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void TraceParentContextTest()
        {
            // Setup
            var xrmContext = new XrmFakedContext();

            Guid userId = Guid.NewGuid();
            const string entityName = "account";

            var pluginContext3 = new XrmFakedPluginExecutionContext
            {
                PrimaryEntityName = entityName,
                UserId = userId,
                InitiatingUserId = userId,
                Depth = 3,
            };

            var pluginContext2 = new XrmFakedPluginExecutionContext
            {
                PrimaryEntityName = entityName,
                UserId = userId,
                InitiatingUserId = userId,
                Depth = 2,
                ParentContext = pluginContext3
            };

            var pluginContext = new XrmFakedPluginExecutionContext
            {
                PrimaryEntityName = entityName,
                UserId = userId,
                InitiatingUserId = userId,
                Depth = 1,
                ParentContext = pluginContext2
            };

            var traceService = xrmContext.GetFakeTracingService();

            const string message = "test message";

            var expectedTraceString =
$@"MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: {pluginContext.Depth}
Mode: 0
Parent context:
MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: {pluginContext2.Depth}
Mode: 0
Parent context:
MessageName: ""Create""
Stage: 0
PrimaryEntityId: ""{{00000000-0000-0000-0000-000000000000}}""
PrimaryEntityName: ""{entityName}""
UserId: ""{{{userId}}}""
InitiatingUserId: ""{{{userId}}}""
Depth: {pluginContext3.Depth}
Mode: 0
test message

";

            // Act
            PluginExecutionTraceContextSettings settings = new PluginExecutionTraceContextSettings()
            {
                ErrorOnly = false,
                IncludeParentContext = true
            };

            using (var traceContext = new PluginExecutionTraceContext(traceService, pluginContext, settings))
            {
                traceContext.Trace(message);
            }

            var actualTraceString = traceService.DumpTrace();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }
    }
}