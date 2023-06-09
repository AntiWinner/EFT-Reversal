using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class DropdownItemSelector : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemView itemView;

		internal void _E000()
		{
			itemView.Kill();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Item item;

		internal bool _E000(ItemView view)
		{
			return view.Item == item;
		}
	}

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private GameObject _containerForFirstMap;

	[SerializeField]
	private Toggle _toggle;

	[SerializeField]
	private ScrollRect _scrollRect;

	[SerializeField]
	private Scrollbar _scrollbar;

	private readonly List<ItemView> _E19E = new List<ItemView>();

	private ItemView _E19F;

	private void Awake()
	{
		_toggle.onValueChanged.AddListener(_E001);
	}

	public void Init(IEnumerable<Item> items, _EAE6 itemController, _EB66 itemContext)
	{
		_E004();
		foreach (Item item2 in items)
		{
			ItemView item = _E000(itemContext, item2, (_EB1E)itemController, _container.transform);
			_E19E.Add(item);
		}
		if (_E19E.Count > 0)
		{
			SelectItem(_E19E[0].Item);
		}
		_E003();
	}

	private ItemView _E000(_EB66 sourceContext, Item item, _EB1E itemController, Transform container)
	{
		ItemView itemView = ItemUiContext.Instance.CreateItemView(item, sourceContext, ItemRotation.Horizontal, itemController, null, null, null, slotView: false, canSelect: true, searched: true);
		Transform obj = itemView.transform;
		obj.localPosition = Vector3.zero;
		obj.rotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		obj.SetParent(container, worldPositionStays: false);
		UI.AddDisposable(delegate
		{
			itemView.Kill();
		});
		return itemView;
	}

	public void SelectItem(Item item)
	{
		_toggle.isOn = false;
		if (_E19F != null)
		{
			_E19F.transform.SetParent(_container.transform);
		}
		_E19F = _E19E.FirstOrDefault((ItemView view) => view.Item == item);
		if (_E19F == null)
		{
			Debug.LogError(_ED3E._E000(258819));
			return;
		}
		_E19F.transform.SetParent(_containerForFirstMap.transform);
		RectTransform component = _E19F.GetComponent<RectTransform>();
		component.pivot = Vector2.up;
		component.localPosition = Vector3.zero;
		component.anchoredPosition = Vector2.zero;
		_E19F.transform.SetAsFirstSibling();
		_E19F.gameObject.SetActive(value: true);
	}

	private void _E001(bool isOn)
	{
		if (isOn)
		{
			_E002();
		}
		else
		{
			_E003();
		}
	}

	private void _E002()
	{
		_scrollbar.gameObject.SetActive(value: true);
		_scrollRect.gameObject.SetActive(value: true);
		foreach (ItemView item in _E19E)
		{
			item.gameObject.SetActive(value: true);
		}
	}

	private void _E003()
	{
		_scrollbar.gameObject.SetActive(value: false);
		_scrollRect.gameObject.SetActive(value: false);
		foreach (ItemView item in _E19E)
		{
			if (item != _E19F)
			{
				item.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_E19E.Count > 1)
		{
			_toggle.gameObject.SetActive(value: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_toggle.gameObject.SetActive(value: false);
	}

	private void _E004()
	{
		foreach (ItemView item in _E19E)
		{
			item.Kill();
		}
		_E19E.Clear();
	}

	public override void Close()
	{
		_E004();
		base.Close();
	}
}
