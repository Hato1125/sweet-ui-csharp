using DxLibDLL;

namespace Sweet.Input;

public class TouchInfo
{
    private sbyte value;

    /// <summary>
    /// 調べるタッチ番号
    /// </summary>
    public int TouchNumber { get; init; }

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="touchNum">タッチ番号</param>
    public TouchInfo(int touchNum)
        => TouchNumber = touchNum;

    /// <summary>
    /// 更新する
    /// </summary>
    public void Update()
    {
        if(DX.GetTouchInputNum() >= TouchNumber)
        {
            if (!IsPushing())
                value = 1;
            else
                value = 2;
        }
        else
        {
            if (IsPushing())
                value = -1;
            else
                value = 0;
        }
    }

    /// <summary>
    /// タッチされいてる間を取得する
    /// </summary>
    /// <returns>タップされている: True</returns>
    public bool IsPushing()
        => value > 0;

    /// <summary>
    /// タッチした瞬間を取得する
    /// </summary>
    /// <returns>タップされている: True</returns>
    public bool IsPushed()
        => value == 1;

    /// <summary>
    /// 離した瞬間を取得する
    /// </summary>
    /// <returns>離された: True</returns>
    public bool IsSeparate()
        => value == -1;
}