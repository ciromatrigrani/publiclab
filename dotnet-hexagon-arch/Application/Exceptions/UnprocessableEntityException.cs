using Newtonsoft.Json;
using System;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException()
        { }

        public UnprocessableEntityException(Object entity, Exception inner = null) : base(
            $"The system cannot process the entity {JsonConvert.SerializeObject(entity)}. Please, check the properties and try again.", inner)
        { }
    }
}

