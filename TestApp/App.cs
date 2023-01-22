using DxLibDLL;
using System.Diagnostics;
using Sweet.Input;
using Sweet.Elements;
using Sweet.Controls;
using System.Drawing;

namespace TestApp;

internal class App
{
    private double ms = 1.0 / 60.0;
    private Stopwatch stopwatch = new();

    private UILabel label = new(225, 350, "Segoe UI", 25, 0);
    private UILabel label2 = new(225, 350, "Segoe UI", 25, 0);
    private UIButton btn = new(175, 50, "Segoe UI", 16, 4);

    public void Run()
    {
        Init();
        Loop();
        End();
    }

    private void Init()
    {
        DX.SetOutApplicationLogValidFlag(DX.FALSE);
        // DX3D9EXにするとなんも表示されなくなる...　D3DX9EXはサポート対象外にしますか...
        //DX.SetUseDirect3DVersion(DX.DX_DIRECT3D_9EX);
        DX.SetGraphMode(352, 430, 32);
        DX.SetWindowSize(352, 430);
        DX.SetBackgroundColor(0, 0, 0);
        DX.ChangeWindowMode(DX.TRUE);
        DX.SetWaitVSyncFlag(DX.FALSE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        DX.CreateMaskScreen();

        label.Text = "Text Size";
        label.ForegroundColor = Color.FromArgb(0, 208, 101);
        label.TextHorizontalAlignment = HorizontalAlignment.Left;
        label.TextVerticalAlignment = VerticalAlignment.Top;
        label.BackgroundColor = Color.Empty;

        label2.Text = "Apps that support\nDynamic Type will\nadjust to your\npreferred reading\nsize below";
        label2.ForegroundColor = Color.White;
        label2.TextHorizontalAlignment = HorizontalAlignment.Left;
        label2.TextVerticalAlignment = VerticalAlignment.Top;
        label2.VerticalOffset = 50;
        label.TextSpace = 3;
        label2.BackgroundColor = Color.Empty;

        btn.VerticalOffset = 100;
        btn.Text = "SweetUI\nButton";
        btn.BackgroundAlpha = 20;
        btn.ClickColor = Color.White;
        btn.ForegroundColor = Color.White;
        btn.ClickFadeAlpha = 10;
        btn.AnimeSpeed = 1650;
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

            label.ParentWidth = w;
            label.ParentHeight = h;
            label.Update();
            label.DrawView();

            label2.ParentWidth = w;
            label2.ParentHeight = h;
            label2.Update();
            label2.DrawView();

            btn.ParentWidth = w;
            btn.ParentHeight = h;
            btn.Update();
            btn.DrawView();

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
        label.Dispose();
        label2.Dispose();

        DX.DxLib_End();
    }
}
