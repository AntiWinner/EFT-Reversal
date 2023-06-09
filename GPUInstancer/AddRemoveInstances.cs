using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class AddRemoveInstances : MonoBehaviour
{
	public GPUInstancerPrefab prefab;

	public GPUInstancerPrefabManager prefabManager;

	private Transform m__E000;

	private int m__E001;

	private List<GPUInstancerPrefab> m__E002 = new List<GPUInstancerPrefab>();

	private List<GPUInstancerPrefab> m__E003 = new List<GPUInstancerPrefab>();

	private Toggle m__E004;

	private Canvas m__E005;

	private void Awake()
	{
		this.m__E000 = GameObject.Find(_ED3E._E000(74244)).transform;
		this.m__E001 = this.m__E000.childCount;
		this.m__E005 = Object.FindObjectOfType<Canvas>();
		this.m__E004 = GameObject.Find(_ED3E._E000(74298)).GetComponent<Toggle>();
	}

	private void Start()
	{
		for (int i = 0; i < this.m__E001; i++)
		{
			this.m__E002.Add(this.m__E000.GetChild(i).gameObject.GetComponent<GPUInstancerPrefab>());
		}
	}

	public void AddInstances()
	{
		_E004();
		StartCoroutine(_E000());
	}

	public void RemoveInstances()
	{
		_E004();
		StartCoroutine(_E001());
	}

	public void AddExtraInstances()
	{
		_E004();
		StartCoroutine(_E002());
	}

	public void RemoveExtraInstances()
	{
		_E004();
		StartCoroutine(_E003());
	}

	private IEnumerator _E000()
	{
		for (int i = 0; i < this.m__E001; i++)
		{
			GPUInstancerPrefab gPUInstancerPrefab = Object.Instantiate(prefab);
			gPUInstancerPrefab.transform.localPosition = Random.insideUnitSphere * 10f;
			gPUInstancerPrefab.transform.SetParent(this.m__E000);
			if (!gPUInstancerPrefab.prefabPrototype.addRuntimeHandlerScript)
			{
				_E4BD.AddPrefabInstance(prefabManager, gPUInstancerPrefab);
			}
			this.m__E002.Add(gPUInstancerPrefab);
			if (!this.m__E004.isOn)
			{
				yield return new WaitForSeconds(0.001f);
			}
		}
		_E005(_ED3E._E000(74275));
		if (this.m__E003.Count == 0)
		{
			_E005(_ED3E._E000(74313));
		}
		else
		{
			_E005(_ED3E._E000(74353));
		}
	}

	private IEnumerator _E001()
	{
		for (int i = 0; i < this.m__E001; i++)
		{
			if (!this.m__E002[this.m__E002.Count - 1].prefabPrototype.addRuntimeHandlerScript)
			{
				_E4BD.RemovePrefabInstance(prefabManager, this.m__E002[this.m__E002.Count - 1]);
			}
			Object.Destroy(this.m__E002[this.m__E002.Count - 1].gameObject);
			this.m__E002.RemoveAt(this.m__E002.Count - 1);
			if (!this.m__E004.isOn)
			{
				yield return new WaitForSeconds(0.001f);
			}
		}
		_E005(_ED3E._E000(74388));
		if (this.m__E003.Count == 0)
		{
			_E005(_ED3E._E000(74313));
		}
		else
		{
			_E005(_ED3E._E000(74353));
		}
	}

	private IEnumerator _E002()
	{
		for (int i = 0; i < prefab.prefabPrototype.extraBufferSize; i++)
		{
			GPUInstancerPrefab gPUInstancerPrefab = Object.Instantiate(prefab);
			gPUInstancerPrefab.transform.localPosition = Random.insideUnitSphere * 5f;
			gPUInstancerPrefab.transform.localPosition = gPUInstancerPrefab.transform.localPosition.normalized * (gPUInstancerPrefab.transform.localPosition.magnitude + 10f);
			gPUInstancerPrefab.transform.SetParent(this.m__E000);
			if (!gPUInstancerPrefab.prefabPrototype.addRuntimeHandlerScript)
			{
				_E4BD.AddPrefabInstance(prefabManager, gPUInstancerPrefab);
			}
			this.m__E003.Add(gPUInstancerPrefab);
			if (!this.m__E004.isOn)
			{
				yield return new WaitForSeconds(0.001f);
			}
		}
		_E005(_ED3E._E000(74353));
		if (this.m__E002.Count == 0)
		{
			_E005(_ED3E._E000(74388));
		}
		else
		{
			_E005(_ED3E._E000(74275));
		}
	}

	private IEnumerator _E003()
	{
		for (int i = 0; i < prefab.prefabPrototype.extraBufferSize; i++)
		{
			if (!this.m__E003[this.m__E003.Count - 1].prefabPrototype.addRuntimeHandlerScript)
			{
				_E4BD.RemovePrefabInstance(prefabManager, this.m__E003[this.m__E003.Count - 1]);
			}
			Object.Destroy(this.m__E003[this.m__E003.Count - 1].gameObject);
			this.m__E003.RemoveAt(this.m__E003.Count - 1);
			if (!this.m__E004.isOn)
			{
				yield return new WaitForSeconds(0.001f);
			}
		}
		_E005(_ED3E._E000(74313));
		if (this.m__E002.Count == 0)
		{
			_E005(_ED3E._E000(74388));
		}
		else
		{
			_E005(_ED3E._E000(74275));
		}
	}

	private void _E004()
	{
		Selectable[] componentsInChildren = this.m__E005.transform.GetComponentsInChildren<Selectable>();
		foreach (Selectable selectable in componentsInChildren)
		{
			if (selectable != this.m__E004)
			{
				selectable.interactable = false;
			}
		}
	}

	private void _E005(string buttonName)
	{
		this.m__E005.transform.GetChild(0).Find(buttonName).GetComponent<Selectable>()
			.interactable = true;
	}
}
