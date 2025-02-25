using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;


namespace DataAccessLayer.Data
{
     public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clıents { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Invoice> Invoıces { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InvoiceDetail>()
                .Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InvoiceDetail>()
                .Property(i => i.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InvoiceDetail>()
    .Property(i => i.TotalPrice)
    .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }






    }
}
