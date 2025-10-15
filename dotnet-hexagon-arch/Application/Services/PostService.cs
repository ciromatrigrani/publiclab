using AutoMapper;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Exceptions;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Services.Helpers;
using MatrigraniCiro.HexagonArch.BestBlogs.Model;
using MatrigraniCiro.HexagonArch.BestBlogs.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Delete(Guid postId, CancellationToken token = default)
        {
            return await this.postRepository.Delete(postId, token);
        }

        public async Task<PostResponse> Get(Guid postId, CancellationToken token = default)
        {
            return mapper.Map<PostResponse>(await this.postRepository.Get(postId, token)); ;
        }

        public async Task<IEnumerable<PostResponse>> GetAll(CancellationToken token = default)
        {
            return mapper.Map<IEnumerable<PostResponse>>(await this.postRepository.GetAll(token)); ;
        }

        public async Task<PostResponse> Patch(Guid postId, JsonPatchDocument<PostRequest> postPatchRequest, CancellationToken token = default)
        {
            try
            {
                var post = await this.postRepository.Get(postId, token);
                var postRequest = mapper.Map<PostRequest>(post);
                postPatchRequest.ApplyTo(postRequest);

                Guard.ValidatePost(postRequest);

                post = mapper.Map<Post>(postRequest);
                post.Id = postId;
                post = await this.postRepository.Update(post, token);
                return mapper.Map<PostResponse>(post);
            }
            catch (JsonPatchException ex) { throw new BadRequestException(postPatchRequest, ex); }
        }

        public async Task<PostResponse> Post(Guid newPostId, PostRequest postRequest, CancellationToken token = default)
        {
            Guard.ValidatePost(postRequest);

            var post = mapper.Map<Post>(postRequest);
            post.Id = newPostId;
            return mapper.Map<PostResponse>(await this.postRepository.Create(post, token));
        }

        public async Task<PostResponse> Put(Guid postId, PostRequest postRequest, CancellationToken token = default)
        {
            Guard.ValidatePost(postRequest);

            var post = mapper.Map<Post>(postRequest);
            post.Id = postId;
            return mapper.Map<PostResponse>(await this.postRepository.Update(post, token));
        }
    }
}
