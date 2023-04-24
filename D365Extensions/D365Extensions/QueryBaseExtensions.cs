using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Extensions for Microsoft.Xrm.Sdk.Query types
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Universal method to set Query paging parameters to next page
        /// </summary>
        public static void NextPage(this QueryBase query, string pagingCookie)
        {
            switch (query)
            {
                case QueryExpression qe:
                    qe.NextPage(pagingCookie);
                    break;
                case QueryByAttribute qa:
                    qa.NextPage(pagingCookie);
                    break;
                case FetchExpression fe:
                    fe.NextPage(pagingCookie);
                    break;
            }
        }

        /// <summary>
        /// Sets QueryExpression paging parameters to next page
        /// </summary>
        public static void NextPage(this QueryExpression query, string pagingCookie)
        {
            query.PageInfo.PageNumber++;
            query.PageInfo.PagingCookie = pagingCookie;
        }

        /// <summary>
        /// Sets QueryByAttribute paging parameters to next page
        /// </summary>
        public static void NextPage(this QueryByAttribute query, string pagingCookie)
        {
            query.PageInfo.PageNumber++;
            query.PageInfo.PagingCookie = pagingCookie;
        }

        /// <summary>
        /// Sets FetchExpression paging parameters to next page
        /// </summary>
        public static void NextPage(this FetchExpression query, string pagingCookie)
        {
            XDocument xDocument = XDocument.Parse(query.Query);
            NextPage(query, xDocument, pagingCookie);
        }

        /// <summary>
        /// Internal method for better FetchExpression paging performance
        /// </summary>
        internal static void NextPage(this FetchExpression query, XDocument xDocument, string pagingCookie)
        {
            /// No paging mean PageInfo.Page = 0
            /// for FetchXml it mean no "page" attribute or 0 value
            string page = xDocument.Root.Attribute("page")?.Value;
            int pageNumber = page != null ? int.Parse(page) : 0;

            xDocument.Root.SetAttributeValue("paging-cookie", pagingCookie);
            xDocument.Root.SetAttributeValue("page", ++pageNumber);

            query.Query = xDocument.ToString();
        }

        /// <summary>
        /// Universal method to get Query page number
        /// </summary>
        public static int GetPageNumber(this QueryBase query)
        {
            switch (query)
            {
                case QueryExpression qe: return qe.GetPageNumber();
                case QueryByAttribute qa: return qa.GetPageNumber();
                case FetchExpression fe: return fe.GetPageNumber();
                default: throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets QueryExpression page number
        /// </summary>
        public static int GetPageNumber(this QueryExpression query)
        {
            return query.PageInfo.PageNumber;
        }

        /// <summary>
        /// Gets QueryByAttribute page number
        /// </summary>
        public static int GetPageNumber(this QueryByAttribute query)
        {
            return query.PageInfo.PageNumber;
        }

        /// <summary>
        /// Gets FetchExpression page number
        /// </summary>
        public static int GetPageNumber(this FetchExpression query)
        {
            return GetPageNumber(query, null);
        }

        /// <summary>
        /// Internal method for better FetchExpression paging performance
        /// </summary>
        internal static int GetPageNumber(this FetchExpression query, XDocument xDoc)
        {
            XDocument xDocument = xDoc ?? XDocument.Parse(query.Query);

            /// No paging mean PageInfo.Page = 0
            /// for FetchXml it mean no "page" attribute or 0 value
            string page = xDocument.Root.Attribute("page")?.Value;
            return page != null ? int.Parse(page) : 0;
        }
    }
}
