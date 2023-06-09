using UnityEngine;

[ExecuteInEditMode]
public class TerrainLod : MonoBehaviour
{
	[SerializeField]
	private Terrain _terrain;

	[SerializeField]
	private GameObject _terrainLod;

	private bool _E000 = true;

	private bool _E001 = true;

	private bool _E002 = true;

	public bool TerrainIsVisible
	{
		get
		{
			return _E002;
		}
		set
		{
			if (_E000)
			{
				_terrain.drawHeightmap = value;
			}
			if (_E001)
			{
				_terrain.drawTreesAndFoliage = value;
			}
			_terrainLod.SetActive(!value);
			_E002 = value;
		}
	}

	public void SetSettings(bool disableTerrainDraw, bool disableTerrainTreesDraw)
	{
		_E000 = disableTerrainDraw;
		_E001 = disableTerrainTreesDraw;
	}

	public void DestroyLod()
	{
		if (_terrainLod != null)
		{
			Object.DestroyImmediate(_terrainLod);
			_terrainLod = null;
		}
	}
}
