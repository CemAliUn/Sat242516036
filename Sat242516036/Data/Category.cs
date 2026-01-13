using System.ComponentModel.DataAnnotations;

namespace Sat242516036.Data;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    public string CategoryName { get; set; } = "";

    public string? Description { get; set; }

    // Generic yapıdaki TotalRecordCount eşleşmesi için
    public int TotalRecordCount { get; set; }
}