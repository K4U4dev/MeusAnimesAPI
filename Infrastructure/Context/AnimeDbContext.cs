using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Configurations;

namespace Infrastructure.Context;

public partial class AnimeDbContext : DbContext
{
    public AnimeDbContext()
    {
    }

    public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Animes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimeConfiguration).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
