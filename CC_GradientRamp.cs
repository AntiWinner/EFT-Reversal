using UnityEngine;

[AddComponentMenu("Colorful/Gradient Ramp")]
[ExecuteInEditMode]
public class CC_GradientRamp : CC_Base
{
	public Texture rampTexture;

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(16726));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (rampTexture == null || amount == 0f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		base.material.SetTexture(_E022, rampTexture);
		base.material.SetFloat(_E008, amount);
		Graphics.Blit(source, destination, base.material);
	}
}
