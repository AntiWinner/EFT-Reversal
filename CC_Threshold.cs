using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Threshold")]
public class CC_Threshold : CC_Base
{
	[Range(1f, 255f)]
	public float threshold = 128f;

	[Range(0f, 128f)]
	public float noiseRange = 48f;

	public bool useNoise;

	private static readonly int _E043 = Shader.PropertyToID(_ED3E._E000(17046));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(16757));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E043, threshold / 255f);
		base.material.SetFloat(_E025, noiseRange / 255f);
		DebugGraphics.Blit(source, destination, base.material, useNoise ? 1 : 0);
	}
}
