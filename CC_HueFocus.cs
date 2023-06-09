using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue Focus")]
public class CC_HueFocus : CC_Base
{
	[Range(0f, 360f)]
	public float hue;

	[Range(1f, 180f)]
	public float range = 30f;

	[Range(0f, 1f)]
	public float boost = 0.5f;

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(16757));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(16734));

	[ImageEffectTransformsToLDR]
	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		float num = hue / 360f;
		float num2 = range / 180f;
		base.material.SetVector(_E025, new Vector2(num - num2, num + num2));
		base.material.SetVector(_E021, new Vector3(num, boost + 1f, amount));
		Graphics.Blit(source, destination, base.material);
	}
}
