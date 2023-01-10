using System.Drawing;

namespace Sweet.Elements;

public record UIStyle : IViewStyle
{
    /// <summary>
    /// 背景の透明度
    /// </summary>
    public int BackOpacity { get; set; }

    /// <summary>
    /// ボーダーの透明度
    /// </summary>
    public int BorderOpacity { get; set; }

    /// <summary>
    /// 角の半径
    /// </summary>
    public float CornerRadius { get; set; }

    /// <summary>
    /// ボーダーのサイズ
    /// </summary>
    public float BorderSize { get; set; }


    /// <summary>
    /// 背景の色
    /// </summary>
    public Color BackColor { get; set; }

    /// <summary>
    /// ボーダーの色
    /// </summary>
    public Color BorderColor { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public UIStyle()
    {
        BackOpacity = 255;
        BorderOpacity = 255;
        CornerRadius = 20.0f;
        BorderSize = 2.0f;
        BackColor = Color.FromArgb(255, 255, 255);
        BorderColor = Color.FromArgb(240, 240, 240);
    }
}