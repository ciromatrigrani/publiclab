using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Exceptions;
using MatrigraniCiro.MinderaChallenge.BestBlogs.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.MinderaChallenge.BestBlogs.Api.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger = null)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all comments entities.
        /// </summary>
        /// <returns>List of Comments</returns>
        /// <response code="200">Ok - List of comments</response>
        /// <response code="404">Not Found - No comments available</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAll(CancellationToken token = default)
        {
            try
            {
                var commentsReponse = await _commentService.GetAll(token);
                return Ok(commentsReponse);
            }
            catch
            {
                return NotFound(new ErrorResponse { Message = $"Not Found - No comments available." });
            }
        }

        /// <summary>
        /// Returns a comment entity based in a given identifier.
        /// </summary>
        /// <param name="commentId">The Comment identifier.</param>
        /// <returns>List of Comments</returns>
        /// <response code="200">Ok - Return a comments by the identifier</response>
        /// <response code="404">Not Found - No comments found with the given identifier.</response>
        [HttpGet("{commentId:guid}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommentResponse>> Get([FromRoute] Guid commentId, CancellationToken token = default)
        {
            try
            {
                var commentReponse = await _commentService.Get(commentId, token);
                return Ok(commentReponse);
            }
            catch
            {
                return NotFound(new ErrorResponse { Message = $"Not Found - Comment entity identifier {commentId} not found." });
            }
        }

        /// <summary>
        /// Register a new comment entity based in a given entity.
        /// </summary>
        /// <param name="commentRequest">The new Comment schema and values.</param>      
        /// <returns>The comment identifier</returns>
        /// <response code="201">Created - Returns the new comment identifier.</response>
        /// <response code="422">Unprocessable Entity - Author or Content exeed the size character limit.</response>
        /// <response code="400">Unprocessable Entity - Wrong schema for entity.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceedMaxSizeCharactersException), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommentResponse>> Post([FromBody] CommentRequest commentRequest, CancellationToken token = default)
        {
            try
            {
                var newCommentId = Guid.NewGuid();
                return Created(newCommentId.ToString(), await _commentService.Post(newCommentId, commentRequest, token));
            }
            catch (ExceedMaxSizeCharactersException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch (UnprocessableEntityException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch
            {
                return BadRequest(new ErrorResponse { Message = $"Bad request - Wrong schema for entity {JsonSerializer.Serialize(commentRequest)}." });
            }
        }

        /// <summary>
        /// Delete a comment entity based in a given identifier.
        /// </summary>
        /// <param name="commentRequest">The new Comment schema and values.</param>      
        /// <returns>The comment identifier</returns>
        /// <response code="204">NoContent - Comment entity deleted sucessfully</response>
        /// <response code="404">Not Found - No comments found with the given identifier.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        [HttpDelete("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid commentId, CancellationToken token = default)
        {
            try
            {
                await _commentService.Delete(commentId, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new NotFoundException(commentId, ex));
            }
        }

        /// <summary>
        /// Update a comment entity based in a given identifier and entity.
        /// </summary>
        /// <param name="commentId">The Comment identifier.</param>
        /// <param name="commentRequest">The Comment schema to update the entity.</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">NoContent - Comment Entity Updated sucessfully</response>
        /// <response code="201">Created - Comment created, returns the new comment identifier.</response>
        /// <response code="422">Unprocessable Entity - Wrong schema for entity.</response>
        /// <response code="409">Conflict - Problems trying to updateg the comment.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        [HttpPut("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UnprocessableEntityException), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromRoute] Guid commentId, [FromBody] CommentRequest commentRequest, CancellationToken token = default)
        {
            try
            {
                var commentResponse = await _commentService.Put(commentId, commentRequest, token);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return Created(commentId.ToString(), commentRequest);
            }
            catch (UnprocessableEntityException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch (ExceedMaxSizeCharactersException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch
            {
                return Conflict(new ErrorResponse { Message = $"Conflict - Problems trying to update the comment {commentId}." });
            }
        }

        /// <summary>
        /// Update partially a comment entity based in a given identifier and a JsonPatchDocument pattern.
        /// </summary>
        /// <param name="commentId">The Comment identifier.</param>
        /// <param name="commentPatchRequest">The patch pattern to update Request. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0.</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">NoContent - Comment entity Updated sucessfully.</response>
        /// <response code="404">Not Found - Comment entity identifier not found.</response>
        /// <response code="400">Bad Request - Check the correct schema. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        /// <seealso cref="See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0"/>
        [HttpPatch("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ExceedMaxSizeCharactersException), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromRoute] Guid commentId, [FromBody] JsonPatchDocument<CommentRequest> commentPatchRequest, CancellationToken token = default)
        {
            try
            {
                var comment = await _commentService.Patch(commentId, commentPatchRequest, token);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (ExceedMaxSizeCharactersException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch (UnprocessableEntityException ex)
            {
                return UnprocessableEntity(ex);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ErrorResponse { Message = $"{ex.Message}. Check the correct schema. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0." });
            }
        }
    }
}