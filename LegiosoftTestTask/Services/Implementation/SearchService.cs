using System.Data;
using LegiosoftTestTask.DataContext;
using LegiosoftTestTask.Extension;
using LegiosoftTestTask.Models;
using LegiosoftTestTask.Services.Interfaces;
using Npgsql;
using NpgsqlTypes;

namespace LegiosoftTestTask.Services.Implementation;

public class SearchService : ISearchService
{
    private readonly ApplicationPostgresContext _context;

    public SearchService(ApplicationPostgresContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransactionModel>> SearchAsync(string searchLine)
    {
        await using var command = new NpgsqlCommand(
            "SELECT * FROM \"Transactions\" WHERE lower(\"ClientName\") LIKE lower(@1)",_context.Connection);
        searchLine = "%" + searchLine + "%";
        command.Parameters.Add("@1", NpgsqlDbType.Varchar).Value = searchLine;
        var dataTable = new DataTable();
        var dataAdapter = new NpgsqlDataAdapter();
        dataAdapter.SelectCommand = command;
        await _context.OpenConnectionAsync();
        try
        {
            dataAdapter.Fill(dataTable);
        }
        finally
        {
            await _context.CloseConnectionAsync();
        }

        return dataTable.ToTransactionModelEnumerable();
    }
}