# UIcontrol Class
```cs
class UIResponder : UIView : UIControl
```

## 概要
UIControlは、すべてのUIコントロールの基底クラスです。<br>
UIの位置を指定することができます。

## 使い方
以下のコードは、500 x 500のWindowの中にに200 x 200の矩形を描画する例です。
```cs
using DxLibDLL;
using Sweet.Input;
using Sweet.Elements;

namespace SampleProject;

internal class Program
{
    private static UIControl control;

    static void Main()
    {
        // DxLibの初期化処理
        DX.ChangeWindowMode(DX.TRUE);
        DX.SetGraphMode(500, 500, 32);
        DX.SetWindowSize(500, 500);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX_DX_SCREEN_BACK);

        control = new(200, 200);
        control.OnPushed += PushMessage;
        control.X = 100;
        control.Y = 100;

        // 水平方向の位置を中央に設定
        control.HorizontalAlignment = HorizontalAlignment.Center;

        // 垂直方向の位置を中央に設定
        control.VerticalAlignment = VerticalAlignment.Center;

        // メインループ
        while (DX.ProcessMessage() != -1)
        {
            DX.ClearDrawScreen();

            // 入力関係の処理の更新
            Touch.Update();
            Mouse.Update();

            // Windowのサイズの取得
            DX.GetWindowSize(out int w, out int h);

            control.ParentWidth = w;
            control.ParentHeight = h;
            control.Update();
            control.DrawView();

            DX.ScreenFlip();
        }

        // Viewを破棄する
        view.Dispose();
    }

    private static void PushMessage()
        => Console.WriteLine("Push!!");
}
```

## プロパティ一覧
|プロパティ名|機能|
|:-------|:---|
|```HorizontalAlignment HorizontalAlignment```|UIの水平方向の位置|
|```VerticalAlignment VerticalAlignment```|UIの垂直方向の位置|
|```int HorizontalOffset```|UIの水平方向の位置のオフセット|
|```int VerticalOffset```|UIの垂直方向の位置のオフセット|
|```UIState State```|UIの状態|

## イベント一覧
|メソッド名|説明|
|:-------|:---|
|```UIControl(int width, int height)```|初期化する|

## protectedメンバー一覧
|メンバー名|説明|
|:-------|:---|
|```void CalculatePosition()```|UIの位置を計算する|