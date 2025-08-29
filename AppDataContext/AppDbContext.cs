// AppDataContext/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using LotusAscend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<PointTransaction> PointTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one-to-one relationship between Member and Otp
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Otp)
            .WithOne(o => o.Member)
            .HasForeignKey<Otp>(o => o.MemberId);
    }
}