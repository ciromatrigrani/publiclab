using System;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Exceptions
{
    public class ExceedMaxSizeCharactersException : Exception
    {
        public ExceedMaxSizeCharactersException()
        { }

        public ExceedMaxSizeCharactersException(Tuple<string, int, string> property, Exception inner = null) : base(
            $"The property {property.Item1} has a limit of {property.Item2} characters. You wrote {property.Item3.Length} characters at '{property.Item3}'.",
            inner)
        { }
    }
}

