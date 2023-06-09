using UnityEngine;

[AddComponentMenu("Colorful/Channel Mixer")]
[ExecuteInEditMode]
public class CC_ChannelMixer : CC_Base
{
	[Range(-200f, 200f)]
	public float redR = 100f;

	[Range(-200f, 200f)]
	public float redG;

	[Range(-200f, 200f)]
	public float redB;

	[Range(-200f, 200f)]
	public float greenR;

	[Range(-200f, 200f)]
	public float greenG = 100f;

	[Range(-200f, 200f)]
	public float greenB;

	[Range(-200f, 200f)]
	public float blueR;

	[Range(-200f, 200f)]
	public float blueG;

	[Range(-200f, 200f)]
	public float blueB = 100f;

	[Range(-200f, 200f)]
	public float constantR;

	[Range(-200f, 200f)]
	public float constantG;

	[Range(-200f, 200f)]
	public float constantB;

	public int currentChannel;

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(16567));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(16556));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(16555));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(16545));

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector(_E00F, new Vector4(redR * 0.01f, greenR * 0.01f, blueR * 0.01f));
		base.material.SetVector(_E010, new Vector4(redG * 0.01f, greenG * 0.01f, blueG * 0.01f));
		base.material.SetVector(_E011, new Vector4(redB * 0.01f, greenB * 0.01f, blueB * 0.01f));
		base.material.SetVector(_E012, new Vector4(constantR * 0.01f, constantG * 0.01f, constantB * 0.01f));
		Graphics.Blit(source, destination, base.material);
	}
}
