using BBL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.ServiceInterfaces
{
    public interface IOrderService
    {
        int GenerateOrder();
        void PayOrder(int orderId, decimal amount);
        IEnumerable<OrderResponseDTO> GetOrders(
            string? sortBy = "Value",
            bool ascending = true,
            int? orderIdFilter = null,
            bool? isPaidFilter = null);

        IEnumerable<OrderPositionResponseDTO> GetOrderItems(int orderId);
    }
}

