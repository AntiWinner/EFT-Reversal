using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[UsedImplicitly]
public class TraderDialogWindowOptionRow : UIElement, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Color _defaultTextColor = new Color32(byte.MaxValue, 244, 210, 1);

	[SerializeField]
	private Color _selectedTextColor = Color.black;

	private _E8C4 _E226;

	public void Show(_E8C4 line)
	{
		_text.color = Color.white;
		_E226 = line;
		_icon.sprite = line.Icon;
		_E226.OnBlockDialog += _E001;
		UI.AddDisposable(delegate
		{
			_E226.OnBlockDialog -= _E001;
		});
		_text.gameObject.SetActive(value: true);
		_text.text = _E226.LineConstructor.GetDialogLine();
		_E000(selected: false);
		ShowGameObject();
	}

	private void _E000(bool selected)
	{
		_background.enabled = selected;
		_text.color = (selected ? _selectedTextColor : _defaultTextColor);
	}

	private void _E001()
	{
		_E000(selected: false);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		_E226.Execute();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		_E000(selected: true);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		_E000(selected: false);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E226.OnBlockDialog -= _E001;
	}
}
