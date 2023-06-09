using System;
using Comfort.Common;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera Scattering")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class TOD_Scattering : TOD_ImageEffect
{
	public bool Lighten;

	public bool FromLevelSettings = true;

	public Shader ScatteringShader;

	public Texture2D DitheringTexture;

	[Range(0f, 0.2f)]
	public float GlobalDensity = 0.001f;

	[Range(0f, 1f)]
	public float HeightFalloff = 0.001f;

	[Range(0.95f, 1f)]
	public float SunrizeGlow = 0.95f;

	public float ZeroLevel;

	private Material _E00E;

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(18590));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(18568));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(16719));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(18618));

	protected void Start()
	{
		if (!ScatteringShader)
		{
			ScatteringShader = _E3AC.Find(_ED3E._E000(18496));
		}
		_E00E = CreateMaterial(ScatteringShader);
	}

	protected void OnDestroy()
	{
		if ((bool)_E00E)
		{
			UnityEngine.Object.DestroyImmediate(_E00E);
		}
	}

	[ImageEffectOpaque]
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckSupport(needDepth: true, needHdr: true))
		{
			Graphics.Blit(source, destination);
			return;
		}
		sky.Components.Scattering = this;
		float nearClipPlane = cam.nearClipPlane;
		float farClipPlane = cam.farClipPlane;
		float fieldOfView = cam.fieldOfView;
		float aspect = cam.aspect;
		Vector3 position = cam.transform.position;
		Vector3 forward = cam.transform.forward;
		Vector3 right = cam.transform.right;
		Vector3 up = cam.transform.up;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = fieldOfView * 0.5f;
		Vector3 vector = right * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f)) * aspect;
		Vector3 vector2 = up * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f));
		Vector3 vector3 = forward * nearClipPlane - vector + vector2;
		float num2 = vector3.magnitude * farClipPlane / nearClipPlane;
		vector3.Normalize();
		vector3 *= num2;
		Vector3 vector4 = forward * nearClipPlane + vector + vector2;
		vector4.Normalize();
		vector4 *= num2;
		Vector3 vector5 = forward * nearClipPlane + vector - vector2;
		vector5.Normalize();
		vector5 *= num2;
		Vector3 vector6 = forward * nearClipPlane - vector - vector2;
		vector6.Normalize();
		vector6 *= num2;
		identity.SetRow(0, vector3);
		identity.SetRow(1, vector4);
		identity.SetRow(2, vector5);
		identity.SetRow(3, vector6);
		if (FromLevelSettings)
		{
			LevelSettings instance = Singleton<LevelSettings>.Instance;
			if (instance != null)
			{
				HeightFalloff = instance.HeightFalloff;
				ZeroLevel = instance.ZeroLevel;
			}
		}
		_E00E.SetMatrix(_E00F, identity);
		_E00E.SetTexture(_E010, DitheringTexture);
		Shader.SetGlobalVector(_E011, new Vector4(HeightFalloff, position.y - ZeroLevel, GlobalDensity, 0f));
		_E00E.SetFloat(_E012, SunrizeGlow);
		if (Lighten)
		{
			_E00E.EnableKeyword(_ED3E._E000(18534));
		}
		else
		{
			_E00E.DisableKeyword(_ED3E._E000(18534));
		}
		CustomBlit(source, destination, _E00E);
	}
}
