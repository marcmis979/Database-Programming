using BBL.DTOModels;
using BBL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductResponseDTO>> GetProducts(
            [FromQuery] string sortBy = "Name",
            [FromQuery] bool ascending = true,
            [FromQuery] string? nameFilter = null,
            [FromQuery] string? groupNameFilter = null,
            [FromQuery] int? groupIdFilter = null,
            [FromQuery] bool includeInactive = false)
        {
            var products = _productService.GetProducts(sortBy, ascending, nameFilter, groupNameFilter, groupIdFilter, includeInactive);
            return Ok(products);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductRequestDTO productDto)
        {
            _productService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetProducts), new { id = productDto.Name }, productDto);
        }

        // PUT: api/products/deactivate/5
        [HttpPut("deactivate/{id}")]
        public IActionResult DeactivateProduct(int id)
        {
            _productService.DeactivateProduct(id);
            return NoContent();
        }

        // PUT: api/products/activate/5
        [HttpPut("activate/{id}")]
        public IActionResult ActivateProduct(int id)
        {
            _productService.ActiveProduct(id);
            return NoContent();
        }
    }
}
