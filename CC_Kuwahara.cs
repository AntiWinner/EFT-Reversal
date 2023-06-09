using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Kuwahara")]
public class CC_Kuwahara : CC_Base
{
	[Range(1f, 4f)]
	public int radius = 3;

	protected Camera m_Camera;

	private static readonly int _E02D = Shader.PropertyToID(_ED3E._E000(16827));

	protected override void Start()
	{
		base.Start();
		m_Camera = GetComponent<Camera>();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		radius = Mathf.Clamp(radius, 1, 4);
		base.material.SetVector(_E02D, new Vector2(1f / (float)m_Camera.pixelWidth, 1f / (float)m_Camera.pixelHeight));
		DebugGraphics.Blit(source, destination, base.material, radius - 1);
	}
}
