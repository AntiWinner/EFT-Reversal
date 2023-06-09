using System;
using EFT.BlitDebug;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/SunOnGlass")]
[ExecuteInEditMode]
public class SunOnGlass : MonoBehaviour
{
	[Serializable]
	public class SunBack
	{
		public Transform Light;

		public Texture SunTexture;

		public Color SunColor;

		public float size = 100f;

		public Transform _trans;

		private Vector3 _position;

		public void Initialize(bool formEditor = false)
		{
			if (Application.isPlaying || formEditor)
			{
				_trans = CreatePolygon();
				_trans.gameObject.hideFlags = HideFlags.DontSave;
				_trans.name = _ED3E._E000(83469);
				_trans.rotation = Light.rotation;
				_position = Light.rotation * Vector3.back * 800f;
				_trans.position = _position;
				_trans.localScale = new Vector3(size, size, 0.1f);
				Debug.LogError(_ED3E._E000(83463));
				Material material = new Material(_E3AC.Find(_ED3E._E000(37375)));
				material.hideFlags = HideFlags.HideAndDontSave;
				_trans.GetComponent<Renderer>().material = material;
				material.mainTexture = SunTexture;
			}
		}

		public void UpdatePos(Vector3 camPos)
		{
			if (!(_trans == null))
			{
				_position = Light.rotation * Vector3.back * 800f;
				_trans.position = _position + camPos;
			}
		}

		public Transform GetSun()
		{
			return _trans;
		}

