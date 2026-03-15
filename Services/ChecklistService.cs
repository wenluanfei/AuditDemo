using AuditDemo.Data;
using AuditDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditDemo.Services
{
    public class ChecklistService
    {
        private readonly AppDbContext _db;

        public ChecklistService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<AuditSession>> GetSessionsAsync()
        {
            return await _db.AuditSessions.Include(s => s.ChecklistItems).ToListAsync();
        }

        public async Task<AuditSession?> GetSessionAsync(int id)
        {
            return await _db.AuditSessions.Include(s => s.ChecklistItems).FirstOrDefaultAsync(s => s.Id == id);
        }

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
            await _db.SaveChangesAsync();
            return session;
        }

        public async Task ToggleItemAsync(int itemId)
        {
            var item = await _db.ChecklistItems.FindAsync(itemId);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                await _db.SaveChangesAsync();
            }
        }

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
