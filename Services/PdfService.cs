using AuditDemo.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AuditDemo.Services
{
    /// <summary>
    /// Generates PDF audit reports using the QuestPDF library.
    /// QuestPDF uses a fluent builder API - you describe the document structure
    /// in C# code rather than using templates or HTML.
    /// 
    /// Registered as Scoped in Program.cs.
    /// </summary>
    public class PdfService
    {
        /// <summary>
        /// Generates a PDF report for a completed audit session.
        /// Returns the PDF as a byte array which the Blazor component
        /// converts to base64 and triggers a browser download via JavaScript.
        /// </summary>
        public byte[] GenerateAuditReport(AuditSession session)
        {
            // Required for community/free use of QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // Header section - displayed at top of every page
                    page.Header().Column(col =>
                    {
                        col.Item().Text("SMSF Audit Report").FontSize(24).Bold();
                        col.Item().Text(session.FundName).FontSize(16).FontColor("#444444");
                        col.Item().Text($"Financial Year: {session.FinancialYear}").FontColor("#666666");
                        col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy}").FontColor("#666666");
                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor("#cccccc");
                    });

                    // Main content area
                    page.Content().PaddingTop(20).Column(col =>
                    {
                        // Calculate summary stats
                        var completed = session.ChecklistItems.Count(i => i.IsCompleted);
                        var total = session.ChecklistItems.Count;
                        var percent = total > 0 ? (completed * 100 / total) : 0;

                        // Summary box
                        col.Item().Background("#f8f9fa").Padding(10).Column(summary =>
                        {
                            summary.Item().Text("Summary").Bold().FontSize(13);
                            summary.Item().Text($"Total Items: {total}");
                            summary.Item().Text($"Completed: {completed}");
                            summary.Item().Text($"Completion Rate: {percent}%");
                        });

                        col.Item().PaddingTop(20);

                        // Render a table for each category group
                        foreach (var category in session.ChecklistItems.Select(i => i.Category).Distinct())
                        {
                            col.Item().PaddingTop(10).Text(category).Bold().FontSize(13);

                            var items = session.ChecklistItems.Where(i => i.Category == category).ToList();

                            // QuestPDF table definition
                            col.Item().Table(table =>
                            {
                                // Define column widths
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(20);  // ID column
                                    cols.RelativeColumn(3);   // Description (wider)
                                    cols.RelativeColumn(2);   // Status
                                });

                                // Table header row
                                table.Header(header =>
                                {
                                    header.Cell().Background("#e9ecef").Padding(5).Text("#").Bold();
                                    header.Cell().Background("#e9ecef").Padding(5).Text("Description").Bold();
                                    header.Cell().Background("#e9ecef").Padding(5).Text("Status").Bold();
                                });

                                // Data rows
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

                    // Footer with page numbers
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            // Generate and return the PDF as bytes
            return document.GeneratePdf();
        }
    }
}
