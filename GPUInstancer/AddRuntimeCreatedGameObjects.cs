using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class AddRuntimeCreatedGameObjects : MonoBehaviour
{
	public GPUInstancerPrefabManager prefabManager;

	public Material material;

	private List<GameObject> m__E000;

	private GPUInstancerPrefabPrototype _E001;

	private GameObject _E002;

	private void Start()
	{
		_E002 = GameObject.CreatePrimitive(PrimitiveType.Quad);
		SetMaterial();
		this.m__E000 = new List<GameObject>();
		this.m__E000.Add(_E002);
		for (int i = 0; i < 1000; i++)
		{
			this.m__E000.Add(UnityEngine.Object.Instantiate(_E002, UnityEngine.Random.insideUnitSphere * 20f, Quaternion.identity));
		}
		_E001 = _E4BD.DefineGameObjectAsPrefabPrototypeAtRuntime(prefabManager, _E002);
		_E001.enableRuntimeModifications = true;
		_E001.autoUpdateTransformData = true;
		_E4BD.AddInstancesToPrefabPrototypeAtRuntime(prefabManager, _E001, this.m__E000);
		StartCoroutine(_E000());
	}

	private IEnumerator _E000()
	{
		while (true)
		{
			foreach (PrimitiveType value in Enum.GetValues(typeof(PrimitiveType)))
			{
				yield return new WaitForSeconds(3f);
				_E4BD.RemovePrototypeAtRuntime(prefabManager, _E001);
				ClearInstances();
				yield return new WaitForSeconds(1f);
				_E002 = GameObject.CreatePrimitive(value);
				SetMaterial();
				this.m__E000.Add(_E002);
				_E001 = _E4BD.DefineGameObjectAsPrefabPrototypeAtRuntime(prefabManager, _E002);
				for (int i = 0; i < 1000; i++)
				{
					this.m__E000.Add(UnityEngine.Object.Instantiate(_E002, UnityEngine.Random.insideUnitSphere * 20f, Quaternion.identity));
				}
				_E4BD.AddInstancesToPrefabPrototypeAtRuntime(prefabManager, _E001, this.m__E000);
				yield return new WaitForSeconds(1f);
			}
		}
	}

	public void ClearInstances()
	{
		foreach (GameObject item in this.m__E000)
		{
			UnityEngine.Object.Destroy(item);
		}
		this.m__E000.Clear();
	}

	public void SetMaterial()
	{
		_E002.GetComponent<MeshRenderer>().sharedMaterial = material;
	}
}
