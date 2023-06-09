using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TextureDecalsPainter : MonoBehaviour
{
	public struct _E000
	{
		public Renderer Renderer;

		public Vector3 Point;

		public Vector3 Normal;

		public Texture2D Texture;

		public bool HasToSetShaderVars;

		public bool HasToSetTexture;
	}

	[SerializeField]
	[Header("Render textures settings")]
	private PowOfTwoDimensions _renderTexDimension = PowOfTwoDimensions._256;

	[SerializeField]
	private DepthSize _renderTexDepthSize;

	[Header("Shaders")]
	[SerializeField]
	private Shader _drawInterceptionShader;

	[SerializeField]
	private Shader _blitShader;

	[SerializeField]
	[Header("Textures")]
	private Texture2D _bloodDecalTexture;

	[SerializeField]
	private Texture2D _vestDecalTexture;

	[SerializeField]
	private Texture2D _backDecalTexture;

	[SerializeField]
	[Header("Decal settings")]
	private float _projectorHeight = 0.2f;

	[SerializeField]
	[_E2BD(0f, 0.5f, -1f)]
	private Vector2 _decalSize = new Vector2(0.1f, 0.17f);

	private Material m__E000;

	private Material m__E001;

	private RenderTexture m__E002;

	private RenderTexture m__E003;

	private CommandBuffer m__E004;

	private Dictionary<Renderer, RenderTexture> m__E005 = new Dictionary<Renderer, RenderTexture>(20);

	private List<_E000> _E006 = new List<_E000>(20);

	private static readonly int _E007 = 20;

	private static int _E008 = 0;

	private const int _E009 = 128;

	private _E385<RenderTexture> _E00A;

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(43160));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(43159));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(43146));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(43198));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(43178));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(43169));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(43229));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(43226));

	public void Awake()
	{
		_E006 = new List<_E000>();
		this.m__E000 = new Material(_drawInterceptionShader);
		this.m__E001 = new Material(_blitShader);
		this.m__E002 = new RenderTexture((int)_renderTexDimension, (int)_renderTexDimension, (int)_renderTexDepthSize)
		{
			name = _ED3E._E000(45026)
		};
		this.m__E003 = new RenderTexture((int)_renderTexDimension, (int)_renderTexDimension, (int)_renderTexDepthSize)
		{
			name = _ED3E._E000(43064)
		};
		this.m__E004 = new CommandBuffer
		{
			name = _ED3E._E000(43101)
		};
		_E00A = new _E385<RenderTexture>(128, _E000, null, _E001);
	}

	private RenderTexture _E000()
	{
		RenderTexture renderTexture = new RenderTexture((int)_renderTexDimension, (int)_renderTexDimension, (int)_renderTexDepthSize);
		renderTexture.name = _ED3E._E000(43073) + _E008++;
		renderTexture.useMipMap = false;
		renderTexture.Create();
		return renderTexture;
	}

	private void _E001(RenderTexture rendererDecalTex)
	{
		rendererDecalTex.Release();
		Object.DestroyImmediate(rendererDecalTex);
	}

	private void Update()
	{
		if (_E006.Count == 0)
		{
			return;
		}
		int num = Mathf.Min(_E006.Count, _E007);
		for (int i = 0; i < num; i++)
		{
			_E000 item = _E006[0];
			if (item.HasToSetShaderVars)
			{
				_E002(item.Point, -item.Normal);
			}
			if (item.HasToSetTexture)
			{
				this.m__E000.SetTexture(_E00B, item.Texture);
			}
			DrawDecal(item.Renderer);
			_E006.Remove(item);
		}
	}

	public void DrawDecal(Renderer objRenderer)
	{
		if (!this.m__E005.TryGetValue(objRenderer, out var value))
		{
			value = _E00A.Withdraw();
			objRenderer.PreventMaterialChangeInEditor();
			Material material = objRenderer.material;
			material.SetTexture(_E00C, value);
			material.SetFloat(_E00D, 1f);
			this.m__E005.Add(objRenderer, value);
		}
		this.m__E004.Clear();
		this.m__E004.SetRenderTarget(this.m__E002);
		this.m__E004.ClearRenderTarget(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f));
		this.m__E004.DrawRenderer(objRenderer, this.m__E000);
		this.m__E001.SetTexture(_E00E, value);
		this.m__E004.Blit(this.m__E002, this.m__E003, this.m__E001);
		this.m__E004.Blit(this.m__E003, value);
		Graphics.ExecuteCommandBuffer(this.m__E004);
	}

	private void _E002(Vector3 point, Vector3 normal)
	{
		Vector4 vector = normal.normalized;
		float num = Mathf.Abs(Vector3.Dot(vector, Vector3.up));
		Vector4 vector2 = Vector3.Cross(vector, (num > 0.999f) ? Vector3.right : Vector3.up).normalized;
		Vector4 value = Vector3.Cross(vector, vector2).normalized;
		float w = (vector2.w = Random.Range(_decalSize.x, _decalSize.y));
		vector.w = _projectorHeight;
		value.w = w;
		this.m__E000.SetVector(_E00F, vector2);
		this.m__E000.SetVector(_E010, vector);
		this.m__E000.SetVector(_E011, value);
		this.m__E000.SetVector(_E012, point);
	}

	private Texture2D _E003(EDecalTextureType type)
	{
		return type switch
		{
			EDecalTextureType.Blood => _bloodDecalTexture, 
			EDecalTextureType.Metal => _vestDecalTexture, 
			EDecalTextureType.Fabric => _backDecalTexture, 
			_ => _backDecalTexture, 
		};
	}

	public void DrawDecal(List<_E3D2> renderers, Vector3 point, Vector3 normal)
	{
		int num = 0;
		foreach (_E3D2 renderer in renderers)
		{
			Texture2D texture = _E003(renderer.DecalType);
			int num2 = 0;
			for (int i = 0; i < renderer.Renderers.Length; i++)
			{
				Renderer objRenderer = renderer.Renderers[i];
				if (_E005(objRenderer))
				{
					bool hasToSetShaderVars = num == 0;
					bool hasToSetTexture = num2 == 0;
					_E004(objRenderer, point, normal, texture, hasToSetShaderVars, hasToSetTexture);
					num++;
					num2++;
				}
			}
		}
	}

	private void _E004(Renderer objRenderer, Vector3 point, Vector3 normal, Texture2D texture, bool hasToSetShaderVars, bool hasToSetTexture)
	{
		_E000 obj = default(_E000);
		obj.Renderer = objRenderer;
		obj.Point = point;
		obj.Normal = normal;
		obj.Texture = texture;
		obj.HasToSetShaderVars = hasToSetShaderVars;
		obj.HasToSetTexture = hasToSetTexture;
		_E000 item = obj;
		_E006.Add(item);
	}

	private bool _E005(Renderer objRenderer)
	{
		if (objRenderer.enabled && objRenderer.gameObject.activeSelf)
		{
			return objRenderer.material.shader.name.Contains(_ED3E._E000(43160));
		}
		return false;
	}

	private void OnDestroy()
	{
		foreach (KeyValuePair<Renderer, RenderTexture> item in this.m__E005)
		{
			item.Value.Release();
		}
		this.m__E005.Clear();
		_E00A.Dispose();
	}
}
