namespace Straisimulator.Models;

public class SkapVegg
{
    public int Id { get; set; }
    public String Name { get; set; }
    public int OpTimeEvert1 { get; set; }
    public int OpTimeEvert2 { get; set; }
    public int CyTimeEvert1 { get; set; }
    public int CyTimeEvert2 { get; set; }
    public SkapVegg(int id, String name, int opTimeEvert1, int opTimeEvert2, int cyTimeEvert1, int cyTimeEvert2)
    {
        Id = id;
        Name = name;
        OpTimeEvert1 = opTimeEvert1;
        OpTimeEvert2 = opTimeEvert2;
        CyTimeEvert1 = cyTimeEvert1;
        CyTimeEvert2 = cyTimeEvert2;
    }
}

