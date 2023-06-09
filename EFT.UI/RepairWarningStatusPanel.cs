using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFT.UI;

public class RepairWarningStatusPanel : UIElement
{
	[SerializeField]
	private RepairWarningStatus _errorPanelTemplate;

	[SerializeField]
	private RepairWarningStatus _warningPanelTemplate;

	[SerializeField]
	private RepairWarningStatus _infoPanelTemplate;

	[SerializeField]
	private RepairWarningStatus _commonBuffPanelTemplate;

	[SerializeField]
	private RepairWarningStatus _rareBuffPanelTemplate;

	[SerializeField]
	private RectTransform _warningsPlaceholder;

	[SerializeField]
	private ConditionCharacteristicsSlider _characteristicsSlider;

	[SerializeField]
	private CanvasGroup _repairerParametersPanel;

	[NonSerialized]
	public readonly _ECED<ERepairStatusWarning?> OnRepairStatusChanged = new _ECED<ERepairStatusWarning?>();

	private readonly Dictionary<ERepairStatusWarning, RepairWarningStatus> _E14A = new Dictionary<ERepairStatusWarning, RepairWarningStatus>();

	private RepairWarningStatus _E14B;

	private RepairWarningStatus _E14C;

	private RepairWarningStatus _E14D;

	private RepairWarningStatus _E14E;

	private Vector2 _E149;

	private RepairWarningStatus _E000
	{
		get
		{
			if (_E14B != null)
			{
				return _E14B;
			}
			_E14B = UnityEngine.Object.Instantiate(_warningPanelTemplate);
			_E14B.transform.SetParent(_warningsPlaceholder, worldPositionStays: false);
			return _E14B;
		}
	}

	private RepairWarningStatus _E001
	{
		get
		{
			if (_E14C != null)
			{
				return _E14C;
			}
			_E14C = UnityEngine.Object.Instantiate(_infoPanelTemplate);
			_E14C.transform.SetParent(_warningsPlaceholder, worldPositionStays: false);
			return _E14C;
		}
	}

	private RepairWarningStatus _E002
	{
		get
		{
			if (_E14D != null)
			{
				return _E14D;
			}
			_E14D = UnityEngine.Object.Instantiate(_commonBuffPanelTemplate);
			_E14D.transform.SetParent(_warningsPlaceholder, worldPositionStays: false);
			return _E14D;
		}
	}

	private RepairWarningStatus _E003
	{
		get
		{
			if (_E14E != null)
			{
				return _E14E;
			}
			_E14E = UnityEngine.Object.Instantiate(_rareBuffPanelTemplate);
			_E14E.transform.SetParent(_warningsPlaceholder, worldPositionStays: false);
			return _E14E;
		}
	}

	public void ShowDurabilityWarning(float percent)
	{
		if (percent > 0f)
		{
			this._E000.Show(string.Format(_ED3E._E000(247360).Localized(), Math.Round(_E149.x, 1), Math.Round(_E149.y, 1)));
		}
		else
		{
			this._E000.Close();
		}
	}

	public void ShowBuffPossibilityInfo(bool isVisible)
	{
		if (isVisible)
		{
			_E001.Show(string.Format(_ED3E._E000(247429).Localized()));
		}
		else
		{
			_E001.Close();
		}
	}

	public void ShowCommonBuffInfo(bool isVisible)
	{
		if (isVisible)
		{
			_E002.Show(string.Format(_ED3E._E000(247465).Localized()));
		}
		else
		{
			_E002.Close();
		}
	}

	public void ShowRareBuffInfo(bool isVisible)
	{
		if (isVisible)
		{
			_E003.Show(string.Format(_ED3E._E000(247510).Localized()));
		}
		else
		{
			_E003.Close();
		}
	}

	public void SetCriticalCondition(ERepairStatusWarning repairStatus, bool hasError, bool isWarningPanelVisible = true)
	{
		if (_E14A.ContainsKey(repairStatus) && !hasError)
		{
			_E14A[repairStatus].Close();
			_E14A.Remove(repairStatus);
		}
		else if (!_E14A.ContainsKey(repairStatus) && hasError)
		{
			RepairWarningStatus repairWarningStatus = UnityEngine.Object.Instantiate(_errorPanelTemplate);
			if (isWarningPanelVisible)
			{
				repairWarningStatus.transform.SetParent(_warningsPlaceholder, worldPositionStays: false);
				repairWarningStatus.Show(repairStatus.ToString().Localized());
			}
			_E14A.Add(repairStatus, repairWarningStatus);
		}
		_characteristicsSlider.SetSliderActive(_E14A.ContainsKey(ERepairStatusWarning.BrokenItem));
		_characteristicsSlider.SetSliderInteractable(_E14A.ContainsKey(ERepairStatusWarning.BrokenItem));
		OnRepairStatusChanged?.Invoke(_E14A.Keys.Any() ? new ERepairStatusWarning?(_E14A.Keys.Max()) : null);
		_E000(!_E14A.ContainsKey(ERepairStatusWarning.ExceptionRepairItem));
	}

	private void _E000(bool value)
	{
		_repairerParametersPanel.interactable = value;
		_repairerParametersPanel.alpha = (value ? 1f : 0.3f);
	}

	public void UpdateDegradationPrediction(Vector2 range)
	{
		_E149 = range;
	}
}
