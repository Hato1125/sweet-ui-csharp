using DxLibDLL;
using Sweet.Input;
using Sweet.Elements;
using System.Drawing;

namespace TestApp;

internal class App
{
    private UIStyle testStyle = new();
    private UIStyle testStyle2 = new();

    private UIView? testView;
    private UIView? testView2;
    private int opacity;
    private int img;

    public void Run()
    {
        Init();
        Loop();
        End();
    }

    private void Init()
    {
        DX.SetOutApplicationLogValidFlag(DX.FALSE);
        DX.SetGraphMode(1000, 1000, 32);
        DX.SetWindowSize(1000, 1000);
        DX.SetBackgroundColor(255, 255, 255);
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        DX.CreateMaskScreen();

        testView2 = new(50, 50);
        testView2.X = 10;
        testView2.Y = 10;
        testView2.BackgroundCornerRadius = 5.0f;

        testView = new(100, 100);
        testView.X = 100;
        testView.Y = 100;
        testView.Children.Add(testView2);

        opacity = 255;
        img = DX.LoadGraph($"{AppContext.BaseDirectory}test.png");
    }

    private void Loop()
    {
        while (DX.ProcessMessage() != -1)
        {
            DX.ClearDrawScreen();

            Keyboard.Update();
            Mouse.Update();
            Touch.Update();
            Joypad.Update();

            //DX.DrawRotaGraph2F(0, 0, 0, 0, 1.0f, 0.0f, img, DX.TRUE);

            if (Keyboard.IsPushing(DX.KEY_INPUT_UP))
                opacity += 1;

            if (Keyboard.IsPushing(DX.KEY_INPUT_DOWN))
                opacity -= 1;

            if (testView != null)
            {
                testView.Update();
                testView.Render();

                testView.BackgroundAlpha = opacity;

                testView.OnRendering = () =>
                {
                    DX.DrawString(15, 10, testView.BackgroundAlpha.ToString(), 0x000000);
                };

                testView.OnPushed = () =>
                {
                    testView.Width += 20;
                    testView.Height += 20;
                };

                if (Keyboard.IsPushed(DX.KEY_INPUT_A))
                {
                    testView.Width -= 20;
                    testView.Height -= 20;
                }
            }

            DX.ScreenFlip();
        }
    }

    private void End()
    {
        testView?.Dispose();

        DX.DxLib_End();
    }
}
