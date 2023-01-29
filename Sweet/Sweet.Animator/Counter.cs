using System.Diagnostics;

namespace Sweet.Animator;

public class Counter
{
    private readonly Stopwatch stopwatch = new();
    private TimeSpan _time;
    private bool _isStart;

    public double Begin { get; set; }
    public double End { get; set; }
    public double Time { get; set; }
    public double Value { get; private set; }
    public bool IsLoop { get; set; }

    public Counter(double begin, double end, double time, bool isloop = false)
    {
        Begin = begin;
        End = end;
        Time = time;
        IsLoop = isloop;
    }

    public void Start()
    {
    }

    public void Tick()
    {
    }
}