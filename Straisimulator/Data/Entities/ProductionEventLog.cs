using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Straisimulator.Data.Entities;

public class ProductionEventLog
{
    [Key]
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string ExtraInfo { get; set; }
    public int? ProductionId { get; set; }
    
    [ForeignKey(nameof(ProductionId))]
    public Production Production { get; set; }

    public int ProductionType { get; set; }
    [ForeignKey(nameof(ProductionType))]
    public ProductionTypes ProductionTypes { get; set; }

    public int EventType { get; set; }
    
    [ForeignKey(nameof(EventType))]
    public ProductionEventTypes ProductionEventType { get; set; }
}