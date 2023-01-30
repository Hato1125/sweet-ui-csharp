using System.Drawing;

namespace Sweet.Controls;

public record UIButtonStyle : UITextStyle, IUIButtonStyle
{
    public Color ClickColor { get; set; }
    public byte FadeAlpha { get; set; }
    public double AnimeSpeed { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <returns></returns>
    public UIButtonStyle() : base()
    {
        ClickColor = Color.Gray;
        FadeAlpha = 255;
        AnimeSpeed = 800;
    }

    public override void StyleAdapt()
    {
        base.StyleAdapt();

        if (Control == null)
            return;

        if (Control is UIButtonBase)
        {
            var btn = (UIButtonBase)Control;

            btn.ClickColor = ClickColor;
            btn.AnimeSpeed = AnimeSpeed;
            btn.FadeAlpha = FadeAlpha;
        }
    }
}