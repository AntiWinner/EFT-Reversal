using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerPrefabListRuntimeHandler : MonoBehaviour
{
	public GPUInstancerPrefabManager prefabManager;

	private IEnumerable<GPUInstancerPrefab> _E000;

	private bool _E001;

	public bool runInThreads = true;

	private void OnEnable()
	{
		if (prefabManager == null)
		{
			return;
		}
		if (!prefabManager.prototypeList.All((GPUInstancerPrototype p) => ((GPUInstancerPrefabPrototype)p).meshRenderersDisabled))
		{
			Debug.LogWarning(_ED3E._E000(118310));
			runInThreads = false;
		}
		_E000 = base.gameObject.GetComponentsInChildren<GPUInstancerPrefab>(includeInactive: true);
		if (_E000 == null || _E000.Count() <= 0)
		{
			return;
		}
		_E001 = true;
		if (runInThreads)
		{
			foreach (GPUInstancerPrefab item in _E000)
			{
				item.GetLocalToWorldMatrix(forceNew: true);
			}
			Thread thread = new Thread(AddPrefabInstancesAsync);
			thread.IsBackground = true;
			prefabManager.threadStartQueue.Enqueue(new GPUInstancerManager._E000
			{
				thread = thread,
				parameter = _E000
			});
		}
		else
		{
			AddPrefabInstancesAsync(_E000);
		}
	}

	private void OnDisable()
	{
		_E001 = false;
		if (prefabManager == null)
		{
			return;
		}
		if (_E000 != null && _E000.Count() > 0)
		{
			if (runInThreads)
			{
				Thread thread = new Thread(RemovePrefabInstancesAsync);
				thread.IsBackground = true;
				prefabManager.threadStartQueue.Enqueue(new GPUInstancerManager._E000
				{
					thread = thread,
					parameter = _E000
				});
			}
			else
			{
				RemovePrefabInstancesAsync(_E000);
			}
		}
		_E000 = null;
	}

	public void AddPrefabInstancesAsync(object param)
	{
		try
		{
			prefabManager.AddPrefabInstances((IEnumerable<GPUInstancerPrefab>)param, runInThreads);
		}
		catch (Exception ex)
		{
			if (runInThreads)
			{
				prefabManager.threadException = ex;
				prefabManager.threadQueue.Enqueue(prefabManager.LogThreadException);
			}
			else
			{
				Debug.LogException(ex);
			}
		}
	}

	public void RemovePrefabInstancesAsync(object param)
	{
		try
		{
			prefabManager.RemovePrefabInstances((IEnumerable<GPUInstancerPrefab>)param, runInThreads);
		}
		catch (Exception ex)
		{
			if (runInThreads)
			{
				prefabManager.threadException = ex;
				prefabManager.threadQueue.Enqueue(prefabManager.LogThreadException);
			}
			else
			{
				Debug.LogException(ex);
			}
		}
	}

	public void SetManager(GPUInstancerPrefabManager prefabManager)
	{
		if (_E001)
		{
			OnDisable();
		}
		this.prefabManager = prefabManager;
		if (base.isActiveAndEnabled)
		{
			OnEnable();
		}
	}
}
