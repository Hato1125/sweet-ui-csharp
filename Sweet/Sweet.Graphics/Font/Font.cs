using System.Numerics;

namespace Sweet.Graphics;

public record Font
{
    /// <summary>
    /// フォント名
    /// </summary>
    public string FontName { get; set; }

    /// <summary>
    /// フォントサイズ
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// フォントの太さ
    /// </summary>
    public int FontWeight { get; set; }

    /// <summary>
    /// 文字の間隔
    /// </summary>
    public int TextSpace { get; set; }

    /// <summary>
    /// 行の間隔
    /// </summary>
    public int LineSpace { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public Font()
    {
        FontName = "Segoe UI";
        FontSize = 20;
        FontWeight = 0;
        TextSpace = 2;
        LineSpace = 3;
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントのサイズ</param>
    /// <param name="fontWeight">フォントの太さ</param>
    /// <param name="textSpace">文字の間隔</param>
    /// <param name="lineSpace">行の間隔</param>
    public Font(string fontName, int fontSize, int fontWeight, int textSpace, int lineSpace)
    {
        FontName = fontName;
        FontSize = fontSize;
        FontWeight = fontWeight;
        TextSpace = textSpace;
        LineSpace = lineSpace;
    }

    /*
    public static bool operator ==(Font f1, Font f2)
    {
        if(f1.FontName == f2.FontName
            && f1.FontSize == f2.FontSize
            && f1.FontWeight == f2.FontWeight
            && f1.TextSpace == f2.TextSpace
            && f1.LineSpace == f2.LineSpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(Font f1, Font f2)
    {
        if (f1.FontName != f2.FontName
            || f1.FontSize != f2.FontSize
            || f1.FontWeight != f2.FontWeight
            || f1.TextSpace != f2.TextSpace
            || f1.LineSpace != f2.LineSpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
}