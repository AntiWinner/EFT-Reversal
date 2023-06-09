using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public abstract class HealthTreatmentView : UIElement
{
	private const string _E1E6 = "UI/HealthTreatmentName_";

	private const string _E1E7 = "UI/HealthTreatmentAction_";

	[SerializeField]
	private UpdatableToggle _checkbox;

	[SerializeField]
	private TextMeshProUGUI _treatmentNameField;

	[SerializeField]
	private TextMeshProUGUI _treatmentActionNameField;

	[SerializeField]
	private TextMeshProUGUI _costField;

	[SerializeField]
	private Image _highlight;

	[NonSerialized]
	public _ECED<bool> OnSelect = new _ECED<bool>();

	private _EC83 _E1E8;

	private bool _E03F = true;

	[CompilerGenerated]
	private float _E1E9;

	protected bool Active
	{
		get
		{
			return _E03F;
		}
		set
		{
			_E03F = value;
			base.gameObject.SetActive(value);
		}
	}

	protected float CostTotal
	{
		[CompilerGenerated]
		get
		{
			return _E1E9;
		}
		[CompilerGenerated]
		set
		{
			_E1E9 = value;
		}
	}

	private string _E000 => _E1E8.TreatmentName;

	protected void Show(_EC83 treatment)
	{
		SetSelected(selected: false);
		_E1E8 = treatment;
		_treatmentNameField.text = (_ED3E._E000(260706) + _E000).Localized();
		_treatmentActionNameField.text = (_ED3E._E000(260746) + _E000).Localized();
		_checkbox.Bind(SetSelected);
		UpdateCost();
		ShowGameObject();
	}

	public void SetSelected(bool selected)
	{
		_checkbox.UpdateValue(selected, sendCallback: false);
		OnSelect.Invoke(selected);
		_highlight.gameObject.SetActive(selected);
	}

	protected void UpdateCost()
	{
		_costField.SetMonospaceText(HealthTreatmentServiceView.FormatRublesString(CostTotal));
	}
}
