using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sat242516036.Data;

[Table("Logs_Table")] // SQL'deki tablo adıyla eşleşiyor
public class LogEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string TableName { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string OperationType { get; set; } = "";

    [MaxLength(50)]
    public string? RecordId { get; set; }

    public DateTime LogDate { get; set; } = DateTime.Now;

    public string? Details { get; set; }
}