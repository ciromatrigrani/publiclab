using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Mapping;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;
using Moq;
using System;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Tests
{
    public class TestServicesFixture : IDisposable
    {
        public Mock<IPostRepository> PostRepositoryMock { get; }
        public IPostService PostService { get; }
        public Mock<ICommentRepository> CommentRepositoryMock { get; }
        public ICommentService CommentService { get; }
        public IMapper Mapper { get; }

        public TestServicesFixture()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Mapping()));
            this.Mapper = mapperConfig.CreateMapper();
            this.CommentRepositoryMock = new Mock<ICommentRepository>();
            this.CommentService = new CommentService(this.CommentRepositoryMock.Object, this.Mapper);
            this.PostRepositoryMock = new Mock<IPostRepository>();
            this.PostService = new PostService(this.PostRepositoryMock.Object, this.Mapper);
        }

        public void Dispose()
        { }
    }
}