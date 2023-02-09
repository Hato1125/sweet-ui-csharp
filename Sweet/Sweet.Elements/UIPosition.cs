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
    /// 位置を計算する
    /// </summary>
    /// <param name="horizontal">水平方向の位置</param>
    /// <param name="vertical">垂直方向の位置</param>
    /// <param name="parentWidth">親要素の横幅</param>
    /// <param name="parentHeight">親要素の高さ</param>
    /// <param name="tergetWidth">ターゲットの横幅</param>
    /// <param name="tergetHeight">ターゲットの高さ</param>
    public static (int X, int Y) CalculatePosition(
        HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        int parentWidth,
        int parentHeight,
        int tergetWidth,
        int tergetHeight,
        int horizontalOffset = 0,
        int verticalOffset = 0
    ) =>
    (
        X: CalculateHorizontalPosition(horizontal, parentWidth, tergetWidth) + horizontalOffset,
        Y: CalculateVerticalPosition(vertical, parentHeight, tergetHeight) + verticalOffset
    );

    /// <summary>
    /// 水平方向の位置を計算する
    /// </summary>
    /// <param name="position">水平方向の位置</param>
    /// <param name="parentWidth">親要素の横幅</param>
    /// <param name="tergetWidth">ターゲットの横幅</param>
    public static int CalculateHorizontalPosition(HorizontalAlignment position, int parentWidth, int tergetWidth) => position switch
    {
        HorizontalAlignment.Left => 0,
        HorizontalAlignment.Center => (parentWidth - tergetWidth) / 2,
        HorizontalAlignment.Right => parentWidth - tergetWidth,
        _ => 0
    };

    /// <summary>
    /// 垂直方向の位置を計算する
    /// </summary>
    /// <param name="position">垂直方向の位置</param>
    /// <param name="parentHeight">親要素の高さ</param>
    /// <param name="tergetHeight">ターゲットの高さ</param>
    public static int CalculateVerticalPosition(VerticalAlignment position, int parentHeight, int tergetHeight) => position switch
    {
        VerticalAlignment.Top => 0,
        VerticalAlignment.Center => (parentHeight - tergetHeight) / 2,
        VerticalAlignment.Bottom => parentHeight - tergetHeight,
        _ => 0
    };
}