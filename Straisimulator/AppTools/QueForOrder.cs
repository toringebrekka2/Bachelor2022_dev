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

    public void AddBremEvQue(Que que)
    {
        _bremEvQue.Add(que);
    }

    public void AddEvPresQue(Que que)
    {
        _evPresQue.Add(que);
    }

    public ArrayList GetBremEvQue()
    {
        return _bremEvQue;
    }

    public ArrayList GetEvPresQue()
    {
        return _evPresQue;
    }

    public int GetId()
    {
        return id;
    }
}