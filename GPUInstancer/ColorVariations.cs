using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class ColorVariations : MonoBehaviour
{
	public GPUInstancerPrefab prefab;

	public GPUInstancerPrefabManager prefabManager;

	public int instances = 1000;

	private string _E000 = _ED3E._E000(40500);

	private List<GPUInstancerPrefab> _E001;

	private void Start()
	{
		_E001 = new List<GPUInstancerPrefab>();
		if (prefabManager != null && prefabManager.isActiveAndEnabled)
		{
			_E4BD.DefinePrototypeVariationBuffer<Vector4>(prefabManager, prefab.prefabPrototype, _E000);
		}
		for (int i = 0; i < instances; i++)
		{
			GPUInstancerPrefab gPUInstancerPrefab = Object.Instantiate(prefab);
			gPUInstancerPrefab.transform.localPosition = Random.insideUnitSphere * 20f;
			gPUInstancerPrefab.transform.SetParent(base.transform);
			_E001.Add(gPUInstancerPrefab);
			gPUInstancerPrefab.AddVariation(_E000, (Vector4)Random.ColorHSV());
		}
		if (prefabManager != null && prefabManager.isActiveAndEnabled)
		{
			_E4BD.RegisterPrefabInstanceList(prefabManager, _E001);
			_E4BD.InitializeGPUInstancer(prefabManager);
		}
	}

	private void Update()
	{
		_E4BD.UpdateVariation(prefabManager, _E001[Random.Range(0, _E001.Count)], _E000, (Vector4)Random.ColorHSV());
	}
}
