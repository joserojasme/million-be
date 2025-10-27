using Microsoft.AspNetCore.Mvc;
using Million.Application.Interfaces;
using System.Threading.Tasks;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _service;

        public PropertyController(IPropertyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] string? address,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var result = await _service.GetFilteredAsync(name, address, minPrice, maxPrice);
            return Ok(result);
        }
    }
}
