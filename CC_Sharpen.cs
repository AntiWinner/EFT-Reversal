using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Sharpen")]
public class CC_Sharpen : CC_Base
{
	[Range(0f, 5f)]
	public float strength = 0.6f;

	[Range(0f, 1f)]
	public float clamp = 0.05f;

	public bool DoDesaturate;

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(16585));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(16581));

	private static readonly int _E03A = Shader.PropertyToID(_ED3E._E000(16975));

	private static readonly int _E03B = Shader.PropertyToID(_ED3E._E000(16961));

	private DesaturateEffect _E03C;

	private bool _E03D;

	public Texture textureRamp;

	public float rampOffsetR;

	public float rampOffsetG;

	public float rampOffsetB;

	public float WeatherDesaturate;

	public float HealthDesaturate;

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(16726));

	private static readonly int _E03E = Shader.PropertyToID(_ED3E._E000(17016));

	private static readonly int _E03F = Shader.PropertyToID(_ED3E._E000(17015));

	private readonly string _E040 = _ED3E._E000(16929);

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!_E03D && _E03C == null)
		{
			_E03C = GetComponent<DesaturateEffect>();
			if (_E03C == null)
			{
				_E03D = true;
			}
		}
		if (!_E03D)
		{
			if (DoDesaturate)
			{
				_E03C.enabled = false;
			}
			else
			{
				_E03C.enabled = true;
			}
		}
		if (DoDesaturate)
		{
			if (!_E03D)
			{
				base.material.SetTexture(_E022, _E03C.textureRamp);
				base.material.SetFloat(_E03E, Mathf.Clamp01(_E03C.WeatherDesaturate + _E03C.HealthDesaturate));
				base.material.SetVector(_E03F, new Vector4(_E03C.rampOffsetR, _E03C.rampOffsetG, _E03C.rampOffsetB, 0f));
			}
			else
			{
				base.material.SetTexture(_E022, textureRamp);
				base.material.SetFloat(_E03E, Mathf.Clamp01(WeatherDesaturate + HealthDesaturate));
				base.material.SetVector(_E03F, new Vector4(rampOffsetR, rampOffsetG, rampOffsetB, 0f));
			}
			Shader.EnableKeyword(_E040);
		}
		if (strength == 0f)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		base.material.SetFloat(_E016, 1f / (float)Screen.width);
		base.material.SetFloat(_E017, 1f / (float)Screen.height);
		base.material.SetFloat(_E03A, strength);
		base.material.SetFloat(_E03B, clamp);
		Graphics.Blit(source, destination, base.material);
		if (DoDesaturate)
		{
			Shader.DisableKeyword(_E040);
		}
	}
}
