using System.Drawing;
using DxLibDLL;

namespace Sweet.Elements;

public class UIView : UIResponder, IDisposable
{
    private (int Width, int Height) _bufSize;
    protected int ViewHandle { get; set; }

    /// <summary>
    /// 透明度
    /// </summary>
    public byte Alpha { get; set; }

    /// <summary>
    /// 背景の角の半径
    /// </summary>
    public float Radius { get; set; }

    /// <summary>
    /// 表示するか
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// 背景の透明度
    /// </summary>
    public byte BackgroundAlpha { get; set; }

    /// <summary>
    /// 背景の色
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// レンダリング時に呼ばれる
    /// </summary>
    public Action? OnRendering { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIView(int width, int height)
        : base(width, height)
    {
        Alpha = 255;
        Radius = 8;
        IsVisible = true;
        BackgroundColor = Color.White;
        BackgroundAlpha = 255;
    }

    public override void Update()
    {
        ViewSizeUpdate();
        base.Update();
    }

    /// <summary>
    /// UIを描画する
    /// </summary>
    public virtual void DrawView()
    {
        int nowScreen = DX.GetDrawScreen();
        DX.SetDrawScreen(ViewHandle);
        DX.ClearDrawScreen();
        DrawViewArea();
        DrawChildren();
        OnRendering?.Invoke();
        DX.SetDrawScreen(nowScreen);

        if (IsVisible)
        {
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, Alpha);
            DX.DrawGraph(X, Y, ViewHandle, DX.TRUE);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
        }
    }

    /// <summary>
    /// UIを破棄する
    /// </summary>
    public virtual void Dispose()
    {
        Tracer.Log("Dispose.");

        if (ViewHandle != -1)
            DX.DeleteGraph(ViewHandle);

        foreach(var item in Children)
        {
            if(item.GetType() != typeof(UIResponder))
            {
                ((UIView)item).Dispose();
            }
        }
    }

    /// <summary>
    /// 領域内に描画する
    /// </summary>
    protected virtual void DrawViewArea()
    {
        uint backColor = DX.GetColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B);

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, BackgroundAlpha);
        if (Radius <= 0)
        {
            DX.DrawFillBox(0, 0, Width, Height, backColor);
        }
        else
        {
            DX.DrawRoundRectAA(0, 0, Width, Height, Radius, Radius, 100, backColor, DX.TRUE);
        }
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
    }

    /// <summary>
    /// 子要素を描画する
    /// </summary>
    private void DrawChildren()
    {
        foreach (var item in Children)
        {
            if (item.GetType() != typeof(UIResponder))
            {
                ((UIView)item).DrawView();
            }
        }
    }

    /// <summary>
    /// サイズを更新する
    /// </summary>
    private void ViewSizeUpdate()
    {
        if (Width == _bufSize.Width && Height == _bufSize.Height)
            return;

        Tracer.Log("SizeUpdate.");

        if (ViewHandle != -1)
            DX.DeleteGraph(ViewHandle);

        ViewHandle = DX.MakeScreen(Width, Height, DX.TRUE);

        // 次のフレームで再生成しないようにセットしとく
        _bufSize = (Width, Height);
    }
}