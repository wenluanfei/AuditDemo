namespace AuditDemo.Models
{
    /// <summary>
    /// Represents a single SMSF audit engagement.
    /// One session = one fund being audited for one financial year.
    /// Contains a list of ChecklistItems that the auditor works through.
    /// </summary>
    public class AuditSession
    {
        /// <summary>Primary key</summary>
        public int Id { get; set; }

        /// <summary>Name of the SMSF being audited e.g. "Smith Family SMSF"</summary>
        public string FundName { get; set; } = "";

        /// <summary>Financial year of the audit e.g. "2024-2025"</summary>
        public string FinancialYear { get; set; } = "";

        /// <summary>Date the audit session was created in the system</summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Navigation property - EF Core loads all checklist items for this session.
        /// Initialized as empty list to avoid null reference exceptions.
        /// </summary>
        public List<ChecklistItem> ChecklistItems { get; set; } = new();
    }
}
