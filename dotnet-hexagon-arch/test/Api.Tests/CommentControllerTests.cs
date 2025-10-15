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
    public class CommentControllerTests : IClassFixture<TestControllersFixture>
    {
        private readonly TestControllersFixture fixture;
        private readonly CommentController commentController;
        private readonly Mock<ICommentService> commentServiceMock;
        private readonly IMapper mapper;

        public CommentControllerTests(TestControllersFixture fixture)
        {
            this.fixture = fixture;
            this.commentServiceMock = this.fixture.CommentServiceMock;
            this.commentController = this.fixture.CommentController;
            this.mapper = this.fixture.Mapper;
        }

        [Fact]
        public async void TestGetsSuccessReturnsList()
        {
            // arrange
            var commentsRes = new List<CommentResponse> { new CommentResponse(), new CommentResponse() };
            this.commentServiceMock.Setup(r => r.GetAll(default)).ReturnsAsync(commentsRes);

            // act
            var comments = await commentController.GetAll(default);
            var okObjectResult = comments.Result as OkObjectResult;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(commentsRes.Count, ((IEnumerable<CommentResponse>)okObjectResult.Value).Count());
        }

        [Fact]
        public async void TestGetAllSuccessReturnsEmptyList()
        {
            // arrange
            var commentsRes = new List<CommentResponse> { };
            this.commentServiceMock.Setup(r => r.GetAll(default)).ReturnsAsync(commentsRes);

            // act
            var comments = await commentController.GetAll(default);
            var okObjectResult = comments.Result as OkObjectResult;
            var commentsList = (IEnumerable<CommentResponse>)okObjectResult.Value;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Empty(commentsList);
        }

        [Fact]
        public async void TestGetSuccessReturnsCommentResponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new CommentResponse() { Id = commentId };
            this.commentServiceMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(commentRes);

            // act
            var comment = await commentController.Get(commentId, default);
            var okObjectResult = comment.Result as OkObjectResult;
            var commentsRes = (CommentResponse)okObjectResult.Value;

            // assert
            Assert.True(okObjectResult is OkObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(commentId, commentsRes.Id);
        }

        [Fact]
        public async void TestGetFailureThrowsNotFoundException()
        {
            // arrange
            this.commentServiceMock.Setup(r => r.Get(Guid.Empty, default)).Throws<NotFoundException>();

            // act
            var error = await this.commentController.Get(Guid.Empty, default);
            var objectResult = error.Result as NotFoundObjectResult;
            var errorRes = objectResult.Value;

            // assert 
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.IsType<ErrorResponse>(errorRes);
        }

        [Fact]
        public async void TestPostCommentSuccessReturnCreatedResponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new CommentResponse()
            {
                Author = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            var commentRequest = this.mapper.Map<CommentRequest>(commentRes);
            this.commentServiceMock.Setup(r => r.Post(commentId, It.IsAny<CommentRequest>(), default)).ReturnsAsync(commentRes);

            // act
            var comment = await this.commentController.Post(commentRequest, default);
            var objectResult = comment.Result as CreatedResult;
            var objectResultRes = objectResult.Value;

            // assert
            Assert.True(objectResult is CreatedResult);
            Assert.Equal(201, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostCommentFailureThrowsBadRequestException()
        {
            // arrange
            var commentResquest = new CommentRequest();
            this.commentServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new BadRequestException());

            // act
            var error = await this.commentController.Post(commentResquest, default);
            var objectResult = error.Result as BadRequestObjectResult;

            // assert 
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostCommentFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // arrange
            var commentResquest = new CommentRequest
            {
                Author = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                PostId = Guid.NewGuid()
            };
            this.commentServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.commentController.Post(commentResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostCommentFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var commentResquest = new CommentRequest
            {
                Author = "Ciro Fernandes Matrigrani",
                Content = new String('*', 12001),
                PostId = Guid.NewGuid()
            };
            this.commentServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.commentController.Post(commentResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPostCommentFailureThrowsUnprocessableEntityException()
        {
            // arrange
            var commentResquest = new CommentRequest();
            this.commentServiceMock.Setup(r => r.Post(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new UnprocessableEntityException());

            // act
            var error = await this.commentController.Post(commentResquest, default);
            var objectResult = error.Result as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteSuccessReturnsTrue()
        {

            // arrange
            var commentId = Guid.NewGuid();
            this.commentServiceMock.Setup(r => r.Delete(commentId, default)).ReturnsAsync(true);

            // act
            var response = await commentController.Delete(commentId, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteFailureThrowNotFoundException()
        {
            // arrange
            this.commentServiceMock.Setup(r => r.Delete(Guid.Empty, default)).Throws<NotFoundException>();

            // act
            var error = await commentController.Delete(Guid.Empty, default);
            var objectResult = error as NotFoundObjectResult;

            // assert
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutSuccessReturnsCommentResponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new CommentResponse()
            {
                Author = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            var commentRequest = this.mapper.Map<CommentRequest>(commentRes);
            this.commentServiceMock.Setup(r => r.Put(commentId, It.IsAny<CommentRequest>(), default)).ReturnsAsync(commentRes);

            // act
            var response = await this.commentController.Put(commentId, commentRequest, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutSuccessReturnsThrowsNotFoundExceptionAndCreated()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new CommentResponse()
            {
                Author = "Ciro Matrigrani",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            var commentRequest = this.mapper.Map<CommentRequest>(commentRes);
            this.commentServiceMock.Setup(r => r.Put(commentId, It.IsAny<CommentRequest>(), default)).Throws(() => new NotFoundException());

            // act
            var comment = await this.commentController.Put(commentId, commentRequest, default);
            var objectResult = comment as CreatedResult;
            var objectResultRes = objectResult.Value;

            // assert
            Assert.True(objectResult is CreatedResult);
            Assert.Equal(201, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutCommentFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // arrange
            var commentResquest = new CommentRequest
            {
                Author = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                Content = "Base Project Hexagon Arch 2025 - DotNet Rest Api.",
                PostId = Guid.NewGuid()
            };
            this.commentServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.commentController.Put(Guid.NewGuid(), commentResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutCommentFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var commentResquest = new CommentRequest
            {
                Author = "Ciro Fernandes Matrigrani",
                Content = new String('*', 12001),
                PostId = Guid.NewGuid()
            };
            this.commentServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await this.commentController.Put(Guid.NewGuid(), commentResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutCommentFailureThrowsGenericExceptionReturnsConflict()
        {
            // arrange
            var commentResquest = new CommentRequest();
            this.commentServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new Exception());

            // act
            var error = await this.commentController.Put(Guid.NewGuid(), commentResquest, default);
            var objectResult = error as ConflictObjectResult;

            // assert 
            Assert.True(objectResult is ConflictObjectResult);
            Assert.Equal(409, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPutCommentFailureThrowsUnprocessableEntityException()
        {
            // arrange
            var commentResquest = new CommentRequest();
            this.commentServiceMock.Setup(r => r.Put(It.IsAny<Guid>(), commentResquest, default)).Throws(() => new UnprocessableEntityException());

            // act
            var error = await this.commentController.Put(Guid.NewGuid(), commentResquest, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert 
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchSuccessReturnsCommentResponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var expectedCommentAuthor = "Ciro";
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "replace", path: "/Author", null, value: expectedCommentAuthor));
            var commentRes = new CommentResponse()
            {
                Author = "Ciro Matrigrani",
                Content = "Base Project - Hexagon Arch Ciro Matrigrani 2025",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            this.commentServiceMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(commentRes);
            this.commentServiceMock.Setup(r => r.Put(commentRes.Id, It.IsAny<CommentRequest>(), default)).ReturnsAsync(commentRes);

            // act
            var response = await commentController.Patch(commentId, patchEntity, default);
            var objectResult = response as NoContentResult;

            // assert
            Assert.True(objectResult is NoContentResult);
            Assert.Equal(204, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsBadRequestException()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var expectedCommentAuthor = "Ciro";
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "replace", path: "/Author", null, value: expectedCommentAuthor));
            this.commentServiceMock.Setup(r => r.Patch(commentId, patchEntity, default)).Throws(() => new BadRequestException());

            // act
            var error = await commentController.Patch(commentId, patchEntity, default);
            var objectResult = error as BadRequestObjectResult;

            // assert
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "add", path: "/Author", null, value: "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani"));
            this.commentServiceMock.Setup(r => r.Patch(commentId, patchEntity, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await commentController.Patch(commentId, patchEntity, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "add", path: "/Content", null, value: new String('*', 121)));
            this.commentServiceMock.Setup(r => r.Patch(commentId, patchEntity, default)).Throws(() => new ExceedMaxSizeCharactersException());

            // act
            var error = await commentController.Patch(commentId, patchEntity, default);
            var objectResult = error as UnprocessableEntityObjectResult;

            // assert
            Assert.True(objectResult is UnprocessableEntityObjectResult);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async void TestPatchFailureThrowsNotFoundException()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "add", path: "/Content", null, value: new String('*', 121)));
            this.commentServiceMock.Setup(r => r.Patch(commentId, patchEntity, default)).Throws(() => new NotFoundException());

            // act
            var error = await commentController.Patch(commentId, patchEntity, default);
            var objectResult = error as NotFoundObjectResult;

            // assert
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }
    }
}