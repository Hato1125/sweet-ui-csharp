using System.Drawing;
using Sweet.Elements;

namespace Sweet.Controls;

public record UITextStyle : UIControlBaseStyle, IUITextStyle
{
    public int TextSpace { get; set; }
    public int LineSpace { get; set; }
    public byte ForeAlpha { get; set; }
    public Color ForeColor { get; set; }
    public string FontName { get; set; }
    public int FontSize { get; set; }
    public int FontThick { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public UITextStyle()
    {
        TextSpace = 1;
        LineSpace = 2;
        ForeAlpha = 255;
        ForeColor = Color.Black;
        FontName = "Segoe UI";
        FontSize = 20;
        FontThick = 0;
    }

    public override void StyleAdapt()
    {
        base.StyleAdapt();

        if (Control == null)
            return;

        if (Control is UITextBaseControl)
        {
            var cnt = (UITextBaseControl)Control;
            cnt.TextContent.TextSpace = TextSpace;
            cnt.TextContent.LineSpace = LineSpace;
            cnt.TextContent.ForegroundAlpha = ForeAlpha;
            cnt.TextContent.ForegroundColor = ForeColor;
            cnt.FontName = FontName;
            cnt.FontSize = FontSize;
            cnt.FontThick = FontThick;
        }
    }
}