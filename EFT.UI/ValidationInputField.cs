using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class ValidationInputField : TMP_InputField
{
	private readonly struct _E000
	{
		public readonly EValidationType Type;

		public readonly string Regex;

		public _E000(EValidationType type, string regex)
		{
			Type = type;
			Regex = regex;
		}
	}

	private const float m__E000 = 0.2f;

	private static readonly _E000[] m__E001 = new _E000[21]
	{
		new _E000(EValidationType.Numbers, _ED3E._E000(255530)),
		new _E000(EValidationType.Latin, _ED3E._E000(255526)),
		new _E000(EValidationType.AnyWordChars, _ED3E._E000(255581)),
		new _E000(EValidationType.Underscore, _ED3E._E000(48793)),
		new _E000(EValidationType.Period, _ED3E._E000(255576)),
		new _E000(EValidationType.Comma, _ED3E._E000(255579)),
		new _E000(EValidationType.Space, _ED3E._E000(255574)),
		new _E000(EValidationType.Slash, _ED3E._E000(255569)),
		new _E000(EValidationType.Brackets, _ED3E._E000(255565)),
		new _E000(EValidationType.Hyphen, _ED3E._E000(255614)),
		new _E000(EValidationType.Exclamation, _ED3E._E000(255609)),
		new _E000(EValidationType.Question, _ED3E._E000(255604)),
		new _E000(EValidationType.Quotes, _ED3E._E000(255607)),
		new _E000(EValidationType.At, _ED3E._E000(255596)),
		new _E000(EValidationType.Colons, _ED3E._E000(255599)),
		new _E000(EValidationType.Math, _ED3E._E000(255588)),
		new _E000(EValidationType.And, _ED3E._E000(255647)),
		new _E000(EValidationType.Dollar, _ED3E._E000(255642)),
		new _E000(EValidationType.Separator, _ED3E._E000(255637)),
		new _E000(EValidationType.NumSign, _ED3E._E000(255632)),
		new _E000(EValidationType.NewLine, _ED3E._E000(2540))
	};

	[SerializeField]
	private TextMeshProUGUI _errorLabel;

	[SerializeField]
	private LocalizedText _usedSymbolsCount;

	[SerializeField]
	private Color _outOfRangeColor = Color.red;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private EValidationType _validationType = EValidationType.Everything;

	[SerializeField]
	private int _minCharsCount = -1;

	[SerializeField]
	private int _maxDigitsCount = -1;

	private readonly Regex m__E002 = new Regex(_ED3E._E000(255535));

	private Regex m__E003;

	private Regex m__E004;

	public readonly _ECF5<bool> HasError = new _ECF5<bool>(initialValue: false);

	protected override void Awake()
	{
		base.Awake();
		base.onValueChanged.AddListener(_E001);
		if (_defaultColor == default(Color))
		{
			_defaultColor = base.colors.normalColor;
		}
		_E000();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_E002();
	}

	private void _E000()
	{
		if (this.m__E003 == null || this.m__E004 == null)
		{
			string text = _E004(_validationType);
			this.m__E003 = new Regex(_ED3E._E000(255499) + text + _ED3E._E000(255494));
			this.m__E004 = new Regex(_ED3E._E000(255490) + text + _ED3E._E000(11164));
			ValidateCurrentInput();
		}
	}

	public void ValidateCurrentInput()
	{
		_E001(base.text);
	}

	private void _E001(string nickname)
	{
		ShowError(_E003(nickname));
		_E002();
	}

	private void _E002()
	{
		if (_usedSymbolsCount != null)
		{
			_usedSymbolsCount.SetFormatValues(_ED3E._E000(255549) + base.text.Length + _ED3E._E000(30703) + base.characterLimit + _ED3E._E000(59467));
		}
	}

	private ENicknameError _E003(string textValue)
	{
		if (textValue.Length > 0 && !this.m__E003.IsMatch(textValue))
		{
			SetTextWithoutNotify(this.m__E004.Replace(base.text, string.Empty));
			return ENicknameError.WrongSymbol;
		}
		if (_minCharsCount > 0 && textValue.Length < _minCharsCount)
		{
			return ENicknameError.TooShort;
		}
		if (textValue.Length > base.characterLimit)
		{
			return ENicknameError.CharacterLimit;
		}
		if (_maxDigitsCount >= 0 && this.m__E002.Matches(textValue).Count > _maxDigitsCount)
		{
			return ENicknameError.DigitsLimit;
		}
		return ENicknameError.ValidNickname;
	}

	public void ShowErrorFromCode(int errorCode)
	{
		switch ((EBackendErrorCode)errorCode)
		{
		case EBackendErrorCode.NicknameChangeTimeout:
			ShowError(ENicknameError.NicknameChangeTimeout);
			break;
		case EBackendErrorCode.NicknameNotUnique:
			ShowError(ENicknameError.NicknameTaken);
			break;
		default:
			ShowError(ENicknameError.InvalidNickname);
			break;
		}
	}

	public void ShowError(ENicknameError error)
	{
		bool flag = error == ENicknameError.ValidNickname;
		string message = ((!flag) ? error.Localized(EStringCase.None) : string.Empty);
		ShowMessage(message, !flag);
	}

	public void HideSymbolCountLabel(bool needHide)
	{
		if (_usedSymbolsCount != null)
		{
			_usedSymbolsCount.gameObject.SetActive(!needHide);
		}
	}

	public void ShowMessage(string message, bool isError)
	{
		_E000();
		HasError.Value = isError;
		if (_errorLabel != null)
		{
			_errorLabel.text = message;
		}
		if ((bool)HasError && IsActive())
		{
			StartCoroutine(base.textComponent.Co_ChangeColorTemporary(0.2f, _defaultColor, _outOfRangeColor, delegate
			{
				StartCoroutine(base.textComponent.Co_ChangeColorTemporary(0.2f, _outOfRangeColor, _defaultColor));
			}));
			if (!(_errorLabel == null))
			{
				StartCoroutine(_errorLabel.Co_ChangeColorTemporary(0.2f, Color.black, _outOfRangeColor, delegate
				{
					StartCoroutine(_errorLabel.Co_ChangeColorTemporary(0.2f, _outOfRangeColor, Color.black));
				}));
			}
		}
		else
		{
			base.textComponent.color = _defaultColor;
			if (_errorLabel != null)
			{
				_errorLabel.color = Color.black;
			}
		}
	}

	public void DeSelect()
	{
		if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	private static string _E004(EValidationType validationType)
	{
		string text = string.Empty;
		_E000[] array = ValidationInputField.m__E001;
		for (int i = 0; i < array.Length; i++)
		{
			_E000 obj = array[i];
			if (!string.IsNullOrEmpty(obj.Regex) && validationType.HasFlagNoBox(obj.Type))
			{
				text += obj.Regex;
			}
		}
		return text;
	}

	[CompilerGenerated]
	private void _E005()
	{
		StartCoroutine(base.textComponent.Co_ChangeColorTemporary(0.2f, _outOfRangeColor, _defaultColor));
	}

	[CompilerGenerated]
	private void _E006()
	{
		StartCoroutine(_errorLabel.Co_ChangeColorTemporary(0.2f, _outOfRangeColor, Color.black));
	}
}
