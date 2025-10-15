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
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ICommentService commentService, ILogger<PostController> logger = null)
        {
            _postService = postService;
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all posts entities.
        /// </summary>
        /// <returns>List of Posts</returns>
        /// <response code="200">Ok - List of posts</response>
        /// <response code="404">Not Found - No posts available</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetAll(CancellationToken token = default)
        {
            try
            {
                var postsReponse = await _postService.GetAll(token);
                return Ok(postsReponse);
            }
            catch
            {
                return NotFound(new ErrorResponse { Message = $"Not Found - No posts available." });
            }
        }

        /// <summary>
        /// Returns a post entity based in a given identifier.
        /// </summary>
        /// <param name="postId">The Post identifier.</param>
        /// <returns>List of Posts</returns>
        /// <response code="200">Ok - Return a posts by the identifier</response>
        /// <response code="404">Not Found - No posts found with the given identifier.</response>
        [HttpGet("{postId:guid}")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostResponse>> Get([FromRoute] Guid postId, CancellationToken token = default)
        {
            try
            {
                var postReponse = await _postService.Get(postId, token);
                return Ok(postReponse);
            }
            catch
            {
                return NotFound(new ErrorResponse { Message = $"Not Found - Post entity identifier {postId} not found." });
            }
        }

        /// <summary>
        /// Register a new post entity based in a given entity.
        /// </summary>
        /// <param name="postRequest">The new Post schema and values.</param>      
        /// <returns>The post identifier</returns>
        /// <response code="201">Created - Returns the new post identifier.</response>
        /// <response code="422">Unprocessable Entity - Title or Content exeed the size character limit of 30 and 1200 repectively.</response>
        /// <response code="400">Bad Request- Wrong schema for entity.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UnprocessableEntityException), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostResponse>> Post([FromBody] PostRequest postRequest, CancellationToken token = default)
        {
            try
            {
                var newPostId = Guid.NewGuid();
                return Created(newPostId.ToString(), await _postService.Post(newPostId, postRequest));
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
                return BadRequest(new ErrorResponse { Message = $"Bad request - Wrong schema for entity {JsonSerializer.Serialize(postRequest)}." });
            }
        }

        /// <summary>
        /// Delete a post entity based in a given identifier.
        /// </summary>
        /// <param name="postRequest">The new Post schema and values.</param>      
        /// <returns>The post identifier</returns>
        /// <response code="204">NoContent - Post entity deleted sucessfully</response>
        /// <response code="404">Not Found - No posts found with the given identifier.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        [HttpDelete("{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId, CancellationToken token = default)
        {
            try
            {
                await _postService.Delete(postId, token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new NotFoundException(postId, ex));
            }
        }

        /// <summary>
        /// Update a post entity based in a given identifier and entity.
        /// </summary>
        /// <param name="postId">The Post identifier.</param>
        /// <param name="postRequest">The Post schema to update the entity.</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">NoContent - Post Entity Updated sucessfully</response>
        /// <response code="201">Created - Post created, returns the new post identifier.</response>
        /// <response code="422">Unprocessable Entity - Wrong schema for entity.</response>
        /// <response code="409">Conflict - Problems trying to updateg the post.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        [HttpPut("{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UnprocessableEntityException), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromRoute] Guid postId, [FromBody] PostRequest postRequest, CancellationToken token = default)
        {
            try
            {
                var postResponse = await _postService.Put(postId, postRequest);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return Created(postId.ToString(), postRequest);
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
                return Conflict(new ErrorResponse { Message = $"Conflict - Problems trying to updateg the post {postId}." });
            }
        }

        /// <summary>
        /// Update partially a post entity based in a given identifier and a JsonPatchDocument pattern.
        /// </summary>
        /// <param name="postId">The Post identifier.</param>
        /// <param name="postPatchRequest">The patch pattern to update Request. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0.</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">NoContent - Post entity Updated sucessfully.</response>
        /// <response code="404">Not Found - Post entity identifier not found.</response>
        /// <response code="400">Bad Request - Check the correct schema. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0.</response>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        /// <seealso cref="See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0"/>
        [HttpPatch("{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(NotFoundException), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromRoute] Guid postId, [FromBody] JsonPatchDocument<PostRequest> postPatchRequest, CancellationToken token = default)
        {
            try
            {
                var post = await _postService.Patch(postId, postPatchRequest, token);
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

        /// <summary>
        /// Returns a list of comments entities of a post given by the identifier.
        /// </summary>
        /// <param name="postId">The Post identifier.</param>
        /// <returns>List of Comments.</returns>
        /// <response code="200">Ok - Returns a list of comments entities of a post given by the identifier.</response>
        /// <response code="404">Not Found - No posts found with the given identifier.</response>
        [HttpGet("{postId:guid}/comments")]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByPostId([FromRoute] Guid postId, CancellationToken token = default)
        {
            try
            {
                var commentsReponse = await _commentService.GetCommentsByPostId(postId, token);
                return Ok(commentsReponse);
            }
            catch
            {
                return NotFound(new ErrorResponse { Message = $"Not Found - Post entity identifier {postId} not found." });
            }
        }
    }
}