using System;
using EFT.BlitDebug;
using UnityEngine;

[DisallowMultipleComponent]
public class BloodOnScreen : MonoBehaviour
{
	public bool DebugMode;

	public float BloodColorValue = 1f;

	public float Refraction = 0.022f;

	public int DownsamplingCount = 1;

	public float MaxBloodTime = 5f;

	public int InitialBloodDrops = 3;

	public Color BloodColor;

	public bool GenerateUniqueMaterials = true;

	public Shader BloodShader;

	public Material BlitMaterial;

	public Material FadeoutMat;

	public Material BloodDropMaterial;

	private Material m__E000;

	public int RenderTextureDimension;

	private _E3FF m__E001;

	private RenderTexture m__E002;

	public Vector2 StartScaleDimension = new Vector2(1.5f, 2.2f);

	public Vector2 EndScaleDimension = new Vector2(0.3f, 0.7f);

	public Vector2 DropCountRange = new Vector2(5f, 10f);

	public Vector2 MaxRayLength = new Vector2(0.15f, 0.4f);

	public Vector2 StartXdistribution = new Vector2(0f, 0.075f);

	public Vector2 StartYdistribution = new Vector2(0f, 0.075f);

	public Vector2 EndXdistribution = new Vector2(0.05f, 0.3f);

	public Vector2 EndYdistribution = new Vector2(0.05f, 0.3f);

	public Vector2 DropLifetimeDistribution = new Vector2(0.05f, 1f);

	public float OffGlassScaleMultiplier = 2.5f;

	public float OffGlassLifetimeMultiplier = 2f;

	private _E3FE m__E003;

	public int Mode;

	[HideInInspector]
	[SerializeField]
	public float InputMinL;

	[SerializeField]
	[HideInInspector]
	public float InputMaxL = 255f;

	[SerializeField]
	[HideInInspector]
	public float InputGammaL = 1f;

	[SerializeField]
	[HideInInspector]
	public float InputMinR;

	[SerializeField]
	[HideInInspector]
	public float InputMaxR = 255f;

	[SerializeField]
	[HideInInspector]
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

	[HideInInspector]
	[SerializeField]
	public float OutputMaxR = 255f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinG;

	[HideInInspector]
	[SerializeField]
	public float OutputMaxG = 255f;

	[HideInInspector]
	[SerializeField]
	public float OutputMinB;

	[SerializeField]
	[HideInInspector]
	public float OutputMaxB = 255f;

	public Vector2 center = new Vector2(0.5f, 0.5f);

	[Range(-100f, 100f)]
	public float sharpness = 10f;

	[Range(0f, 100f)]
	public float darkness = 30f;

	private float m__E004;

	protected Material _vignetteMaterial;

	private float m__E005 = float.MinValue;

	private RenderTexture m__E006;

	private RenderTexture _E007;

	private RenderTexture _E008;

	private RenderTexture _E009;

	private bool _E00A;

