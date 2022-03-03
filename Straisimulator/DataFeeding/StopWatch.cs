namespace Straisimulator.DataFeeding;

public class StopWatch
{
    private DateTime first;
    private DateTime second;
    private DateTime result;
    
    public StopWatch()
    {
        
    }

    public void start()
    {
        first = DateTime.Now;
    }

    public void stop()
    {
        second = DateTime.Now;
    }

    /*public DateTime getResult()
    {
        result = second - first;
    }*/
}