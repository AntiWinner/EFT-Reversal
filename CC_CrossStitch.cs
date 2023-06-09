using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Cross Stitch")]
public class CC_CrossStitch : CC_Base
{
	[Range(1f, 128f)]
	public int size = 8;

	public float brightness = 1.5f;

	public bool invert;

	public bool pixelize = true;

	protected Camera m_Camera;

	protected SSAA m_SSAA;

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(16612));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(16664));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(16652));

	protected override void Start()
	{
		base.Start();
		m_Camera = GetComponent<Camera>();
		m_SSAA = GetComponent<SSAA>();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E01B, size);
		base.material.SetFloat(_E01C, brightness);
		int num = (invert ? 1 : 0);
		if (pixelize)
		{
			int num2 = (m_SSAA ? m_SSAA.GetInputWidth() : m_Camera.pixelWidth);
			int num3 = (m_SSAA ? m_SSAA.GetInputHeight() : m_Camera.pixelHeight);
			num += 2;
			base.material.SetFloat(_E007, num2 / size);
			base.material.SetFloat(_E01D, num2 / num3);
		}
		DebugGraphics.Blit(source, destination, base.material, num);
	}
}
