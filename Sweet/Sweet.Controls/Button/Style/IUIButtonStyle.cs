using System.Drawing;

namespace Sweet.Controls;

public interface IUIButtonStyle : IUITextStyle
{
    /// <summary>
    /// クリック時の色
    /// </summary>
    Color ClickColor { get; set; }

    /// <summary>
    /// フェードの透明度
    /// </summary>
    byte FadeAlpha { get; set; }

    /// <summary>
    /// アニメのスピード
    /// </summary>
    double AnimeSpeed { get; set; }
}