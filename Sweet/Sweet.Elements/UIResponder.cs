using Sweet.Input;
using System.Diagnostics;

namespace Sweet.Elements;

public class UIResponder
{
    private readonly Stopwatch _stopwatch = new();
    private int _doublePushCounter;
    private bool _isHoverJudge;
    private (int X, int Y) _mousePosition;
    private (int X, int Y) _touchPosition;

    /// <summary>
    /// 相対X座標
    /// </summary>
    public int RelativeX { get; set; }
    
    /// <summary>
    /// 相対Y座標
    /// </summary>
    public int RelativeY { get; set; }

    /// <summary>
    /// UIのX座標の位置
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// UIのY座標の位置
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// 横幅
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 高さ
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 親要素の横幅
    /// </summary>
    public int ParentWidth { get; set; }

    /// <summary>
    /// 親要素の高さ
    /// </summary>
    public int ParentHeight { get; set; }

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
    public event Action OnHover = delegate { };

    /// <summary>
    /// 押されている間呼ばれる
    /// </summary>
    public event Action OnPushing = delegate { };

    /// <summary>
    /// 押された瞬間のみ呼ばれる
    /// </summary>
    public event Action OnPushed = delegate { };

    /// <summary>
    /// 離された瞬間のみ呼ばれる
    /// </summary>
    public event Action OnSeparate = delegate { };

    /// <summary>
    /// キー入力のキーコードリスト
    /// </summary>
    public readonly int[] KeyCodes = new int[256];

    /// <summary>
    /// ジョイパッド入力のキーコード
    /// </summary>
    public readonly JoypadKey[] joypadKeys = new JoypadKey[14];

    /// <summary>
    /// 子要素リスト
    /// </summary>
    public readonly List<UIResponder> Children = new();

    /// <summary>
    /// 初期化する
    /// </summary>
    /// <param name="width">横幅</param>
    /// <param name="height">高さ</param>
    public UIResponder(int width, int height)
    {
        Width = width;
        Height = height;
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
        FireEvents();
    }

    /// <summary>
    /// マウスの位置の計算
    /// </summary>
    private void CalculateMousePosition()
    {
        if (RelativeX == 0 && RelativeY == 0)
        {
            _mousePosition.X = Mouse.X - X;
            _mousePosition.Y = Mouse.Y - Y;

            _touchPosition.X = Touch.X - X;
            _touchPosition.Y = Touch.Y - Y;
        }
        else
        {
            _mousePosition.X = RelativeX - X;
            _mousePosition.Y = RelativeY - Y;

            _touchPosition.X = RelativeX - X;
            _touchPosition.Y = RelativeY - Y;
        }
    }

    /// <summary>
    /// アクションを呼び出す
    /// </summary>
    private void FireEvents()
    {
        if (IsHover()) OnHover();
        if (IsPushing()) OnPushing();
        if (IsPushed()) OnPushed();
        if(IsSeparate()) OnSeparate();
    }

    /// <summary>
    /// 子要素の更新
    /// </summary>
    protected virtual void UpdateChildren()
    {
        foreach (var item in Children)
        {
            item.ParentWidth = Width;
            item.ParentHeight = Height;
            item.RelativeX = _mousePosition.X;
            item.RelativeY = _mousePosition.Y;
            item.Update();

            if (IsInput)
                _isHoverJudge = !item.IsHover();
        }
    }

    private bool JudgePhysicalDeviceState(JudgeInputType type) => IsKeyPush(type) || IsJoypadPush(type) || IsTouchPush(type);

    /// <summary>
    /// ホバーしたかを取得する
    /// </summary>
    public bool IsHover()
    {
        if (!IsInput || !_isHoverJudge)
            return false;

        // タッチしている間はマウスカーソルはないのでタッチのみ判定する
        return Touch.IsPushing() ? IsTouchHover() : IsMouseHover();
    }

    /// <summary>
    /// ホバー時に押しているかを取得する
    /// </summary>
    public bool IsPushing()
    {
        if (!IsHover())
            return false;

        return Mouse.IsPushing(MouseKey.Left) || JudgePhysicalDeviceState(JudgeInputType.Pushing);
    }

    /// <summary>
    /// ホバー時に押した瞬間を取得する
    /// </summary>
    public bool IsPushed()
    {
        if (!IsHover())
            return false;

        return Mouse.IsPushed(MouseKey.Left) || JudgePhysicalDeviceState(JudgeInputType.Pushed);
    }

    /// <summary>
    /// ホバー時に離した瞬間を取得する
    /// </summary>
    public bool IsSeparate()
    {
        if (!IsHover())
            return false;

        return Mouse.IsSeparate(MouseKey.Left) || JudgePhysicalDeviceState(JudgeInputType.Separate);
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
    /// マウスカーソルがホバーしてるかを取得する
    /// </summary>
    private bool IsMouseHover()
    {
        return _mousePosition.X >= 0 && _mousePosition.X <= Width
            && _mousePosition.Y >= 0 && _mousePosition.Y <= Height;
    }

    /// <summary>
    /// タッチカーソルがホバーしてるかを取得する
    /// </summary>
    private bool IsTouchHover()
    {
        return _touchPosition.X >= 0 && _touchPosition.X <= Width
            && _touchPosition.Y >= 0 && _touchPosition.Y <= Height;
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
    /// 操作方法の列挙型
    /// </summary>
    private enum JudgeInputType
    {
        Pushing,
        Pushed,
        Separate,
    }
}