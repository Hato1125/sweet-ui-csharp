using System.Drawing;
using DxLibDLL;

namespace Sweet.Graphics;

public class FontText : ITexturable, IDisposable
{
    private int _fontHandle;
    private string[]? _texts;
    private int _fontWidth;
    private int _fontHeight;
    private string _bufText;
    private Color _bufColor;
    private DrawArea? _area;
    private Font? _bufFont;

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text;

    /// <summary>
    /// テキストの再描画をするか
    /// </summary>
    public bool IsReDraw { get; set; }

    /// <summary>
    /// フォントのスタイル
    /// </summary>
    public Font FontStyle { get; set; }

    /// <summary>
    /// 前景色
    /// </summary>
    public Color ForeColor { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public FontText()
    {
        _bufText = string.Empty;
        Text = "Text";

        FontStyle = new();
        ForeColor = Color.White;
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontWeight">フォントの太さ</param>
    public FontText(string fontName, int fontSize, int fontWeight)
    {
        _bufText = string.Empty;
        Text = "Text";

        FontStyle = new(fontName, fontSize, fontWeight, 2, 3);
        ForeColor = Color.White;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public void Update()
    {
        if(_bufFont == null || FontStyle != _bufFont || Text != _bufText || ForeColor != _bufColor || IsReDraw)
        {
            DX.SetFontCacheCharNum(Text.Length);
            MakeFontHandle();

            // 改行するか
            if (Text.Contains('\n'))
            {
                GetSplitTextSize();
                DrawSplitText();
            }
            else
            {
                GetSingleTextSize();
                DrawSingleText();
            }

            DX.DeleteFontToHandle(_fontHandle);
            DX.SetFontCacheCharNum(0);

            _bufFont = FontStyle;
            _bufText = Text;
            _bufColor = ForeColor;
            IsReDraw = false;
        }
    }

    public Texture GetTexture()
    {
        if (_area == null)
            throw new Exception("Failed to generate texture.");
        else
            return _area.GetTexture();
    }

    public int GetGraphHandle()
    {
        if (_area == null)
            throw new Exception("Failed to generate texture.");
        else
            return _area.GetGraphHandle();
    }

    public void Dispose()
    {
        if (_area == null)
            return;

        _area.GetTexture().Dispose();
    }

    private void MakeFontHandle()
    {
        _fontHandle = DX.CreateFontToHandle(
            FontStyle.FontName,
            FontStyle.FontSize,
            FontStyle.FontWeight,
            DX.DX_FONTTYPE_ANTIALIASING_16X16
        );
    }

    private void GetSplitTextSize()
    {
        Tracer.Log("GetSplitTextSize.");

        _texts = Text.Split('\n');
        int[] width = new int[_texts.Length];

        DX.SetFontSpaceToHandle(FontStyle.TextSpace, _fontHandle);
        for(int i = 0; i < width.Length; i++)
        {
            width[i] = DX.GetDrawStringWidthToHandle(
                _texts[i],
                _texts[i].Length,
                _fontHandle
            );
        }

        _fontWidth = width.Max();
        _fontHeight = DX.GetFontSizeToHandle(_fontHandle);
    }

    private void GetSingleTextSize()
    {
        Tracer.Log("GetSingleTextSize.");

        DX.SetFontSpaceToHandle(FontStyle.TextSpace, _fontHandle);
        _fontWidth = DX.GetDrawStringWidthToHandle(Text, Text.Length, _fontHandle);
        _fontHeight = DX.GetFontSizeToHandle(_fontHandle);
    }

    private void DrawSplitText()
    {
        if (_texts == null || ForeColor == Color.Empty)
            return;

        Tracer.Log("TextRendering.");
        uint color = DX.GetColor(ForeColor.R, ForeColor.G, ForeColor.B);

        // 描画領域の高さを計算
        int height = (_fontHeight + FontStyle.LineSpace) * _texts.Length;
        Console.WriteLine();
        _area = new(_fontWidth, height);
        _area.DrawingArea(() =>
        {
            for(int i = 0; i < _texts.Length; i++)
                DX.DrawStringToHandle(0, (_fontHeight + FontStyle.LineSpace) * i, _texts[i], color, _fontHandle);
        });
    }

    private void DrawSingleText()
    {
        if(ForeColor == Color.Empty)
            return;

        Tracer.Log("TextRendering.");
        uint color = DX.GetColor(ForeColor.R, ForeColor.G, ForeColor.B);

        _area = new(_fontWidth, _fontHeight);
        _area.DrawingArea(() => DX.DrawStringToHandle(0, 0, Text, color, _fontHandle));
    }
}