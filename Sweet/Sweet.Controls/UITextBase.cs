using DxLibDLL;
using Sweet.Elements;

namespace Sweet.Controls;

public class UITextBase : UIView
{
    private string _bufFontName;
    private int _bufFontSize;
    private int _bufFontThick;

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
    public UITextBase(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height)
    {
        FontName = fontName;
        FontSize = fontSize;
        FontThick = fontThick;
        _bufFontName = fontName;
        _bufFontSize = fontSize;
        _bufFontThick = fontThick;

        IsBuild = true;
    }

    protected override void Build()
    {
        base.Build();

        DX.SetFontCacheUsePremulAlphaFlag(DX.TRUE);
        FontHandle = DX.CreateFontToHandle(FontName, FontSize, FontThick, DX.DX_FONTTYPE_ANTIALIASING_4X4);
    }

    protected override bool WatchBuild()
    {
        if (_bufFontName != FontName
            || _bufFontSize != FontSize
            || _bufFontThick != FontThick)
        {
            _bufFontName = FontName;
            _bufFontSize = FontSize;
            _bufFontThick = FontThick;

            return true;
        }
        else
        {
            return base.WatchBuild();
        }
    }

    public override void Dispose(bool isChildDispoes = true)
    {
        base.Dispose(isChildDispoes);

        if (FontHandle != -1 && FontHandle != 0)
            DX.DeleteFontToHandle(FontHandle);
    }
}