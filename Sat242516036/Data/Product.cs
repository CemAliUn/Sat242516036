using System.ComponentModel.DataAnnotations;

namespace Sat242516036.Data;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    public string ProductName { get; set; } = "";

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Puan en az 1 olmalıdır.")]
    public int PointCost { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stok negatif olamaz.")]
    public int StockQuantity { get; set; }

    // Listeleme için toplam kayıt sayısı (SP'den gelecek)
    public int TotalRecordCount { get; set; }
}