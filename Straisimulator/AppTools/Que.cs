namespace Straisimulator.AppTools;

public class Que
{
    private int _id;
    private TimeSpan _queTime;

    public Que(int id, DateTime queStart, DateTime queEnd)
    {
        _id = id;
        _queTime = queEnd - queStart;
    }

    public string GetAsString()
    {
        return _queTime.ToString();
    }
}