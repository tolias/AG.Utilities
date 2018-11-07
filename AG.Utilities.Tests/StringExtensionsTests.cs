using System.Web;
using AG.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AG.Utilities.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ToLowerFirstLetterIfItsUpperCase_Tests()
        {
            var actual = "word1 WORD2".ToLowerFirstLetterIfItsUpperCase();
            Assert.AreEqual("word1 WORD2", actual);

            actual = "Word1 WORD2".ToLowerFirstLetterIfItsUpperCase();
            Assert.AreEqual("word1 WORD2", actual);

            actual = "W".ToLowerFirstLetterIfItsUpperCase();
            Assert.AreEqual("w", actual);

            actual = "w".ToLowerFirstLetterIfItsUpperCase();
            Assert.AreEqual("w", actual);

            actual = "WORD1 word2".ToLowerFirstLetterIfItsUpperCase();
            Assert.AreEqual("WORD1 word2", actual);
        }

        [TestMethod]
        public void Replace_Tests()
        {
            var input = @"D:\_|" + HttpUtility.UrlEncode(@"Книжки\Їжа і здоров'я") + @"|_\English books\_|" + HttpUtility.UrlEncode(@"ха-ха-ха") + @"|_\How to be healthy.pdf";

            var actual = input.Replace(@"_\|([^\|]+)\|_", match =>
            {
                var occurence = match.Groups[1].Value;
                var decodedOccurence = HttpUtility.UrlDecode(occurence);
                return decodedOccurence;
            });

            var expected = @"D:\Книжки\Їжа і здоров'я\English books\ха-ха-ха\How to be healthy.pdf";
            Assert.AreEqual(expected, actual);
        }
    }
}
