using AutoMapper;
using MatrigraniCiro.HexagonArch.BestBlogs.Api.Controllers;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Exceptions;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api.Tests
{
    public class PostControllerTests : IClassFixture<TestControllersFixture>
    {
        private readonly TestControllersFixture fixture;
        private readonly PostController postController;
        private readonly Mock<IPostService> postServiceMock;
        private readonly Mock<ICommentService> commentServiceMock;
        private readonly IMapper mapper;

        public PostControllerTests(TestControllersFixture fixture)
        {
            this.fixture = fixture;
            this.postServiceMock = this.fixture.PostServiceMock;
            this.postController = this.fixture.PostController;
            this.commentServiceMock = this.fixture.CommentServiceMock;
            this.mapper = this.fixture.Mapper;
        }

        [Fact]
        public async void TestGetsSuccessReturnsList()
        {
            // arrange
            var postsRes = new List<PostResponse> { new PostResponse(), new PostResponse() };
            this.postServiceMock.Setup(r => r.GetAll(default)).ReturnsAsync(postsRes);

            // act
            var posts = await postController.GetAll(default);
            var okObjectResult = posts.Result as OkObjectResult;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(postsRes.Count, ((IEnumerable<PostResponse>)okObjectResult.Value).Count());
        }

        [Fact]
        public async void TestGetAllSuccessReturnsEmptyList()
        {
            // arrange
            var postsRes = new List<PostResponse> { };
            this.postServiceMock.Setup(r => r.GetAll(default)).ReturnsAsync(postsRes);

            // act
            var posts = await postController.GetAll(default);
            var okObjectResult = posts.Result as OkObjectResult;
            var postsList = (IEnumerable<PostResponse>)okObjectResult.Value;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Empty(postsList);
        }

        [Fact]
        public async void TestGetSuccessReturnsPostResponse()
        {
            // arrange
            var postId = Guid.NewGuid();
            var postRes = new PostResponse() { Id = postId };
            this.postServiceMock.Setup(r => r.Get(postId, default)).ReturnsAsync(postRes);

            // act
            var post = await postController.Get(postId, default);
            var okObjectResult = post.Result as OkObjectResult;
            var postsRes = (PostResponse)okObjectResult.Value;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(postId, postsRes.Id);
        }

        [Fact]
        public async void TestGetFailureThrowsNotFoundException()
        {
            // arrange
            this.postServiceMock.Setup(r => r.Get(Guid.Empty, default)).Throws<NotFoundException>();

            // act
            var error = await this.postController.Get(Guid.Empty, default);
            var objectResult = error.Result as NotFoundObjectResult;
            var errorRes = objectResult.Value;

            // assert 
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.IsType<ErrorResponse>(errorRes);
        }

        [Fact]
        public async void TestPostPostSuccessReturnCreatedResponse()
        {
            // arrange
            var postId = Guid.NewGuid();
            var postRes = new PostResponse()
            {
                Title = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,

                Id = postId
            };
            var postRequest = this.mapper.Map<PostRequest>(postRes);
            this.postServiceMock.Setup(r => r.Post(postId, It.IsAny<PostRequest>(), default)).ReturnsAsync(postRes);

            // act
            var post = await this.postController.Post(postRequest, default);
            var objectResult = post.Result as CreatedResult;
            var objectResultRes = objectResult.Value;

            // assert
            Assert.True(objectResult is CreatedResult);
            Assert.Equal(201, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostPostFailureThrowsBadRequestException()
        {
            // arrange
            var postResquest = new PostRequest();
            this.postServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), postResquest, default)).Throws(() => new BadRequestException());

            // act
            var error = await this.postController.Post(postResquest, default);
            var objectResult = error.Result as BadRequestObjectResult;

            // assert 
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostPostFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
        {
            // arrange
            var postResquest = new PostRequest
            {
                Title = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",

            };
            this.postServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), postResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.postController.Post(postResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostPostFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var postResquest = new PostRequest
            {
                Title = "Ciro Fernandes Matrigrani",
                Content = new String('*', 12001),

            };
            this.postServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), postResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.postController.Post(postResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostPostFailureThrowsUnprocessableEntityException()
        {
            // arrange
            var postResquest = new PostRequest();
            this.postServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), postResquest, default)).Throws(() => new UnprocessableEntityException());

            // act
            var error = await this.postController.Post(postResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteSuccessReturnsTrue()
        {

            // arrange
            var postId = Guid.NewGuid();
            this.postServiceMock.Setup(r => r.Delete(postId, default)).ReturnsAsync(true);

            // act
            var response = await postController.Delete(postId, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteFailureThrowNotFoundException()
        {
            // arrange
            this.postServiceMock.Setup(r => r.Delete(Guid.Empty, default)).Throws<NotFoundException>();

            // act
            var error = await postController.Delete(Guid.Empty, default);
            var objectResult = error as NotFoundObjectResult;

            // assert
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutSuccessReturnsPostResponse()
        {
            // arrange
            var postId = Guid.NewGuid();
            var postRes = new PostResponse()
            {
                Title = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,

                Id = postId
            };
            var postRequest = this.mapper.Map<PostRequest>(postRes);
            this.postServiceMock.Setup(r => r.Put(postId, It.IsAny<PostRequest>(), default)).ReturnsAsync(postRes);

            // act
            var response = await this.postController.Put(postId, postRequest, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutSuccessReturnsThrowsNotFoundExceptionAndCreated()
        {
            // arrange
            var postId = Guid.NewGuid();
            var postRes = new PostResponse()
            {
                Title = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,

                Id = postId
            };
            var postRequest = this.mapper.Map<PostRequest>(postRes);
            this.postServiceMock.Setup(r => r.Put(postId, It.IsAny<PostRequest>(), default)).Throws(() => new NotFoundException());

            // act
            var post = await this.postController.Put(postId, postRequest, default);
            var objectResult = post as CreatedResult;
            var objectResultRes = objectResult.Value;

            // assert
            Assert.True(objectResult is CreatedResult);
            Assert.Equal(201, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutPostFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
        {
            // arrange
            var postResquest = new PostRequest
            {
                Title = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",

            };
            this.postServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), postResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.postController.Put(Guid.NewGuid(), postResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutPostFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var postResquest = new PostRequest
            {
                Title = "Ciro Fernandes Matrigrani",
                Content = new String('*', 12001),

            };
            this.postServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), postResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.postController.Put(Guid.NewGuid(), postResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutPostFailureThrowsGenericExceptionReturnsConflict()
        {
            // arrange
            var postResquest = new PostRequest();
            this.postServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), postResquest, default)).Throws(() => new Exception());

            // act
            var error = await this.postController.Put(Guid.NewGuid(), postResquest, default);
            var objectResult = error as ConflictObjectResult;

            // assert 
            Assert.True(objectResult is ConflictObjectResult);
            Assert.Equal(409, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutPostFailureThrowsUnprocessableEntityException()
        {
            // arrange
            var postResquest = new PostRequest();
            this.postServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), postResquest, default)).Throws(() => new UnprocessableEntityException());

            // act
            var error = await this.postController.Put(Guid.NewGuid(), postResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchSuccessReturnsPostResponse()
        {
            // arrange
            var postId = Guid.NewGuid();
            var expectedPostTitle = "Ciro";
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "replace", path: "/Title", null, value: expectedPostTitle));
            var postRes = new PostResponse()
            {
                Title = "Ciro Matrigrani",
                Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
                CreationDate = DateTime.Now,

                Id = postId
            };
            this.postServiceMock.Setup(r => r.Get(postId, default)).ReturnsAsync(postRes);
            this.postServiceMock.Setup(r => r.Put(postRes.Id, It.IsAny<PostRequest>(), default)).ReturnsAsync(postRes);

            // act
            var response = await postController.Patch(postId, patchEntity, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsBadRequestException()
        {
            // arrange
            var postId = Guid.NewGuid();
            var expectedPostTitle = "Ciro";
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "replace", path: "/Title", null, value: expectedPostTitle));
            this.postServiceMock.Setup(r => r.Patch(postId, patchEntity, default)).Throws(() => new BadRequestException());

            // act
            var error = await postController.Patch(postId, patchEntity, default);
            var objectResult = error as BadRequestObjectResult;

            // assert
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsExceedMaxSizeCharactersExceptionByTitle()
        {
            // arrange
            var postId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "add", path: "/Title", null, value: "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani"));
            this.postServiceMock.Setup(r => r.Patch(postId, patchEntity, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await postController.Patch(postId, patchEntity, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var postId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "add", path: "/Content", null, value: new String('*', 121)));
            this.postServiceMock.Setup(r => r.Patch(postId, patchEntity, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await postController.Patch(postId, patchEntity, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsNotFoundException()
        {
            // arrange
            var postId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<PostRequest>();
            patchEntity.Operations.Add(new Operation<PostRequest>(op: "add", path: "/Content", null, value: new String('*', 121)));
            this.postServiceMock.Setup(r => r.Patch(postId, patchEntity, default)).Throws(() => new NotFoundException());

            // act
            var error = await postController.Patch(postId, patchEntity, default);
            var objectResult = error as NotFoundObjectResult;

            // assert
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void TestGetCommentsByPostIdSuccessReturnsCommentList()
        {
            // arrange
            var postId = Guid.NewGuid();
            var commentList = new List<CommentResponse> { new CommentResponse() { Id = postId } };
            this.commentServiceMock.Setup(r => r.GetCommentsByPostId(postId, default)).ReturnsAsync(commentList);

            // act
            var comments = await postController.GetCommentsByPostId(postId, default);
            var okObjectResult = comments.Result as OkObjectResult;
            var commentsRes = (IEnumerable<CommentResponse>)okObjectResult.Value;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(commentList.Count, commentsRes.Count());
        }

        [Fact]
        public async void TestGetCommentsByPostIdFailureThrowsNotFoundException()
        {
            // arrange
            var postId = Guid.Empty;
            this.commentServiceMock.Setup(r => r.GetCommentsByPostId(postId, default)).Throws(() => new NotFoundException());

            // act
            var error = await this.postController.GetCommentsByPostId(postId, default);
            var objectResult = error.Result as NotFoundObjectResult;
            var errorRes = objectResult.Value;

            // assert 
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.IsType<ErrorResponse>(errorRes);
        }
    }
}
