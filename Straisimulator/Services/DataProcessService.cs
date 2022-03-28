namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }
    
    public List<int> getOpAndCykTime(string inputText)
    {
        
    }

    public bool checkForOperation(string input)
    {
        if(input.Contains("Operation"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //tid: (?<operasjonstid>\d{2}:\d{2}) .+tid: (?<cykeltid>\d{2}:\d{2})
    
    //henter hele productionEventTypes tabellen, men tar kun med EventType (som er id):
    /*var productionEventTypes = ApplicationDbContext.ProductionEventTypes

    /*List<ProductionEventTypes> eventTypes = productionEventTypes.Select(e => new ProductionEventTypes
        {
            EventType = e.EventType,
            DescriptionE = e.DescriptionE,
        })
        .ToList();*/
}