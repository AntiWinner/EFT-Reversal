using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class GesturesQuickPanelItem : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _label;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _yObject;

	[SerializeField]
	private Color _defaultBackground;

	[SerializeField]
	private Color _selectedBackground;

	[SerializeField]
	private Color _defaultText;

	[SerializeField]
	private Color _selectedText;

	public void Show(EPhraseTrigger trigger)
	{
		ShowGameObject();
		_label.text = trigger.LocalizedShort(EStringCase.Upper);
	}

	public void Select()
	{
		_background.color = _selectedBackground;
		_label.color = _selectedText;
		_yObject.SetActive(value: true);
	}

	public void Deselect()
	{
		_background.color = _defaultBackground;
		_label.color = _defaultText;
		_yObject.SetActive(value: false);
	}

	public override void Close()
	{
		Deselect();
		base.Close();
	}
}
