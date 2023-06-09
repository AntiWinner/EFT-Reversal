using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Settings;

public sealed class CommandKeyPair : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public int index;

		internal bool _E000(InputSource t, int i)
		{
			if (i != index)
			{
				return !t.IsEmpty;
			}
			return false;
		}
	}

	private const string m__E000 = "Settings/NotSet";

	[SerializeField]
	private LocalizedText _commandName;

	[SerializeField]
	private Button _keyButton;

	[SerializeField]
	private CustomTextMeshProUGUI _keyName;

	[SerializeField]
	private Button _key2Button;

	[SerializeField]
	private CustomTextMeshProUGUI _key2Name;

	[SerializeField]
	private GameObject _emptyPressTypeCell;

	[SerializeField]
	private GameObject _unavailableCell;

	[SerializeField]
	private DropDownBox _typeDropdown;

	[SerializeField]
	private Image _commandBackground;

	[SerializeField]
	private Image _keyBackground;

	[SerializeField]
	private Image _key2Background;

	[SerializeField]
	private Image _pressTypeBackground;

	[SerializeField]
	private Image _unavailableBackground;

	[SerializeField]
	private Color _defaultBackgroundColor;

	[SerializeField]
	private Color _resetBackgroundColor;

	[SerializeField]
	private Color _notInteractableTextColor = new Color(0.317f, 0.313f, 0.286f);

	[CompilerGenerated]
	private KeyGroup m__E001;

	internal Action _E002;

	internal Func<KeyGroup, InputSource, bool, bool> _E003;

	private bool m__E004 = true;

	private bool m__E005;

	private List<EPressType> m__E006;

	public KeyGroup KeyGroup
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public bool DropdownAvailable
	{
		get
		{
			if (KeyGroup.keyName.IsPressTypeSelectionAllowed())
			{
				return !KeyGroup.variants.Any((InputSource variant) => variant.isAxis);
			}
			return false;
		}
	}

	private void Awake()
	{
		if (this.m__E004)
		{
			_keyButton.onClick.AddListener(delegate
			{
				StartCoroutine(_E000(_keyName, 0));
			});
			_key2Button.onClick.AddListener(delegate
			{
				StartCoroutine(_E000(_key2Name, 1));
			});
		}
	}

	private IEnumerator _E000(CustomTextMeshProUGUI uiText, int variantIndex)
	{
		_keyButton.onClick.RemoveAllListeners();
		_key2Button.onClick.RemoveAllListeners();
		_E004(_defaultBackgroundColor);
		uiText.text = _ED3E._E000(257951).Localized();
		this._E002?.Invoke();
		InputSource inputSource = new InputSource();
		bool flag = true;
		while (flag)
		{
			EKeyPress eKeyPress = InputSource.Listen(ref inputSource, KeyGroup.keyName);
			if (eKeyPress != 0 && inputSource.keyCode.Contains(KeyCode.Escape))
			{
				_E001(uiText, variantIndex);
				_E002();
				_E004(this.m__E005 ? _resetBackgroundColor : _defaultBackgroundColor);
				_keyButton.onClick.AddListener(delegate
				{
					StartCoroutine(_E000(_keyName, 0));
				});
				_key2Button.onClick.AddListener(delegate
				{
					StartCoroutine(_E000(_key2Name, 1));
				});
				this._E003?.Invoke(KeyGroup, KeyGroup.variants[variantIndex], arg3: true);
				yield break;
			}
			if ((eKeyPress == EKeyPress.Up && (inputSource.isAxis || (!inputSource.isAxis && inputSource.keyCode.Count > 0))) || (eKeyPress == EKeyPress.Down && inputSource.keyCode.Count >= 3))
			{
				flag = false;
			}
			else if (eKeyPress == EKeyPress.Down)
			{
				uiText.text = string.Format(_ED3E._E000(257976), inputSource);
			}
			yield return null;
		}
		KeyGroup.variants[variantIndex] = inputSource;
		bool flag2 = this.m__E005;
		this.m__E005 = false;
		_E001(uiText, variantIndex);
		if ((this._E003 == null || !this._E003(KeyGroup, inputSource, arg3: false) || flag2) && DropdownAvailable && _E006(variantIndex))
		{
			if (flag2)
			{
				KeyGroup.pressType = this.m__E006[0];
				_typeDropdown.UpdateValue(0);
			}
			_typeDropdown.OnPointerClick(null);
		}
		else
		{
			if (KeyGroup.variants.Any((InputSource variant) => variant.isAxis))
			{
				KeyGroup.pressType = EPressType.Press;
			}
			this._E003?.Invoke(KeyGroup, inputSource, arg3: true);
		}
		for (int i = 0; i < KeyGroup.variants.Count; i++)
		{
			if (i != variantIndex && KeyGroup.variants[i].Equals(inputSource))
			{
				ResetInput(i);
			}
		}
		_E002();
		_keyButton.onClick.AddListener(delegate
		{
			StartCoroutine(_E000(_keyName, 0));
		});
		_key2Button.onClick.AddListener(delegate
		{
			StartCoroutine(_E000(_key2Name, 1));
		});
	}

	private void _E001(CustomTextMeshProUGUI uiText, int variantIndex)
	{
		bool flag = this.m__E005 || KeyGroup.variants.Count <= variantIndex || KeyGroup.variants[variantIndex].IsEmpty;
		uiText.text = (flag ? _ED3E._E000(176416).Localized() : KeyGroup.variants[variantIndex].ToString());
	}

	private void _E002()
	{
		bool active = false;
		bool active2 = false;
		bool active3 = false;
		if (!DropdownAvailable)
		{
			active3 = true;
		}
		else if (!KeyGroup.PressTypeAvailable)
		{
			active2 = true;
		}
		else
		{
			active = true;
		}
		_typeDropdown.gameObject.SetActive(active);
		_emptyPressTypeCell.SetActive(active2);
		_unavailableCell.SetActive(active3);
	}

	private void _E003(int valueIndex)
	{
		KeyGroup.pressType = this.m__E006[valueIndex];
		if (this._E003 != null)
		{
			foreach (InputSource variant in KeyGroup.variants)
			{
				this._E003(KeyGroup, variant, arg3: true);
			}
		}
		_E002();
	}

	private void _E004(Color color)
	{
		_commandBackground.color = color;
		_keyBackground.color = color;
		_key2Background.color = color;
		_pressTypeBackground.color = color;
		_unavailableBackground.color = color;
	}

	public void ResetInput(int index)
	{
		KeyGroup.variants[index].axisName = null;
		KeyGroup.variants[index].keyCode.Clear();
		KeyGroup.variants[index].isAxis = false;
		this.m__E005 = _E005();
		_E001(_keyName, 0);
		_E001(_key2Name, 1);
		if (this.m__E005)
		{
			_E002();
			_E004(_resetBackgroundColor);
		}
	}

	public int ConflictIndex(InputSource input)
	{
		for (int i = 0; i < KeyGroup.variants.Count; i++)
		{
			if (KeyGroup.variants[i].Equals(input))
			{
				return i;
			}
		}
		return -1;
	}

	private bool _E005()
	{
		return KeyGroup.variants.All((InputSource keyVariant) => keyVariant.IsEmpty);
	}

	private bool _E006(int index)
	{
		return !KeyGroup.variants.Where((InputSource t, int i) => i != index && !t.IsEmpty).Any();
	}

	public void Show(KeyGroup keyGroup, Dictionary<string, EPressType> pressTypes, bool interactable = true)
	{
		KeyGroup = keyGroup ?? throw new ArgumentNullException(_ED3E._E000(257927));
		_commandName.LocalizationKey = keyGroup.keyName.ToString();
		_E001(_keyName, 0);
		_E001(_key2Name, 1);
		_typeDropdown.Show(pressTypes.Keys);
		this.m__E006 = new List<EPressType>(pressTypes.Values);
		int num = this.m__E006.IndexOf(keyGroup.pressType);
		if (num < 0)
		{
			num = 0;
		}
		_typeDropdown.UpdateValue(num, sendCallback: false);
		_typeDropdown.DropdownClosedHandler(_E003);
		_typeDropdown.enabled = true;
		this.m__E004 = interactable;
		if (!this.m__E004)
		{
			_typeDropdown.gameObject.SetActive(value: false);
			_emptyPressTypeCell.SetActive(value: false);
			_unavailableCell.SetActive(value: true);
			_keyName.color = _notInteractableTextColor;
			_key2Name.color = _notInteractableTextColor;
			_keyButton.onClick.RemoveAllListeners();
			_key2Button.onClick.RemoveAllListeners();
		}
		else
		{
			_E002();
			this.m__E005 = _E005();
			if (this.m__E005)
			{
				_keyName.text = _ED3E._E000(176416).Localized();
				_E004(_resetBackgroundColor);
			}
			else
			{
				_E004(_defaultBackgroundColor);
			}
		}
	}

	public void Close()
	{
		_typeDropdown.Hide();
	}

	[CompilerGenerated]
	private void _E007()
	{
		StartCoroutine(_E000(_keyName, 0));
	}

	[CompilerGenerated]
	private void _E008()
	{
		StartCoroutine(_E000(_key2Name, 1));
	}

	[CompilerGenerated]
	private void _E009()
	{
		StartCoroutine(_E000(_keyName, 0));
	}

	[CompilerGenerated]
	private void _E00A()
	{
		StartCoroutine(_E000(_key2Name, 1));
	}

	[CompilerGenerated]
	private void _E00B()
	{
		StartCoroutine(_E000(_keyName, 0));
	}

	[CompilerGenerated]
	private void _E00C()
	{
		StartCoroutine(_E000(_key2Name, 1));
	}
}
