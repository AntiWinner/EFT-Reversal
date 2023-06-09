using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

public sealed class GesturesAudioSubItem : GestureBaseItem
{
	public EPhraseTrigger PhraseTrigger = EPhraseTrigger.PhraseNone;

	public bool IsSituational;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _situationalMarker;

	[SerializeField]
	private CustomTextMeshProUGUI _textField;

	[SerializeField]
	[Space]
	private GameObject _bindObject;

	[SerializeField]
	private CustomTextMeshProUGUI _bindLabel;

	private Sprite _E2FA;

	private Sprite _E2FB;

	private Color _E2FC;

	private Color _E2FD;

	private ECommand _E2FE;

	public override int ItemIndex => (int)PhraseTrigger;

	public void Show(Sprite defaultBackground, Sprite selectedBackground, Color defaultAudioColor, Color selectedAudioColor, _ECAB binds)
	{
		_E2FC = defaultAudioColor;
		_E2FD = selectedAudioColor;
		_E2FA = defaultBackground;
		_E2FB = selectedBackground;
		_bindObject.SetActive(value: false);
		UnderPointerChanged(isUnderPointer: false);
		if (_situationalMarker != null)
		{
			_situationalMarker.SetActive(IsSituational);
		}
		_E39D.Deconstruct(binds.ContainsValue(ItemIndex) ? binds.FirstOrDefault((KeyValuePair<ECommand, int> x) => x.Value == ItemIndex) : new KeyValuePair<ECommand, int>(ECommand.None, 0), out var key, out var value);
		ECommand eCommand = key;
		int index = value;
		_E2FE = eCommand;
		_textField.text = PhraseTrigger.ToString().Localized();
		ShowInternal(_E2FE, index, _textField.text);
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
		_background.sprite = (isUnderPointer ? _E2FB : _E2FA);
		_textField.color = (isUnderPointer ? _E2FD : _E2FC);
	}

	[CompilerGenerated]
	private bool _E000(KeyValuePair<ECommand, int> x)
	{
		return x.Value == ItemIndex;
	}
}
