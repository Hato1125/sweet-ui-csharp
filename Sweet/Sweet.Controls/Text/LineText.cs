using DxLibDLL;
using System.Drawing;
using Sweet.Elements;

namespace Sweet.Controls;

public class LineText : ILineText
{
    private int _bufTextSpace;
    private string _bufText;
    private string[]? _texts;
    private int[]? _textWidths;
    private int _textHeight;

    public int TextSpace { get; set; }
    public int LineSpace { get; set; }
    public int FontHandle { get; set; }
    public int ParentWidth { get; set; }
    public int ParentHeight { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public VerticalAlignment VerticalAlignment { get; set; }
    public int HorizontalOffset { get; set; }
    public int VerticalOffset { get; set; }
    public string Text { get; set; }
    public byte ForegroundAlpha { get; set; }
    public Color ForegroundColor { get; set; }

    public LineText()
    {
        TextSpace = 1;
        LineSpace = 2;
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
        ForegroundColor = Color.Black;
        ForegroundAlpha = 255;
        Text = string.Empty;
        _bufText = string.Empty;
    }

    public void UpdateText()
    {
        if (_bufText != Text || _bufTextSpace != TextSpace)
        {
            DX.SetFontSpaceToHandle(TextSpace, FontHandle);

            // 改行するか
            if (Text.Contains("\n"))
                GetMultipleLineTextSize();
            else
                GetSingleLineTextSize();

            _bufText = Text;
            _bufTextSpace = TextSpace;
        }
    }

    public void DrawText()
    {
        if (ForegroundAlpha <= 0 || ForegroundColor == Color.Empty || _textWidths == null)
            return;

        uint foreColor = DX.GetColor(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B);

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, ForegroundAlpha);

        // 改行するか
        if (Text.Contains("\n"))
        {
            if (_texts != null)
            {
                int tergetHeight = (_textHeight + LineSpace) * _texts.Length;

                for (int i = 0; i < _texts.Length; i++)
                {
                    var pos = UIPositionUtilt.CalculateUIPosition(
                        HorizontalAlignment,
                        VerticalAlignment,
                        HorizontalOffset,
                        VerticalOffset,
                        ParentWidth,
                        ParentHeight,
                        _textWidths[i],
                        tergetHeight
                    );

                    DX.DrawStringFToHandle(pos.X, pos.Y + (_textHeight + LineSpace) * i, _texts[i], foreColor, FontHandle);
                }
            }
        }
        else
        {
            var pos = UIPositionUtilt.CalculateUIPosition(
                HorizontalAlignment,
                VerticalAlignment,
                HorizontalOffset,
                VerticalOffset,
                ParentWidth,
                ParentHeight,
                _textWidths[0],
                _textHeight
            );

            DX.DrawStringFToHandle(pos.X, pos.Y, Text, foreColor, FontHandle);
        }

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }

    private void GetMultipleLineTextSize()
    {
        _texts = Text.Split("\n");
        _textWidths = new int[_texts.Length];

        for (int i = 0; i < _texts.Length; i++)
            _textWidths[i] = DX.GetDrawStringWidthToHandle(_texts[i], _texts[i].Length, FontHandle);

        _textHeight = DX.GetFontSizeToHandle(FontHandle);
    }

    private void GetSingleLineTextSize()
    {
        _textWidths = new int[] {
            DX.GetDrawStringWidthToHandle(Text, Text.Length, FontHandle)
        };

        _textHeight = DX.GetFontSizeToHandle(FontHandle);
    }
}