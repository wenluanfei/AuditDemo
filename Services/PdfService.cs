using AuditDemo.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AuditDemo.Services
{
    public class PdfService
    {
        public byte[] GenerateAuditReport(AuditSession session)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("SMSF Audit Report").FontSize(24).Bold();
                        col.Item().Text(session.FundName).FontSize(16).FontColor("#444444");
                        col.Item().Text($"Financial Year: {session.FinancialYear}").FontColor("#666666");
                        col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy}").FontColor("#666666");
                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor("#cccccc");
                    });

                    page.Content().PaddingTop(20).Column(col =>
                    {
                        var completed = session.ChecklistItems.Count(i => i.IsCompleted);
                        var total = session.ChecklistItems.Count;
                        var percent = total > 0 ? (completed * 100 / total) : 0;

                        col.Item().Background("#f8f9fa").Padding(10).Column(summary =>
                        {
                            summary.Item().Text("Summary").Bold().FontSize(13);
                            summary.Item().Text($"Total Items: {total}");
                            summary.Item().Text($"Completed: {completed}");
                            summary.Item().Text($"Completion Rate: {percent}%");
                        });

                        col.Item().PaddingTop(20);

                        foreach (var category in session.ChecklistItems.Select(i => i.Category).Distinct())
                        {
                            col.Item().PaddingTop(10).Text(category).Bold().FontSize(13);

                            var items = session.ChecklistItems.Where(i => i.Category == category).ToList();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(20);
                                    cols.RelativeColumn(3);
                                    cols.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background("#e9ecef").Padding(5).Text("#").Bold();
                                    header.Cell().Background("#e9ecef").Padding(5).Text("Description").Bold();
                                    header.Cell().Background("#e9ecef").Padding(5).Text("Status").Bold();
                                });

                                foreach (var item in items)
                                {
                                    table.Cell().BorderBottom(1).BorderColor("#dee2e6").Padding(5).Text(item.Id.ToString());
                                    table.Cell().BorderBottom(1).BorderColor("#dee2e6").Padding(5).Text(item.Description);
                                    table.Cell().BorderBottom(1).BorderColor("#dee2e6").Padding(5)
                                        .Text(item.IsCompleted ? "Completed" : "Pending")
                                        .FontColor(item.IsCompleted ? "#198754" : "#dc3545");
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
