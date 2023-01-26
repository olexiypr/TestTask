using System.Data;
using Npgsql;

namespace LegiosoftTestTask.DataContext;

public class ApplicationPostgresContext : IDisposable
{
    public NpgsqlConnection Connection { get; }
    private TextReader _reader;

    public ApplicationPostgresContext()
    {
        Connection = new NpgsqlConnection("Host=localhost;Username=aloshaprokopenko5;Password=787898;Database=legiosoft_test");
    }

    public async Task OpenConnectionAsync()
    {
        if (Connection.State == ConnectionState.Closed)
        {
            await Connection.OpenAsync();
        }
    }

    public async Task CloseConnectionAsync()
    {
        if (Connection.State != ConnectionState.Closed)
        {
            await Connection.CloseAsync();
        }
    }

    public async Task<string?> ReadFromStdinAsync()
    {
        return await _reader.ReadLineAsync();
    }
    public async Task StartReadFromStdinAsync()
    {
        await Connection.OpenAsync();
        _reader = await Connection.BeginTextExportAsync("COPY \"Transactions\" TO STDOUT DELIMITER ','");
    }

    public async Task EndReadFromStdinAsync()
    {
        _reader.Close();
        await CloseConnectionAsync();
    }

    public void Dispose()
    {
        if (_reader != null)
        {
            _reader.Close();
            _reader.Dispose();
        }

        Connection.Close();
        Connection.Dispose();
    }

    public async Task AddTriggerAsync()
    {
        try
        {
            await OpenConnectionAsync();
            await ExecuteAddFunctionCommandAsync();
            await ExecuteAddTriggerCommandAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            await CloseConnectionAsync();
        }
    }

    private async Task ExecuteAddTriggerCommandAsync()
    {
        var triggerText = @"CREATE TRIGGER on_insert BEFORE INSERT ON ""Transactions"" FOR EACH ROW EXECUTE FUNCTION delete();";
        await using var triggerCommand = new NpgsqlCommand(triggerText, Connection);
        await triggerCommand.ExecuteNonQueryAsync();
    }
    private async Task ExecuteAddFunctionCommandAsync()
    {
        var functionText = @"CREATE OR REPLACE FUNCTION delete() RETURNS TRIGGER 
                        AS 
                        $$ 
                        begin 
                            DELETE FROM ""Transactions"" WHERE ""Id"" = new.""Id"";
                            RETURN new;
                        end;
                        $$
                        LANGUAGE plpgsql";
        await using var functionCommand = new NpgsqlCommand(functionText, Connection);
        await functionCommand.ExecuteNonQueryAsync();
    }
}