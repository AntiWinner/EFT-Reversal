using System;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.ProfileEditor.UI;

public sealed class ProfileEditorRightPanel : UIElement, _EC60
{
	[SerializeField]
	private SimpleStashPanel _simpleStash;

	[SerializeField]
	private Button _nextButton;

	[SerializeField]
	private Button _prevButton;

	[SerializeField]
	private TMP_InputField _searchInput;

	[SerializeField]
	private GameObject _loader;

	private Action<_EA40> _E079;

	private _E63B _E07A;

	private _EA40[] _E07B;

	private int _E07C;

	private _E8D7 _E07D;

	private void Awake()
	{
		_nextButton.onClick.AddListener(_E003);
		_prevButton.onClick.AddListener(_E004);
		_searchInput.onValueChanged.AddListener(_E000);
	}

	private void _E000(string text)
	{
		if (text.Length >= 3)
		{
			_E07D.Search(text).HandleExceptions();
		}
		else if (_E07D.IsSearchActive)
		{
			_E07D.Stop();
		}
	}

	public void Configure(Action<_EA40> configureStashPanel, _E63B itemFactory, _EA40[] lootItems)
	{
		_E07A = itemFactory;
		_E079 = configureStashPanel;
		_E07B = lootItems;
		_E07D = new _E8D7(_E07A, _loader);
		_E07D.SearchComplete += _E002;
		_E07D.SearchStopped += _E001;
	}

	public void Show(_EAED leftSideInventoryController = null, ItemsPanel.EItemsTab tab = ItemsPanel.EItemsTab.Gear)
	{
		ShowGameObject();
		_E07C = 0;
		_E079(_E07B[_E07C]);
		_simpleStash.Show();
	}

	private void _E001()
	{
		_EA40 lootItem = _E07B[_E07C];
		_E006(lootItem);
	}

	private void _E002(_EA40 lootItem)
	{
		_E006(lootItem);
	}

	private void Update()
	{
		if (!_searchInput.isFocused && _E07B.Length > 1)
		{
			int step = (Input.GetKeyDown(KeyCode.C) ? 1 : (Input.GetKeyDown(KeyCode.X) ? (-1) : 0));
			_E005(step);
		}
	}

	private void _E003()
	{
		_E005(1);
	}

	private void _E004()
	{
		_E005(-1);
	}

	private void _E005(int step)
	{
		if (step != 0)
		{
			_E07D.Stop(silent: true);
			_searchInput.text = string.Empty;
			_E07C = (_E07C + step + _E07B.Length) % _E07B.Length;
			_EA40 lootItem = _E07B[_E07C];
			_E006(lootItem);
		}
	}

	private void _E006(_EA40 lootItem)
	{
		if (_simpleStash.gameObject.activeSelf)
		{
			_simpleStash.Close();
		}
		_E079(lootItem);
		_simpleStash.Show();
	}

	public override void Close()
	{
		_simpleStash.Close();
		base.Close();
	}
}
