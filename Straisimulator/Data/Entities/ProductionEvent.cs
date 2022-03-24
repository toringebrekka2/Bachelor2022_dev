using System.ComponentModel.DataAnnotations;

namespace Straisimulator.Data.Entities;

public class ProductionEvent
{
    [Key]
    public int Id { get; set; }
    public Production ProductionId { get; set; }
    public DateTime TimeStamp { get; set; }
    public ProductionEventTypes EventType { get; set; }
}