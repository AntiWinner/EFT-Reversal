using System;
using System.Collections;
using System.Collections.Generic;
using EFT.BlitDebug;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VisorEffect : MonoBehaviour
{
	public enum EMask
	{
		NoMask,
		Narrow,
		Wide
	}

	[Serializable]
	public class VisorMask
	{
		public EMask EMask;

		public Texture Texture;
	}

	private const string m__E000 = "_Mask";

	private const string m__E001 = "_Parameter";

	private const string m__E002 = "_SwingValue";

	private const string m__E003 = "_HitZoomValue";

	private const string m__E004 = "_ScratchesTex";

	private const string m__E005 = "_ScratcesIntensity";

	private const string m__E006 = "_BlurMask";

	private const string m__E007 = "_DistortMask";

	private const string m__E008 = "_GlassDamageTex";

	private const string _E009 = "_Rotation";

	private const string _E00A = "_DistortIntensity";

	private const string _E00B = "_Intensity";

	private const string _E00C = "_EdgeFade";

	private const string _E00D = "_BlurTex";

	private const string _E00E = "_MinLightValue";

	private const string _E00F = "_IsAffectedByLight";

	[Range(0f, 1f)]
	public float Intensity = 1f;

	public AnimationCurve EdgeFadeCurve = AnimationCurve.Linear(0f, 1f, 0f, 1f);

	public Shader VisorShader;

	public List<VisorMask> VisorMasks;

	public Texture Mask;

	public Texture Scratches;

	[Range(0f, 1f)]
	public float ScratcesIntensity = 1f;

	[SerializeField]
	private Material _glassDamageMaterial;

	[Range(0f, 1f)]
	public float MinLightValue = 0.15f;

	public bool IsAffectedByLight = true;

	private RenderTexture _E010;

	public GameObject VisorEffectPrefab;

	[Header("Blur")]
	public Texture BlurMask;

	[Range(0f, 2f)]
	public int downsample = 1;

	[Range(0f, 10f)]
	public float blurSize = 3f;

	[Range(1f, 4f)]
	public int blurIterations = 2;

	[Header("Distort")]
	public Texture DistortMask;

	public float DistortIntensity = 1f;

	[Header("Swing")]
	[Range(-1f, 1f)]
	public float SwingValue = 1f;

	public AnimationCurve DX;

	public AnimationCurve DY;

	public AnimationCurve HitSwingCurve = AnimationCurve.Linear(0f, 1f, 0f, 1f);

	public AnimationCurve WalkRotation;

	public float HitZoomMultiply = 1.5f;

	private bool _E011 = true;

	private float _E012;

	private Vector2 _E013;

	private float _E014;

	private Vector2 _E015 = Vector2.zero;

	private float _E016;

	private bool _E017 = true;

	[HideInInspector]
	public float StepFrequency;

	[HideInInspector]
	public float Velocity;

	private Material _E018;

	private Material _E019;

	private Dictionary<EMask, Texture> _E01A;

	[SerializeField]
	private Shader _blurShader;

	private Material _E01B;

	private GameObject _E01C;

	public bool Visible
	{
		get
		{
			return _E017;
		}
		set
		{
			_E017 = value;
			SetIntensity(Intensity);
		}
	}

	private void OnEnable()
	{
		_E008();
	}

	private void Start()
	{
		_E007();
	}

	private void OnDestroy()
	{
		Clear();
	}

	private void Update()
	{
	}

	private RenderTexture _E000()
	{
		if (_E010 != null)
		{
			return _E010;
		}
		_E010 = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Default)
		{
			name = _ED3E._E000(82079),
			autoGenerateMips = false,
			useMipMap = false
		};
		return _E010;
	}

	public void SetIntensity(float intensity)
	{
		Intensity = intensity;
		base.enabled = _E017 && intensity > 0f;
	}

	public void SetMask(EMask mask)
	{
		Material material = _E003();
		_E001();
		if (_E01A.ContainsKey(mask))
		{
			if (mask == EMask.Narrow)
			{
				_E002();
			}
			material.SetTexture(_ED3E._E000(86305), _E01A[mask]);
		}
	}

	public void DrawScratches(byte seed, int hitCount)
	{
		RenderTexture active = RenderTexture.active;
		Graphics.SetRenderTarget(_E000());
		GL.Clear(clearDepth: true, clearColor: true, Color.clear);
		UnityEngine.Random.InitState(seed);
		for (int i = 0; i < hitCount; i++)
		{
			Vector4 vector = new Vector4(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(1.6f, 2.6f));
			Draw(new Vector3(vector.x, vector.y), new Vector3(vector.w / _E8A8.Instance.Camera.aspect, vector.w), vector.z);
		}
		RenderTexture.active = active;
	}

	public void BreakCompletely(byte seed, bool isBroken)
	{
		if (isBroken)
		{
			RenderTexture active = RenderTexture.active;
			Graphics.SetRenderTarget(_E000());
			UnityEngine.Random.InitState(seed);
			float aspect = _E8A8.Instance.Camera.aspect;
			Vector4 vector = new Vector4(UnityEngine.Random.Range(-1f, 0f), -1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector.x, vector.y), new Vector3(vector.w / aspect, vector.w), vector.z);
			Vector4 vector2 = new Vector4(UnityEngine.Random.Range(0f, 1f), -1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector2.x, vector2.y), new Vector3(vector2.w / aspect, vector2.w), vector2.z);
			Vector4 vector3 = new Vector4(UnityEngine.Random.Range(-1f, 0f), 1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector3.x, vector3.y), new Vector3(vector3.w / aspect, vector3.w), vector3.z);
			Vector4 vector4 = new Vector4(UnityEngine.Random.Range(0f, 1f), 1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector4.x, vector4.y), new Vector3(vector4.w / aspect, vector4.w), vector4.z);
			Vector4 vector5 = new Vector4(-1.1f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector5.x, vector5.y), new Vector3(vector5.w / aspect, vector5.w), vector5.z);
			Vector4 vector6 = new Vector4(-1.1f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector6.x, vector6.y), new Vector3(vector6.w / aspect, vector6.w), vector6.z);
			Vector4 vector7 = new Vector4(1.1f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector7.x, vector7.y), new Vector3(vector7.w / aspect, vector7.w), vector7.z);
			Vector4 vector8 = new Vector4(1.1f, UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector8.x, vector8.y), new Vector3(vector8.w / aspect, vector8.w), vector8.z);
			Vector4 vector9 = new Vector4(UnityEngine.Random.Range(-0.5f, 0.5f), -1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector9.x, vector9.y), new Vector3(vector9.w / aspect, vector9.w), vector9.z);
			Vector4 vector10 = new Vector4(UnityEngine.Random.Range(-0.5f, 0.5f), 1.1f, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(2f, 3.2f));
			Draw(new Vector3(vector10.x, vector10.y), new Vector3(vector10.w / aspect, vector10.w), vector10.z);
			RenderTexture.active = active;
		}
	}

	public void Draw(Vector3 position, Vector3 scale, float rotation)
	{
		_glassDamageMaterial.SetPass(0);
		_glassDamageMaterial.SetFloat(_ED3E._E000(43507), rotation);
		float x = position.x - scale.x;
		float x2 = position.x + scale.x;
		float y = 0f - position.y - scale.y;
		float y2 = 0f - position.y + scale.y;
		GL.Begin(7);
		_glassDamageMaterial.SetPass(0);
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(x, y, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(x2, y, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(x2, y2, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(x, y2, 0f);
		GL.End();
	}

	public void Clear()
	{
		if (_E01C != null)
		{
			UnityEngine.Object.DestroyImmediate(_E01C);
		}
		UnityEngine.Object.DestroyImmediate(_E000());
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Intensity < 0.01f)
		{
			if (_E01C != null && _E01C.activeSelf)
			{
				_E01C.SetActive(value: false);
			}
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		if (_E01C != null && !_E01C.activeSelf)
		{
			_E01C.SetActive(value: true);
		}
		Material material = _E003();
		RenderTexture myTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		RenderTexture source2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
		_E005(material);
		material.SetFloat(_ED3E._E000(82061), ScratcesIntensity);
		material.SetFloat(_ED3E._E000(82104), DistortIntensity);
		material.SetFloat(_ED3E._E000(35970), Intensity);
		material.SetFloat(_ED3E._E000(82090), EdgeFadeCurve.Evaluate(Intensity));
		material.SetFloat(_ED3E._E000(82140), MinLightValue);
		material.SetFloat(_ED3E._E000(82131), Convert.ToInt32(IsAffectedByLight));
		DebugGraphics.Blit(source, temporary, material, 0);
		DebugGraphics.Blit(temporary, temporary2, material, 1);
		DebugGraphics.Blit(temporary2, source2, material, 2);
		Modify(ref source2, ref myTex);
		material.SetTexture(_ED3E._E000(43641), myTex);
		DebugGraphics.Blit(source2, destination, material, 3);
		RenderTexture.ReleaseTemporary(myTex);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(source2);
	}

	public void Modify(ref RenderTexture source, ref RenderTexture myTex)
	{
		Material material = _E004();
		float num = 1f / (1f * (float)(1 << downsample));
		material.SetVector(_ED3E._E000(82174), new Vector4(blurSize * num, (0f - blurSize) * num, 0f, 0f));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> downsample;
		int height = source.height >> downsample;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		DebugGraphics.Blit(source, renderTexture, material, 0);
		for (int i = 0; i < blurIterations; i++)
		{
			float num2 = (float)i * 1f;
			material.SetVector(_ED3E._E000(82174), new Vector4(blurSize * num + num2, (0f - blurSize) * num - num2, 0f, 0f));
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			DebugGraphics.Blit(renderTexture, temporary, material, 1);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			DebugGraphics.Blit(renderTexture, temporary, material, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, myTex);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	private void _E001()
	{
		Material material = _E003();
		material.SetTexture(_ED3E._E000(86305), Mask);
		material.SetTexture(_ED3E._E000(82161), Scratches);
		material.SetTexture(_ED3E._E000(88019), BlurMask);
		material.SetTexture(_ED3E._E000(82151), DistortMask);
		material.SetTexture(_ED3E._E000(82196), _E000());
	}

	private void _E002()
	{
		Material material = _E003();
		material.SetTexture(_ED3E._E000(86305), null);
		material.SetTexture(_ED3E._E000(82161), null);
		material.SetTexture(_ED3E._E000(88019), null);
		material.SetTexture(_ED3E._E000(82151), null);
		material.SetTexture(_ED3E._E000(82196), null);
	}

	private Material _E003()
	{
		if (_E018 != null)
		{
			return _E018;
		}
		return _E018 = new Material(VisorShader);
	}

	private Material _E004()
	{
		if (_E01B != null)
		{
			return _E01B;
		}
		if (_blurShader == null)
		{
			_blurShader = _E3AC.Find(_ED3E._E000(37954));
			if (_blurShader == null)
			{
				Debug.LogError(_ED3E._E000(82180));
			}
		}
		return _E01B = new Material(_blurShader);
	}

	private void _E005(Material material)
	{
		if (!(StepFrequency < 0.01f))
		{
			if (Velocity < 0.001f)
			{
				_E013 = Vector2.Lerp(_E013, Vector2.zero, Time.deltaTime * 10f);
				_E014 = Mathf.Lerp(_E014, 0f, Time.deltaTime * 10f);
				_E012 = 0f;
			}
			else
			{
				_E012 += Time.deltaTime * StepFrequency;
				_E013 = new Vector2(DX.Evaluate(_E012), DY.Evaluate(_E012));
				_E014 = WalkRotation.Evaluate(_E012);
			}
			_E013 += _E015;
			_E014 += _E015.y;
			material.SetFloat(_ED3E._E000(43507), _E014);
			material.SetVector(_ED3E._E000(82217), _E013);
			material.SetFloat(_ED3E._E000(82269), _E016);
		}
	}

	private IEnumerator _E006()
	{
		float num = UnityEngine.Random.Range(-1f, 1f);
		float num2 = UnityEngine.Random.Range(-1f, 1f);
		_E011 = false;
		for (float num3 = 0f; num3 < 1f; num3 += 0.1f)
		{
			float num4 = HitSwingCurve.Evaluate(num3);
			float x = num * num4;
			float y = num2 * num4;
			_E015 = new Vector2(x, y);
			_E016 = num4 * HitZoomMultiply;
			yield return null;
		}
		_E015 = Vector2.zero;
		_E016 = 0f;
		_E011 = true;
	}

	private void _E007()
	{
		if (!(_E01C != null) && Application.isPlaying)
		{
			_E01C = UnityEngine.Object.Instantiate(VisorEffectPrefab);
			_E01C.transform.SetParent(base.transform, worldPositionStays: false);
			_E01C.name = _ED3E._E000(82259);
			_E01C.SetActive(value: false);
		}
	}

	private void _E008()
	{
		_E01A = new Dictionary<EMask, Texture>();
		for (int i = 0; i < VisorMasks.Count; i++)
		{
			_E01A.Add(VisorMasks[i].EMask, VisorMasks[i].Texture);
		}
	}
}
