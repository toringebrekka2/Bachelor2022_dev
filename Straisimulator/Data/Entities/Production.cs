using System.ComponentModel.DataAnnotations;

namespace Straisimulator.Data.Entities;

public class Production
{
    [Key]
    public int ProductionId { get; set; }
    public int ProductionSequence { get; set; }
    public DateTime MESProductionDate;
    public string OrderNumber { get; set; }
    public string Comment { get; set; }

    public ICollection<ProductionEvent> ProductionEvents { get; set; }
}