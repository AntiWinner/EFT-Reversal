using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Contrast Vignette")]
public class CC_ContrastVignette : CC_Base
{
	public Vector2 center = new Vector2(0.5f, 0.5f);

	[Range(-100f, 100f)]
	public float sharpness = 32f;

	[Range(0f, 100f)]
	public float darkness = 28f;

	[Range(0f, 200f)]
	public float contrast = 20f;

	[Range(0f, 1f)]
	public float redCoeff = 0.5f;

	[Range(0f, 1f)]
	public float greenCoeff = 0.5f;

	[Range(0f, 1f)]
	public float blueCoeff = 0.5f;

	[Range(0f, 200f)]
	public float edge;

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(16603));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(16542));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(16593));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E014, new Vector4(sharpness * 0.01f, darkness * 0.02f, contrast * 0.01f, edge * 0.01f));
		base.material.SetVector(_E00B, new Vector4(redCoeff, greenCoeff, blueCoeff, 1f));
		base.material.SetVector(_E015, center);
		Graphics.Blit(source, destination, base.material);
	}
}
