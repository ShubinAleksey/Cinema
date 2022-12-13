using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinema.Models;

namespace Cinema.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountingReport> AccountingReport { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<MovieSession> MovieSession { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PurchaseReport> PurchaseReport { get; set; }
        public virtual DbSet<StockReport> StockReport { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountingReport>(entity =>
            {
                entity.ToTable("AccountingReport");

                entity.HasIndex(e => e.EmployeeId, "IX_AccountingReport_EmployeeId");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");
            });

            modelBuilder.Entity<MovieSession>(entity =>
            {
                entity.ToTable("MovieSession");

                entity.Property(e => e.SessionTime).HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.BuyerId, "IX_Order_BuyerId");

                entity.HasIndex(e => e.TicketId, "IX_Order_TicketId");

                entity.HasOne(d => d.Ticket).WithMany(p => p.Orders).HasForeignKey(d => d.TicketId);

                entity.Property(e => e.IsConfirmed)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsRejected)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<PurchaseReport>(entity =>
            {
                entity.ToTable("PurchaseReport");

                entity.HasIndex(e => e.DepartmentId, "IX_PurchaseReport_DepartmentId");

                entity.HasOne(d => d.Department).WithMany(p => p.PurchaseReports).HasForeignKey(d => d.DepartmentId);
            });

            modelBuilder.Entity<StockReport>(entity =>
            {
                entity.ToTable("StockReport");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.HasIndex(e => e.SessionId, "IX_Ticket_SessionId");

                entity.HasOne(d => d.Session).WithMany(p => p.Tickets).HasForeignKey(d => d.SessionId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}