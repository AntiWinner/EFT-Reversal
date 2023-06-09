using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/LED")]
public class CC_Led : CC_Base
{
	[Range(1f, 255f)]
	public float scale = 80f;

	[Range(0f, 10f)]
	public float brightness = 1f;

	public bool automaticRatio;

	public float ratio = 1f;

	public int mode;

	protected Camera m_Camera;

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(16652));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(16664));

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
		base.material.SetFloat(_E01C, brightness);
		Graphics.Blit(source, destination, base.material);
	}
}
