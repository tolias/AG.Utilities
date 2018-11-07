using System;
using AG.Utilities.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AG.Utilities.Tests
{
    [TestClass]
    public class PeriodTests
    {
        [TestMethod]
        public void ZeroForTheSameDates()
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = date1;

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(0, 0, 0), actual);
        }

        [TestMethod]
        public void TestForMonths()
        {
            DateTime date1 = new DateTime(2000, 1, 3);
            DateTime date2 = new DateTime(2004, 6, 25);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(4, 5, 22), actual);
        }

        [TestMethod]
        public void TestForMonths2()
        {
            DateTime date1 = new DateTime(2000, 1, 3);
            DateTime date2 = new DateTime(2000, 6, 25);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(0, 5, 22), actual);
        }

        [TestMethod]
        public void TestForMonths3()
        {
            DateTime date1 = new DateTime(2000, 1, 3);
            DateTime date2 = new DateTime(2000, 1, 25);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(0, 0, 22), actual);
        }

        [TestMethod]
        public void TestForMonths4()
        {
            DateTime date1 = new DateTime(2000, 1, 3);
            DateTime date2 = new DateTime(2006, 7, 3);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(6, 6, 0), actual);
        }

        [TestMethod]
        public void TestForMonths5()
        {
            DateTime date1 = new DateTime(2000, 1, 3);
            DateTime date2 = new DateTime(2006, 7, 2);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(6, 5, 29), actual);
        }

        [TestMethod]
        public void TestForMonths6()
        {
            DateTime date1 = new DateTime(2000, 2, 3);
            DateTime date2 = new DateTime(2006, 1, 4);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(5, 11, 1), actual);
        }

        [TestMethod]
        public void TestForMonths7()
        {
            DateTime date1 = new DateTime(2000, 10, 30);
            DateTime date2 = new DateTime(2006, 8, 4);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(5, 9, 5), actual);
        }

        [TestMethod]
        public void TestForMonths8()
        {
            DateTime date1 = new DateTime(2000, 2, 28);
            DateTime date2 = new DateTime(2006, 3, 1);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(6, 0, 1), actual);
        }

        [TestMethod]
        public void TestForMonths9()
        {
            DateTime date1 = new DateTime(2015, 12, 31);
            DateTime date2 = new DateTime(2016, 1, 1);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(0, 0, 1), actual);
        }

        [TestMethod]
        public void Test10()
        {
            DateTime date1 = new DateTime(2016, 12, 31);
            DateTime date2 = new DateTime(2016, 1, 1);

            var actual = Period.GetPeriodBetween(date1, date2);

            Assert.AreEqual(new Period(0, -11, -30), actual);
        }
    }
}
