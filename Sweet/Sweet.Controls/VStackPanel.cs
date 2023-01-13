using Sweet.Elements;

namespace Sweet.Controls;

public class VStackPanel : UIView
{
    /// <summary>
    /// UIとUIの間隔
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    /// スタックの位置
    /// </summary>
    public VerticalAlignment StackVerticalAlignment { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public VStackPanel(int width, int height) : base(width, height)
    {
        Interval = 10;
        StackVerticalAlignment = VerticalAlignment.Center;
    }

    protected override void UpdateChildPosition()
    {
        base.UpdateChildPosition();

        int offset = CalOffset();
        int vpos = 0;

        for (int i = 0; i < Children.Count(); i++)
        {
            if (IsUIView())
            {
                var child = (UIView)Children[i];

                child.VerticalAlignment = VerticalAlignment.Top;
                child.VerticalOffset = 0;
                child.Y = vpos + offset;
                vpos += child.Height + Interval;
            }
        }
    }

    /// <summary>
    /// オフセットを計算
    /// </summary>
    private int CalOffset()
    {
        int targetHeight = 0;
        foreach(var item in Children)
            targetHeight += item.Height + Interval;

        // 一つ間隔分引く
        targetHeight -= Interval;

        return StackVerticalAlignment switch
        {
            VerticalAlignment.Top => 0,
            VerticalAlignment.Center => (Height - targetHeight) / 2,
            VerticalAlignment.Bottom => Height - targetHeight,
            _ => 0
        };
    }
}