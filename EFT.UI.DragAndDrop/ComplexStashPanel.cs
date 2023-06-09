using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class ComplexStashPanel : UIElement, _EC60
{
	[SerializeField]
	private Image _gridIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _complexGridName;

	[SerializeField]
	private Transform _lootPanel;

	[SerializeField]
	private ContainersPanel _containersPanel;

	[SerializeField]
	private EquipmentTab _equipmentPanelSource;

	[SerializeField]
	private GameObject _complexPanel;

	[SerializeField]
	private GameObject _containerNamePanel;

	[SerializeField]
	private CustomTextMeshProUGUI _containerName;

	private EquipmentTab _E2B8;

	private _EAED _E092;

	private _EB0B _E18C;

	private ItemUiContext _E089;

	private _E74F _E135;

	private _ECB1 _E18F;

	private _EB63<_EB0B> _E2B9;

	public void Configure(_EAED inventoryController, _EB63<_EB0B> equipmentContext, _E74F skills, _ECB1 insurance, ItemUiContext itemUiContext)
	{
		_E092 = inventoryController;
		_E18C = equipmentContext.CastItem;
		_E2B9 = equipmentContext;
		_E135 = skills;
		_E18F = insurance;
		_E089 = itemUiContext;
	}

	public void Show(_EAED leftSideInventoryController = null, ItemsPanel.EItemsTab tab = ItemsPanel.EItemsTab.Gear)
	{
		ShowGameObject();
		IItemOwner owner = _E18C.Parent.GetOwner();
		_gridIcon.gameObject.SetActive(value: true);
		_containerName.text = owner.ContainerName;
		_containerNamePanel.SetActive(owner != _E092);
		_complexPanel.SetActive(value: true);
		_containersPanel.Show(_E2B9, _E092, _E135, _E18F);
		_E2B8 = Object.Instantiate(_equipmentPanelSource, _lootPanel, worldPositionStays: false);
		_E2B8.transform.SetAsFirstSibling();
		_E2B8.Show(_E2B9, _E092, _E135, _E18F);
		_E089.RegisterView(_E2B9);
		UI.AddDisposable(delegate
		{
			_E089.UnregisterView(_E2B9);
		});
		_complexGridName.text = _ED3E._E000(258161).Localized();
	}

	public override void Close()
	{
		if (_complexPanel.activeSelf)
		{
			_complexPanel.SetActive(value: false);
		}
		if (_E2B8 != null)
		{
			_E2B8.Hide();
			Object.DestroyImmediate(_E2B8.gameObject);
			_E2B8 = null;
		}
		_containersPanel.Close();
		base.Close();
	}

	public void UnConfigure()
	{
		_E092 = null;
		_E18C = null;
		_E2B9 = null;
		_E135 = null;
		_E18F = null;
		_E089 = null;
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E089.UnregisterView(_E2B9);
	}
}
