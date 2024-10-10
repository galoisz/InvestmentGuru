﻿using Microsoft.EntityFrameworkCore;
using StockLibrary.Data.Entities;

namespace StockLibrary.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>().ToTable("stocks");
    }
}