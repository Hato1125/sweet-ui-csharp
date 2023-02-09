namespace Sweet.Graphics;

/// <summary>
/// 水平方向の描画基準点
/// </summary>
public enum HorizontalReferencePosition
{
    Left,
    Center,
    Right,
}

/// <summary>
/// 垂直方向の描画基準点
/// </summary>
public enum VerticalReferencePosition
{
    Top,
    Center,
    Bottom,
}

public static class ReferencePointUtilt
{
    /// <summary>
    /// 水平方向の基準点を計算する
    /// </summary>
    /// <param name="horizontal">水平方向の位置</param>
    /// <param name="tergetWidth">ターゲットの横幅</param>
    /// <param name="offset">オフセット</param>
    public static int CalculateHorizontalReferencePoint(HorizontalReferencePosition horizontal, int tergetWidth, int offset) => horizontal switch
    {
        HorizontalReferencePosition.Left => offset,
        HorizontalReferencePosition.Center => tergetWidth / 2 + offset,
        HorizontalReferencePosition.Right => tergetWidth + offset,
        _ => offset
    };

    /// <summary>
    /// 垂直方向の基準点を計算する
    /// </summary>
    /// <param name="vertical">垂直方向の位置</param>
    /// <param name="tergetWidth">ターゲットの横幅</param>
    /// <param name="offset">オフセット</param>
    public static int CalculateVerticalReferencePoint(VerticalReferencePosition vertical, int tergetHeight, int offset) => vertical switch
    {
        VerticalReferencePosition.Top => offset,
        VerticalReferencePosition.Center => tergetHeight / 2 + offset,
        VerticalReferencePosition.Bottom => tergetHeight + offset,
        _ => offset
    };
}