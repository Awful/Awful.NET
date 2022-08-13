// <copyright file="ForumHandlerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Linq;
using AngleSharp.Html.Parser;
using Awful.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Awful.Test
{
    /// <summary>
    /// Forum Handler Test.
    /// </summary>
    [TestClass]
    public class ForumHandlerTest
    {
        private HtmlParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumHandlerTest"/> class.
        /// </summary>
        public ForumHandlerTest()
        {
            this.parser = new HtmlParser();
        }

        /// <summary>
        /// Can parse bookmark page.
        /// </summary>
        /// <param name="htmlFile">Bookmark Html sample file.</param>
        [DataRow("gbs_thread_list.html")]
        [DataTestMethod]
        public void CanParseForumPage(string htmlFile)
        {
            var html = TestHelpers.GetSampleHtmlFile(htmlFile);
            var document = this.parser.ParseDocument(html);
            Assert.IsNotNull(document);

            var (currentPage, totalPages) = CommonHandlers.GetCurrentPageAndTotalPagesFromSelector(document);
            var forumInfo = ForumHandler.GetForumInfoViaThreadListPage(document);
            var threads = ThreadHandler.ParseForumThreadList(document);
            Assert.IsTrue(totalPages > 0);
            Assert.IsNotNull(forumInfo.ForumName);
            Assert.IsTrue(threads.Count > 0);
        }
    }
}
