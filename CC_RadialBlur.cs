using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Radial Blur")]
public class CC_RadialBlur : CC_Base
{
	[Range(0f, 1f)]
	public float amount = 0.1f;

	[Range(2f, 24f)]
	public int samples = 10;

	public Vector2 center = new Vector2(0.5f, 0.5f);

	public int quality = 1;

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(16593));

	private static readonly int _E039 = Shader.PropertyToID(_ED3E._E000(16936));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetFloat(_E008, amount);
		base.material.SetVector(_E015, center);
		base.material.SetFloat(_E039, samples);
		DebugGraphics.Blit(source, destination, base.material, quality);
	}
}
