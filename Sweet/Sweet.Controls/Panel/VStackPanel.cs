using Sweet.Elements;

namespace Sweet.Controls;

public class VStackPanel : UIControl
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
    /// スタックの水平方向の位置
    /// </summary>
    public VerticalAlignment StackVerticalAlignment { get; set; }

    /// <summary>
    /// 水平方向のオフセット
    /// </summary>
    public int StackHorizontalOffset { get; set; }

    /// <summary>
    /// スタックの間隔
    /// </summary>
    public int StackInterval { get; set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public VStackPanel(int width, int height)
        : base(width, height)
    {
        StackVerticalAlignment = VerticalAlignment.Center;
        StackInterval = 10;
    }

    public override void Update()
    {
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();
    }

    protected override void UpdateChildren()
    {
        base.UpdateChildren();

        if (Children.Count > 0)
        {
            // UIの横幅と間隔の合計を計算
            int height = Children.Aggregate(0, (prev, current) => prev + current.Height + StackInterval);

            height -= StackInterval;

            int posY = UIPositionUtilt.CalculateBeginPosition(this.Height, height, StackVerticalAlignment);

            // 配置する
            foreach (var item in Children)
            {
                if (item.GetType() != typeof(UIView) && item.GetType() != typeof(UIResponder))
                {
                    var child = (UIControl)item;
                    child.VerticalAlignment = VerticalAlignment.Top;
                }

                item.Y = posY;
                posY += item.Height + StackInterval;
            }
        }
    }
}