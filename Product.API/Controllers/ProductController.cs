using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("v1/product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() 
        {
            return Ok("ok");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            return Ok($"product id: {id}");
        }
    }
}