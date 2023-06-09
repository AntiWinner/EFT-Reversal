using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Grayscale")]
public class CC_Grayscale : CC_Base
{
	[Range(0f, 1f)]
	public float redLuminance = 0.299f;

	[Range(0f, 1f)]
	public float greenLuminance = 0.587f;

	[Range(0f, 1f)]
	public float blueLuminance = 0.114f;

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(16603));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetVector(_E014, new Vector4(redLuminance, greenLuminance, blueLuminance, amount));
		Graphics.Blit(source, destination, base.material);
	}
}
