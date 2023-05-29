using FakeItEasy;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
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
    public class IServiceProviderExtensionsTests
    {
        [TestMethod()]
        public void GetPluginExecutionContextTest()
        {
            // Setup
            var expectedPluginExecutionContext = A.Fake<IPluginExecutionContext>();

            var serviceProvider = A.Fake<IServiceProvider>();

            var call = A.CallTo(()=> serviceProvider.GetService(A<Type>.That.IsEqualTo(typeof(IPluginExecutionContext))));
            call.Returns(expectedPluginExecutionContext);

            // Act
            var actualPluginExecutionContext = serviceProvider.GetPluginExecutionContext();

            // Assert
            Assert.AreEqual(expectedPluginExecutionContext, actualPluginExecutionContext);
            
            call.MustHaveHappenedOnceExactly();
        }

        [TestMethod()]
        public void GetOrganizationServiceFactoryTest()
        {
            // Setup
            var expectedOrgServiceFactory = A.Fake<IOrganizationServiceFactory>();

            var serviceProvider = A.Fake<IServiceProvider>();

            var call = A.CallTo(() => serviceProvider.GetService(A<Type>.That.IsEqualTo(typeof(IOrganizationServiceFactory))));
            call.Returns(expectedOrgServiceFactory);

            // Act
            var actualOrgServiceFactory = serviceProvider.GetOrganizationServiceFactory();

            // Assert
            Assert.AreEqual(expectedOrgServiceFactory, actualOrgServiceFactory);

            call.MustHaveHappenedOnceExactly();
        }

        [TestMethod()]
        public void GetTracingServiceTest()
        {
            // Setup
            var expectedTracingService = A.Fake<ITracingService>();

            var serviceProvider = A.Fake<IServiceProvider>();

            var call = A.CallTo(() => serviceProvider.GetService(A<Type>.That.IsEqualTo(typeof(ITracingService))));
            call.Returns(expectedTracingService);

            // Act
            var actualTracingService = serviceProvider.GetTracingService();

            // Assert
            Assert.AreEqual(expectedTracingService, actualTracingService);

            call.MustHaveHappenedOnceExactly();
        }

        [TestMethod()]
        public void GetPluginExecutionTraceContextTest()
        {
            // Setup
            var expectedTracingService = A.Fake<ITracingService>();
            var expectedPluginExecutionContext = A.Fake<IPluginExecutionContext>();

            var serviceProvider = A.Fake<IServiceProvider>();

            var callGetTracingService = A.CallTo(() => serviceProvider.GetService(A<Type>.That.IsEqualTo(typeof(ITracingService))));
            callGetTracingService.Returns(expectedTracingService);

            var callGetPluginContext = A.CallTo(() => serviceProvider.GetService(A<Type>.That.IsEqualTo(typeof(IPluginExecutionContext))));
            callGetPluginContext.Returns(expectedPluginExecutionContext);

            var expectedSettings = new PluginExecutionTraceContextSettings();

            // Act
            var traceContext = serviceProvider.GetPluginExecutionTraceContext(expectedSettings);

            // Assert
            Assert.AreEqual(expectedPluginExecutionContext, traceContext.PluginExecutionContext);
            Assert.AreEqual(expectedTracingService, traceContext.TracingService);
            Assert.AreEqual(expectedSettings, traceContext.Settings);

            callGetTracingService.MustHaveHappenedOnceExactly();
            callGetPluginContext.MustHaveHappenedOnceExactly();
        }
    }
}