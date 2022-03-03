using System.Collections;

namespace Straisimulator.AppTools;

public class QueForOrder
{
    private int id;
    private ArrayList _bremEvQue = new ArrayList();
    private ArrayList _evPresQue = new ArrayList();

    public QueForOrder(int id)
    {
        this.id = id;
    }

    public void addBremEvQue(Que que)
    {
        _bremEvQue.Add(que);
    }

    public void addEvPresQue(Que que)
    {
        _evPresQue.Add(que);
    }

    public ArrayList getBremEvQue()
    {
        return _bremEvQue;
    }

    public ArrayList getEvPresQue()
    {
        return _evPresQue;
    }

    public int getID()
    {
        return id;
    }
}