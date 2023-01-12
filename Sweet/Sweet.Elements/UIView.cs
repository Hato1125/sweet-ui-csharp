using System.Drawing;
using DxLibDLL;

namespace Sweet.Elements;

public class UIView : UIResponder, IUIDisposable
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
    /// 親要素の横幅
    /// </summary>
    public int ParentWidth { get; set; }

    /// <summary>
    /// 親要素の高さ
    /// </summary>
    public int ParentHeight { get; set; }

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
    /// 水平方向のオフセット
    /// </summary>
    public int HorizontalOffset { get; set; }

    /// <summary>
    /// 垂直方向のオフセット
    /// </summary>
    public int VerticalOffset { get; set; }

    /// <summary>
    /// 親要素に対しての水平方向の位置
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; }

    /// <summary>
    /// 親要素に対しての垂直方向の位置
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; set; }

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
        BorderSize = 1.0f;
        BackgroundColor = Color.FromArgb(254, 252, 255);
        BorderColor = Color.FromArgb(240, 240, 240);
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
    }

    ~UIView() => Dispose(true);

    /// <summary>
    /// ビルドする
    /// </summary>
    protected virtual void Build()
    {
        if (!IsBuild)
            return;

        Dispose(false);
        _viewHandle = DX.MakeScreen(Width, Height, DX.TRUE);
        _maskHandle = DX.MakeScreen(Width, Height, DX.TRUE);

        IsBuild = false;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public override void Update()
    {
        if (!IsBuild)
            IsBuild = WatchBuild();
        Build();

        UpdatePosition();
        base.Update();
        UpdateRenderProperty();
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
    public virtual void Dispose(bool isChildDispoes = true)
    {
        if (_viewHandle != -1 && _viewHandle != 0)
            DX.DeleteGraph(_viewHandle);

        if (_maskHandle != -1 && _maskHandle != 0)
            DX.DeleteGraph(_maskHandle);

        if (isChildDispoes)
        {
            foreach (var item in Children)
            {
                if (IsUIView())
                {
                    var child = (UIView)item;
                    child.Dispose(true);
                }
            }

            Children.Clear();
        }
    }

    /// <summary>
    /// UIをビルドすべきかを判断する
    /// </summary>
    protected virtual bool WatchBuild()
    {
        if (bufWidth != Width || bufHeight != Height)
        {
            bufWidth = Width;
            bufHeight = Height;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// UIの位置の更新
    /// </summary>
    protected virtual void UpdatePosition()
    {
        SetChildParentSize();
        SetUIPosition();
    }

    /// <summary>
    /// レンダリング前のレンダリング用プロパティの更新
    /// </summary>
    protected virtual void UpdateRenderProperty()
    {
        RoundCornerRadius();
    }

    /// <summary>
    /// 角の半径を調整する
    /// </summary>
    private void RoundCornerRadius()
    {
        if (!IsRoundRadius)
            return;

        if (BackgroundCornerRadius > Height / 2.0f)
            BackgroundCornerRadius = Height / 2.0f;
    }

    /// <summary>
    /// UIの位置をセットする
    /// </summary>
    private void SetUIPosition()
    {
        var pos = UIPositionUtilt.CalUIPosition(
            HorizontalAlignment,
            VerticalAlignment,
            HorizontalOffset,
            VerticalOffset,
            ParentWidth,
            ParentHeight,
            Width,
            Height
        );

        (X, Y) = pos;
    }

    /// <summary>
    /// 子要素に親要素のサイズをセットする
    /// </summary>
    private void SetChildParentSize()
    {
        foreach (var item in Children)
        {
            // UIResponderは位置指定機能はないので
            // UIResponder以外の子要素の親サイズを更新
            if (IsUIView())
            {
                var child = (UIView)item;
                child.ParentWidth = Width;
                child.ParentHeight = Height;
            }
        }
    }

    /// <summary>
    /// 矩形をレンダリングする
    /// </summary>
    private void RenderRect()
    {
        var renderMask = () =>
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
        var renderRect = () =>
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
            if (IsUIView())
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
    /// このインスタンスがUIViewかどうかを取得する
    /// </summary>
    protected bool IsUIView()
        => this.GetType() != typeof(UIResponder);
}