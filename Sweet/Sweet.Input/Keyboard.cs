using DxLibDLL;

namespace Sweet.Input;

public static class Keyboard
{
    private static byte[] buffer = new byte[256];
    private static sbyte[] value = new sbyte[256];

    /// <summary>
    /// 更新する
    /// </summary>
    public static void Update()
    {
        DX.GetHitKeyStateAll(buffer);

        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 1)
            {
                if (!IsPushing(i))
                    value[i] = 1;
                else
                    value[i] = 2;
            }
            else
            {
                if (IsPushing(i))
                    value[i] = -1;
                else
                    value[i] = 0;
            }
        }
    }

    /// <summary>
    /// 押している間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns></returns>
    public static bool IsPushing(int keyCode)
        => value[keyCode] > 0;

    /// <summary>
    /// 推した瞬間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns></returns>
    public static bool IsPushed(int keyCode)
        => value[keyCode] == 1;

    /// <summary>
    /// 離した瞬間を取得する
    /// </summary>
    /// <param name="keyCode">キーコード</param>
    /// <returns></returns>
    public static bool IsSeparate(int keyCode)
        => value[keyCode] == -1;
}