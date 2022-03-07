namespace Straisimulator.AppTools;

public class Que
{
    private TimeSpan _queTime;

    public Que(TimeOnly queStart, TimeOnly queEnd)
    {
        _queTime = queEnd - queStart;
    }
}