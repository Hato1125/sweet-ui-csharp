using System.Drawing;
using Sweet.Elements;

namespace Sweet.Controls;

public interface ILineText
{
    /// <summary>
    /// 文字の間隔
    /// </summary>
    int TextSpace { get; set; }

    /// <summary>
    /// 行の間隔
    /// </summary>
    int LineSpace { get; set; }

    /// <summary>
    /// 水平方向の位置
    /// </summary>
    HorizontalAlignment HorizontalAlignment { get; set; }

    /// <summary>
    /// 垂直方向の位置
    /// </summary>
    VerticalAlignment VerticalAlignment { get; set; }

    /// <summary>
    /// 水平方向のオフセット
    /// </summary>
    int HorizontalOffset { get; set; }

    /// <summary>
    /// 垂直方向のオフセット
    /// </summary>
    int VerticalOffset { get; set; }

    /// <summary>
    /// 前景の透明度
    /// </summary>
    byte ForegroundAlpha { get; set; }
    
    /// <summary>
    /// 前景の色
    /// </summary>
    Color ForegroundColor { get; set; }
}