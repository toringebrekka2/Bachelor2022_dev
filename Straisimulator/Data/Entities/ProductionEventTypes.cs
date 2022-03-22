using Microsoft.EntityFrameworkCore;

namespace Straisimulator.Data.Entities;

[Keyless]
public class ProductionEventTypes
{
    public int EventType { get; set; }
    public string DescriptionE { get; set; }
}