using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

public class Auto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CarNumber { get; set; }
    public string Brand { get; set; }
    public double Price { get; set; }
    public string Address { get; set; }
}

public class AutoDbContext : DbContext
{
    public DbSet<Auto> Autos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=AutoDatabase.db");
    }
}
class Program
{
    static void Main()
    {
        using (var context = new AutoDbContext())
        {
            context.Database.EnsureCreated();

            var autos = new List<Auto>
            {
                new Auto
                {
                    Name = "Стас",
                    CarNumber = "AC679BA",
                    Brand = "BMW",
                    Price = 23000,
                    Address = "Ужгород, вул. Грушевського, 12"
                },
                new Auto
                {
                    Name = "Іван",
                    CarNumber = "AC689BA",
                    Brand = "BMW",
                    Price = 23000,
                    Address = "Ужгород, вул. Грушевського, 16"
                }
            };

            foreach (var auto in autos)
            {
                if (!context.Autos.Any(a => a.CarNumber == auto.CarNumber))
                {
                    context.Autos.Add(auto);
                }
            }

            context.SaveChanges();
        }

        using (var context = new AutoDbContext())
        {
            var autos = context.Autos.ToList();
            foreach (var auto in autos)
            {
                Console.WriteLine($"{auto.Name}, {auto.CarNumber}, {auto.Brand}, {auto.Price}, {auto.Address}");
            }
        }

        using (var context = new AutoDbContext())
        {
            var count = context.Autos
                .Where(a => a.Brand == "BMW" && a.CarNumber.Contains("7"))
                .Count();

            Console.WriteLine($"Кількість власників машин марки 'X' з номером, що містить '7': {count}");
        }

        using (var context = new AutoDbContext())
        {
            var totalCost = context.Autos
                .Where(a => a.Brand == "BMW")
                .AsEnumerable()
                .Sum(a => a.Price);

            Console.WriteLine($"Загальна вартість усіх машин марки 'BMW': {totalCost}");
        }
    }
}