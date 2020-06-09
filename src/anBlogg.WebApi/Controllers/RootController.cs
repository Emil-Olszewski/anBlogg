using anBlogg.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace anBlogg.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>
            {
                new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"),
                new LinkDto(Url.Link("GetPosts", new { }), "posts", "GET")
            };

            return Ok(links);
        }
    }
}