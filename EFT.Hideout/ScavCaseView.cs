using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.Hideout;

public sealed class ScavCaseView : ProduceViewBase<_E82D, _E82A>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemUiContext itemUiContext;

		public _EAED inventoryController;

		public ScavCaseView _003C_003E4__this;

		public List<Item> allItems;

		internal void _E000(ItemRequirement requirement, HideoutProductionRequirementView view)
		{
			view.Show(itemUiContext, inventoryController, requirement, _003C_003E4__this.Scheme, allItems, _003C_003E4__this.Producer.ProducingItems.Any());
			_003C_003E4__this._E009 &= view.IsFulfilled;
		}
	}

	private const float m__E000 = 0.5f;

	private static readonly string m__E001 = _ED3E._E000(171762) + EDetailsType.Collecting.ToStringNoBox();

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private HideoutProductionRequirementView _requiredItemTemplate;

	[SerializeField]
	private Transform _requiredItemsContainer;

	[SerializeField]
	private GameObject _expectedTimePanel;

	[SerializeField]
	private TextMeshProUGUI _expectedTime;

	[SerializeField]
	private GameObject _productionPercentagesPanel;

	[SerializeField]
	private GameObject _productionBackground;

	[SerializeField]
	private TextMeshProUGUI _productionPercentages;

	[SerializeField]
	private GameObject _resultItemImage;

	[SerializeField]
	private DefaultUIButton _startButton;

	[SerializeField]
	private DefaultUIButton _getItemsButton;

	[SerializeField]
	private TextMeshProUGUI _productionStatus;

	[SerializeField]
	private GameObject _loaderPanel;

	[SerializeField]
	private GameObject _timerPanel;

	private _EC79<ItemRequirement, HideoutProductionRequirementView> m__E002;

	private SimpleTooltip m__E003;

	private MultiLineTooltip m__E004;

	private _EC80 m__E005;

	private bool m__E006;

	private bool m__E007;

	private DateTime m__E008;

	protected override bool ShowItemsListWindow => true;

	private bool _E009
	{
		get
		{
			return this.m__E006;
		}
		set
		{
			if (this.m__E006 != value)
			{
				this.m__E006 = value;
				_E003();
			}
		}
	}

	public bool AnyItemsReady
	{
		set
		{
			if (this.m__E007 != value)
			{
				this.m__E007 = value;
				_E003();
			}
		}
	}

	private void Awake()
	{
		this.m__E003 = ItemUiContext.Instance.Tooltip;
		this.m__E004 = ItemUiContext.Instance.MultiLineTooltip;
		HoverTrigger orAddComponent = _resultItemImage.GetOrAddComponent<HoverTrigger>();
		HoverTrigger orAddComponent2 = _timerPanel.GetOrAddComponent<HoverTrigger>();
		orAddComponent2.OnHoverStart += delegate
		{
			if (this.m__E005 != null)
			{
				this.m__E004.Show(this.m__E005);
			}
		};
		orAddComponent2.OnHoverEnd += delegate
		{
			if (this.m__E004 != null && this.m__E004.isActiveAndEnabled)
			{
				this.m__E004.Close();
			}
		};
		orAddComponent.Init(delegate
		{
			int min = base.Producer.CompleteItemsCount.Min;
			int max = base.Producer.CompleteItemsCount.Max;
			string text = ((min == max) ? string.Format(_ED3E._E000(170742).Localized(), min) : string.Format(_ED3E._E000(170696).Localized(), min, max));
			this.m__E003.Show(text);
		}, delegate
		{
			this.m__E003.Close();
		});
	}

	public new void Show(ItemUiContext itemUiContext, _EAED inventoryController, _E82A scheme, _E82D producer, Action<string> onStartProducing, Action<string> getProducedItems)
	{
		base.Show(itemUiContext, inventoryController, scheme, producer, onStartProducing, getProducedItems);
		_startButton.OnClick.AddListener(_E002);
		_getItemsButton.OnClick.AddListener(base.GetProducedItems);
		_E001();
		if (base.Scheme.HasRequirements)
		{
			List<Item> allItems = Singleton<_E815>.Instance.GetAvailableItemsByFilter<Item>().ToList();
			_E009 = true;
			this.m__E005 = _E000(base.Scheme);
			this.m__E002 = UI.AddViewList(base.Scheme.RequiredItems, _requiredItemTemplate, _requiredItemsContainer, delegate(ItemRequirement requirement, HideoutProductionRequirementView view)
			{
				view.Show(itemUiContext, inventoryController, requirement, base.Scheme, allItems, base.Producer.ProducingItems.Any());
				_E009 &= view.IsFulfilled;
			});
			_E003();
		}
	}

	private _EC80 _E000(_E828 scheme)
	{
		if (scheme.SchemeOwnerType != EOwnerType.ScavCase)
		{
			return null;
		}
		_EC80 obj = null;
		Requirement[] requirements = scheme.requirements;
		for (int i = 0; i < requirements.Length; i++)
		{
			if (requirements[i] is ItemRequirement itemRequirement && _EA10.TryGetCurrencyType(itemRequirement.TemplateId, out var type))
			{
				if (obj != null)
				{
					Debug.LogError(_ED3E._E000(170623));
					break;
				}
				obj = new _EC80(type, ECharismaDiscountType.ScavCaseCharismaDiscount, itemRequirement.BaseCount, itemRequirement.IntCount, itemRequirement.UserItemsCount);
			}
		}
		return obj;
	}

	public override void UpdateView()
	{
		if (this == null)
		{
			return;
		}
		base.UpdateView();
		_E001();
		if (this.m__E002 == null)
		{
			return;
		}
		List<Item> allItems = Singleton<_E815>.Instance.GetAvailableItemsByFilter<Item>().ToList();
		bool flag = true;
		foreach (var (requirement, hideoutProductionRequirementView2) in this.m__E002)
		{
			hideoutProductionRequirementView2.Show(base.ItemUiContext, base.InventoryController, requirement, base.Scheme, allItems, base.Producer.ProducingItems.Any());
			flag &= hideoutProductionRequirementView2.IsFulfilled;
		}
		_E009 = flag;
	}

	private void _E001()
	{
		AnyItemsReady = base.Producer.CompleteItemsStorage.AnyItemsReady;
		bool flag = base.Producer.CompleteItemsStorage.GetItemsCount(base.Scheme._id) > 0;
		bool flag2 = false;
		if (base.Producer.ProducingItems.Any() || flag)
		{
			flag2 = base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out var value);
			_expectedTimePanel.SetActive(value: false);
			_productionPercentagesPanel.SetActive(value: true);
			if (flag2)
			{
				_E004(value);
			}
			else
			{
				_productionPercentages.text = string.Empty;
			}
		}
		else
		{
			_expectedTime.SetMonospaceText(new TimeSpan(0, 0, (int)base.Producer.CalculateProductionTime(base.Scheme)).TimeLeftShortFormat());
			_expectedTimePanel.SetActive(value: true);
			_productionPercentagesPanel.SetActive(value: false);
		}
		_canvasGroup.SetUnlockStatus(base.Producer.CanStartByProductionCount || flag2 || !base.Producer.ProducingItems.Any());
		_productionBackground.SetActive(flag2);
		_loaderPanel.SetActive(base.GettingItems);
		_getItemsButton.gameObject.SetActive(flag && !base.GettingItems);
		_productionStatus.gameObject.SetActive(!flag && flag2);
		_startButton.gameObject.SetActive(!flag && !flag2);
		_resultItemImage.SetActive(flag);
	}

	private void _E002()
	{
		if (_E009)
		{
			Singleton<_E815>.Instance.StartScavCaseProduction(base.Scheme._id, this.m__E002.Select((KeyValuePair<ItemRequirement, HideoutProductionRequirementView> _) => _.Key).ToArray());
			base.OnStartProducing?.Invoke(base.Scheme._id);
		}
	}

	private void _E003()
	{
		bool flag = this.m__E006 && !this.m__E007 && base.Producer.CanStart;
		string tooltip = string.Empty;
		if (!flag)
		{
			if (this.m__E007)
			{
				tooltip = _ED3E._E000(170017);
			}
			else if (!this.m__E006)
			{
				tooltip = _ED3E._E000(171571);
			}
			else if (!base.Producer.CanStart)
			{
				tooltip = _ED3E._E000(170052);
			}
		}
		_startButton.SetDisabledTooltip(tooltip);
		_startButton.Interactable = flag;
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (!((utcNow - this.m__E008).TotalSeconds < 0.5))
		{
			this.m__E008 = utcNow;
			if (base.Producer.ProducingItems.TryGetValue(base.Scheme._id, out var value))
			{
				_E004(value);
			}
		}
	}

	private void _E004(_E827 producingItem)
	{
		_productionStatus.SetMonospaceText(ScavCaseView.m__E001.Localized() + _ED3E._E000(54246) + producingItem.EstimatedTimeInt.ToTimeString() + _ED3E._E000(27308));
		_productionPercentages.SetMonospaceText(string.Format(_ED3E._E000(171594), Mathf.FloorToInt((float)producingItem.Progress * 100f)));
	}

	public override void Close()
	{
		_startButton.OnClick.RemoveAllListeners();
		_getItemsButton.OnClick.RemoveAllListeners();
		this.m__E002 = null;
		if (this.m__E003 != null)
		{
			this.m__E003.Close();
		}
		if (this.m__E004 != null)
		{
			this.m__E004.Close();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E005(PointerEventData arg)
	{
		if (this.m__E005 != null)
		{
			this.m__E004.Show(this.m__E005);
		}
	}

	[CompilerGenerated]
	private void _E006(PointerEventData arg)
	{
		if (this.m__E004 != null && this.m__E004.isActiveAndEnabled)
		{
			this.m__E004.Close();
		}
	}

	[CompilerGenerated]
	private void _E007(PointerEventData arg)
	{
		int min = base.Producer.CompleteItemsCount.Min;
		int max = base.Producer.CompleteItemsCount.Max;
		string text = ((min == max) ? string.Format(_ED3E._E000(170742).Localized(), min) : string.Format(_ED3E._E000(170696).Localized(), min, max));
		this.m__E003.Show(text);
	}

	[CompilerGenerated]
	private void _E008(PointerEventData arg)
	{
		this.m__E003.Close();
	}
}
