using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace EFT.UI.Ragfair;

public sealed class MassPurchasePanel : UIElement
{
	[SerializeField]
	private DefaultUIButton _purchaseButtonSpawner;

	[SerializeField]
	private TextMeshProUGUI _totalPriceLabel;

	private Dictionary<string, int> _E34F;

	private Dictionary<string, int> _E350;

	private Dictionary<Offer, int> _E351 = new Dictionary<Offer, int>();

	private _ECBD _E328;

	private Action<bool, Dictionary<_E7D3, int>> _E352;

	private HashSet<Offer> _E353 = new HashSet<Offer>();

	private void Awake()
	{
		_purchaseButtonSpawner.OnClick.AddListener(delegate
		{
			_E352?.Invoke(arg1: false, ((IEnumerable<KeyValuePair<Offer, int>>)_E351).ToDictionary((Func<KeyValuePair<Offer, int>, _E7D3>)((KeyValuePair<Offer, int> x) => x.Key), (Func<KeyValuePair<Offer, int>, int>)((KeyValuePair<Offer, int> x) => x.Value)));
		});
	}

	private void _E000()
	{
		_E351.Clear();
		foreach (var (key, value) in _E34F)
		{
			_E350[key] = value;
		}
		foreach (Offer item in _E353)
		{
			if (_E350.TryGetValue(item.Item.TemplateId, out var value2) && value2 > 0 && !item.LimitsReached)
			{
				if (value2 > item.CurrentItemCount)
				{
					value2 = item.CurrentItemCount;
				}
				_E351.Add(item, value2);
				_E350[item.Item.TemplateId] -= value2;
			}
		}
	}

	public void Show(Dictionary<string, int> includedItems, _ECBD ragfair, Action<bool, Dictionary<_E7D3, int>> onPurchaseButtonClicked)
	{
		UI.Dispose();
		_E34F = new Dictionary<string, int>(includedItems);
		_E350 = new Dictionary<string, int>(includedItems);
		_E328 = ragfair;
		_E352 = onPurchaseButtonClicked;
		_E328.OnOfferSelected += _E002;
		_E328.OnSelectedOffersCleared += _E003;
		UI.AddDisposable(delegate
		{
			_E328.OnSelectedOffersCleared -= _E003;
		});
		UI.AddDisposable(delegate
		{
			_E328.OnOfferSelected -= _E002;
		});
		UI.AddDisposable(_E328.OnFilteredOffersCountChanged.Subscribe(_E001));
		_E004();
		ShowGameObject();
	}

	private void _E001(EViewListType viewType)
	{
		_ECBB source = (viewType.Equals(EViewListType.WeaponBuild) ? _E328.WeaponBuildsFiltered : _E328.Offers);
		_E353 = LinqExtensions.ToHashSet(source.Where((Offer offer) => _E328.IsSelected(offer)));
		_E004();
	}

	private void _E002(Offer offer, bool selected)
	{
		if (selected)
		{
			_E353.Add(offer);
		}
		else
		{
			_E353.Remove(offer);
		}
		_E004();
	}

	private void _E003()
	{
		_E353.Clear();
		_E351?.Clear();
		_totalPriceLabel.text = _ED3E._E000(27314);
		_purchaseButtonSpawner.Interactable = false;
	}

	private void _E004()
	{
		_E000();
		int num = 0;
		foreach (KeyValuePair<Offer, int> item in _E351)
		{
			_E39D.Deconstruct(item, out var key, out var value);
			Offer offer = key;
			int num2 = value;
			num += offer.RequirementsCost * num2;
		}
		_totalPriceLabel.text = num.ToString();
		_purchaseButtonSpawner.Interactable = _E353.Any();
	}

	public override void Close()
	{
		_E003();
		base.Close();
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E352?.Invoke(arg1: false, ((IEnumerable<KeyValuePair<Offer, int>>)_E351).ToDictionary((Func<KeyValuePair<Offer, int>, _E7D3>)((KeyValuePair<Offer, int> x) => x.Key), (Func<KeyValuePair<Offer, int>, int>)((KeyValuePair<Offer, int> x) => x.Value)));
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E328.OnSelectedOffersCleared -= _E003;
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E328.OnOfferSelected -= _E002;
	}

	[CompilerGenerated]
	private bool _E008(Offer offer)
	{
		return _E328.IsSelected(offer);
	}
}
