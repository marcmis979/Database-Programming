using BBL.DTOModels;
using BBL.ServiceInterfaces;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    // 1. Implementacja GenerateOrder()
    public int GenerateOrder()
    {
        throw new NotImplementedException("Implementacja tej metody wymaga dalszej logiki.");
    }

    // 2. Implementacja PayOrder()
    public void PayOrder(int orderId, decimal amount)
    {
        var order = _context.Orders.FirstOrDefault(o => o.ID == orderId);

        if (order == null)
            throw new ArgumentException("Zamówienie nie istnieje.");

        if (order.IsPaid)
            throw new InvalidOperationException("Zamówienie jest już opłacone.");

        decimal totalAmount = _context.OrderPositions
            .Where(op => op.OrderID == orderId)
            .Sum(op => (decimal)op.Price * op.Amount);

        if (amount != totalAmount)
            throw new InvalidOperationException("Podana kwota nie zgadza się z wartością zamówienia.");

        order.IsPaid = true;
        _context.SaveChanges();
    }

    // 3. Implementacja GetOrders()
    public IEnumerable<OrderResponseDTO> GetOrders(
        string? sortBy = "Value",
        bool ascending = true,
        int? orderIdFilter = null,
        bool? isPaidFilter = null)
    {
        var orders = _context.Orders.AsQueryable();

        // Filtrowanie
        if (orderIdFilter.HasValue)
            orders = orders.Where(o => o.ID == orderIdFilter.Value);

        if (isPaidFilter.HasValue)
            orders = orders.Where(o => o.IsPaid == isPaidFilter.Value);

        // Sortowanie
        orders = sortBy switch
        {
            "ID" => ascending ? orders.OrderBy(o => o.ID) : orders.OrderByDescending(o => o.ID),
            "Date" => ascending ? orders.OrderBy(o => o.Date) : orders.OrderByDescending(o => o.Date),
            _ => ascending ? orders.OrderBy(o => o.ID) : orders.OrderByDescending(o => o.ID)
        };

        // Mapowanie do DTO
        return orders.Select(o => new OrderResponseDTO(
            o.ID, // Przekazanie OrderID
            (double)_context.OrderPositions
                .Where(op => op.OrderID == o.ID)
                .Sum(op => (decimal)op.Price * op.Amount), // Rzutowanie z decimal na double
            o.IsPaid,
            o.Date
        )).ToList();


    }

    // 4. Implementacja GetOrderItems()
    public IEnumerable<OrderPositionResponseDTO> GetOrderItems(int orderId)
    {
        return _context.OrderPositions
            .Where(op => op.OrderID == orderId)
            .Select(op => new OrderPositionResponseDTO(
                op.OrderID, // OrderID
                op.ProductID, // ProductID
                op.Amount, // Amount
                (double)op.Price, // Price, rzutowanie z decimal na double
                op.Product.Name // ProductName
            ))
            .ToList();

    }
}
