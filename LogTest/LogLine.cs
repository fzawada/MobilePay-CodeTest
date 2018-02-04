namespace LogTest
{
    using System;

    public class LogLine
    {
        private readonly string message;
        private readonly DateTime timestamp;
        private string logLineText;

        public LogLine(string message)
        {
            this.message = message;
            this.timestamp = DateTimeProvider.Current.Now;
        }

        public string Text
        {
            get
            {
                if (logLineText == null)
                {
                    logLineText = $"{timestamp:yyyy-MM-dd HH:mm:ss:fff}\t{message}.{Environment.NewLine}";
                }
            
                return logLineText;
            }
        }
    }
}