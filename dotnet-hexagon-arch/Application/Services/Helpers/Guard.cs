using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Exceptions;
using System;
using System.Linq;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services.Helpers
{
    public class Guard
    {
        public static bool IsAnyPropertyNull(Object myObject)
        {
            return myObject.GetType().GetProperties()
                .Select(pi => pi.GetValue(myObject))
                .Any(value => value is null);
        }

        public static bool IsPropertyExceedTheSizeLimit(string property, int limit)
        {
            return property.Length > limit;
        }

        public static void ValidateComment(CommentRequest commentRequest)
        {
            if (IsAnyPropertyNull(commentRequest))
                throw new UnprocessableEntityException(commentRequest);
            if (IsPropertyExceedTheSizeLimit(commentRequest.Author, 30))
                throw new ExceedMaxSizeCharactersException(new Tuple<string, int, string>("Author", 30, commentRequest.Author), null);
            if (IsPropertyExceedTheSizeLimit(commentRequest.Content, 120))
                throw new ExceedMaxSizeCharactersException(new Tuple<string, int, string>("Content", 120, commentRequest.Content), null);
        }

        public static void ValidatePost(PostRequest postRequest)
        {
            if (IsAnyPropertyNull(postRequest))
                throw new UnprocessableEntityException(postRequest);
            if (IsPropertyExceedTheSizeLimit(postRequest.Title, 30))
                throw new ExceedMaxSizeCharactersException(new Tuple<string, int, string>("Title", 30, postRequest.Title), null);
            if (IsPropertyExceedTheSizeLimit(postRequest.Content, 1200))
                throw new ExceedMaxSizeCharactersException(new Tuple<string, int, string>("Content", 120, postRequest.Content), null);
        }
    }
}
