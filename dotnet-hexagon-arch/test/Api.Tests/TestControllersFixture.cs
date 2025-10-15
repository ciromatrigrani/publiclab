using AutoMapper;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Api.Controllers;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Mapping;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services;
using Moq;
using System;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.Tests
{
    public class TestControllersFixture : IDisposable
    {
        public Mock<IPostService> PostServiceMock { get; }
        public PostController PostController { get; }
        public Mock<ICommentService> CommentServiceMock { get; }
        public CommentController CommentController { get; }
        public IMapper Mapper { get; }

        public TestControllersFixture()
        {
            this.CommentServiceMock = new Mock<ICommentService>();
            this.CommentController = new CommentController(this.CommentServiceMock.Object, default);
            this.PostServiceMock = new Mock<IPostService>();
            this.PostController = new PostController(this.PostServiceMock.Object, this.CommentServiceMock.Object, default);
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Mapping()));
            this.Mapper = mapperConfig.CreateMapper();
        }

        public void Dispose()
        { }
    }
}