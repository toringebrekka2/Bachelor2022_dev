using System.Collections;
using System.Linq;

namespace Straisimulator.AppTools;

public class AllOrderQues
{
    private ArrayList _orders = new ArrayList();

    public AllOrderQues()
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
            lastOrderQue = (QueForOrder)_orders[_orders.Count - 1];
            return lastOrderQue;
        }
        else
        {
            throw new Exception(string.Format("ArrayList '_orders' does not contain any QueForOrder objects"));
        }
        
    }
}