using LegiosoftTestTask.Models;

namespace LegiosoftTestTask.Services.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<TransactionModel>> SearchAsync(string searchLine);
}