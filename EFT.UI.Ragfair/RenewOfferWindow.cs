using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class RenewOfferWindow : UIElement
{
	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _captionLabel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private CustomTextMeshProUGUI _currentTimeLabel;

	[SerializeField]
	private CanvasGroup _minusButtonGroup;

	[SerializeField]
	private CanvasGroup _plusButtonGroup;

	[SerializeField]
	private Button _minusButton;

	[SerializeField]
	private Button _plusButton;

	[SerializeField]
	private DefaultUIButton _renewButton;

	[SerializeField]
	private CustomTextMeshProUGUI _taxPriceLabel;

	[CompilerGenerated]
	private Offer _E1D4;

	[CompilerGenerated]
	private TimeSpan _E37F;

	[CompilerGenerated]
	private int _E380;

	[CompilerGenerated]
	private bool _E381;

	private Action _E35D;

	private _ECBD _E328;

	private _EAED _E092;

	private TimeSpan _E382;

	private TimeSpan _E383;

	public Offer Offer
	{
		[CompilerGenerated]
		get
		{
			return _E1D4;
		}
		[CompilerGenerated]
		private set
		{
			_E1D4 = value;
		}
	}

	public TimeSpan CurrentTime
	{
		[CompilerGenerated]
		get
		{
			return _E37F;
		}
		[CompilerGenerated]
		private set
		{
			_E37F = value;
		}
	}

	public int HoursCount
	{
		[CompilerGenerated]
		get
		{
			return _E380;
		}
		[CompilerGenerated]
		private set
		{
			_E380 = value;
		}
	}

	public bool Prioritized
	{
		[CompilerGenerated]
		get
		{
			return _E381;
		}
		[CompilerGenerated]
		private set
		{
			_E381 = value;
		}
	}

	private TimeSpan _E000 => CurrentTime.Subtract(TimeSpan.FromHours(1.0));

	private TimeSpan _E001 => CurrentTime.Add(TimeSpan.FromHours(1.0));

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
		_minusButton.onClick.AddListener(delegate
		{
			HoursCount--;
			CurrentTime = this._E000;
			_E002();
		});
		_plusButton.onClick.AddListener(delegate
		{
			HoursCount++;
			CurrentTime = this._E001;
			_E002();
		});
		_renewButton.OnClick.AddListener(_E001);
		_closeButton.onClick.AddListener(Close);
	}

	public void Show(_EAED inventoryController, _ECBD ragfair, Offer offer, TimeSpan time, Action onRenew)
	{
		_E092 = inventoryController;
		_E328 = ragfair;
		Offer = offer;
		_E382 = time;
		CurrentTime = time;
		_E35D = onRenew;
		HoursCount = 0;
		TimeSpan timeSpan = _ECBD.Settings.DefaultDuration.MultiplyByPercent(offer.RenewPercent);
		_E383 = timeSpan;
		_captionLabel.text = offer.ShortName.Localized();
		_E328.OnOfferRemoved += _E000;
		UI.AddDisposable(delegate
		{
			_E328.OnOfferRemoved -= _E000;
		});
		ShowGameObject();
		_E002();
	}

	private void _E000(string offerId)
	{
		if (Offer.Id == offerId)
		{
			Close();
		}
	}

	private void _E001()
	{
		_E35D?.Invoke();
		Close();
	}

	private void _E002()
	{
		int num = this._E000.CompareTo(_E382);
		_minusButtonGroup.SetUnlockStatus(num >= 0);
		int num2 = this._E001.CompareTo(_E383);
		_plusButtonGroup.SetUnlockStatus(num2 <= 0);
		_currentTimeLabel.text = CurrentTime.RagfairDateFormatLong();
		float num3 = (float)((Offer.ItemsCost + Offer.RequirementsCost) * Offer.CurrentItemCount) * (_ECBD.Settings.renewPricePerHour / 100f) * (float)HoursCount;
		_taxPriceLabel.text = num3.FormatSeparate(_ED3E._E000(18502)) + _ED3E._E000(260492);
		float num4 = num3;
		Dictionary<ECurrencyType, int> moneySums = _EB0E.GetMoneySums(_E092.Inventory.Stash.Grid.ContainedItems.Keys);
		bool flag = num4 <= (float)moneySums[ECurrencyType.RUB];
		_renewButton.Interactable = flag && num4 > 0f;
	}

	[CompilerGenerated]
	private void _E003()
	{
		HoursCount--;
		CurrentTime = this._E000;
		_E002();
	}

	[CompilerGenerated]
	private void _E004()
	{
		HoursCount++;
		CurrentTime = this._E001;
		_E002();
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E328.OnOfferRemoved -= _E000;
	}
}
