using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerPrefabRuntimeHandler : MonoBehaviour
{
	[HideInInspector]
	public GPUInstancerPrefab gpuiPrefab;

	private GPUInstancerPrefabManager m__E000;

	private static Dictionary<GPUInstancerPrefabPrototype, GPUInstancerPrefabManager> m__E001;

	private void Awake()
	{
		gpuiPrefab = GetComponent<GPUInstancerPrefab>();
		if (GPUInstancerPrefabRuntimeHandler.m__E001 != null)
		{
			return;
		}
		GPUInstancerPrefabRuntimeHandler.m__E001 = new Dictionary<GPUInstancerPrefabPrototype, GPUInstancerPrefabManager>();
		GPUInstancerPrefabManager[] array = Object.FindObjectsOfType<GPUInstancerPrefabManager>();
		if (array == null || array.Length == 0)
		{
			return;
		}
		GPUInstancerPrefabManager[] array2 = array;
		foreach (GPUInstancerPrefabManager gPUInstancerPrefabManager in array2)
		{
			foreach (GPUInstancerPrefabPrototype prototype in gPUInstancerPrefabManager.prototypeList)
			{
				if (!GPUInstancerPrefabRuntimeHandler.m__E001.ContainsKey(prototype))
				{
					GPUInstancerPrefabRuntimeHandler.m__E001.Add(prototype, gPUInstancerPrefabManager);
				}
			}
		}
	}

	private void Start()
	{
		if (gpuiPrefab.state != 0)
		{
			return;
		}
		if (this.m__E000 == null)
		{
			this.m__E000 = _E000();
		}
		if (this.m__E000 != null)
		{
			if (!this.m__E000.isInitialized)
			{
				this.m__E000.InitializeRuntimeDataAndBuffers();
			}
			this.m__E000.AddPrefabInstance(gpuiPrefab, automaticallyIncreaseBufferSize: true);
		}
	}

	private void OnDisable()
	{
		if (gpuiPrefab.state == PrefabInstancingState.Instanced)
		{
			if (this.m__E000 == null)
			{
				this.m__E000 = _E000();
			}
			if (this.m__E000 != null)
			{
				this.m__E000.RemovePrefabInstance(gpuiPrefab, setRenderersEnabled: false);
			}
		}
	}

	private GPUInstancerPrefabManager _E000()
	{
		GPUInstancerPrefabManager value = null;
		if (GPUInstancerManager.activeManagerList != null)
		{
			if (!GPUInstancerPrefabRuntimeHandler.m__E001.TryGetValue(gpuiPrefab.prefabPrototype, out value))
			{
				value = (GPUInstancerPrefabManager)GPUInstancerManager.activeManagerList.Find((GPUInstancerManager manager) => manager.prototypeList.Contains(gpuiPrefab.prefabPrototype));
				if (value == null)
				{
					Debug.LogWarning(_ED3E._E000(117179) + gpuiPrefab.prefabPrototype);
					return null;
				}
				GPUInstancerPrefabRuntimeHandler.m__E001.Add(gpuiPrefab.prefabPrototype, value);
			}
			if (value == null)
			{
				value = (GPUInstancerPrefabManager)GPUInstancerManager.activeManagerList.Find((GPUInstancerManager manager) => manager.prototypeList.Contains(gpuiPrefab.prefabPrototype));
				if (value == null)
				{
					return null;
				}
				GPUInstancerPrefabRuntimeHandler.m__E001[gpuiPrefab.prefabPrototype] = value;
			}
		}
		return value;
	}

	[CompilerGenerated]
	private bool _E001(GPUInstancerManager manager)
	{
		return manager.prototypeList.Contains(gpuiPrefab.prefabPrototype);
	}

	[CompilerGenerated]
	private bool _E002(GPUInstancerManager manager)
	{
		return manager.prototypeList.Contains(gpuiPrefab.prefabPrototype);
	}
}
