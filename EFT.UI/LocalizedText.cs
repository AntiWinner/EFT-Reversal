using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

[UsedImplicitly]
public sealed class LocalizedText : UIElement
{
	[SerializeField]
	private string localizationKey;

	[SerializeField]
	private List<TextMeshProUGUI> _labels = new List<TextMeshProUGUI>();

	[SerializeField]
	private EStringCase _stringCase;

	private string _E0CC;

	private object[] _E0CD;

	private Action _E0BC;

	[CompilerGenerated]
	private string _E0CE;

	public string FormattedText
	{
		[CompilerGenerated]
		get
		{
			return _E0CE;
		}
		[CompilerGenerated]
		private set
		{
			_E0CE = value;
		}
	}

	public string LocalizationKey
	{
		get
		{
			return localizationKey;
		}
		set
		{
			localizationKey = value;
			_E001();
		}
	}

	private void OnEnable()
	{
		_E000();
		if (_E0BC == null)
		{
			_E001();
			_E0BC = _E7AD._E010.AddLocaleUpdateListener(_E001);
		}
	}

	private void OnDisable()
	{
		_E0BC?.Invoke();
		_E0BC = null;
	}

	private void _E000()
	{
		if (_labels.Count > 0)
		{
			return;
		}
		TextMeshProUGUI component = base.gameObject.GetComponent<TextMeshProUGUI>();
		if (component == null)
		{
			Debug.LogError(_ED3E._E000(250773));
			return;
		}
		if (!_labels.Contains(component))
		{
			_labels.Add(component);
		}
		if (string.IsNullOrEmpty(localizationKey))
		{
			localizationKey = component.text;
		}
		_E001();
	}

	internal void _E001()
	{
		_E0CC = localizationKey.Localized(_stringCase);
		if (!string.IsNullOrEmpty(_E0CC))
		{
			if (_E0CD != null)
			{
				SetFormatValues(_E0CD);
			}
			else
			{
				_E002(_E0CC);
			}
		}
	}

	private void _E002(string text)
	{
		FormattedText = text;
		foreach (TextMeshProUGUI label in _labels)
		{
			if (!(label == null))
			{
				label.text = text;
				label.SetAllDirty();
			}
		}
	}

	public void SetFormatValues(params object[] values)
	{
		_E0CD = values;
		if (!string.IsNullOrEmpty(_E0CC))
		{
			_E002(string.Format(_E0CC, values));
		}
		else
		{
			Debug.LogWarning(_ED3E._E000(250797), this);
		}
	}

	private void OnDestroy()
	{
		Dispose();
	}
}
