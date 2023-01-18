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
    public int Alpha { get; set; }

    /// <summary>
    /// 背景の角の半径
    /// </summary>
    public float Radius { get; set; }

    /// <summary>
    /// 表示するか
    /// </summary>
    public bool IsVisible { get; set; }

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
            DX.DrawGraph(Position.X, Position.Y, ViewHandle, DX.TRUE);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
        }
    }

    /// <summary>
    /// UIを破棄する
    /// </summary>
    public virtual void Dispose()
    {
        if (ViewHandle != -1)
            DX.DeleteGraph(ViewHandle);
    }

    /// <summary>
    /// 領域内に描画する
    /// </summary>
    protected virtual void DrawViewArea()
    {
        uint backColor = DX.GetColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B);

        if (Radius <= 0)
        {
            DX.DrawFillBox(0, 0, Size.Width, Size.Height, backColor);
        }
        else
        {
            DX.DrawRoundRectAA(0, 0, Size.Width, Size.Height, Radius, Radius, 100, backColor, DX.TRUE);
        }
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
                var child = (UIView)item;
                child.DrawView();
            }
        }
    }

    /// <summary>
    /// サイズを更新する
    /// </summary>
    private void ViewSizeUpdate()
    {
        if (Size == _bufSize)
            return;

        Tracer.Log("SizeUpdate.");

        if (ViewHandle != -1)
            DX.DeleteGraph(ViewHandle);

        ViewHandle = DX.MakeScreen(Size.Width, Size.Height, DX.TRUE);

        // 次のフレームで再生成しないようにSizeと_bufSizeは同じにする
        _bufSize = Size;
    }
}