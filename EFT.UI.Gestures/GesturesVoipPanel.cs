using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

public sealed class GesturesVoipPanel : UIElement
{
	[SerializeField]
	private Button _statusButton;

	[SerializeField]
	private Image _statusImage;

	[SerializeField]
	private HoverTooltipArea _tooltipArea;

	[SerializeField]
	private Button _reportButton;

	[SerializeField]
	[Space]
	private Sprite _enabledSprite;

	[SerializeField]
	private Sprite _disabledSprite;

	private _E7B3 _E0A3;

	private Action _E309;

	private bool _E000
	{
		get
		{
			if (_E0A3 != null)
			{
				return _E0A3.Status.Value != EVoipControllerStatus.Banned;
			}
			return false;
		}
	}

	private void Awake()
	{
		_statusButton.onClick.AddListener(delegate
		{
			if (this._E000)
			{
				_E7DE._E000<_E7E0, _E7DF> sound = Singleton<_E7DE>.Instance.Sound;
				sound.Settings.VoipEnabled.Value = !sound.Settings.VoipEnabled;
				sound.Save().HandleExceptions();
			}
		});
		_reportButton.onClick.AddListener(delegate
		{
			if (this._E000)
			{
				_E857.DisplayMessageNotification(_ED3E._E000(233572).Localized());
				_E0A3.ReportAbuse();
			}
		});
	}

	public void Show(_E7B3 controller)
	{
		if (controller == null)
		{
			HideGameObject();
			return;
		}
		_E0A3 = controller;
		_E7DE._E000<_E7E0, _E7DF> sound = Singleton<_E7DE>.Instance.Sound;
		UI.BindState(sound.Settings.VoipEnabled, _E000);
		if (_E7DF.IsVoipFirstMessageShown)
		{
			ShowGameObject();
			return;
		}
		_E309 = sound.Settings.VoipEnabled.Bind(delegate
		{
			if (!_E7DF.IsVoipFirstMessageShown)
			{
				HideGameObject();
			}
			else
			{
				ShowGameObject();
				_E309();
				_E309 = null;
			}
		});
	}

	private void _E000(bool value)
	{
		if (value)
		{
			_statusImage.sprite = _enabledSprite;
			_tooltipArea.SetMessageText(() => _ED3E._E000(229763).Localized());
		}
		else
		{
			_statusImage.sprite = _disabledSprite;
			_tooltipArea.SetMessageText(() => _ED3E._E000(229807).Localized());
		}
		_reportButton.interactable = value && this._E000;
		_statusButton.interactable = this._E000;
	}

	public override void Close()
	{
		_E309?.Invoke();
		_E309 = null;
		base.Close();
		_E0A3 = null;
	}

	[CompilerGenerated]
	private void _E001()
	{
		if (this._E000)
		{
			_E7DE._E000<_E7E0, _E7DF> sound = Singleton<_E7DE>.Instance.Sound;
			sound.Settings.VoipEnabled.Value = !sound.Settings.VoipEnabled;
			sound.Save().HandleExceptions();
		}
	}

	[CompilerGenerated]
	private void _E002()
	{
		if (this._E000)
		{
			_E857.DisplayMessageNotification(_ED3E._E000(233572).Localized());
			_E0A3.ReportAbuse();
		}
	}

	[CompilerGenerated]
	private void _E003(bool _)
	{
		if (!_E7DF.IsVoipFirstMessageShown)
		{
			HideGameObject();
			return;
		}
		ShowGameObject();
		_E309();
		_E309 = null;
	}
}
