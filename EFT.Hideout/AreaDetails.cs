using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public class AreaDetails : UIElement
{
	[Serializable]
	public class DetailsDisplaySettings
	{
		public Sprite Icon;

		public Color BackgroundColor;
	}

	[SerializeField]
	private DetailsPanel _importantDetailsPanel;

	[SerializeField]
	private DetailsPanel _regularDetailsPanelTemplate;

	[SerializeField]
	private Transform _detailsContainer;

	[SerializeField]
	private Dictionary<EDetailsType, DetailsDisplaySettings> _detailsSettings;

	private _E835 _E034;

	private _E836 _E035;

	private bool _E036;

	private bool _E037;

	private AreaData _E038;

	private Action _E039;

	private readonly Dictionary<_E835, DetailsPanel> _E03A = new Dictionary<_E835, DetailsPanel>();

	private bool _E000
	{
		get
		{
			if (_E034.IsActive)
			{
				if (_E034.Type != 0 || !_E036)
				{
					if (_E034.Type == EDetailsType.NoFuel || _E034.Type == EDetailsType.SwitchedOff)
					{
						return _E037;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public void Show(AreaData data)
	{
		_E038 = data;
		_E831 areaBehaviour = _E038.Template.AreaBehaviour;
		_E034 = GeneratorBehaviour.FuelDetails;
		_E035 = areaBehaviour.DetailsGroup;
		_E000();
		_E001();
	}

	private void _E000()
	{
		if (_E034 == null)
		{
			return;
		}
		_E036 = false;
		_E037 = false;
		if (_E038.DisplayOutOfFuelIcon)
		{
			if (_E038.CurrentLevel == 0)
			{
				_E039 = _E038.LevelUpdated.Bind(_E003);
				return;
			}
			_E004();
		}
		bool flag = this._E000;
		_E008(_importantDetailsPanel, flag);
		if (flag)
		{
			_E009(_importantDetailsPanel, _E034, default(_E834), force: true);
		}
		_E034.OnChanged += _E002;
	}

	private void _E001()
	{
		if (_E035 == null || _E035.Count <= 0)
		{
			return;
		}
		foreach (_E835 item in _E035)
		{
			_E005(item);
		}
		_E035.OnDetailsChanged += _E007;
	}

	private void _E002(_E835 datails, _E834 oldData)
	{
		bool flag = this._E000;
		bool flag2 = _importantDetailsPanel.gameObject.activeSelf ^ flag;
		if (flag2)
		{
			_E008(_importantDetailsPanel, flag);
		}
		if (flag)
		{
			_E009(_importantDetailsPanel, _E034, oldData, flag2);
		}
	}

	private void _E003()
	{
		if (_E038.CurrentLevel > 0)
		{
			_E004();
			_E002(_E034, default(_E834));
			_E039();
		}
	}

	private void _E004()
	{
		_ = Singleton<_E80F>.Instance;
		_E036 = _E038.Template.Type == EAreaType.Generator;
		_E037 = true;
	}

	private void _E005(_E835 detailsData)
	{
		DetailsPanel detailsPanel = UnityEngine.Object.Instantiate(_regularDetailsPanelTemplate, _detailsContainer, worldPositionStays: false);
		_E03A.Add(detailsData, detailsPanel);
		_E006(detailsPanel, detailsData, default(_E834), force: true);
	}

	private void _E006(DetailsPanel view, _E835 datails, _E834 oldData, bool force)
	{
		if (!(view == null))
		{
			bool flag = force || (datails.IsActive ^ oldData.IsActive);
			if (flag)
			{
				_E008(view, datails.IsActive);
			}
			if (datails.IsActive)
			{
				_E009(view, datails, oldData, flag);
			}
		}
	}

	private void _E007(_E836 group, _E835 detailsData, _E834 oldData, EDetailsChangeType changeType)
	{
		switch (changeType)
		{
		case EDetailsChangeType.Added:
			_E005(detailsData);
			break;
		case EDetailsChangeType.Removed:
		{
			if (_E03A.TryGetValue(detailsData, out var value2))
			{
				if (value2 != null)
				{
					UnityEngine.Object.Destroy(value2);
				}
				_E03A.Remove(detailsData);
			}
			break;
		}
		case EDetailsChangeType.Changed:
		{
			if (_E03A.TryGetValue(detailsData, out var value))
			{
				_E006(value, detailsData, oldData, force: false);
			}
			break;
		}
		}
	}

	private void _E008(DetailsPanel view, bool value)
	{
		view.gameObject.SetActive(value);
	}

	private void _E009(DetailsPanel detailsPanel, _E835 detailsData, _E834 oldData, bool force)
	{
		if (force || detailsData.Type != oldData.Type)
		{
			if (_detailsSettings.TryGetValue(detailsData.Type, out var value))
			{
				detailsPanel.SetIcon(value.Icon);
				detailsPanel.SetBackgroundColor(value.BackgroundColor);
			}
			force = true;
		}
		if (force || detailsData.Percentages != oldData.Percentages || detailsData.Time != oldData.Value)
		{
			detailsPanel.SetText(detailsData.Text);
		}
	}

	public override void Close()
	{
		_E039?.Invoke();
		_importantDetailsPanel.HideGameObject();
		if (_E034 != null)
		{
			_E034.OnChanged -= _E002;
		}
		foreach (var (_, detailsPanel2) in _E03A)
		{
			if (detailsPanel2 != null)
			{
				UnityEngine.Object.Destroy(detailsPanel2.gameObject);
			}
		}
		_E03A.Clear();
		if (_E035 != null)
		{
			_E035.OnDetailsChanged -= _E007;
			_E035 = null;
		}
	}
}
