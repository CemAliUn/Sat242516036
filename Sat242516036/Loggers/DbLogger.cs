using Sat242516036.Data;

namespace Sat242516036.Loggers;

public class DbLogger(ApplicationDbContext context)
{
    public async Task LogAsync(string tableName, string operation, string recordId, string details)
    {
        try
        {
            var log = new LogEntry
            {
                TableName = tableName,
                OperationType = operation,
                RecordId = recordId,
                Details = details,
                LogDate = DateTime.Now
            };

            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
        catch { }
    }
}