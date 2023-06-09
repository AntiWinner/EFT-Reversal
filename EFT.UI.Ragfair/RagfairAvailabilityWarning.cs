using TMPro;
using UnityEngine;

namespace EFT.UI.Ragfair;

public sealed class RagfairAvailabilityWarning : UIElement
{
	private const float _E379 = 1f;

	[SerializeField]
	private TextMeshProUGUI _textField;

	private _ECBD _E328;

	private double _E37A;

	public void Show(_ECBD ragFair)
	{
		_E328 = ragFair;
		_E37A = 0.0;
		_E000();
		ShowGameObject();
	}

	private void Update()
	{
		if (!(_E5AD.UtcNowUnix < _E37A))
		{
			_E000();
		}
	}

	private void _E000()
	{
		_E37A = _E5AD.UtcNowUnix + 1.0;
		TextMeshProUGUI textField = _textField;
		_ECBD obj = _E328;
		textField.SetMonospaceText((obj != null && !obj.Available) ? _E328.GetRawStatusDescription() : string.Empty);
	}

	public override void Close()
	{
		_E328 = null;
		_E37A = 0.0;
		base.Close();
	}
}
