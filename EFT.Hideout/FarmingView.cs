using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class FarmingView : ProduceViewBase<_E81E, _E829>
{
	private const float m__E000 = 0.5f;

	private const string m__E001 = "<color=#c5c3b2>{0}</color>\n({1})";

	private const string m__E002 = "<color=#c5c3b2><b>{0}</b></color>/{1}";

	private static readonly string m__E003 = _ED3E._E000(171762) + EDetailsType.Farming.ToStringNoBox();

	[SerializeField]
	private HideoutItemViewFactory _stashItemIconViewFactory;

	[SerializeField]
	private HideoutItemViewFactory _installedIconViewFactory;

	[SerializeField]
	private HideoutItemViewFactory _resultItemIconViewFactory;

	[SerializeField]
	private GameObject _productionPercentagesPanel;

	[SerializeField]
	private TextMeshProUGUI _productionPercentages;

	[SerializeField]
	private DefaultUIButton _getItemsButton;

	[SerializeField]
	private TextMeshProUGUI _productionStatus;

	[SerializeField]
	private GameObject _statusContainer;

	[SerializeField]
	private Button _setAllButton;

	[SerializeField]
	private Button _setOneButton;

	[SerializeField]
	private Button _removeButton;

	[SerializeField]
	private CanvasGroup[] _addButtonsCanvases;

	[SerializeField]
	private CanvasGroup[] _removeButtonsCanvases;

	[SerializeField]
	private GameObject _loaderPanel;

	[SerializeField]
	private new Dictionary<EProductionState, List<GameObject>> _stateActiveObjects;

	private bool m__E004;

	private DateTime m__E005;

	protected override bool ShowItemsListWindow => false;

	public void Show(ItemUiContext itemUiContext, _EAED inventoryController, _E829 scheme, _E81E producer, Action<string> getProducedItems)
	{
		Show(itemUiContext, inventoryController, scheme, producer, null, getProducedItems);
		_getItemsButton.OnClick.AddListener(base.GetProducedItems);
		_setAllButton.onClick.AddListener(_E002);
		_setOneButton.onClick.AddListener(_E001);
		_removeButton.onClick.AddListener(_E000);
		_restartButton.OnClick.AddListener(delegate
		{
			RestartProducing().HandleExceptions();
		});
		_E63B instance = Singleton<_E63B>.Instance;
		if (!string.IsNullOrEmpty(base.Producer.SlotItemTemplate))
		{
			Item hideoutSchemeItem = instance.GetHideoutSchemeItem(base.Producer.SlotItemTemplate, scheme.areaType.ToString(_ED3E._E000(27314)), EOwnerType.HideoutSlot);
			_stashItemIconViewFactory.Show(hideoutSchemeItem, base.InventoryController, base.ItemUiContext);
			UI.AddDisposable(_stashItemIconViewFactory);
			_installedIconViewFactory.Show(hideoutSchemeItem, base.InventoryController, base.ItemUiContext);
			UI.AddDisposable(_installedIconViewFactory);
		}
		Item hideoutSchemeItem2 = instance.GetHideoutSchemeItem(base.Scheme.endProduct, scheme._id, EOwnerType.HideoutProduction);
		_resultItemIconViewFactory.Show(hideoutSchemeItem2, base.InventoryController, base.ItemUiContext);
		UI.AddDisposable(_resultItemIconViewFactory);
		_E005();
		_E004();
	}

	public override void UpdateView()
	{
		if (!(this == null))
		{
			base.UpdateView();
			_E004();
			_E005();
		}
	}

	private void _E000()
	{
		this.m__E004 = true;
		base.Producer.RemoveSupply(_E003);
		UpdateView();
	}

	private void _E001()
	{
		this.m__E004 = true;
		base.Producer.InstallSupplies(installAll: false, _E003);
		UpdateView();
	}

	private void _E002()
	{
		this.m__E004 = true;
		base.Producer.InstallSupplies(installAll: true, _E003);
		UpdateView();
	}

	private void _E003()
	{
		this.m__E004 = false;
		UpdateView();
	}

	private void _E004()
	{
		_stashItemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(171648), base.Producer.AvailableItemsCount, _ED3E._E000(171681).Localized()));
		_installedIconViewFactory.SetCounterText(string.Format(_ED3E._E000(171743), base.Producer.InstalledSuppliesCount, base.Producer.MaxSuppliesCount));
	}

	private void _E005()
	{
		int itemsCount = base.Producer.CompleteItemsStorage.GetItemsCount(base.Scheme._id);
		int itemsLimit = base.Producer.CompleteItemsStorage.GetItemsLimit(base.Scheme._id);
		bool flag = !base.GettingItems && itemsCount > 0;
		EProductionState productionState = base.Producer.GetProductionState(base.Scheme._id);
		ApplyState(productionState, _stateActiveObjects);
		_getItemsButton.gameObject.SetActive(flag);
		_loaderPanel.SetActive(base.GettingItems);
		bool interactable = base.Producer.CanStart && base.Producer.CanStartByProductionCount && base.Producer.CanStartScheme(base.Scheme);
		_restartButton.Interactable = interactable;
		_resultItemIconViewFactory.SetCounterText(string.Format(_ED3E._E000(171743), itemsCount, itemsLimit));
		_E827 value;
		bool flag2 = base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out value) && base.Producer.IsWorking;
		_productionStatus.gameObject.SetActive(flag2);
		_productionPercentagesPanel.SetActive(flag2);
		if (flag2)
		{
			_E006(value);
		}
		_statusContainer.SetActive(flag || base.GettingItems || flag2);
		bool value2 = !this.m__E004 && base.Producer.AvailableItemsCount > 0 && base.Producer.InstalledSuppliesCount < base.Producer.MaxSuppliesCount;
		bool value3 = !this.m__E004 && base.Producer.InstalledSuppliesCount > 0;
		CanvasGroup[] addButtonsCanvases = _addButtonsCanvases;
		for (int i = 0; i < addButtonsCanvases.Length; i++)
		{
			addButtonsCanvases[i].SetUnlockStatus(value2);
		}
		addButtonsCanvases = _removeButtonsCanvases;
		for (int i = 0; i < addButtonsCanvases.Length; i++)
		{
			addButtonsCanvases[i].SetUnlockStatus(value3);
		}
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (!((utcNow - this.m__E005).TotalSeconds < 0.5))
		{
			this.m__E005 = utcNow;
			if (base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out var value))
			{
				_E006(value);
			}
		}
	}

	private void _E006(_E827 producingItem)
	{
		_productionStatus.SetMonospaceText(FarmingView.m__E003.Localized() + _ED3E._E000(54246) + producingItem.EstimatedTimeInt.ToTimeString() + _ED3E._E000(171765));
		_productionPercentages.SetMonospaceText(string.Format(_ED3E._E000(171594), Mathf.FloorToInt((float)producingItem.Progress * 100f)));
	}

	public override void Close()
	{
		_getItemsButton.OnClick.RemoveListener(base.GetProducedItems);
		_setAllButton.onClick.RemoveListener(_E002);
		_setOneButton.onClick.RemoveListener(_E001);
		_removeButton.onClick.RemoveListener(_E000);
		base.Close();
	}

	[CompilerGenerated]
	private void _E007()
	{
		RestartProducing().HandleExceptions();
	}
}
