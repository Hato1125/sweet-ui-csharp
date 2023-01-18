using DxLibDLL;
using System.Diagnostics;
using Sweet.Input;
using Sweet.Elements;
using System.Drawing;

namespace TestApp;

internal class App
{
    private double ms = 1.0 / 60.0;
    private Stopwatch stopwatch = new();

    private UIControls responder = new(100, 100);
    private UIControls responder2 = new(35, 35);

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

        responder2.BackgroundColor = Color.SkyBlue;
        responder.Children.Add(responder2);
    }

    private void Loop()
    {
        while (true)
        {
            stopwatch.Restart();

            int ml = DX.ProcessMessage();

            if (ml == -1)
                break;

            DX.ClearDrawScreen();

            Keyboard.Update();
            Mouse.Update();
            Touch.Update();
            Joypad.Update();

            DX.GetWindowSize(out int w, out int h);

            responder.ParentSize = (w, h);
            responder.Update();
            responder.DrawView();

            if (responder.IsPushed())
                responder.HorizontalOffset += 10;
            

            if (responder.IsDoublePush())
            {
                responder.Size = (responder.Size.Width + 10, responder.Size.Height + 10);
                responder.BackgroundColor = Color.Red;
            }
            else
            {
                responder.BackgroundColor = Color.White;
            }

            DX.ScreenFlip();

            if (stopwatch.Elapsed.TotalSeconds < ms)
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
        DX.DxLib_End();
    }
}
