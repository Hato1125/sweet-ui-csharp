using Sweet.Elements;
using Sweet.Graphics;
using System.Drawing;

namespace Sweet.Controls;

public class UIPanel : UIControl
{
    protected UIControlBaseStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIControlBaseStyle Style
    {
        get => _style;
        set => _style = (UIControlBaseStyle)value;
    }

    /// <summary>
    /// テクスチャのX座標
    /// </summary>
    public int TexturePositionX { get; set; }

    /// <summary>
    /// テクスチャのY座標
    /// </summary>
    public int TexturePositionY { get; set; }

    /// <summary>
    /// テクスチャのレクタングル
    /// </summary>
    public Rectangle? TextureRectangle { get; set; }

    /// <summary>
    /// テクスチャ
    /// </summary>
    public Texture? Texture { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIPanel(int width, int height)
        : base(width, height)
    {
    }

    public override void Update()
    {
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();
    }

    protected override void DrawViewArea()
    {
        base.DrawViewArea();
        DrawImage();
    }

    public override void Dispose()
    {
        base.Dispose();
        Texture?.Dispose();
    }

    private void DrawImage()
        => Texture?.Draw(
            TexturePositionX,
            TexturePositionY,
            TextureRectangle
        );
}