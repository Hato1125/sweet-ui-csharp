using System.Drawing;
using DxLibDLL;

namespace Sweet.Graphics;

public class Texture : ITexturable, IDisposable
{
    private int _graphHandle;
    private int _bufGraphHandle;
    private float _bufWidthScale;
    private float _bufHeightScale;
    private bool _isReCalculate;
    private HorizontalReferencePosition _bufHorizontal;
    private VerticalReferencePosition _bufVertical;

    /// <summary>
    /// 画像の横幅
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// 画像の高さ
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// 横幅のスケール
    /// </summary>
    public float WidthScale { get; set; }

    /// <summary>
    /// 高さのスケール
    /// </summary>
    public float HeightScale { get; set; }

    /// <summary>
    /// 回転率
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// ブレンド率
    /// </summary>
    public byte BlendParam { get; set; }

    /// <summary>
    /// 描画基準点の水平方向の位置
    /// </summary>
    public HorizontalReferencePosition HorizontalReferencePosition { get; set; }

    /// <summary>
    /// 描画基準点の垂直方向の位置
    /// </summary>
    public VerticalReferencePosition VerticalReferencePosition { get; set; }

    /// <summary>
    /// 描画基準点の水平方向のオフセット
    /// </summary>
    public int HorizontalReferenceOffset { get; set; }

    /// <summary>
    /// 描画基準点の垂直方向のオフセット
    /// </summary>
    public int VerticalReferenceOffset { get; set; }

    /// <summary>
    /// 適応されている描画基準点の水平方向の座標
    /// </summary>
    public int AdaptHorizontalReferencePoint { get; private set; }

    /// <summary>
    /// 適応されている描画基準点の垂直方向の座標
    /// </summary>
    public int AdaptVerticalReferencePoint { get; private set; }

    /// <summary>
    /// 画像のブレンドモード
    /// </summary>
    public BlendMode BlendMode { get; set; }

    /// <summary>
    /// 画像の描画モード
    /// </summary>
    public DrawMode DrawMode { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    public Texture()
    {
        WidthScale = 1.0f;
        HeightScale = 1.0f;
        BlendParam = 255;
        HorizontalReferencePosition = HorizontalReferencePosition.Left;
        VerticalReferencePosition = VerticalReferencePosition.Top;
        BlendMode = BlendMode.Alpha;
        DrawMode = DrawMode.Auto;
    }

    ~Texture() => Dispose();

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="path">パス</param>
    public Texture(string path) : this()
    {
        _graphHandle = DX.LoadGraph(path);

        if (_graphHandle == -1)
            throw new FileLoadException("Failed to read the file.");
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="gHandle">グラフィックハンドル</param>
    public Texture(int gHandle) : this()
    {
        _graphHandle = gHandle;

        if (_graphHandle == -1)
            throw new Exception("Not a valid handle.");
    }

    /// <summary>
    /// 画像を描画する
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <param name="rectangle">レクタングル</param>
    public void Draw(float x, float y, Rectangle? rectangle = null)
    {
        if (rectangle == null)
            rectangle = new(0, 0, Width, Height);

        WatchChangeGraphHandle();
        WatchChangeReferencePosition();
        SetDrawMode();
        DX.SetDrawBlendMode((int)BlendMode, BlendParam);

        DX.DrawRectRotaGraph3F(
            x, y,
            rectangle.Value.X,
            rectangle.Value.Y,
            rectangle.Value.Width,
            rectangle.Value.Height,
            AdaptHorizontalReferencePoint,
            AdaptVerticalReferencePoint,
            WidthScale,
            HeightScale,
            Rotation,
            _graphHandle,
            DX.TRUE
        );

        DX.SetDrawBlendMode((int)BlendMode.None, 255);
        DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST);
    }

    /// <summary>
    /// 画像の破棄をする
    /// </summary>
    public void Dispose()
    {
        if (_graphHandle == -1)
            return;

        DX.DeleteGraph(_graphHandle);
        GC.SuppressFinalize(this);
    }
    
    public int GetGraphHandle()
        => _graphHandle;

    public Texture GetTexture()
        => this;

    /// <summary>
    /// 描画モードをセットする
    /// </summary>
    private void SetDrawMode()
    {
        switch (DrawMode)
        {
            case DrawMode.Nearest:
                DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST);
                break;

            case DrawMode.Bilinear:
                DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);
                break;

            case DrawMode.Auto:
                if (WidthScale > 1.0f || HeightScale > 1.0f)
                    DX.SetDrawMode(DX.DX_DRAWMODE_NEAREST);
                else
                    DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);
                break;
        }
    }

    /// <summary>
    /// グラフィックハンドルの監視をする
    /// </summary>
    private void WatchChangeGraphHandle()
    {
        if (_bufGraphHandle != _graphHandle)
        {
            DX.GetGraphSize(_graphHandle, out int w, out int h);
            Width = w;
            Height = h;

            _bufGraphHandle = _graphHandle;
            _isReCalculate = true;
        }
    }

    /// <summary>
    /// ReferencePositionの監視をする
    /// </summary>
    private void WatchChangeReferencePosition()
    {
        // 変更されたときのみ計算する
        if (_bufHorizontal != HorizontalReferencePosition
            || _bufWidthScale != WidthScale
            || _isReCalculate)
        {
            AdaptHorizontalReferencePoint =
                ReferencePointUtilt.CalculateHorizontalReferencePoint(
                    HorizontalReferencePosition,
                    (int)(Width * WidthScale),
                    HorizontalReferenceOffset
                );

            _bufHorizontal = HorizontalReferencePosition;
            _bufWidthScale = WidthScale;
        }

        if (_bufVertical != VerticalReferencePosition
            || _bufHeightScale != HeightScale
            || _isReCalculate)
        {
            AdaptVerticalReferencePoint =
                ReferencePointUtilt.CalculateVerticalReferencePoint(
                    VerticalReferencePosition,
                    (int)(Height * HeightScale),
                    VerticalReferenceOffset
                );

            _bufVertical = VerticalReferencePosition;
            _bufHeightScale = HeightScale;
        }

        if (_isReCalculate)
            _isReCalculate = false;
    }
}