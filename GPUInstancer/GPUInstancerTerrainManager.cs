using UnityEngine;

namespace GPUInstancer;

public abstract class GPUInstancerTerrainManager : GPUInstancerManager
{
	[SerializeField]
	private Terrain _terrain;

	protected bool replacingInstances;

	protected bool initalizingInstances;

	public Terrain terrain => _terrain;

	protected override bool isCulled => !_terrain.drawHeightmap;

	public override void Awake()
	{
		base.Awake();
		if (Application.isPlaying && useFloatingOriginHandler && terrain != null)
		{
			floatingOriginTransform = terrain.transform;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void Reset()
	{
		base.Reset();
		if (terrain == null && base.gameObject.GetComponent<Terrain>() != null)
		{
			SetupManagerWithTerrain(base.gameObject.GetComponent<Terrain>());
		}
	}

	public virtual void SetupManagerWithTerrain(Terrain terrain, bool isOptic = false)
	{
		if (!(terrain == null) && !Application.isPlaying)
		{
			_terrain = terrain;
			IsOptic = isOptic;
			prototypeList.Clear();
			terrainSettings = _E000(terrain, isOptic);
			GeneratePrototypes();
			AddProxyToTerrain();
		}
	}

	public GPUInstancerTerrainProxy AddProxyToTerrain()
	{
		return null;
	}

	private GPUInstancerTerrainSettings _E000(Terrain terrain, bool isOptic = false)
	{
		string assetGUID = _E4C8.GetAssetGUID(terrain.terrainData);
		GPUInstancerTerrainSettings gPUInstancerTerrainSettings = ScriptableObject.CreateInstance<GPUInstancerTerrainSettings>();
		gPUInstancerTerrainSettings.name = (string.IsNullOrEmpty(terrain.terrainData.name) ? terrain.gameObject.name : terrain.terrainData.name) + _ED3E._E000(48793) + assetGUID.Substring(0, 6) + (isOptic ? _ED3E._E000(77769) : "");
		gPUInstancerTerrainSettings.isOptic = isOptic;
		gPUInstancerTerrainSettings.terrainDataGUID = _E4C8.GetAssetGUID(terrain.terrainData);
		gPUInstancerTerrainSettings.maxDetailDistance = _E4BF.gpuiSettings.GetDefaultDetailDistance(isOptic);
		gPUInstancerTerrainSettings.frustumOffset = _E4BF.gpuiSettings.GetDefaultFrustumOffset(isOptic);
		gPUInstancerTerrainSettings.maxDetailDistanceLegacy = terrain.detailObjectDistance;
		gPUInstancerTerrainSettings.maxTreeDistance = terrain.treeDistance;
		gPUInstancerTerrainSettings.detailDensity = _E4BF.gpuiSettings.GetDefaultDetailDensity(terrain, isOptic);
		gPUInstancerTerrainSettings.autoSPCellSize = false;
		gPUInstancerTerrainSettings.preferedSPCellSize = _E4BF.gpuiSettings.GetDefaultDetailCellSize(isOptic);
		gPUInstancerTerrainSettings.healthyDryNoiseTexture = Resources.Load<Texture2D>(_E4BF.NOISE_TEXTURES_PATH + _E4BF.DEFAULT_HEALTHY_DRY_NOISE);
		gPUInstancerTerrainSettings.windWaveNormalTexture = Resources.Load<Texture2D>(_E4BF.NOISE_TEXTURES_PATH + _E4BF.DEFAULT_WIND_WAVE_NOISE);
		gPUInstancerTerrainSettings.wavingGrassTint = terrain.terrainData.wavingGrassTint;
		return gPUInstancerTerrainSettings;
	}

	private static void _E001(GPUInstancerTerrainSettings terrainSettings)
	{
	}
}
