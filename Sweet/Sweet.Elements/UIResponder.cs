using Sweet.Input;
using System.Diagnostics;

namespace Sweet.Elements;

public class UIResponder
{
    private readonly Stopwatch _stopwatch = new();
    private int _doublePushCounter;
    private bool _isHoverJudge;
    private (int X, int Y) _mousePosition;

    /// <summary>
    /// 相対位置
    /// </summary>
    public (int X, int Y) RelativePosition { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public (int X, int Y) Position { get; set; }

    /// <summary>
    /// サイズ
    /// </summary>
    public (int Width, int Height) Size { get; set; }

    /// <summary>
    /// 親要素のサイズ
    /// </summary>
    public (int Width, int Height) ParentSize { get; set; }

    /// <summary>
    /// 入力を受け付けるか
    /// </summary>
    public bool IsInput { get; set; }

    /// <summary>
    /// キーボードの入力ができるか
    /// </summary>
    public bool IsKeyboardInput { get; set; }

    /// <summary>
    /// ジョイパッドの入力ができるか
    /// </summary>
    /// <value></value>
    public bool IsJoypadInput { get; set; }

    /// <summary>
    /// タップでの入力ができるか
    /// </summary>
    public bool IsTopInput { get; set; }

    /// <summary>
    /// ダブルプッシュの間隔
    /// </summary>
    public double DoublePushMs { get; set; }

    /// <summary>
    /// ホバー時に呼ばれる
    /// </summary>
    public Action? OnHover { get; set; }

    /// <summary>
    /// 押されている間呼ばれる
    /// </summary>
    public Action? OnPushing { get; set; }

    /// <summary>
    /// 押された瞬間のみ呼ばれる
    /// </summary>
    public Action? OnPushed { get; set; }

    /// <summary>
    /// 離された瞬間のみ呼ばれる
    /// </summary>
    public Action? OnSeparate { get; set; }

    /// <summary>
    /// キー入力のキーコードリスト
    /// </summary>
    public readonly List<int> KeyCodes = new(5);

    /// <summary>
    /// ジョイパッド入力のキーコード
    /// </summary>
    public readonly List<JoypadKey> joypadKeys = new(5);

    /// <summary>
    /// 子要素リスト
    /// </summary>
    public readonly List<UIResponder> Children = new(100);

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIResponder(int width, int height)
    {
        Size = (width, height);
        _stopwatch.Reset();
        DoublePushMs = 0.4;
        IsInput = true;
        IsKeyboardInput = true;
        IsJoypadInput = true;
        IsTopInput = true;
        _isHoverJudge = true;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public virtual void Update()
    {
        CalculateMousePosition();
        UpdateChildren();
        CallAllAction();
    }

    /// <summary>
    /// マウスの位置の計算
    /// </summary>
    private void CalculateMousePosition()
    {
        if (RelativePosition == (0, 0))
        {
            _mousePosition.X = Mouse.X - Position.X;
            _mousePosition.Y = Mouse.Y - Position.Y;
        }
        else
        {
            _mousePosition.X = RelativePosition.X - Position.X;
            _mousePosition.Y = RelativePosition.Y - Position.Y;
        }
    }

    /// <summary>
    /// アクションを呼び出す
    /// </summary>
    private void CallAllAction()
    {
        CallAction(OnHover, IsHover());
        CallAction(OnPushing, IsPushing());
        CallAction(OnPushed, IsPushed());
        CallAction(OnSeparate, IsSeparate());
    }

    /// <summary>
    /// 子要素の更新
    /// </summary>
    protected virtual void UpdateChildren()
    {
        foreach (var item in Children)
        {
            item.ParentSize = Size;
            item.RelativePosition = (
                _mousePosition.X,
                _mousePosition.Y
            );
            item.Update();

            if (IsInput)
                _isHoverJudge = item.IsHover() ? false : true;
        }
    }

    /// <summary>
    /// ホバーしたかを取得する
    /// </summary>
    public bool IsHover()
    {
        if (!IsInput || !_isHoverJudge)
            return false;

        if (_mousePosition.X >= 0 && _mousePosition.X <= Size.Width
            && _mousePosition.Y >= 0 && _mousePosition.Y <= Size.Height)
            return true;
        else
            return false;
    }

    /// <summary>
    /// ホバー時に押しているかを取得する
    /// </summary>
    public bool IsPushing()
    {
        if (!IsHover())
            return false;

        if (Mouse.IsPushing(MouseKey.Left)
            || IsKeyPush(JudgeInputType.Pushing)
            || IsJoypadPush(JudgeInputType.Pushing)
            || IsTouchPush(JudgeInputType.Pushing))
            return true;
        else
            return false;
    }

    /// <summary>
    /// ホバー時に押した瞬間を取得する
    /// </summary>
    public bool IsPushed()
    {
        if (!IsHover())
            return false;

        if (Mouse.IsPushed(MouseKey.Left)
            || IsKeyPush(JudgeInputType.Pushed)
            || IsJoypadPush(JudgeInputType.Pushed)
            || IsTouchPush(JudgeInputType.Pushed))
            return true;
        else
            return false;
    }

    /// <summary>
    /// ホバー時に離した瞬間を取得する
    /// </summary>
    public bool IsSeparate()
    {
        if (!IsHover())
            return false;

        if (Mouse.IsSeparate(MouseKey.Left)
            || IsKeyPush(JudgeInputType.Separate)
            || IsJoypadPush(JudgeInputType.Separate)
            || IsTouchPush(JudgeInputType.Separate))
            return true;
        else
            return false;
    }

    /// <summary>
    /// ホバー時にダブルプッシュしたかを取得する
    /// </summary>
    /// <returns></returns>
    public bool IsDoublePush()
    {
        if (IsPushed())
        {
            if (_doublePushCounter <= 0)
                _stopwatch.Start();

            _doublePushCounter++;
        }

        // ストップウォッチのタイムが一秒を越していたらリセット
        if (_stopwatch.Elapsed.TotalSeconds > DoublePushMs)
        {
            _doublePushCounter = 0;
            _stopwatch.Reset();
            _stopwatch.Stop();
            return false;
        }

        // もしカウンタが1回を超えていたらダブルクリック成功！
        if (_doublePushCounter > 1)
        {
            _doublePushCounter = 0;
            _stopwatch.Reset();
            _stopwatch.Stop();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// キーで操作したかを取得する
    /// </summary>
    /// <param name="inputType">操作方法</param>
    /// <returns></returns>
    private bool IsKeyPush(JudgeInputType inputType)
    {
        if (!IsKeyboardInput)
            return false;

        foreach (var item in KeyCodes)
        {
            return inputType switch
            {
                JudgeInputType.Pushing => Keyboard.IsPushing(item),
                JudgeInputType.Pushed => Keyboard.IsPushed(item),
                JudgeInputType.Separate => Keyboard.IsSeparate(item),
                _ => Keyboard.IsPushing(item)
            };
        }

        return false;
    }

    /// <summary>
    /// ジョイパッドで操作したかを取得する
    /// </summary>
    /// <param name="inputType">操作方法</param>
    /// <returns></returns>
    private bool IsJoypadPush(JudgeInputType inputType)
    {
        if (!IsKeyboardInput)
            return false;

        foreach (var item in joypadKeys)
        {
            return inputType switch
            {
                JudgeInputType.Pushing => Joypad.IsPushing(item),
                JudgeInputType.Pushed => Joypad.IsPushed(item),
                JudgeInputType.Separate => Joypad.IsSeparate(item),
                _ => Joypad.IsPushing(item)
            };
        }

        return false;
    }

    /// <summary>
    /// タッチで操作したかを取得する
    /// </summary>
    /// <param name="inputType">操作方法</param>
    /// <returns></returns>
    private bool IsTouchPush(JudgeInputType inputType)
    {
        if (!IsTopInput)
            return false;

        return inputType switch
        {
            JudgeInputType.Pushing => Touch.IsPushing(),
            JudgeInputType.Pushed => Touch.IsPushed(),
            JudgeInputType.Separate => Touch.IsSeparate(),
            _ => Touch.IsPushing()
        };
    }

    /// <summary>
    /// アクションを呼ぶ
    /// </summary>
    /// <param name="action">アクション</param>
    /// <param name="isCall">呼ぶか</param>
    private void CallAction(Action? action, bool isCall)
    {
        if (isCall)
            action?.Invoke();
    }

    /// <summary>
    /// 操作方法の列挙型
    /// </summary>
    private enum JudgeInputType
    {
        Pushing,
        Pushed,
        Separate,
    }
}