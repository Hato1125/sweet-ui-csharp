using Sweet.Elements;
using System.Drawing;
using DxLibDLL;

namespace Sweet.Controls;

public class VRadioButton
{
    public VStackPanel Stack;

    protected UIToggleSwitchStyle _style = new();

    /// <summary>
    /// スタイル
    /// </summary>
    public IUIToggleSwitchStyle Style
    {
        get => (IUIToggleSwitchStyle)_style;
        set => _style = (UIToggleSwitchStyle)value;
    }

    /// <summary>
    /// 選択されているインデックス
    /// </summary>
    public int OnIndex { get; private set; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public VRadioButton(int width, int height)
    {
        Stack = new(width, height);
        Stack.StackInterval = 20;
    }

    /// <summary>
    /// ラジオボタンを追加する
    /// </summary>
    /// <param name="text">テキスト</param>
    /// <param name="size">サイズ</param>
    /// <param name="fontName">フォント名</param>
    /// <param name="fontSize">フォントサイズ</param>
    /// <param name="fontThick">フォントの太さ</param>
    public void AddRadioButton(string text, int size = 30, string fontName = "Segoe UI", int fontSize = 15, int fontThick = 0)
    {
        Stack.Children.Add(new RadioButton(size, fontName, fontSize, fontThick) { Text = text });
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public void Update()
    {
        Stack.Update();
        Select();
    }

    /// <summary>
    /// 描画する
    /// </summary>
    public void DrawView()
    {
        Stack.DrawView();
    }

    /// <summary>
    /// 選択する
    /// </summary>
    private void Select()
    {
        for (int i = 0; i < Stack.Children.Count; i++)
        {
            if (Stack.Children[i] is UIButtonBase)
            {
                var child = (RadioButton)Stack.Children[i];
                child.Style = Style;

                bool on = child.IsSeparate();

                // 押されているボタンのインデックスを取得
                if (on && i != OnIndex)
                    OnIndex = i;
            }
        }

        if (OnIndex != -1)
        {
            for (int j = 0; j < Stack.Children.Count; j++)
            {
                var schild = (RadioButton)Stack.Children[j];

                // 押されたインデックス以外のボタンはoffにする
                if (OnIndex != j)
                    schild.IsOn = false;
                else
                    schild.IsOn = true;
            }
        }
    }

    private class RadioButton : UIButtonBase
    {
        protected UIToggleSwitchStyle _style = new();

        /// <summary>
        /// スタイル
        /// </summary>
        public IUIToggleSwitchStyle Style
        {
            get => (IUIToggleSwitchStyle)_style;
            set => _style = (UIToggleSwitchStyle)value;
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
        /// オンか
        /// </summary>
        public bool IsOn { get; set; }

        public RadioButton(int size, string fontName, int fontSize, int fontThick)
            : base(size, size, fontName, fontSize, fontThick)
        {
            Style.FontName = fontName;
            Style.FontSize = fontSize;
            Style.FontThick = fontThick;
            Style.ClickColor = Color.FromArgb(197, 112, 238);
            Style.Radius = (size / 2) - 0.5f;
            Style.ForeColor = Color.White;
            Style.AnimeSpeed = 700;
            TextContent.HorizontalAlignment = HorizontalAlignment.Left;
            TextContent.HorizontalOffset = 10;
            Text = "ToggleSwitch";
        }

        public override void Update()
        {
            _style.Control = this;
            _style.StyleAdapt();
            base.Update();

            if (IsSeparate())
                IsOn = !IsOn;

            _text.FontHandle = FontHandle;
            _text.ParentWidth = Width;
            _text.ParentHeight = Height;
            _text.UpdateText();

            IsTickAnimation = IsOn;
        }

        public override void DrawView()
        {
            base.DrawView();

            (_text.X, _text.Y) = (X + Width, Y);
            _text.DrawText();
        }

        protected override void DrawViewArea()
        {
            base.DrawViewArea();
            DrawFade();
            DrawToggle();
        }

        /// <summary>
        /// フェードの描画
        /// </summary>
        private void DrawFade()
        {
            uint clickColor = DX.GetColor(ClickColor.R, ClickColor.G, ClickColor.B);
            double fade = Math.Sin(AnimeValue * Math.PI / 180) * FadeAlpha;

            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, (int)fade);
            if (Radius <= 0)
            {
                DX.DrawFillBox(0, 0, Width, Height, clickColor);
            }
            else
            {
                DX.DrawRoundRectAA(0, 0, Width, Height, Radius, Radius, 100, clickColor, DX.TRUE);
            }
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
        }

        /// <summary>
        /// トグルの描画
        /// </summary>
        private void DrawToggle()
        {
            uint toggleColor = DX.GetColor(Style.ToggleColor.R, Style.ToggleColor.G, Style.ToggleColor.B);
            double radius = Math.Sin(AnimeValue * Math.PI / 180) * ((Height / 2) - Style.TogglePadding);

            if (radius <= 0)
                return;

            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, (int)Style.ToggleAlpha);
            DX.DrawCircleAA(
                Height / 2,
                Height / 2,
                (float)radius,
                byte.MaxValue,
                toggleColor
            );
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_PMA_ALPHA, 255);
        }
    }
}