using UnityEngine;

[AddComponentMenu("Colorful/Technicolor")]
[ExecuteInEditMode]
public class CC_Technicolor : CC_Base
{
	[Range(0f, 8f)]
	public float exposure = 4f;

	public Vector3 balance = new Vector3(0.25f, 0.25f, 0.25f);

	[Range(0f, 1f)]
	public float amount = 0.5f;

	private static readonly int _E041 = Shader.PropertyToID(_ED3E._E000(17003));

	private static readonly int _E042 = Shader.PropertyToID(_ED3E._E000(17053));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E041, 8f - exposure);
		base.material.SetVector(_E042, Vector3.one - balance);
		base.material.SetFloat(_E008, amount);
		Graphics.Blit(source, destination, base.material);
	}
}
