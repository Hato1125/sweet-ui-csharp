namespace Sweet.Controls;

public class UILabel : UITextBaseControl
{
    private LineText _text = new();

    /// <summary>
    /// テキストコンテンツ
    /// </summary>
    public ILineText TextContent
    {
        get => (ILineText)_text;
        set => _text = (LineText)value;
    }

    /// <summary>
    /// テキスト
    /// </summary>
    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
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
    }

    public override void Update()
    {
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