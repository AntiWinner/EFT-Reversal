using System;
using System.Collections.Generic;
using EFT.HealthSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class HealthTreatmentFactorView : HealthTreatmentView
{
	[Serializable]
	public struct HealthFactorSettings
	{
		[SerializeField]
		public Color Color;

		[SerializeField]
		public Sprite Icon;
	}

	[SerializeField]
	private TextMeshProUGUI _valueToHealField;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Dictionary<EHealthFactorType, HealthFactorSettings> _healthFactorsSettings;

	private _EC84 _E1D7;

	public void Show(_EC84 observer)
	{
		_E1D7 = observer;
		HealthFactorSettings healthFactorSettings = _healthFactorsSettings[observer.FactorType];
		_valueToHealField.color = healthFactorSettings.Color;
		_icon.sprite = healthFactorSettings.Icon;
		Show((_EC83)observer);
		UI.AddDisposable(_E1D7.Subscribe(_E000));
		_E000();
	}

	private void _E000()
	{
		base.CostTotal = _E1D7.TreatmentCost;
		float valueToHeal = _E1D7.ValueToHeal;
		_valueToHealField.text = ((int)Math.Round(valueToHeal)).ToString();
		base.Active = _E1D7.Active;
		if (base.Active)
		{
			UpdateCost();
		}
	}
}
