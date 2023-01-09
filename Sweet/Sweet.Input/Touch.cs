using DxLibDLL;

namespace Sweet.Input;

public static class Touch
{
    /// <summary>
    /// タッチ情報リスト
    /// </summary>
    public static readonly List<TouchInfo> TouchInfos = new(20)
    {
        { new TouchInfo(1) },
        { new TouchInfo(2) },
        { new TouchInfo(3) },
        { new TouchInfo(4) },
        { new TouchInfo(5) },
        { new TouchInfo(6) },
        { new TouchInfo(7) },
        { new TouchInfo(8) },
        { new TouchInfo(9) },
        { new TouchInfo(10) },
    };

    /// <summary>
    /// 一番最後に置いた番号のX座標
    /// </summary>
    public static int X { get; private set; }

    /// <summary>
    /// 一番最後に置いた番号のY座標
    /// </summary>
    public static int Y { get; private set; }

    /// <summary>
    /// 更新する
    /// </summary>
    public static void Update()
    {
        DX.GetTouchInput(DX.GetTouchInputNum() - 1, out int tx, out int ty, out int _, out int _, out float _);
        (X, Y) = (tx, ty);
        foreach (var info in TouchInfos)
            info.Update();
    }

    /// <summary>
    /// タッチされいてる間を取得する
    /// </summary>
    /// <returns>タップされている: True</returns>
    public static bool IsPushing()
    {
        foreach(var info in TouchInfos)
        {
            if (info.IsPushing())
                return true;
        }

        return false;
    }

    /// <summary>
    /// タッチした瞬間を取得する
    /// </summary>
    /// <returns>タップされている: True</returns>
    public static bool IsPushed()
    {
        foreach (var info in TouchInfos)
        {
            if (info.IsPushed())
                return true;
        }

        return false;
    }

    /// <summary>
    /// 離した瞬間を取得する
    /// </summary>
    /// <returns>離された: True</returns>
    public static bool IsSeparate()
    {
        foreach (var info in TouchInfos)
        {
            if (info.IsSeparate())
                return true;
        }

        return false;
    }
}
