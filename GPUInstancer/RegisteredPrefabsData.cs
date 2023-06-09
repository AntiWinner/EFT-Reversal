using System;
using System.Collections.Generic;

namespace GPUInstancer;

[Serializable]
public class RegisteredPrefabsData
{
	public GPUInstancerPrefabPrototype prefabPrototype;

	public List<GPUInstancerPrefab> registeredPrefabs;

	public RegisteredPrefabsData(GPUInstancerPrefabPrototype prefabPrototype)
	{
		this.prefabPrototype = prefabPrototype;
		registeredPrefabs = new List<GPUInstancerPrefab>();
	}
}
