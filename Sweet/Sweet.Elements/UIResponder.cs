using DxLibDLL;
using Sweet.Input;

namespace Sweet.Elements;

public class UIResponder
{
    private int mouseX;
    private int mouseY;
    private bool isHoverJudge;

    /// <summary>
    /// 実際の横幅
    /// </summary>
    protected int ActualWidth { get; private set; }

    /// <summary>
    /// 実際の高さ
    /// </summary>
    protected int ActualHeight { get; private set; }

    /// <summary>
    /// 相対的なX座標
    /// </summary>
    public int RelativeX { get; set; }

    /// <summary>
    /// 相対的なY座標
    /// </summary>
    public int RelativeY { get; set; }

    /// <summary>
    /// X座標
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Y座標
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
    /// 操作を受け付けるか
    /// </summary>
    public bool IsResponse { get; set; }

    /// <summary>
    /// ホバー時に呼び出される
    /// </summary>
    public Action? OnHover { get; set; }

    /// <summary>
    /// ホバー時に押していたら呼び出される
    /// </summary>
    public Action? OnPushing { get; set; }

    /// <summary>
    /// ホバー時に押した瞬間に呼び出される
    /// </summary>
    public Action? OnPushed { get; set; }

    /// <summary>
    /// ホバー時に離した瞬間に呼び出される
    /// </summary>
    public Action? OnSeparate { get; set; }

    /// <summary>
    /// ホバー時にタッチしていたら呼び出される
    /// </summary>
    public Action? OnTaping { get; set; }

    /// <summary>
    /// ホバー時にタッチした瞬間に呼び出される
    /// </summary>
    public Action? OnTaped { get; set; }

    /// <summary>
    /// ホバー時にタッチを離した瞬間に呼び出される
    /// </summary>
    public Action? OnTapSeparate { get; set; }

    /// <summary>
    /// ホバー時にキーを押していたら呼び出される
    /// </summary>
    public Action? OnKeyPushing { get; set; }

    /// <summary>
    /// ホバー時にキーを押した瞬間に呼び出される
    /// </summary>
    public Action? OnKeyPushed { get; set; }

    /// <summary>
    /// ホバー時にキーを離した瞬間に呼び出される
    /// </summary>
    public Action? OnKeySeparate { get; set; }

    /// <summary>
    /// ホバー時にジョイパッドのボタンを押していたら呼び出される
    /// </summary>
    public Action? OnJoypadPushing { get; set; }

    /// <summary>
    /// ホバー時にジョイパッドのボタンを押した瞬間に呼び出される
    /// </summary>
    public Action? OnJoypadPushed { get; set; }

    /// <summary>
    /// ホバー時にジョイパッドのボタンを離した瞬間に呼び出される
    /// </summary>
    public Action? OnJoypadSeparate { get; set; }

    /// <summary>
    /// サイズの計算方法
    /// </summary>
    public UISize SizeType { get; set; }

    /// <summary>
    /// キーコードリスト
    /// </summary>
    public readonly List<int> KeyList = new(5);

    /// <summary>
    /// ジョイパッドのキーリスト
    /// </summary>
    public readonly List<JoypadKey> JoypadList = new(5);

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
        KeyList.Add(DX.KEY_INPUT_SPACE);
        KeyList.Add(DX.KEY_INPUT_RETURN);
        JoypadList.Add(JoypadKey.Right);
        JoypadList.Add(JoypadKey.Input_1);
        Width = width;
        Height = height;
        IsResponse = true;
        isHoverJudge = true;
        SizeType = UISize.Pixel;
        CalUISize();
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public virtual void Update()
    {
        if (RelativeX == 0 && RelativeY == 0)
        {
            mouseX = Mouse.X - X;
            mouseY = Mouse.Y - Y;
        }
        else
        {
            mouseX = Mouse.X - RelativeX;
            mouseY = Mouse.Y - RelativeY;
        }

        CalUISize();
        UpdateChildPosition();

        // Actionを発生させる
        ActiveOnAction(OnHover, IsHover());
        ActiveOnAction(OnPushing, IsPushing());
        ActiveOnAction(OnPushed, IsPushed());
        ActiveOnAction(OnSeparate, IsSeparate());
        ActiveOnAction(OnTaping, IsTaping());
        ActiveOnAction(OnTaped, IsTaped());
        ActiveOnAction(OnTapSeparate, IsTapSeparate());
        ActiveOnAction(OnKeyPushing, IsKeyPushing());
        ActiveOnAction(OnKeyPushed, IsKeyPushed());
        ActiveOnAction(OnKeySeparate, IsKeySeparate());
        ActiveOnAction(OnJoypadPushing, IsJoyPadPushing());
        ActiveOnAction(OnJoypadPushed, IsJoyPadPushed());
        ActiveOnAction(OnJoypadSeparate, IsJoyPadSeparate());
    }

