using System.Diagnostics;
using System.Drawing;

namespace Sweet.Controls;

public class UIButtonBase : UITextBaseControl
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan Time;

    protected UIButtonStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIButtonStyle Style
    {
        get => (IUIButtonStyle)_style;
        set => _style = (UIButtonStyle)value;
    }

    /// <summary>
    /// アニメの現在の値
    /// </summary>
    protected double AnimeValue { get; set; }

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
    /// クリック時のアニメーションの透明度
    /// </summary>
    public byte FadeAlpha { get; set; }

    /// <summary>
    /// クリック時の色
    /// </summary>
    public Color ClickColor { get; set; }

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
        Style.AnimeSpeed = 1025;
        Style.FadeAlpha = 255;
        Style.ClickColor = Color.FromArgb(220, 220, 220);
        Style.FontName = fontName;
        Style.FontSize = fontSize;
        Style.FontThick = fontThick;
    }

    public override void Update()
    {
        Time = _stopwatch.Elapsed;
        DeltaTime = Time.TotalSeconds;
        _stopwatch.Restart();

        _style.Control = this;
        _style.StyleAdapt();
        base.Update();
        TickAnimation();
    }

    /// <summary>
    /// アニメを進行する
    /// </summary>
    private void TickAnimation()
    {
        if (IsTickAnimation)
        {
            AnimeValue += AnimeSpeed * DeltaTime;

            if (AnimeValue > 90)
                AnimeValue = 90;
        }
        else
        {
            AnimeValue -= AnimeSpeed * DeltaTime;

            if (AnimeValue < 0)
                AnimeValue = 0;
        }
    }
}