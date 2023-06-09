using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Lookup Filter (Color Grading)")]
[ExecuteInEditMode]
public class CC_LookupFilter : CC_Base
{
	public Texture lookupTexture;

	[Range(0f, 1f)]
	public float amount = 1f;

	private static readonly int _E033 = Shader.PropertyToID(_ED3E._E000(16884));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(16501));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (lookupTexture == null)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetTexture(_E033, lookupTexture);
		base.material.SetFloat(_E008, amount);
		DebugGraphics.Blit(source, destination, base.material, CC_Base.IsLinear() ? 1 : 0);
	}
}
