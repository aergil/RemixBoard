using System;

namespace RemixBoard.Core
{
    public class JobException : Exception
    {
        public JobException(string message) : base(message) { }
    }

    public class WebRequestException : Exception { }
}