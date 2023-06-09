using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Interactive;

public class LootPointViewer : MonoBehaviour
{
	private LootPoint _E000;

	private List<GameObject> _E001 = new List<GameObject>();

	[CompilerGenerated]
	private Item _E002;

	private LootPoint _E003 => _E000 ?? (_E000 = GetComponent<LootPoint>());

	public Item CurrentItem
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public void ShowItem(Item item)
	{
		Clear();
		CurrentItem = item;
		List<Transform> list = new List<Transform>();
		if (_E003.IsGroupPosition)
		{
			foreach (GroupLootPoint groupPosition in _E003.GroupPositions)
			{
				list.Add(groupPosition.transform);
			}
		}
		else
		{
			list.Add(base.transform);
		}
		foreach (Transform item2 in list)
		{
			GameObject gameObject = _E2E4<_EC0A>.Instance.ObjectFactory.CreateItem(item, isAnimated: false);
			_E001.Add(gameObject);
			gameObject.SetActive(value: true);
			gameObject.hideFlags = HideFlags.DontSave;
			gameObject.transform.SetParent(item2, worldPositionStays: false);
			PreviewPivot component = gameObject.GetComponent<PreviewPivot>();
			Vector3 vector = ((component != null) ? component.SpawnPosition : Vector3.zero);
			gameObject.transform.localPosition = -vector;
			MonoBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren[i]);
			}
			Animator[] componentsInChildren2 = gameObject.GetComponentsInChildren<Animator>(includeInactive: true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				Object.DestroyImmediate(componentsInChildren2[i]);
			}
		}
	}

	public void Clear()
	{
		foreach (GameObject item in _E001)
		{
			Object.DestroyImmediate(item);
		}
		_E001.Clear();
		CurrentItem = null;
		List<Transform> list = new List<Transform>();
		if (_E003.IsGroupPosition)
		{
			foreach (GroupLootPoint groupPosition in _E003.GroupPositions)
			{
				list.Add(groupPosition.transform);
			}
		}
		else
		{
			list.Add(base.transform);
		}
		foreach (Transform item2 in list)
		{
			while (item2.childCount > 0)
			{
				Object.DestroyImmediate(item2.GetChild(0).gameObject);
			}
		}
	}
}
