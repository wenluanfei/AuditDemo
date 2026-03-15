namespace AuditDemo.Models
{
    public class AuditSession
    {
        public int Id { get; set; }
        public string FundName { get; set; } = "";
        public string FinancialYear { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<ChecklistItem> ChecklistItems { get; set; } = new();
    }
}
