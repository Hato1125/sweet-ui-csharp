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

    private readonly UIButtonStyle BtnStyle = new();

    private readonly UIButton Btn1 = new(300, 65, "Segoe UI", 30, 5);
    private readonly UIButton Btn2 = new(300, 65, "Segoe UI", 30, 5);

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
        DX.SetGraphMode(1920, 1080, 32);
        DX.SetWindowSize(1920, 1080);
        DX.SetBackgroundColor(0, 0, 0);
        DX.ChangeWindowMode(DX.TRUE);
        DX.SetWaitVSyncFlag(DX.FALSE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
        DX.CreateMaskScreen();

        BtnStyle.BackAlpha = 20;
        BtnStyle.FadeAlpha = 20;
        BtnStyle.Radius = 20;
        BtnStyle.FontSize = 30;
        BtnStyle.FontThick = 5;
        BtnStyle.ForeColor = Color.White;

        Btn1.HorizontalAlignment = HorizontalAlignment.Left;
        Btn2.HorizontalAlignment = HorizontalAlignment.Right;
        Btn1.HorizontalOffset = 600;
        Btn2.HorizontalOffset = -600;

        Btn1.Style = BtnStyle;
        Btn2.Style = BtnStyle;
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

            loop();

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

    private void loop()
    {
        DX.GetWindowSize(out int w, out int h);

        Btn1.ParentWidth = w;
        Btn1.ParentHeight = h;
        Btn1.Update();
        Btn1.DrawView();

        Btn2.ParentWidth = w;
        Btn2.ParentHeight = h;
        Btn2.Update();
        Btn2.DrawView();

        DX.DrawString(0, 0, $"{Touch.X} : {Touch.Y}", 0xffffff);
    }
}
