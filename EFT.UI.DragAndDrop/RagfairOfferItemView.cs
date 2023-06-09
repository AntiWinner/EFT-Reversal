using Diz.Binding;
using EFT.InventoryLogic;
using EFT.UI.Ragfair;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class RagfairOfferItemView : StaticGridItemView
{
	private const float _E05D = 10f;

	private const int _E05E = 330;

	private const int _E05F = 125;

	[SerializeField]
	private LayoutElement _layoutElement;

	[SerializeField]
	private DurabilitySlider _durabilitySlider;

	private _EB10 _E060;

	private bool _E061;

	private Offer _E062;

	public override ItemRotation ItemRotation
	{
		get
		{
			return _itemRotation;
		}
		protected set
		{
			_itemRotation = value;
			if (_E061)
			{
				_itemRotation = value;
				MainImage.transform.rotation = ((value == ItemRotation.Horizontal) ? ItemViewFactory.HorizontalRotation : ItemViewFactory.VerticalRotation);
				if (!(base.RectTransform.anchorMin != base.RectTransform.anchorMax))
				{
					_E313 cellPixelSize = ItemViewFactory.GetCellPixelSize(base.Item.CalculateRotatedSize(_itemRotation));
					Vector2 sizeDelta = new Vector2(Mathf.Clamp(cellPixelSize.X, 0, 330), Mathf.Clamp(cellPixelSize.Y, 0, 125));
					_layoutElement.minWidth = sizeDelta.x;
					_layoutElement.minHeight = sizeDelta.y;
					base.RectTransform.sizeDelta = sizeDelta;
				}
			}
		}
	}

	protected override IBindable<float> Transparency => new _ECF5<float>(1f);

	private bool _E000
	{
		get
		{
			if (_E061)
			{
				return base.Item.GetItemComponent<RepairableComponent>() != null;
			}
			return false;
		}
	}

	public static GridItemView Create(Item item, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		RagfairOfferItemView ragfairOfferItemView = ItemViewFactory.CreateFromPool<RagfairOfferItemView>(_ED3E._E000(236337));
		ragfairOfferItemView.Show(null, item, rotation, expanded: false, itemController, itemOwner, itemUiContext, insurance);
		return ragfairOfferItemView;
	}

	public void Show(Offer offer, Item item, ItemRotation rotation, bool expanded, _EB1E itemController, IItemOwner itemOwner, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		_E062 = offer;
		_E061 = expanded;
		NewGridItemView(item, new _EB64(item, EItemViewType.Ragfair), rotation, itemController, itemOwner, null, null, itemUiContext, insurance).Init();
		foreach (_EAF6 item2 in ItemOwner.SelectEvents<_EAF6>(base.Item))
		{
			SetBeingExaminedState(item2);
		}
		if (!(_durabilitySlider == null) && item.TryGetItemComponent<RepairableComponent>(out var component))
		{
			_durabilitySlider._E000(component, 10f);
			_E060 = item.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability));
			_E060.OnUpdate -= _E000;
			_E060.OnUpdate += _E000;
		}
	}

	protected override void OnBeingExaminedChanged(bool isBeingExamined)
	{
		base.OnBeingExaminedChanged(isBeingExamined);
		if (!(_durabilitySlider == null))
		{
			bool active = this._E000 && !isBeingExamined;
			_durabilitySlider.gameObject.SetActive(active);
		}
	}

	protected override string GetErrorText()
	{
		return null;
	}

	private void _E000()
	{
		_durabilitySlider._E001(_E060.Base());
	}

	protected override void UpdateScale()
	{
		Vector2 sizeDelta = MainImage.rectTransform.sizeDelta;
		float x = sizeDelta.x;
		float y = sizeDelta.y;
		int num;
		int num2;
		if (_E061)
		{
			base.UpdateScale();
			num = 330;
			num2 = 125;
			if (x < (float)num && y < (float)num2)
			{
				return;
			}
		}
		else
		{
			num = 64;
			num2 = 64;
		}
		float num3 = Mathf.Min((float)num / x, (float)num2 / y);
		MainImage.rectTransform.sizeDelta = new Vector2(x * num3, y * num3);
	}

	protected override void SetItemValue(EItemValueFormat format, bool display, string color, object arg1, object arg2 = null, string color2 = null)
	{
		if (_E062 != null)
		{
			string text = _E062.TotalItemCount.ToString();
			if (_E062.BuyRestrictionMax <= 0 && _E062.UnlimitedCount)
			{
				text = _ED3E._E000(254994).Localized();
			}
			ItemValue.text = _ED3E._E000(235984) + text + _ED3E._E000(59467);
		}
	}

	public override void UpdateInfo()
	{
		base.UpdateInfo();
		Caption.gameObject.SetActive(value: false);
		ItemInscription.gameObject.SetActive(value: false);
		base.SecureIcon.gameObject.SetActive(value: false);
		base.LockedIcon.gameObject.SetActive(value: false);
		base.TogglableIcon.gameObject.SetActive(value: false);
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (doubleClick)
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect);
			}
			break;
		case PointerEventData.InputButton.Middle:
			ExecuteMiddleClick();
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}
}
