using Asp.Versioning;
using ASPNETCoreWithHeadersMiddleware.DTOs;
using ASPNETCoreWithHeadersMiddleware.Entities;
using ASPNETCoreWithHeadersMiddleware.Exceptions;
using ASPNETCoreWithHeadersMiddleware.Filters;
using ASPNETCoreWithHeadersMiddleware.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Net;

namespace ASPNETCoreWithHeadersMiddleware.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/posts")]
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePost([FromBody, Required] Post post)
        {
            var entity = TinyMapper.Map<PostEntity>(post);

            var isExist = await _postService.IsExistAsync(entity.Id);
            if (isExist)
                throw new ApiException((int)HttpStatusCode.Conflict, $"The post with the same id {post.Id} already exists");

            await _postService.CreateAsync(entity);

            var createdPost = await _postService.GetAsync(entity.Id);
            return Ok(TinyMapper.Map<Post>(createdPost));
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPost([FromRoute, Required] string id)
        {
            var post = await _postService.GetAsync(id);

            if (post is null) return NotFound();

            return Ok(post);
        }

        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost([FromRoute, Required] string id, [FromBody, Required] Post post)
        {
            var entity = TinyMapper.Map<PostEntity>(post);

            var isExist = await _postService.IsExistAsync(id);
            if (!isExist) return NotFound();

            if (!string.Equals(post.Id, id, StringComparison.OrdinalIgnoreCase))
                throw new ApiException((int)HttpStatusCode.BadRequest, "The post id should be the same");

            await _postService.UpdateAsync(entity);
            return Ok(TinyMapper.Map<Post>(entity));
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePost([FromRoute, Required] string id)
        {
            var isExist = await _postService.IsExistAsync(id);
            if (!isExist) return NotFound();

            await _postService.DeleteAsync(id);
            return NoContent();
        }
    }
}
