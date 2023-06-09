using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class HideoutProductionRequirementView : UIElement
{
	[SerializeField]
	private HideoutItemViewFactory _itemViewFactory;

	[SerializeField]
	private Image _toolMarker;

	[SerializeField]
	private Color _fullFilledColor = Color.HSVToRGB(0.542f, 0.96f, 66f);

	[SerializeField]
	private Color _notFilledColor = Color.HSVToRGB(0f, 0.96f, 66f);

	[CompilerGenerated]
	private bool _E05F;

	public bool IsFulfilled
	{
		[CompilerGenerated]
		get
		{
			return _E05F;
		}
		[CompilerGenerated]
		private set
		{
			_E05F = value;
		}
	}

	private Color _E000
	{
		get
		{
			if (!IsFulfilled)
			{
				return _notFilledColor;
			}
			return _fullFilledColor;
		}
	}

	private void Awake()
	{
		if (_toolMarker != null)
		{
			_toolMarker.gameObject.SetActive(value: false);
		}
	}

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, ItemRequirement requirement, _E828 scheme, IEnumerable<Item> allItems, bool isProducing)
	{
		ShowGameObject();
		Item hideoutSchemeItem = Singleton<_E63B>.Instance.GetHideoutSchemeItem(requirement.TemplateId, scheme._id, scheme.SchemeOwnerType);
		if (hideoutSchemeItem is _EA85)
		{
			hideoutSchemeItem.GetItemComponentsInChildren<RecodableComponent>().First()?.SetEncoded(requirement.IsEncoded);
		}
		_itemViewFactory.Show(hideoutSchemeItem, inventoryController, itemUiContext);
		UI.AddDisposable(_itemViewFactory);
		requirement.Test(allItems);
		int num = (isProducing ? requirement.IntCount : Mathf.Min(requirement.UserItemsCount, requirement.IntCount));
		IsFulfilled = num >= requirement.IntCount && requirement.Error == null;
		_itemViewFactory.FulfilledStatus = IsFulfilled;
		_itemViewFactory.ShowInfo(!isProducing, !isProducing);
		_E000(requirement is ToolRequirement);
		if (!isProducing)
		{
			_itemViewFactory.SetError(requirement.Error);
		}
		_itemViewFactory.SetCounterText(string.Format(_ED3E._E000(165809), num.ToString(_ED3E._E000(164283)), requirement.IntCount.ToString(_ED3E._E000(164283)), _ED3E._E000(165836)));
	}

	private void _E000(bool isTool)
	{
		if (!(_toolMarker == null))
		{
			_toolMarker.gameObject.SetActive(isTool);
			_toolMarker.color = this._E000;
			if (isTool)
			{
				HideoutItemViewFactory itemViewFactory = _itemViewFactory;
				Color color = this._E000;
				itemViewFactory.SetBorderColor(in color);
			}
		}
	}
}
