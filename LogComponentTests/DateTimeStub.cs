using System;
using LogTest;

namespace LogComponentTests
{
    internal class DateTimeStub : IDateTime
    {
        internal DateTimeStub(DateTime dateTime)
        {
            Now = dateTime;
        }

        public DateTime Now { get; }
    }
}
