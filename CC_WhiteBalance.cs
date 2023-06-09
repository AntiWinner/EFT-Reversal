using System;
using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/White Balance")]
public class CC_WhiteBalance : CC_Base
{
	public Color white = new Color(0.5f, 0.5f, 0.5f);

	public int mode = 1;

	private static readonly int _E045 = Shader.PropertyToID(_ED3E._E000(17078));

	protected virtual void Reset()
	{
		white = (CC_Base.IsLinear() ? new Color((float)Math.PI * 59f / 254f, (float)Math.PI * 59f / 254f, (float)Math.PI * 59f / 254f) : new Color(0.5f, 0.5f, 0.5f));
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetColor(_E045, white);
		DebugGraphics.Blit(source, destination, base.material, mode);
	}
}
