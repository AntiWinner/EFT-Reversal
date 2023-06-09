using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerTerrainSettings : ScriptableObject
{
	public bool isOptic;

	public string terrainDataGUID;

	public float maxDetailDistance = 350f;

	public float maxDetailDistanceLegacy = 50f;

	[Range(0f, 2500f)]
	public float maxTreeDistance = 500f;

	public Texture2D healthyDryNoiseTexture;

	public Texture2D windWaveNormalTexture;

	public bool autoSPCellSize;

	[Range(25f, 500f)]
	public int preferedSPCellSize = 50;

	[Range(0f, 1f)]
	public float detailDensity = 1f;

	[Range(1f, 10f)]
	public int resizeDensityCount = 1;

	public string warningText;

	public Color wavingGrassTint;

	public float frustumOffset = 0.6f;

	public Texture2DArray densityMapArray;

	public Texture2D GetHealthyDryNoiseTexture(GPUInstancerDetailPrototype detailPrototype)
	{
		if (detailPrototype.useCustomHealthyDryNoiseTexture && detailPrototype.healthyDryNoiseTexture != null)
		{
			return detailPrototype.healthyDryNoiseTexture;
		}
		return healthyDryNoiseTexture;
	}
}