	private bool _E00B;

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(41665));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(41722));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(41702));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(41754));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(41740));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(41728));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(41786));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(41772));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(41760));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(41819));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(41806));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(41798));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(16603));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(36528));

	protected Material VignetteMaterial
	{
		get
		{
			if (_vignetteMaterial == null)
			{
				Shader shader = Shader.Find(_ED3E._E000(41551));
				_vignetteMaterial = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			return _vignetteMaterial;
		}
	}

	private void Start()
	{
		DropLifetimeDistribution.x *= 3f;
		DropLifetimeDistribution.y *= 1.2f;
		StartScaleDimension *= 1.2f;
		StartScaleDimension.x = Mathf.Min(StartXdistribution.x, EndScaleDimension.x);
		EndScaleDimension.x = StartScaleDimension.x;
		StartScaleDimension.y = Mathf.Max(StartXdistribution.y, EndScaleDimension.y);
		EndScaleDimension.y = StartScaleDimension.y;
		OffGlassScaleMultiplier = 1.8f;
		this.m__E001 = new _E3FF(GetComponent<Camera>().aspect, RenderTextureDimension, BloodDropMaterial, DropLifetimeDistribution, OffGlassLifetimeMultiplier);
		_E3FF obj = this.m__E001;
		obj.OnImageRendered = (Action<RenderTexture>)Delegate.Combine(obj.OnImageRendered, new Action<RenderTexture>(_E005));
		_E003();
		this.m__E003 = this.m__E001.DropPlacer;
		this.m__E001._E009 = 2 * InitialBloodDrops;
		this.m__E003.Setup(StartScaleDimension, EndScaleDimension, DropCountRange, MaxRayLength, StartXdistribution, StartYdistribution, EndXdistribution, EndYdistribution, OffGlassScaleMultiplier);
		this.m__E001.ChangeGlassState(_E00B);
		this.m__E003.ChangeGlassState(_E00B);
		this.m__E000 = new Material(BloodShader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		FadeoutMat.SetFloat(_E00F, Time.deltaTime / MaxBloodTime);
		_E006();
		_E000();
	}

	private void _E000()
	{
		this.m__E005 = Time.time - MaxBloodTime * 0.99f;
		this.m__E001.CaptureBloodShot(Vector3.zero);
	}

	private void OnDestroy()
	{
		_E3FF obj = this.m__E001;
		obj.OnImageRendered = (Action<RenderTexture>)Delegate.Remove(obj.OnImageRendered, new Action<RenderTexture>(_E005));
		this.m__E001?.Destroy();
		_E004();
	}

	private void Update()
	{
		if (!(Time.time - this.m__E005 > MaxBloodTime))
		{
			this.m__E001.Update();
			_E002();
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Time.time - this.m__E005 > MaxBloodTime)
		{
			Graphics.Blit(source, destination);
			return;
		}
		Graphics.Blit(source, _E008);
		for (int i = 0; i < DownsamplingCount; i++)
		{
			Graphics.Blit(_E008, _E009);
			Graphics.Blit(_E009, _E008);
		}
		this.m__E000.SetTexture(_E016, _E008);
		Graphics.Blit(source, this.m__E006, this.m__E000);
		_E001(this.m__E006, destination);
	}

	private void _E001(RenderTexture source, RenderTexture destination)
	{
		if (Time.time - this.m__E004 > 6f)
		{
			Graphics.Blit(source, destination);
			return;
		}
		darkness = 15f * (Mathf.Sin(Time.time) + 1f);
		VignetteMaterial.SetVector(_E018, new Vector4(center.x, center.y, sharpness * 0.01f, darkness * 0.02f));
		DebugGraphics.Blit(source, destination, VignetteMaterial, 1);
	}

	public void Hit(Vector3 direction)
	{
		if (this.m__E001 != null && !_E00A && UnityEngine.Random.Range(0, 3) == 0)
		{
			this.m__E005 = Time.time;
			this.m__E001.CaptureBloodShot(direction);
			_E00A = true;
		}
	}

	public void HitBleeding(Vector3 direction, bool isArterial = false)
	{
		if (this.m__E001 != null && !_E00A)
		{
			this.m__E005 = Time.time;
			if (isArterial)
			{
				this.m__E004 = Time.time;
			}
			this.m__E001.CaptureBloodShotBleeding(direction, isArterial);
			_E00A = true;
		}
	}

	public void ChangeGlassesState(bool hasGlasses)
	{
		_E00B = hasGlasses;
		this.m__E003?.ChangeGlassState(hasGlasses);
		this.m__E001?.ChangeGlassState(hasGlasses);
	}

	private void _E002()
	{
		FadeoutMat.SetFloat(_E00F, Time.deltaTime / MaxBloodTime);
		Graphics.Blit(this.m__E002, this.m__E006, FadeoutMat);
		Graphics.Blit(this.m__E006, this.m__E002);
	}

	private void _E003()
	{
		Vector2Int vector2Int = new Vector2Int(Mathf.CeilToInt(Mathf.Pow(2f, RenderTextureDimension)), Mathf.CeilToInt(Mathf.Pow(2f, RenderTextureDimension)));
		this.m__E002 = new RenderTexture(vector2Int.x, vector2Int.y, 0)
		{
			name = _ED3E._E000(41590)
		};
		this.m__E006 = new RenderTexture(vector2Int.x, vector2Int.y, 0)
		{
			name = _ED3E._E000(41629)
		};
		_E007 = new RenderTexture(vector2Int.x, vector2Int.y, 0)
		{
			name = _ED3E._E000(41611)
		};
		_E008 = new RenderTexture(512, 512, 0)
		{
			name = _ED3E._E000(41645)
		};
		_E009 = new RenderTexture(256, 256, 0)
		{
			name = _ED3E._E000(41691)
		};
	}

	private void _E004()
	{
		if (this.m__E002 != null)
		{
			UnityEngine.Object.Destroy(this.m__E002);
		}
		if (this.m__E006 != null)
		{
			UnityEngine.Object.Destroy(this.m__E006);
		}
		if (_E007 != null)
		{
			UnityEngine.Object.Destroy(_E007);
		}
		if (_E008 != null)
		{
			UnityEngine.Object.Destroy(_E008);
		}
		if (_E009 != null)
		{
			UnityEngine.Object.Destroy(_E009);
		}
		if (this.m__E000 != null)
		{
			UnityEngine.Object.Destroy(this.m__E000);
		}
	}

	private void _E005(RenderTexture texture)
	{
		BlitMaterial.SetTexture(_E017, this.m__E002);
		if (_E00B)
		{
			Graphics.Blit(texture, this.m__E006, BlitMaterial);
			Graphics.Blit(this.m__E006, this.m__E002);
		}
		else
		{
			_E40B.MakeBlur(texture, _E007, 34f);
			Graphics.Blit(texture, this.m__E006, BlitMaterial);
			Graphics.Blit(this.m__E006, this.m__E002);
		}
		_E00A = false;
	}

	private void _E006()
	{
		VignetteMaterial.SetColor(_E019, Color.red);
		darkness = 0f;
		this.m__E000.SetTexture(_E00C, this.m__E002);
		this.m__E000.SetFloat(_E00E, Refraction);
		this.m__E000.SetFloat(_E00D, BloodColorValue);
		this.m__E000.SetColor(_E010, BloodColor);
		if (Mode == 0)
		{
			this.m__E000.SetVector(_E011, new Vector4(InputMinL / 255f, InputMinL / 255f, InputMinL / 255f, 1f));
			this.m__E000.SetVector(_E012, new Vector4(InputMaxL / 255f, InputMaxL / 255f, InputMaxL / 255f, 1f));
			this.m__E000.SetVector(_E013, new Vector4(InputGammaL, InputGammaL, InputGammaL, 1f));
			this.m__E000.SetVector(_E014, new Vector4(OutputMinL / 255f, OutputMinL / 255f, OutputMinL / 255f, 1f));
			this.m__E000.SetVector(_E015, new Vector4(OutputMaxL / 255f, OutputMaxL / 255f, OutputMaxL / 255f, 1f));
		}
		else
		{
			this.m__E000.SetVector(_E011, new Vector4(InputMinR / 255f, InputMinG / 255f, InputMinB / 255f, 1f));
			this.m__E000.SetVector(_E012, new Vector4(InputMaxR / 255f, InputMaxG / 255f, InputMaxB / 255f, 1f));
			this.m__E000.SetVector(_E013, new Vector4(InputGammaR, InputGammaG, InputGammaB, 1f));
			this.m__E000.SetVector(_E014, new Vector4(OutputMinR / 255f, OutputMinG / 255f, OutputMinB / 255f, 1f));
			this.m__E000.SetVector(_E015, new Vector4(OutputMaxR / 255f, OutputMaxG / 255f, OutputMaxB / 255f, 1f));
		}
	}
}
