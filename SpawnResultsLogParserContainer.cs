using System.Collections.Generic;
using UnityEngine;

public class SpawnResultsLogParserContainer : MonoBehaviour
{
	public List<SpawnResultDebug> _elements = new List<SpawnResultDebug>();

	public void Clear()
	{
		foreach (SpawnResultDebug element in _elements)
		{
			Object.DestroyImmediate(element.gameObject);
		}
		_elements.Clear();
	}

	public void AddData(_E626._E001 data)
	{
		SpawnResultDebug spawnResultDebug = new GameObject(data.ProfileId).AddComponent<SpawnResultDebug>();
		_elements.Add(spawnResultDebug);
		spawnResultDebug.Init(data);
		spawnResultDebug.transform.SetParent(base.transform);
	}
}
