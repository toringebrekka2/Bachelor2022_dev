namespace Straisimulator.Models;

public class SkapVegg
{
    public int Id { get; set; }
    public String Description { get; set; }
    public int OpTimeBrema { get; set; }
    public int OpTimeEvert1 { get; set; }
    public int OpTimeEvert2 { get; set; }
    public int OpTimeEvert3 { get; set; }
    public int CyTimeBrema { get; set; }
    public int CyTimeEvert1 { get; set; }
    public int CyTimeEvert2 { get; set; }
    public int CyTimeEvert3 { get; set; }
    public SkapVegg(int id, String description, int opTimeBrema, int opTimeEvert1, int opTimeEvert2,int opTimeEvert3, int cyTimeBrema, int cyTimeEvert1, int cyTimeEvert2, int cyTimeEvert3)
    {
        Id = id;
        Description = description;
        OpTimeBrema = opTimeBrema;
        OpTimeEvert1 = opTimeEvert1;
        OpTimeEvert2 = opTimeEvert2;
        OpTimeEvert3 = opTimeEvert3;
        CyTimeBrema = cyTimeBrema;
        CyTimeEvert1 = cyTimeEvert1;
        CyTimeEvert2 = cyTimeEvert2;
        CyTimeEvert3 = cyTimeEvert3;
    }
}

