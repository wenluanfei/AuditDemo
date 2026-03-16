using AuditDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuditDemo.Data
{
    /// <summary>
    /// The main database context for the application.
    /// Inherits from IdentityDbContext to include ASP.NET Identity tables
    /// (AspNetUsers, AspNetRoles etc.) alongside our custom tables.
    /// 
    /// EF Core uses this class to:
    /// 1. Map C# models to database tables
    /// 2. Track changes to entities
    /// 3. Generate and run migrations
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // Constructor passes options (connection string etc.) to the base DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>Represents the ChecklistItems table in the database</summary>
        public DbSet<ChecklistItem> ChecklistItems { get; set; }

        /// <summary>Represents the AuditSessions table in the database</summary>
        public DbSet<AuditSession> AuditSessions { get; set; }

        /// <summary>
        /// Called by EF Core when building the database schema.
        /// We use this to seed initial demo data so the app has something to show on first run.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must call base first - sets up Identity tables
            base.OnModelCreating(modelBuilder);

            // Seed two demo audit sessions
            modelBuilder.Entity<AuditSession>().HasData(
                new AuditSession { Id = 1, FundName = "Smith Family SMSF", FinancialYear = "2024-2025", CreatedAt = new DateTime(2025, 1, 1) },
                new AuditSession { Id = 2, FundName = "Johnson Retirement Fund", FinancialYear = "2024-2025", CreatedAt = new DateTime(2025, 1, 15) }
            );

            // Seed checklist items for session 1
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
