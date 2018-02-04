using System;
using System.Reflection;
using LogTest;

namespace LogComponentTests
{
    internal static class DateTimeContextHelper
    {
        internal static void SetDateTime(DateTime value)
        {
            var dateTimeStub = new DateTimeStub(value);
            typeof(DateTimeProvider).GetProperty("Current", BindingFlags.NonPublic |BindingFlags.Public| BindingFlags.Static).SetValue(null, dateTimeStub);
        }
    }
}
