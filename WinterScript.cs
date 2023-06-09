using System.Collections.Generic;
using UnityEngine;

public class WinterScript : MonoBehaviour
{
	public class _E000
	{
		private AudioClip m__E000;

		private float[] _E001;

		private float[] _E002;

		private float[] _E003;

		private int _E004;

		public _E000(AudioClip sourceClip, float[] addData)
		{
			if (sourceClip == null)
			{
				Debug.Log(_ED3E._E000(96231));
			}
			if (addData == null)
			{
				Debug.Log(_ED3E._E000(94226));
			}
			m__E000 = sourceClip;
			_E002 = new float[sourceClip.samples];
			sourceClip.GetData(_E002, 0);
			_E001 = new float[_E002.Length];
			for (int i = 0; i < _E001.Length; i++)
			{
				_E001[i] = _E002[i];
			}
			_E003 = addData;
			_E004 = ((sourceClip.samples < addData.Length) ? sourceClip.samples : addData.Length);
		}

		public void Add(float f)
		{
			for (int i = 0; i < _E004; i++)
			{
				_E001[i] = _E002[i] + (_E003[i] - _E002[i]) * f;
			}
			m__E000.SetData(_E001, 0);
		}
	}

	public class _E001
	{
		private class _E000
		{
			private static readonly Color m__E000 = Color.white;

			private DetailPrototype _E001;

			private Color _E002;

			private Color _E003;

			public _E000(DetailPrototype prototype)
			{
				_E001 = prototype;
				_E002 = prototype.dryColor;
				_E003 = prototype.healthyColor;
			}

			public void Lerp(float t)
			{
				if (t >= 1f)
				{
					_E001.dryColor = WinterScript._E001._E000.m__E000;
					_E001.healthyColor = WinterScript._E001._E000.m__E000;
				}
				else
				{
					_E001.dryColor = _E000(_E002, WinterScript._E001._E000.m__E000, t);
					_E001.healthyColor = _E000(_E003, WinterScript._E001._E000.m__E000, t);
				}
			}

			private static Color _E000(Color a, Color b, float t)
			{
				return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
			}
		}

		private TerrainData m__E000;

		private DetailPrototype[] m__E001;

		private _E000[] _E002;

		private int[] _E003;

		private Color32[] _E004;

		private Color32[] _E005;

		private Texture2D _E006;

		private float _E007;

		public _E001(Terrain terrain, Texture2D detailTex)
		{
			Debug.Log(_ED3E._E000(94210));
			if (terrain == null)
			{
				Debug.Log(_ED3E._E000(95634));
			}
			if (detailTex == null)
			{
				Debug.Log(_ED3E._E000(94244));
			}
			this.m__E000 = terrain.terrainData;
			this.m__E001 = this.m__E000.detailPrototypes;
			if (this.m__E000 == null)
			{
				Debug.Log(_ED3E._E000(94294));
			}
			if (this.m__E001 == null)
			{
				Debug.Log(_ED3E._E000(94275));
			}
			if (this.m__E001.Length == 0)
			{
				Debug.Log(_ED3E._E000(94319));
			}
			if (this.m__E001.Length != 0)
			{
				_E004 = detailTex.GetPixels32();
				_E005 = new Color32[_E004.Length];
				for (int i = 0; i < _E005.Length; i++)
				{
					_E005[i] = _E004[i];
				}
				_E006 = new Texture2D(detailTex.width, detailTex.height, TextureFormat.RGBA32, mipChain: true);
				_E006.name = _ED3E._E000(94354);
				_E002 = new _E000[this.m__E001.Length];
				for (int j = 0; j < this.m__E001.Length; j++)
				{
					this.m__E001[j].prototypeTexture = _E006;
					_E002[j] = new _E000(this.m__E001[j]);
				}
			}
			_E003 = new int[256];
		}

