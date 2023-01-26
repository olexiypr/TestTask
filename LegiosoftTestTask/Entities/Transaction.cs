using LegiosoftTestTask.Entities.Enums;
using Type = LegiosoftTestTask.Entities.Enums.Type;

namespace LegiosoftTestTask.Entities;

public class Transaction
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public Type Type { get; set; }
    public string ClientName { get; set; } = String.Empty;
    public decimal Amount { get; set; }
}