using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System.Xml.Linq;

namespace D365Extensions.Tests
{
    [TestClass]
    public class QueryBaseExtensionsTests
    {
        [TestMethod]
        public void FetchNextPageTest()
        {
            /// Setup
            string expectedCookie = "<cookie page=\"1\"><accountid last=\"{ E79B8B61 - 9658 - 460F - B6B5 - A11CECF9F872}\" first=\"{ D65C4096 - B356 - 4EDB - A6C6 - A12EDA3F34EF}\" /></cookie>";

            string fetch = @"<fetch count='5000' no-lock='true' page='0' >
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                             </fetch>";

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            (query as QueryBase).NextPage(expectedCookie);

            /// Assert
            XDocument document = XDocument.Parse(query.Query);
            string page = document.Root.Attribute("page").Value;
            string actualCookie = document.Root.Attribute("paging-cookie").Value;

            Assert.AreEqual("1", page);
            Assert.AreEqual(expectedCookie, actualCookie);
        }

        [TestMethod]
        public void FetchNextPage_NoPage_Test()
        {
            /// Setup
            string expectedCookie = "<cookie page=\"1\"><accountid last=\"{ E79B8B61 - 9658 - 460F - B6B5 - A11CECF9F872}\" first=\"{ D65C4096 - B356 - 4EDB - A6C6 - A12EDA3F34EF}\" /></cookie>";

            string fetch = @"<fetch count='5000' no-lock='true' >
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                             </fetch>";

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            (query as QueryBase).NextPage(expectedCookie);

            /// Assert
            XDocument document = XDocument.Parse(query.Query);
            string page = document.Root.Attribute("page").Value;
            string actualCookie = document.Root.Attribute("paging-cookie").Value;

            Assert.AreEqual("1", page);
            Assert.AreEqual(expectedCookie, actualCookie);
        }

        [TestMethod]
        public void QueryExpressionNextPageTest()
        {
            /// Setup
            string expectedCookie = "<cookie page=\"1\"><accountid last=\"{ E79B8B61 - 9658 - 460F - B6B5 - A11CECF9F872}\" first=\"{ D65C4096 - B356 - 4EDB - A6C6 - A12EDA3F34EF}\" /></cookie>";

            QueryExpression query = new QueryExpression();

            // Act
            (query as QueryBase).NextPage(expectedCookie);

            //Assert
            Assert.AreEqual(expectedCookie, query.PageInfo.PagingCookie);
            Assert.AreEqual(1, query.PageInfo.PageNumber);
        }

        [TestMethod]
        public void QueryByAttributeNextPageTest()
        {
            /// Setup
            string expectedCookie = "<cookie page=\"1\"><accountid last=\"{ E79B8B61 - 9658 - 460F - B6B5 - A11CECF9F872}\" first=\"{ D65C4096 - B356 - 4EDB - A6C6 - A12EDA3F34EF}\" /></cookie>";

            QueryByAttribute query = new QueryByAttribute();

            // Act
            (query as QueryBase).NextPage(expectedCookie);

            //Assert
            Assert.AreEqual(expectedCookie, query.PageInfo.PagingCookie);
            Assert.AreEqual(1, query.PageInfo.PageNumber);
        }

        [TestMethod]
        public void QueryByAttributeNextPage_No_PageTest()
        {
            /// Setup
            string expectedCookie = "<cookie page=\"1\"><accountid last=\"{ E79B8B61 - 9658 - 460F - B6B5 - A11CECF9F872}\" first=\"{ D65C4096 - B356 - 4EDB - A6C6 - A12EDA3F34EF}\" /></cookie>";

            //this constructor doesn't initialize PageInfo
            QueryByAttribute query = new QueryByAttribute(Account.EntityLogicalName);

            // Act
            (query as QueryBase).NextPage(expectedCookie);

            //Assert
            Assert.AreEqual(expectedCookie, query.PageInfo.PagingCookie);
            Assert.AreEqual(1, query.PageInfo.PageNumber);
        }

