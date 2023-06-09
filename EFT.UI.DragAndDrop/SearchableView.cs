using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class SearchableView : MonoBehaviour
{
	[SerializeField]
	private SearchButton _searchButton;

	[SerializeField]
	private TimerText _searchTimer;

	[SerializeField]
	private Button _unsearchedPanel;

	[SerializeField]
	private GameObject _unsearchedButtonLoader;

	private _EAED m__E000;

	private bool m__E001;

	private _EA91 m__E002;

	private Action m__E003;

	private readonly _E3A4 m__E004 = new _E3A4();

	private readonly _ECF5<_EB94> m__E005 = new _ECF5<_EB94>();

	private readonly _ECF5<bool> m__E006 = new _ECF5<bool>();

	private void Start()
	{
		_unsearchedPanel.onClick.AddListener(delegate
		{
			this.m__E000.SearchContents(this.m__E002);
		});
	}

	public void Configure(_EAED inventoryController, bool searchButtonDisplay)
	{
		this.m__E000 = inventoryController;
		this.m__E001 = searchButtonDisplay;
	}

	public void Show(_EA91 item, Action showGrids)
	{
		this.m__E002 = item;
		this.m__E003 = showGrids;
		_ECF5<SearchedState> searchState2 = this.m__E000.GetSearchState(this.m__E002);
		this.m__E004.BindState(this.m__E000.SearchOperations.CountChanged, delegate
		{
			this.m__E005.Value = this.m__E000.SearchOperations.FirstOrDefault((_EB94 x) => x._E016 == this.m__E002);
			this.m__E006.Value = this.m__E000.CanStartNewSearchOperation();
		});
		this.m__E004.BindState(searchState2, delegate(SearchedState searchState)
		{
			_searchButton.gameObject.SetActive(searchState != SearchedState.FullySearched && this.m__E001);
			_unsearchedPanel.gameObject.SetActive(searchState == SearchedState.Unsearched);
			if (searchState != 0)
			{
				this.m__E003();
			}
		});
		this.m__E004.BindState(this.m__E005, delegate(_EB94 currentOperation)
		{
			if (_searchTimer != null)
			{
				if (currentOperation != null)
				{
					_searchTimer.Show(currentOperation.StartTime);
				}
				else
				{
					_searchTimer.Hide();
				}
			}
			_searchButton.SetSearchStatus(currentOperation != null);
		});
		this.m__E004.AddDisposable(_ECF3.Combine(this.m__E005, searchState2, (_EB94 operation, SearchedState state) => operation != null && state == SearchedState.Unsearched).Bind(_unsearchedButtonLoader.SetActive));
		this.m__E004.AddDisposable(_ECF3.Combine(this.m__E005, this.m__E006, (_EB94 currentOperation, bool canStartNewOperation) => new _E050<_EB94, bool>(currentOperation, canStartNewOperation)).Bind(delegate(_E050<_EB94, bool> x)
		{
			bool flag = x.currentOperation != null && x.currentOperation._E016 == this.m__E002;
			_searchButton.SetEnabled(flag || x.canStartNewOperation);
			_unsearchedPanel.enabled = !flag && x.canStartNewOperation;
			_unsearchedPanel.interactable = !flag && x.canStartNewOperation;
		}));
		_searchButton.Show(delegate(bool search)
		{
			if (search)
			{
				this.m__E000.SearchContents(this.m__E002);
			}
			else
			{
				this.m__E000.StopSearching(this.m__E002);
			}
		});
	}

	public void ShowContents()
	{
		if (this.m__E000.GetSearchState(this.m__E002).Value != 0)
		{
			this.m__E003();
		}
	}

	public void Close()
	{
		this.m__E004.Dispose();
		_unsearchedPanel.gameObject.SetActive(value: false);
		_searchButton.Close();
		if (_searchTimer != null)
		{
			_searchTimer.gameObject.SetActive(value: false);
		}
		this.m__E000?.StopSearching(this.m__E002);
		this.m__E000 = null;
		this.m__E003 = null;
		this.m__E002 = null;
	}

	[CompilerGenerated]
	private void _E000()
	{
		this.m__E000.SearchContents(this.m__E002);
	}

	[CompilerGenerated]
	private void _E001(int _)
	{
		this.m__E005.Value = this.m__E000.SearchOperations.FirstOrDefault((_EB94 x) => x._E016 == this.m__E002);
		this.m__E006.Value = this.m__E000.CanStartNewSearchOperation();
	}

	[CompilerGenerated]
	private bool _E002(_EB94 x)
	{
		return x._E016 == this.m__E002;
	}

	[CompilerGenerated]
	private void _E003(SearchedState searchState)
	{
		_searchButton.gameObject.SetActive(searchState != SearchedState.FullySearched && this.m__E001);
		_unsearchedPanel.gameObject.SetActive(searchState == SearchedState.Unsearched);
		if (searchState != 0)
		{
			this.m__E003();
		}
	}

	[CompilerGenerated]
	private void _E004(_EB94 currentOperation)
	{
		if (_searchTimer != null)
		{
			if (currentOperation != null)
			{
				_searchTimer.Show(currentOperation.StartTime);
			}
			else
			{
				_searchTimer.Hide();
			}
		}
		_searchButton.SetSearchStatus(currentOperation != null);
	}

	[CompilerGenerated]
	private void _E005(_E050<_EB94, bool> x)
	{
		bool flag = x.currentOperation != null && x.currentOperation._E016 == this.m__E002;
		_searchButton.SetEnabled(flag || x.canStartNewOperation);
		_unsearchedPanel.enabled = !flag && x.canStartNewOperation;
		_unsearchedPanel.interactable = !flag && x.canStartNewOperation;
	}

	[CompilerGenerated]
	private void _E006(bool search)
	{
		if (search)
		{
			this.m__E000.SearchContents(this.m__E002);
		}
		else
		{
			this.m__E000.StopSearching(this.m__E002);
		}
	}
}
