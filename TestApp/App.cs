using DxLibDLL;
using Sweet.Input;
using Sweet.Elements;

namespace TestApp;

internal class App
{
    private UIView? testView;
    private int opacity;

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
        DX.SetBackgroundColor(255, 0, 255);
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        DX.CreateMaskScreen();

        testView = new(100, 100);
        testView.X = 100;
        testView.Y = 100;

        opacity = 255;
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
