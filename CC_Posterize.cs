using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Posterize")]
public class CC_Posterize : CC_Base
{
	[Range(2f, 255f)]
	public int levels = 4;

	private static readonly int _E035 = Shader.PropertyToID(_ED3E._E000(16868));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E035, levels);
		Graphics.Blit(source, destination, base.material);
	}
}
