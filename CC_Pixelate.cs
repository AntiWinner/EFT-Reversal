using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Pixelate")]
public class CC_Pixelate : CC_Base
{
	[Range(1f, 1024f)]
	public float scale = 80f;

	public bool automaticRatio;

	public float ratio = 1f;

	public int mode;

	protected Camera m_Camera;

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(16652));

	protected override void Start()
	{
		base.Start();
		m_Camera = GetComponent<Camera>();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		switch (mode)
		{
		case 0:
			base.material.SetFloat(_E007, scale);
			break;
		default:
			base.material.SetFloat(_E007, (float)m_Camera.pixelWidth / scale);
			break;
		}
		base.material.SetFloat(_E01D, automaticRatio ? ((float)m_Camera.pixelWidth / (float)m_Camera.pixelHeight) : ratio);
		Graphics.Blit(source, destination, base.material);
	}
}
