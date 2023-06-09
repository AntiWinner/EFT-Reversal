using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

[UsedImplicitly]
public sealed class HideoutItemViewFactory : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _counterField;

	[SerializeField]
	private GameObject _counterContainer;

	[SerializeField]
	private RequirementFulfilledStatus _fulfilledStatus;

	[SerializeField]
	private GameObject _fulfilledStatusContainer;

	[SerializeField]
	private HorizontalOrVerticalLayoutGroup _horizontalLayout;

	[SerializeField]
	private HorizontalOrVerticalLayoutGroup _verticalLayout;

	[SerializeField]
	private Transform _itemViewContainer;

	[SerializeField]
	private GameObject _infoContainer;

	[SerializeField]
	private bool _showCounter;

	[SerializeField]
	private bool _showFulfilledStatus;

	[SerializeField]
	private GridLayoutGroup.Axis _layout;

	[SerializeField]
	private HoverTooltipArea _hoverTooltipArea;

	[SerializeField]
	private GameObject _errorBlock;

	[CompilerGenerated]
	private ItemView _E019;

	private ItemView _E000
	{
		[CompilerGenerated]
		get
		{
			return _E019;
		}
		[CompilerGenerated]
		set
		{
			_E019 = value;
		}
	}

	public bool FulfilledStatus
	{
		set
		{
			_fulfilledStatus.Show(value);
		}
	}

	public void Show(Item item, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		Close();
		_EB64 obj = new _EB64(item, EItemViewType.Hideout);
		UI.AddDisposable(obj.CloseDependentWindows);
		UI.AddDisposable(obj);
		this._E000 = itemUiContext.CreateItemView(item, obj, ItemRotation.Horizontal, inventoryController, null, null, null, slotView: false, canSelect: false, searched: true);
		Transform obj2 = this._E000.transform;
		obj2.localPosition = Vector3.zero;
		obj2.rotation = Quaternion.identity;
		obj2.localScale = Vector3.one;
		obj2.SetParent(_itemViewContainer, worldPositionStays: false);
		obj2.SetAsFirstSibling();
		ShowInfo(_showCounter, _showFulfilledStatus);
		SetError(null);
		_E000(_layout);
		ShowGameObject();
	}

	public void SetBorderColor(in Color color)
	{
		this._E000.BorderColor = color;
	}

	public void SetCounterText(string text)
	{
		_counterField.text = text;
	}

	public void ShowInfo(bool showCounter, bool showFulfilledStatus)
	{
		_counterContainer.gameObject.SetActive(showCounter);
		_fulfilledStatusContainer.gameObject.SetActive(showFulfilledStatus);
	}

	public void SetError(InventoryError error)
	{
		_hoverTooltipArea.enabled = error != null;
		_errorBlock.SetActive(error != null);
		if (error != null)
		{
			_hoverTooltipArea.SetMessageText(string.Format(_ED3E._E000(59958), error.GetLocalizedDescription()));
		}
	}

	private void _E000(GridLayoutGroup.Axis layoutType)
	{
		HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = null;
		HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup2 = null;
		Object.DestroyImmediate(_infoContainer.GetComponent<HorizontalOrVerticalLayoutGroup>());
		switch (layoutType)
		{
		case GridLayoutGroup.Axis.Horizontal:
			horizontalOrVerticalLayoutGroup = _infoContainer.AddComponent<HorizontalLayoutGroup>();
			horizontalOrVerticalLayoutGroup2 = _horizontalLayout;
			break;
		case GridLayoutGroup.Axis.Vertical:
			horizontalOrVerticalLayoutGroup = _infoContainer.AddComponent<VerticalLayoutGroup>();
			horizontalOrVerticalLayoutGroup2 = _verticalLayout;
			break;
		}
		horizontalOrVerticalLayoutGroup.padding = horizontalOrVerticalLayoutGroup2.padding;
		horizontalOrVerticalLayoutGroup.spacing = horizontalOrVerticalLayoutGroup2.spacing;
		horizontalOrVerticalLayoutGroup.childControlWidth = horizontalOrVerticalLayoutGroup2.childControlWidth;
		horizontalOrVerticalLayoutGroup.childControlHeight = horizontalOrVerticalLayoutGroup2.childControlHeight;
		horizontalOrVerticalLayoutGroup.childForceExpandWidth = horizontalOrVerticalLayoutGroup2.childForceExpandWidth;
		horizontalOrVerticalLayoutGroup.childForceExpandHeight = horizontalOrVerticalLayoutGroup2.childForceExpandHeight;
		horizontalOrVerticalLayoutGroup.childAlignment = horizontalOrVerticalLayoutGroup2.childAlignment;
	}

	public override void Close()
	{
		base.Close();
		if (this._E000 != null)
		{
			this._E000.Kill();
			this._E000 = null;
		}
	}
}
