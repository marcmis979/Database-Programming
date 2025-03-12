using BBL.DTOModels;
using BBL.ServiceInterfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL_EF
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public int GenerateOrder()
        {
            var order = new Order
            {
                Date = DateTime.Now,
                IsPaid = false,
                UserID = UserService.LoggedUser!.ID  
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.ID;
        }

        public void PayOrder(int orderId, decimal amount)
        {
            var order = _context.Orders.SingleOrDefault(o => o.ID == orderId);
            if (order == null) throw new InvalidOperationException("Order not found.");
            if (order.IsPaid) throw new InvalidOperationException("Order has already been paid.");

            order.IsPaid = true;
            _context.SaveChanges();
        }

        public IEnumerable<OrderResponseDTO> GetOrders(string? sortBy = "Value", bool ascending = true, int? orderIdFilter = null, bool? isPaidFilter = null)
        {
            var query = _context.Orders.AsQueryable();

            if (orderIdFilter.HasValue)
            {
                query = query.Where(o => o.ID == orderIdFilter);
            }

            if (isPaidFilter.HasValue)
            {
                query = query.Where(o => o.IsPaid == isPaidFilter);
            }

            query = sortBy?.ToLower() switch
            {
                "value" => ascending ? query.OrderBy(o => o.OrderPositions.Sum(op => op.Price)) : query.OrderByDescending(o => o.OrderPositions.Sum(op => op.Price)),
                "date" => ascending ? query.OrderBy(o => o.Date) : query.OrderByDescending(o => o.Date),
                _ => query.OrderBy(o => o.ID)
            };

            return query.Select(o => new OrderResponseDTO(
                o.ID,
                o.OrderPositions.Sum(op => op.Price),
                o.IsPaid,
                o.Date
            )).ToList();

        }

        public IEnumerable<OrderPositionResponseDTO> GetOrderItems(int orderId)
        {
            var orderItems = _context.OrderPositions
                .Include(op => op.Product)
                .Where(oi => oi.OrderID == orderId)
                .ToList();

            return orderItems.Select(op => new OrderPositionResponseDTO(
                op.OrderID,
                op.ProductID,
                op.Amount,
                op.Price,
                op.Product.Name
            )).ToList();

        }
    }
}
