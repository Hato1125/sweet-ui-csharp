using System.Drawing;
using Sweet.Elements;

namespace Sweet.Controls;

public record UIControlBaseStyle : IUIControlBaseStyle
{
    /// <summary>
    /// 適応させるコントロール
    /// </summary>
    public UIControl? Control { get; set; }

    public byte Alpha { get; set; }
    public byte BackAlpha { get; set; }
    public Color BackColor { get; set; }
    public float Radius { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public UIControlBaseStyle()
    {
        Alpha = 255;
        BackAlpha = 255;
        BackColor = Color.White;
        Radius = 10;
    }

    /// <summary>
    /// スタイルをコントロールに適応させる
    /// </summary>
    public virtual void StyleAdapt()
    {
        if (Control == null)
            return;

        Control.Alpha = Alpha;
        Control.BackgroundAlpha = BackAlpha;
        Control.BackgroundColor = BackColor;
        Control.Radius = Radius;
    }
}