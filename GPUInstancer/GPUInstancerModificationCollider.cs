using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerModificationCollider : MonoBehaviour
{
	public GPUInstancerPrefabManager prefabManager;

	private List<GPUInstancerPrefab> _E000;

	private Collider _E001;

	private void Awake()
	{
		_E000 = new List<GPUInstancerPrefab>();
		_E001 = GetComponent<Collider>();
		if (prefabManager == null)
		{
			prefabManager = Object.FindObjectOfType<GPUInstancerPrefabManager>();
		}
		if (prefabManager != null)
		{
			prefabManager.AddModificationCollider(this);
		}
		else
		{
			Debug.LogWarning(_ED3E._E000(118266));
		}
	}

	private void Update()
	{
		if (!(prefabManager != null) || !prefabManager.isActiveAndEnabled)
		{
			return;
		}
		for (int i = 0; i < _E000.Count; i++)
		{
			if (!IsInsideCollider(_E000[i]))
			{
				Rigidbody component = _E000[i].GetComponent<Rigidbody>();
				if (!(component != null) || component.IsSleeping())
				{
					_E4BD.EnableInstancingForInstance(prefabManager, _E000[i]);
					_E000.Remove(_E000[i]);
					i--;
				}
			}
			else if (_E000[i].state != PrefabInstancingState.Disabled)
			{
				prefabManager.DisableIntancingForInstance(_E000[i]);
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (prefabManager != null && prefabManager.isActiveAndEnabled && (bool)collider.gameObject)
		{
			GPUInstancerPrefab component = collider.gameObject.GetComponent<GPUInstancerPrefab>();
			if (component != null && component.prefabPrototype.enableRuntimeModifications && component.state != PrefabInstancingState.Disabled)
			{
				prefabManager.DisableIntancingForInstance(component);
				_E000.Add(component);
			}
		}
	}

	public bool IsInsideCollider(GPUInstancerPrefab prefabInstance)
	{
		Collider component = prefabInstance.GetComponent<Collider>();
		if (component == null)
		{
			return false;
		}
		return _E001.bounds.Intersects(component.bounds);
	}

	public void AddEnteredInstance(GPUInstancerPrefab prefabInstance)
	{
		_E000.Add(prefabInstance);
	}

	public int GetEnteredInstanceCount()
	{
		return _E000.Count;
	}
}
