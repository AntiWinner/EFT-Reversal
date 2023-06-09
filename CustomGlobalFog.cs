using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/CustomGlobalFog")]
[DisallowMultipleComponent]
public class CustomGlobalFog : MonoBehaviour
{
	public enum BlendModes
	{
		Normal,
		Lighten,
		Screen,
		Overlay,
		SoftLight
	}

	public Shader shader;

	public BlendModes BlendMode;

	public Color FogColor;

	public float FogStrength;

	public float FogY;

	public float FogToplength;

	public float FogTopIntensity;

	public float FogMaxDistance;

	public float FogStart;

	public float DirectionDifferenceThreshold = 0.047f;

	public float FuncSoftness;

	public float FuncStart;

	private Material m__E000;

	private Mesh m__E001;

	private Camera m__E002;

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(44135));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(44186));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(44170));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(44163));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(44214));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(44196));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(44195));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(44241));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(44226));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(19728));

	private Material _E00D
	{
		get
		{
			if (this.m__E000 == null)
			{
				this.m__E000 = new Material(shader);
				this.m__E000.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m__E000;
		}
	}

	private void Start()
	{
		this.m__E001 = _E002();
		this.m__E002 = GetComponent<Camera>();
		if (this.m__E002.renderingPath != RenderingPath.DeferredShading)
		{
			this.m__E002.depthTextureMode |= DepthTextureMode.DepthNormals;
		}
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else if (shader == null)
		{
			Debug.Log(_ED3E._E000(41960));
			base.enabled = false;
		}
		else if (!shader.isSupported)
		{
			base.enabled = false;
		}
	}

	protected void OnDestroy()
	{
		if (this.m__E001 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E001);
		}
		if ((bool)this.m__E000)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E000);
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		FogToplength = Math.Max(FogToplength, 1E-13f);
		FuncStart = Math.Max(FuncStart, 1E-13f);
		Material material = _E00D;
		material.SetFloat(_E003, (float)BlendMode);
		Shader.SetGlobalMatrix(_E004, this.m__E002.cameraToWorldMatrix);
		Shader.SetGlobalFloat(_E005, FogMaxDistance);
		Shader.SetGlobalColor(_E006, FogColor);
		Shader.SetGlobalFloat(_E007, FogStrength);
		Shader.SetGlobalFloat(_E008, FogY);
		Shader.SetGlobalVector(_E009, new Vector4(FuncStart, FuncSoftness, FuncSoftness / FuncStart));
		Shader.SetGlobalVector(_E00A, new Vector4(0f - FogToplength, 0.5f / FogToplength, FogTopIntensity, FuncSoftness * Mathf.Log(FuncSoftness / FuncStart)));
		Shader.SetGlobalFloat(_E00B, FogStart);
		source.filterMode = FilterMode.Bilinear;
		_E001(source, destination, material);
	}

	private static void _E000(Camera cam, out Vector3 topLeft, out Vector3 topRight, out Vector3 bottomLeft, out Vector3 bottomRight)
	{
		float farClipPlane = cam.farClipPlane;
		Vector3 position = cam.transform.position;
		topLeft = cam.ViewportToWorldPoint(new Vector3(0f, 1f, farClipPlane)) - position;
		topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, farClipPlane)) - position;
		bottomLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0f, farClipPlane)) - position;
		bottomRight = cam.ViewportToWorldPoint(new Vector3(1f, 0f, farClipPlane)) - position;
	}

	protected void CustomBlit(Camera cam, RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr = 0)
	{
		RenderTexture.active = dest;
		fxMaterial.SetTexture(_E00C, source);
		_E000(cam, out var topLeft, out var topRight, out var bottomLeft, out var bottomRight);
		GL.PushMatrix();
		GL.LoadOrtho();
		fxMaterial.SetPass(passNr);
		GL.Begin(7);
		GL.MultiTexCoord(0, bottomLeft);
		GL.Vertex3(0f, 0f, 0.1f);
		GL.MultiTexCoord(0, bottomRight);
		GL.Vertex3(1f, 0f, 0.1f);
		GL.MultiTexCoord(0, topRight);
		GL.Vertex3(1f, 1f, 0.1f);
		GL.MultiTexCoord(0, topLeft);
		GL.Vertex3(0f, 1f, 0.1f);
		GL.End();
		GL.PopMatrix();
	}

	private void _E001(RenderTexture source, RenderTexture dest, Material mat)
	{
		RenderTexture.active = dest;
		mat.SetTexture(_E00C, source);
		mat.SetPass(0);
		Graphics.DrawMeshNow(this.m__E001, Matrix4x4.identity);
	}

	private static Mesh _E002()
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, -0.1f),
			new Vector3(-1f, 1f, -0.1f),
			new Vector3(1f, 1f, -0.1f),
			new Vector3(1f, -1f, -0.1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
		return new Mesh
		{
			vertices = vertices,
			triangles = triangles,
			bounds = new Bounds(Vector3.zero, Vector3.one * 2.1474836E+09f),
			name = _ED3E._E000(44156)
		};
	}
}
