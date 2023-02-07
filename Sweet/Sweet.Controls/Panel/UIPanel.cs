using Sweet.Elements;

namespace Sweet.Controls;

public class UIPanel : UIControl
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
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIPanel(int width, int height)
        : base(width, height)
    {
    }

    public override void Update()
    {
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();
    }
}