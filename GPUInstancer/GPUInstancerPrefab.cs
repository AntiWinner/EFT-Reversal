using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerPrefab : MonoBehaviour
{
	[HideInInspector]
	public GPUInstancerPrefabPrototype prefabPrototype;

	[HideInInspector]
	public int gpuInstancerID;

	[HideInInspector]
	public PrefabInstancingState state;

	public Dictionary<string, object> variationDataList;

	private bool _E000;

	private Transform _E001;

	private bool _E002;

	private Matrix4x4 _E003;

	public void AddVariation<T>(string bufferName, T value)
	{
		if (variationDataList == null)
		{
			variationDataList = new Dictionary<string, object>();
		}
		if (variationDataList.ContainsKey(bufferName))
		{
			variationDataList[bufferName] = value;
		}
		else
		{
			variationDataList.Add(bufferName, value);
		}
	}

	public Transform GetInstanceTransform(bool forceNew = false)
	{
		if (!_E000 || forceNew)
		{
			_E001 = base.transform;
			_E001.hasChanged = false;
			_E000 = true;
		}
		return _E001;
	}

	public Matrix4x4 GetLocalToWorldMatrix(bool forceNew = false)
	{
		if (!_E002 || forceNew)
		{
			_E003 = GetInstanceTransform(forceNew).localToWorldMatrix;
			_E002 = true;
		}
		return _E003;
	}

	public virtual void SetupPrefabInstance(_E4C2 runtimeData, bool forceNew = false)
	{
	}
}
