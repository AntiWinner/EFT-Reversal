using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Image Effects/ScreenWater")]
[RequireComponent(typeof(Camera))]
public class ScreenWater : MonoBehaviour
{
	[CompilerGenerated]
	private float _E000;

	public Shader shaderRGB;

	public float _distortionCooldownTime = 2f;

	public Texture2D WaterFlows;

	public Texture2D WaterMask;

	public Texture2D WetScreen;

	public float Speed = 0.5f;

	public float InitialIntensity = 0.1f;

	private Material _E001;

	private float _E002;

	private bool _E003;

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(84332));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(84331));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(84321));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(84376));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(84375));

	public float Intensity
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	protected Material material
	{
		get
		{
			if (_E001 == null)
			{
				_E001 = new Material(shaderRGB);
				_E001.hideFlags = HideFlags.HideAndDontSave;
			}
			return _E001;
		}
	}

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else if (shaderRGB == null)
		{
			Debug.Log(_ED3E._E000(84267));
			base.enabled = false;
		}
		else if (!shaderRGB.isSupported)
		{
			base.enabled = false;
		}
	}

	protected void OnDestroy()
	{
		if ((bool)_E001)
		{
			Object.DestroyImmediate(_E001);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Intensity < Mathf.Epsilon || !_E003)
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		if (RainController.IsCameraUnderRain)
		{
			_E002 += Time.deltaTime / _distortionCooldownTime;
		}
		else
		{
			_E002 -= Time.deltaTime / _distortionCooldownTime;
		}
		_E002 = Mathf.Clamp01(_E002);
		Material material = this.material;
		material.SetTexture(_E004, WaterFlows);
		material.SetTexture(_E005, WaterMask);
		material.SetTexture(_E006, WetScreen);
		material.SetFloat(_E007, Speed);
		material.SetFloat(_E008, InitialIntensity * Intensity * _E002);
		Graphics.Blit(source, destination, material);
	}

	public void SetIntensity(float intensity)
	{
		Intensity = intensity;
		base.enabled = intensity > 0f;
	}

	public void ChangeGlassesState(bool hasGlases)
	{
		_E003 = hasGlases;
	}
}
