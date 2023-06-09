using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerFloatingOriginHandler : MonoBehaviour
{
	public List<GPUInstancerManager> gPUIManagers;

	private Vector3 _E000;

	private void OnEnable()
	{
		_E000 = base.transform.position;
		base.transform.hasChanged = false;
	}

	private void Update()
	{
		if (!base.transform.hasChanged || !(base.transform.position != _E000))
		{
			return;
		}
		foreach (GPUInstancerManager gPUIManager in gPUIManagers)
		{
			if (gPUIManager != null)
			{
				_E4BD.SetGlobalPositionOffset(gPUIManager, base.transform.position - _E000);
			}
		}
		_E000 = base.transform.position;
		base.transform.hasChanged = false;
	}
}
