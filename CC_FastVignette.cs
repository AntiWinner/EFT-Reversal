using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Fast Vignette")]
[ExecuteInEditMode]
public class CC_FastVignette : CC_Base
{
	public Vector2 center = new Vector2(0.5f, 0.5f);

	[Range(-100f, 100f)]
	public float sharpness = 10f;

	[Range(0f, 100f)]
	public float darkness = 30f;

	public bool desaturate;

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(16603));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E014, new Vector4(center.x, center.y, sharpness * 0.01f, darkness * 0.02f));
		DebugGraphics.Blit(source, destination, base.material, desaturate ? 1 : 0);
	}
}
