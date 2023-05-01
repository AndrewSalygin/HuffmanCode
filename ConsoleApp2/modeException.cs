using System;

namespace HuffmanCode
{
    public class modeException : Exception
    {
        public modeException(string message)
            : base(message) { }
    }
}