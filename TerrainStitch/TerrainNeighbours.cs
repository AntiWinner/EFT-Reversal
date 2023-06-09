using System.Collections.Generic;
using UnityEngine;

namespace TerrainStitch;

public class TerrainNeighbours : MonoBehaviour
{
	private Terrain[] _E000;

	private Dictionary<int[], Terrain> _E001;

	public Vector2 firstPosition;

	private void Start()
	{
		CreateNeighbours();
	}

	public void CreateNeighbours()
	{
		if (_E001 == null)
		{
			_E001 = new Dictionary<int[], Terrain>(new _E435());
		}
		else
		{
			_E001.Clear();
		}
		_E000 = Terrain.activeTerrains;
		if (_E000.Length == 0)
		{
			return;
		}
		firstPosition = new Vector2(_E000[0].transform.position.x, _E000[0].transform.position.z);
		int num = (int)_E000[0].terrainData.size.x;
		int num2 = (int)_E000[0].terrainData.size.z;
		Terrain[] array = _E000;
		foreach (Terrain terrain in array)
		{
			int[] key = new int[2]
			{
				Mathf.RoundToInt((terrain.transform.position.x - firstPosition.x) / (float)num),
				Mathf.RoundToInt((terrain.transform.position.z - firstPosition.y) / (float)num2)
			};
			_E001.Add(key, terrain);
		}
		foreach (KeyValuePair<int[], Terrain> item in _E001)
		{
			int[] key2 = item.Key;
			Terrain value = null;
			Terrain value2 = null;
			Terrain value3 = null;
			Terrain value4 = null;
			_E001.TryGetValue(new int[2]
			{
				key2[0],
				key2[1] + 1
			}, out value);
			_E001.TryGetValue(new int[2]
			{
				key2[0] - 1,
				key2[1]
			}, out value2);
			_E001.TryGetValue(new int[2]
			{
				key2[0] + 1,
				key2[1]
			}, out value3);
			_E001.TryGetValue(new int[2]
			{
				key2[0],
				key2[1] - 1
			}, out value4);
			item.Value.SetNeighbors(value2, value, value3, value4);
			item.Value.Flush();
		}
	}
}
