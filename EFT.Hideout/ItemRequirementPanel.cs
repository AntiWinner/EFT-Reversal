using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ItemRequirementPanel : UIElement, _E83B, IUIView, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemRequirement itemRequirement;

		public ItemRequirementPanel _003C_003E4__this;

		public bool ignoreFulfillment;

		internal void _E000()
		{
			int intCount = itemRequirement.IntCount;
			_003C_003E4__this._itemIconViewFactory.ShowInfo(showCounter: true, !ignoreFulfillment);
			_003C_003E4__this._itemIconViewFactory.FulfilledStatus = itemRequirement.Fulfilled;
			if (!ignoreFulfillment)
			{
				_003C_003E4__this._itemIconViewFactory.SetError(itemRequirement.Error);
				_003C_003E4__this._itemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(165809), Mathf.Clamp(itemRequirement.UserItemsCount, 0, intCount).FormatSeparate(_ED3E._E000(18502)), intCount.FormatSeparate(_ED3E._E000(18502)), _ED3E._E000(165836)));
			}
			else
			{
				_003C_003E4__this._itemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(165809), intCount.FormatSeparate(_ED3E._E000(18502)), intCount.FormatSeparate(_ED3E._E000(18502)), _ED3E._E000(165836)));
			}
		}
	}

	[SerializeField]
	private HideoutItemViewFactory _itemIconViewFactory;

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, Requirement requirement, EAreaType areaType, bool ignoreFulfillment)
	{
		ItemRequirement itemRequirement = (ItemRequirement)requirement;
		ShowGameObject();
		HideoutItemViewFactory itemIconViewFactory = _itemIconViewFactory;
		_E63B instance = Singleton<_E63B>.Instance;
		string templateId = itemRequirement.TemplateId;
		int num = (int)areaType;
		itemIconViewFactory.Show(instance.GetHideoutSchemeItem(templateId, num.ToString(_ED3E._E000(27314)), EOwnerType.HideoutUpgrade), inventoryController, itemUiContext);
		UI.AddDisposable(_itemIconViewFactory);
		UI.AddDisposable(requirement.OnFulfillmentChange.Bind(delegate
		{
			int intCount = itemRequirement.IntCount;
			_itemIconViewFactory.ShowInfo(showCounter: true, !ignoreFulfillment);
			_itemIconViewFactory.FulfilledStatus = itemRequirement.Fulfilled;
			if (!ignoreFulfillment)
			{
				_itemIconViewFactory.SetError(itemRequirement.Error);
				_itemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(165809), Mathf.Clamp(itemRequirement.UserItemsCount, 0, intCount).FormatSeparate(_ED3E._E000(18502)), intCount.FormatSeparate(_ED3E._E000(18502)), _ED3E._E000(165836)));
			}
			else
			{
				_itemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(165809), intCount.FormatSeparate(_ED3E._E000(18502)), intCount.FormatSeparate(_ED3E._E000(18502)), _ED3E._E000(165836)));
			}
		}));
	}
}
