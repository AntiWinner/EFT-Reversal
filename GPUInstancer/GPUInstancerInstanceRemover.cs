using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerInstanceRemover : MonoBehaviour
{
	public bool useBounds;

	public List<Collider> selectedColliders;

	public bool removeFromDetailManagers = true;

	public bool removeFromTreeManagers = true;

	public bool removeFromPrefabManagers = true;

	public bool removeAtUpdate;

	public float offset;

	public bool onlyRemoveSelectedPrototypes;

	public List<GPUInstancerPrototype> selectedPrototypes;

	private void Reset()
	{
		selectedColliders = new List<Collider>(GetComponentsInChildren<Collider>());
	}

	private void Start()
	{
		foreach (GPUInstancerManager activeManager in GPUInstancerManager.activeManagerList)
		{
			if ((!removeFromDetailManagers && activeManager is GPUInstancerDetailManager) || (!removeFromTreeManagers && activeManager is GPUInstancerTreeManager) || (!removeFromPrefabManagers && activeManager is GPUInstancerPrefabManager))
			{
				continue;
			}
			foreach (Collider selectedCollider in selectedColliders)
			{
				if (useBounds)
				{
					_E4BD.RemoveInstancesInsideBounds(activeManager, selectedCollider.bounds, offset, onlyRemoveSelectedPrototypes ? selectedPrototypes : null);
				}
				else
				{
					_E4BD.RemoveInstancesInsideCollider(activeManager, selectedCollider, offset, onlyRemoveSelectedPrototypes ? selectedPrototypes : null);
				}
			}
		}
	}

	private void Update()
	{
		if (removeAtUpdate)
		{
			Start();
		}
	}
}
