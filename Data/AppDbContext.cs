using AuditDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuditDemo.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<AuditSession> AuditSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditSession>().HasData(
                new AuditSession { Id = 1, FundName = "Smith Family SMSF", FinancialYear = "2024-2025", CreatedAt = new DateTime(2025, 1, 1) },
                new AuditSession { Id = 2, FundName = "Johnson Retirement Fund", FinancialYear = "2024-2025", CreatedAt = new DateTime(2025, 1, 15) }
            );

            modelBuilder.Entity<ChecklistItem>().HasData(
                new ChecklistItem { Id = 1, AuditSessionId = 1, Category = "Member Information", Description = "Verify member details and TFNs", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 2, AuditSessionId = 1, Category = "Member Information", Description = "Confirm member contributions within caps", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 3, AuditSessionId = 1, Category = "Member Information", Description = "Check pension payment minimums met", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 4, AuditSessionId = 1, Category = "Financial Statements", Description = "Review financial statements for accuracy", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 5, AuditSessionId = 1, Category = "Financial Statements", Description = "Verify opening balances match prior year", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 6, AuditSessionId = 1, Category = "Financial Statements", Description = "Confirm investment income correctly recorded", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 7, AuditSessionId = 1, Category = "Investments", Description = "Confirm investments held in fund's name", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 8, AuditSessionId = 1, Category = "Investments", Description = "Check sole purpose test compliance", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 9, AuditSessionId = 1, Category = "Investments", Description = "Verify no in-house assets exceed 5% limit", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 10, AuditSessionId = 1, Category = "Compliance", Description = "Confirm trust deed is up to date", IsCompleted = false, Notes = "" },
                new ChecklistItem { Id = 11, AuditSessionId = 1, Category = "Compliance", Description = "Check audit report lodged within deadlines", IsCompleted = false, Notes = "" }
            );
        }
    }
}
