using Sweet.Elements;

namespace Sweet.Controls;

public class Panel : UIControl
{
    protected UIControlBaseStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIControlBaseStyle Style
    {
        get => (IUIControlBaseStyle)_style;
        set => _style = (UIControlBaseStyle)value;
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public Panel(int width, int height)
        : base(width, height)
    {
    }
}