using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class CancellableFilterPanel : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _filterNameLabel;

	[SerializeField]
	private Button _cancelButton;

	private _ECBD _E323;

	private _ECC0 _E2A7;

	private _ECC3 _E324;

	private void Awake()
	{
		_cancelButton.onClick.AddListener(delegate
		{
			_E323.RemoveCancellableFilter(_E2A7, sendCallback: true);
		});
	}

	public void Show(_ECBD ragFair, _ECC0 filter)
	{
		_E323 = ragFair;
		_E2A7 = filter;
		ShowGameObject();
		_filterNameLabel.text = (_ED3E._E000(232326) + filter.Type).Localized() + _ED3E._E000(18502) + filter.Name;
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E323.RemoveCancellableFilter(_E2A7, sendCallback: true);
	}
}
