using System;
using LogTest;
using NUnit.Framework;

namespace LogComponentTests
{
    [TestFixture]
    class LogLineTests
    {
        [Test]
        public void FormatsCorrectly()
        {
            var msg = "SomeMsg";
            DateTimeContextHelper.SetDateTime(new DateTime(2019, 1, 2, 15, 57, 58));

            var logLine = new LogLine(msg);

            Assert.That(logLine.Text, Is.EqualTo("2019-01-02 15:57:58:000\tSomeMsg."));
        }
    }
}
