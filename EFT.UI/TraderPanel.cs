using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class TraderPanel : UIElement
{
	private const double _E011 = 1.0;

	[SerializeField]
	private CustomTextMeshProUGUI _nickName;

	[SerializeField]
	private RankPanel _rankPanel;

	[SerializeField]
	private GameObject _questionMark;

	[SerializeField]
	private GameObject _newTraderLabel;

	[SerializeField]
	private TraderAvatar _traderAvatar;

	[SerializeField]
	private CustomTextMeshProUGUI _standing;

	[SerializeField]
	private CustomTextMeshProUGUI _timeLeft;

	[SerializeField]
	private TraderHoverPanel _proceedPanel;

	[SerializeField]
	private CanvasGroup _baseInfoGroup;

	[SerializeField]
	private GameObject _detailsGroup;

	[SerializeField]
	private GameObject _hiddenGroup;

	[SerializeField]
	private GameObject _timeGroup;

	[SerializeField]
	private GameObject _restockedGroup;

	private TraderTooltip _E02A;

	private DateTime _E012;

	private Profile._E001 _E1FB;

	private Action _E204;

	public string TraderId => _E1FB.Id;

	public void Awake()
	{
		HoverTrigger orAddComponent = _questionMark.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			_E02A.Show(_E1FB);
		};
		orAddComponent.OnHoverEnd += delegate
		{
			_E02A.Hide();
		};
	}

	public void Show(Profile._E001 trader, _E935 quests, TraderTooltip tooltip, Action onBarterSelected)
	{
		ShowGameObject();
		_E204 = onBarterSelected;
		_E1FB = trader;
		_E02A = tooltip;
		_E000();
		_proceedPanel.Show(delegate
		{
			_E204();
		});
		UI.AddDisposable(_proceedPanel);
		_traderAvatar.Show(_E1FB, quests);
		UI.AddDisposable(_traderAvatar);
		_E1FB.OnStandingChanged += _E001;
		UI.AddDisposable(delegate
		{
			_E1FB.OnStandingChanged -= _E001;
		});
		_E1FB.OnLoyaltyChanged += _E002;
		UI.AddDisposable(delegate
		{
			_E1FB.OnLoyaltyChanged -= _E002;
		});
	}

	public void SelectBarter()
	{
		_E204();
	}

	private void _E000()
	{
		bool available = _E1FB.Available;
		_baseInfoGroup.SetUnlockStatus(available, setRaycast: false);
		_detailsGroup.SetActive(available);
		_hiddenGroup.SetActive(!available);
		_nickName.text = _E1FB.Settings.Nickname.Localized();
		_newTraderLabel.SetActive(value: false);
		_E001();
		_E002();
	}

	private void _E001()
	{
		if (!(base.gameObject == null) && base.gameObject.activeSelf)
		{
			_standing.text = _E1FB.Standing.ToString(_ED3E._E000(253692));
		}
	}

	private void _E002()
	{
		if (!(base.gameObject == null) && base.gameObject.activeSelf)
		{
			_rankPanel.Show(_E1FB.LoyaltyLevel, _E1FB.MaxLoyaltyLevel);
		}
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (_E1FB != null && !((utcNow - _E012).TotalSeconds < 1.0))
		{
			_E012 = utcNow;
			TimeSpan timeLeft = _E5AD.UnixEpoch.AddSeconds(_E1FB.NextResupply) - utcNow;
			bool flag = timeLeft.Ticks <= 0;
			_timeLeft.SetMonospaceText((!flag) ? timeLeft.TraderFormat() : TimeSpan.Zero.TraderFormat());
			_timeGroup.SetActive(!flag);
			_restockedGroup.SetActive(flag);
		}
	}

	[CompilerGenerated]
	private void _E003(PointerEventData _)
	{
		_E02A.Show(_E1FB);
	}

	[CompilerGenerated]
	private void _E004(PointerEventData _)
	{
		_E02A.Hide();
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E204();
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E1FB.OnStandingChanged -= _E001;
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E1FB.OnLoyaltyChanged -= _E002;
	}
}
