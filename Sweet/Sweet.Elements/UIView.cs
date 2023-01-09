using System.Drawing;
using DxLibDLL;

namespace Sweet.Elements;

public class UIView : UIResponder, IDisposable
{
    private int _viewHandle;
    private int _maskHandle;
    private int bufWidth;
    private int bufHeight;

    /// <summary>
    /// レンダリングアクション
    /// </summary>
    protected Action? Rendering { get; set; }

    /// <summary>
    /// ビルドするか
    /// </summary>
    public bool IsBuild { get; set; }

    /// <summary>
    /// Radiusを丸めるか
    /// </summary>
    public bool IsRoundRadius { get; set; }

    /// <summary>
    /// 背景の透明度
    /// </summary>
    public int BackgroundAlpha { get; set; }

    /// <summary>
    /// 背景の角の半径
    /// </summary>
    public float BackgroundCornerRadius { get; set; }

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
    public UIView(int width, int height) : base(width, height)
    {
        IsBuild = true;
        Build();

        IsRoundRadius = true;
        bufWidth = width;
        bufHeight = height;
        BackgroundAlpha = 255;
        BackgroundCornerRadius = 35.0f;
        BackgroundColor = Color.White;
    }

    ~UIView() => Dispose();

    /// <summary>
    /// ビルドする
    /// </summary>
    protected virtual void Build()
    {
        if (!IsBuild)
            return;

        Dispose();
        _viewHandle = DX.MakeScreen(Width, Height, DX.TRUE);
        _maskHandle = DX.MakeScreen(Width, Height, DX.TRUE);

        IsBuild = false;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public override void Update()
    {
        CheckResize();
        Build();

        base.Update();

        RoundRadius();
    }

    /// <summary>
    /// レンダリングする
    /// </summary>
    public virtual void Render()
    {
        RenderRect();
    }

    /// <summary>
    /// 解放する
    /// </summary>
    public virtual void Dispose()
    {
        if (_viewHandle != -1 && _viewHandle != 0)
            DX.DeleteGraph(_viewHandle);

        if (_maskHandle != -1 && _maskHandle != 0)
            DX.DeleteGraph(_maskHandle);
    }

    /// <summary>
    /// 矩形をレンダリングする
    /// </summary>
    private void RenderRect()
    {
        Action renderMask = RenderMask;

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);

        // 領域にレンダリング
        RenderViewArea(_viewHandle, renderMask);

        // viewをレンダリング
        DX.DrawGraph(X, Y, _viewHandle, DX.TRUE);
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 255);
    }

    /// <summary>
    /// マスクにレンダリングする
    /// </summary>
    private void RenderMask()
    {
        // マスクの内容
        Action renderRect = () =>
        {
            DX.DrawRoundRectAA(
                0,
                0,
                Width,
                Height,
                BackgroundCornerRadius,
                BackgroundCornerRadius,
                byte.MaxValue,
                0xffffff,
                DX.TRUE
            );

            DX.SetMaskScreenGraph(_maskHandle);
        };

        DX.SetMaskReverseEffectFlag(DX.TRUE);
        RenderViewArea(_maskHandle, renderRect);

        // マスクを有効にする
        DX.SetUseMaskScreenFlag(DX.TRUE);

        uint backColor = DX.GetColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B);

        // 背景をレンダリング
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, BackgroundAlpha);
        DX.DrawFillBox(0, 0, Width, Height, backColor);
        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);

        Rendering?.Invoke();
        OnRendering?.Invoke();

        DX.SetUseMaskScreenFlag(DX.FALSE);
        DX.SetMaskReverseEffectFlag(DX.FALSE);
    }

    /// <summary>
    /// グラフィックハンドルにレンダリングする
    /// </summary>
    /// <param name="gHandle">グラフィックハンドル</param>
    /// <param name="action">レンダリングアクション</param>
    private void RenderViewArea(int gHandle, Action action)
    {
        int nowScreen = DX.GetDrawScreen();

        DX.SetDrawScreen(gHandle);
        DX.ClearDrawScreen();
        action();
        DX.SetDrawScreen(nowScreen);
    }

    /// <summary>
    /// サイズが変更されたかを監視しビルドさせる
    /// </summary>
    private void CheckResize()
    {
        if (bufWidth != Width || bufHeight != Height)
        {
            IsBuild = true;
            bufWidth = Width;
            bufHeight = Height;
        }
    }

    /// <summary>
    /// Radiusを丸める
    /// </summary>
    private void RoundRadius()
    {
        if (!IsRoundRadius)
            return;

        if (BackgroundCornerRadius > Height / 2.0f)
            BackgroundCornerRadius = Height / 2.0f;
    }
}