using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class BarterSchemePanel : UIElement
{
	[SerializeField]
	private Button _autoButton;

	[SerializeField]
	private CustomTextMeshProInputField _quantity;

	[SerializeField]
	private CustomTextMeshProUGUI _purchaseCaption;

	[SerializeField]
	private ItemOnGrid _itemOnDisplay;

	[SerializeField]
	private GameObject _validSchemeWarning;

	[SerializeField]
	private CustomTextMeshProUGUI _buyRestrictionLabel;

	[SerializeField]
	private GameObject _buyRestrictionWarning;

	[SerializeField]
	private TradingRequisitePanel _requisiteTemplate;

	[SerializeField]
	private Transform _requisitesContainer;

	[SerializeField]
	private ScrollRect _mainPart;

	[SerializeField]
	private GameObject _noItemsSelectedPanel;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private UpdatableToggle _autoFillRequirements;

	[SerializeField]
	private HoverTooltipArea _autoFillHover;

	private bool _E1AF;

	private _EAE6 _E19A;

	private _EC79<_E8B1, TradingRequisitePanel> _E1B0;

	private _E8AF _E1B1;

	private _E9EF _E1B2;

	[CanBeNull]
	private Item _E000 => _E1B1.SelectedItem;

	private bool _E001
	{
		get
		{
			if (this._E000 != null)
			{
				if (this._E000.BuyRestrictionMax <= 0)
				{
					return this._E000.IsEmptyStack;
				}
				return true;
			}
			return false;
		}
	}

	private int _E002
	{
		get
		{
			if (this._E000 == null)
			{
				return 0;
			}
			return this._E000.BuyRestrictionMax - this._E000.BuyRestrictionCurrent;
		}
	}

	private int _E003
	{
		get
		{
			if (!int.TryParse(_quantity.text, out var result))
			{
				return 0;
			}
			return result;
		}
	}

	private void Awake()
	{
		_quantity.onValueChanged.AddListener(delegate
		{
			_E1B1.CurrentQuantity = this._E003;
		});
		_autoButton.onClick.AddListener(delegate
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
			_E003();
		});
		_autoFillRequirements.onValueChanged.AddListener(delegate(bool arg)
		{
			_E1AF = arg;
			_E762.SetBool(_ED3E._E000(261356), _E1AF);
		});
		_autoFillHover.SetMessageText(_ED3E._E000(261331));
	}

	public void Show(_E8AF traderAssortment, _EAE6 itemController, _E9EF userStashGrid)
	{
		ShowGameObject();
		_E1B1 = traderAssortment;
		_E19A = itemController;
		_E1B2 = userStashGrid;
		_E1AF = _E762.GetBool(_ED3E._E000(261356));
		_autoFillRequirements.isOn = _E1AF;
		_E19A.ExamineEvent += _E000;
		UI.BindEvent(_E1B1.SelectedItemChanged, delegate
		{
			_E1B0?.Dispose();
			_mainPart.gameObject.SetActive(this._E000 != null);
			_noItemsSelectedPanel.gameObject.SetActive(this._E000 == null);
			if (_mainPart.GetIsScrollActive())
			{
				_mainPart.normalizedPosition = Vector2.one;
			}
			if (_itemOnDisplay.gameObject.activeSelf)
			{
				_itemOnDisplay.Close();
			}
			if (this._E000 != null)
			{
				_E001(this._E000);
				_E1B0 = new _EC79<_E8B1, TradingRequisitePanel>(_E1B1.CurrentRequisites, _requisiteTemplate, _requisitesContainer, delegate(_E8B1 anItem, TradingRequisitePanel view)
				{
					view.Show(anItem, _E1B1);
				});
				_E002();
				if (_E1AF)
				{
					_E003();
				}
			}
			else
			{
				_itemOnDisplay.Show(null);
			}
		});
		UI.BindEvent(_E1B1.ValidityChanged, delegate
		{
			_validSchemeWarning.SetActive(!_E1B1.IsValid);
		});
		UI.BindEvent(_E1B1.QuantityChanged, delegate
		{
			_quantity.SetTextWithoutNotify(_E1B1.CurrentQuantity.ToString());
			if (_E1AF)
			{
				_E003();
			}
		});
		UI.BindEvent(_E1B1.TransactionChanged, delegate
		{
			bool transactionInProgress = _E1B1.TransactionInProgress;
			_canvasGroup.interactable = !transactionInProgress;
			_canvasGroup.alpha = (transactionInProgress ? 0.3f : 1f);
			_canvasGroup.blocksRaycasts = !transactionInProgress;
			_E002();
			if (this._E000 != null && !_E1B1.TransactionInProgress)
			{
				_quantity.text = Mathf.Min(this._E003, this._E002).ToString();
			}
		});
	}

	private void _E000(_EAF6 examineArgs)
	{
		if (this._E000?.Id == examineArgs.Item.Id)
		{
			_E001(this._E000);
		}
	}

	private void _E001(Item selectedItem)
	{
		bool flag = _E19A.Examined(selectedItem);
		string text = (flag ? selectedItem.ShortName.Localized() : _ED3E._E000(193009).Localized());
		_purchaseCaption.text = _ED3E._E000(261401).Localized() + _ED3E._E000(12201) + text;
		_itemOnDisplay.Show(selectedItem, correctSize: true, flag);
		EventSystem.current.SetSelectedGameObject(_quantity.gameObject, null);
		_quantity.ActivateInputField();
		_quantity.Select();
	}

	private void _E002()
	{
		if (this._E000 == null)
		{
			return;
		}
		_buyRestrictionWarning.SetActive(this._E001);
		if (this._E001)
		{
			if (this._E000.IsEmptyStack)
			{
				_buyRestrictionLabel.text = _ED3E._E000(186100).Localized();
			}
			else if (this._E000.BuyRestrictionCurrent < this._E000.BuyRestrictionMax)
			{
				_buyRestrictionLabel.text = string.Format(_ED3E._E000(261386).Localized(), this._E000.BuyRestrictionCurrent.ToString(), this._E000.BuyRestrictionMax.ToString());
			}
			else
			{
				_buyRestrictionLabel.text = _ED3E._E000(261485).Localized();
			}
		}
		bool active = !this._E001 || this._E002 > 0;
		_quantity.gameObject.SetActive(active);
		_autoButton.gameObject.SetActive(active);
	}

	internal void _E003()
	{
		List<Item> source = _E1B2.ContainedItems.SelectMany((KeyValuePair<Item, LocationInGrid> x) => x.Key.GetAllItems()).ToList();
		_E1B1?.CurrentRequisites?.AutoFill(source.Where((Item item) => item.IsExchangeable()));
	}

	public override void Close()
	{
		_E19A.ExamineEvent -= _E000;
		_E1B0?.Dispose();
		base.Close();
	}

	[CompilerGenerated]
	private void _E004(string x)
	{
		_E1B1.CurrentQuantity = this._E003;
	}

	[CompilerGenerated]
	private void _E005()
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
		_E003();
	}

	[CompilerGenerated]
	private void _E006(bool arg)
	{
		_E1AF = arg;
		_E762.SetBool(_ED3E._E000(261356), _E1AF);
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E1B0?.Dispose();
		_mainPart.gameObject.SetActive(this._E000 != null);
		_noItemsSelectedPanel.gameObject.SetActive(this._E000 == null);
		if (_mainPart.GetIsScrollActive())
		{
			_mainPart.normalizedPosition = Vector2.one;
		}
		if (_itemOnDisplay.gameObject.activeSelf)
		{
			_itemOnDisplay.Close();
		}
		if (this._E000 != null)
		{
			_E001(this._E000);
			_E1B0 = new _EC79<_E8B1, TradingRequisitePanel>(_E1B1.CurrentRequisites, _requisiteTemplate, _requisitesContainer, delegate(_E8B1 anItem, TradingRequisitePanel view)
			{
				view.Show(anItem, _E1B1);
			});
			_E002();
			if (_E1AF)
			{
				_E003();
			}
		}
		else
		{
			_itemOnDisplay.Show(null);
		}
	}

	[CompilerGenerated]
	private void _E008(_E8B1 anItem, TradingRequisitePanel view)
	{
		view.Show(anItem, _E1B1);
	}

	[CompilerGenerated]
	private void _E009()
	{
		_validSchemeWarning.SetActive(!_E1B1.IsValid);
	}

	[CompilerGenerated]
	private void _E00A()
	{
		_quantity.SetTextWithoutNotify(_E1B1.CurrentQuantity.ToString());
		if (_E1AF)
		{
			_E003();
		}
	}

	[CompilerGenerated]
	private void _E00B()
	{
		bool transactionInProgress = _E1B1.TransactionInProgress;
		_canvasGroup.interactable = !transactionInProgress;
		_canvasGroup.alpha = (transactionInProgress ? 0.3f : 1f);
		_canvasGroup.blocksRaycasts = !transactionInProgress;
		_E002();
		if (this._E000 != null && !_E1B1.TransactionInProgress)
		{
			_quantity.text = Mathf.Min(this._E003, this._E002).ToString();
		}
	}
}
