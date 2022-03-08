using System.Collections;

namespace Straisimulator.AppTools;

public class QueForOrder
{
    private int id;
    private List<Que> _bremEvQue = new List<Que>();
    private List<Que> _evPresQue = new List<Que>();
    private List<Que> _generalQue = new List<Que>();

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

    public List<Que> GetBremEvQue()
    {
        return _bremEvQue;
    }

    public List<Que> GetEvPresQue()
    {
        return _evPresQue;
    }

    public void AddGeneralQue(Que que)
    {
        _generalQue.Add(que);
    }

    public List<Que> GetGeneralQue()
    {
        return _generalQue;
    }

    public int GetId()
    {
        return id;
    }
    
    // samlet 
}