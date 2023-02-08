# UIView Class
```cs
class UIResponder : UIView
```

## 概要
UIViewは、UIResponderを継承しており、矩形の描画領域を管理するクラスです。

## 使い方
以下のコードは、200 x 200のサイズの中でユーザーがプッシュした場合にある関数を実行する例です。
```cs
using DxLibDLL;
using Sweet.Input;
using Sweet.Elements;

namespace SampleProject;

internal class Program
{
    private static UIView view;

    static void Main()
    {
        // DxLibの初期化処理
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX_DX_SCREEN_BACK);

        view = new(200, 200);
        view.OnPushed += PushMessage;
        view.X = 100;
        view.Y = 100;

        // メインループ
        while (DX.ProcessMessage() != -1)
        {
            DX.ClearDrawScreen();

            // 入力関係の処理の更新
            Touch.Update();
            Mouse.Update();

            view.Update();

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
|プロパティ名|説明|
|```byte Alpha```|Viewの透明度|
|```byte BackgroundAlpha```|背景の透明度|
|```float Radius```|角の半径|
|```bool IsVisible```|Viewを表示するか|
|```IsAlphaBlend```|アルファブレンドで描画するか|
|```BackgroundColor```|背景の色|

## メソッド一覧
|メソッド名|説明|
|```UIView(int width, int height)```|初期化する|
|```void Update()```|更新する|
|```void DrawView()```|描画する|
|```void Dispose()```|破棄する|

## protectedメンバー一覧
|メンバー名|説明|
|```int ViewHandle```|Viewのグラフィックハンドル|
|```void DrawViewArea```|描画領域内に描画する|