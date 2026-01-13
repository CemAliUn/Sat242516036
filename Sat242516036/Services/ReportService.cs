using QuestPDF.Fluent;
using QuestPDF.Helpers; // Bu namespace ŞART (Centimeter için)
using QuestPDF.Infrastructure;
using Sat242516036.Data;

namespace Sat242516036.Services;

public class ReportService
{
    // 1. ÜRÜN RAPORU
    public byte[] GenerateProductReport(List<Product> products)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                // DÜZELTME: Extension Method kullanımı (2.Centimeter())
                page.Margin(2, Unit.Centimetre); 
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Ürün Listesi Raporu")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50); 
                            columns.RelativeColumn();   
                            columns.ConstantColumn(80); 
                            columns.ConstantColumn(80); 
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID");
                            header.Cell().Element(CellStyle).Text("Ürün Adı");
                            header.Cell().Element(CellStyle).Text("Puan");
                            header.Cell().Element(CellStyle).Text("Stok");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            }
                        });

                        foreach (var item in products)
                        {
                            table.Cell().Element(CellStyle).Text(item.Id.ToString());
                            table.Cell().Element(CellStyle).Text(item.ProductName);
                            table.Cell().Element(CellStyle).Text(item.PointCost.ToString());
                            table.Cell().Element(CellStyle).Text(item.StockQuantity.ToString());

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    // 2. KATEGORİ RAPORU
    public byte[] GenerateCategoryReport(List<Category> categories)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                
                page.Header()
                    .Text("Kategori Listesi Raporu")
                    .SemiBold().FontSize(24).FontColor(Colors.Red.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("ID").SemiBold();
                            header.Cell().Text("Kategori Adı").SemiBold();
                            header.Cell().Text("Açıklama").SemiBold();
                        });

                        foreach (var item in categories)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.CategoryName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Description ?? "-");
                        }
                    });

                page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
            });
        });

        return document.GeneratePdf();
    }
}