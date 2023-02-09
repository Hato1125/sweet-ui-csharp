using DxLibDLL;

namespace Sweet.Graphics;

/// <summary>
/// ブレンドモード
/// </summary>
public enum BlendMode
{
    None = DX.DX_BLENDMODE_NOBLEND,
    Alpha = DX.DX_BLENDMODE_ALPHA,
    Add = DX.DX_BLENDMODE_ADD,
    Sub = DX.DX_BLENDMODE_SUB,
    Mula = DX.DX_BLENDMODE_MULA,
    Invsrc = DX.DX_BLENDMODE_INVSRC,
    PmaAlpha = DX.DX_BLENDMODE_PMA_ALPHA,
    PmaAdd = DX.DX_BLENDMODE_PMA_ADD,
    PmaSub = DX.DX_BLENDMODE_PMA_SUB,
    PmaInvsrc = DX.DX_BLENDMODE_PMA_INVSRC,
}