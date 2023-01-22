using DxLibDLL;
using Sweet.Elements;
using System.Drawing;

namespace Sweet.Controls;

public class UIToggleButton : UIButtonBase
{
    private string _bufText = string.Empty;
    private string[]? _texts;
    private int[]? _textWidths;
    private int _textHeight;
    private int _bufTextSpace;

    /// <summary>
    /// 行の間隔
    /// </summary>
    public int LineSpace { get; set; }

    /// <summary>
    /// 文字の間隔
    /// </summary>
    public int TextSpace { get; set; }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// テキストの水平方向の位置
    /// </summary>
    public HorizontalAlignment TextHorizontalAlignment { get; set; }

    /// <summary>
    /// テキストの垂直方向の位置
    /// </summary>
    public VerticalAlignment TextVerticalAlignment { get; set; }

    /// <summary>
    /// テキストの水平方向のオフセット
    /// </summary>
    /// <value></value>
    public int TextHorizontalOffset { get; set; }

    /// <summary>
    /// テキストの垂直方向のオフセット
    /// </summary>
    public int TextVerticalOffset { get; set; }

    /// <summary>
    /// オンか
    /// </summary>
    public bool IsOn { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UIToggleButton(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height, fontName, fontSize, fontThick)
    {
        LineSpace = 3;
        TextSpace = 1;
        TextHorizontalAlignment = HorizontalAlignment.Center;
        TextVerticalAlignment = VerticalAlignment.Center;
        ClickColor = Color.FromArgb(177, 92, 178);
        Text = "ToggleButton";
    }

    public override void Update()
    {
        base.Update();

        if (IsPushed())
            IsOn = !IsOn;

        CheckChangeText();

        IsTickAnimation = IsOn;
    }

    protected override void DrawViewArea()
    {
        base.DrawViewArea();
        DrawFade();
        DrawText();
    }

    /// <summary>
    /// テキストの更新
    /// </summary>
    private void CheckChangeText()
    {
        if (_bufText != Text || _bufTextSpace != TextSpace)
        {
            Tracer.Log("ChangeText");

            DX.SetFontSpaceToHandle(TextSpace, FontHandle);
            if (Text.Contains("\n"))
                GetSplitTextSize();
            else
                GetSingleTextSize();

            _bufText = Text;
            _bufTextSpace = TextSpace;
        }
    }

    /// <summary>
    /// テキストのサイズを取得する
    /// </summary>
    private void GetSplitTextSize()
    {
        Tracer.Log("SplitText.");

        _texts = Text.Split("\n");
        _textWidths = new int[_texts.Length];

        for (int i = 0; i < _textWidths.Length; i++)
            _textWidths[i] = DX.GetDrawStringWidthToHandle(_texts[i], _texts[i].Length, FontHandle);

        _textHeight = DX.GetFontSizeToHandle(FontHandle);
    }

    /// <summary>
    /// テキストのサイズを取得する
    /// </summary>
    private void GetSingleTextSize()
    {
        Tracer.Log("SingleText.");

        _textWidths = new int[1] {
            DX.GetDrawStringWidthToHandle(Text, Text.Length, FontHandle),
        };
        _textHeight = DX.GetFontSizeToHandle(FontHandle);
    }

    /// <summary>
    /// フェードの描画
    /// </summary>
    private void DrawFade()
    {
        uint clickColor = DX.GetColor(ClickColor.R, ClickColor.G, ClickColor.B);
        double fade = Math.Sin(AnimeValue * Math.PI / 180) * ClickFadeAlpha;

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, (int)fade);
        if (Radius <= 0)
        {
            DX.DrawFillBox(0, 0, Width, Height, clickColor);
        }
        else
        {
            DX.DrawRoundRectAA(0, 0, Width, Height, Radius, Radius, 100, clickColor, DX.TRUE);
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }

    /// <summary>
    /// テキストの描画
    /// </summary>
    private void DrawText()
    {
        if (ForegroundColor == Color.Empty
            || ForegroundAlpha <= 0)
            return;

        uint foreColor = DX.GetColor(ForegroundColor.R, ForegroundColor.G, ForegroundColor.B);

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, ForegroundAlpha);

        if (_textWidths != null)
        {
            // 改行すべきか
            if (Text.Contains("\n"))
            {
                if (_texts != null)
                {
                    // TODO: 毎フレーム位置を計算してFPSが低下しているので一フレームだけ計算するようにする
                    for (int i = 0; i < _texts.Length; i++)
                    {
                        int tergetHeight = (_textHeight + LineSpace) * _texts.Length;

                        var pos = UIPositionUtilt.CalculateUIPosition(
                            TextHorizontalAlignment,
                            TextVerticalAlignment,
                            TextHorizontalOffset,
                            TextVerticalOffset,
                            Width,
                            Height,
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
                    TextHorizontalAlignment,
                    TextVerticalAlignment,
                    TextHorizontalOffset,
                    TextVerticalOffset,
                    Width,
                    Height,
                    _textWidths[0],
                    _textHeight
                );

                DX.DrawStringFToHandle(pos.X, pos.Y, Text, foreColor, FontHandle);
            }
        }

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }
}