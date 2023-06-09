using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Halftone")]
public class CC_Halftone : CC_Base
{
	[Range(0f, 512f)]
	public float density = 64f;

	public int mode = 1;

	public bool antialiasing = true;

	public bool showOriginal;

	protected Camera m_Camera;

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(16719));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(16704));

	protected override void Start()
	{
		base.Start();
		m_Camera = GetComponent<Camera>();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E023, density);
		base.material.SetFloat(_E024, m_Camera.aspect);
		int pass = 0;
		if (mode == 0)
		{
			if (antialiasing && showOriginal)
			{
				pass = 3;
			}
			else if (antialiasing)
			{
				pass = 1;
			}
			else if (showOriginal)
			{
				pass = 2;
			}
		}
		else if (mode == 1)
		{
			pass = (antialiasing ? 5 : 4);
		}
		DebugGraphics.Blit(source, destination, base.material, pass);
	}
}
