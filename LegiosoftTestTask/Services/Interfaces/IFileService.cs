using LegiosoftTestTask.Models;

namespace LegiosoftTestTask.Services.Interfaces;

public interface IFileService
{
    Task<MemoryStream> ExportAsync(FilterModel? filterModel);
    Task<bool> ImportAsync(IFormFile file);
}