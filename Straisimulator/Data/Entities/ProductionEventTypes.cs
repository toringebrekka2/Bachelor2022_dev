using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Straisimulator.Data.Entities;

public class ProductionEventTypes
{
    [Key]
    public int EventType { get; set; }
    public string DescriptionE { get; set; }
}