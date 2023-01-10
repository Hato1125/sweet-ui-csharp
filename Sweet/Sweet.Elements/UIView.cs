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
    /// ボーダーの透明度
    /// </summary>
    public int BorderAlpha { get; set; }

    /// <summary>
    /// 背景の角の半径
    /// </summary>
    public float BackgroundCornerRadius { get; set; }

    /// <summary>
    /// ボーダーのサイズ
    /// </summary>
    public float BorderSize { get; set; }

    /// <summary>
    /// 背景の色
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// ボーダーの色
    /// </summary>
    public Color BorderColor { get; set; }

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
        BorderAlpha = 255;
        BackgroundCornerRadius = 35.0f;
        BorderSize = 2.0f;
        BackgroundColor = Color.White;
        BorderColor = Color.Gray;
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
        RenderBorder();
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
        Action renderMask = () =>
        {
            RenderMask();
            RenderChildren();
        };

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);

        // 領域にレンダリング
        RenderViewArea(_viewHandle, renderMask);

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 255);

        // viewをレンダリング
        DX.DrawGraph(X, Y, _viewHandle, DX.TRUE);
    }

    /// <summary>
    /// マスクにレンダリングする
    /// </summary>
    private void RenderMask()
    {
        // マスクの内容
        Action renderRect = () =>
        {
            if (BackgroundCornerRadius <= 0)
            {
                DX.DrawFillBox(
                    0,
                    0,
                    Width,
                    Height,
                    0xffffff
                );
            }
            else
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
            }

            DX.SetMaskScreenGraph(_maskHandle);
        };

        DX.SetMaskReverseEffectFlag(DX.TRUE);
        RenderViewArea(_maskHandle, renderRect);

        // マスクを有効にする
        DX.SetUseMaskScreenFlag(DX.TRUE);

        uint backColor = DX.GetColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B);

        // 背景をレンダリング
        if (BackgroundColor != Color.Empty && BackgroundAlpha > 0)
        {
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, BackgroundAlpha);
            DX.DrawFillBox(0, 0, Width, Height, backColor);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
        }

        Rendering?.Invoke();
        OnRendering?.Invoke();

        DX.SetUseMaskScreenFlag(DX.FALSE);
        DX.SetMaskReverseEffectFlag(DX.FALSE);
    }

    /// <summary>
    /// ボーダーをレンダリングする
    /// </summary>
    private void RenderBorder()
    {
        if (BorderColor == Color.Empty || BorderAlpha <= 0)
            return;

        uint borderColor = DX.GetColor(BorderColor.R, BorderColor.G, BorderColor.B);

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, BorderAlpha);

        if (BackgroundCornerRadius <= 0)
        {
            DX.DrawBoxAA(
                X,
                Y,
                X + Width,
                Y + Height,
                borderColor,
                DX.FALSE,
                BorderSize * 2
            );
        }
        else
        {
            DX.DrawRoundRectAA(
                X,
                Y,
                X + Width,
                Y + Height,
                BackgroundCornerRadius,
                BackgroundCornerRadius,
                byte.MaxValue,
                borderColor,
                DX.FALSE,
                BorderSize * 2
            );
        }

        DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
    }

    /// <summary>
    /// 子要素をレンダリングする
    /// </summary>
    private void RenderChildren()
    {
        foreach (var item in Children)
        {
            // UIResponderはレンダリング機能はないので
            // UIResponder以外の子要素をレンダリング
            if (this.GetType() != typeof(UIResponder))
            {
                var child = (UIView)item;
                child.Render();
            }
        }
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