using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogTest
{
    internal class LogWriter
    {
        private string LogDirectory = ".\\Logs";
        private readonly string LogFileHeader = "Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" +
                                       Environment.NewLine;

        private DateTime? lastWriteAt;
        private string lastWrittenToAFilePath;

        public void WriteSingleLine(string msg)
        {
            try
            {
                var now = DateTimeProvider.Current.Now;
                var filePath = DiscoverFilePath(now);

                EnsureLogFileWithHeaderExists(filePath);
                File.AppendAllText(filePath, new LogLine(msg).Text);

                lastWriteAt = now;
                lastWrittenToAFilePath = filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                // deliberately muting all errors that could occur during logging
            }
        }

        private void EnsureLogFileWithHeaderExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                return;
            }

            File.AppendAllText(filePath, LogFileHeader);
        }

        /// <summary>
        /// Returns file name of the existing log file (if one exists for today) 
        /// or creates a new one 
        /// </summary>
        /// <returns>File path</returns>
        private string DiscoverFilePath(DateTime now)
        {
            if (lastWriteAt.HasValue &&
                lastWriteAt.Value.Date == now.Date)
            {
                return lastWrittenToAFilePath;
            }

            //todo: this doesn't belong here
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            var fileNameRegexPattern = $"Log{now:yyyyMMdd} " + @"\d{6} \d{3}";
            var files = Directory.GetFiles(LogDirectory);
            var todaysFilePathWithName = files.FirstOrDefault(x => Regex.IsMatch(x, fileNameRegexPattern));

            if (todaysFilePathWithName != null)
            {
                return todaysFilePathWithName;
            }

            var newFileName = "Log" + now.ToString("yyyyMMdd HHmmss fff") + ".log";
            return Path.Combine(LogDirectory, newFileName);
        }
    }
}
