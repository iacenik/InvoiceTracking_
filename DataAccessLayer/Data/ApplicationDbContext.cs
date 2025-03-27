using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CashRegister
            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalIncomeEUR)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalExpenseEUR)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalIncomeRON)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalExpenseRON)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalIncomeUSD)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CashRegister>()
                .Property(c => c.TotalExpenseUSD)
                .HasColumnType("decimal(18,2)");

            // Client
            modelBuilder.Entity<Client>()
                .Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Client>()
                .Property(c => c.Country)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Client>()
                .Property(c => c.Address)
                .HasMaxLength(300);

            modelBuilder.Entity<Client>()
                .Property(c => c.BankName)
                .HasMaxLength(100);

            modelBuilder.Entity<Client>()
                .Property(c => c.Iban)
                .HasMaxLength(34);

            // Employee
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeName)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeSurname)
                .IsRequired();

            // Expense
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Employee)
                .WithMany(e => e.Expenses)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExpenseCategory
            modelBuilder.Entity<ExpenseCategory>()
                .Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ExpenseCategory>()
                .Property(c => c.Description)
                .HasMaxLength(300);

            // Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Employee)
                .WithMany()
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // InvoiceDetail
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne(d => d.Invoice)
                .WithMany(i => i.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Item
            modelBuilder.Entity<Item>()
                .Property(i => i.ItemName)
                .IsRequired()
                .HasMaxLength(200);

            // Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentDetail
            modelBuilder.Entity<PaymentDetail>()
                .HasOne(d => d.Payment)
                .WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // CashRegister
            modelBuilder.Entity<CashRegister>().HasData(
                new CashRegister
                {
                    CashRegisterId = 1,
                    TotalIncomeEUR = 0m,
                    TotalExpenseEUR = 0m,
                    TotalIncomeRON = 0m,
                    TotalExpenseRON = 0m,
                    TotalIncomeUSD = 0m,
                    TotalExpenseUSD = 0m
                }
            );
        }
    }
}