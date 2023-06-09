using System;
using System.Collections.Generic;
using EFT.Weather;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class CloudsController : MonoBehaviour
{
	[Serializable]
	public class CloudAutomatization
	{
		[Header("midnight(left) sunrize/sunset(center) midday(right)")]
		public Gradient CloudColor;

		public Gradient SunMultyplyer;

		public Gradient BottomReflections;

		public Gradient CloudySun;

		[Range(0f, 1f)]
		public float CloudColorOvercast;

		[Range(0f, 1f)]
		public float SunMultyplyerOvercast;

		[Range(0f, 50f)]
		public float PlanetSizeOvercast;

		public void Update(CloudLayer[] cloudLayers)
		{
			TOD_Sky instance = TOD_Sky.Instance;
			float y = instance.SunDirection.y;
			y = (y + 1f) * 0.5f;
			Color cloudColor = _E000(instance, y);
			Color sunMultyplyer = _E001(instance, y);
			Color bottomReflections = BottomReflections.Evaluate(y);
			Color cloudySun = CloudySun.Evaluate(y);
			float planetSize = 3f + instance.Atmosphere.Fogginess * PlanetSizeOvercast;
			_E002(cloudLayers, cloudColor, sunMultyplyer, bottomReflections, cloudySun, planetSize);
		}

		private Color _E000(TOD_Sky todSky, float t)
		{
			return CloudColor.Evaluate(t) * (1f - todSky.Atmosphere.Fogginess * CloudColorOvercast);
		}

		private Color _E001(TOD_Sky todSky, float t)
		{
			return ToDController.SaturateColor(SunMultyplyer.Evaluate(t), 1f - TOD_Sky.Instance.Atmosphere.Fogginess) * (1f - todSky.Atmosphere.Fogginess * SunMultyplyerOvercast);
		}

		private void _E002(CloudLayer[] cloudLayers, Color cloudColor, Color sunMultyplyer, Color bottomReflections, Color cloudySun, float planetSize)
		{
			foreach (CloudLayer obj in cloudLayers)
			{
				obj.SetColor(_ED3E._E000(95336), cloudColor);
				obj.SetColor(_ED3E._E000(95388), sunMultyplyer);
				obj.SetColor(_ED3E._E000(95379), bottomReflections);
				obj.SetColor(_ED3E._E000(95422), cloudySun);
				obj.SetFloat(_ED3E._E000(95409), planetSize);
			}
		}
	}

	[Serializable]
	public class CloudLayer
	{
		public bool Enabled;

		[Range(0f, 1f)]
		public float RoughnessMin;

		[Range(0f, 1f)]
		public float NoiseMapRoughness;

		[Range(-1f, 1f)]
		public float DensityShift;

		[Space(16f)]
		public float Height = 400f;

		public float Curviness = 1f;

		public float Scale = 0.1f;

		[Space(16f)]
		public float CloudPositionMultyply = 1f;

		public Vector2 CloudPositionShift;

		[Space(16f)]
		[Range(0f, 1f)]
		public float ShadowStrength;

		[HideInInspector]
		public bool IsDrawLight;

		private Material _cloudMaterial;

		private Material _lightMaterial;

		private Material _pointLightMaterial;

		private MaterialPropertyBlock _cloudBlock;

		private MaterialPropertyBlock _lightBlock;

		private MaterialPropertyBlock _pointLightBlock;

		private MaterialPropertyBlock[] _materialBlocks;

		private Texture _lightMap;

		private Vector4 _matTransform;

		private Vector4 _lightPosition;

		private Color _lightIntensity;

		private Color _pointLightIntensity;

		public float Density { private get; set; }

		public float FogDensity
		{
			set
			{
				_cloudMaterial.SetFloat(CloudsController.m__E000, value);
			}
		}

		public float GetRealDensity()
		{
			return Density + DensityShift;
		}

		public void SetLightSettings(Texture lightMap, Vector4 matTransform, Vector4 lightPosition)
		{
			_lightMap = lightMap;
			_matTransform = matTransform;
			_lightPosition = lightPosition;
		}

		public void SetLightIntensity(Color lightIntensity, Color pointLightIntensity)
		{
			_lightIntensity = lightIntensity;
			_pointLightIntensity = pointLightIntensity;
		}

		public void SetColor(string name, Color color)
		{
			_cloudMaterial.SetColor(name, color);
		}

		public void SetFloat(string name, float value)
		{
			_cloudMaterial.SetFloat(name, value);
		}

		public void Initialize(bool full = true)
		{
			if (full)
			{
				_E000();
			}
			else
			{
				_cloudMaterial.CopyPropertiesFromMaterial(Instance.CloudMaterial);
			}
			_E001();
		}

		public Vector2 GetCloudPosition(Vector2 cloudPosition)
		{
			cloudPosition *= CloudPositionMultyply;
			cloudPosition += CloudPositionShift;
			cloudPosition.x %= 100f;
			return cloudPosition;
		}

		public void Update(int index, int layer)
		{
			if (Enabled)
			{
				Vector2 cloudPosition = GetCloudPosition(Instance.CloudPosition);
				float density = Density + DensityShift;
				_E002(cloudPosition, density, index);
				Graphics.DrawMesh(Instance._E00F, Matrix4x4.identity, _cloudMaterial, layer, null, 0, _cloudBlock, castShadows: false, receiveShadows: false);
				if (IsDrawLight)
				{
					_E003(cloudPosition, density, layer);
				}
			}
		}

		private void _E000()
		{
			_cloudMaterial = UnityEngine.Object.Instantiate(Instance.CloudMaterial);
			_lightMaterial = UnityEngine.Object.Instantiate(Instance.CloudLightMaterial);
			_pointLightMaterial = UnityEngine.Object.Instantiate(Instance.CloudPointLightMaterial);
			_cloudBlock = new MaterialPropertyBlock();
			_lightBlock = new MaterialPropertyBlock();
			_pointLightBlock = new MaterialPropertyBlock();
			_materialBlocks = new MaterialPropertyBlock[3] { _cloudBlock, _lightBlock, _pointLightBlock };
		}

		private void _E001()
		{
			for (int i = 0; i < _materialBlocks.Length; i++)
			{
				_materialBlocks[i].SetFloat(CloudsController._E001, RoughnessMin);
				_materialBlocks[i].SetFloat(CloudsController._E002, NoiseMapRoughness);
				_materialBlocks[i].SetFloat(CloudsController._E003, Density + DensityShift);
				_materialBlocks[i].SetFloat(_E004, Curviness);
				_materialBlocks[i].SetFloat(_E005, Scale);
				_materialBlocks[i].SetFloat(_E006, Height);
			}
		}

		private void _E002(Vector2 cloudPosition, float density, int index)
		{
			_cloudBlock.SetVector(_E007, cloudPosition);
			_cloudBlock.SetFloat(CloudsController._E003, density);
			_cloudBlock.SetFloat(_E008, Mathf.Lerp(0.5f, 0f, 0f - Density));
			_cloudBlock.SetFloat(_E009, (index != 0) ? 1 : 0);
			_cloudMaterial.renderQueue = 1050 + index;
		}

		private void _E003(Vector2 cloudPosition, float density, int layer)
		{
			_lightBlock.SetVector(_E007, cloudPosition);
			_lightBlock.SetVector(_E00A, _matTransform);
			_lightBlock.SetTexture(_E00B, _lightMap);
			_lightBlock.SetColor(_E00C, _lightIntensity);
			_lightBlock.SetFloat(CloudsController._E003, density);
			Graphics.DrawMesh(Instance._E00F, Matrix4x4.identity, _lightMaterial, layer, null, 0, _lightBlock, castShadows: false, receiveShadows: false);
			_pointLightBlock.SetVector(_E007, cloudPosition);
			_pointLightBlock.SetVector(_E00D, _lightPosition);
			_pointLightBlock.SetColor(_E00C, _pointLightIntensity);
			_pointLightBlock.SetFloat(CloudsController._E003, density);
			Graphics.DrawMesh(Instance._E00F, Matrix4x4.identity, _pointLightMaterial, layer, null, 0, _pointLightBlock, castShadows: false, receiveShadows: false);
		}
	}

	[Serializable]
	public class CloudShadows
	{
		[Flags]
		private enum ECommandBufferVariant : byte
		{
			Blur = 1,
			Modify = 2,
			Draw = 4
		}

		private int _lightdirection_ID;

		private int _sunmatrix_ID;

		private int _cloudroughnessmin_ID;

		private int _cloudnoisemaproughness_ID;

		private int _clouddensity_ID;

		private int _cloudcurviness_ID;

		private int _cloudscale_ID;

		private int _cloudposition_ID;

		private int _shadowscale_ID;

		private int _shadowstrength_ID;

		private int _addColorFieldID = Shader.PropertyToID(_ED3E._E000(86324));

		private int _multColorFieldID = Shader.PropertyToID(_ED3E._E000(86318));

		private int _intensityFieldID = Shader.PropertyToID(_ED3E._E000(35970));

		private int _blurOffsets0FieldID = Shader.PropertyToID(_ED3E._E000(86300));

		public Material CloudShadowMaterial;

		public int TextureSize = 512;

		public float Blur;

		public float ViewDistance = 1000f;

		public float ShadowScale = 1f;

		private Material[] _cloudShadowMaterial;

		private Material _cloudModifyMaterial;

		private Material _cloudBlurMaterial0;

		private Material _cloudBlurMaterial1;

		private Material _cloudColorBlendMaterial;

		private Transform _playerT;

		private Vector3 _lastPlayerPosition = Vector3.zero;

		private Transform _lightT;

		private RenderTexture _renderTexture0;

		private RenderTexture _renderTexture1;

		private Light _light;

		private CommandBuffer[] _commandBuffer;

		private readonly CameraEvent _cameraEvent = CameraEvent.BeforeGBuffer;

		private ECommandBufferVariant _prevCommandBufferVariant;

		public void Initialize(CloudLayer[] cloudLayers, Transform player, Transform light)
		{
			_lightdirection_ID = Shader.PropertyToID(_ED3E._E000(89961));
			_sunmatrix_ID = Shader.PropertyToID(_ED3E._E000(95397));
			_cloudroughnessmin_ID = Shader.PropertyToID(_ED3E._E000(84935));
			_cloudnoisemaproughness_ID = Shader.PropertyToID(_ED3E._E000(84978));
			_clouddensity_ID = Shader.PropertyToID(_ED3E._E000(95258));
			_cloudcurviness_ID = Shader.PropertyToID(_ED3E._E000(95240));
			_cloudscale_ID = Shader.PropertyToID(_ED3E._E000(95288));
			_cloudposition_ID = Shader.PropertyToID(_ED3E._E000(95264));
			_shadowscale_ID = Shader.PropertyToID(_ED3E._E000(95448));
			_shadowstrength_ID = Shader.PropertyToID(_ED3E._E000(95437));
			_addColorFieldID = Shader.PropertyToID(_ED3E._E000(86324));
			_multColorFieldID = Shader.PropertyToID(_ED3E._E000(86318));
			_intensityFieldID = Shader.PropertyToID(_ED3E._E000(35970));
			_blurOffsets0FieldID = Shader.PropertyToID(_ED3E._E000(86300));
			_renderTexture0 = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32)
			{
				name = _ED3E._E000(95485)
			};
			_renderTexture1 = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.ARGB32)
			{
				name = _ED3E._E000(95458)
			};
			_cloudModifyMaterial = new Material(_E3AC.Find(_ED3E._E000(38647)));
			_cloudBlurMaterial0 = new Material(_E3AC.Find(_ED3E._E000(37933)));
			_cloudBlurMaterial1 = new Material(_E3AC.Find(_ED3E._E000(37933)));
			_playerT = player;
			_lightT = light;
			_light = light.GetComponent<Light>();
			_light.cookie = _renderTexture0;
			_light.cookieSize = ViewDistance * 2f;
			_cloudShadowMaterial = new Material[cloudLayers.Length];
			for (int i = 0; i < cloudLayers.Length; i++)
			{
				_cloudShadowMaterial[i] = UnityEngine.Object.Instantiate(CloudShadowMaterial);
			}
			_commandBuffer = new CommandBuffer[6];
			for (int j = 0; j < _commandBuffer.Length; j++)
			{
				_commandBuffer[j] = new CommandBuffer();
				_commandBuffer[j].name = _ED3E._E000(95495) + j;
				_commandBuffer[j].SetRenderTarget(_renderTexture0);
				_E40B.ColorBlend(_commandBuffer[j], Color.white, null, BlendMode.One, BlendMode.Zero);
				if (j > 1)
				{
					_E002(_commandBuffer[j], cloudLayers);
				}
				if (j < 2 || j > 3)
				{
					_E004(_commandBuffer[j]);
				}
				if (j % 2 == 1)
				{
					_E006(_commandBuffer[j], _renderTexture0, _renderTexture1);
				}
			}
			Camera camera = _E8A8.Instance.Camera;
			_prevCommandBufferVariant = ECommandBufferVariant.Modify;
			camera.AddCommandBuffer(_cameraEvent, _commandBuffer[_E000(_prevCommandBufferVariant)]);
		}

		public void Draw(CloudLayer[] cloudLayers, Vector2 cloudPosition)
		{
			if (!(_playerT == null))
			{
				if (Vector3.SqrMagnitude(_playerT.position - _lastPlayerPosition) > Instance.SunPositionUpdateDeltaSqrMangitude)
				{
					Vector3 position = _playerT.position;
					_lightT.position = position;
					_lastPlayerPosition = position;
				}
				RenderTexture active = RenderTexture.active;
				float num = Mathf.Clamp01(Mathf.Clamp01(1f - Math.Abs(_lightT.forward.y) * 20f) * 4f);
				ECommandBufferVariant eCommandBufferVariant = ((num < 0.999999f) ? ECommandBufferVariant.Draw : ECommandBufferVariant.Modify);
				if (num < 0.999999f)
				{
					eCommandBufferVariant |= ECommandBufferVariant.Draw;
					_E001(cloudLayers, cloudPosition);
				}
				if (num > 0f)
				{
					eCommandBufferVariant |= ECommandBufferVariant.Modify;
					_E003(cloudLayers, num);
				}
				if (Blur > 0f)
				{
					eCommandBufferVariant |= ECommandBufferVariant.Blur;
					_E005(_renderTexture0, Blur);
				}
				if (eCommandBufferVariant != _prevCommandBufferVariant)
				{
					Camera camera = _E8A8.Instance.Camera;
					int num2 = _E000(_prevCommandBufferVariant);
					int num3 = _E000(eCommandBufferVariant);
					camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer[num2]);
					camera.AddCommandBuffer(_cameraEvent, _commandBuffer[num3]);
				}
				_prevCommandBufferVariant = eCommandBufferVariant;
				RenderTexture.active = active;
			}
		}

		public void Destroy()
		{
			if (_renderTexture0 != null)
			{
				UnityEngine.Object.DestroyImmediate(_renderTexture0);
			}
			if (_renderTexture1 != null)
			{
				UnityEngine.Object.DestroyImmediate(_renderTexture1);
			}
		}

		private int _E000(ECommandBufferVariant commandBufferVariant)
		{
			return _E3A5<ECommandBufferVariant>.GetIntFromValue(commandBufferVariant) - 2;
		}

		private void _E001(IReadOnlyList<CloudLayer> cloudLayers, Vector2 cloudPosition)
		{
			for (int i = 0; i < cloudLayers.Count; i++)
			{
				CloudLayer cloudLayer = cloudLayers[i];
				_cloudShadowMaterial[i].SetVector(_lightdirection_ID, _lightT.forward);
				_cloudShadowMaterial[i].SetMatrix(_sunmatrix_ID, _lightT.localToWorldMatrix);
				_cloudShadowMaterial[i].SetFloat(_cloudroughnessmin_ID, cloudLayer.RoughnessMin);
				_cloudShadowMaterial[i].SetFloat(_cloudnoisemaproughness_ID, cloudLayer.NoiseMapRoughness);
				_cloudShadowMaterial[i].SetFloat(_clouddensity_ID, cloudLayer.GetRealDensity());
				_cloudShadowMaterial[i].SetFloat(_cloudcurviness_ID, cloudLayer.Curviness);
				_cloudShadowMaterial[i].SetFloat(_cloudscale_ID, cloudLayer.Scale);
				_cloudShadowMaterial[i].SetVector(_cloudposition_ID, cloudLayer.GetCloudPosition(cloudPosition));
				_cloudShadowMaterial[i].SetFloat(_shadowscale_ID, ShadowScale / cloudLayer.Height);
				_cloudShadowMaterial[i].SetFloat(_shadowstrength_ID, cloudLayer.ShadowStrength);
			}
		}

		private void _E002(CommandBuffer commandBuffer, IReadOnlyList<CloudLayer> cloudLayers)
		{
			for (int i = 0; i < cloudLayers.Count; i++)
			{
				_E40B.DrawFullscreenTriangle(commandBuffer, _cloudShadowMaterial[i], null);
			}
		}

		private void _E003(IEnumerable<CloudLayer> cloudLayers, float hide)
		{
			float num = 1f;
			foreach (CloudLayer cloudLayer in cloudLayers)
			{
				num *= (0.5f - cloudLayer.GetRealDensity() * 0.5f) * (1f - cloudLayer.ShadowStrength);
			}
			float num2 = num * hide;
			float num3 = 1f - hide;
			Color value = new Color(num2, num2, num2, num2);
			Color value2 = new Color(num3, num3, num3, num3);
			_cloudModifyMaterial.SetColor(_addColorFieldID, value);
			_cloudModifyMaterial.SetColor(_multColorFieldID, value2);
		}

		private void _E004(CommandBuffer commandBuffer)
		{
			_cloudModifyMaterial.mainTexture = _renderTexture0;
			_E40B.DrawFullscreenTriangle(commandBuffer, _cloudModifyMaterial, _renderTexture1);
			commandBuffer.Blit(_renderTexture1, _renderTexture0);
		}

		private void _E005(Texture source, float blurInPixels, float intensity = 1f)
		{
			Vector2 vector = new Vector2(blurInPixels / (float)source.width, blurInPixels / (float)source.height);
			_cloudBlurMaterial0.SetFloat(_intensityFieldID, intensity);
			_cloudBlurMaterial0.SetVector(_blurOffsets0FieldID, new Vector2(vector.x, 0f));
			_cloudBlurMaterial1.SetFloat(_intensityFieldID, intensity);
			_cloudBlurMaterial1.SetVector(_blurOffsets0FieldID, new Vector2(0f, vector.y));
		}

		private void _E006(CommandBuffer commandBuffer, RenderTexture source, RenderTexture temp)
		{
			_cloudBlurMaterial0.mainTexture = source;
			_cloudBlurMaterial1.mainTexture = temp;
			_E40B.DrawFullscreenTriangle(commandBuffer, _cloudBlurMaterial0, temp);
			_E40B.DrawFullscreenTriangle(commandBuffer, _cloudBlurMaterial1, source);
		}
	}

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(84947));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(84935));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(84978));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(95258));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(95240));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(95288));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(95276));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(95264));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(95319));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(95298));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(84008));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(95350));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(19825));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(19778));

	public static CloudsController Instance;

	public Mesh Icosphere;

	public Material CloudMaterial;

	public Material CloudShadowMaterial;

	public Material CloudLightMaterial;

	public Material CloudPointLightMaterial;

	public float FogDensityMultyplier = 1f;

	public float SunPositionUpdateDeltaSqrMangitude = 100f;

	public CloudLayer[] CloudLayers;

	public CloudShadows Shadows;

	public CloudAutomatization Automatization;

	public Vector2 CloudPosition;

	private float _E00E;

	private Mesh _E00F;

	public float FogDensity
	{
		set
		{
			value *= FogDensityMultyplier;
			CloudLayer[] cloudLayers = CloudLayers;
			for (int i = 0; i < cloudLayers.Length; i++)
			{
				cloudLayers[i].FogDensity = value;
			}
		}
	}

	public float Density
	{
		get
		{
			return _E00E;
		}
		set
		{
			_E00E = value;
			CloudLayer[] cloudLayers = CloudLayers;
			for (int i = 0; i < cloudLayers.Length; i++)
			{
				cloudLayers[i].Density = value;
			}
		}
	}

	private void Awake()
	{
		Refresh();
	}

	public void Refresh()
	{
		Instance = this;
		_E000();
		CloudLayer[] cloudLayers = CloudLayers;
		for (int i = 0; i < cloudLayers.Length; i++)
		{
			cloudLayers[i].Initialize();
		}
	}

	private void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(_E00F);
		Shadows.Destroy();
	}

	public void SetPlayer(Transform player)
	{
		if (!(TOD_Sky.Instance == null))
		{
			Shadows.Initialize(CloudLayers, player, TOD_Sky.Instance.Components.LightTransform);
		}
	}

	private void LateUpdate()
	{
		Automatization.Update(CloudLayers);
		int layer = base.gameObject.layer;
		for (int i = 0; i < CloudLayers.Length; i++)
		{
			CloudLayers[i].Update(i, layer);
		}
		Shadows.Draw(CloudLayers, CloudPosition);
	}

	private void _E000()
	{
		_E00F = UnityEngine.Object.Instantiate(Icosphere);
		_E00F.bounds = new Bounds(Vector3.zero, Vector3.one * 12000000f);
	}
}
