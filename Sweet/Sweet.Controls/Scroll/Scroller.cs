using Sweet.Input;
using System.Diagnostics;

namespace Sweet.Controls;

public class Scroller
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _time;

    private bool _isTick;
    private bool _isDire;
    private double _delta;
    private double _counter;

    /// <summary>
    /// スクロールスピード
    /// </summary>
    public double ScrollSpeed { get; set; }

    /// <summary>
    /// 摩擦
    /// </summary>
    public double Friction { get; set; }

    /// <summary>
    /// 最小値
    /// </summary>
    public double MinValue { get; set; }

    /// <summary>
    /// 最大値
    /// </summary>
    public double MaxValue { get; set; }

    /// <summary>
    /// 現在の値
    /// </summary>
    public double Value { get; private set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <param name="speed">スピード</param>
    /// <param name="friction">摩擦</param>
    public Scroller(double min, double max ,double speed = 100, double friction = 1.0)
    {
        ScrollSpeed = speed;
        Friction = friction;
        MinValue = min;
        MaxValue = max;
        Value = min;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public void Update()
    {
        _time = _stopwatch.Elapsed;
        _delta = _time.TotalSeconds;
        _stopwatch.Restart();

        if (Mouse.Wheel != 0)
            StartEasing();

        if (_isTick)
        {
            Tick(90);
            double easing = GetEasing(90, _counter);
            Value += _isDire ? easing : -easing;

            if (Value > MaxValue)
            {
                Value = MaxValue;
                _isTick = false;
            }
            else if (Value < MinValue)
            {
                Value = MinValue;
                _isTick = false;
            }
        }
    }

    private void StartEasing()
    {
        _isTick = true;
        _isDire = Mouse.Wheel < 0;
        _counter = 0;
    }

    private void Tick(double end)
    {
        if (!_isTick)
            return;

        _counter += _delta * ScrollSpeed * Friction;

        if (_counter > end)
        {
            _counter = end;
            _isTick = false;
        }
    }

    private double GetEasing(double end, double nowValue)
        => (end - nowValue) * 0.03;
}
