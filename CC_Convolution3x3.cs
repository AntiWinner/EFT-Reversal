using UnityEngine;

[AddComponentMenu("Colorful/Convolution Matrix 3x3")]
[ExecuteInEditMode]
public class CC_Convolution3x3 : CC_Base
{
	public Vector3 kernelTop = Vector3.zero;

	public Vector3 kernelMiddle = Vector3.up;

	public Vector3 kernelBottom = Vector3.zero;

	public float divisor = 1f;

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(16585));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(16581));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(16577));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(16634));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(16627));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E016, 1f / (float)Screen.width);
		base.material.SetFloat(_E017, 1f / (float)Screen.height);
		base.material.SetFloat(_E008, amount);
		base.material.SetVector(_E018, kernelTop / divisor);
		base.material.SetVector(_E019, kernelMiddle / divisor);
		base.material.SetVector(_E01A, kernelBottom / divisor);
		Graphics.Blit(source, destination, base.material);
	}
}
