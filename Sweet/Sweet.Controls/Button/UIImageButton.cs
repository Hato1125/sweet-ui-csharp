using DxLibDLL;
using System.Drawing;

namespace Sweet.Controls;

public class UIImageButton : UIButtonBase
{
    private int _bufImageHandle;

    protected UIButtonStyle _style = new();
    protected int ImageHandle { get; private set; }

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIButtonStyle Style
    {
        get => _style;
        set => _style = (UIButtonStyle)value;
    }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="image">グラフィックハンドル</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UIImageButton(int image, string fontName, int fontSize, int fontThick)
        : base(0, 0, fontName, fontSize, fontThick)
    {
        Style.FontName = fontName;
        Style.FontSize = fontSize;
        Style.FontThick = fontThick;
        Style.BackColor = Color.Empty;
        Text = "ImageButton";

        DX.SetUsePremulAlphaConvertLoad(DX.TRUE);
        ImageHandle = image;
        Tracer.Log($"ImageHandle: {ImageHandle}");
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="path">画像のパス</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UIImageButton(string path, string fontName, int fontSize, int fontThick)
        : base(0, 0, fontName, fontSize, fontThick)
    {
        Style.FontName = fontName;
        Style.FontSize = fontSize;
        Style.FontThick = fontThick;
        Style.BackColor = Color.Empty;
        Text = "ImageButton";

        DX.SetUsePremulAlphaConvertLoad(DX.TRUE);
        ImageHandle = DX.LoadGraph(path);
        Tracer.Log($"ImageHandle: {ImageHandle}");
    }

    public override void Update()
    {
        CheckImage();
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();
        _text.FontHandle = FontHandle;
        _text.ParentWidth = Width;
        _text.ParentHeight = Height;
        _text.UpdateText();

        IsTickAnimation = IsPushing();
    }

    /// <summary>
    /// 画像を更新する
    /// </summary>
    private void CheckImage()
    {
        if (_bufImageHandle != ImageHandle)
        {
            Tracer.Log("ImageUpdate.");
            DX.GetGraphSize(ImageHandle, out int w, out int h);
            Width = w;
            Height = h;

            _bufImageHandle = ImageHandle;
        }
    }

    protected override void DrawViewArea()
    {
        base.DrawViewArea();
        DrawImage();
        _text.DrawText();
    }

    /// <summary>
    /// 画像を描画する
    /// </summary>
    private void DrawImage()
    {
        if (ImageHandle == -1)
            return;

        var fade = CalculateFade();
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, BackgroundAlpha);
        DX.SetDrawBright(255 - (int)fade.R, 255 - (int)fade.G, 255 - (int)fade.B);
        DX.DrawGraph(0, 0, ImageHandle, DX.TRUE);
        DX.SetDrawBright(255, 255, 255);
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 255);
    }

    /// <summary>
    /// フェードを計算する
    /// </summary>
    private (double R, double G, double B) CalculateFade()
    {
        return (
            Math.Sin(AnimeValue * Math.PI / 180) * (255 - ClickColor.R),
            Math.Sin(AnimeValue * Math.PI / 180) * (255 - ClickColor.G),
            Math.Sin(AnimeValue * Math.PI / 180) * (255 - ClickColor.B)
        );
    }
}