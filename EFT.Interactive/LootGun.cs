using System.Collections.Generic;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Interactive;

public class LootGun : MonoBehaviour
{
	[SerializeField]
	private float _castDistance = 50f;

	[SerializeField]
	private Vector3 _localStartPoint = new Vector3(0f, 0.1f, 0.5f);

	[SerializeField]
	private float _boundsMult = 1f;

	[SerializeField]
	private float _force = 2f;

	[SerializeField]
	private Vector2 _forceRange = new Vector2(0.1f, 50f);

	[SerializeField]
	private float _forceStep = 5f;

	private Item m__E000;

	private GameObject m__E001;

	private List<LootItem> m__E002 = new List<LootItem>();

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			_E000();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			_E002();
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			_E003();
		}
		else if (!Mathf.Approximately(Input.GetAxis(_ED3E._E000(205973)), 0f))
		{
			_force += _forceStep * Input.GetAxis(_ED3E._E000(205973));
			_force = Mathf.Clamp(_force, _forceRange.x, _forceRange.y);
		}
	}

	private void _E000()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, out var hitInfo, _castDistance, _E37B.LootLayerMask, QueryTriggerInteraction.Collide))
		{
			LootItem component = hitInfo.collider.gameObject.GetComponent<LootItem>();
			_E001(component.Item);
		}
	}

	private void _E001(Item item)
	{
		if (this.m__E000 == item)
		{
			return;
		}
		this.m__E000 = item;
		if (this.m__E000 != item)
		{
			this.m__E000 = item;
			if (this.m__E001 != null)
			{
				this.m__E001 = Singleton<_E760>.Instance.CreateLootPrefab(this.m__E000);
			}
			this.m__E001.SetActive(value: true);
			this.m__E001.transform.parent = base.transform;
			this.m__E001.transform.localRotation = Quaternion.identity;
			Collider[] componentsInChildren = this.m__E001.GetComponentsInChildren<Collider>(includeInactive: true);
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			Bounds totalBounds = componentsInChildren.GetTotalBounds();
			array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			this.m__E001.transform.localPosition = _localStartPoint + new Vector3(0f, totalBounds.extents.y, 0f) * _boundsMult;
		}
	}

	private void _E002()
	{
		if (this.m__E000 != null)
		{
			Vector3 angularVelocity = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 2f * Mathf.Sign(Random.Range(-1, 2)));
			LootItem item = Singleton<GameWorld>.Instance.ThrowItem(this.m__E000, null, base.transform.TransformPoint(_localStartPoint), Quaternion.identity, base.transform.forward * _force, angularVelocity, syncable: false);
			this.m__E002.Add(item);
		}
	}

	private void _E003()
	{
		foreach (LootItem item in this.m__E002)
		{
			Singleton<GameWorld>.Instance.DestroyLoot(item);
		}
		this.m__E002.Clear();
	}
}
