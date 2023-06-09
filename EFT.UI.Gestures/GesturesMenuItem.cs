using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

public sealed class GesturesMenuItem : GestureBaseItem
{
	public EGesture Gesture;

	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _backgroundImage;

	[Space]
	[SerializeField]
	private GameObject _bindObject;

	[SerializeField]
	private CustomTextMeshProUGUI _bindLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _commandLabel;

	private Sprite _E2FA;

	private Sprite _E2FB;

	private ECommand _E2FE;

	public Sprite Icon
	{
		set
		{
			_iconImage.sprite = value;
		}
	}

	public override int ItemIndex => (int)Gesture;

	public void Show(Sprite defaultSubColor, Sprite selectedSubColor, _ECAB binds)
	{
		_E2FA = defaultSubColor;
		_E2FB = selectedSubColor;
		UnderPointerChanged(isUnderPointer: false);
		_E39D.Deconstruct(binds.ContainsValue(ItemIndex) ? binds.FirstOrDefault((KeyValuePair<ECommand, int> x) => x.Value == ItemIndex) : new KeyValuePair<ECommand, int>(ECommand.None, 0), out var key, out var value);
		ECommand eCommand = key;
		int index = value;
		_E2FE = eCommand;
		_commandLabel.text = Gesture.ToString().Localized();
		ShowInternal(_E2FE, index, _commandLabel.text);
	}

	protected override void BindUpdatedHandler(GesturesMenu._E000 bind)
	{
		if (bind.Index > 0 && bind.Index == ItemIndex)
		{
			_E2FE = bind.Command;
		}
		else if (bind.Command == _E2FE)
		{
			_E2FE = ECommand.None;
		}
		_bindLabel.text = _E2FE.ToString();
		_bindObject.SetActive(_E2FE != ECommand.None);
	}

	protected override void UnderPointerChanged(bool isUnderPointer)
	{
		_backgroundImage.sprite = (isUnderPointer ? _E2FB : _E2FA);
	}

	[CompilerGenerated]
	private bool _E000(KeyValuePair<ECommand, int> x)
	{
		return x.Value == ItemIndex;
	}
}
