using System;

namespace TTG_Tools.Core.Exceptions
{
    public class ExportException : Exception
    {
        public ExportException(string message, Exception inner = null) : base(message, inner) { }
    }
}