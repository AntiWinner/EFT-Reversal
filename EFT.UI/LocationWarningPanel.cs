using System;
using UnityEngine;

namespace EFT.UI;

public class LocationWarningPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _description;

	public void Set(_E554.Location.LocationRules rules)
	{
		base.gameObject.SetActive(!_E38D.DisabledForNow);
		switch (rules)
		{
		case _E554.Location.LocationRules.Normal:
		case _E554.Location.LocationRules.AvoidOwnPmc:
			_description.text = _ED3E._E000(249276).Localized();
			break;
		case _E554.Location.LocationRules.AvoidAllPmc:
			_description.text = _ED3E._E000(249305).Localized();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(249338), rules, null);
		}
	}
}
