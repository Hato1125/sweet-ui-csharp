namespace Sweet.Elements;

/// <summary>
/// 水平方向の位置
/// </summary>
public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
}

/// <summary>
/// 垂直方向の位置
/// </summary>
public enum VerticalAlignment
{
    Top,
    Center,
    Bottom,
}

public static class UIPositionUtilt
{
    /// <summary>
    /// UIの座標を計算する
    /// </summary>
    /// <param name="horizontal">水平方向の位置</param>
    /// <param name="vertical">垂直方向の位置</param>
    /// <param name="horizontalOffset">水平方向のオフセット</param>
    /// <param name="verticalOffset">垂直方向のオフセット</param>
    /// <param name="parentWidth">親要素の横幅</param>
    /// <param name="parentHeight">親要素の高さ</param>
    /// <param name="tergetWidth">ターゲットの横幅</param>
    /// <param name="tergetHeight">ターゲットの高さ</param>
    /// <returns>座標</returns>
    public static (int X, int Y) CalculateUIPosition(
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        int horizontalOffset,
        int verticalOffset,
        int parentWidth,
        int parentHeight,
        int tergetWidth,
        int tergetHeight)
    {
        var result = (X: 0, Y: 0);

        result.X = horizontal switch
        {
            HorizontalAlignment.Left => horizontalOffset,
            HorizontalAlignment.Center => (parentWidth - tergetWidth) / 2 + horizontalOffset,
            HorizontalAlignment.Right => parentWidth - tergetWidth + horizontalOffset,
            _ => horizontalOffset
        };

        result.Y = vertical switch
        {
            VerticalAlignment.Top => verticalOffset,
            VerticalAlignment.Center => (parentHeight - tergetHeight) / 2 + verticalOffset,
            VerticalAlignment.Bottom => parentHeight - parentWidth + verticalOffset,
            _ => verticalOffset
        };

        return result;
    }
}