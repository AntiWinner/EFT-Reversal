using System;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class KeyCardItem : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private Image _itemIcon;

	[SerializeField]
	private Image _background;

	private _E3E2 _E11A;

	private Action<Item> _E03D;

	private Item _E11B;

	public void SetItem([CanBeNull] Item item, Action<Item> onSelected)
	{
		_E03D = onSelected;
		_E11B = item;
		if (item == null)
		{
			_name.text = _ED3E._E000(249328);
			return;
		}
		_name.text = item.ShortName.Localized();
		_E11A = ItemViewFactory.LoadItemIcon(item);
		UI.AddDisposable(_E11A.Changed.Bind(_E000));
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
