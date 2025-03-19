using BBL.DTOModels;
using BBL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult GetOrders(
            string sortBy = "Value",
            bool ascending = true,
            int? orderIdFilter = null,
            bool? isPaidFilter = null)
        {
            var orders = _orderService.GetOrders(sortBy, ascending, orderIdFilter, isPaidFilter);
            return Ok(orders);
        }

        // GET: api/orders/{orderId}/items
        [HttpGet("{orderId}/items")]
        public IActionResult GetOrderItems(int orderId)
        {
            var orderItems = _orderService.GetOrderItems(orderId);
            if (orderItems == null)
            {
                return NotFound();
            }
            return Ok(orderItems);
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult CreateOrder()
        {
            var orderId = _orderService.GenerateOrder();
            return CreatedAtAction(nameof(GetOrders), new { orderId = orderId }, orderId);
        }

        // PUT: api/orders/{orderId}/pay
        [HttpPut("{orderId}/pay")]
        public IActionResult PayOrder(int orderId, decimal amount)
        {
            _orderService.PayOrder(orderId, amount);
            return NoContent(); // Successful payment, no content to return
        }
    }
}
