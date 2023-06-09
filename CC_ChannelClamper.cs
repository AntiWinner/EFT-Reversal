using UnityEngine;

[AddComponentMenu("Colorful/Channel Clamper")]
[ExecuteInEditMode]
public class CC_ChannelClamper : CC_Base
{
	public Vector2 red = new Vector2(0f, 1f);

	public Vector2 green = new Vector2(0f, 1f);

	public Vector2 blue = new Vector2(0f, 1f);

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(16534));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(16520));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(16572));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E00C, red);
		base.material.SetVector(_E00D, green);
		base.material.SetVector(_E00E, blue);
		Graphics.Blit(source, destination, base.material);
	}
}
