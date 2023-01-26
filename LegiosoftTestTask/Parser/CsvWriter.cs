using LegiosoftTestTask.DataContext;
using LegiosoftTestTask.Extension;
using LegiosoftTestTask.Models;

namespace LegiosoftTestTask.Parser;

public class CsvWriter
{
    public async Task WriteToFileFromStdin(ApplicationPostgresContext postgresContext,
        FilterModel? filterModel, TextWriter writer)
    {
        string? line;
        while ((line = await postgresContext.ReadFromStdinAsync()) != null)
        {
            if (Filtrate(filterModel, line))
            {
                await writer.WriteLineAsync(line.ToCsvFileString());
            }
        }
    }

    private bool Filtrate(FilterModel? filterModel, string line)
    {
        if (filterModel is null || (filterModel.Status is null && filterModel.AllowedTypes is null))
        {
            return true;
        }

        var transaction = line.ToTransaction();
        if (filterModel.Status! == transaction.Status && filterModel.AllowedTypes!.Contains(transaction.Type))
        {
            return true;
        }

        return false;
    }
}