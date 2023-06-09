using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerLODColorDebugger : MonoBehaviour
{
	public GPUInstancerManager gPUIManager;

	public List<Color> lODColors = new List<Color>
	{
		Color.red,
		Color.blue,
		Color.yellow
	};

	private Dictionary<Material, Color> m__E000;

	private void OnEnable()
	{
		if (gPUIManager != null)
		{
			StartCoroutine(_E000());
		}
	}

	private void OnDisable()
	{
		if (gPUIManager != null)
		{
			_E001();
		}
	}

	private void Reset()
	{
		if (GetComponent<GPUInstancerManager>() != null)
		{
			gPUIManager = GetComponent<GPUInstancerManager>();
		}
	}

	private IEnumerator _E000()
	{
		while (!gPUIManager.isInitialized)
		{
			yield return null;
		}
		ChangeLODColors();
	}

	public void ChangeLODColors()
	{
		this.m__E000 = new Dictionary<Material, Color>();
		foreach (_E4C2 runtimeData in gPUIManager.runtimeDataList)
		{
			for (int i = 1; i < runtimeData.instanceLODs.Count && i <= lODColors.Count; i++)
			{
				for (int j = 0; j < runtimeData.instanceLODs[i].renderers.Count; j++)
				{
					for (int k = 0; k < runtimeData.instanceLODs[i].renderers[j].materials.Count; k++)
					{
						if (runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(36528)))
						{
							this.m__E000.Add(runtimeData.instanceLODs[i].renderers[j].materials[k], runtimeData.instanceLODs[i].renderers[j].materials[k].color);
							runtimeData.instanceLODs[i].renderers[j].materials[k].color = lODColors[i - 1];
						}
					}
				}
			}
		}
	}

	private void _E001()
	{
		if (this.m__E000 == null)
		{
			return;
		}
		foreach (_E4C2 runtimeData in gPUIManager.runtimeDataList)
		{
			for (int i = 1; i < runtimeData.instanceLODs.Count && i <= lODColors.Count; i++)
			{
				for (int j = 0; j < runtimeData.instanceLODs[i].renderers.Count; j++)
				{
					for (int k = 0; k < runtimeData.instanceLODs[i].renderers[j].materials.Count; k++)
					{
						if (runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(36528)) && this.m__E000.ContainsKey(runtimeData.instanceLODs[i].renderers[j].materials[k]))
						{
							runtimeData.instanceLODs[i].renderers[j].materials[k].color = this.m__E000[runtimeData.instanceLODs[i].renderers[j].materials[k]];
						}
					}
				}
			}
		}
	}
}
