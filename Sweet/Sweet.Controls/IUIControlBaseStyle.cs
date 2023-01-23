using System.Drawing;

namespace Sweet.Controls;

public interface IUIControlBaseStyle
{
    /// <summary>
    /// 透明度
    /// </summary>
    byte Alpha { get; set; }

    /// <summary>
    /// 背景の透明度
    /// </summary>
    byte BackAlpha { get; set; }

    /// <summary>
    /// 背景の色
    /// </summary>
    Color BackColor { get; set; }

    /// <summary>
    /// 角の半径
    /// </summary>
    float Radius { get; set; }
}