using DxLibDLL;

namespace Sweet.Graphics;

public class DrawArea : ITexturable
{
    /// <summary>
    /// テクスチャ
    /// </summary>
    public Texture Texture { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public DrawArea(int width, int height)
    {
        Texture = new(DX.MakeScreen(width, height, DX.TRUE));
    }

    /// <summary>
    /// 描画領域に描画する
    /// </summary>
    /// <param name="draw"></param>
    public void DrawingArea(Action draw)
    {
        int nowScreen = DX.GetDrawScreen();

        DX.SetDrawScreen(Texture.GetGraphHandle());
        DX.ClearDrawScreen();
        draw();
        DX.SetDrawScreen(nowScreen);
    }

    public int GetGraphHandle()
        => Texture.GetGraphHandle();

    public Texture GetTexture()
        => Texture;
}
