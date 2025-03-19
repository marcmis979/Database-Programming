using DAL;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("your_connection_string")
            .Options;

        using var context = new AppDbContext(options);
        var seeder = new DataSeeder(context);
        seeder.Seed();
    }
}
