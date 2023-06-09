using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class TradingRequisitePanel : ItemOnGrid, _EC9E, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private CustomTextMeshProUGUI _count;

	[SerializeField]
	private CustomTextMeshProUGUI _dogtagLevel;

	[SerializeField]
	private CustomTextMeshProUGUI _dogtagSide;

	[SerializeField]
	private GameObject _yesCheck;

	[SerializeField]
	private GameObject _noCheck;

	private SimpleTooltip _E02A;

	private _E8B1 _E206;

	private _E8AF _E1B1;

	private ItemUiContext _E089;

	private _EB68 _E133;

	private void Awake()
	{
		HoverTrigger orAddComponent = Icon.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			if (_E206.IsDogtagRequired)
			{
				_E02A.Show(_ED3E._E000(200879).Localized() + _ED3E._E000(255020) + _E206.Level + _ED3E._E000(18502) + _ED3E._E000(255032).Localized() + ((_E206.Side != EDogtagExchangeSide.Any) ? (_ED3E._E000(10270) + _E206.Side) : "").ToUpper());
			}
			else
			{
				_E02A.Show(_E206.RequiredItem.ShortName.Localized());
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (_E02A != null)
			{
				_E02A.Close();
			}
		};
	}

	public override void Close()
	{
		base.Close();
		_E133?.Dispose();
		_E133 = null;
	}

	public void Show(_E8B1 requisite, _E8AF trader)
	{
		_E206 = requisite;
		_E1B1 = trader;
		_E089 = ItemUiContext.Instance;
		_E02A = _E089.Tooltip;
		_E133 = new _EB64(_E206.RequiredItem, EItemViewType.Handbook);
		Show(requisite.RequiredItem);
		UI.BindEvent(requisite.PreparedChanged, _E000);
		UI.BindEvent(trader.QuantityChanged, _E000);
	}

	private void _E000()
	{
		int preparedItemsCount = _E206.PreparedItemsCount;
		Icon.color = ((preparedItemsCount > 0) ? Color.white : new Color(1f, 1f, 1f, 0.3f));
		int requiredItemsCount = _E206.RequiredItemsCount;
		bool flag = requiredItemsCount != 1;
		_count.gameObject.SetActive(flag);
		if (flag)
		{
			_count.text = ((requiredItemsCount < int.MaxValue && requiredItemsCount >= 0) ? (_ED3E._E000(254984) + preparedItemsCount + _ED3E._E000(30703) + requiredItemsCount + _ED3E._E000(59467)) : _ED3E._E000(254994).Localized());
		}
		bool isDogtagRequired = _E206.IsDogtagRequired;
		if (isDogtagRequired)
		{
			_dogtagLevel.text = _ED3E._E000(255032).Localized() + _ED3E._E000(18502) + _E206.Level;
			_dogtagSide.text = _E206.Side.ToString().Localized();
		}
		_dogtagLevel.gameObject.SetActive(isDogtagRequired);
		_dogtagSide.gameObject.SetActive(isDogtagRequired && _E206.Side != EDogtagExchangeSide.Any);
		bool enough = _E206.Enough;
		_yesCheck.SetActive(enough);
		_noCheck.SetActive(!enough);
	}

	public void HighlightItemViewPosition(_EB69 itemContext, _EB68 targetItemContext, bool preview)
	{
	}

	public void DisableHighlight()
	{
	}

	public void DragStarted()
	{
	}

	public void DragCancelled()
	{
	}

	public bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		Item item = itemContext.Item;
		operation = default(_ECD7);
		if (!_E206.Enough && _E206.IsRequired(item) && item.IsExchangeable())
		{
			return !_E1B1.CurrentRequisites.IsPreparedForTrade(item);
		}
		return false;
	}

	public bool CanDrag(_EB68 itemContext)
	{
		return false;
	}

	public Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		_E206.PrepareItem(itemContext.Item);
		return Task.CompletedTask;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			_E089.ShowContextMenu(_E133, eventData.pressPosition);
		}
	}

	[CompilerGenerated]
	private void _E001(PointerEventData arg)
	{
		if (_E206.IsDogtagRequired)
		{
			_E02A.Show(_ED3E._E000(200879).Localized() + _ED3E._E000(255020) + _E206.Level + _ED3E._E000(18502) + _ED3E._E000(255032).Localized() + ((_E206.Side != EDogtagExchangeSide.Any) ? (_ED3E._E000(10270) + _E206.Side) : "").ToUpper());
		}
		else
		{
			_E02A.Show(_E206.RequiredItem.ShortName.Localized());
		}
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		if (_E02A != null)
		{
			_E02A.Close();
		}
	}
}
