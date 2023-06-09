using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class CaptchaGridItemView : BaseSelectableItemView
{
	protected override bool Examined => true;

	public static CaptchaGridItemView Create(Item item, _EB66 sourceItemContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, ItemUiContext itemUiContext)
	{
		CaptchaGridItemView captchaGridItemView = ItemViewFactory.CreateFromPool<CaptchaGridItemView>(_ED3E._E000(235629))._E000(item, sourceItemContext, rotation, itemController, itemOwner, itemUiContext);
		captchaGridItemView.Init();
		return captchaGridItemView;
	}

	private CaptchaGridItemView _E000(Item item, _EB66 sourceItemContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, ItemUiContext itemUiContext)
	{
		NewBaseSelectableItemView(item, sourceItemContext, rotation, itemController, itemOwner, null, null, itemUiContext, null);
		base.RectTransform.anchorMin = Vector2.zero;
		base.RectTransform.anchorMax = Vector2.zero;
		return this;
	}

	public override void UpdateInfo()
	{
		base.UpdateInfo();
		Caption.gameObject.SetActive(value: false);
		ItemInscription.gameObject.SetActive(value: false);
		if (InsuredIcon != null)
		{
			InsuredIcon.gameObject.SetActive(value: false);
		}
		if (InsuredItemBorder != null)
		{
			InsuredItemBorder.gameObject.SetActive(value: false);
		}
		if (RepairBuffIcon != null)
		{
			RepairBuffIcon.gameObject.SetActive(value: false);
		}
		base.SecureIcon.gameObject.SetActive(value: false);
		base.LockedIcon.gameObject.SetActive(value: false);
		base.TogglableIcon.gameObject.SetActive(value: false);
		ItemValue.gameObject.SetActive(value: false);
	}

	protected override void ShowTooltip()
	{
	}

	protected override void HideTooltip()
	{
	}

	protected override void SetAvailability(bool available, string tooltip)
	{
	}
}
