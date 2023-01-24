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

    private UIToggleButton toggle = new(200, 40, "Segoe UI", 20, 0);
    private HRadioButton radio = new(352, 300);

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

        radio.Stack.BackgroundColor = Color.Empty;
        radio.Stack.HorizontalOffset = -30;
        radio.AddRadioButton("AKSK1");
        radio.AddRadioButton("AKSK2");
        radio.AddRadioButton("AKSK3");
        radio.Style.TogglePadding = 9;
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

            radio.Stack.ParentWidth = w;
            radio.Stack.ParentHeight = h;
            radio.Update();
            radio.DrawView();

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
