using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Constants;

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

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] string productName) 
        {
            return Ok(productName);
        }

        [Authorize(Policy = Policies.BusinessHours)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id) 
        {
            return Ok($"Item deleted: {id}");
        }
    }
}