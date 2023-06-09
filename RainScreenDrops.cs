using UnityEngine;

[DisallowMultipleComponent]
public class RainScreenDrops : MonoBehaviour
{
	[Header("Appearance settings")]
	[SerializeField]
	private Shader _blitShader;

	[SerializeField]
	[Range(0f, 1f)]
	private float _intensity = 1f;

	[SerializeField]
	private float _refraction = 0.1f;

	[SerializeField]
	private float _refractionWithoutGlass = 0.1f;

	[SerializeField]
	private int _downsamplingCount = 7;

	[Header("Drops settings")]
	[SerializeField]
	private AnimationCurve _dropScaleCurve;

	[SerializeField]
	private int _dropsAmount = 32;

	[SerializeField]
	private float _rainDropsDelay = 0.1f;

	[SerializeField]
	private Vector2 _dropScale = new Vector2(0.025f, 0.6f);

	[SerializeField]
	private float _dropLifetime = 25f;

	[SerializeField]
	private float _dropLifetimeWithoutGlass = 10f;

	[SerializeField]
	private bool _isDropsShouldMove;

	[SerializeField]
	private int _maxDropsAtOnce = 4;

	[SerializeField]
	private Material _dropMaterial;

	[SerializeField]
	private float _scaleMultiplierWithoutGlass = 3f;

	private Material _E000;

	public RenderTexture DuDvMap;

	[Space(10f)]
	private RenderTexture _E001;

	private GameObject _E002;

	private _E41C _E003;

	private float _E004 = 25f;

	private float _E005;

	private bool _E006;

	[HideInInspector]
	public int Mode;

	[HideInInspector]
	[SerializeField]
	public float InputMinL;

	[HideInInspector]
	[SerializeField]
	public float InputMaxL = 255f;

	[SerializeField]
	[HideInInspector]
	public float InputGammaL = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinR;

	[HideInInspector]
	[SerializeField]
	public float InputMaxR = 255f;

	[HideInInspector]
	[SerializeField]
	public float InputGammaR = 1f;

	[HideInInspector]
	[SerializeField]
	public float InputMinG;

	[HideInInspector]
	[SerializeField]
	public float InputMaxG = 255f;

	[SerializeField]
	[HideInInspector]
	public float InputGammaG = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinB;

	[HideInInspector]
	[SerializeField]
	public float InputMaxB = 255f;

	[HideInInspector]
	[SerializeField]
	public float InputGammaB = 1f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinL;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxL = 255f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinR;

	[SerializeField]
	[HideInInspector]
	public float OutputMaxR = 255f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinG;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxG = 255f;

	[SerializeField]
	[HideInInspector]
	public float OutputMinB;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxB = 255f;

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(41728));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(41786));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(41772));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(41760));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(41819));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(41806));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(41665));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(41702));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(35970));

	public float Intensity
	{
		get
		{
			return _intensity;
		}
		set
		{
			_intensity = Mathf.Clamp01(value);
			if (_E003 != null)
			{
				_E003.Intensity = _intensity;
			}
		}
	}

	public void ChangeGlassesState(bool hasGlasses)
	{
		_E006 = hasGlasses;
		_E003?.ChangeGlassState(hasGlasses);
	}

	public void Init()
	{
		_E41D.IsDropsShouldMove = _isDropsShouldMove;
		_E003 = new _E41C(_dropScaleCurve, DuDvMap, _dropsAmount, _rainDropsDelay, _dropScale, _dropLifetime, _dropLifetimeWithoutGlass, _maxDropsAtOnce, GetComponent<Camera>(), _dropMaterial, _scaleMultiplierWithoutGlass);
		_E003.ChangeGlassState(_E006);
		_E000 = new Material(_blitShader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		_E000.SetTexture(_E00D, DuDvMap);
		_E005 = _E004 + 3f;
	}

	private void OnValidate()
	{
		if (_E003 != null)
		{
			_E003.UpdateValues(_dropScaleCurve, DuDvMap, _dropsAmount, _rainDropsDelay, _dropScale, _dropLifetime, _dropLifetimeWithoutGlass, _maxDropsAtOnce, GetComponent<Camera>(), _dropMaterial, _scaleMultiplierWithoutGlass);
			_E003.ChangeGlassState(_E006);
		}
	}

	private void Update()
	{
		if (_E003 != null)
		{
			if (Mathf.Abs(_intensity) < 0.1f)
			{
				_E005 += Time.deltaTime;
			}
			else
			{
				_E005 = 0f;
			}
			_E003.Update(Time.deltaTime);
		}
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destanation)
	{
		if (DuDvMap == null || _E005 > _E004 || _E003 == null || !DuDvMap.IsCreated())
		{
			_E3A1.BlitOrCopy(source, destanation);
			return;
		}
		_E000.SetFloat(_E00E, _E006 ? _refraction : _refractionWithoutGlass);
		_E000.SetFloat(_E00F, _intensity);
		if (Mode == 0)
		{
			_E000.SetVector(_E007, new Vector4(InputMinL / 255f, InputMinL / 255f, InputMinL / 255f, 1f));
			_E000.SetVector(_E008, new Vector4(InputMaxL / 255f, InputMaxL / 255f, InputMaxL / 255f, 1f));
			_E000.SetVector(_E009, new Vector4(InputGammaL, InputGammaL, InputGammaL, 1f));
			_E000.SetVector(_E00A, new Vector4(OutputMinL / 255f, OutputMinL / 255f, OutputMinL / 255f, 1f));
			_E000.SetVector(_E00B, new Vector4(OutputMaxL / 255f, OutputMaxL / 255f, OutputMaxL / 255f, 1f));
		}
		else
		{
			_E000.SetVector(_E007, new Vector4(InputMinR / 255f, InputMinG / 255f, InputMinB / 255f, 1f));
			_E000.SetVector(_E008, new Vector4(InputMaxR / 255f, InputMaxG / 255f, InputMaxB / 255f, 1f));
			_E000.SetVector(_E009, new Vector4(InputGammaR, InputGammaG, InputGammaB, 1f));
			_E000.SetVector(_E00A, new Vector4(OutputMinR / 255f, OutputMinG / 255f, OutputMinB / 255f, 1f));
			_E000.SetVector(_E00B, new Vector4(OutputMaxR / 255f, OutputMaxG / 255f, OutputMaxB / 255f, 1f));
		}
		RenderTexture temporary = RenderTexture.GetTemporary(512, 512, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(256, 256, 0);
		Graphics.Blit(source, temporary);
		for (int i = 0; i < _downsamplingCount; i++)
		{
			Graphics.Blit(temporary, temporary2);
			Graphics.Blit(temporary2, temporary);
		}
		_E000.SetTexture(_E00C, temporary);
		Graphics.Blit(source, destanation, _E000);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	private void OnDestroy()
	{
		if (_E003 != null)
		{
			_E003.Clear();
		}
	}

	public void SetIntensity(float intensity)
	{
		Intensity = intensity;
		base.enabled = intensity > 0f;
	}
}
