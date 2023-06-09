using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Swapper")]
public class CC_ChannelSwapper : CC_Base
{
	public int red;

	public int green = 1;

	public int blue = 2;

	private static Vector4[] _E013 = new Vector4[3]
	{
		new Vector4(1f, 0f, 0f, 0f),
		new Vector4(0f, 1f, 0f, 0f),
		new Vector4(0f, 0f, 1f, 0f)
	};

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(16567));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(16556));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(16555));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E00F, _E013[red]);
		base.material.SetVector(_E010, _E013[green]);
		base.material.SetVector(_E011, _E013[blue]);
		Graphics.Blit(source, destination, base.material);
	}
}
