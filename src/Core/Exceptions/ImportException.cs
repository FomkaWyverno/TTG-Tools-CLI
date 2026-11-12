using System;

namespace TTG_Tools.Core.Exceptions
{
    public class ImportException : Exception
    {
        public ImportException(string message, Exception inner = null) : base(message, inner) { }
    }
}