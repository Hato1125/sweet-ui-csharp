using Sweet.Elements;

namespace Sweet.Controls;

public class HStackPanel : UIControl
{
    /// <summary>
    /// スタックの水平方向の位置
    /// </summary>
    public HorizontalAlignment StackHorizontalAlignment { get; set; }

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
    public HStackPanel(int width, int height)
        : base(width, height)
    {
        StackHorizontalAlignment = HorizontalAlignment.Center;
        StackInterval = 10;
    }

    protected override void UpdateChildren()
    {
        base.UpdateChildren();

        if (Children.Count() > 0)
        {
            int width = 0;

            // UIの横幅と間隔の合計を計算
            foreach (var item in Children)
                width += item.Width + StackInterval;

            width -= StackInterval;
            int posX = UIPositionUtilt.CalculateBeginPosition(this.Width, width, StackHorizontalAlignment);

            // 配置する
            foreach (var item in Children)
            {
                if (item.GetType() != typeof(UIView) && item.GetType() != typeof(UIResponder))
                {
                    var child = (UIControl)item;
                    child.HorizontalAlignment = HorizontalAlignment.Left;
                }

                item.X = posX;
                posX += item.Width + StackInterval;
            }
        }
    }
}