        [TestMethod]
        public void FetchExpressionGetPageNumberTest()
        {
            /// Setup
            string fetch = @"<fetch count='5000' no-lock='true' page='0' >
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                             </fetch>";

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            int actualPageNumber = (query as QueryBase).GetPageNumber();

            /// Assert
            Assert.AreEqual(0, actualPageNumber);            
        }

        [TestMethod]
        public void FetchExpressionGetPageNumberTest_NoPage_Test()
        {
            /// Setup
            string fetch = @"<fetch count='5000' no-lock='true' >
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                             </fetch>";

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            int actualPageNumber = (query as QueryBase).GetPageNumber();

            /// Assert
            Assert.AreEqual(0, actualPageNumber);
        }

        [TestMethod]
        public void QueryExpressionGetPageNumberTest()
        {
            /// Setup
            QueryExpression query = new QueryExpression();

            // Act
            int actualPageNumber = (query as QueryBase).GetPageNumber();

            //Assert
            Assert.AreEqual(0, actualPageNumber);
        }

        [TestMethod]
        public void QueryByAttributeGetPageNumberTest()
        {
            /// Setup
            QueryByAttribute query = new QueryByAttribute();

            // Act
            int actualPageNumber = (query as QueryBase).GetPageNumber();

            //Assert
            Assert.AreEqual(0, actualPageNumber);
        }

        [TestMethod]
        public void QueryByAttributeGetPageNumber_No_Page_Test()
        {
            /// Setup
            ///this constructor doesn't initialize PageInfo
            QueryByAttribute query = new QueryByAttribute(Account.EntityLogicalName);

            // Act
            int actualPageNumber = (query as QueryBase).GetPageNumber();

            //Assert
            Assert.AreEqual(0, actualPageNumber);
        }

        [TestMethod]
        public void FetchExpressionSetTopCountTest()
        {
            /// Setup
            var expectedTopCount = 100; 

            string fetch = $"""
                <fetch no-lock='true' top='{expectedTopCount * 5}' >
                    <entity name='account' >
                        <attribute name='name' />
                    </entity>
                </fetch>
                """;

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            (query as QueryBase).SetTopCount(expectedTopCount);

            var xDocument = XDocument.Parse(query.Query);
            var actualTopCount = xDocument.Root.Attribute("top")?.Value;

            /// Assert
            Assert.IsNotNull(actualTopCount);
            Assert.AreEqual(expectedTopCount, int.Parse(actualTopCount));
        }

        [TestMethod]
        public void FetchExpressionSetTopCount_NoAttribute_Test()
        {
            /// Setup
            var expectedTopCount = 100;

            string fetch = $"""
                <fetch no-lock='true' >
                    <entity name='account' >
                        <attribute name='name' />
                    </entity>
                </fetch>
                """;

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            (query as QueryBase).SetTopCount(expectedTopCount);

            var xDocument = XDocument.Parse(query.Query);
            var actualTopCount = xDocument.Root.Attribute("top")?.Value;

            /// Assert
            Assert.IsNotNull(actualTopCount);
            Assert.AreEqual(expectedTopCount, int.Parse(actualTopCount));
        }

        [TestMethod]
        public void QueryExpressionSetTopCountTest()
        {
            /// Setup
            var expectedTopCount = 100;
            
            QueryExpression query = new QueryExpression();

            // Act
            (query as QueryBase).SetTopCount(expectedTopCount);

            //Assert
            Assert.AreEqual(expectedTopCount, query.TopCount);
        }

        [TestMethod]
        public void QueryByAttributeSetTopCountTest()
        {
            /// Setup
            var expectedTopCount = 100;

            QueryByAttribute query = new QueryByAttribute();

            // Act
            (query as QueryBase).SetTopCount(expectedTopCount);

            //Assert
            Assert.AreEqual(expectedTopCount, query.TopCount);
        }
    }
}