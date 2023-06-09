using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class TerrainGenerator : MonoBehaviour
{
	public Texture2D groundTexture;

	public Texture2D detailTexture;

	public GameObject FpsController;

	public GameObject FixedCamera;

	private int m__E000 = 32;

	private int m__E001;

	private Vector3 m__E002;

	private Vector3 m__E003;

	private Terrain[] m__E004;

	private bool m__E005 = true;

	private Color m__E006 = new Color(0.263f, 0.976f, 0.165f, 1f);

	private Color m__E007 = new Color(0.804f, 0.737f, 0.102f, 1f);

	private float m__E008 = 0.2f;

	private float m__E009 = 0.5f;

	private float _E00A = 0.5f;

	private float _E00B = 0.6f;

	private bool _E00C;

	private float _E00D = 0.5f;

	private float _E00E = 0.5f;

	private float _E00F = 0.5f;

	private Color _E010 = new Color(32f / 51f, 0.32156864f, 0.1764706f, 1f);

	private bool _E011 = true;

	private bool _E012;

	private int _E013 = 2;

	private float _E014 = 50f;

	private Vector4 _E015 = new Vector4(0.5f, 3f, 0.5f, 3f);

	private bool _E016;

	private bool _E017 = true;

	private float _E018 = 0.2f;

	private float _E019 = 250f;

	private Image _E01A;

	private Image _E01B;

	private Slider _E01C;

	private Slider _E01D;

	private Slider _E01E;

	private Slider _E01F;

	private Toggle _E020;

	private Slider _E021;

	private Slider _E022;

	private Slider _E023;

	private Image _E024;

	private Toggle _E025;

	private Toggle _E026;

	private Slider _E027;

	private Slider _E028;

	private InputField _E029;

	private InputField _E02A;

	private InputField _E02B;

	private InputField _E02C;

	private Toggle _E02D;

	private Toggle _E02E;

	private Slider _E02F;

	private Slider _E030;

	private Text _E031;

	private Text _E032;

	private Selectable _E033;

	private Selectable _E034;

	private Canvas _E035;

	public static readonly string HELPTEXT_detailHealthyColor = _ED3E._E000(69546);

	public static readonly string HELPTEXT_detailDryColor = _ED3E._E000(67967);

	public static readonly string HELPTEXT_noiseSpread = _ED3E._E000(68373);

	public static readonly string HELPTEXT_ambientOcclusion = _ED3E._E000(79133);

	public static readonly string HELPTEXT_gradientPower = _ED3E._E000(79227);

	public static readonly string HELPTEXT_windIdleSway = _ED3E._E000(79506);

	public static readonly string HELPTEXT_windWavesOn = _ED3E._E000(79746);

	public static readonly string HELPTEXT_windWaveTintColor = _ED3E._E000(78093);

	public static readonly string HELPTEXT_windWaveSize = _ED3E._E000(78392);

	public static readonly string HELPTEXT_windWaveSway = _ED3E._E000(78524);

	public static readonly string HELPTEXT_windWaveTint = _ED3E._E000(78792);

	public static readonly string HELPTEXT_isBillboard = _ED3E._E000(81067);

	public static readonly string HELPTEXT_crossQuads = _ED3E._E000(81217);

	public static readonly string HELPTEXT_quadCount = _ED3E._E000(81618);

	public static readonly string HELPTEXT_billboardDistance = _ED3E._E000(81851);

	public static readonly string HELPTEXT_detailScale = _ED3E._E000(80217);

	public static readonly string HELPTEXT_isShadowCasting = _ED3E._E000(80591);

	public static readonly string HELPTEXT_isFrustumCulling = _ED3E._E000(74806);

	public static readonly string HELPTEXT_frustumOffset = _ED3E._E000(75182);

	public static readonly string HELPTEXT_maxDetailDistance = _ED3E._E000(75761);

	public static readonly string HELPTEXT_windVector = _ED3E._E000(74030);

	private void Start()
	{
		_E002();
		if ((bool)FixedCamera)
		{
			FixedCamera.SetActive(value: true);
		}
		if ((bool)FpsController)
		{
			FpsController.SetActive(value: false);
		}
		this.m__E001 = 0;
		this.m__E002 = new Vector3(this.m__E000, 0f, 0f);
		this.m__E003 = new Vector3(0f, 0f, -this.m__E000);
		this.m__E004 = new Terrain[9];
		AddTerrain();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Q))
		{
			AddTerrain();
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			RemoveTerrain();
		}
		if (Input.GetKeyUp(KeyCode.C))
		{
			_E004();
			this.m__E005 = !this.m__E005;
		}
		if (Input.GetKeyUp(KeyCode.U))
		{
			_E035.gameObject.SetActive(!_E035.gameObject.activeSelf);
		}
	}

	public void AddTerrain()
	{
		if (this.m__E001 != 9)
		{
			_E005();
			_E000(this.m__E004[this.m__E001]);
			this.m__E001++;
			_E003();
		}
	}

	public void RemoveTerrain()
	{
		if (this.m__E001 != 0)
		{
			Object.Destroy(this.m__E004[this.m__E001 - 1].gameObject);
			this.m__E001--;
			_E003();
		}
	}

	private void _E000(Terrain terrain)
	{
		GPUInstancerDetailManager gPUInstancerDetailManager = terrain.gameObject.AddComponent<GPUInstancerDetailManager>();
		_E4BD.SetupManagerWithTerrain(gPUInstancerDetailManager, terrain);
		if (gPUInstancerDetailManager.prototypeList.Count > 0)
		{
			GPUInstancerDetailPrototype obj = (GPUInstancerDetailPrototype)gPUInstancerDetailManager.prototypeList[0];
			obj.detailHealthyColor = this.m__E006;
			obj.detailDryColor = this.m__E007;
			obj.noiseSpread = this.m__E008;
			obj.ambientOcclusion = this.m__E009;
			obj.gradientPower = _E00A;
			obj.windIdleSway = _E00B;
			obj.windWavesOn = _E00C;
			obj.windWaveTint = _E00D;
			obj.windWaveSway = _E00F;
			obj.windWaveTintColor = _E010;
			obj.isBillboard = _E011;
			obj.useCrossQuads = _E012;
			obj.quadCount = _E013;
			obj.billboardDistance = _E014;
			obj.detailScale = _E015;
			obj.isShadowCasting = _E016;
			obj.isFrustumCulling = _E017;
			obj.maxDistance = _E019;
		}
		_E4BD.InitializeGPUInstancer(gPUInstancerDetailManager);
	}

	private void _E001()
	{
		for (int i = 0; i < this.m__E001; i++)
		{
			GPUInstancerDetailManager component = this.m__E004[i].GetComponent<GPUInstancerDetailManager>();
			for (int j = 0; j < component.prototypeList.Count; j++)
			{
				GPUInstancerDetailPrototype obj = (GPUInstancerDetailPrototype)component.prototypeList[j];
				obj.detailHealthyColor = this.m__E006;
				obj.detailDryColor = this.m__E007;
				obj.noiseSpread = this.m__E008;
				obj.ambientOcclusion = this.m__E009;
				obj.gradientPower = _E00A;
				obj.windIdleSway = _E00B;
				obj.windWavesOn = _E00C;
				obj.windWaveTint = _E00D;
				obj.windWaveSway = _E00F;
				obj.windWaveTintColor = _E010;
				obj.isBillboard = !_E012 && _E011;
				obj.useCrossQuads = _E012;
				obj.quadCount = _E013;
				obj.billboardDistance = _E014;
				obj.detailScale = _E015;
				obj.isShadowCasting = _E016;
				obj.isFrustumCulling = _E017;
				obj.maxDistance = _E019;
			}
			_E4BD.UpdateDetailInstances(component, updateMeshes: true);
		}
	}

	public void ReInitializeManagers()
	{
		for (int i = 0; i < this.m__E001; i++)
		{
			_E4BD.InitializeGPUInstancer(this.m__E004[i].GetComponent<GPUInstancerDetailManager>());
		}
	}

	private void _E002()
	{
		_E035 = GameObject.Find(_ED3E._E000(66300)).GetComponent<Canvas>();
		_E033 = GameObject.Find(_ED3E._E000(66299)).GetComponent<Selectable>();
		_E034 = GameObject.Find(_ED3E._E000(66276)).GetComponent<Selectable>();
		_E031 = GameObject.Find(_ED3E._E000(66320)).GetComponent<Text>();
		_E032 = GameObject.Find(_ED3E._E000(66364)).GetComponent<Text>();
		_E01A = GameObject.Find(_ED3E._E000(66341)).transform.GetChild(0).GetComponent<Image>();
		GameObject.Find(_ED3E._E000(66341)).GetComponent<ColorPicker>().Color = this.m__E006;
		GameObject.Find(_ED3E._E000(66341)).GetComponent<ColorPicker>().SetOnValueChangeCallback(UpdateDetailSettings);
		_E01B = GameObject.Find(_ED3E._E000(66384)).transform.GetChild(0).GetComponent<Image>();
		GameObject.Find(_ED3E._E000(66384)).GetComponent<ColorPicker>().Color = this.m__E007;
		GameObject.Find(_ED3E._E000(66384)).GetComponent<ColorPicker>().SetOnValueChangeCallback(UpdateDetailSettings);
		_E01C = GameObject.Find(_ED3E._E000(66375)).GetComponent<Slider>();
		_E01D = GameObject.Find(_ED3E._E000(66417)).GetComponent<Slider>();
		_E01E = GameObject.Find(_ED3E._E000(66456)).GetComponent<Slider>();
		_E01F = GameObject.Find(_ED3E._E000(66436)).GetComponent<Slider>();
		_E020 = GameObject.Find(_ED3E._E000(66487)).GetComponent<Toggle>();
		_E021 = GameObject.Find(_ED3E._E000(66471)).GetComponent<Slider>();
		_E022 = GameObject.Find(_ED3E._E000(66515)).GetComponent<Slider>();
		_E023 = GameObject.Find(_ED3E._E000(66559)).GetComponent<Slider>();
		_E024 = GameObject.Find(_ED3E._E000(66539)).transform.GetChild(0).GetComponent<Image>();
		GameObject.Find(_ED3E._E000(66539)).GetComponent<ColorPicker>().Color = _E010;
		GameObject.Find(_ED3E._E000(66539)).GetComponent<ColorPicker>().SetOnValueChangeCallback(UpdateDetailSettings);
		_E025 = GameObject.Find(_ED3E._E000(68620)).GetComponent<Toggle>();
		_E026 = GameObject.Find(_ED3E._E000(68668)).GetComponent<Toggle>();
		_E027 = GameObject.Find(_ED3E._E000(68653)).GetComponent<Slider>();
		_E028 = GameObject.Find(_ED3E._E000(68699)).GetComponent<Slider>();
		_E029 = GameObject.Find(_ED3E._E000(68725)).GetComponent<InputField>();
		_E02A = GameObject.Find(_ED3E._E000(68764)).GetComponent<InputField>();
		_E02B = GameObject.Find(_ED3E._E000(68747)).GetComponent<InputField>();
		_E02C = GameObject.Find(_ED3E._E000(68787)).GetComponent<InputField>();
		_E02D = GameObject.Find(_ED3E._E000(68827)).GetComponent<Toggle>();
		_E02E = GameObject.Find(_ED3E._E000(68807)).GetComponent<Toggle>();
		_E02F = GameObject.Find(_ED3E._E000(68844)).GetComponent<Slider>();
		_E030 = GameObject.Find(_ED3E._E000(68888)).GetComponent<Slider>();
	}

	public void UpdateDetailSettings()
	{
		this.m__E006 = _E01A.color;
		this.m__E007 = _E01B.color;
		this.m__E008 = _E01C.value;
		this.m__E009 = _E01D.value;
		_E00A = _E01E.value;
		_E00B = _E01F.value;
		_E00C = _E020.isOn;
		_E00D = _E021.value;
		_E00E = _E022.value;
		_E00F = _E023.value;
		_E010 = _E024.color;
		_E011 = !_E026.isOn && _E025.isOn;
		_E025.isOn = _E011;
		_E012 = _E026.isOn;
		_E013 = (int)_E027.value;
		_E014 = _E028.value;
		if (!float.TryParse(_E029.text, out _E015.x))
		{
			_E015.x = 1f;
			_E029.text = _ED3E._E000(68874);
		}
		if (!float.TryParse(_E02A.text, out _E015.y))
		{
			_E015.y = 1f;
			_E02A.text = _ED3E._E000(68874);
		}
		if (!float.TryParse(_E02B.text, out _E015.z))
		{
			_E015.z = 1f;
			_E02B.text = _ED3E._E000(68874);
		}
		if (!float.TryParse(_E02C.text, out _E015.w))
		{
			_E015.w = 1f;
			_E02C.text = _ED3E._E000(68874);
		}
		_E016 = _E02D.isOn;
		_E017 = _E02E.isOn;
		_E018 = _E02F.value;
		_E019 = _E030.value;
		_E001();
	}

	public void ShowHelpDescription(Text itemTitle)
	{
		_E032.text = itemTitle.text;
		string text = itemTitle.text;
		switch (_ED3C._E000(text))
		{
		case 3045790633u:
			if (text == _ED3E._E000(68870))
			{
				_E031.text = HELPTEXT_detailHealthyColor;
			}
			break;
		case 1195358637u:
			if (text == _ED3E._E000(68916))
			{
				_E031.text = HELPTEXT_detailDryColor;
			}
			break;
		case 159681588u:
			if (text == _ED3E._E000(68910))
			{
				_E031.text = HELPTEXT_noiseSpread;
			}
			break;
		case 178565150u:
			if (text == _ED3E._E000(68899))
			{
				_E031.text = HELPTEXT_ambientOcclusion;
			}
			break;
		case 154768752u:
			if (text == _ED3E._E000(68941))
			{
				_E031.text = HELPTEXT_gradientPower;
			}
			break;
		case 1239456949u:
			if (text == _ED3E._E000(68988))
			{
				_E031.text = HELPTEXT_windIdleSway;
			}
			break;
		case 1789134789u:
			if (text == _ED3E._E000(68979))
			{
				_E031.text = HELPTEXT_windWavesOn;
			}
			break;
		case 219126802u:
			if (text == _ED3E._E000(68966))
			{
				_E031.text = HELPTEXT_windWaveTint;
			}
			break;
		case 1997187254u:
			if (text == _ED3E._E000(69014))
			{
				_E031.text = HELPTEXT_windWaveSize;
			}
			break;
		case 4102596883u:
			if (text == _ED3E._E000(68998))
			{
				_E031.text = HELPTEXT_windWaveSway;
			}
			break;
		case 5515769u:
			if (text == _ED3E._E000(69046))
			{
				_E031.text = HELPTEXT_windWaveTintColor;
			}
			break;
		case 2802141700u:
			if (text == _ED3E._E000(69084))
			{
				_E031.text = HELPTEXT_isBillboard;
			}
			break;
		case 3876160651u:
			if (text == _ED3E._E000(69078))
			{
				_E031.text = HELPTEXT_crossQuads;
			}
			break;
		case 1743262317u:
			if (text == _ED3E._E000(69066))
			{
				_E031.text = HELPTEXT_quadCount;
			}
			break;
		case 4108190592u:
			if (text == _ED3E._E000(69115))
			{
				_E032.text = _ED3E._E000(69192);
				_E031.text = HELPTEXT_billboardDistance;
			}
			break;
		case 676498961u:
			if (text == _ED3E._E000(69094))
			{
				_E031.text = HELPTEXT_detailScale;
			}
			break;
		case 3157479340u:
			if (text == _ED3E._E000(69148))
			{
				_E031.text = HELPTEXT_isShadowCasting;
			}
			break;
		case 2820061190u:
			if (text == _ED3E._E000(69139))
			{
				_E031.text = HELPTEXT_isFrustumCulling;
			}
			break;
		case 1799054852u:
			if (text == _ED3E._E000(69177))
			{
				_E031.text = HELPTEXT_frustumOffset;
			}
			break;
		case 2090033114u:
			if (text == _ED3E._E000(69160))
			{
				_E031.text = HELPTEXT_maxDetailDistance;
			}
			break;
		case 1555381858u:
			if (text == _ED3E._E000(69204))
			{
				_E031.text = HELPTEXT_windVector;
			}
			break;
		}
	}

	public void HideHelpDescription()
	{
		_E032.text = _ED3E._E000(69230);
		_E031.text = _ED3E._E000(69218);
	}

	private void _E003()
	{
		_E033.interactable = this.m__E001 < 9;
		_E034.interactable = this.m__E001 > 0;
	}

	private void _E004()
	{
		if ((bool)FpsController && (bool)FixedCamera)
		{
			FpsController.SetActive(this.m__E005);
			FixedCamera.SetActive(!this.m__E005);
			GPUInstancerDetailManager[] array = Object.FindObjectsOfType<GPUInstancerDetailManager>();
			for (int i = 0; i < array.Length; i++)
			{
				_ = array[i];
				_E4BD.SetCamera(this.m__E005 ? FpsController.GetComponentInChildren<Camera>() : FixedCamera.GetComponentInChildren<Camera>());
			}
			if (!this.m__E005)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}

	private void _E005()
	{
		SplatPrototype[] array = new SplatPrototype[1]
		{
			new SplatPrototype()
		};
		array[0].texture = groundTexture;
		DetailPrototype[] array2 = new DetailPrototype[1]
		{
			new DetailPrototype()
		};
		array2[0].prototypeTexture = detailTexture;
		array2[0].renderMode = DetailRenderMode.GrassBillboard;
		Vector3 position = Vector3.zero + this.m__E002 * (this.m__E001 % 3) + this.m__E003 * Mathf.FloorToInt((float)this.m__E001 / 3f);
		this.m__E004[this.m__E001] = _E006(position, this.m__E000, (float)this.m__E000 / 2f, 16, 16, array, array2);
		this.m__E004[this.m__E001].transform.SetParent(base.transform);
		_E009(this.m__E004[this.m__E001]);
	}

	private Terrain _E006(Vector3 position, int terrainSize, float terrainHeight, int baseTextureResolution = 16, int detailResolutionPerPatch = 16, SplatPrototype[] splatPrototypes = null, DetailPrototype[] detailPrototypes = null)
	{
		GameObject obj = new GameObject(_ED3E._E000(69562));
		obj.transform.position = position;
		Terrain terrain = obj.AddComponent<Terrain>();
		TerrainData terrainData2 = (obj.AddComponent<TerrainCollider>().terrainData = _E007(terrainSize, terrainHeight, baseTextureResolution, detailResolutionPerPatch, splatPrototypes, detailPrototypes));
		terrain.terrainData = terrainData2;
		return terrain;
	}

	private TerrainData _E007(int terrainSize, float terrainHeight, int baseTextureResolution = 16, int detailResolutionPerPatch = 16, SplatPrototype[] splatPrototypes = null, DetailPrototype[] detailPrototypes = null)
	{
		TerrainData terrainData = new TerrainData();
		terrainData.heightmapResolution = terrainSize + 1;
		terrainData.baseMapResolution = baseTextureResolution;
		terrainData.alphamapResolution = terrainSize;
		terrainData.SetDetailResolution(terrainSize, detailResolutionPerPatch);
		terrainData.terrainLayers = _E008(splatPrototypes);
		terrainData.detailPrototypes = detailPrototypes;
		terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);
		return terrainData;
	}

	private TerrainLayer[] _E008(SplatPrototype[] splatPrototypes)
	{
		if (splatPrototypes == null)
		{
			return null;
		}
		TerrainLayer[] array = new TerrainLayer[splatPrototypes.Length];
		for (int i = 0; i < splatPrototypes.Length; i++)
		{
			array[i] = new TerrainLayer
			{
				diffuseTexture = splatPrototypes[i].texture,
				normalMapTexture = splatPrototypes[i].normalMap
			};
		}
		return array;
	}

	private void _E009(Terrain terrain)
	{
		int[,] array = new int[terrain.terrainData.detailResolution, terrain.terrainData.detailResolution];
		for (int i = 0; i < terrain.terrainData.detailPrototypes.Length; i++)
		{
			for (int j = 0; j < terrain.terrainData.detailResolution; j++)
			{
				for (int k = 0; k < terrain.terrainData.detailResolution; k++)
				{
					array[j, k] = Random.Range(4, 8);
				}
			}
			terrain.terrainData.SetDetailLayer(0, 0, i, array);
		}
		terrain.detailObjectDistance = 250f;
	}
}
