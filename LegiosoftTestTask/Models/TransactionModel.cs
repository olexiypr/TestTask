namespace LegiosoftTestTask.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string ClientName { get; set; }
    public decimal Amount { get; set; }
}