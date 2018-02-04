using System;
using System.IO;
using System.Linq;
using System.Threading;
using LogTest;
using NUnit.Framework;

namespace LogComponentTests
{
    [TestFixture]
    class AsyncLogRequiredTests
    {
        private string logsDir = ".\\Logs";

        [Test]
        public void CallToLogEndsUpWriting()
        {
            var msgToLog = "something";
            var log = new AsyncLog();

            log.Write(msgToLog);
            log.StopWithFlush();

            var logFile = Directory.GetFiles(logsDir).Single();
            var logFileContent = File.ReadAllText(logFile);
            Assert.That(logFileContent, Contains.Substring(msgToLog));
        }

        [Test]
        public void NewFileIsBeingCreatedAfterMidnight()
        {
            var log = new AsyncLog();

            DateTimeContextHelper.SetDateTime(new DateTime(2018, 02, 01, 23, 58, 00));
            log.Write("a");
            Thread.Sleep(10);
            DateTimeContextHelper.SetDateTime(new DateTime(2018, 02, 02, 1, 58, 00));
            log.Write("b");
            log.StopWithFlush();

            Assert.That(Directory.GetFiles(logsDir).Length, Is.EqualTo(2));
        }

        [Test]
        public void CanStopWithoutFlushing()
        {
            var log = new AsyncLog();
            for (int i = 0; i < 15; i++)
            {
                log.Write("Number with Flush: " + i);
            }

            log.StopWithoutFlush();

            if (!Directory.Exists(logsDir) ||
                Directory.GetFiles(logsDir).Length == 0)
            {
                Assert.Pass("Didn't even create a file due to no-wait stopping");    
            }

            var logFile = Directory.GetFiles(logsDir).Single();
            var logFileContent = File.ReadAllText(logFile);
            Assert.That(logFileContent, !Contains.Substring("Flush: 10"));
        }

        [Test]
        public void CanStopWithFlushing()
        {
            var log = new AsyncLog();
            for (int i = 0; i < 15; i++)
            {
                log.Write("Number with Flush: " + i);
            }

            log.StopWithFlush();

            var logFile = Directory.GetFiles(logsDir).Single();
            var logFileContent = File.ReadAllText(logFile);
            Assert.That(logFileContent, !Contains.Substring("Flush: 15"));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(logsDir))
            {
                Directory.Delete(logsDir, true);
            }
        }
    }
}
