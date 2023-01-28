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
    private readonly UIButtonStyle TglBtnStyle = new();
    private readonly IUIToggleSwitchStyle TglSwtStyle = new UIToggleSwitchStyle();
    private readonly IUIToggleSwitchStyle TglSwtStyle2 = new UIToggleSwitchStyle();

    private readonly UIButton Btn1 = new(300, 65, "Segoe UI", 30, 5);
    private readonly UIToggleButton TglBtn = new(300, 65, "Segoe UI", 30, 5);
    private readonly UIToggleSwitch TglSwt = new(100, 50, "Segoe UI", 30, 5);
    private readonly VRadioButton Radio = new(300, 500);

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

        TglBtnStyle.BackAlpha = 20;
        TglBtnStyle.FadeAlpha = 255;
        TglBtnStyle.Radius = 20;
        TglBtnStyle.FontSize = 30;
        TglBtnStyle.FontThick = 5;
        TglBtnStyle.ForeColor = Color.White;
        TglBtnStyle.ClickColor = Color.Purple;

        TglSwtStyle.BackAlpha = 20;
        TglSwtStyle.FadeAlpha = 255;
        TglSwtStyle.Radius = 20;
        TglSwtStyle.FontSize = 30;
        TglSwtStyle.FontThick = 5;
        TglSwtStyle.Radius = 24;
        TglSwtStyle.TogglePadding = 8;
        TglSwtStyle.ForeColor = Color.White;
        TglSwtStyle.ClickColor = Color.Purple;
        TglSwtStyle.ToggleColor = Color.White;

        TglSwtStyle2.BackAlpha = 20;
        TglSwtStyle2.FadeAlpha = 255;
        TglSwtStyle2.Radius = 20;
        TglSwtStyle2.FontSize = 25;
        TglSwtStyle2.FontThick = 5;
        TglSwtStyle2.Radius = 24;
        TglSwtStyle2.TogglePadding = 13;
        TglSwtStyle2.ForeColor = Color.White;
        TglSwtStyle2.ClickColor = Color.Purple;
        TglSwtStyle2.ToggleColor = Color.White;

        Btn1.HorizontalAlignment = HorizontalAlignment.Left;
        Btn1.VerticalAlignment = VerticalAlignment.Top;
        Btn1.HorizontalOffset = 100;
        Btn1.VerticalOffset = 100;
        Btn1.Style = BtnStyle;

        TglBtn.HorizontalAlignment = HorizontalAlignment.Left;
        TglBtn.VerticalAlignment = VerticalAlignment.Top;
        TglBtn.HorizontalOffset = 450;
        TglBtn.VerticalOffset = 100;
        TglBtn.Style = TglBtnStyle;

        TglSwt.HorizontalAlignment = HorizontalAlignment.Left;
        TglSwt.VerticalAlignment = VerticalAlignment.Top;
        TglSwt.HorizontalOffset = 100;
        TglSwt.VerticalOffset = 200;
        TglSwt.Style = TglSwtStyle;

        Radio.Stack.HorizontalAlignment = HorizontalAlignment.Left;
        Radio.Stack.VerticalAlignment = VerticalAlignment.Top;
        Radio.Stack.HorizontalOffset = 850;
        Radio.Stack.VerticalOffset = 100;
        Radio.Stack.Style.BackColor = Color.Empty;
        Radio.Style = TglSwtStyle2;
        Radio.AddRadioButton("Test1", 50);
        Radio.AddRadioButton("Test2", 50);
        Radio.AddRadioButton("Test3", 50);
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

        Call(Btn1);
        Call(TglBtn);
        Call(TglSwt);

        Radio.Stack.ParentWidth = w;
        Radio.Stack.ParentHeight = h;
        Radio.Update();
        Radio.DrawView();


        DX.DrawString(0, 0, $"{Touch.X} : {Touch.Y}", 0xffffff);
    }

    private void Call(UIControl cnt)
    {
        DX.GetWindowSize(out int w, out int h);
        cnt.ParentWidth = w;
        cnt.ParentHeight = h;
        cnt.Update();
        cnt.DrawView();
    }
}