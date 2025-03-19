using BBL.DTOModels;
using BBL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IProductGroupService _productGroupService;

        public ProductGroupsController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        [HttpPost]
        public IActionResult AddGroup([FromBody] ProductGroupCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Group name cannot be empty.");
            }

            _productGroupService.AddGroup(dto.Name, dto.ParentId);
            return Ok("Group added successfully.");
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductGroupDTO>> GetGroups([FromQuery] int? parentId, [FromQuery] string sortBy = "Name", [FromQuery] bool ascending = true)
        {
            var groups = _productGroupService.GetGroups(parentId, sortBy, ascending);
            return Ok(groups);
        }
    }

    public record ProductGroupCreateDTO(string Name, int? ParentId);
}
