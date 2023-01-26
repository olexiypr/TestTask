using LegiosoftTestTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegiosoftTestTask.DataContext;

public class ApplicationEfContext : DbContext
{
    public ApplicationEfContext(DbContextOptions<ApplicationEfContext> options) : base(options)
    {
        
    }

    public ApplicationEfContext()
    {
        
    }
    public DbSet<Transaction> Transactions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
    }
}