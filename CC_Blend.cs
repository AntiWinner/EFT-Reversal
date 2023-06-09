using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Blend")]
[ExecuteInEditMode]
public class CC_Blend : CC_Base
{
	public Texture texture;

	[Range(0f, 1f)]
	public float amount = 1f;

	public int mode;

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(16493));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (texture == null || amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetTexture(_E009, texture);
		base.material.SetFloat(_E008, amount);
		DebugGraphics.Blit(source, destination, base.material, mode);
	}
}
