namespace Sweet.Elements;

public class UIControl : UIView
{
    /// <summary>
    /// 水平方向の位置
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; }

    /// <summary>
    /// 垂直方向の位置
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; set; }

    /// <summary>
    /// 水平方向のオフセット
    /// </summary>
    public int HorizontalOffset { get; set; }

    /// <summary>
    /// 垂直方向のオフセット
    /// </summary>
    public int VerticalOffset { get; set; }

    /// <summary>
    /// UIの状態
    /// </summary>
    public UIState State
    {
        get
        {
            if (IsInput)
                return UIState.Enable;
            else
                return UIState.Disable;
        }
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIControl(int width, int height)
        : base(width, height)
    {
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Center;
    }

    public override void Update()
    {
        CalculatePosition();
        base.Update();
    }

    /// <summary>
    /// UIの位置を計算する
    /// </summary>
    protected virtual void CalculatePosition()
    {
        var pos = UIPositionUtilt.CalculateUIPosition(
            HorizontalAlignment,
            VerticalAlignment,
            HorizontalOffset,
            VerticalOffset,
            ParentWidth,
            ParentHeight,
            Width,
            Height
        );

        X = pos.X;
        Y = pos.Y;
    }
}