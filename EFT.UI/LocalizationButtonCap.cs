using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class LocalizationButtonCap : UIElement
{
	[SerializeField]
	private UIAnimatedToggleSpawner _toggleSpawner;

	[SerializeField]
	private GameObject _loader;

	private Action<bool> _E06D;

	[CompilerGenerated]
	private CanvasGroup _E16B;

	public CanvasGroup CanvasGroup
	{
		[CompilerGenerated]
		get
		{
			return _E16B;
		}
		[CompilerGenerated]
		private set
		{
			_E16B = value;
		}
	}

	private void Awake()
	{
		_toggleSpawner.SpawnedObject.onValueChanged.AddListener(_E000);
		CanvasGroup = base.gameObject.GetOrAddComponent<CanvasGroup>();
	}

	private void _E000(bool value)
	{
		if (!_loader.activeSelf)
		{
			_E06D?.Invoke(value);
		}
	}

	public void Show(Action<bool> onClick)
	{
		_E06D = onClick;
		SetLoadedStatus(string.Empty, value: false);
	}

	public void SetLoadedStatus(string text, bool value)
	{
		_loader.SetActive(!value);
		_toggleSpawner.gameObject.SetActive(value);
		_toggleSpawner._E02E(text, 36);
	}

	public void SetToggle(bool value)
	{
		_toggleSpawner.SpawnedObject._E001 = value;
	}
}
