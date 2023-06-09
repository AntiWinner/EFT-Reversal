using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class FiltersPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FiltersPanel _003C_003E4__this;

		public RagfairFilterButton button;

		public ESortType sortType;

		internal void _E000(RagfairFilterButton _)
		{
			_003C_003E4__this._E003(button, sortType);
		}
	}

	[SerializeField]
	private Toggle _checkBox;

	[SerializeField]
	private RagfairFilterButton _idButton;

	[SerializeField]
	private RagfairFilterButton _barterButton;

	[SerializeField]
	private RagfairFilterButton _ratingButton;

	[SerializeField]
	private RagfairFilterButton _offerItemButton;

	[SerializeField]
	private RagfairFilterButton _priceButton;

	[SerializeField]
	private RagfairFilterButton _expirationButton;

	[CompilerGenerated]
	private bool _E1A0;

	private readonly List<RagfairFilterButton> _E329 = new List<RagfairFilterButton>();

	private _ECBD _E328;

	private RagfairFilterButton _E1A2;

	private bool _E32A;

	private bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E1A0;
		}
		[CompilerGenerated]
		set
		{
			_E1A0 = value;
		}
	}

	private RagfairFilterButton _E001 => _E328.FilterRule.SortType switch
	{
		ESortType.Id => _idButton, 
		ESortType.Barter => _barterButton, 
		ESortType.Rating => _ratingButton, 
		ESortType.OfferItem => _offerItemButton, 
		ESortType.Price => _priceButton, 
		ESortType.ExpirationDate => _expirationButton, 
		_ => throw new ArgumentOutOfRangeException(), 
	};

	public void Show(_ECBD ragfair, EViewListType type)
	{
		UI.Dispose();
		ShowGameObject();
		_E329.Clear();
		_E328 = ragfair;
		_E328.OnGettingOffersProcessing += _E005;
		UI.AddDisposable(delegate
		{
			_E328.OnGettingOffersProcessing -= _E005;
		});
		_E328.OnOfferSelected += _E000;
		UI.AddDisposable(delegate
		{
			_E328.OnOfferSelected -= _E000;
		});
		_checkBox.gameObject.SetActive(type == EViewListType.WeaponBuild);
		UI.SubscribeEvent(_checkBox.onValueChanged, _E001);
		_E002(_idButton, ESortType.Id);
		_E002(_barterButton, ESortType.Barter);
		_E002(_ratingButton, ESortType.Rating);
		_E002(_offerItemButton, ESortType.OfferItem);
		_E002(_priceButton, ESortType.Price);
		_E002(_expirationButton, ESortType.ExpirationDate);
		this._E000 = ragfair.FilterRule.SortDirection;
		_E004(this._E001);
	}

	private void _E000(Offer _, bool __)
	{
		bool isOn = true;
		foreach (Offer weaponBuild in _E328.WeaponBuilds)
		{
			if (!weaponBuild.CanBeBought || _E328.IsSelected(weaponBuild))
			{
				continue;
			}
			isOn = false;
			break;
		}
		_E32A = true;
		_checkBox.isOn = isOn;
		_E32A = false;
	}

	private void _E001(bool isOn)
	{
		if (_E32A)
		{
			return;
		}
		foreach (Offer weaponBuild in _E328.WeaponBuilds)
		{
			if (weaponBuild.CanBeBought && _E328.IsSelected(weaponBuild) != isOn)
			{
				_E328.SwitchOfferSelection(weaponBuild);
			}
		}
	}

	private void _E002(RagfairFilterButton button, ESortType sortType)
	{
		_E329.Add(button);
		button.Show(delegate
		{
			_E003(button, sortType);
		});
		UI.AddDisposable(button);
	}

	private void _E003(RagfairFilterButton button, ESortType sortType)
	{
		if (!_E328.GettingOffers)
		{
			if (_E1A2 == button)
			{
				this._E000 = !this._E000;
			}
			_E004(button);
			FilterRule filterRule = _E328.FilterRule;
			filterRule.SortType = sortType;
			filterRule.SortDirection = this._E000;
			filterRule.Page = 0;
			_E39B.LogRagfair(string.Concat(_ED3E._E000(242942), sortType, _ED3E._E000(242930), this._E000 ? _ED3E._E000(242913) : _ED3E._E000(242926), _ED3E._E000(11164)));
			_E328.SetFilterRule(filterRule, clear: true);
		}
	}

	private void _E004(RagfairFilterButton currentButton)
	{
		_E1A2 = currentButton;
		if (this._E000)
		{
			currentButton.ApplyDescend();
		}
		else
		{
			currentButton.ApplyAscend();
		}
		currentButton.ShowIcon();
		foreach (RagfairFilterButton item in _E329)
		{
			if (!(item == currentButton))
			{
				item.HideIcon();
			}
		}
	}

	private void _E005(bool finished)
	{
		if (_E1A2 != null)
		{
			_E1A2.ShowLoader(finished);
		}
	}

	public override void Close()
	{
		base.Close();
		_E005(finished: true);
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E328.OnGettingOffersProcessing -= _E005;
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E328.OnOfferSelected -= _E000;
	}
}
