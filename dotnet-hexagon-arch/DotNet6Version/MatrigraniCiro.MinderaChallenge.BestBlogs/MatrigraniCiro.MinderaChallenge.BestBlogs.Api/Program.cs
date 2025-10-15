using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Exceptions;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Mapping;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Application.Services;
using MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;

/*
 * General config
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("flight_mesh_db"));
builder.Services.AddAutoMapper(mapper => mapper.AddMaps(typeof(Mapping).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ciro Matrigrani Minimal Api Mindera Challenge BlogPosts",
        Description = "DotNet Core Minimal API for BlogPosts and Comments",
        Contact = new OpenApiContact
        {
            Name = "Ciro Matrigrani",
            Email = "ciromatrigrani@gmail.com",
            Url = new Uri("https://sites.google.com/site/cmatripgita/")
        }
    });
});

builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<ICommentService, CommentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * Dot NET 6 Minimal API Comment Controller
 */

app.MapGet("Comment", async ([FromServices] ICommentService commentService) => Results.Ok(await commentService.GetAll()));

app.MapGet("Comment/{commentId:guid}", async ([FromRoute] Guid commentId, [FromServices] ICommentService commentService) =>
{
    try
    {
        var commentReponse = await commentService.Get(commentId);
        Results.Ok(commentReponse);
    }
    catch
    {
        Results.NotFound(new ErrorResponse { Message = $"Not Found - Comment entity identifier {commentId} not found." });
    }
});

app.MapGet("Comment/{CommentId}", async ([FromRoute] Guid commentId, [FromServices] ICommentService commentService) =>
{
    try
    {
        var commentReponse = await commentService.Get(commentId);
        return Results.Ok(commentReponse);
    }
    catch { return Results.NotFound(commentId); }
});

app.MapPost("Comment", async ([FromBody] CommentRequest commentRequest, [FromServices] ICommentService commentService) =>
    {
        try
        {
            var newCommentId = Guid.NewGuid();
            Results.Created(newCommentId.ToString(), await commentService.Post(newCommentId, commentRequest));
        }
        catch (ExceedMaxSizeCharactersException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch (UnprocessableEntityException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch
        {
            Results.BadRequest(new ErrorResponse { Message = $"Bad request - Wrong schema for entity {JsonSerializer.Serialize(commentRequest)}." });
        }
    });

app.MapDelete("Comment/{CommentId}", async ([FromRoute] Guid commentId, [FromServices] ICommentService commentService) =>
        await commentService.Delete(commentId) ? Results.NoContent() : Results.NotFound(commentId));

app.MapPut("Comment/{commentId}/", async ([FromRoute] Guid commentId, [FromBody] CommentRequest commentRequest, [FromServices] ICommentService commentService) =>
    {
        try
        {
            var commentResponse = await commentService.Put(commentId, commentRequest);
            Results.NoContent();
        }
        catch (NotFoundException)
        {
            Results.Created(commentId.ToString(), commentRequest);
        }
        catch (UnprocessableEntityException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch (ExceedMaxSizeCharactersException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch
        {
            Results.Conflict(new ErrorResponse { Message = $"Conflict - Problems trying to update the comment {commentId}." });
        }
    });

app.MapMethods("Comment/{commentId}/", new List<string> { "PATCH" },
    async ([FromRoute] Guid commentId, [FromBody] JsonPatchDocument<CommentRequest> commentPatchRequest, [FromServices] ICommentService commentService) =>
    {
        try
        {
            var comment = await commentService.Patch(commentId, commentPatchRequest);
            Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            Results.NotFound(ex);
        }
        catch (ExceedMaxSizeCharactersException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch (UnprocessableEntityException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch (BadRequestException ex)
        {
            Results.BadRequest(new ErrorResponse { Message = $"{ex.Message}. Check the correct schema. See https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0." });
        }
    });



/*
 * Dot NET 6 Minimal API Posts Controller
 */

app.MapGet("Post", async ([FromServices] IPostService postService) => Results.Ok(await postService.GetAll()));

app.MapGet("Post/{TenantId}", async ([FromRoute] Guid tenantId, [FromServices] IPostService postService) =>
    Results.Ok(await postService.Get(tenantId)));

app.MapPost("Post", async ([FromBody] PostRequest postRequest, [FromServices] IPostService postService) =>
    {
        try
        {
            var newPostId = Guid.NewGuid();
            Results.Created(newPostId.ToString(), await postService.Post(newPostId, postRequest));
        }
        catch (ExceedMaxSizeCharactersException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch (UnprocessableEntityException ex)
        {
            Results.UnprocessableEntity(ex);
        }
        catch
        {
            Results.BadRequest(new ErrorResponse { Message = $"Bad request - Wrong schema for entity {JsonSerializer.Serialize(postRequest)}." });
        }
    });

app.MapDelete("Post/{PostId}", async ([FromRoute] Guid postId, [FromServices] IPostService postService) =>
        await postService.Delete(postId) ? Results.NoContent() : Results.NotFound());

app.MapPut("Post", async ([FromQuery] Guid postId, [FromBody] PostRequest postRequest, [FromServices] IPostService postService) =>
{
    try
    {
        var postResponse = await postService.Put(postId, postRequest);
        Results.NoContent();
    }
    catch (NotFoundException)
    {
        Results.Created(postId.ToString(), postRequest);
    }
    catch (ExceedMaxSizeCharactersException ex)
    {
        Results.UnprocessableEntity(ex);
    }
    catch (UnprocessableEntityException ex)
    {
        Results.UnprocessableEntity(ex);
    }
    catch
    {
        Results.Conflict(new ErrorResponse { Message = $"Conflict - Problems trying to updateg the post {postId}." });
    }
});

app.MapGet("Post/{postId:guid}/Comments", async ([FromRoute] Guid postId, [FromServices] ICommentService commentService) =>
{
    try
    {
        var commentsReponse = await commentService.GetCommentsByPostId(postId, default);
        Results.Ok(commentsReponse);
    }
    catch
    {
        Results.NotFound(new ErrorResponse { Message = $"Not Found - Post entity identifier {postId} not found." });
    }
});


app.Run();