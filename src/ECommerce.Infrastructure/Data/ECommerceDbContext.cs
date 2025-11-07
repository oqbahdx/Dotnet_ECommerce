using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Adress => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Email).IsRequired().HasMaxLength(256);
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.FirstName).HasMaxLength(100);
            b.Property(x => x.LastName).HasMaxLength(100);
        });
        modelBuilder.Entity<Order>(b =>
        {
            b.Property(x => x.TotalAmount).HasPrecision(18, 2);
            b.Property(x => x.Discount).HasPrecision(18, 2);
            b.Property(x => x.ShippingCost).HasPrecision(18, 2);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.Property(x => x.UnitPrice).HasPrecision(18, 2);
        });
    }
}


