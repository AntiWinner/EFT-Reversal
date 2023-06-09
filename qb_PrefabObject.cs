using System;
using UnityEngine;

[Serializable]
public class qb_PrefabObject
{
	public UnityEngine.Object prefab;

	public float weight = 0.1f;

	public qb_PrefabObject(UnityEngine.Object prefab)
	{
		this.prefab = prefab;
	}

	public qb_PrefabObject(UnityEngine.Object prefab, float weight)
	{
		this.prefab = prefab;
		this.weight = weight;
	}
}
