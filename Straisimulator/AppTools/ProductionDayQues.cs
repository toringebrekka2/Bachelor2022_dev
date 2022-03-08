using System.Collections;
using System.Linq;

namespace Straisimulator.AppTools;

public class ProductionDayQues
{
    private List<QueForOrder> _orders = new List<QueForOrder>();

    public ProductionDayQues()
    {
        
    }

    public void AddQueForOrder(QueForOrder queForOrder)
    {
        _orders.Add(queForOrder);
    }

    // Ikke ferdig:
    public QueForOrder GetLast()
    {
        QueForOrder lastOrderQue = null;
        if (_orders.Count > 0)
        {
            lastOrderQue = _orders[_orders.Count - 1];
            return lastOrderQue;
        }
        else
        {
            throw new Exception(string.Format("ArrayList '_orders' does not contain any QueForOrder objects"));
        }
        
    }
}