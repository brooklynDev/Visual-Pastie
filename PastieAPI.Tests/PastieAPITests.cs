using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace PastieAPI.Tests
{
    [TestFixture]
    public class PastieAPITests
    {
        [Test]
        public void WhenPastingCodeReturnsValidLink()
        {
            var result = Pastie.Paste("some sample code", Languages.CSharp);
            Assert.IsTrue(IsValidLink(result));
        }

        private static bool IsValidLink(string link)
        {
            const string pastieLink = "http://pastie.org/";
            if (!link.StartsWith(pastieLink))
            {
                return false;
            }

            link = link.Replace(pastieLink, String.Empty);
            if (String.IsNullOrEmpty(link))
            {
                return false;
            }

            return Regex.IsMatch(link, @"^[0-9]+$");
        }
    }
}
