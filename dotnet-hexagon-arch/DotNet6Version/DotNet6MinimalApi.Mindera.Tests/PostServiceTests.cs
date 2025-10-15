using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Exceptions;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Tests;

public class PostServiceTests : IClassFixture<TestServicesFixture>
{
    private readonly TestServicesFixture fixture;
    private readonly IPostService postService;
    private readonly Mock<IPostRepository> postRepositoryMock;
    private readonly IMapper mapper;


    public PostServiceTests(TestServicesFixture fixture)
    {
        this.fixture = fixture;
        this.postRepositoryMock = this.fixture.PostRepositoryMock;
        this.postService = this.fixture.PostService;
        this.mapper = this.fixture.Mapper;
    }

    [Fact]
    public async void TestGetPostsSuccessReturnsList()
    {
        // arrange
        var postsRes = new List<Post> { new Post() };
        this.postRepositoryMock.Setup(r => r.GetAll(default)).ReturnsAsync(postsRes);

        // act
        var posts = await postService.GetAll();

        // assert
        Assert.Equal(postsRes.Count, posts.Count());
    }

    [Fact]
    public async void TestGetPostsSuccessReturnsEmptyList()
    {
        // arrange
        var postsRes = new List<Post> { };
        this.postRepositoryMock.Setup(r => r.GetAll(default)).ReturnsAsync(postsRes);

        // act
        var posts = await postService.GetAll(default);

        // assert
        Assert.Empty(posts);
    }

    [Fact]
    public async void TestGetPostSuccessReturnsPostReponse()
    {
        // arrange
        var postId = Guid.NewGuid();
        var postRes = new Post() { Id = postId };
        this.postRepositoryMock.Setup(r => r.Get(postId, default)).ReturnsAsync(postRes);

        // act
        var post = await postService.Get(postId, default);

        // assert
        Assert.Equal(postId, post.Id);
    }

    [Fact]
    public async void TestGetPostFailureThrowsException()
    {
        // arrange
        this.postRepositoryMock.Setup(r => r.Get(Guid.Empty, default)).Throws<Exception>();

        // act, assert
        await Assert.ThrowsAsync<Exception>(() => this.postService.Get(Guid.Empty, default));
    }

    [Fact]
    public async void TestPostPostSuccessReturnPostReponse()
    {

        // arrange
        var postId = Guid.NewGuid();
        var postRes = new Post()
        {
            Title = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api.",
            CreationDate = DateTime.Now,
            Id = postId
        };
        var postRequest = this.mapper.Map<PostRequest>(postRes);
        this.postRepositoryMock.Setup(r => r.Create(It.IsAny<Post>(), default)).ReturnsAsync(postRes);

        // act
        var post = await this.postService.Post(postId, postRequest, default);

        // assert
        Assert.Equal(postId, post.Id);
    }

    [Fact]
    public async void TestPostPostFailureThrowsUnprocessableEntityException()
    {
        // act, assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.postService.Post(Guid.Empty, new PostRequest(), default));
    }

