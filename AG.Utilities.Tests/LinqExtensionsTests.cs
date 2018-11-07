using System;
using System.Linq;
using AG.Utilities.Linq;
using AG.Utilities.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AG.Utilities.Tests
{
    [TestClass]
    public class LinqExtensionsTests
    {
        [TestMethod]
        public void RemoveCommonItems_When_no_common()
        {
            int[] s1 = { 1, 2 };
            int[] s2 = { 4, 6, 8 };
            int[] s3 = { 10, 21 };

            var res = LinqExtensionMethods.RemoveCommonItems(s1, s2, s3);

            Assert.AreEqual(3, res.Length);

            Assert.AreNotEqual(s1, res[0]);
            Assert.AreNotEqual(s2, res[1]);
            Assert.AreNotEqual(s3, res[2]);

            Assert.IsTrue(s1.SequenceEqual(res[0]));
            Assert.IsTrue(s2.SequenceEqual(res[1]));
            Assert.IsTrue(s3.SequenceEqual(res[2]));
        }

        [TestMethod]
        public void RemoveCommonItems_When_common()
        {
            int[] s1 = { 1, 2, 3 };
            int[] s2 = { 4, 2, 6, 8, 1 };
            int[] s3 = { 10, 21, 2, 1 };

            var res = LinqExtensionMethods.RemoveCommonItems(s1, s2, s3);

            Assert.AreEqual(3, res.Length);

            Assert.AreNotEqual(s1, res[0]);
            Assert.AreNotEqual(s2, res[1]);
            Assert.AreNotEqual(s3, res[2]);

            Assert.IsTrue(res[0].SequenceEqual(new[] { 3 }));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 4, 6, 8 }));
            Assert.IsTrue(res[2].SequenceEqual(new[] { 10, 21 }));
        }

        [TestMethod]
        public void RemoveCommonItems_When_common_and_sequence_becomes_empty()
        {
            int[] s1 = { 1, 2 };
            int[] s2 = { 4, 2, 6, 8, 1 };
            int[] s3 = { 10, 21, 2, 1 };

            var res = LinqExtensionMethods.RemoveCommonItems(s1, s2, s3);

            Assert.AreEqual(3, res.Length);

            Assert.AreNotEqual(s1, res[0]);
            Assert.AreNotEqual(s2, res[1]);
            Assert.AreNotEqual(s3, res[2]);

            Assert.IsTrue(res[0].SequenceEqual(new int[0]));
            Assert.IsTrue(res[1].SequenceEqual(new[] { 4, 6, 8 }));
            Assert.IsTrue(res[2].SequenceEqual(new[] { 10, 21 }));
        }
    }
}
