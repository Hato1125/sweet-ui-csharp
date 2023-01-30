using System.Drawing;

namespace Sweet.Controls;

public record UIToggleSwitchStyle : UIButtonStyle, IUIToggleSwitchStyle
{
    public Color ToggleColor { get; set; }
    public byte ToggleAlpha { get; set; }
    public int TogglePadding { get; set; }

    public UIToggleSwitchStyle() : base()
    {
        ToggleColor = Color.Black;
        ToggleAlpha = 255;
        TogglePadding = 5;
    }
}