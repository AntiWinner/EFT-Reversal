using System;
using System.Runtime.CompilerServices;
using EFT.Hideout;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class FuelSelectionCell : ItemSelectionCell
{
	[SerializeField]
	private TextMeshProUGUI _details;

	[SerializeField]
	private Color _defaultDetailsColor;

	[SerializeField]
	private Color _lowCountDetailsColor;

	[SerializeField]
	private Color _noItemDetailsColor;

	[SerializeField]
	private float _lowCountLevel = 10f;

	[SerializeField]
	private GameObject _noFuelPanel;

	[SerializeField]
	private GameObject _timePanel;

	[SerializeField]
	private TextMeshProUGUI _timeText;

	[SerializeField]
	private Image _lockIcon;

	private SimpleTooltip _E02A;

	private float _E112;

	private _E819 _E113;

	private EProductionState _E114;

	protected override EContextPriorDirection Direction => EContextPriorDirection.RightUpDown;

	private ResourceComponent _E000 => CurrentItem?.GetItemComponent<ResourceComponent>();

	protected override void Awake()
	{
		base.Awake();
		_E02A = ItemUiContext.Instance.Tooltip;
		_timePanel.GetOrAddComponent<HoverTrigger>().Init(delegate
		{
			if (!_E113.ConsumptionReduction.ZeroOrNegative())
			{
				_E02A.Show(string.Format(_ED3E._E000(252212).Localized(), _E113.ConsumptionReduction));
			}
		}, delegate
		{
			_E02A.Close();
		});
	}

	public void Show([CanBeNull] Item currentSelectedItem, _E819 consumer, Func<ItemSelectionCell, Item, bool, bool> itemSelectionChanged)
	{
		_E113 = consumer;
		base.SelectingItems = _E113.Slots.Filters;
		Show(currentSelectedItem, itemSelectionChanged);
	}

	public override void SetItem(Item item)
	{
		_E000(item);
		base.SetItem(item);
		_E001();
	}

	public void SetProductionState(EProductionState productionState)
	{
		_E114 = productionState;
		_E001();
	}

	private void _E000(Item selectedItem)
	{
		bool flag = selectedItem == null || _E113.Changeable;
		_lockIcon.gameObject.SetActive(!flag);
		base.DropdownButton.gameObject.SetActive(flag);
	}

	private void _E001()
	{
		ResourceComponent resourceComponent = this._E000;
		_details.text = _E002(resourceComponent);
		float num = resourceComponent?.Value ?? 0f;
		bool flag = num > Mathf.Epsilon;
		if (resourceComponent != null)
		{
			if (_noFuelPanel != null)
			{
				_noFuelPanel.SetActive(!flag);
			}
			_timePanel.SetActive(flag && _E114 != EProductionState.Error);
			if (flag)
			{
				int num2 = Mathf.FloorToInt(num / _E113.Consumption);
				string arg = _ED3E._E000(47900);
				if (num2 >= 60)
				{
					num2 = Mathf.FloorToInt((float)num2 / 60f);
					arg = _ED3E._E000(47861);
					if (num2 >= 60)
					{
						num2 = Mathf.FloorToInt((float)num2 / 60f);
						arg = _ED3E._E000(47880);
					}
				}
				_timeText.text = string.Format(_ED3E._E000(168928), num2, arg);
			}
		}
		else
		{
			if (_noFuelPanel != null)
			{
				_noFuelPanel.SetActive(value: false);
			}
			_timePanel.SetActive(value: false);
		}
		_E112 = num;
	}

	protected override string GetDetails(Item item)
	{
		ResourceComponent itemComponent = item.GetItemComponent<ResourceComponent>();
		if (itemComponent == null)
		{
			return string.Empty;
		}
		return _E002(itemComponent);
	}

	private string _E002(ResourceComponent resourceHolderItem)
	{
		int num = 0;
		float num2 = 100f;
		Color color = _noItemDetailsColor;
		if (resourceHolderItem != null)
		{
			num = Mathf.CeilToInt(resourceHolderItem.Value);
			num2 = resourceHolderItem.MaxResource;
			color = (((float)num > _lowCountLevel) ? _defaultDetailsColor : _lowCountDetailsColor);
		}
		return string.Format(_ED3E._E000(252186), color.GetRichTextColor(), num, num2);
	}

	private void Update()
	{
		if (!(_E112 <= 0f) && this._E000 != null && !this._E000.Value.ApproxEquals(_E112))
		{
			_E001();
		}
	}

	public override void Close()
	{
		_E02A.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E003(PointerEventData arg)
	{
		if (!_E113.ConsumptionReduction.ZeroOrNegative())
		{
			_E02A.Show(string.Format(_ED3E._E000(252212).Localized(), _E113.ConsumptionReduction));
		}
	}

	[CompilerGenerated]
	private void _E004(PointerEventData arg)
	{
		_E02A.Close();
	}
}
