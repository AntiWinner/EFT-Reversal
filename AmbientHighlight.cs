using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[_E2E2(19100)]
[ExecuteInEditMode]
public class AmbientHighlight : OnRenderObjectConnector
{
	[Serializable]
	private struct AmbientDirectionalLight
	{
		public Transform LightTransform;

		public Color Color;

		public float Intensity;
	}

	public enum StencilType
	{
		Static,
		Characters,
		Hands
	}

	[Serializable]
	private class HighlightSettings
	{
		public float HighlightMinMultiplier = 0.2f;

		public float HighlightMaxMultiplier = 1.7f;

		public AnimationCurve HighlightIntensityCurve = new AnimationCurve(new Keyframe(-1f, 0f), new Keyframe(0f, 0f), new Keyframe(0.3f, 1f));

		public StencilType StencilTypeToUse = StencilType.Hands;

		[HideInInspector]
		public Material HighlightMaterial;
	}

	[_E3EC("Use method SetSH to set spherical harmonics")]
	public Shader ScreenAmbientShader;

	public float AmbientBlur = 1f;

	[SerializeField]
	private HighlightSettings[] _highlightSettings = new HighlightSettings[0];

	[SerializeField]
	private AmbientDirectionalLight[] _additionalLights;

	private Material m__E000;

	private _E42E m__E001 = new _E42E(_ED3E._E000(41368), CameraEvent.AfterLighting);

	private static Mesh _E002;

	private static readonly Matrix4x4 _E003 = Matrix4x4.identity;

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(41354));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(41404));

	private static readonly int[] _E006 = new int[3]
	{
		Shader.PropertyToID(_ED3E._E000(41398)),
		Shader.PropertyToID(_ED3E._E000(41388)),
		Shader.PropertyToID(_ED3E._E000(41386))
	};

	private static readonly int[] _E007 = new int[3]
	{
		Shader.PropertyToID(_ED3E._E000(41376)),
		Shader.PropertyToID(_ED3E._E000(41438)),
		Shader.PropertyToID(_ED3E._E000(41428))
	};

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(41426));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(41423));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(41468));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(41449));

	public override void ManualOnRenderObject(Camera currentCamera)
	{
		base.ManualOnRenderObject(currentCamera);
		if (_highlightSettings != null && _highlightSettings.Length != 0 && !(currentCamera != _E8A8.Instance.Camera))
		{
			this.m__E001.OnRenderObject(out var buffer);
			_E000(buffer, currentCamera);
		}
	}

	private void _E000(CommandBuffer ambientBuffer, Camera currentCamera)
	{
		if (!_E002)
		{
			_E002 = GetQuadMesh();
		}
		ambientBuffer.Clear();
		BlendMode blendMode = (currentCamera.allowHDR ? BlendMode.One : BlendMode.DstColor);
		BlendMode blendMode2 = (currentCamera.allowHDR ? BlendMode.One : BlendMode.Zero);
		ambientBuffer.SetGlobalFloat(_E004, (float)blendMode);
		ambientBuffer.SetGlobalFloat(_E005, (float)blendMode2);
		ambientBuffer.SetRenderTarget(BuiltinRenderTextureType.CurrentActive, BuiltinRenderTextureType.CurrentActive);
		if (TOD_Sky.Instance == null || this.m__E000 == null)
		{
			return;
		}
		float y = TOD_Sky.Instance.SunDirection.y;
		HighlightSettings[] highlightSettings = _highlightSettings;
		foreach (HighlightSettings highlightSettings2 in highlightSettings)
		{
			if (!(highlightSettings2.HighlightMaterial == null))
			{
				float value = Mathf.Lerp(highlightSettings2.HighlightMinMultiplier, highlightSettings2.HighlightMaxMultiplier, highlightSettings2.HighlightIntensityCurve.Evaluate(y));
				highlightSettings2.HighlightMaterial.SetFloat(_E00A, value);
				highlightSettings2.HighlightMaterial.SetFloat(_E009, (float)highlightSettings2.StencilTypeToUse);
				ambientBuffer.DrawMesh(_E002, _E003, highlightSettings2.HighlightMaterial);
			}
		}
	}

	private void _E001()
	{
		this.m__E000 = new Material(ScreenAmbientShader);
		this.m__E000.SetFloat(_E00B, 1f / AmbientBlur);
		HighlightSettings[] highlightSettings = _highlightSettings;
		foreach (HighlightSettings highlightSettings2 in highlightSettings)
		{
			if (highlightSettings2.HighlightMaterial == null)
			{
				highlightSettings2.HighlightMaterial = new Material(this.m__E000);
			}
		}
		foreach (KeyValuePair<Camera, CommandBuffer> camera in this.m__E001.Cameras)
		{
			_E000(camera.Value, camera.Key);
		}
	}

	private void OnValidate()
	{
		_E001();
	}

	protected override void Start()
	{
		base.Start();
		_E001();
	}

	private void OnEnable()
	{
		_E001();
	}

	private void OnDisable()
	{
		this.m__E001.DestroyBuffers();
		HighlightSettings[] highlightSettings = _highlightSettings;
		foreach (HighlightSettings highlightSettings2 in highlightSettings)
		{
			if (highlightSettings2.HighlightMaterial != null)
			{
				UnityEngine.Object.DestroyImmediate(highlightSettings2.HighlightMaterial);
				highlightSettings2.HighlightMaterial = null;
			}
		}
	}

	public void SetSH(SphericalHarmonicsL2 sh)
	{
		AmbientDirectionalLight[] additionalLights = _additionalLights;
		for (int i = 0; i < additionalLights.Length; i++)
		{
			AmbientDirectionalLight ambientDirectionalLight = additionalLights[i];
			if (!(ambientDirectionalLight.LightTransform == null))
			{
				sh.AddDirectionalLight(-ambientDirectionalLight.LightTransform.forward, ambientDirectionalLight.Color, ambientDirectionalLight.Intensity);
			}
		}
		HighlightSettings[] highlightSettings = _highlightSettings;
		foreach (HighlightSettings highlightSettings2 in highlightSettings)
		{
			if (!(highlightSettings2.HighlightMaterial == null))
			{
				highlightSettings2.HighlightMaterial.SetVector(_E008, new Vector4(sh[0, 8], sh[1, 8], sh[2, 8], 1f));
				for (int j = 0; j < 3; j++)
				{
					highlightSettings2.HighlightMaterial.SetVector(_E006[j], new Vector4(sh[j, 3], sh[j, 1], sh[j, 2], sh[j, 0] - sh[j, 6]));
					highlightSettings2.HighlightMaterial.SetVector(_E007[j], new Vector4(sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]));
				}
			}
		}
	}

	public static Mesh GetQuadMesh()
	{
		float z = 0.1f;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, z),
			new Vector3(1f, -1f, z),
			new Vector3(-1f, 1f, z),
			new Vector3(1f, 1f, z)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 1, 3, 3, 2, 0 };
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.name = _ED3E._E000(41299);
		mesh.RecalculateBounds();
		mesh.name = _ED3E._E000(41328);
		return mesh;
	}
}
