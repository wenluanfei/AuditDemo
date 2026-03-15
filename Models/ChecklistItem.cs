namespace AuditDemo.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public int AuditSessionId { get; set; }
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsCompleted { get; set; } = false;
        public string Notes { get; set; } = "";
        public AuditSession? AuditSession { get; set; }
    }
}
