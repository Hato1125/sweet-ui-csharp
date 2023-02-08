# UIResponder Class
```cs
class UIResponder
```

## 概要
UIResponderクラスは、ユーザーのアクションを取得する機能やイベントを提供するクラスです。

## 使い方
以下のコードは、200 x 200のサイズの中でユーザーがプッシュした場合にある関数を実行する例です。
```cs
using DxLibDLL;
using Sweet.Input;
using Sweet.Elements;

namespace SampleProject;

internal class Program
{
    private static UIResponder responder;

    static void Main()
    {
        // DxLibの初期化処理
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX_DX_SCREEN_BACK);

        responder = new(200, 200);
        responder.OnPushed += PushMessage;
        responder.X = 100;
        responder.Y = 100;

        // メインループ
        while (DX.ProcessMessage() != -1)
        {
            DX.ClearDrawScreen();

            // 入力関係の処理の更新
            Touch.Update();
            Mouse.Update();

            responder.Update();

            DX.ScreenFlip();
        }
    }

    private static void PushMessage()
        => Console.WriteLine("Push!!");
}
```

## プロパティ一覧
|プロパティ名|機能|
|:---------|:---|
|```int RelativeX```|親要素からの相対的なX座標|
|```int RelativeY```|親要素からの相対的なY座標|
|```int X```|WindowからのX座標|
|```int Y```|WindowからのY座標|
|```int Width```|UIResponderがユーザーの入力を取得できる横幅|
|```int Height```|UIResponderがユーザーの入力を取得できる高さ|
|```int ParentWidth```|親要素の横幅|
|```int ParentHeight```|親要素の高さ|
|```bool IsInput```|UIResponderがユーザーの入力を受け付けるか|
|```bool IsKeyboardInput```|キーボードでの入力ができるか|
|```bool IsJoypadInput```|Joypadでの入力ができるか|
|```bool IsTapInput```|タップでの入力ができるか|
|```doubleDoublePushMs```|ダブルプッシュの有効な秒数|
|```int[] KeyCodes```|受け付けるキーコード ※Length:256|
|```JoypadKey[] JoypadKeys```|受け付けるジョイパッドのキーコード ※Length:14|
|```List<UIResponder> Children```|子要素のリスト|

## イベント一覧
|イベント名|説明|
|:-------|:---|
|```Action OnPushing```|ホバーしている状態で、押されている間呼ばれる|
|```Action OnPushed```|ホバーしている状態で、押された瞬間のみ呼ばれる|
|```Action OnSeparate```|ホバーしている状態で、離された瞬間のみ呼ばれる|

## メソッド一覧
|メソッド名|説明|
|:-------|:---|
|```UIResponder(int width, int height)```|初期化する|
|```void Update()```|更新する|
|```bool IsPushing()```|ホバー時に、押しているかを取得する|
|```bool IsPushed()```|ホバー時に、押した瞬間を取得する|
|```bool IsSeparate()```|ホバー時に、離した瞬間を取得する|
|```bool IsDoublePush()```|ホバー時に、ダブルプッシュしたかを取得する|

## protectedメンバー一覧
|メンバー名|説明|
|:-------|:---|
|```void UpdateChildren```|子要素を更新する|