    [Fact]
    public async void TestPostPostFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
    {
        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Post(Guid.Empty,
            new PostRequest()
            {
                Title = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api."
            },
            default));
    }

    [Fact]
    public async void TestPostPostFailureThrowsExceedMaxSizeCharactersExceptionByContent()
    {
        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Post(Guid.Empty,
            new PostRequest()
            {
                Title = "Ciro Fernandes Matrigrani",
                Content = new string('*', 1201),
            },
            default));
    }

    [Fact]
    public async void TestDeletePostSuccessReturnsTrue()
    {

        // arrange
        var postId = Guid.NewGuid();
        this.postRepositoryMock.Setup(r => r.Delete(postId, default)).ReturnsAsync(true);

        // act
        var response = await postService.Delete(postId, default);

        // assert
        Assert.True(response);
    }

    [Fact]
    public async void TestDeletePostFailureReturnsFalse()
    {
        // arrange
        this.postRepositoryMock.Setup(r => r.Delete(Guid.Empty, default)).ReturnsAsync(false);

        // act
        var response = await postService.Delete(Guid.Empty, default);

        // assert
        Assert.False(response);
    }

    [Fact]
    public async void TestDeletePostFailureReturnException()
    {
        // arrange
        this.postRepositoryMock.Setup(r => r.Delete(Guid.Empty, default)).ThrowsAsync(new Exception());

        // act, assert
        await Assert.ThrowsAsync<Exception>(() => this.postService.Delete(Guid.Empty, default));
    }

    [Fact]
    public async void TestPutPostSuccessReturnsPostReponse()
    {
        // arrange
        var postId = Guid.NewGuid();
        var postRes = new Post()
        {
            Title = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = DateTime.Now,
            Id = postId
        };
        var postRequest = this.mapper.Map<PostRequest>(postRes);
        this.postRepositoryMock.Setup(r => r.Update(It.IsAny<Post>(), default)).ReturnsAsync(postRes);

        // act
        var postResponse = await postService.Put(postId, postRequest, default);

        // assert
        Assert.Equal(postId, postResponse.Id);
    }

    [Fact]
    public async void TestPutPostFailureThrowsUnprocessableEntityException()
    {
        // act, assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.postService.Put(Guid.Empty, new PostRequest(), default));
    }


    [Fact]
    public async void TestPutPostFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
    {
        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Put(Guid.Empty,
            new PostRequest()
            {
                Title = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api."
            },
            default));
    }

    [Fact]
    public async void TestPutPostFailureThrowsExceedMaxSizeCharactersExceptionByContent()
    {
        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Put(Guid.Empty,
            new PostRequest()
            {
                Title = "Ciro Fernandes Matrigrani",
                Content = new string('*', 1201),
            },
            default));
    }

    [Fact]
    public async void TestPatchPostSuccessReturnsPostReponse()
    {
        // arrange
        var postId = Guid.NewGuid();
        var expectedPostTitle = "Ciro";
        var patchEntity = new JsonPatchDocument<PostRequest>();
        patchEntity.Operations.Add(new Operation<PostRequest>(op: "replace", path: "/Title", null, value: expectedPostTitle));
        var postRes = new Post()
        {

            Title = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = DateTime.Now,
            Id = postId
        };
        this.postRepositoryMock.Setup(r => r.Get(postId, default)).ReturnsAsync(postRes);
        this.postRepositoryMock.Setup(r => r.Update(It.IsAny<Post>(), default)).ReturnsAsync(postRes);

        // act
        var postResponse = await postService.Patch(postId, patchEntity, default);

        // assert
        Assert.Equal(postId, postResponse.Id);
    }

    [Fact]
    public async void TestPatchPostFailureThrowsUnprocessableEntityException()
    {
        // arrange
        this.postRepositoryMock.Setup(r => r.Get(Guid.Empty, default)).ReturnsAsync(new Post());
        var patchEntity = new JsonPatchDocument<PostRequest>();
        patchEntity.Operations.Add(new Operation<PostRequest>(op: "x", path: "/y", null, value: "z"));

        // act, assert
        await Assert.ThrowsAsync<BadRequestException>(() => this.postService.Patch(Guid.Empty, patchEntity, default));
    }

    [Fact]
    public async void TestPatchPostFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
    {
        // arrange
        var postId = Guid.NewGuid();
        this.postRepositoryMock.Setup(r => r.Get(postId, default)).ReturnsAsync(new Post
        {
            Title = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = DateTime.Now,
            Id = postId
        });
        var patchEntity = new JsonPatchDocument<PostRequest>();
        patchEntity.Operations.Add(new Operation<PostRequest>(op: "add", path: "/Title", null, value: "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani"));

        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Patch(postId,
            patchEntity,
            default));
    }

    [Fact]
    public async void TestPatchPostFailureThrowsExceedMaxSizeCharactersExceptionByContent()
    {
        // arrange
        var postId = Guid.NewGuid();
        this.postRepositoryMock.Setup(r => r.Get(postId, default)).ReturnsAsync(new Post
        {
            Title = "Ciro Matrigrani",
            Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
            CreationDate = DateTime.Now,
            Id = postId
        });
        var patchEntity = new JsonPatchDocument<PostRequest>();
        patchEntity.Operations.Add(new Operation<PostRequest>(op: "add", path: "/Content", null, value: new string('*', 1201)));

        // act, assert
        await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.postService.Patch(postId,
            patchEntity,
            default));
    }

}
