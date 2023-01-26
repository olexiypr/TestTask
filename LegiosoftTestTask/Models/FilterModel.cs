using LegiosoftTestTask.Entities.Enums;
using Type = LegiosoftTestTask.Entities.Enums.Type;

namespace LegiosoftTestTask.Models;

public class FilterModel
{
    public IEnumerable<Type>? AllowedTypes { get; set; }
    public Status? Status { get; set; }
}