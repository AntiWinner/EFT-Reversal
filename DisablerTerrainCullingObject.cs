using UnityEngine;

[ExecuteInEditMode]
public class DisablerTerrainCullingObject : DisablerCullingObjectBase
{
	public Terrain Terrain;

	public TerrainLod TerrainLod;

	public bool IsDisableTerrainDraw = true;

	public bool IsDisableTerrainTreesDraw = true;

	protected override void Awake()
	{
		base.Awake();
		if (TerrainLod == null && Terrain != null)
		{
			TerrainLod = Terrain.GetComponent<TerrainLod>();
		}
		if (TerrainLod != null)
		{
			TerrainLod.SetSettings(IsDisableTerrainDraw, IsDisableTerrainTreesDraw);
		}
	}

	protected override void SetComponentsEnabled(bool hasEntered)
	{
		if (TerrainLod != null)
		{
			TerrainLod.TerrainIsVisible = hasEntered;
		}
		else if (!(Terrain == null))
		{
			if (IsDisableTerrainDraw)
			{
				Terrain.drawHeightmap = hasEntered;
			}
			if (IsDisableTerrainTreesDraw)
			{
				Terrain.drawTreesAndFoliage = hasEntered;
			}
		}
	}
}
