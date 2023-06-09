using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class SearchButton : Button
{
	private const string m__E000 = "SEARCHING...";

	private const string m__E001 = "SEARCH";

	[SerializeField]
	private CustomTextMeshProUGUI _buttonCaption;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _searchIcon;

	[SerializeField]
	private Button _stopSearchButton;

	private bool _E002;

	private Action<bool> _E003;

	protected override void Awake()
	{
		base.Awake();
		base.onClick.AddListener(delegate
		{
			_E003(!_E002);
		});
		if (_stopSearchButton != null)
		{
			_stopSearchButton.onClick.AddListener(delegate
			{
				_E003(obj: false);
			});
		}
	}

	public void Show(Action<bool> onSearchStatusChanged)
	{
		if (!(this == null))
		{
			_E003 = onSearchStatusChanged;
			SetSearchStatus(active: false);
		}
	}

	public void SetEnabled(bool value)
	{
		if (!(this == null))
		{
			base.interactable = value;
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (!(component == null))
			{
				component.SetUnlockStatus(value);
			}
		}
	}

	public void SetSearchStatus(bool active, bool changeButtonEnabling = true)
	{
		if (!(this == null))
		{
			_loader.SetActive(active);
			_searchIcon.SetActive(!active);
			_buttonCaption.text = (active ? _ED3E._E000(253670).Localized().ToUpper() : _ED3E._E000(253679).Localized().ToUpper());
			if (_stopSearchButton != null)
			{
				_stopSearchButton.gameObject.SetActive(active);
			}
			if (changeButtonEnabling)
			{
				base.interactable = !active;
			}
			_E002 = active;
		}
	}

	public void Close()
	{
		if (!(this == null))
		{
			if (_stopSearchButton != null)
			{
				_stopSearchButton.gameObject.SetActive(value: false);
			}
			_loader.SetActive(value: false);
			_searchIcon.SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E003(!_E002);
	}

	[CompilerGenerated]
	private void _E001()
	{
		_E003(obj: false);
	}
}
