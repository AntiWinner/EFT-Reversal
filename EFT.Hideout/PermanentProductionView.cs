using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;

namespace EFT.Hideout;

public sealed class PermanentProductionView : ProduceViewBase<_E824, _E829>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FuelSelectionCell selectionView;

		internal void _E000()
		{
			selectionView.Close();
			UnityEngine.Object.Destroy(selectionView.gameObject);
		}
	}

	private const string m__E000 = "<color=#c5c3b2><b>{0}</b></color>/{1}";

	[SerializeField]
	private HideoutItemViewFactory _resultItemIconView;

	[SerializeField]
	private GameObject _productionPercentagesPanel;

	[SerializeField]
	private TextMeshProUGUI _productionPercentages;

	[SerializeField]
	private DefaultUIButton _getItemsButton;

	[SerializeField]
	private TextMeshProUGUI _productionStatus;

	[SerializeField]
	private FuelSelectionCell _resourceContainerTemplate;

	[SerializeField]
	private Transform _resourceCellsContainer;

	[SerializeField]
	private GameObject _loaderPanel;

	private FuelSelectionCell[] m__E001;

	protected override bool ShowItemsListWindow => false;

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, _E829 scheme, _E824 producer, Action<string> getItems)
	{
		Show(itemUiContext, inventoryController, scheme, producer, null, getItems);
		_getItemsButton.OnClick.AddListener(base.GetProducedItems);
		_restartButton.OnClick.AddListener(delegate
		{
			RestartProducing().HandleExceptions();
		});
		Item hideoutSchemeItem = Singleton<_E63B>.Instance.GetHideoutSchemeItem(base.Scheme.endProduct, scheme._id, EOwnerType.HideoutProduction);
		_resultItemIconView.Show(hideoutSchemeItem, base.InventoryController, base.ItemUiContext);
		UI.AddDisposable(_resultItemIconView);
		this.m__E001 = new FuelSelectionCell[base.Producer.UsingItems.Length];
		base.Producer.OnConsumableItemChanged += _E002;
		for (int i = 0; i < base.Producer.UsingItems.Length; i++)
		{
			_EA1E currentSelectedItem = base.Producer.UsingItems[i];
			FuelSelectionCell selectionView = UnityEngine.Object.Instantiate(_resourceContainerTemplate, _resourceCellsContainer);
			selectionView.Show(currentSelectedItem, base.Producer, _E000);
			this.m__E001[i] = selectionView;
			UI.AddDisposable(delegate
			{
				selectionView.Close();
				UnityEngine.Object.Destroy(selectionView.gameObject);
			});
		}
		UpdateView();
	}

	private bool _E000(ItemSelectionCell sender, Item selectedItem, bool selected)
	{
		if (!selected)
		{
			return true;
		}
		int num = Array.IndexOf(this.m__E001, sender as FuelSelectionCell);
		if (num < 0)
		{
			return false;
		}
		bool num2 = base.Producer.InstallSupplySlot(selectedItem, num);
		if (num2)
		{
			UpdateView();
		}
		return num2;
	}

	public override void UpdateView()
	{
		if (this == null)
		{
			return;
		}
		base.UpdateView();
		bool isWorking = base.Producer.IsWorking;
		_productionStatus.gameObject.SetActive(isWorking);
		_productionPercentagesPanel.gameObject.SetActive(isWorking);
		int itemsCount = base.Producer.CompleteItemsStorage.GetItemsCount(base.Scheme._id);
		_loaderPanel.SetActive(base.GettingItems);
		_getItemsButton.gameObject.SetActive(!base.GettingItems && itemsCount > 0);
		_resultItemIconView.ShowInfo(base.Scheme.productionLimitCount > 1, showFulfilledStatus: false);
		if (base.Scheme.productionLimitCount > 1)
		{
			_resultItemIconView.SetCounterText(string.Format(_ED3E._E000(171743), itemsCount, base.Scheme.productionLimitCount));
		}
		bool interactable = base.Producer.CanStart && base.Producer.CanStartByProductionCount;
		_restartButton.Interactable = interactable;
		if (this.m__E001 != null)
		{
			FuelSelectionCell[] array = this.m__E001;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetProductionState(base.Producer.GetProductionState(base.Scheme._id));
			}
		}
	}

	private void Update()
	{
		if (base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out var value))
		{
			_E001(value);
		}
	}

	private void _E001(_E827 producingItem)
	{
		_productionStatus.SetMonospaceText((_ED3E._E000(171762) + EDetailsType.Producing).Localized() + _ED3E._E000(54246) + producingItem.EstimatedTimeInt.ToTimeString() + _ED3E._E000(171765));
		_productionPercentages.SetMonospaceText(string.Format(_ED3E._E000(171594), Mathf.FloorToInt((float)producingItem.Progress * 100f)));
	}

	private void _E002(Item item, int index)
	{
		this.m__E001[index].SetItem(item);
	}

	public override void Close()
	{
		this.m__E001 = null;
		_getItemsButton.OnClick.RemoveListener(base.GetProducedItems);
		base.Producer.OnConsumableItemChanged -= _E002;
		base.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		RestartProducing().HandleExceptions();
	}
}
