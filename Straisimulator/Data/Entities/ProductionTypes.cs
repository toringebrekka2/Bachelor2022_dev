using System.ComponentModel.DataAnnotations;

namespace Straisimulator.Data.Entities;

public class ProductionTypes
{
    [Key]
    public int ProductionType { get; set; }

    public string Description { get; set; }
    public ICollection<ProductionEventLog> ProductionEvents { get; set; }
}