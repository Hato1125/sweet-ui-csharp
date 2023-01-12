namespace Sweet.Elements;

public interface IUIDisposable
{
    /// <summary>
    /// UIを解放する
    /// </summary>
    /// <param name="isChildDispoes">UIの子要素の解放も行うか</param>
    void Dispose(bool isChildDispoes = true);
}