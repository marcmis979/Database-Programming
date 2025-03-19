using BBL.DTOModels;
using BBL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketPositionService _basketService;

        public BasketController(IBasketPositionService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        public IActionResult AddToBasket([FromBody] BasketPositionDTO item)
        {
            if (item == null || item.ProductID <= 0 || item.Amount <= 0)
            {
                return BadRequest("Invalid basket item data.");
            }

            _basketService.AddToBasket(item);
            return Ok("Item added to basket.");
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateBasketItem(int productId, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            _basketService.UpdateBasketItems(productId, quantity);
            return Ok("Basket item updated.");
        }

        [HttpDelete("{productId}")]
        public IActionResult RemoveFromBasket(int productId)
        {
            _basketService.RemoveFromBasket(productId);
            return Ok("Item removed from basket.");
        }
    }
}
