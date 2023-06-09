using System;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SelectingItemView : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private const string _E119 = "EMPTY_SLOT";

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _details;

	[SerializeField]
	private Image _itemIcon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _foundInRaid;

	private _E3E2 _E11A;

	private Action<Item> _E03D;

	private Item _E11B;

	public void SetItem([CanBeNull] Item item, Action<Item> onSelected)
	{
		_E03D = onSelected;
		_E11B = item;
		if (item == null)
		{
			_name.text = _ED3E._E000(252245).Localized();
			_details.text = string.Empty;
			_foundInRaid.SetActive(value: false);
		}
		else
		{
			_name.text = item.ShortName.Localized();
			_E11A = ItemViewFactory.LoadItemIcon(item);
			_foundInRaid.SetActive(item.MarkedAsSpawnedInSession);
			UI.AddDisposable(_E11A.Changed.Bind(_E000));
		}
	}

	public void SetDetails(string details)
	{
		_details.text = details;
	}

	private void _E000()
	{
		_itemIcon.sprite = _E11A.Sprite;
		_itemIcon.gameObject.SetActive(_E11A.Sprite != null);
		_itemIcon.SetNativeSize();
		_E001(((RectTransform)base.transform).rect.size);
	}

	private void _E001(Vector2 size)
	{
		RectTransform rectTransform = (RectTransform)_itemIcon.transform;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float num = size.x / sizeDelta.x;
		float num2 = size.y / sizeDelta.y;
		if (!(num > 1f) || !(num2 > 1f))
		{
			float num3 = Mathf.Min(num, num2);
			rectTransform.localScale = new Vector3(num3, num3, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E03D?.Invoke(_E11B);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_background.color = Color.gray;
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_background.color = Color.white;
	}
}
