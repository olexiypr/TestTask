using System.Data;
using LegiosoftTestTask.DataContext;
using LegiosoftTestTask.Entities;
using LegiosoftTestTask.Entities.Enums;
using LegiosoftTestTask.Extension;
using LegiosoftTestTask.Models;
using LegiosoftTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace LegiosoftTestTask.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ApplicationEfContext _dbContext;
    private readonly ApplicationPostgresContext _postgresContext;

    public TransactionService(ApplicationEfContext dbContext, ApplicationPostgresContext postgresContext)
    {
        _dbContext = dbContext;
        _postgresContext = postgresContext;
    }

    public async Task<IEnumerable<TransactionModel>> GetAllTransactionsAsync(FilterModel? filterModel)
    {
        var connection = _postgresContext.Connection;
        var commandText = "SELECT * FROM \"Transactions\"";
        if (filterModel is not null)
        {
            commandText = GetQueryByFilter(filterModel, commandText);
        }
        await using var command = new NpgsqlCommand(commandText, connection);
        var dataTable = new DataTable();
        var dataAdapter = new NpgsqlDataAdapter();
        dataAdapter.SelectCommand = command;
        await _postgresContext.OpenConnectionAsync();
        try
        {
            dataAdapter.Fill(dataTable);
        }
        finally
        {
            await _postgresContext.CloseConnectionAsync();
        }
        return dataTable.ToTransactionModelEnumerable();
    }

    private string GetQueryByFilter(FilterModel? filterModel, string defaultCommandText)
    {
        if (filterModel.AllowedTypes is not null)
        {
            var allowedTypes = string.Join(", ",filterModel.AllowedTypes.Select(type => (int)type));
            defaultCommandText += " WHERE \"Type\" IN (" + allowedTypes + ")";
        }

        if (filterModel.Status is not null)
        {
            var allowedStatus = (int) filterModel.Status;
            defaultCommandText += " AND \"Status\" = " + allowedStatus;
        }
        return defaultCommandText;
    }
    public async Task<TransactionModel> GetTransactionByIdAsync(int id)
    {
        var transaction = await GetTransactionByIdFromDbAsync(id);
        return transaction.ToTransactionModel();
    }

    private async Task<Transaction> GetTransactionByIdFromDbAsync(int id)
    {
        var transaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(transaction => transaction.Id == id);
        if (transaction == null)
        {
            throw new ArgumentException("Transaction with this id not found!");
        }

        return transaction;
    }
    public async Task<TransactionModel> UpdateStatusAsync(int id, Status statusToUpdate)
    {
        if (!Enum.GetValues<Status>().Contains(statusToUpdate))
        {
            throw new ArgumentException("Incorrect status!");
        }
        var transaction = await GetTransactionByIdFromDbAsync(id);
        transaction.Status = statusToUpdate;
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _dbContext.Database.RollbackTransactionAsync();
            throw;
        }

        return transaction.ToTransactionModel();
    }
}