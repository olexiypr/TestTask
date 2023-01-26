using System.Data;
using System.Globalization;
using LegiosoftTestTask.Entities;
using LegiosoftTestTask.Entities.Enums;
using LegiosoftTestTask.Models;
using Type = LegiosoftTestTask.Entities.Enums.Type;

namespace LegiosoftTestTask.Extension;

public static class TransactionExtension
{
    public static Transaction ToTransaction(this string line)
    {
        var arr = line.Split(",");
        var transaction = new Transaction
        {
            Id = int.Parse(arr[0]),
            Status = Enum.Parse<Status>(arr[1]),
            Type = Enum.Parse<Type>(arr[2]),
            ClientName = arr[3]
        };
        var amount = decimal.Parse(arr[4].Replace(".", ""), NumberStyles.AllowCurrencySymbol);
        transaction.Amount = amount/100;
        return transaction;
    }

    public static string ToCsvFileString(this string line)
    {
        var arr = line.Split(",");
        arr[1] = Enum.Parse<Status>(arr[1]).ToString();
        arr[2] = Enum.Parse<Type>(arr[2]).ToString();
        return string.Join(",", arr);
    }
    public static TransactionModel ToTransactionModel(this DataRow row)
    {
        var transaction = new TransactionModel()
        {
            Id = (int) row[0],
            Status = ((Status)row[1]).ToString(),
            Type = ((Type)row[2]).ToString(),
            ClientName = (row[3].ToString()),
            Amount = decimal.Parse(row[4].ToString())
        };
        return transaction;
    }

    public static IEnumerable<TransactionModel> ToTransactionModelEnumerable(this DataTable dataTable)
    {
        return (from DataRow row in dataTable.Rows select row.ToTransactionModel()).ToList();
    }

    public static TransactionModel ToTransactionModel(this Transaction transaction)
    {
        var transactionModel = new TransactionModel()
        {
            Id = transaction.Id,
            Status = transaction.Status.ToString(),
            Type = transaction.Type.ToString(),
            ClientName = transaction.ClientName,
            Amount = transaction.Amount
        };
        return transactionModel;
    }
}