using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Insurance;

public sealed class ItemToInsurePanel : UIElement, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemToInsurePanel _003C_003E4__this;

		public ItemToInsurePanel panelTemplate;

		public ItemsToInsureScreen insureScreen;

		internal void _E000(_ECB4 child, ItemToInsurePanel view)
		{
			view.Show(child, ItemViewFactory.GetItemType(child.Type), _003C_003E4__this._E18F, panelTemplate, insureScreen);
			int parentsCount = _003C_003E4__this._E18F.GetParentsCount(child, 0);
			if (parentsCount > 0)
			{
				_003C_003E4__this.SetAsChild(view, parentsCount);
			}
			_003C_003E4__this._E001(value: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _ECAF selectedInsurer;

		internal int _E000(_ECB4 x)
		{
			return selectedInsurer[x].Amount;
		}
	}

	[SerializeField]
	private TextMeshProUGUI _itemNameLabel;

	[SerializeField]
	private TextMeshProUGUI _itemPriceLabel;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _selectedColor;

	[SerializeField]
	private TradeItemType _itemType;

	[SerializeField]
	private GameObject _childItemSpace;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private TextMeshProUGUI _itemsToInsureCount;

	[SerializeField]
	private InsurerParametersPanel _insurerParametersPanel;

	[SerializeField]
	private RectTransform _childContainer;

	[SerializeField]
	private SwitchableParamsButton _dropdownButton;

	private _EC71<_ECB4, ItemToInsurePanel> _E07E;

	private _ECB1 _E18F;

	private bool _E317;

	[CompilerGenerated]
	private _ECB4 _E318;

	private _ECB4 _E000
	{
		[CompilerGenerated]
		get
		{
			return _E318;
		}
		[CompilerGenerated]
		set
		{
			_E318 = value;
		}
	}

	private _ECB1._E000 _E001 => _E18F.OverallPrice;

	private void Awake()
	{
		_dropdownButton.AddSubscription(delegate
		{
			_E317 = !_E317;
			_childContainer.gameObject.SetActive(_E317);
			_E003();
		}, isOn: true);
		_childItemSpace.gameObject.SetActive(value: false);
	}

	public void Show(_ECB4 insuredItem, EItemType itemType, _ECB1 insurance, ItemToInsurePanel panelTemplate, ItemsToInsureScreen insureScreen)
	{
		ShowGameObject();
		_E18F = insurance;
		_E317 = true;
		this._E000 = insuredItem;
		_itemNameLabel.text = insuredItem.Name.Localized();
		_itemType.Show(itemType);
		_E004(-1, 0);
		UpdateInsuranceItemsPrices(new List<_ECB4> { this._E000 });
		_E001(value: false);
		UI.AddDisposable(insureScreen.OnTraderChanged.Subscribe(_E000));
		_E07E?.Dispose();
		_E07E = UI.AddDisposable(new _EC71<_ECB4, ItemToInsurePanel>(insuredItem.ChildrenToInsure, panelTemplate, _childContainer, delegate(_ECB4 child, ItemToInsurePanel view)
		{
			view.Show(child, ItemViewFactory.GetItemType(child.Type), _E18F, panelTemplate, insureScreen);
			int parentsCount = _E18F.GetParentsCount(child, 0);
			if (parentsCount > 0)
			{
				SetAsChild(view, parentsCount);
			}
			_E001(value: true);
		}));
		UpdateInsuranceItemsPrices(insuredItem.ChildrenToInsure);
	}

	private void _E000()
	{
		_E07E?.UpdateItems(delegate(ItemToInsurePanel arg)
		{
			arg.UpdateInsuranceItemsPrices(_E18F.AllItemsToInsure);
		});
	}

	private void _E001(bool value)
	{
		_dropdownButton.gameObject.SetActive(value);
		_childContainer.gameObject.SetActive(value);
	}

	private void _E002()
	{
		_ECB1._E000 obj = this._E001;
		_insurerParametersPanel.UpdateLabels(obj.Price, updateMoneyToPay: true, updateMoneySums: false);
		_insurerParametersPanel.UpdateInsureButtonStatus(obj.Error);
		_itemsToInsureCount.text = string.Format(_ED3E._E000(233098).Localized(), _E18F.AllItemsToInsure.Count);
	}

	public void UpdateInsuranceItemsPrices(IEnumerable<_ECB4> items)
	{
		_E18F.UpdateInsuranceItemsPrices(items, delegate
		{
			_E003();
			_E002();
		});
	}

	public void SetAsChild(ItemToInsurePanel child, int parentsLevel)
	{
		for (int i = 0; i < parentsLevel; i++)
		{
			GameObject obj = Object.Instantiate(_childItemSpace, child._childItemSpace.transform.parent);
			obj.gameObject.SetActive(value: true);
			obj.transform.SetAsFirstSibling();
		}
	}

	private void _E003()
	{
		_ECAF selectedInsurer = _E18F.InsureSummary[_E18F.SelectedInsurerId];
		_ECAE obj = selectedInsurer[this._E000];
		int childPrice = _E18F.GetFlattenChildren(this._E000).Sum((_ECB4 x) => selectedInsurer[x].Amount);
		_E004(obj?.Amount ?? (-1), childPrice);
	}

	private void _E004(int price, int childPrice)
	{
		_itemPriceLabel.text = ((price < 0) ? _ED3E._E000(59476) : ((this._E000.ChildrenToInsure.Count <= 0) ? price.ToString() : (_E317 ? price.ToString() : (price + _ED3E._E000(91195) + childPrice + _ED3E._E000(54246) + _E18F.GetFlattenChildren(this._E000).Count + _ED3E._E000(18502) + _ED3E._E000(233142).Localized() + _ED3E._E000(27308)))));
		_canvasGroup.SetUnlockStatus(price > 0, setRaycast: false);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			_E18F.RemoveItemFromInsuranceQueue(this._E000);
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_background.color = _selectedColor;
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_background.color = _defaultColor;
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E317 = !_E317;
		_childContainer.gameObject.SetActive(_E317);
		_E003();
	}

	[CompilerGenerated]
	private void _E006(ItemToInsurePanel arg)
	{
		arg.UpdateInsuranceItemsPrices(_E18F.AllItemsToInsure);
	}

	[CompilerGenerated]
	private void _E007(_ECB4[] insuredItems)
	{
		_E003();
		_E002();
	}
}
