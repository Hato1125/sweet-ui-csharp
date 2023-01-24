using DxLibDLL;
using Sweet.Elements;

namespace Sweet.Controls;

public class UITextBaseControl : UIControl
{
    private string _bufFontName = string.Empty;
    private int _bufFontSize;
    private int _bufFontThick;

    protected LineText _text = new();

    /// <summary>
    /// テキストコンテンツ
    /// </summary>
    public ILineText TextContent
    {
        get => (ILineText)_text;
        set => _text = (LineText)value;
    }

    /// <summary>
    /// フォントハンドル
    /// </summary>
    protected int FontHandle { get; set; }

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
    public int FontThick { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UITextBaseControl(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height)
    {
        FontName = fontName;
        FontSize = fontSize;
        FontThick = fontThick;
    }

    public override void Update()
    {
        FontUpdate();
        base.Update();
    }

    public override void Dispose()
    {
        base.Dispose();

        DisposeFont();
    }

    protected void DisposeFont()
    {
        if (FontHandle != -1)
            DX.DeleteFontToHandle(FontHandle);
    }

    /// <summary>
    /// フォントを更新する
    /// </summary>
    private void FontUpdate()
    {
        if (_bufFontName != FontName
            || _bufFontSize != FontSize
            || _bufFontThick != FontThick)
        {
            Tracer.Log("FontUpdate.");

            DisposeFont();

            DX.SetFontCacheUsePremulAlphaFlag(DX.TRUE);
            FontHandle = DX.CreateFontToHandle(FontName, FontSize, FontThick, DX.DX_FONTTYPE_ANTIALIASING_16X16);

            // 次のフレームで再生成しないようにセットしとく
            _bufFontName = FontName;
            _bufFontSize = FontSize;
            _bufFontThick = FontThick;
        }
    }
}