using System.Collections;

namespace Straisimulator.AppTools;

public class QueForOrder
{
    private int id;
    private ArrayList _bremEvQue = new ArrayList();
    private ArrayList _evPresQue = new ArrayList();
    private ArrayList _generalQue = new ArrayList();

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

    public void AddGeneralQue(Que que)
    {
        _generalQue.Add(que);
    }

    public ArrayList GetGeneralQue()
    {
        return _generalQue;
    }

    public int GetId()
    {
        return id;
    }
}