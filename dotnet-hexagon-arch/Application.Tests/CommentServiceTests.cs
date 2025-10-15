using AutoMapper;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Exceptions;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Model;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Repository;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Services.Tests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Tests
{
    public class CommentServiceTests : IClassFixture<TestServicesFixture>
    {
        private readonly TestServicesFixture fixture;
        private readonly ICommentService commentService;
        private readonly Mock<ICommentRepository> commentRepositoryMock;
        private readonly IMapper mapper;

        public CommentServiceTests(TestServicesFixture fixture)
        {
            this.fixture = fixture;
            this.commentRepositoryMock = this.fixture.CommentRepositoryMock;
            this.commentService = this.fixture.CommentService;
            this.mapper = this.fixture.Mapper;
        }

        [Fact]
        public async void TestGetCommentsSuccessReturnsList()
        {
            // arrange
            var commentsRes = new List<Comment> { new Comment() };
            this.commentRepositoryMock.Setup(r => r.GetAll(default)).ReturnsAsync(commentsRes);

            // act
            var comments = await commentService.GetAll(default);

            // assert
            Assert.Equal(commentsRes.Count, comments.Count());
        }

        [Fact]
        public async void TestGetCommentsSuccessReturnsEmptyList()
        {
            // arrange
            var commentsRes = new List<Comment> { };
            this.commentRepositoryMock.Setup(r => r.GetAll(default)).ReturnsAsync(commentsRes);

            // act
            var comments = await commentService.GetAll(default);

            // assert
            Assert.Empty(comments);
        }

        [Fact]
        public async void TestGetCommentsByPostIdSuccessReturnsList()
        {
            // arrange
            var postId = Guid.NewGuid();
            var commentRes = new List<Comment> { new Comment() { PostId = postId } };
            this.commentRepositoryMock.Setup(r => r.GetByPostId(postId, default)).ReturnsAsync(commentRes);

            // act
            var comments = await this.commentService.GetCommentsByPostId(postId, default);

            // assert
            Assert.Equal(commentRes.Count, comments.Count());
            Assert.Equal(commentRes.First().Id, comments.First().Id);
        }

        [Fact]
        public async void TestGetCommentsByPostIdSuccessReturnsEmptyList()
        {
            // arrange
            var postId = Guid.NewGuid();
            var commentRes = new List<Comment> { };
            this.commentRepositoryMock.Setup(r => r.GetByPostId(postId, default)).ReturnsAsync(commentRes);

            // act
            var comments = await this.commentService.GetCommentsByPostId(postId, default);

            // assert
            Assert.Empty(comments);
        }

        [Fact]
        public async void TestGetCommentSuccessReturnsCommentReponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new Comment() { Id = commentId };
            this.commentRepositoryMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(commentRes);

            // act
            var comment = await commentService.Get(commentId, default);

            // assert
            Assert.Equal(commentId, comment.Id);
        }

        [Fact]
        public async void TestGetCommentFailureThrowsException()
        {
            // arrange
            this.commentRepositoryMock.Setup(r => r.Get(Guid.Empty, default)).Throws<Exception>();

            // act, assert
            await Assert.ThrowsAsync<Exception>(() => this.commentService.Get(Guid.Empty, default));
        }

        [Fact]
        public async void TestPostCommentSuccessReturnCommentReponse()
        {

            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new Comment()
            {
                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api.",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            var commentRequest = this.mapper.Map<CommentRequest>(commentRes);
            this.commentRepositoryMock.Setup(r => r.Create(It.IsAny<Comment>(), default)).ReturnsAsync(commentRes);

            // act
            var comment = await this.commentService.Post(commentId, commentRequest, default);

            // assert
            Assert.Equal(commentId, comment.Id);
        }

        [Fact]
        public async void TestPostCommentFailureThrowsUnprocessableEntityException()
        {
            // act, assert
            await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.commentService.Post(Guid.Empty, new CommentRequest(), default));
        }

        [Fact]
        public async void TestPostCommentFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Post(Guid.Empty,
                new CommentRequest()
                {
                    Author = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                    Content = "Mindera Challenge 2022 - DotNet 5 Rest Api.",
                    PostId = Guid.NewGuid()
                },
                default));
        }

        [Fact]
        public async void TestPostCommentFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Post(Guid.Empty,
                new CommentRequest()
                {
                    Author = "Ciro Fernandes Matrigrani",
                    Content = new string('*', 121),
                    PostId = Guid.NewGuid()
                },
                default));
        }

        [Fact]
        public async void TestDeleteCommentSuccessReturnsTrue()
        {

            // arrange
            var commentId = Guid.NewGuid();
            this.commentRepositoryMock.Setup(r => r.Delete(commentId, default)).ReturnsAsync(true);

            // act
            var response = await commentService.Delete(commentId, default);

            // assert
            Assert.True(response);
        }

        [Fact]
        public async void TestDeleteCommentFailureReturnsFalse()
        {
            // arrange
            this.commentRepositoryMock.Setup(r => r.Delete(Guid.Empty, default)).ReturnsAsync(false);

            // act
            var response = await commentService.Delete(Guid.Empty, default);

            // assert
            Assert.False(response);
        }

        [Fact]
        public async void TestDeleteCommentFailureReturnException()
        {
            // arrange
            this.commentRepositoryMock.Setup(r => r.Delete(Guid.Empty, default)).ThrowsAsync(new Exception());

            // act, assert
            await Assert.ThrowsAsync<Exception>(() => this.commentService.Delete(Guid.Empty, default));
        }

        [Fact]
        public async void TestPutCommentSuccessReturnsCommentReponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var commentRes = new Comment()
            {
                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            var commentRequest = this.mapper.Map<CommentRequest>(commentRes);
            this.commentRepositoryMock.Setup(r => r.Update(It.IsAny<Comment>(), default)).ReturnsAsync(commentRes);

            // act
            var commentResponse = await commentService.Put(commentId, commentRequest, default);

            // assert
            Assert.Equal(commentId, commentResponse.Id);
        }

        [Fact]
        public async void TestPutCommentFailureThrowsUnprocessableEntityException()
        {
            // act, assert
            await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.commentService.Put(Guid.Empty, new CommentRequest(), default));
        }


        [Fact]
        public async void TestPutCommentFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Put(Guid.Empty,
                new CommentRequest()
                {
                    Author = "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani ",
                    Content = "Mindera Challenge 2022 - DotNet 5 Rest Api.",
                    PostId = Guid.NewGuid()
                },
                default));
        }

        [Fact]
        public async void TestPutCommentFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Put(Guid.Empty,
                new CommentRequest()
                {
                    Author = "Ciro Fernandes Matrigrani",
                    Content = new string('*', 121),
                    PostId = Guid.NewGuid()
                },
                default));
        }

        [Fact]
        public async void TestPatchCommentSuccessReturnsCommentReponse()
        {
            // arrange
            var commentId = Guid.NewGuid();
            var expectedCommentAuthor = "Ciro";
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "replace", path: "/Author", null, value: expectedCommentAuthor));
            var commentRes = new Comment()
            {

                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            };
            this.commentRepositoryMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(commentRes);
            this.commentRepositoryMock.Setup(r => r.Update(It.IsAny<Comment>(), default)).ReturnsAsync(commentRes);

            // act
            var commentResponse = await commentService.Patch(commentId, patchEntity, default);

            // assert
            Assert.Equal(commentId, commentResponse.Id);
        }

        [Fact]
        public async void TestPatchCommentFailureThrowsBadRequestException()
        {
            // arrange
            this.commentRepositoryMock.Setup(r => r.Get(Guid.Empty, default)).ReturnsAsync(new Comment());
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "x", path: "/y", null, value: "z"));

            // act, assert
            await Assert.ThrowsAsync<BadRequestException>(() => this.commentService.Patch(Guid.Empty, patchEntity, default));
        }

        [Fact]
        public async void TestPatchCommentFailureThrowsExceedMaxSizeCharactersExceptionByAuthor()
        {
            // arrange
            var commentId = Guid.NewGuid();
            this.commentRepositoryMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(new Comment
            {
                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            });
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "add", path: "/Author", null, value: "Ciro Fernandes Matrigrani x2 Ciro Fernandes Matrigrani"));

            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Patch(commentId,
                patchEntity,
                default));
        }

        [Fact]
        public async void TestPatchCommentFailureThrowsExceedMaxSizeCharactersExceptionByContent()
        {
            // arrange
            var commentId = Guid.NewGuid();
            this.commentRepositoryMock.Setup(r => r.Get(commentId, default)).ReturnsAsync(new Comment
            {
                Author = "Ciro Matrigrani",
                Content = "Mindera Challenge 2022 - DotNet 5 Rest Api .",
                CreationDate = DateTime.Now,
                PostId = Guid.NewGuid(),
                Id = commentId
            });
            var patchEntity = new JsonPatchDocument<CommentRequest>();
            patchEntity.Operations.Add(new Operation<CommentRequest>(op: "add", path: "/Content", null, value: new string('*', 121)));

            // act, assert
            await Assert.ThrowsAsync<ExceedMaxSizeCharactersException>(() => this.commentService.Patch(commentId,
                patchEntity,
                default));
        }

    }
}
