using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Photo Filter")]
public class CC_PhotoFilter : CC_Base
{
	public Color color = new Color(1f, 0.5f, 0.2f, 1f);

	[Range(0f, 1f)]
	public float density = 0.35f;

	private static readonly int _E034 = Shader.PropertyToID(_ED3E._E000(16879));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(16719));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (density == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetColor(_E034, color);
		base.material.SetFloat(_E023, density);
		Graphics.Blit(source, destination, base.material);
	}
}
