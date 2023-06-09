using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class PriceRow : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _amount;

	[SerializeField]
	private TextMeshProUGUI _extraLabel;

	private void _E000()
	{
		_amount.gameObject.SetActive(value: false);
		_extraLabel.gameObject.SetActive(value: false);
	}

	public void Show(_EC7F rowInfo)
	{
		ShowGameObject();
		_E000();
		if (!string.IsNullOrEmpty(rowInfo.Label))
		{
			_amount.gameObject.SetActive(value: true);
			_amount.text = rowInfo.Label;
		}
		if (!string.IsNullOrEmpty(rowInfo.Value))
		{
			_extraLabel.gameObject.SetActive(value: true);
			_extraLabel.text = rowInfo.Value;
		}
	}
}