		public void Update(float t)
		{
			if (Mathf.Abs(_E007 - t) < 0.02f)
			{
				return;
			}
			_E007 = t;
			int num = (int)(t * 140f) - 50;
			t *= 2f;
			if (_E002 == null)
			{
				Debug.Log(_ED3E._E000(94343));
			}
			_E000[] array = _E002;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					Debug.Log(_ED3E._E000(94376));
				}
			}
			array = _E002;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Lerp(t);
			}
			for (int j = 0; j < 256; j++)
			{
				int num2 = (j >> 1) + num;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				_E003[j] = num2;
			}
			_E000(_E004, _E005, _E003);
			if (_E006 == null)
			{
				Debug.Log(_ED3E._E000(94423));
			}
			if (this.m__E000 == null)
			{
				Debug.Log(_ED3E._E000(94400));
			}
			if (this.m__E001 == null)
			{
				Debug.Log(_ED3E._E000(94446));
			}
			_E006.SetPixels32(_E005);
			_E006.Apply(updateMipmaps: true);
			this.m__E000.detailPrototypes = this.m__E001;
		}

		private static void _E000(Color32[] source, Color32[] current, int[] green)
		{
			for (int i = 0; i < source.Length; i++)
			{
				int b = green[source[i].g];
				current[i].r = _E001(source[i].r, b);
				current[i].g = _E001(source[i].g, b);
				current[i].b = _E001(source[i].b, b);
			}
		}

		private static byte _E001(byte a, int b)
		{
			b += a;
			if (b > 255)
			{
				b = 255;
			}
			return (byte)b;
		}
	}

	public float debugValue;

	public bool debugWrite;

	public Texture SnowTex;

	public float StartTime;

	public AnimationCurve SnowLevelCurve;

	public AnimationCurve SnowFallingCurve;

	public AnimationCurve DesaturateSunCurve;

	public AnimationCurve SunIntensityCurve = AnimationCurve.Linear(0f, 1f, 100f, 1f);

	public AnimationCurve SoundsLerpCurve;

	public AudioClip[] SnowStepClip;

	public AudioClip SnowyWind;

	public AnimationCurve MusicFadeOut;

	public AnimationCurve MusicFadeIn;

	public AnimationCurve BreathCurve;

	public float FadeShadow = 0.4f;

	public float FadeScratches = 0.7f;

	public float FadeFog = 1.2f;

	private Light m__E000;

	private Color m__E001;

	private Color m__E002;

	private float m__E003;

	private float m__E004;

	private float m__E005;

	private AudioSource m__E006;

	private AudioSource _E007;

	private LinkedList<_E000> _E008 = new LinkedList<_E000>();

	public Transform BreathSystem;

	private Transform _E009;

	private ParticleSystem _E00A;

	private Transform _E00B;

	public LayerMask DepthRendererMask;

	public Material DepthMaterial;

	public Material TerrainMaterial;

	private _E001 _E00C;

	public AnimationCurve TerrainDetailCurve;

	public Texture2D[] TerrainDetails;

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(96236));

	private void Start()
	{
		_E000();
		_E004();
		Terrain activeTerrain = Terrain.activeTerrain;
		if (activeTerrain == null)
		{
			Debug.Log(_ED3E._E000(95634));
		}
		_E00C = null;
		if (!(activeTerrain != null) || _E38D.DisabledForNow)
		{
			return;
		}
		if (TerrainDetails == null)
		{
			Debug.Log(_ED3E._E000(95618));
		}
		if (TerrainDetails.Length == 0)
		{
			Debug.Log(_ED3E._E000(95657));
		}
		Texture2D detailTex = TerrainDetails[0];
		if (activeTerrain.terrainData == null)
		{
			Debug.Log(_ED3E._E000(95695));
		}
		if (activeTerrain.terrainData.detailPrototypes == null)
		{
			Debug.Log(_ED3E._E000(95731));
		}
		DetailPrototype[] detailPrototypes = activeTerrain.terrainData.detailPrototypes;
		if (detailPrototypes == null)
		{
			Debug.Log(_ED3E._E000(95744));
		}
		if (detailPrototypes.Length == 0)
		{
			Debug.Log(_ED3E._E000(95795));
		}
		if (detailPrototypes == null || detailPrototypes.Length == 0)
		{
			return;
		}
		string text = null;
		DetailPrototype[] array = detailPrototypes;
		foreach (DetailPrototype detailPrototype in array)
		{
			if (detailPrototype.prototypeTexture != null)
			{
				text = detailPrototype.prototypeTexture.name;
				break;
			}
		}
		if (text == null)
		{
			Debug.Log(_ED3E._E000(95829));
		}
		if (text == null)
		{
			return;
		}
		Texture2D[] terrainDetails = TerrainDetails;
		foreach (Texture2D texture2D in terrainDetails)
		{
			if (texture2D.name == text)
			{
				detailTex = texture2D;
			}
		}
		_E00C = new _E001(activeTerrain, detailTex);
	}

	private void Update()
	{
		float num = 0f - StartTime;
		num = 60f;
		float num2 = SnowFallingCurve.Evaluate(num);
		if (this.m__E000 != null)
		{
			this.m__E000.color = Color.Lerp(this.m__E001, this.m__E002, DesaturateSunCurve.Evaluate(num));
			float num3 = 1f - num2 * FadeShadow;
			this.m__E000.intensity = this.m__E003 * num3 * SunIntensityCurve.Evaluate(num);
			this.m__E000.shadowStrength = this.m__E004 * num3;
		}
		float num4 = SoundsLerpCurve.Evaluate(num);
		if (Mathf.Abs(this.m__E005 - num4) > 0.04f)
		{
			this.m__E005 = num4;
			if (_E008 == null)
			{
				Debug.Log(_ED3E._E000(95808));
			}
			foreach (_E000 item in _E008)
			{
				if (item == null)
				{
					Debug.Log(_ED3E._E000(95857));
				}
			}
			foreach (_E000 item2 in _E008)
			{
				item2.Add(num4);
			}
		}
		if (this.m__E006 != null)
		{
			this.m__E006.volume = MusicFadeOut.Evaluate(num);
			if (MusicFadeOut[MusicFadeOut.length - 1].time < num)
			{
				Object.Destroy(this.m__E006);
				this.m__E006 = null;
			}
		}
		if (_E007 != null)
		{
			_E007.volume = MusicFadeIn.Evaluate(num);
			if (MusicFadeIn[MusicFadeIn.length - 1].time < num)
			{
				_E007 = null;
			}
		}
		if (_E00A != null)
		{
			ParticleSystem.MainModule main = _E00A.main;
			main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 1f, 1f, BreathCurve.Evaluate(num)));
			if (Camera.main.transform.parent != _E00B)
			{
				_E00B = Camera.main.transform.parent;
				if (_E00B.name == _ED3E._E000(95840))
				{
					_E009.parent = Camera.main.transform;
					_E009.localPosition = new Vector3(0f, -0.015f, 0.0301f);
					_E009.localRotation = Quaternion.identity;
					_E00A.gameObject.SetActive(value: true);
					ParticleSystem.EmissionModule emission = _E00A.emission;
					emission.enabled = true;
					_E00A.Play(withChildren: false);
				}
				else
				{
					_E00A.gameObject.SetActive(value: false);
					ParticleSystem.EmissionModule emission2 = _E00A.emission;
					emission2.enabled = false;
					_E00A.Pause(withChildren: false);
				}
			}
		}
		if (_E00C != null)
		{
			_E00C.Update(TerrainDetailCurve.Evaluate(num));
		}
	}

	private void _E000()
	{
		this.m__E000 = GameObject.Find(_ED3E._E000(95893)).GetComponent<Light>();
		this.m__E001 = this.m__E000.color;
		this.m__E003 = this.m__E000.intensity;
		this.m__E004 = this.m__E000.shadowStrength;
		LevelSettings levelSettings = (LevelSettings)_E3AA.FindUnityObjectOfType(typeof(LevelSettings));
		_ = levelSettings != null;
		this.m__E002 = _E003(this.m__E001);
		_E002();
		AudioSource[] array = new AudioSource[0];
		if (levelSettings != null)
		{
			array = levelSettings.gameObject.GetComponentsInChildren<AudioSource>();
		}
		AudioSource[] array2 = array;
		foreach (AudioSource audioSource in array2)
		{
			if (audioSource.playOnAwake && audioSource.loop)
			{
				this.m__E006 = audioSource;
				_E007 = audioSource.gameObject.AddComponent<AudioSource>();
				_E007.clip = SnowyWind;
				_E007.loop = true;
				_E007.Play();
				break;
			}
		}
		_E009 = Object.Instantiate(BreathSystem);
		_E009.gameObject.name = _ED3E._E000(95889);
		_E00A = _E009.GetComponent<ParticleSystem>();
		_E001();
	}

	private void _E001()
	{
		TerrainMaterial.SetColor(_E00D, Color.black);
	}

	private void _E002()
	{
	}

	private static Color _E003(Color color)
	{
		float num = (color.r + color.g + color.b) / 3f;
		return new Color(num, num, num, color.a);
	}

	private void _E004()
	{
		Vector3 position = _E3AA.FindUnityObjectOfType<UpperLeftAnchor>().transform.position;
		Vector3 position2 = _E3AA.FindUnityObjectOfType<LowerRightAnchor>().transform.position;
		Vector2 vector = new Vector2(Mathf.Max(position.x, position2.x), Mathf.Max(position.z, position2.z));
		Vector2 vector2 = new Vector2(Mathf.Min(position.x, position2.x), Mathf.Min(position.z, position2.z));
		Vector2 vector3 = (vector + vector2) * 0.5f;
		Vector2 vector4 = vector - vector2;
		Vector2 vector5 = new Vector2(Mathf.Min(position.y, position2.y), Mathf.Max(position.y, position2.y));
		float orthographicSize = Mathf.Max(vector4.x, vector4.y);
		float farClipPlane = vector5.y - vector5.x;
		GameObject gameObject = new GameObject(_ED3E._E000(95880), typeof(Camera));
		gameObject.transform.position = new Vector3(vector3.x, vector5.y, vector3.y);
		gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
		Camera component = gameObject.GetComponent<Camera>();
		component.orthographic = true;
		component.orthographicSize = orthographicSize;
		component.aspect = 1f;
		component.farClipPlane = farClipPlane;
		component.depthTextureMode = DepthTextureMode.Depth;
		LinkedList<Renderer> linkedList = _E005();
		foreach (Renderer item in linkedList)
		{
			item.enabled = false;
		}
		RenderTexture dest = new RenderTexture(2048, 2048, 1, RenderTextureFormat.ARGB32)
		{
			name = _ED3E._E000(95872)
		};
		component.cullingMask = DepthRendererMask.value;
		component.targetTexture = RenderTexture.GetTemporary(2048, 2048, 1, RenderTextureFormat.ARGB32);
		component.Render();
		Graphics.Blit(component.targetTexture, dest, DepthMaterial);
		RenderTexture.ReleaseTemporary(component.targetTexture);
		foreach (Renderer item2 in linkedList)
		{
			item2.enabled = true;
		}
		Object.Destroy(gameObject);
	}

	private static LinkedList<Renderer> _E005()
	{
		LevelSettings levelSettings = (LevelSettings)_E3AA.FindUnityObjectOfType(typeof(LevelSettings));
		if (levelSettings == null)
		{
			return new LinkedList<Renderer>();
		}
		GameObject gameObject = levelSettings.gameObject;
		HashSet<string> hashSet = new HashSet<string>(new string[7]
		{
			_ED3E._E000(95920),
			_ED3E._E000(95943),
			_ED3E._E000(96025),
			_ED3E._E000(96036),
			_ED3E._E000(96120),
			_ED3E._E000(96142),
			_ED3E._E000(96213)
		});
		LinkedList<Renderer> linkedList = new LinkedList<Renderer>();
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (hashSet.Contains(material.shader.name))
				{
					linkedList.AddLast(renderer);
					break;
				}
			}
		}
		return linkedList;
	}

	private static Vector2 _E006()
	{
		return new Vector2(-50f, 60f);
	}
}
