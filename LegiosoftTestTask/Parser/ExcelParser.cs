using System.Text;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LegiosoftTestTask.Entities;
using LegiosoftTestTask.Entities.Enums;
using LegiosoftTestTask.Extension;
using Type = LegiosoftTestTask.Entities.Enums.Type;

namespace LegiosoftTestTask.Parser;

public class ExcelParser : IDisposable
{
    private SpreadsheetDocument _spreadsheetDocument;
    private MemoryStream _memoryStream;
    public async Task<IEnumerable<Row>> GetRowsAsync(IFormFile file)
    {
        _memoryStream = new MemoryStream();
        await file.CopyToAsync(_memoryStream);
        _spreadsheetDocument = SpreadsheetDocument.Open(_memoryStream, false);
        var sheet = _spreadsheetDocument.WorkbookPart?.Workbook.Sheets?.GetFirstChild<Sheet>();
        var worksheetPart = (_spreadsheetDocument.WorkbookPart?.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
        var rows = worksheetPart.GetFirstChild<SheetData>()?.Descendants<Row>();
        return rows;
    }
    private string GetCellValue(Cell cell)
    {
        string value = cell.CellValue?.InnerText;
        if (cell.DataType is {Value: CellValues.SharedString})
        {
            return _spreadsheetDocument.WorkbookPart?.SharedStringTablePart?.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
        }
        return value;
    }

    public Transaction? GetTransaction(Row row)
    {
        var builder = new StringBuilder();
        foreach (var cell in row.Elements<Cell>())
        {
            builder.Append(GetCellValue(cell) + ",");
        }
        var line = builder.ToString();
        if (!IsFileHeader(line))
        {
            return null;
        }
        line = line.Replace("$", "").Substring(0, line.Length-2);
        return line.ToTransaction();
    }

    private bool IsFileHeader(string transaction)
    {
        return int.TryParse(transaction.Split(",")[0], out var id);
    }
    public void Dispose()
    {
        _memoryStream.Dispose();
        _spreadsheetDocument.Dispose();
    }
}