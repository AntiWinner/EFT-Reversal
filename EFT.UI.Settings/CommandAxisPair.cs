using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Settings;

public class CommandAxisPair : MonoBehaviour
{
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
	private Image _commandBackground;

	[SerializeField]
	private Image _keyBackground;

	[SerializeField]
	private Image _key2Background;

	[SerializeField]
	private Color _defaultBackgroundColor;

	[SerializeField]
	private Color _resetBackgroundColor;

	[CompilerGenerated]
	private AxisGroup m__E000;

	[CompilerGenerated]
	private bool m__E001;

	internal Action _E002;

	internal Action<AxisGroup, InputSource, bool> _E003;

	public AxisGroup AxisGroup
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public bool Positive
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

	private void Awake()
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

	private IEnumerator _E000(CustomTextMeshProUGUI uiText, int pairIndex)
	{
		_E002(_defaultBackgroundColor);
		uiText.text = _ED3E._E000(257951).Localized();
		if (this._E002 != null)
		{
			this._E002();
		}
		InputSource inputSource = new InputSource();
		bool flag = true;
		while (flag)
		{
			EKeyPress eKeyPress = InputSource.Listen(ref inputSource, EGameKey.None);
			if (eKeyPress != 0 && inputSource.keyCode.Contains(KeyCode.Escape))
			{
				_E001(uiText, pairIndex);
				InputSource inputSource2 = (Positive ? AxisGroup.pairs[pairIndex].positive : AxisGroup.pairs[pairIndex].negative);
				_E002(inputSource2.IsEmpty ? _resetBackgroundColor : _defaultBackgroundColor);
				if (this._E003 != null)
				{
					this._E003(AxisGroup, inputSource2, Positive);
				}
				yield break;
			}
			if ((eKeyPress == EKeyPress.Up && (inputSource.isAxis || (!inputSource.isAxis && inputSource.keyCode.Count > 0))) || (eKeyPress == EKeyPress.Down && inputSource.keyCode.Count >= 3))
			{
				flag = false;
			}
			else if (eKeyPress == EKeyPress.Down)
			{
				uiText.text = inputSource.ToString() + _ED3E._E000(257928);
			}
			yield return null;
		}
		if (Positive)
		{
			AxisGroup.pairs[pairIndex].positive = inputSource;
		}
		else
		{
			AxisGroup.pairs[pairIndex].negative = inputSource;
		}
		if (this._E003 != null)
		{
			this._E003(AxisGroup, inputSource, Positive);
		}
		for (int i = 0; i < AxisGroup.pairs.Count; i++)
		{
			if (i != pairIndex && (Positive ? AxisGroup.pairs[i].positive : AxisGroup.pairs[i].negative).Equals(inputSource))
			{
				ResetInput(i);
			}
		}
		_E001(uiText, pairIndex);
	}

	private void _E001(CustomTextMeshProUGUI uiText, int index)
	{
		InputSource inputSource = (Positive ? AxisGroup.pairs[index].positive : AxisGroup.pairs[index].negative);
		uiText.text = (inputSource.IsEmpty ? _ED3E._E000(176416).Localized() : inputSource.ToString());
	}

	public int ConflictIndex(InputSource input)
	{
		for (int i = 0; i < AxisGroup.pairs.Count; i++)
		{
			if ((Positive ? AxisGroup.pairs[i].positive : AxisGroup.pairs[i].negative).Equals(input))
			{
				return i;
			}
		}
		return -1;
	}

	public void ResetInput(int index)
	{
		InputSource obj = (Positive ? AxisGroup.pairs[index].positive : AxisGroup.pairs[index].negative);
		obj.isAxis = false;
		obj.axisName = null;
		obj.keyCode.Clear();
		_E001(_keyName, 0);
		_E001(_key2Name, 1);
		if (_E003())
		{
			_E002(_resetBackgroundColor);
		}
	}

	private void _E002(Color color)
	{
		_commandBackground.color = color;
		_keyBackground.color = color;
		_key2Background.color = color;
	}

	private bool _E003()
	{
		for (int i = 0; i < AxisGroup.pairs.Count; i++)
		{
			if (!(Positive ? AxisGroup.pairs[i].positive : AxisGroup.pairs[i].negative).IsEmpty)
			{
				return false;
			}
		}
		return true;
	}

	public void Show(AxisGroup axisGroup, bool positive)
	{
		AxisGroup = axisGroup ?? throw new ArgumentNullException(_ED3E._E000(257893));
		Positive = positive;
		_commandName.LocalizationKey = string.Format(_ED3E._E000(53834), axisGroup.axisName, positive ? _ED3E._E000(261907) : _ED3E._E000(261914));
		_E001(_keyName, 0);
		_E001(_key2Name, 1);
		_E002(_E003() ? _resetBackgroundColor : _defaultBackgroundColor);
	}

	[CompilerGenerated]
	private void _E004()
	{
		StartCoroutine(_E000(_keyName, 0));
	}

	[CompilerGenerated]
	private void _E005()
	{
		StartCoroutine(_E000(_key2Name, 1));
	}
}
