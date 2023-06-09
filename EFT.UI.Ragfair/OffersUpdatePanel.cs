using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class OffersUpdatePanel : UIElement
{
	public static readonly ReadOnlyCollection<int> LinesCountVariants = Array.AsReadOnly(new int[4] { 15, 30, 50, 100 });

	[SerializeField]
	private DefaultUIButton _updateButton;

	[SerializeField]
	private DropDownBox _pagesCountDropdown;

	[SerializeField]
	private PageButton _pageButton;

	[SerializeField]
	private RectTransform _buttonsContainer;

	[SerializeField]
	private Button _previousPageButton;

	[SerializeField]
	private Button _nextPageButton;

	[SerializeField]
	private Button _firstPageButton;

	[SerializeField]
	private Button _lastPageButton;

	private readonly List<PageButton> _E329 = new List<PageButton>();

	private int _E373;

	private int _E374;

	private Action _E375;

	private Action<int> _E376;

	private bool _E210;

	private void Awake()
	{
		_E000();
	}

	public void Show(int onePageCount, int currentPage, int pageCount, Action onUpdate, Action<int> onPageClicked)
	{
		_E000();
		_E373 = currentPage;
		_E374 = pageCount;
		_E375 = onUpdate;
		_E376 = onPageClicked;
		ShowGameObject();
		_E001(currentPage);
		_E7EF settings = Singleton<_E7DE>.Instance.Game.Settings;
		SettingDropDown.BindDropDownToSetting(_pagesCountDropdown, settings.RagfairLinesCount, LinesCountVariants, (int x) => x.ToString());
		UI.SubscribeState(settings.RagfairLinesCount, delegate
		{
			Singleton<_E7DE>.Instance.Game.Save().HandleExceptions();
		});
		UI.AddDisposable(_pagesCountDropdown.Hide);
	}

	private void _E000()
	{
		if (!_E210)
		{
			_E210 = true;
			_updateButton.OnClick.AddListener(delegate
			{
				_E375?.Invoke();
			});
			_previousPageButton.onClick.AddListener(delegate
			{
				_E376(_E373 - 1);
			});
			_nextPageButton.onClick.AddListener(delegate
			{
				_E376(_E373 + 1);
			});
			_firstPageButton.onClick.AddListener(delegate
			{
				_E376(0);
			});
			_lastPageButton.onClick.AddListener(delegate
			{
				_E376(_E374 - 1);
			});
			for (int i = 0; i < 5; i++)
			{
				_E329.Add(UnityEngine.Object.Instantiate(_pageButton, _buttonsContainer));
			}
		}
	}

	private void _E001(int currentPage)
	{
		for (int i = 0; i < _E329.Count; i++)
		{
			PageButton pageButton = _E329[i];
			int num = i + (currentPage - 2);
			if (num < 0 || num >= _E374)
			{
				pageButton.HideGameObject();
			}
			else
			{
				pageButton.Show(currentPage == num, num, _E376);
			}
		}
		_previousPageButton.gameObject.SetActive(currentPage > 0);
		_nextPageButton.gameObject.SetActive(currentPage < _E374 - 1 && _E374 > 1);
		_firstPageButton.gameObject.SetActive(currentPage > 1);
		_lastPageButton.gameObject.SetActive(currentPage < _E374 - 2);
		_updateButton.gameObject.SetActive(currentPage < _E374 - 1);
		_pagesCountDropdown.gameObject.SetActive(_E374 > 1);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E375?.Invoke();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E376(_E373 - 1);
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E376(_E373 + 1);
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E376(0);
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E376(_E374 - 1);
	}
}
