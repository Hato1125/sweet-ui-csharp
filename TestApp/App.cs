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

    private UIView? testView;
    private UIView? testView2;
    private UIView? testView3;

    private Label? testView1_0;
    private Label? testView1_2;
    private Label? testView1_3;

    private Label? label;

    private VStackPanel? vstack;
    private HStackPanel? hstack;

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

        testView = new(50, 50);
        testView2 = new(50, 50);
        testView3 = new(50, 50);
        testView.BorderColor = Color.Empty;
        testView2.BorderColor = Color.Empty;
        testView3.BorderColor = Color.Empty;
        testView.BackgroundCornerRadius = 100;
        testView2.BackgroundCornerRadius = 100;
        testView3.BackgroundCornerRadius = 100;
        testView.BackgroundColor = Color.Green;
        testView2.BackgroundColor = Color.Yellow;
        testView3.BackgroundColor = Color.Red;

        testView1_0 = new(150, 30, "Segoe UI", 18, 5);
        testView1_2 = new(150, 30, "Segoe UI", 18, 5);
        testView1_3 = new(150, 30, "Segoe UI", 18, 5);
        testView1_0.BorderColor = Color.Empty;
        testView1_2.BorderColor = Color.Empty;
        testView1_3.BorderColor = Color.Empty;
        testView1_0.BackgroundCornerRadius = 100;
        testView1_2.BackgroundCornerRadius = 100;
        testView1_3.BackgroundCornerRadius = 100;
        testView1_0.BackgroundColor = Color.Green;
        testView1_2.BackgroundColor = Color.Yellow;
        testView1_3.BackgroundColor = Color.Red;

        hstack = new(200, 250);
        hstack.Children.Add(testView);
        hstack.Children.Add(testView2);
        hstack.Children.Add(testView3);

        vstack = new(200, 250);
        vstack.Children.Add(testView1_0);
        vstack.Children.Add(testView1_2);
        vstack.Children.Add(testView1_3);

        hstack.HorizontalAlignment = HorizontalAlignment.Left;
        vstack.HorizontalAlignment = HorizontalAlignment.Right;

        hstack.HorizontalOffset = 200;
        vstack.HorizontalOffset = -200;

        label = new(170, 30, "Segoe UI", 18, 5);
        label.BackgroundColor = Color.Empty;
        label.BorderColor = Color.Empty;
        label.ForegroundColor = Color.White;

        opacity = 255;
        img = DX.LoadGraph($"{AppContext.BaseDirectory}test.png");
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

            //DX.DrawRotaGraph2F(0, 0, 0, 0, 1.0f, 0.0f, img, DX.TRUE);
            DX.GetWindowSize(out int w, out int h);

            if (Keyboard.IsPushing(DX.KEY_INPUT_UP))
                opacity += 1;

            if (Keyboard.IsPushing(DX.KEY_INPUT_DOWN))
                opacity -= 1;

            if (hstack != null)
            {
                hstack.ParentWidth = w;
                hstack.ParentHeight = h;
                hstack.X = 100;
                hstack.Y = 100;

                hstack.Update();
                hstack.Render();
            }

            if (vstack != null)
            {
                vstack.ParentWidth = w;
                vstack.ParentHeight = h;
                vstack.X = 800;
                vstack.Y = 100;

                vstack.Update();
                vstack.Render();
            }

            if (label != null)
            {
                label.ParentWidth = w;
                label.ParentHeight = h;
                label.Update();
                label.Render();
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
        testView?.Dispose(true);

        DX.DxLib_End();
    }
}
