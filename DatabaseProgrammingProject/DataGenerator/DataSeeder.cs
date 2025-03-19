using Bogus;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;

public class DataSeeder
{
    private readonly AppDbContext _context;

    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        Console.WriteLine("Czyszczenie bazy...");
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();

        Console.WriteLine("Generowanie danych...");

        var users = GenerateUsers(10);
        _context.Users.AddRange(users);
        _context.SaveChanges();

        var productGroups = GenerateProductGroups();
        _context.ProductGroups.AddRange(productGroups);
        _context.SaveChanges();

        var products = GenerateProducts(30, productGroups);
        _context.Products.AddRange(products);
        _context.SaveChanges();

        var orders = GenerateOrders(users, products);
        _context.Orders.AddRange(orders);
        _context.SaveChanges();

        Console.WriteLine("Dane wygenerowane!");
    }

    private List<User> GenerateUsers(int count)
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.Login, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Type, f => f.PickRandom<UserType>())
            .RuleFor(u => u.IsActive, f => f.Random.Bool());

        return faker.Generate(count);
    }

    private List<ProductGroup> GenerateProductGroups()
    {
        var groups = new List<ProductGroup>
        {
            new() { Name = "Narzędzia", ParentID = null },
            new() { Name = "Elektryczne", ParentID = 1 },
            new() { Name = "Wiertarki", ParentID = 2 }
        };

        return groups;
    }

    private List<Product> GenerateProducts(int count, List<ProductGroup> groups)
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Double(10, 500))
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.GroupID, f => f.PickRandom(groups).ID)
            .RuleFor(p => p.Image, f => "default.jpg");  // 📌 Ustaw domyślną wartość dla Image

        return faker.Generate(count);
    }


    private List<Order> GenerateOrders(List<User> users, List<Product> products)
    {
        var orders = new List<Order>();

        foreach (var user in users)
        {
            var order = new Order
            {
                UserID = user.ID,
                Date = DateTime.UtcNow.AddDays(-new Random().Next(30)),
                IsPaid = new Random().Next(2) == 1
            };

            var orderPositions = new List<OrderPosition>();

            foreach (var product in products.OrderBy(_ => Guid.NewGuid()).Take(3))
            {
                orderPositions.Add(new OrderPosition
                {
                    OrderID = order.ID,
                    ProductID = product.ID,
                    Amount = new Random().Next(1, 5),
                    Price = product.Price
                });
            }

            orders.Add(order);
        }

        return orders;
    }
}
