using EFT.BlitDebug;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Analog TV")]
public class CC_AnalogTV : CC_Base
{
	public bool autoPhase = true;

	public float phase = 0.5f;

	public bool grayscale;

	[Range(0f, 1f)]
	public float noiseIntensity = 0.5f;

	[Range(0f, 10f)]
	public float scanlinesIntensity = 2f;

	[Range(0f, 4096f)]
	public float scanlinesCount = 768f;

	public float scanlinesOffset;

	[Range(-2f, 2f)]
	public float distortion = 0.2f;

	[Range(-2f, 2f)]
	public float cubicDistortion = 0.6f;

	[Range(0.01f, 2f)]
	public float scale = 0.8f;

	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(18405));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(16412));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(16396));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(16440));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(16424));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(16473));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(16461));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(16510));

	protected virtual void Update()
	{
		if (autoPhase)
		{
			phase += Time.deltaTime * 0.25f;
		}
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat(_E000, phase);
		base.material.SetFloat(_E001, noiseIntensity);
		base.material.SetFloat(_E002, scanlinesIntensity);
		base.material.SetFloat(_E003, (int)scanlinesCount);
		base.material.SetFloat(_E004, scanlinesOffset);
		base.material.SetFloat(_E005, distortion);
		base.material.SetFloat(_E006, cubicDistortion);
		base.material.SetFloat(_E007, scale);
		DebugGraphics.Blit(source, destination, base.material, grayscale ? 1 : 0);
	}
}
