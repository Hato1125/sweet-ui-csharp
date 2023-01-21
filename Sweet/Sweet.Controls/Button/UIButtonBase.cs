using DxLibDLL;
using System.Diagnostics;
using Sweet.Input;

namespace Sweet.Controls;

public class UIButtonBase : UITextBaseControl
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan Time;

    /// <summary>
    /// アニメの現在の値
    /// </summary>
    protected double ClickAnimeValue { get; set; }

    /// <summary>
    /// アニメを進行するか
    /// </summary>
    protected bool IsTickAnimation { get; set; }

    /// <summary>
    /// デルタタイム
    /// </summary>
    protected double DeltaTime { get; private set; }

    /// <summary>
    /// アニメのスピード
    /// </summary>
    public double AnimeSpeed { get; set; }

    /// <summary>
    ///初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UIButtonBase(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height, fontName, fontSize, fontThick)
    {
        AnimeSpeed = 250;
    }

    public override void Update()
    {
        Time = _stopwatch.Elapsed;
        DeltaTime = Time.TotalSeconds;
        _stopwatch.Restart();

        base.Update();
        TickAnimation();


        DX.DrawStringF((float)ClickAnimeValue, 150, DeltaTime.ToString(), 0xffffff);
    }

    /// <summary>
    /// アニメを進行する
    /// </summary>
    private void TickAnimation()
    {
        if (IsTickAnimation)
        {
            ClickAnimeValue += AnimeSpeed * DeltaTime;

            if (ClickAnimeValue > 90)
                ClickAnimeValue = 90;
        }
        else
        {
            ClickAnimeValue -= AnimeSpeed * DeltaTime;

            if (ClickAnimeValue < 0)
                ClickAnimeValue = 0;
        }
    }
}