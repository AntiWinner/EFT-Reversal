using System;
using System.Collections.Generic;
using System.Linq;
using AmplifyMotion;
using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class AmplifyMotionEffectBase : MonoBehaviour
{
	[Header("Motion Blur")]
	public Quality QualityLevel = Quality.Standard;

	public int QualitySteps = 1;

	public float MotionScale = 3f;

	public float CameraMotionMult = 1f;

	public float MinVelocity = 1f;

	public float MaxVelocity = 10f;

	public float DepthThreshold = 0.01f;

	public bool Noise;

	[Header("Camera")]
	public Camera[] OverlayCameras = new Camera[0];

	public LayerMask CullingMask = -1;

	[Header("Objects")]
	public bool AutoRegisterObjs = true;

	public float MinResetDeltaDist = 1000f;

	[NonSerialized]
	public float MinResetDeltaDistSqr;

	public int ResetFrameDelay = 1;

	[FormerlySerializedAs("workerThreads")]
	[Header("Low-Level")]
	public int WorkerThreads;

	public bool SystemThreadPool;

	public bool ForceCPUOnly;

	public bool DebugMode;

	private Camera m__E000;

	private bool m__E001 = true;

	private int m__E002;

	private int m__E003;

	private RenderTexture m__E004;

	private Material m__E005;

	private Material m__E006;

	private Material m__E007;

	private Material m__E008;

	private Material m__E009;

	private Material m__E00A;

	private Material m__E00B;

	private Material m__E00C;

	private Material m__E00D;

	private Dictionary<Camera, AmplifyMotionCamera> m__E00E = new Dictionary<Camera, AmplifyMotionCamera>();

	internal Camera[] _E00F;

	internal AmplifyMotionCamera[] _E010;

	internal bool _E011 = true;

	private AmplifyMotionPostProcess m__E012;

	private int m__E013 = 1;

	private float m__E014;

	private float m__E015;

	private float m__E016;

	private float m__E017;

	private Quality m__E018;

	private AmplifyMotionCamera m__E019;

	private _ED33 m__E01A;

	public static Dictionary<GameObject, AmplifyMotionObjectBase> m_activeObjects = new Dictionary<GameObject, AmplifyMotionObjectBase>();

	public static Dictionary<Camera, AmplifyMotionCamera> m_activeCameras = new Dictionary<Camera, AmplifyMotionCamera>();

	private static bool m__E01B = false;

	private bool m__E01C;

	private const CameraEvent _E01D = CameraEvent.BeforeImageEffectsOpaque;

	private CommandBuffer _E01E;

	private const CameraEvent _E01F = CameraEvent.BeforeImageEffectsOpaque;

	private CommandBuffer _E020;

	private static bool _E021 = false;

	private static AmplifyMotionEffectBase _E022 = null;

	[Obsolete("workerThreads is deprecated, please use WorkerThreads instead.")]
	public int workerThreads
	{
		get
		{
			return WorkerThreads;
		}
		set
		{
			WorkerThreads = value;
		}
	}

	internal Material _E023 => this.m__E009;

	internal Material _E024 => this.m__E006;

	internal Material _E025 => this.m__E007;

	internal Material _E026 => this.m__E008;

	internal RenderTexture _E027 => this.m__E004;

	public Dictionary<Camera, AmplifyMotionCamera> LinkedCameras => this.m__E00E;

	internal float _E028 => this.m__E016;

	internal float _E029 => this.m__E017;

	public AmplifyMotionCamera BaseCamera => this.m__E019;

	internal _ED33 _E02A => this.m__E01A;

	public static bool IsD3D => AmplifyMotionEffectBase.m__E01B;

	public bool CanUseGPU => this.m__E01C;

	public static bool IgnoreMotionScaleWarning => _E021;

	public static AmplifyMotionEffectBase FirstInstance => _E022;

	public static AmplifyMotionEffectBase Instance => _E022;

	private void Awake()
	{
		if (_E022 == null)
		{
			_E022 = this;
		}
		AmplifyMotionEffectBase.m__E01B = SystemInfo.graphicsDeviceVersion.StartsWith(_ED3E._E000(23402));
		this.m__E013 = 1;
		this.m__E002 = (this.m__E003 = 0);
		if (ForceCPUOnly)
		{
			this.m__E01C = false;
			return;
		}
		bool flag = SystemInfo.graphicsShaderLevel >= 30;
		bool flag2 = SystemInfo.SupportsTextureFormat(TextureFormat.RHalf);
		bool flag3 = SystemInfo.SupportsTextureFormat(TextureFormat.RGHalf);
		bool flag4 = SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf);
		bool flag5 = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat);
		this.m__E01C = flag && flag2 && flag3 && flag4 && flag5;
	}

	internal void _E000()
	{
		this.m__E013 = 1;
	}

	internal int _E001(GameObject obj)
	{
		if (obj.isStatic)
		{
			return 0;
		}
		this.m__E013++;
		if (this.m__E013 > 254)
		{
			this.m__E013 = 1;
		}
		return this.m__E013;
	}

	private void _E002(ref Material mat)
	{
		if (mat != null)
		{
			UnityEngine.Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private bool _E003(Material material, string name)
	{
		bool result = true;
		if (material == null || material.shader == null)
		{
			Debug.LogWarning(_ED3E._E000(23395) + name + _ED3E._E000(23427));
			result = false;
		}
		else if (!material.shader.isSupported)
		{
			Debug.LogWarning(_ED3E._E000(23477) + name + _ED3E._E000(23462));
			result = false;
		}
		return result;
	}

	private void _E004()
	{
		_E002(ref this.m__E005);
		_E002(ref this.m__E006);
		_E002(ref this.m__E007);
		_E002(ref this.m__E008);
		_E002(ref this.m__E009);
		_E002(ref this.m__E00A);
		_E002(ref this.m__E00B);
		_E002(ref this.m__E00C);
		_E002(ref this.m__E00D);
	}

	private bool _E005()
	{
		_E004();
		int num = ((SystemInfo.graphicsShaderLevel >= 30) ? 3 : 2);
		string text = _ED3E._E000(23549) + num;
		string text2 = _ED3E._E000(17432);
		string text3 = _ED3E._E000(17467);
		string text4 = _ED3E._E000(17488);
		string text5 = _ED3E._E000(17523);
		string text6 = _ED3E._E000(17541);
		string text7 = _ED3E._E000(17579);
		string text8 = _ED3E._E000(17610);
		string text9 = _ED3E._E000(17646);
		try
		{
			this.m__E005 = new Material(_E3AC.Find(text))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E006 = new Material(_E3AC.Find(text2))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E007 = new Material(_E3AC.Find(text3))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E008 = new Material(_E3AC.Find(text4))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E009 = new Material(_E3AC.Find(text5))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E00A = new Material(_E3AC.Find(text6))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E00B = new Material(_E3AC.Find(text7))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E00C = new Material(_E3AC.Find(text8))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m__E00D = new Material(_E3AC.Find(text9))
			{
				hideFlags = HideFlags.DontSave
			};
		}
		catch (Exception)
		{
		}
		if (_E003(this.m__E005, text) && _E003(this.m__E006, text2) && _E003(this.m__E007, text3) && _E003(this.m__E008, text4) && _E003(this.m__E009, text5) && _E003(this.m__E00A, text6) && _E003(this.m__E00B, text7) && _E003(this.m__E00C, text8))
		{
			return _E003(this.m__E00D, text9);
		}
		return false;
	}

	private RenderTexture _E006(string name, int depth, RenderTextureFormat fmt, RenderTextureReadWrite rw, FilterMode fm)
	{
		RenderTexture renderTexture = new RenderTexture(this.m__E002, this.m__E003, depth, fmt, rw);
		renderTexture.hideFlags = HideFlags.DontSave;
		renderTexture.name = name;
		renderTexture.wrapMode = TextureWrapMode.Clamp;
		renderTexture.filterMode = fm;
		renderTexture.Create();
		return renderTexture;
	}

	private void _E007(ref RenderTexture rt)
	{
		if (rt != null)
		{
			RenderTexture.active = null;
			rt.Release();
			UnityEngine.Object.DestroyImmediate(rt);
			rt = null;
		}
	}

	private void _E008(ref Texture tex)
	{
		if (tex != null)
		{
			UnityEngine.Object.DestroyImmediate(tex);
			tex = null;
		}
	}

	private void _E009()
	{
		RenderTexture.active = null;
		_E007(ref this.m__E004);
	}

	private void _E00A(bool qualityChanged)
	{
		int num = Mathf.Max(Mathf.FloorToInt((float)this.m__E000.pixelWidth + 0.5f), 1);
		int num2 = Mathf.Max(Mathf.FloorToInt((float)this.m__E000.pixelHeight + 0.5f), 1);
		if (QualityLevel == Quality.Mobile)
		{
			num /= 2;
			num2 /= 2;
		}
		if (this.m__E002 != num || this.m__E003 != num2)
		{
			this.m__E002 = num;
			this.m__E003 = num2;
			_E009();
		}
		if (this.m__E004 == null)
		{
			this.m__E004 = _E006(_ED3E._E000(17682), 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear, FilterMode.Point);
		}
	}

	public bool CheckSupport()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogError(_ED3E._E000(17667));
			return false;
		}
		return true;
	}

	private void _E00B()
	{
		if (WorkerThreads <= 0)
		{
			WorkerThreads = Mathf.Max(Environment.ProcessorCount / 2, 1);
		}
		this.m__E01A = new _ED33();
		this.m__E01A._E000(WorkerThreads, SystemThreadPool);
	}

	private void _E00C()
	{
		if (this.m__E01A != null)
		{
			this.m__E01A._E001();
			this.m__E01A = null;
		}
	}

	private void _E00D()
	{
		_E00E();
		_E01E = new CommandBuffer();
		_E01E.name = _ED3E._E000(17814);
		this.m__E000.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _E01E);
		_E020 = new CommandBuffer();
		_E020.name = _ED3E._E000(17795);
		this.m__E000.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _E020);
	}

	private void _E00E()
	{
		if (_E01E != null)
		{
			this.m__E000.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _E01E);
			_E01E.Release();
			_E01E = null;
		}
		if (_E020 != null)
		{
			this.m__E000.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _E020);
			_E020.Release();
			_E020 = null;
		}
	}

	private void OnEnable()
	{
		this.m__E000 = GetComponent<Camera>();
		if (!CheckSupport())
		{
			base.enabled = false;
			return;
		}
		_E00B();
		this.m__E001 = true;
		if (!_E005())
		{
			Debug.LogError(_ED3E._E000(17829));
			base.enabled = false;
			return;
		}
		if (AutoRegisterObjs)
		{
			UpdateActiveObjects();
		}
		_E011();
		_E00D();
		_E00A(qualityChanged: true);
		this.m__E00E.TryGetValue(this.m__E000, out this.m__E019);
		if (this.m__E019 == null)
		{
			Debug.LogError(_ED3E._E000(17954));
			base.enabled = false;
			return;
		}
		if (this.m__E012 != null)
		{
			this.m__E012.enabled = true;
		}
		this.m__E018 = QualityLevel;
	}

	private void OnDisable()
	{
		if (this.m__E012 != null)
		{
			this.m__E012.enabled = false;
		}
		_E00E();
		_E00C();
	}

	private void Start()
	{
		_E01A();
	}

	internal void _E00F(Camera reference)
	{
		this.m__E00E.Remove(reference);
	}

	private void OnDestroy()
	{
		AmplifyMotionCamera[] array = this.m__E00E.Values.ToArray();
		foreach (AmplifyMotionCamera amplifyMotionCamera in array)
		{
			if (amplifyMotionCamera != null && amplifyMotionCamera.gameObject != base.gameObject)
			{
				Camera component = amplifyMotionCamera.GetComponent<Camera>();
				if (component != null)
				{
					component.targetTexture = null;
				}
				UnityEngine.Object.DestroyImmediate(amplifyMotionCamera);
			}
		}
		_E009();
		_E004();
	}

	private GameObject _E010(GameObject obj, string auxCameraName)
	{
		GameObject gameObject = null;
		if (obj.name == auxCameraName)
		{
			return obj;
		}
		foreach (Transform item in obj.transform)
		{
			gameObject = _E010(item.gameObject, auxCameraName);
			if (gameObject != null)
			{
				return gameObject;
			}
		}
		return gameObject;
	}

	private void _E011()
	{
		List<Camera> list = new List<Camera>(OverlayCameras.Length);
		for (int i = 0; i < OverlayCameras.Length; i++)
		{
			if (OverlayCameras[i] != null)
			{
				list.Add(OverlayCameras[i]);
			}
		}
		Camera[] array = new Camera[list.Count + 1];
		array[0] = this.m__E000;
		for (int j = 0; j < list.Count; j++)
		{
			array[j + 1] = list[j];
		}
		this.m__E00E.Clear();
		for (int k = 0; k < array.Length; k++)
		{
			Camera camera = array[k];
			if (!this.m__E00E.ContainsKey(camera))
			{
				AmplifyMotionCamera amplifyMotionCamera = camera.gameObject.GetComponent<AmplifyMotionCamera>();
				if (amplifyMotionCamera != null)
				{
					amplifyMotionCamera.enabled = false;
					amplifyMotionCamera.enabled = true;
				}
				else
				{
					amplifyMotionCamera = camera.gameObject.AddComponent<AmplifyMotionCamera>();
				}
				amplifyMotionCamera.LinkTo(this, k > 0);
				this.m__E00E.Add(camera, amplifyMotionCamera);
				this._E011 = true;
			}
		}
	}

	public void UpdateActiveCameras()
	{
		_E011();
	}

	internal static void _E012(AmplifyMotionCamera cam)
	{
		if (!m_activeCameras.ContainsValue(cam))
		{
			m_activeCameras.Add(cam.GetComponent<Camera>(), cam);
		}
		foreach (AmplifyMotionObjectBase value in m_activeObjects.Values)
		{
			value._E000(cam);
		}
	}

	internal static void _E013(AmplifyMotionCamera cam)
	{
		foreach (AmplifyMotionObjectBase value in m_activeObjects.Values)
		{
			value._E001(cam);
		}
		m_activeCameras.Remove(cam.GetComponent<Camera>());
	}

	public void UpdateActiveObjects()
	{
		GameObject[] array = _E3AA.FindUnityObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i < array.Length; i++)
		{
			if (!m_activeObjects.ContainsKey(array[i]))
			{
				_E018(array[i], autoReg: true);
			}
		}
	}

	internal static void _E014(AmplifyMotionObjectBase obj)
	{
		m_activeObjects.Add(obj.gameObject, obj);
		foreach (AmplifyMotionCamera value in m_activeCameras.Values)
		{
			obj._E000(value);
		}
	}

	internal static void _E015(AmplifyMotionObjectBase obj)
	{
		foreach (AmplifyMotionCamera value in m_activeCameras.Values)
		{
			obj._E001(value);
		}
		m_activeObjects.Remove(obj.gameObject);
	}

	internal static bool _E016(Material[] materials)
	{
		foreach (Material material in materials)
		{
			if (!(material != null))
			{
				continue;
			}
			string text = material.GetTag(_ED3E._E000(18067), searchFallbacks: false);
			if (text == _ED3E._E000(18054) || text == _ED3E._E000(18109))
			{
				if (!material.IsKeywordEnabled(_ED3E._E000(18095)))
				{
					return !material.IsKeywordEnabled(_ED3E._E000(18142));
				}
				return false;
			}
		}
		return false;
	}

	internal static bool _E017(GameObject gameObj, bool autoReg)
	{
		if (gameObj.isStatic)
		{
			return false;
		}
		Renderer component = gameObj.GetComponent<Renderer>();
		if (component == null || component.sharedMaterials == null || component.isPartOfStaticBatch)
		{
			return false;
		}
		if (!component.enabled)
		{
			return false;
		}
		if (component.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
		{
			return false;
		}
		if (component.GetType() == typeof(SpriteRenderer))
		{
			return false;
		}
		if (!_E016(component.sharedMaterials))
		{
			return false;
		}
		Type type = component.GetType();
		if (type == typeof(MeshRenderer) || type == typeof(SkinnedMeshRenderer))
		{
			return true;
		}
		if (type == typeof(ParticleSystemRenderer) && !autoReg)
		{
			ParticleSystemRenderMode renderMode = (component as ParticleSystemRenderer).renderMode;
			if (renderMode != ParticleSystemRenderMode.Mesh)
			{
				return renderMode == ParticleSystemRenderMode.Billboard;
			}
			return true;
		}
		return false;
	}

	internal static void _E018(GameObject gameObj, bool autoReg)
	{
		if (_E017(gameObj, autoReg) && gameObj.GetComponent<AmplifyMotionObjectBase>() == null)
		{
			AmplifyMotionObjectBase._E000 = false;
			gameObj.AddComponent<AmplifyMotionObjectBase>();
			AmplifyMotionObjectBase._E000 = true;
		}
	}

	internal static void _E019(GameObject gameObj)
	{
		AmplifyMotionObjectBase component = gameObj.GetComponent<AmplifyMotionObjectBase>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	public void Register(GameObject gameObj)
	{
		if (!m_activeObjects.ContainsKey(gameObj))
		{
			_E018(gameObj, autoReg: false);
		}
	}

	public static void RegisterS(GameObject gameObj)
	{
		if (!m_activeObjects.ContainsKey(gameObj))
		{
			_E018(gameObj, autoReg: false);
		}
	}

	public void RegisterRecursively(GameObject gameObj)
	{
		if (!m_activeObjects.ContainsKey(gameObj))
		{
			_E018(gameObj, autoReg: false);
		}
		foreach (Transform item in gameObj.transform)
		{
			RegisterRecursively(item.gameObject);
		}
	}

	public static void RegisterRecursivelyS(GameObject gameObj)
	{
		if (!m_activeObjects.ContainsKey(gameObj))
		{
			_E018(gameObj, autoReg: false);
		}
		foreach (Transform item in gameObj.transform)
		{
			RegisterRecursivelyS(item.gameObject);
		}
	}

	public void Unregister(GameObject gameObj)
	{
		if (m_activeObjects.ContainsKey(gameObj))
		{
			_E019(gameObj);
		}
	}

	public static void UnregisterS(GameObject gameObj)
	{
		if (m_activeObjects.ContainsKey(gameObj))
		{
			_E019(gameObj);
		}
	}

	public void UnregisterRecursively(GameObject gameObj)
	{
		if (m_activeObjects.ContainsKey(gameObj))
		{
			_E019(gameObj);
		}
		foreach (Transform item in gameObj.transform)
		{
			UnregisterRecursively(item.gameObject);
		}
	}

	public static void UnregisterRecursivelyS(GameObject gameObj)
	{
		if (m_activeObjects.ContainsKey(gameObj))
		{
			_E019(gameObj);
		}
		foreach (Transform item in gameObj.transform)
		{
			UnregisterRecursivelyS(item.gameObject);
		}
	}

	private void _E01A()
	{
		Camera camera = null;
		float num = float.MinValue;
		if (this._E011)
		{
			_E01B();
		}
		for (int i = 0; i < this._E00F.Length; i++)
		{
			if (this._E00F[i] != null && this._E00F[i].isActiveAndEnabled && this._E00F[i].depth > num)
			{
				camera = this._E00F[i];
				num = this._E00F[i].depth;
			}
		}
		if (this.m__E012 != null && this.m__E012.gameObject != camera.gameObject)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E012);
			this.m__E012 = null;
		}
		if (!(this.m__E012 == null) || !(camera != null) || !(camera != this.m__E000))
		{
			return;
		}
		AmplifyMotionPostProcess[] components = base.gameObject.GetComponents<AmplifyMotionPostProcess>();
		if (components != null && components.Length != 0)
		{
			for (int j = 0; j < components.Length; j++)
			{
				UnityEngine.Object.DestroyImmediate(components[j]);
			}
		}
		this.m__E012 = camera.gameObject.AddComponent<AmplifyMotionPostProcess>();
		this.m__E012.Instance = this;
	}

	private void LateUpdate()
	{
		if (this.m__E019.AutoStep)
		{
			float num = (Application.isPlaying ? Time.unscaledDeltaTime : Time.fixedDeltaTime);
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.m__E014 = ((num >= float.Epsilon) ? num : this.m__E014);
			this.m__E015 = ((num >= float.Epsilon) ? fixedDeltaTime : this.m__E015);
		}
		QualitySteps = Mathf.Clamp(QualitySteps, 0, 16);
		MotionScale = Mathf.Max(MotionScale, 0f);
		MinVelocity = Mathf.Min(MinVelocity, MaxVelocity);
		DepthThreshold = Mathf.Max(DepthThreshold, 0f);
		MinResetDeltaDist = Mathf.Max(MinResetDeltaDist, 0f);
		MinResetDeltaDistSqr = MinResetDeltaDist * MinResetDeltaDist;
		ResetFrameDelay = Mathf.Max(ResetFrameDelay, 0);
		_E01A();
	}

	public void StopAutoStep()
	{
		foreach (AmplifyMotionCamera value in this.m__E00E.Values)
		{
			value.StopAutoStep();
		}
	}

	public void StartAutoStep()
	{
		foreach (AmplifyMotionCamera value in this.m__E00E.Values)
		{
			value.StartAutoStep();
		}
	}

	public void Step(float delta)
	{
		this.m__E014 = delta;
		this.m__E015 = delta;
		foreach (AmplifyMotionCamera value in this.m__E00E.Values)
		{
			value.Step();
		}
	}

	private void _E01B()
	{
		Dictionary<Camera, AmplifyMotionCamera>.KeyCollection keys = this.m__E00E.Keys;
		Dictionary<Camera, AmplifyMotionCamera>.ValueCollection values = this.m__E00E.Values;
		if (this._E00F == null || keys.Count != this._E00F.Length)
		{
			this._E00F = new Camera[keys.Count];
		}
		if (this._E010 == null || values.Count != this._E010.Length)
		{
			this._E010 = new AmplifyMotionCamera[values.Count];
		}
		keys.CopyTo(this._E00F, 0);
		values.CopyTo(this._E010, 0);
		this._E011 = false;
	}

	private void FixedUpdate()
	{
		if (!this.m__E000.enabled)
		{
			return;
		}
		if (this._E011)
		{
			_E01B();
		}
		_E020.Clear();
		for (int i = 0; i < this._E010.Length; i++)
		{
			if (this._E010[i] != null && this._E010[i].isActiveAndEnabled)
			{
				this._E010[i].FixedUpdateTransform(this, _E020);
			}
		}
	}

	private void OnPreRender()
	{
		if (!this.m__E000.enabled || (Time.frameCount != 1 && !(Mathf.Abs(Time.unscaledDeltaTime) >= float.Epsilon)))
		{
			return;
		}
		if (this._E011)
		{
			_E01B();
		}
		_E01E.Clear();
		for (int i = 0; i < this._E010.Length; i++)
		{
			if (this._E010[i] != null && this._E010[i].isActiveAndEnabled)
			{
				this._E010[i].UpdateTransform(this, _E01E);
			}
		}
	}

	private void OnPostRender()
	{
		bool qualityChanged = QualityLevel != this.m__E018;
		this.m__E018 = QualityLevel;
		_E00A(qualityChanged);
		_E000();
		bool flag = CameraMotionMult >= float.Epsilon;
		bool clearColor = !flag || this.m__E001;
		float num = ((DepthThreshold >= float.Epsilon) ? (1f / DepthThreshold) : float.MaxValue);
		this.m__E016 = ((this.m__E014 >= float.Epsilon) ? (MotionScale * (1f / this.m__E014)) : 0f);
		this.m__E017 = ((this.m__E015 >= float.Epsilon) ? (MotionScale * (1f / this.m__E015)) : 0f);
		float scale = ((!this.m__E001) ? this.m__E016 : 0f);
		float fixedScale = ((!this.m__E001) ? this.m__E017 : 0f);
		Shader.SetGlobalFloat(_ED3E._E000(18123), MinVelocity);
		Shader.SetGlobalFloat(_ED3E._E000(18164), MaxVelocity);
		Shader.SetGlobalFloat(_ED3E._E000(18149), 1f / (MaxVelocity - MinVelocity));
		Shader.SetGlobalVector(_ED3E._E000(18188), new Vector2(DepthThreshold, num));
		this.m__E004.DiscardContents();
		this.m__E019.PreRenderVectors(this.m__E004, clearColor, num);
		for (int i = 0; i < this._E010.Length; i++)
		{
			AmplifyMotionCamera amplifyMotionCamera = this._E010[i];
			if (amplifyMotionCamera != null && amplifyMotionCamera.Overlay && amplifyMotionCamera.isActiveAndEnabled)
			{
				amplifyMotionCamera.PreRenderVectors(this.m__E004, clearColor, num);
				amplifyMotionCamera.RenderVectors(scale, fixedScale, QualityLevel);
			}
		}
		if (flag)
		{
			float num2 = ((this.m__E014 >= float.Epsilon) ? (MotionScale * CameraMotionMult * (1f / this.m__E014)) : 0f);
			float scale2 = ((!this.m__E001) ? num2 : 0f);
			this.m__E004.DiscardContents();
			this.m__E019.RenderReprojectionVectors(this.m__E004, scale2);
		}
		this.m__E019.RenderVectors(scale, fixedScale, QualityLevel);
		for (int j = 0; j < this._E010.Length; j++)
		{
			AmplifyMotionCamera amplifyMotionCamera2 = this._E010[j];
			if (amplifyMotionCamera2 != null && amplifyMotionCamera2.Overlay && amplifyMotionCamera2.isActiveAndEnabled)
			{
				amplifyMotionCamera2.RenderVectors(scale, fixedScale, QualityLevel);
			}
		}
		this.m__E001 = false;
	}

	private void _E01C(RenderTexture source, RenderTexture destination, Vector4 blurStep)
	{
		bool flag = QualityLevel == Quality.Mobile;
		int pass = (int)(QualityLevel + (Noise ? 4 : 0));
		RenderTexture renderTexture = null;
		if (flag)
		{
			renderTexture = RenderTexture.GetTemporary(this.m__E002, this.m__E003, 0, RenderTextureFormat.ARGB32);
			renderTexture.name = _ED3E._E000(18232);
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.filterMode = FilterMode.Point;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(this.m__E002, this.m__E003, 0, source.format);
		temporary.name = _ED3E._E000(18221);
		temporary.wrapMode = TextureWrapMode.Clamp;
		temporary.filterMode = FilterMode.Point;
		temporary.DiscardContents();
		this.m__E00A.SetTexture(_ED3E._E000(18269), this.m__E004);
		source.filterMode = FilterMode.Point;
		DebugGraphics.Blit(source, temporary, this.m__E00A, 0);
		this.m__E005.SetTexture(_ED3E._E000(18269), this.m__E004);
		if (flag)
		{
			DebugGraphics.Blit(null, renderTexture, this.m__E00C, 0);
			this.m__E005.SetTexture(_ED3E._E000(18256), renderTexture);
		}
		if (QualitySteps > 1)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(this.m__E002, this.m__E003, 0, source.format);
			temporary2.name = _ED3E._E000(18250);
			temporary2.filterMode = FilterMode.Point;
			float num = 1f / (float)QualitySteps;
			float num2 = 1f;
			RenderTexture renderTexture2 = temporary;
			RenderTexture renderTexture3 = temporary2;
			for (int i = 0; i < QualitySteps; i++)
			{
				if (renderTexture3 != destination)
				{
					renderTexture3.DiscardContents();
				}
				this.m__E005.SetVector(_ED3E._E000(18299), blurStep * num2);
				DebugGraphics.Blit(renderTexture2, renderTexture3, this.m__E005, pass);
				if (i < QualitySteps - 2)
				{
					RenderTexture renderTexture4 = renderTexture3;
					renderTexture3 = renderTexture2;
					renderTexture2 = renderTexture4;
				}
				else
				{
					renderTexture2 = renderTexture3;
					renderTexture3 = destination;
				}
				num2 -= num;
			}
			RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			this.m__E005.SetVector(_ED3E._E000(18299), blurStep);
			DebugGraphics.Blit(temporary, destination, this.m__E005, pass);
		}
		if (flag)
		{
			this.m__E00A.SetTexture(_ED3E._E000(18269), this.m__E004);
			DebugGraphics.Blit(source, destination, this.m__E00A, 1);
		}
		RenderTexture.ReleaseTemporary(temporary);
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m__E012 == null)
		{
			PostProcess(source, destination);
		}
		else
		{
			_E3A1.BlitOrCopy(source, destination);
		}
	}

	public void PostProcess(RenderTexture source, RenderTexture destination)
	{
		Vector4 zero = Vector4.zero;
		zero.x = MaxVelocity / 1000f;
		zero.y = MaxVelocity / 1000f;
		RenderTexture renderTexture = null;
		if (QualitySettings.antiAliasing > 1)
		{
			renderTexture = RenderTexture.GetTemporary(this.m__E002, this.m__E003, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			renderTexture.name = _ED3E._E000(18281);
			renderTexture.filterMode = FilterMode.Point;
			this.m__E00B.SetTexture(_ED3E._E000(18269), this.m__E004);
			DebugGraphics.Blit(this.m__E004, renderTexture, this.m__E00B, 0);
			this.m__E00B.SetTexture(_ED3E._E000(18269), renderTexture);
			DebugGraphics.Blit(renderTexture, this.m__E004, this.m__E00B, 1);
		}
		if (DebugMode)
		{
			this.m__E00D.SetTexture(_ED3E._E000(18269), this.m__E004);
			Graphics.Blit(source, destination, this.m__E00D);
		}
		else
		{
			_E01C(source, destination, zero);
		}
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}
}
