using LegiosoftTestTask.Entities.Enums;
using LegiosoftTestTask.Models;

namespace LegiosoftTestTask.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionModel>> GetAllTransactionsAsync(FilterModel? filterModel);
    Task<TransactionModel> GetTransactionByIdAsync(int id);
    Task<TransactionModel> UpdateStatusAsync(int id, Status statusToUpdate);
}