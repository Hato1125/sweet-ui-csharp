using System.Drawing;

namespace Sweet.Controls;

public interface IUITextStyle : IUIControlBaseStyle
{
    /// <summary>
    /// テキストの間隔
    /// </summary>
    int TextSpace { get; set; }

    /// <summary>
    /// 行の間隔
    /// </summary>
    int LineSpace { get; set; }

    /// <summary>
    /// 前景の透明度
    /// </summary>
    byte ForeAlpha { get; set; }

    /// <summary>
    /// 前景の色
    /// </summary>
    Color ForeColor { get; set; }

    /// <summary>
    /// フォント名
    /// </summary>
    string FontName { get; set; }

    /// <summary>
    /// フォントサイズ
    /// </summary>
    int FontSize { get; set; }

    /// <summary>
    /// フォントの太さ
    /// </summary>
    int FontThick { get; set; }
}