using DxLibDLL;

namespace Sweet.Input;

public static class Joypad
{
    private static readonly sbyte[] value = new sbyte[14];

    /// <summary>
    /// 入力を取得するパッド
    /// </summary>
    public static JoypadInputType InputType { get; set; } = JoypadInputType.KeyPad;

    /// <summary>
    /// ジョイパッドが接続されている数
    /// </summary>
    public static int JoypadNumber { get; private set; }

    /// <summary>
    /// 更新する
    /// </summary>
    public static void Update()
    {
        JoypadNumber = DX.GetJoypadNum();

        for (int i = 0; i < value.Length; i++)
        {
            if (DX.GetJoypadInputState((int)InputType) == (int)GetJoypadKey(i))
                value[i] = (sbyte)(IsPushing(GetJoypadKey(i)) ? 2 : 1);
            else
                value[i] = (sbyte)(IsPushing(GetJoypadKey(i)) ? -1 : 0);
        }
    }

    /// <summary>
    /// 押している間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns>押された: True</returns>
    public static bool IsPushing(JoypadKey keyCode)
        => value[GetJoypadKeyIndex(keyCode)] > 0;

    /// <summary>
    /// 推した瞬間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns>押された: True</returns>
    public static bool IsPushed(JoypadKey keyCode)
        => value[GetJoypadKeyIndex(keyCode)] == 1;

    /// <summary>
    /// 離した瞬間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns>離された: True</returns>
    public static bool IsSeparate(JoypadKey keyCode)
        => value[GetJoypadKeyIndex(keyCode)] == -1;

    private static JoypadKey GetJoypadKey(int index) => index switch
    {
        0 => JoypadKey.Left,
        1 => JoypadKey.Right,
        2 => JoypadKey.Up,
        3 => JoypadKey.Down,
        4 => JoypadKey.Input_1,
        5 => JoypadKey.Input_2,
        6 => JoypadKey.Input_3,
        7 => JoypadKey.Input_4,
        8 => JoypadKey.Input_5,
        9 => JoypadKey.Input_6,
        10 => JoypadKey.Input_7,
        11 => JoypadKey.Input_8,
        12 => JoypadKey.Input_9,
        13 => JoypadKey.Input_10,
        _ => JoypadKey.Left
    };

    private static int GetJoypadKeyIndex(JoypadKey key) => key switch
    {
        JoypadKey.Left => 0,
        JoypadKey.Right => 1,
        JoypadKey.Up => 2,
        JoypadKey.Down => 3,
        JoypadKey.Input_1 => 4,
        JoypadKey.Input_2 => 5,
        JoypadKey.Input_3 => 6,
        JoypadKey.Input_4 => 7,
        JoypadKey.Input_5 => 8,
        JoypadKey.Input_6 => 9,
        JoypadKey.Input_7 => 10,
        JoypadKey.Input_8 => 11,
        JoypadKey.Input_9 => 12,
        JoypadKey.Input_10 => 13,
        _ => 0
    };
}

/// <summary>
/// ジョイパッド入力の列挙型
/// </summary>
public enum JoypadInputType
{
    KeyPad = DX.DX_INPUT_KEY_PAD1,
    Pad_1 = DX.DX_INPUT_PAD1,
    Pad_2 = DX.DX_INPUT_PAD2,
    Pad_3 = DX.DX_INPUT_PAD3,
    Pad_4 = DX.DX_INPUT_PAD4,
    Key = DX.DX_INPUT_KEY
}

/// <summary>
/// ジョイパッドのキーコード
/// </summary>
public enum JoypadKey
{
    Left = DX.PAD_INPUT_LEFT,
    Right = DX.PAD_INPUT_RIGHT,
    Up = DX.PAD_INPUT_UP,
    Down = DX.PAD_INPUT_DOWN,
    Input_1 = DX.PAD_INPUT_1,
    Input_2 = DX.PAD_INPUT_2,
    Input_3 = DX.PAD_INPUT_3,
    Input_4 = DX.PAD_INPUT_4,
    Input_5 = DX.PAD_INPUT_5,
    Input_6 = DX.PAD_INPUT_6,
    Input_7 = DX.PAD_INPUT_7,
    Input_8 = DX.PAD_INPUT_8,
    Input_9 = DX.PAD_INPUT_9,
    Input_10 = DX.PAD_INPUT_10,
}