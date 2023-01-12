using DxLibDLL;
using System.Diagnostics;
using Sweet.Input;
using Sweet.Elements;
using System.Drawing;

namespace TestApp;

internal class App
{
    private UIStyle testStyle = new();
    private UIStyle testStyle2 = new();

    private double ms = 1.0 / 60.0;
    private Stopwatch stopwatch = new();

    private UIView? testView;
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
        DX.SetBackgroundColor(0, 0, 0);
        DX.ChangeWindowMode(DX.TRUE);
        DX.SetWaitVSyncFlag(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        DX.CreateMaskScreen();

        testView = new(100, 100);
        testView.X = 100;
        testView.Y = 100;
        testView.VerticalAlignment = VerticalAlignment.Top;
        testView.VerticalOffset = 20;

        opacity = 255;
        img = DX.LoadGraph($"{AppContext.BaseDirectory}test.png");
    }

    private void Loop()
    {
        while (DX.ProcessMessage() != -1)
        {
            stopwatch.Restart();

            DX.ClearDrawScreen();

            Keyboard.Update();
            Mouse.Update();
            Touch.Update();
            Joypad.Update();

            //DX.DrawRotaGraph2F(0, 0, 0, 0, 1.0f, 0.0f, img, DX.TRUE);
            DX.GetWindowSize(out int w, out int h);

            if (Keyboard.IsPushing(DX.KEY_INPUT_UP))
                opacity += 1;

            if (Keyboard.IsPushing(DX.KEY_INPUT_DOWN))
                opacity -= 1;

            if (testView != null)
            {
                testView.ParentWidth = w;
                testView.ParentHeight = h;
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

            if(stopwatch.Elapsed.TotalSeconds < ms)
            {
                double sleepMs = (ms - stopwatch.Elapsed.TotalSeconds) * 1000.0;

                // Thread.Sleepで止めたらCPU使用率上がらない!!!!
                Thread.Sleep((int)sleepMs);

                // WaitTimerで止めるとCPU使用率が上がる...
                //DX.WaitTimer((int)sleepMs);
            }
        }
    }

    private void End()
    {
        testView?.Dispose(true);

        DX.DxLib_End();
    }
}
