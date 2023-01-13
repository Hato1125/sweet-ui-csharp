using Sweet.Elements;

namespace Sweet.Controls;

public class HStackPanel : UIView
{
    /// <summary>
    /// UIとUIの間隔
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    /// スタックの位置
    /// </summary>
    public HorizontalAlignment StackHorizontalAlignment { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public HStackPanel(int width, int height) : base(width, height)
    {
        Interval = 10;
        StackHorizontalAlignment = HorizontalAlignment.Center;
    }

    protected override void UpdateChildPosition()
    {
        base.UpdateChildPosition();

        int offset = CalOffset();
        int hpos = 0;

        for (int i = 0; i < Children.Count(); i++)
        {
            if (IsUIView())
            {
                var child = (UIView)Children[i];

                child.HorizontalAlignment = HorizontalAlignment.Left;
                child.HorizontalOffset = 0;
                child.X = hpos + offset;
                hpos += child.Width + Interval;
            }
        }
    }

    /// <summary>
    /// オフセットを計算
    /// </summary>
    private int CalOffset()
    {
        int targetWidth = 0;
        foreach(var item in Children)
            targetWidth += item.Width + Interval;

        // 一つ間隔分引く
        targetWidth -= Interval;

        return StackHorizontalAlignment switch
        {
            HorizontalAlignment.Left => 0,
            HorizontalAlignment.Center => (Width - targetWidth) / 2,
            HorizontalAlignment.Right => Width - targetWidth,
            _ => 0
        };
    }
}