using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerDrawCallColorDebugger : MonoBehaviour
{
	public GPUInstancerManager gPUIManager;

	public List<Color> drawCallColors = new List<Color>
	{
		Color.red,
		Color.blue,
		Color.yellow,
		Color.green,
		Color.magenta
	};

	public bool removeMainTextureWhenColored;

	private Dictionary<Material, Color> m__E000;

	private Dictionary<Material, Texture> m__E001;

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
		ChangeDrawCallColors();
	}

	public void ChangeDrawCallColors()
	{
		this.m__E000 = new Dictionary<Material, Color>();
		if (removeMainTextureWhenColored)
		{
			this.m__E001 = new Dictionary<Material, Texture>();
		}
		else
		{
			this.m__E001 = null;
		}
		int num = 0;
		foreach (_E4C2 runtimeData in gPUIManager.runtimeDataList)
		{
			for (int i = 0; i < runtimeData.instanceLODs.Count; i++)
			{
				for (int j = 0; j < runtimeData.instanceLODs[i].renderers.Count; j++)
				{
					for (int k = 0; k < runtimeData.instanceLODs[i].renderers[j].materials.Count; k++)
					{
						if (runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(36528)))
						{
							this.m__E000.Add(runtimeData.instanceLODs[i].renderers[j].materials[k], runtimeData.instanceLODs[i].renderers[j].materials[k].color);
							runtimeData.instanceLODs[i].renderers[j].materials[k].color = ((num < drawCallColors.Count) ? drawCallColors[num] : Random.ColorHSV());
						}
						if (this.m__E001 != null && runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(19728)))
						{
							this.m__E001.Add(runtimeData.instanceLODs[i].renderers[j].materials[k], runtimeData.instanceLODs[i].renderers[j].materials[k].mainTexture);
							runtimeData.instanceLODs[i].renderers[j].materials[k].mainTexture = null;
						}
						num++;
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
			for (int i = 0; i < runtimeData.instanceLODs.Count; i++)
			{
				for (int j = 0; j < runtimeData.instanceLODs[i].renderers.Count; j++)
				{
					for (int k = 0; k < runtimeData.instanceLODs[i].renderers[j].materials.Count; k++)
					{
						if (runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(36528)) && this.m__E000.ContainsKey(runtimeData.instanceLODs[i].renderers[j].materials[k]))
						{
							runtimeData.instanceLODs[i].renderers[j].materials[k].color = this.m__E000[runtimeData.instanceLODs[i].renderers[j].materials[k]];
						}
						if (this.m__E001 != null && runtimeData.instanceLODs[i].renderers[j].materials[k].HasProperty(_ED3E._E000(19728)) && this.m__E001.ContainsKey(runtimeData.instanceLODs[i].renderers[j].materials[k]))
						{
							runtimeData.instanceLODs[i].renderers[j].materials[k].mainTexture = this.m__E001[runtimeData.instanceLODs[i].renderers[j].materials[k]];
						}
					}
				}
			}
		}
	}
}
