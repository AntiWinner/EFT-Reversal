using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public class AreaPanel : ButtonFeedback
{
	private const float _E03B = 0.5f;

	[SerializeField]
	private AreaIcon _areaIcon;

	[SerializeField]
	private RectTransform _iconImageContainer;

	[SerializeField]
	private TextMeshProUGUI _areaName;

	[SerializeField]
	private GameObject _statusObject;

	[SerializeField]
	private TextMeshProUGUI _areaStatus;

	[SerializeField]
	private TextMeshProUGUI _progressTextField;

	private int _E03C;

	private Action<AreaPanel> _E03D;

	[CompilerGenerated]
	private AreaData _E032;

	[CompilerGenerated]
	private AreaIcon _E03E;

	private bool _E03F;

	public AreaData Data
	{
		[CompilerGenerated]
		get
		{
			return _E032;
		}
		[CompilerGenerated]
		private set
		{
			_E032 = value;
		}
	}

	protected AreaIcon AreaIconCreated
	{
		[CompilerGenerated]
		get
		{
			return _E03E;
		}
		[CompilerGenerated]
		set
		{
			_E03E = value;
		}
	}

	protected AreaIcon AreaIcon => _areaIcon;

	protected TextMeshProUGUI AreaName => _areaName;

	protected RectTransform Container => _iconImageContainer;

	private (string, string, bool) _E000
	{
		get
		{
			switch (Data.Status)
			{
			case EAreaStatus.LockedToConstruct:
			case EAreaStatus.ReadyToConstruct:
			case EAreaStatus.ReadyToInstallConstruct:
			case EAreaStatus.ReadyToUpgrade:
			case EAreaStatus.ReadyToInstallUpgrade:
				return (Data.Status.ToString().Localized(), string.Empty, true);
			case EAreaStatus.NotSet:
			case EAreaStatus.LockedToUpgrade:
			case EAreaStatus.NoFutureUpgrades:
			case EAreaStatus.AutoUpgrading:
				return (string.Empty, string.Empty, false);
			case EAreaStatus.Constructing:
			case EAreaStatus.Upgrading:
			{
				_E03C++;
				if (_E03C > 4)
				{
					_E03C = 1;
				}
				string item = Data.Status.ToString().Localized() + _ED3E._E000(54246) + Data.TimeLeftFormatted + _ED3E._E000(27308);
				string text = string.Empty;
				switch (_E03C)
				{
				case 1:
					text = _ED3E._E000(165709);
					break;
				case 2:
					text += _ED3E._E000(165705);
					break;
				case 3:
					text += _ED3E._E000(59476);
					break;
				case 4:
					text += _ED3E._E000(165701);
					break;
				}
				return (item, text, true);
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public void Show(AreaData data, Action<AreaPanel> onSelected = null)
	{
		Init(data, onSelected);
		if (AreaIconCreated == null)
		{
			AreaIconCreated = UnityEngine.Object.Instantiate(_areaIcon, Container);
		}
		AreaIconCreated.Show(Data);
		UI.AddDisposable(delegate
		{
			AreaIconCreated.Close();
			UnityEngine.Object.Destroy(AreaIconCreated.gameObject);
			AreaIconCreated = null;
		});
	}

	protected virtual void Init(AreaData data, Action<AreaPanel> onSelected)
	{
		Data = data;
		_E03D = onSelected;
		ShowGameObject();
		_E03F = true;
		SetInfo();
		UI.AddDisposable(Data.LevelUpdated.Subscribe(SetInfo));
		UI.AddDisposable(Data.StatusUpdated.Subscribe(SetInfo));
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(SetInfo));
	}

	protected virtual async void SetInfo()
	{
		base.gameObject.SetActive(Data.DisplayInterface);
		AreaName.text = Data.Template.Name;
		while (!(_areaStatus == null))
		{
			var (text, text2, active) = this._E000;
			_areaStatus.SetMonospaceText(text);
			if (_progressTextField != null)
			{
				_progressTextField.text = text2;
				_progressTextField.gameObject.SetActive(!string.IsNullOrEmpty(text2));
			}
			_statusObject.SetActive(active);
			await TasksExtensions.Delay(0.5f);
			if ((Data.Status != EAreaStatus.Constructing && Data.Status != EAreaStatus.Upgrading) || !_E03F)
			{
				break;
			}
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		_E03D?.Invoke(this);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		Data?.Hover(value: true);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		Data?.Hover(value: false);
	}

	public override void Close()
	{
		_E03F = false;
		_areaIcon.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		AreaIconCreated.Close();
		UnityEngine.Object.Destroy(AreaIconCreated.gameObject);
		AreaIconCreated = null;
	}
}