		public Transform CreatePolygon()
		{
			GameObject gameObject = new GameObject(_ED3E._E000(83518), typeof(MeshFilter), typeof(MeshRenderer));
			gameObject.GetComponent<MeshFilter>().mesh = new Mesh
			{
				vertices = new Vector3[4]
				{
					new Vector3(-0.5f, -0.5f),
					new Vector3(-0.5f, 0.5f),
					new Vector3(0.5f, -0.5f),
					new Vector3(0.5f, 0.5f)
				},
				uv = new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 0f),
					new Vector2(1f, 1f)
				},
				triangles = new int[6] { 0, 2, 3, 3, 1, 0 },
				name = _ED3E._E000(83510)
			};
			return gameObject.transform;
		}
	}

	public float Size = 1f;

	public Transform Sun;

	public Texture ScreenTexture;

	public Shader ScreenShader;

	public Shader VisibilityCheckerShader;

	public CustomLensFlare LensFlares;

	public SunBack SunBackGround;

	public Color SunColor;

	public AnimationCurve SunCurve;

	private bool m__E000 = true;

	private bool m__E001 = true;

	private bool m__E002 = true;

	private Texture m__E003;

	private Material m__E004;

	private Material m__E005;

	private Camera _E006;

	private Transform _E007;

	private Texture2D _E008;

	private RenderTexture _E009;

	private RenderTexture _E00A;

	private Rect _E00B;

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(83451));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(83438));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(33344));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(83425));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(83481));

	public int Settings
	{
		set
		{
			switch (value)
			{
			case 0:
				this.m__E000 = false;
				this.m__E001 = false;
				this.m__E002 = true;
				break;
			case 1:
				this.m__E000 = false;
				this.m__E001 = true;
				this.m__E002 = true;
				break;
			case 2:
				this.m__E000 = true;
				this.m__E001 = true;
				this.m__E002 = true;
				break;
			default:
				this.m__E000 = true;
				this.m__E001 = true;
				this.m__E002 = true;
				break;
			}
		}
	}

	public void Start()
	{
		if (!SystemInfo.supportsImageEffects || SystemInfo.graphicsShaderLevel < 30)
		{
			base.enabled = false;
			return;
		}
		if (ScreenShader == null)
		{
			Debug.Log(_ED3E._E000(83343));
			base.enabled = false;
		}
		_E006 = GetComponent<Camera>();
		_E007 = _E006.transform;
		_E006.depthTextureMode |= DepthTextureMode.Depth;
		SunBackGround.Initialize();
		LensFlares.Initialize();
		this.m__E003 = GetTexFromCurve(SunCurve);
	}

	private void Update()
	{
		if (this.m__E002)
		{
			SunBackGround.UpdatePos(base.transform.position);
		}
	}

	private Material _E000()
	{
		if (this.m__E005 == null)
		{
			this.m__E005 = new Material(VisibilityCheckerShader);
			this.m__E005.hideFlags = HideFlags.HideAndDontSave;
		}
		return this.m__E005;
	}

	private Material _E001()
	{
		if (this.m__E004 == null)
		{
			this.m__E004 = new Material(ScreenShader);
			this.m__E004.hideFlags = HideFlags.HideAndDontSave;
			this.m__E004.SetTexture(_E00C, ScreenTexture);
			this.m__E004.SetTexture(_E00D, this.m__E003);
		}
		return this.m__E004;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if ((!this.m__E000 && !this.m__E001) || !_E004(-Sun.forward, out var onScreen))
		{
			_E3A1.BlitOrCopy(source, destination);
			return;
		}
		Vector4 vector = new Vector4(onScreen.x, onScreen.y, (float)Screen.width / (float)Screen.height, Size);
		_E002(source, destination, vector);
		if (this.m__E000)
		{
			Material material = _E001();
			Shader.SetGlobalColor(_E00E, _E003(SunColor));
			material.SetVector(_E00F, vector);
			if (Application.isEditor)
			{
				this.m__E004.SetTexture(_E00C, ScreenTexture);
				this.m__E004.SetTexture(_E00D, this.m__E003);
			}
			Graphics.Blit(source, destination, material);
		}
		else
		{
			Graphics.CopyTexture(source, destination);
		}
		if (this.m__E001)
		{
			LensFlares.Draw(onScreen);
		}
	}

	private void OnDestroy()
	{
		if (SunBackGround != null && (bool)SunBackGround.GetSun())
		{
			UnityEngine.Object.DestroyImmediate(SunBackGround.GetSun().gameObject);
		}
	}

	private void OnApplicationQuit()
	{
		if (SunBackGround.GetSun() != null)
		{
			UnityEngine.Object.DestroyImmediate(SunBackGround.GetSun().gameObject);
		}
	}

	private void _E002(RenderTexture source, RenderTexture destination, Vector4 sunPos)
	{
		Material material = _E000();
		material.SetVector(_E00F, sunPos);
		if (_E009 == null)
		{
			_E009 = new RenderTexture(4, 4, 0)
			{
				name = _ED3E._E000(83360)
			};
		}
		if (_E00A == null)
		{
			_E00A = new RenderTexture(1, 1, 0)
			{
				name = _ED3E._E000(83408)
			};
			Shader.SetGlobalTexture(_E010, _E00A);
		}
		DebugGraphics.Blit(source, destination, material, 2);
		DebugGraphics.Blit(destination, _E009, material, 0);
		DebugGraphics.Blit(_E009, _E00A, material, 1);
	}

	private static Color _E003(Color color)
	{
		float num = 1f / ((color.r + color.g + color.b) / 3f);
		color.a = num - 1f;
		return color;
	}

	private bool _E004(Vector3 lightDirection, out Vector2 onScreen)
	{
		Vector3 position = _E007.position;
		Vector3 vector = _E006.WorldToViewportPoint(position + lightDirection);
		onScreen = vector;
		if (vector.z < 0f)
		{
			return false;
		}
		if (!_E005(vector))
		{
			return false;
		}
		return true;
	}

	private bool _E005(Vector2 lightPos)
	{
		if (lightPos.x < -0.2f)
		{
			return false;
		}
		if (lightPos.x > 1.2f)
		{
			return false;
		}
		if (lightPos.y < -0.2f)
		{
			return false;
		}
		if (lightPos.y > 1.2f)
		{
			return false;
		}
		return true;
	}

	public static Texture2D GetTexFromCurve(AnimationCurve curve, int width = 255)
	{
		Texture2D texture2D = new Texture2D(width, 1, TextureFormat.Alpha8, mipChain: false);
		texture2D.name = _ED3E._E000(83392);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		float num = 1f / (float)width;
		float num2 = 0f;
		for (int i = 0; i < width; i++)
		{
			float num3 = Mathf.Clamp(curve.Evaluate(num2), 0f, 1f);
			texture2D.SetPixel(i, 0, new Color(num3, num3, num3, num3));
			num2 += num;
		}
		texture2D.Apply();
		return texture2D;
	}
}
