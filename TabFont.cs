using UnityEngine;
using UnityEngine.EventSystems;

public class TabFont : Tab
{
	[SerializeField]
	private CustomTextMeshProUGUI _label;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _hoverColor;

	[SerializeField]
	private FontStyle _defaultFont;

	[SerializeField]
	private FontStyle _hoverFont;

	public override void OnPointerEnter(PointerEventData eventData)
	{
		_label.SetFontStyle(_hoverFont);
		_label.color = _hoverColor;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		_label.SetFontStyle(_defaultFont);
		_label.color = _defaultColor;
	}

	internal override void _E001(bool active)
	{
		Interactable = active;
	}
}