    /// <summary>
    /// 子要素の位置を更新する
    /// </summary>
    protected virtual void UpdateChildPosition()
    {
        foreach (var child in Children)
        {
            child.RelativeX = X + child.X;
            child.RelativeY = Y + child.Y;
            child.Update();

            isHoverJudge = child.IsHover() ? false : true;
        }
    }

    /// <summary>
    /// UIサイズを計算する
    /// </summary>
    protected void CalUISize()
    {
        if (SizeType == UISize.Pixel)
        {
            ActualWidth = Width;
            ActualHeight = Height;
        }
        else
        {
            ActualWidth = (int)(ParentWidth * (Width / 100.0));
            ActualHeight = (int)(ParentHeight * (Height / 100.0));
        }
    }

    /// <summary>
    /// ホバーしているかを取得する
    /// </summary>
    public bool IsHover()
    {
        if (!IsResponse || !isHoverJudge)
            return false;

        if (mouseX >= 0 && mouseX <= ActualWidth
            && mouseY >= 0 && mouseY <= ActualHeight)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 押している間を取得する
    /// </summary>
    public bool IsPushing()
        => (IsHover() && Mouse.IsPushing(MouseKey.Left))
            || IsKeyPushing()
            || IsTaping()
            || IsJoyPadPushing();

    /// <summary>
    /// 押した瞬間を取得する
    /// </summary>
    public bool IsPushed()
        => (IsHover() && Mouse.IsPushed(MouseKey.Left))
            || IsKeyPushed()
            || IsTaped()
            || IsJoyPadPushed();

    /// <summary>
    /// 離した瞬間を取得する
    /// </summary>
    /// <returns></returns>
    public bool IsSeparate()
        => (IsHover() && Mouse.IsSeparate(MouseKey.Left))
            || IsKeySeparate()
            || IsTapSeparate()
            || IsJoyPadSeparate();

    /// <summary>
    /// ホバー時にタップしている間を取得する
    /// </summary>
    public bool IsTaping()
        => IsHover() && Touch.IsPushing();

    /// <summary>
    /// ホバー時にタップした瞬間を取得する
    /// </summary>
    public bool IsTaped()
        => IsHover() && Touch.IsPushed();

    /// <summary>
    /// ホバー時にタップを離した瞬間を取得する
    /// </summary>
    public bool IsTapSeparate()
        => IsHover() && Touch.IsSeparate();

    /// <summary>
    /// ホバー時にキーを押している間を取得する
    /// </summary>
    public bool IsKeyPushing()
    {
        if (!IsHover())
            return false;

        foreach (var key in KeyList)
            if (Keyboard.IsPushing(key))
                return true;

        return false;
    }

    /// <summary>
    /// ホバー時にキーを押した瞬間を取得する
    /// </summary>
    public bool IsKeyPushed()
    {
        if (!IsHover())
            return false;

        foreach (var key in KeyList)
            if (Keyboard.IsPushed(key))
                return true;

        return false;
    }

    /// <summary>
    /// ホバー時にキーを離した瞬間を取得する
    /// </summary>
    public bool IsKeySeparate()
    {
        if (!IsHover())
            return false;

        foreach (var key in KeyList)
            if (Keyboard.IsSeparate(key))
                return true;

        return false;
    }

    /// <summary>
    /// ホバー時にジョイパッドのボタンを押している間を取得する
    /// </summary>
    public bool IsJoyPadPushing()
    {
        if (!IsHover())
            return false;

        foreach (var key in JoypadList)
            if (Joypad.IsPushing(key))
                return true;

        return false;
    }

    /// <summary>
    /// ホバー時にジョイパッドの押した瞬間を取得する
    /// </summary>
    public bool IsJoyPadPushed()
    {
        if (!IsHover())
            return false;

        foreach (var key in JoypadList)
            if (Joypad.IsPushed(key))
                return true;

        return false;
    }

    /// <summary>
    /// ホバー時にジョイパッドのボタンを離した瞬間を取得する
    /// </summary>
    public bool IsJoyPadSeparate()
    {
        if (!IsHover())
            return false;

        foreach (var key in JoypadList)
            if (Joypad.IsSeparate(key))
                return true;

        return false;
    }

    /// <summary>
    /// アクションを発生させる
    /// </summary>
    /// <param name="action">アクション</param>
    /// <param name="isActive">発生させるか</param>
    private void ActiveOnAction(Action? action, bool isActive)
    {
        if (action == null)
            return;

        if (isActive)
        {
            Tracer.Log("Active Action.");
            action();
        }
    }
}