using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace EFT.UI.Ragfair;

public class ItemMarketPricesPanel : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _lowestLabel;

	[SerializeField]
	private TextMeshProUGUI _averageLabel;

	[SerializeField]
	private TextMeshProUGUI _maximumLabel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _pricesPanel;

	private _ECBD _E328;

	[CompilerGenerated]
	private float _E34C;

	[CompilerGenerated]
	private float _E34D;

	[CompilerGenerated]
	private float _E34E;

	public float Minimum
	{
		[CompilerGenerated]
		get
		{
			return _E34C;
		}
		[CompilerGenerated]
		private set
		{
			_E34C = value;
		}
	}

	public float Average
	{
		[CompilerGenerated]
		get
		{
			return _E34D;
		}
		[CompilerGenerated]
		private set
		{
			_E34D = value;
		}
	}

	public float Maximum
	{
		[CompilerGenerated]
		get
		{
			return _E34E;
		}
		[CompilerGenerated]
		private set
		{
			_E34E = value;
		}
	}

	public void Show(_ECBD ragfair)
	{
		_E328 = ragfair;
	}

	public void UpdatePrices(string templateId)
	{
		_loader.SetActive(value: true);
		_pricesPanel.SetActive(value: false);
		_E328.GetMarketPrices(templateId, delegate(ItemMarketPrices result)
		{
			Minimum = result.min;
			Average = result.avg;
			Maximum = result.max;
			_lowestLabel.text = _ED3E._E000(243280).Localized() + _ED3E._E000(243269) + _E000(Minimum) + _ED3E._E000(59467);
			_averageLabel.text = _ED3E._E000(243318).Localized() + _ED3E._E000(243269) + _E000(Average) + _ED3E._E000(59467);
			_maximumLabel.text = _ED3E._E000(243303).Localized() + _ED3E._E000(243269) + _E000(Maximum) + _ED3E._E000(59467);
			_loader.SetActive(value: false);
			_pricesPanel.SetActive(value: true);
		});
	}

	private string _E000(float value)
	{
		int num = Mathf.RoundToInt(value);
		if (num <= 0)
		{
			return _ED3E._E000(254434);
		}
		return num.FormatSeparate(_ED3E._E000(18502));
	}

	[CompilerGenerated]
	private void _E001(ItemMarketPrices result)
	{
		Minimum = result.min;
		Average = result.avg;
		Maximum = result.max;
		_lowestLabel.text = _ED3E._E000(243280).Localized() + _ED3E._E000(243269) + _E000(Minimum) + _ED3E._E000(59467);
		_averageLabel.text = _ED3E._E000(243318).Localized() + _ED3E._E000(243269) + _E000(Average) + _ED3E._E000(59467);
		_maximumLabel.text = _ED3E._E000(243303).Localized() + _ED3E._E000(243269) + _E000(Maximum) + _ED3E._E000(59467);
		_loader.SetActive(value: false);
		_pricesPanel.SetActive(value: true);
	}
}
