using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.UI.Screens;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ActionPanel : UIInputNode
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ActionPanel _003C_003E4__this;

		public _EC3F interactionState;

		internal InteractionButton _E000(_EC3E arg)
		{
			return _003C_003E4__this._interactionButtonTemplate;
		}

		internal Transform _E001(_EC3E arg)
		{
			return _003C_003E4__this._interactionButtonsContainer;
		}

		internal void _E002(_EC3E model, InteractionButton view)
		{
			view.Show(interactionState, model);
		}
	}

	private const float _E074 = 5f;

	[SerializeField]
	private TextMeshProUGUI _itemName;

	[SerializeField]
	private GameObject _itemPanel;

	[SerializeField]
	private GameObject _errorPanel;

	[SerializeField]
	private TextMeshProUGUI _errorText;

	[SerializeField]
	private Image _progressBar;

	[SerializeField]
	private UIPointer _pointer;

	[SerializeField]
	private InteractionButton _interactionButtonTemplate;

	[SerializeField]
	private RectTransform _interactionButtonsContainer;

	[SerializeField]
	private Vector2 _aimingShift = new Vector2(0f, -200f);

	private Vector2 _E075;

	private _EC79<_EC3E, InteractionButton> _E03E;

	private GamePlayerOwner _E076;

	private bool _E077;

	private bool _E078;

	private float _E079;

	private float _E07A;

	private bool _E07B;

	private void Start()
	{
		_E079 = _pointer.Image.color.a;
		_E005();
	}

	internal void Show(GamePlayerOwner owner)
	{
		_E076 = owner;
		UI.BindState(_E076.AvailableInteractionState, _E000);
		_E076.Player.OnHandsControllerChanged += delegate(Player.AbstractHandsController oldValue, Player.AbstractHandsController newValue)
		{
			if (oldValue != null)
			{
				oldValue.OnAimingChanged -= _E001;
			}
			if (newValue != null)
			{
				newValue.OnAimingChanged += _E001;
			}
		};
		UI.AddDisposable(delegate
		{
			_E076.Player.OnHandsControllerChanged -= delegate(Player.AbstractHandsController oldValue, Player.AbstractHandsController newValue)
			{
				if (oldValue != null)
				{
					oldValue.OnAimingChanged -= _E001;
				}
				if (newValue != null)
				{
					newValue.OnAimingChanged += _E001;
				}
			};
		});
		if (_E076.Player.HandsController != null)
		{
			_E076.Player.HandsController.OnAimingChanged += _E001;
		}
		ShowGameObject();
	}

	private void _E000([CanBeNull] _EC3F interactionState)
	{
		if (!_E07B)
		{
			_E075 = base.RectTransform.anchoredPosition;
			_E07B = true;
		}
		if (_E03E != null)
		{
			_E03E.SoftDispose();
			_E03E = null;
		}
		bool active = interactionState?.SelectedAction?.TargetName != null;
		_itemPanel.SetActive(active);
		bool flag = interactionState != null && interactionState.Actions.Count == 0 && !string.IsNullOrEmpty(interactionState.Error);
		_errorPanel.SetActive(flag);
		if (flag)
		{
			_errorText.text = interactionState.Error.ToUpper();
		}
		_progressBar.gameObject.SetActive(value: false);
		_interactionButtonsContainer.gameObject.SetActive(interactionState != null && interactionState.Actions.Count > 0);
		if (_interactionButtonsContainer.gameObject.activeSelf)
		{
			if (flag)
			{
				_E004();
			}
			else
			{
				_E003();
			}
		}
		else
		{
			_E005();
		}
		if (_E07B && _E076.Player.HandsController != null)
		{
			_E002(!_E076.Player.HandsController.IsAiming);
		}
		if (interactionState != null)
		{
			_E03E = new _EC79<_EC3E, InteractionButton>(interactionState.Actions, (_EC3E arg) => _interactionButtonTemplate, (_EC3E arg) => _interactionButtonsContainer, delegate(_EC3E model, InteractionButton view)
			{
				view.Show(interactionState, model);
			});
			if (interactionState.SelectedAction != null && !string.IsNullOrEmpty(interactionState.SelectedAction.TargetName))
			{
				_itemName.text = interactionState.SelectedAction.TargetName.Localized().ToUpper();
			}
		}
	}

	private void _E001(bool isAiming)
	{
		if (_E07B)
		{
			_E002(!isAiming);
		}
	}

	private void _E002(bool isDefaultPosition)
	{
		base.RectTransform.anchoredPosition = (isDefaultPosition ? _E075 : (_E075 + _aimingShift));
	}

	private void _E003()
	{
		_E077 = true;
		_pointer.Image.sprite = _pointer.HoverSprite;
		_pointer.Image.SetNativeSize();
		_E07A = 1f;
	}

	private void _E004()
	{
		_E077 = true;
		_pointer.Image.sprite = _pointer.UnavailableSprite;
		_pointer.Image.SetNativeSize();
		_E07A = 1f;
	}

	private void _E005()
	{
		_E077 = false;
		if (_E078)
		{
			ShowPointer(b: true);
		}
		else
		{
			_E07A = 0f;
		}
	}

	public void ShowPointer(bool b)
	{
		_E078 = b;
		if (!_E077)
		{
			_pointer.Image.sprite = _pointer.SenseSprite;
			_pointer.Image.SetNativeSize();
			_E07A = (b ? 1 : 0);
		}
	}

	private void Update()
	{
		if (!Mathf.Approximately(_E079, _E07A))
		{
			_E079 = Mathf.Lerp(_E079, _E07A, Time.deltaTime * 5f);
			_pointer.Image.color = new Color(1f, 1f, 1f, _E079);
		}
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (_E076 == null)
		{
			return ETranslateResult.Ignore;
		}
		if (_E076.AvailableInteractionState.Value == null)
		{
			return ETranslateResult.Ignore;
		}
		switch ((int)command)
		{
		case 72:
			_E076.AvailableInteractionState.Value.SelectNextAction();
			return ETranslateResult.Block;
		case 71:
			_E076.AvailableInteractionState.Value.SelectPreviousAction();
			return ETranslateResult.Block;
		case 9:
			if (_E076.AvailableInteractionState.Value.SelectedAction == null)
			{
				break;
			}
			if (_EC92.Instance.CheckCurrentScreen(EEftScreenType.Inventory))
			{
				return ETranslateResult.Block;
			}
			_E076.AvailableInteractionState.Value.SelectedAction.Action();
			return ETranslateResult.Block;
		}
		return ETranslateResult.Ignore;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}

	public void Hide()
	{
		if (this != null)
		{
			StopAllCoroutines();
		}
		if (_E03E != null)
		{
			_E03E.Dispose();
			_E03E = null;
		}
		if (_E076.Player.HandsController != null)
		{
			_E076.Player.HandsController.OnAimingChanged -= _E001;
		}
		UI.Dispose();
		_E076 = null;
	}

	[CompilerGenerated]
	private void _E006(Player.AbstractHandsController oldValue, Player.AbstractHandsController newValue)
	{
		if (oldValue != null)
		{
			oldValue.OnAimingChanged -= _E001;
		}
		if (newValue != null)
		{
			newValue.OnAimingChanged += _E001;
		}
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E076.Player.OnHandsControllerChanged -= delegate(Player.AbstractHandsController oldValue, Player.AbstractHandsController newValue)
		{
			if (oldValue != null)
			{
				oldValue.OnAimingChanged -= _E001;
			}
			if (newValue != null)
			{
				newValue.OnAimingChanged += _E001;
			}
		};
	}
}
