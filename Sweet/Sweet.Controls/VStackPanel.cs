using Sweet.Elements;

namespace Sweet.Controls;

public class VStackPanel : UIControl
{
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

    protected override void UpdateChildren()
    {
        base.UpdateChildren();

        if (Children.Count() > 0)
        {
            int height = 0;

            // UIの横幅と間隔の合計を計算
            foreach (var item in Children)
                height += item.Height + StackInterval;

            height -= StackInterval;
            int posY = UIPositionUtilt.CalculateBeginPosition(this.Height, height, StackVerticalAlignment);

            // 配置する
            foreach (var item in Children)
            {
                if (item.GetType() == typeof(UIControl))
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