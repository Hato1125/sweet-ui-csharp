using DxLibDLL;
using System.Drawing;
using Sweet.Elements;

namespace Sweet.Controls;

public class UIToggleSwitch : UIButtonBase
{
    protected UIToggleSwitchStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIToggleSwitchStyle Style
    {
        get => (IUIToggleSwitchStyle)_style;
        set => _style = (UIToggleSwitchStyle)value;
    }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    /// <summary>
    /// オンか
    /// </summary>
    public bool IsOn { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UIToggleSwitch(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height, fontName, fontSize, fontThick)
    {
        Style.FontName = fontName;
        Style.FontSize = fontSize;
        Style.FontThick = fontThick;
        Style.ClickColor = Color.FromArgb(197, 112, 238);
        Style.Radius = height / 2;
        Style.ForeColor = Color.White;
        Style.AnimeSpeed = 700;
        TextContent.HorizontalAlignment = HorizontalAlignment.Left;
        TextContent.HorizontalOffset = 10;
        Text = "ToggleSwitch";
    }

    public override void Update()
    {
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();

        if (IsPushed())
            IsOn = !IsOn;

        _text.FontHandle = FontHandle;
        _text.ParentWidth = Width;
        _text.ParentHeight = Height;
        _text.UpdateText();

        IsTickAnimation = IsOn;
    }

    public override void DrawView()
    {
        base.DrawView();

        (_text.X, _text.Y) = (X + Width, Y);
        _text.DrawText();
    }

    protected override void DrawViewArea()
    {
        base.DrawViewArea();
        DrawFade();
        DrawToggle();
    }

    /// <summary>
    /// フェードの描画
    /// </summary>
    private void DrawFade()
    {
        if (AnimeSpeed <= 0 || FadeAlpha <= 0 || ClickColor == Color.Empty)
            return;

        uint clickColor = DX.GetColor(ClickColor.R, ClickColor.G, ClickColor.B);
        double fade = Math.Sin(AnimeValue * Math.PI / 180) * FadeAlpha;

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, (int)fade);
        if (Radius <= 0)
        {
            DX.DrawFillBox(0, 0, Width, Height, clickColor);
        }
        else
        {
            DX.DrawRoundRectAA(0, 0, Width, Height, Radius, Radius, 100, clickColor, DX.TRUE);
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }

    /// <summary>
    /// トグルの描画
    /// </summary>
    private void DrawToggle()
    {
        if (Style.ToggleAlpha <= 0 || Style.ToggleColor == Color.Empty)
            return;

        uint toggleColor = DX.GetColor(Style.ToggleColor.R, Style.ToggleColor.G, Style.ToggleColor.B);
        int radius = Height / 2;
        double moveArea = Width - radius * 2;
        double move = Math.Sin(AnimeValue * Math.PI / 180) * moveArea;

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, Style.ToggleAlpha);
        DX.DrawCircleAA(
            radius + (float)move,
            radius,
            radius - Style.TogglePadding,
            byte.MaxValue,
            toggleColor
        );
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }
}