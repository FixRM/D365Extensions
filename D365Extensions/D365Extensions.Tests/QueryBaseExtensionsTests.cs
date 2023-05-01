﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
