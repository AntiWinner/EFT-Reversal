using UnityEngine;

namespace EFT.UI;

public class StatItemView : UIElement
{
	public const string UNITS_LITERS = "UI/ProfileStats/Liters";

	public const string UNITS_METERS = "UI/ProfileStats/Meters";

	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _value;

	public void Show(_E34D._E000 statItem)
	{
		ShowGameObject();
		_caption.text = statItem.Caption;
		_value.text = string.Concat(statItem.Value, _E000(statItem.Type));
	}

	private static string _E000(_E34D.EStatType statType)
	{
		if (statType == _E34D.EStatType.Litres)
		{
			return _ED3E._E000(258203).Localized();
		}
		return string.Empty;
	}
}
