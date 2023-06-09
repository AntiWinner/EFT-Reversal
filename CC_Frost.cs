using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Frost")]
[ExecuteInEditMode]
public class CC_Frost : CC_Base
{
	[Range(0f, 16f)]
	public float scale = 1.2f;

	[Range(-100f, 100f)]
	public float sharpness = 40f;

	[Range(0f, 100f)]
	public float darkness = 35f;

	public bool enableVignette = true;

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(16701));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(16688));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (scale == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetFloat(_E007, scale);
		base.material.SetFloat(_E01F, sharpness * 0.01f);
		base.material.SetFloat(_E020, darkness * 0.02f);
		DebugGraphics.Blit(source, destination, base.material, enableVignette ? 1 : 0);
	}
}
