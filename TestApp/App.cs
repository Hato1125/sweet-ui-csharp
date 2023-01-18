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

    private UIResponder responder = new(100, 100);

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

            responder.HorizontalAlignment = HorizontalAlignment.Center;
            responder.VerticalAlignment = VerticalAlignment.Center;
            responder.ParentSize = (w, h);
            responder.Update();
            DX.DrawFillBox(
                responder.Position.X,
                responder.Position.Y,
                responder.Position.X + responder.Size.Width,
                responder.Position.Y + responder.Size.Height,
                DX.GetColor(255, 255, 255)
            );

            if (responder.IsHover())
                DX.DrawString(200, 100, "Hover", 0xffffff);

            if (responder.IsPushing())
                DX.DrawString(200, 120, "Pushing", 0xffffff);

            if (responder.IsPushed())
                DX.DrawString(200, 140, "Pushed", 0xffffff);

            if (responder.IsSeparate())
                DX.DrawString(200, 160, "Separate", 0xffffff);

            if (responder.IsDoublePush())
            {
                DX.DrawString(200, 180, "DoublePush", 0xffffff);

            DX.DrawFillBox(
                responder.Position.X,
                responder.Position.Y,
                responder.Position.X + responder.Size.Width,
                responder.Position.Y + responder.Size.Height,
                DX.GetColor(255, 0, 0)
            );
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
