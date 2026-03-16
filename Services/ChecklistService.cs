using AuditDemo.Data;
using AuditDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditDemo.Services
{
    /// <summary>
    /// Service layer for all checklist and audit session operations.
    /// 
    /// This sits between the Blazor UI components and the database.
    /// Components call methods here instead of touching the DbContext directly.
    /// This separation makes the code easier to test and maintain.
    /// 
    /// Registered as Scoped in Program.cs - a new instance is created per HTTP request.
    /// </summary>
    public class ChecklistService
    {
        // EF Core DbContext injected via constructor dependency injection
        private readonly AppDbContext _db;

        public ChecklistService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns all audit sessions with their checklist items loaded.
        /// Include() tells EF Core to do a SQL JOIN and load the related items.
        /// Without Include(), ChecklistItems would be null (lazy loading is off by default).
        /// </summary>
        public async Task<List<AuditSession>> GetSessionsAsync()
        {
            return await _db.AuditSessions.Include(s => s.ChecklistItems).ToListAsync();
        }

        /// <summary>
        /// Returns a single audit session by ID, with checklist items.
        /// Returns null if not found (hence the nullable return type AuditSession?).
        /// </summary>
        public async Task<AuditSession?> GetSessionAsync(int id)
        {
            return await _db.AuditSessions.Include(s => s.ChecklistItems).FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Creates a new audit session with a standard set of checklist items.
        /// Every new session gets the same 11 items - this represents a standard SMSF audit template.
        /// EF Core automatically inserts the child ChecklistItems when we add the parent session.
        /// </summary>
        public async Task<AuditSession> CreateSessionAsync(string fundName, string financialYear)
        {
            var session = new AuditSession
            {
                FundName = fundName,
                FinancialYear = financialYear,
                CreatedAt = DateTime.Now,
                ChecklistItems = new List<ChecklistItem>
                {
                    new() { Category = "Member Information", Description = "Verify member details and TFNs" },
                    new() { Category = "Member Information", Description = "Confirm member contributions within caps" },
                    new() { Category = "Member Information", Description = "Check pension payment minimums met" },
                    new() { Category = "Financial Statements", Description = "Review financial statements for accuracy" },
                    new() { Category = "Financial Statements", Description = "Verify opening balances match prior year" },
                    new() { Category = "Financial Statements", Description = "Confirm investment income correctly recorded" },
                    new() { Category = "Investments", Description = "Confirm investments held in fund's name" },
                    new() { Category = "Investments", Description = "Check sole purpose test compliance" },
                    new() { Category = "Investments", Description = "Verify no in-house assets exceed 5% limit" },
                    new() { Category = "Compliance", Description = "Confirm trust deed is up to date" },
                    new() { Category = "Compliance", Description = "Check audit report lodged within deadlines" },
                }
            };

            _db.AuditSessions.Add(session);
            await _db.SaveChangesAsync(); // Commits the INSERT to the database
            return session;
        }

        /// <summary>
        /// Toggles the completion status of a checklist item.
        /// FindAsync() looks up by primary key - fastest way to find a single record.
        /// SaveChangesAsync() commits the UPDATE to the database.
        /// </summary>
        public async Task ToggleItemAsync(int itemId)
        {
            var item = await _db.ChecklistItems.FindAsync(itemId);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the notes for a completed checklist item.
        /// Called when the auditor types in the notes field after completing an item.
        /// </summary>
        public async Task UpdateNotesAsync(int itemId, string notes)
        {
            var item = await _db.ChecklistItems.FindAsync(itemId);
            if (item != null)
            {
                item.Notes = notes;
                await _db.SaveChangesAsync();
            }
        }
    }
}
