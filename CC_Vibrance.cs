using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vibrance")]
public class CC_Vibrance : CC_Base
{
	[Range(-100f, 100f)]
	public float amount;

	[Range(-5f, 5f)]
	public float redChannel = 1f;

	[Range(-5f, 5f)]
	public float greenChannel = 1f;

	[Range(-5f, 5f)]
	public float blueChannel = 1f;

	public bool advanced;

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	private static readonly int _E044 = Shader.PropertyToID(_ED3E._E000(17033));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
		}
		else if (advanced)
		{
			base.material.SetFloat(_E008, amount * 0.01f);
			base.material.SetVector(_E044, new Vector3(redChannel, greenChannel, blueChannel));
			DebugGraphics.Blit(source, destination, base.material, 1);
		}
		else
		{
			base.material.SetFloat(_E008, amount * 0.02f);
			DebugGraphics.Blit(source, destination, base.material, 0);
		}
	}
}
