using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/RGB Split")]
public class CC_RGBSplit : CC_Base
{
	public float amount;

	public float angle;

	private static readonly int _E036 = Shader.PropertyToID(_ED3E._E000(16924));

	private static readonly int _E037 = Shader.PropertyToID(_ED3E._E000(16908));

	private static readonly int _E038 = Shader.PropertyToID(_ED3E._E000(16958));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetFloat(_E036, amount * 0.001f);
		base.material.SetFloat(_E037, Mathf.Cos(angle));
		base.material.SetFloat(_E038, Mathf.Sin(angle));
		Graphics.Blit(source, destination, base.material);
	}
}
