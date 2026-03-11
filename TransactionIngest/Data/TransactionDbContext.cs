using Microsoft.EntityFrameworkCore;
using TransactionIngest.Models;

public class TransactionDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Transaction> TransactionsChanges { get; set; }

    public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.TransactionId)
            .IsUnique();
    }
}