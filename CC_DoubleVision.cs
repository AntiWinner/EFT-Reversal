using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Double Vision")]
public class CC_DoubleVision : CC_Base
{
	public Vector2 displace = new Vector2(0.7f, 0f);

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(16651));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (amount == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetVector(_E01E, new Vector2(displace.x / (float)Screen.width, displace.y / (float)Screen.height));
		base.material.SetFloat(_E008, amount);
		Graphics.Blit(source, destination, base.material);
	}
}
