namespace SkyMap.Net.SqlOM
{
    using System;

    public class InvalidQueryException : ApplicationException
    {
        public InvalidQueryException(string text) : base(text)
        {
        }
    }
}

