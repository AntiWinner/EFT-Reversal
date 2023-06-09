using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class TraderRequirementPanel : UIElement, _E83B, IUIView, IDisposable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TraderRequirementPanel _003C_003E4__this;

		public bool ignoreFulfillment;

		internal void _E000()
		{
			_003C_003E4__this._fulfilledStatus.Show(_003C_003E4__this._E04B.Fulfilled);
			_003C_003E4__this._fulfilledStatus.gameObject.SetActive(!ignoreFulfillment);
		}
	}

	private const string _E049 = "Required <b>{0}</b>, level <b>{1}</b>";

	private const string _E04A = "Required <b>{0}</b> to be unlocked";

	[SerializeField]
	private TraderRequirementIcon _traderRequirementIcon;

	[SerializeField]
	private RequirementFulfilledStatus _fulfilledStatus;

	private _E83C _E04B;

	private SimpleTooltip _E02A;

	private void Awake()
	{
		HoverTrigger orAddComponent = _traderRequirementIcon.GetOrAddComponent<HoverTrigger>();
		_E02A = ItemUiContext.Instance.Tooltip;
		orAddComponent.OnHoverStart += delegate
		{
			string arg = _E04B.Trader.Settings.Nickname.Localized();
			string text = string.Empty;
			_E83C obj = _E04B;
			if (obj != null)
			{
				if (!(obj is TraderLoyaltyRequirement traderLoyaltyRequirement))
				{
					if (obj is TraderUnlockRequirement)
					{
						text = string.Format(_ED3E._E000(163939).Localized(), arg);
					}
				}
				else
				{
					TraderLoyaltyRequirement traderLoyaltyRequirement2 = traderLoyaltyRequirement;
					text = string.Format(_ED3E._E000(163909).Localized(), arg, traderLoyaltyRequirement2.LoyaltyLevel);
				}
			}
			_E02A.Show(text);
		};
		orAddComponent.OnHoverEnd += delegate
		{
			_E02A.Close();
		};
	}

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, Requirement requirement, EAreaType areaType, bool ignoreFulfillment)
	{
		_E04B = (_E83C)requirement;
		if (_E04B.Trader != null)
		{
			_traderRequirementIcon.Show(_E04B);
		}
		ShowGameObject();
		UI.AddDisposable(requirement.OnFulfillmentChange.Bind(delegate
		{
			_fulfilledStatus.Show(_E04B.Fulfilled);
			_fulfilledStatus.gameObject.SetActive(!ignoreFulfillment);
		}));
	}

	public override void Close()
	{
		_traderRequirementIcon.Close();
		_fulfilledStatus.Close();
		if (_E02A != null)
		{
			_E02A.Close();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E000(PointerEventData eventData)
	{
		string arg = _E04B.Trader.Settings.Nickname.Localized();
		string text = string.Empty;
		_E83C obj = _E04B;
		if (obj != null)
		{
			if (!(obj is TraderLoyaltyRequirement traderLoyaltyRequirement))
			{
				if (obj is TraderUnlockRequirement)
				{
					text = string.Format(_ED3E._E000(163939).Localized(), arg);
				}
			}
			else
			{
				TraderLoyaltyRequirement traderLoyaltyRequirement2 = traderLoyaltyRequirement;
				text = string.Format(_ED3E._E000(163909).Localized(), arg, traderLoyaltyRequirement2.LoyaltyLevel);
			}
		}
		_E02A.Show(text);
	}

	[CompilerGenerated]
	private void _E001(PointerEventData eventData)
	{
		_E02A.Close();
	}
}
