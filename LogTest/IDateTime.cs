using System;

namespace LogTest
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }

    public class StandardDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }

    /// <summary>
    /// Provides ambient context for DateTime
    /// </summary>
    public static class DateTimeProvider
    {
        public static IDateTime Current { get; private set; } = new StandardDateTime();
    }
}
