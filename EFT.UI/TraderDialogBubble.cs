using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class TraderDialogBubble : UIElement
{
	[SerializeField]
	private TMP_Text _speakerNameField;

	[SerializeField]
	private TMP_Text _text;

	public void Show(_E8BC._E000 line)
	{
		_speakerNameField.text = line.SpeakerName;
		_text.text = line.Line;
		ShowGameObject();
	}
}
