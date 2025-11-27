using DeveloperAgent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperAgent.Infrastructure.Data
{
    public class DeveloperAgentDbContext : DbContext
    {
        public DeveloperAgentDbContext(DbContextOptions<DeveloperAgentDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<AuditTrail> AuditTrails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Username).IsUnique();
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(x => new { x.UserId, x.RoleId });
                b.HasOne(x => x.User).WithMany(u => u.UserRoles).HasForeignKey(x => x.UserId);
                b.HasOne(x => x.Role).WithMany(r => r.UserRoles).HasForeignKey(x => x.RoleId);
            });

            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.User).WithMany(u => u.RefreshTokens).HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<AuditTrail>(b =>
            {
                b.HasKey(x => x.Id);
            });
        }
    }
}
