namespace Sweet.Controls;

public class UILabel : UITextBaseControl
{
    /// <summary>
    /// テキスト
    /// </summary>
    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    protected UITextStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUITextStyle Style
    {
        get => (IUITextStyle)_style;
        set => _style = (UITextStyle)value;
    }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public UILabel(int width, int height, string fontName, int fontSize, int fontThick)
        : base(width, height, fontName, fontSize, fontThick)
    {
        Style.FontName = fontName;
        Style.FontSize = fontSize;
        Style.FontThick = fontThick;
    }

    public override void Update()
    {
        _style.Control = this;
        _style.StyleAdapt();
        base.Update();

        _text.FontHandle = FontHandle;
        _text.ParentWidth = Width;
        _text.ParentHeight = Height;
        _text.UpdateText();
    }

    protected override void DrawViewArea()
    {
        base.DrawViewArea();
        _text.DrawText();
    }
}