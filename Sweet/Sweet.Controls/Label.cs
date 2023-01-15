using DxLibDLL;
using System.Drawing;

namespace Sweet.Controls;

public class Label : UITextBase
{
    /// <summary>
    /// UIView内でのテキストの水平方向の位置
    /// </summary>
    public HorizontalAlignment TextHorizontalAlignment { get; set; }

    /// <summary>
    /// UIView内でのテキストの垂直方向の位置
    /// </summary>
    public VerticalAlignment TextVerticalAlignment { get; set; }

    /// <summary>
    /// 水平方向のオフセット
    /// </summary>
    public int TextHorizontalOffset { get; set; }

    /// <summary>
    /// 垂直方向のオフセット
    /// </summary>
    public int TextVerticalOffset { get; set; }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public Label(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height, fontName, fontSize, fontThick)
    {
        TextHorizontalAlignment = HorizontalAlignment.Center;
        TextVerticalAlignment = VerticalAlignment.Center;
        ForegroundAlpha = 255;
        Text = "Label";
    }

    protected override void RenderForeground()
    {
        base.RenderForeground();

        RenderText();
    }

    /// <summary>
    /// テキストをレンダリングする
    /// </summary>
    private void RenderText()
    {
        if (string.IsNullOrWhiteSpace(Text)
            || ForegroundAlpha <= 0
            || ForegroundColor == Color.Empty)
            return;

        uint foreColor = DX.GetColor(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B);
        int width = DX.GetDrawStringWidthToHandle(Text, Text.Length, FontHandle);
        int height = DX.GetFontSizeToHandle(FontHandle);
        var pos = UIPositionUtilt.CalUIPosition(
            TextHorizontalAlignment,
            TextVerticalAlignment,
            TextHorizontalOffset,
            TextVerticalOffset,
            Width,
            Height,
            width,
            height
        );

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, ForegroundAlpha);
        DX.DrawStringToHandle(pos.X, pos.Y, Text, foreColor, FontHandle);
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }
}