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
    private UIToggleSwitch btn = new(70, 35, "Segoe UI", 16, 0);
    private UIButton btn2 = new(100, 47, "Segoe UI", 16, 0);

    public UIButtonStyle BtnStyle = new();

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
        label.Style.ForeColor = Color.FromArgb(0, 208, 101);
        label.TextContent.HorizontalAlignment = HorizontalAlignment.Left;
        label.TextContent.VerticalAlignment = VerticalAlignment.Top;
        label.Style.BackColor = Color.Empty;

        label2.Text = "Apps that support\nDynamic Type will\nadjust to your\npreferred reading\nsize below";
        label2.Style.ForeColor = Color.White;
        label2.TextContent.HorizontalAlignment = HorizontalAlignment.Left;
        label2.TextContent.VerticalAlignment = VerticalAlignment.Top;
        label2.VerticalOffset = 50;
        label.Style.TextSpace = 3;
        label2.Style.BackColor = Color.Empty;

        BtnStyle.BackAlpha = 20;
        BtnStyle.FadeAlpha = 20;
        BtnStyle.ClickColor = Color.White;
        BtnStyle.ForeColor = Color.White;

        btn.VerticalOffset = 100;
        btn.HorizontalAlignment = HorizontalAlignment.Left;
        btn.HorizontalOffset = 30;
        btn.Style.BackAlpha = 30;
        btn.Style.ToggleColor = Color.White;

        btn2.VerticalOffset = 100;
        btn2.HorizontalAlignment = HorizontalAlignment.Right;
        btn2.HorizontalOffset = -20;
        btn2.Text = "Cancel";
        btn2.Style = BtnStyle;
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

            btn2.ParentWidth = w;
            btn2.ParentHeight = h;
            btn2.Update();
            btn2.DrawView();


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
