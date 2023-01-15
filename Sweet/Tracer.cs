public static class Tracer
{
    /// <summary>
    /// Debugモード時にログをコンソールに出力する
    /// </summary>
    /// <param name="message">メッセージ</param>
    public static void Log(string message)
    {
#if DEBUG
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"[Log] {message}");
        Console.ResetColor();
#endif
    }
}