using Microsoft.EntityFrameworkCore;
using TransactionIngest.Models;

public class TransactionDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=transactions.db");
    }
}