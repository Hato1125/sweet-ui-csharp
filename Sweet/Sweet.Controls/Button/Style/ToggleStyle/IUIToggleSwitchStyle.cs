using System.Drawing;

namespace Sweet.Controls;

public interface IUIToggleSwitchStyle : IUIButtonStyle
{
    /// <summary>
    /// トグルの色
    /// </summary>
    Color ToggleColor { get; set; }

    /// <summary>
    /// トグルの透明度
    /// </summary>
    byte ToggleAlpha { get; set; }

    /// <summary>
    /// トグルのパディング
    /// </summary>
    int TogglePadding { get; set; }
}