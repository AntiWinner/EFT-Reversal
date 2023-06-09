using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class GridWindow : Window<_EC7C>, IPointerClickHandler, IEventSystemHandler, _E641, _E63F
{
	[SerializeField]
	private SearchableView _searchableView;

	[SerializeField]
	private ContainedGridsView _containedGridsTemplate;

	[SerializeField]
	private TextMeshProUGUI _tagName;

	[SerializeField]
	private GridSortPanel _sortPanel;

	private ContainedGridsView m__E000;

	private _EAED m__E001;

	private _EB68 m__E002;

	private _EA40 m__E003;

	private IItemOwner m__E004;

	private ItemUiContext _E005;

	private Action _E006;

	private Item[] _E007;

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E006?.Invoke();
	}

	public void Show(_EA40 compoundItem, _EB68 itemContext, _EAED inventoryController, Action onSelected, Action onClosed, ItemUiContext itemUiContext, bool searchButtonDisplay)
	{
		Show(onClosed);
		_E006 = onSelected;
		this.m__E003 = compoundItem;
		this.m__E002 = itemContext.CreateChild(compoundItem);
		UI.AddDisposable(this.m__E002);
		base.Caption.text = this.m__E003.ShortName.Localized();
		this.m__E001 = inventoryController;
		_E005 = itemUiContext;
		this.m__E004 = this.m__E003.Parent.GetOwner();
		this.m__E004.RegisterView(this);
		_E007 = this.m__E003.GetAllParentItemsAndSelf().ToArray();
		this.m__E000 = ContainedGridsView.CreateGrids(this.m__E003, _containedGridsTemplate);
		this.m__E000.transform.SetParent(base.transform, worldPositionStays: false);
		if (this.m__E003 is _EA91 item && this.m__E001 != null && _searchableView != null)
		{
			_searchableView.Configure(this.m__E001, searchButtonDisplay);
			_searchableView.Show(item, _E002);
		}
		_E001();
		_E003();
		if (_sortPanel != null)
		{
			if (this.m__E001.IsAllowedToSort(this.m__E003) && itemUiContext.SortAvailable)
			{
				_sortPanel.gameObject.SetActive(value: true);
				_sortPanel.Show(this.m__E001, this.m__E003);
			}
			else
			{
				_sortPanel.gameObject.SetActive(value: false);
			}
		}
		this.m__E002.OnCloseWindow += Close;
		this.WaitOneFrame(delegate
		{
			CorrectPosition();
		});
	}

	private void _E000()
	{
		CorrectPosition();
	}

	private void _E001()
	{
		if (this.m__E003 is _EA91)
		{
			_searchableView.ShowContents();
		}
		else
		{
			_E002();
		}
	}

	private void _E002()
	{
		if (!this.m__E000.gameObject.activeSelf)
		{
			this.m__E000.Show(this.m__E003, this.m__E002, this.m__E001, null, _E005);
		}
	}

	private void _E003()
	{
		if (!(_tagName == null))
		{
			TagComponent itemComponent = this.m__E003.GetItemComponent<TagComponent>();
			if (itemComponent != null && !string.IsNullOrEmpty(itemComponent.Name))
			{
				_tagName.gameObject.SetActive(value: true);
				_tagName.text = itemComponent.Name;
			}
			else
			{
				_tagName.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnItemRemoved(_EAF3 removeItemEventArgs)
	{
		if (removeItemEventArgs.Status == CommandStatus.Succeed && _E007.Contains(removeItemEventArgs.Item))
		{
			Close();
		}
	}

	public override void Close()
	{
		this.m__E002.OnCloseWindow -= Close;
		this.m__E004.UnregisterView(this);
		if (this.m__E003 is _EA91 && this.m__E001 != null && _searchableView != null)
		{
			_searchableView.Close();
		}
		if (this.m__E000 != null)
		{
			if (this.m__E000.isActiveAndEnabled)
			{
				this.m__E000.Hide();
			}
			UnityEngine.Object.DestroyImmediate(this.m__E000.gameObject);
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E004()
	{
		CorrectPosition();
	}
}
