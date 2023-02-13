namespace Sweet.Graphics;

public interface ITexturable
{
    /// <summary>
    /// グラフィックハンドルを取得する
    /// </summary>
    int GetGraphHandle();

    /// <summary>
    /// テクスチャを取得する
    /// </summary>
    Texture GetTexture();
}
