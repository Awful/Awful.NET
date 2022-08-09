// <copyright file="BookmarkHandlerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Linq;
using AngleSharp.Html.Parser;
using Awful.Core.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Awful.Test
{
    /// <summary>
    /// Bookmark Handler Test.
    /// </summary>
    [TestClass]
    public class BookmarkHandlerTest
    {
        private HtmlParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkHandlerTest"/> class.
        /// </summary>
        public BookmarkHandlerTest()
        {
            this.parser = new HtmlParser();
        }

        /// <summary>
        /// Can parse bookmark page.
        /// </summary>
        /// <param name="htmlFile">Bookmark Html sample file.</param>
        [DataRow("bookmark_multi_page.html")]
        [DataRow("bookmark_single_page.html")]
        [DataTestMethod]
        public void CanParseBookmarksPage(string htmlFile)
        {
            var html = TestHelpers.GetSampleHtmlFile(htmlFile);
            var document = this.parser.ParseDocument(html);
            Assert.IsNotNull(document);

            var (currentPage, totalPages) = CommonHandlers.GetCurrentPageAndTotalPagesFromSelector(document);
            var threads = ThreadHandler.ParseForumThreadList(document);

            Assert.IsNotNull(threads);
            Assert.IsTrue(currentPage >= 1);
            Assert.IsTrue(totalPages >= 1);
            Assert.IsTrue(threads.Any());

            foreach (var thread in threads)
            {
                Assert.IsNotNull(thread.Name);
                Assert.IsTrue(thread.ThreadId > 0);
                Assert.IsNotNull(thread.ImageIconEndpoint);
                Assert.IsNotNull(thread.ImageIconLocation);
            }
        }
    }
}
