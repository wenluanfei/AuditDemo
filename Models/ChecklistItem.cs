namespace AuditDemo.Models
{
    /// <summary>
    /// Represents a single audit checklist item within an audit session.
    /// Each item belongs to a category and tracks completion status and notes.
    /// </summary>
    public class ChecklistItem
    {
        /// <summary>Primary key</summary>
        public int Id { get; set; }

        /// <summary>Foreign key linking this item to its parent AuditSession</summary>
        public int AuditSessionId { get; set; }

        /// <summary>Grouping label e.g. "Member Information", "Investments"</summary>
        public string Category { get; set; } = "";

        /// <summary>The audit task description shown to the auditor</summary>
        public string Description { get; set; } = "";

        /// <summary>Whether the auditor has marked this item as done</summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>Optional auditor notes added when item is completed</summary>
        public string Notes { get; set; } = "";

        /// <summary>Navigation property - EF Core uses this to load the parent session</summary>
        public AuditSession? AuditSession { get; set; }
    }
}
