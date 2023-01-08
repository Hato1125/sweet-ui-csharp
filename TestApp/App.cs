using DxLibDLL;
using Sweet.Input;

namespace TestApp;

internal class App
{
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
        DX.ChangeWindowMode(DX.TRUE);
        DX.DxLib_Init();
        DX.SetDrawScreen(DX.DX_SCREEN_BACK);
    }

    private void Loop()
    {
        while (DX.ProcessMessage() != -1)
        {
            DX.ClearDrawScreen();

            Touch.Update();

            if(Touch.IsSeparate())
                Console.WriteLine("Tap");

            DX.ScreenFlip();
        }
    }

    private void End()
    {
        DX.DxLib_End();
    }
}
