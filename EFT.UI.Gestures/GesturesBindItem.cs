using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

public sealed class GesturesBindItem : GestureBaseItem
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Button _bindButton;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private CustomTextMeshProUGUI _commandLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _bindLabel;

	private HashSet<EPhraseTrigger> _E2FF;

	private ColorMap _E300;

	private string _E301;

	private int _E288;

	[CompilerGenerated]
	private ECommand _E302;

	public ECommand Command
	{
		[CompilerGenerated]
		get
		{
			return _E302;
		}
		[CompilerGenerated]
		private set
		{
			_E302 = value;
		}
	}

	private new bool _E000
	{
		get
		{
			if (ItemIndex < 8)
			{
				return ItemIndex == 0;
			}
			return true;
		}
	}

	private Color _E001
	{
		get
		{
			if (!this._E000)
			{
				return _E300[_ED3E._E000(229552)];
			}
			return _E300[_ED3E._E000(229543)];
		}
	}

	private Color _E002
	{
		get
		{
			if (!this._E000)
			{
				return _E300[_ED3E._E000(229588)];
			}
			return _E300[_ED3E._E000(229572)];
		}
	}

	private Color _E003
	{
		get
		{
			if (!this._E000)
			{
				return _E300[_ED3E._E000(229626)];
			}
			return _E300[_ED3E._E000(229605)];
		}
	}

	private Color _E004
	{
		get
		{
			if (!this._E000)
			{
				return _E300[_ED3E._E000(229654)];
			}
			return _E300[_ED3E._E000(229634)];
		}
	}

	public override int ItemIndex => _E288;

	private bool _E005
	{
		set
		{
			_canvasGroup.SetUnlockStatus(value);
		}
	}

	private void Awake()
	{
		_bindButton.onClick.AddListener(delegate
		{
			OnPointerClicked.Invoke(new _E000
			{
				ItemIndex = ItemIndex
			});
		});
	}

	public void Init(ColorMap colorMap, ECommand command, HashSet<EPhraseTrigger> availablePhrases)
	{
		_E2FF = availablePhrases;
		Command = command;
		_E300 = colorMap;
		_bindLabel.text = command.ToString();
		_background.color = this._E001;
		_commandLabel.color = _E003;
	}

	public void Show(int index, string caption)
	{
		_E288 = index;
		_E301 = caption;
		ShowInternal(Command, _E288, _E301);
	}

	protected override void BindUpdatedHandler(GesturesMenu._E000 bind)
	{
		if (bind.Command == Command)
		{
			_E000(bind.Index, bind.Caption);
		}
		else if (bind.Index.Equals(_E288))
		{
			_E000(0, string.Empty);
		}
		_background.color = this._E001;
		_commandLabel.color = _E003;
	}

	private void _E000(int index, string caption)
	{
		_E288 = index;
		_E301 = caption;
		if (string.IsNullOrEmpty(_E301))
		{
			_commandLabel.text = string.Empty;
			_E005 = false;
			return;
		}
		_commandLabel.text = _E301.Localized();
		if (!this._E000)
		{
			_E005 = true;
			return;
		}
		EPhraseTrigger itemIndex = (EPhraseTrigger)ItemIndex;
		_E005 = GesturesQuickPanel.IsPhraseAvailable(itemIndex) && (_E2FF.Contains(itemIndex) || itemIndex == EPhraseTrigger.MumblePhrase);
	}

	protected override void UnderPointerChanged(bool isUnderPointer)
	{
		_background.color = (isUnderPointer ? _E002 : this._E001);
		_commandLabel.color = (isUnderPointer ? _E004 : _E003);
	}

	[CompilerGenerated]
	private void _E001()
	{
		OnPointerClicked.Invoke(new _E000
		{
			ItemIndex = ItemIndex
		});
	}
}
