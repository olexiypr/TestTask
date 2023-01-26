using LegiosoftTestTask.DataContext;
using LegiosoftTestTask.Models;
using LegiosoftTestTask.Parser;
using LegiosoftTestTask.Services.Interfaces;

namespace LegiosoftTestTask.Services.Implementation;

public class FileService : IFileService
{
    private readonly ApplicationEfContext _context;
    private readonly ApplicationPostgresContext _postgresContext;

    public FileService(ApplicationEfContext context, ApplicationPostgresContext postgresContext)
    {
        _context = context;
        _postgresContext = postgresContext;
    }

    public async Task<MemoryStream> ExportAsync(FilterModel? filterModel)
    {
       var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter();
        await _postgresContext.StartReadFromStdinAsync();
        await csvWriter.WriteToFileFromStdin(_postgresContext, filterModel, writer);
        await _postgresContext.EndReadFromStdinAsync();
        await writer.FlushAsync();
        memoryStream.Position = 0;
        return memoryStream;
    }
    public async Task<bool> ImportAsync(IFormFile file)
    {
        ValidateFile(file);
        using var parser = new ExcelParser();
        var rows = await parser.GetRowsAsync(file);
        foreach (var row in rows)
        {
            var transaction = parser.GetTransaction(row);
            if (transaction != null)
            {
                _context.Add(transaction);
            }
        }
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return true;
    }
    private void ValidateFile(IFormFile file)
    {
        if (file.Length == 0)
        {
            throw new ArgumentException("File is empty!");
        }
        var fileInfo = new FileInfo(file.FileName);
        if (fileInfo.Extension is not (".xlsx" or ".xlsm" or ".xlsb" or ".xls"))
        {
            throw new ArgumentException($"Invalid file extension! {fileInfo.Extension}");
        }
    }
}