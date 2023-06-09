using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class MultiLineRow : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _label;

	[SerializeField]
	private TextMeshProUGUI _value;

	[SerializeField]
	private GameObject _line;

	private void _E000()
	{
		_label.gameObject.SetActive(value: false);
		_value.gameObject.SetActive(value: false);
		_line.gameObject.SetActive(value: false);
	}

	public void Show(_EC7F rowInfo)
	{
		ShowGameObject();
		_E000();
		if (rowInfo == null)
		{
			_line.gameObject.SetActive(value: true);
			return;
		}
		if (!string.IsNullOrEmpty(rowInfo.Label))
		{
			_label.gameObject.SetActive(value: true);
			_label.text = rowInfo.Label.Localized();
		}
		if (!string.IsNullOrEmpty(rowInfo.Value))
		{
			_value.gameObject.SetActive(value: true);
			_value.text = rowInfo.Value.Localized();
		}
	}
}
