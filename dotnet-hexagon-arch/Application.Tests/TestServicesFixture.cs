using AutoMapper;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Mapping;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Repository;
using Moq;
using System;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Services.Tests
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