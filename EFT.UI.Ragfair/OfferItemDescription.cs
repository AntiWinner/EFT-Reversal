using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Ragfair;

public sealed class OfferItemDescription : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public OfferItemDescription _003C_003E4__this;

		public Offer offer;

		internal void _E000()
		{
			_003C_003E4__this._E092.ExamineEvent -= _003C_003E4__this._E000;
		}

		internal void _E001(PointerEventData arg)
		{
			bool flag = _003C_003E4__this._E092?.Examined(offer.Item) ?? true;
			_003C_003E4__this._E02A.Show((flag ? offer.Name : _ED3E._E000(193009)).Localized());
		}

		internal void _E002(PointerEventData arg)
		{
			_003C_003E4__this._E02A.Close();
		}
	}

	[SerializeField]
	private HoverTrigger _hoverTrigger;

	[SerializeField]
	private TextMeshProUGUI _offerItemName;

	[SerializeField]
	private TextMeshProUGUI _offerStackCount;

	[SerializeField]
	private TextMeshProUGUI _offersCount;

	[SerializeField]
	private TextMeshProUGUI _itemCategory;

	[SerializeField]
	private InteractionButtonsContainer _offerContextMenu;

	[SerializeField]
	private RagfairOfferItemView _offerItemView;

	[SerializeField]
	private Transform _resourceCountersContainer;

	[SerializeField]
	private OfferViewResourceCounter _resourceCounterTemplate;

	private const int _E091 = 10;

	private Offer _E354;

	private ItemUiContext _E089;

	private _EAED _E092;

	private _EBAB _E083;

	private _EC4E<EItemInfoButton> _E2A3;

	private SimpleTooltip _E02A;

	private bool _E355;

	private List<OfferViewResourceCounter> _E356 = new List<OfferViewResourceCounter>();

	public void Show(_EBA8 handbook, Offer offer, bool expanded, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany)
	{
		_E006();
		_E354 = offer;
		_E355 = expanded;
		_E089 = itemUiContext;
		_E092 = inventoryController;
		_E092.ExamineEvent += _E000;
		UI.AddDisposable(delegate
		{
			_E092.ExamineEvent -= _E000;
		});
		_E02A = itemUiContext.Tooltip;
		if (_E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
		ShowGameObject();
		_E083 = handbook.StructuredItems[offer.Item.TemplateId];
		if (_E083 == null)
		{
			_E39B.LogRagfair(_ED3E._E000(243348) + offer.Item.Name.Localized() + _ED3E._E000(243384));
			return;
		}
		_hoverTrigger.enabled = !_E355;
		if (!_E355)
		{
			_hoverTrigger.Init(delegate
			{
				bool flag = _E092?.Examined(offer.Item) ?? true;
				_E02A.Show((flag ? offer.Name : _ED3E._E000(193009)).Localized());
			}, delegate
			{
				_E02A.Close();
			});
		}
		_E092.ExamineEvent += _E002;
		_E001();
		_offerItemView.Show(offer, offer.Item, ItemRotation.Horizontal, _E355, _E092, _E092, _E089, insuranceCompany);
		if (expanded)
		{
			_E2A3 = _E089.GetItemContextInteractions(_offerItemView.ItemContext, delegate
			{
			});
			_offerContextMenu.Show(_E2A3, null, null, offer.Item, null, autoClose: false);
		}
		else
		{
			_offerContextMenu.Close();
			_E003();
		}
	}

	private void _E000(_EAF6 args)
	{
		if (_E083 != null && _E2A3 != null && _E083.Data.Item.TemplateId == args.Item.TemplateId)
		{
			_E2A3.CallRedraw(args.Item.TemplateId);
		}
	}

	private void _E001()
	{
		Item item = _E354.Item;
		_offerItemName.text = (_E092.Examined(item) ? Singleton<_E63B>.Instance.BriefItemName(item, _E354.Name.Localized()) : _ED3E._E000(193009).Localized());
		string text = _E354.TotalItemCount.ToString();
		text = ((!_E354.SellInOnePiece || item.StackObjectsCount <= 1) ? (_ED3E._E000(243455).Localized() + _ED3E._E000(243438) + text + _ED3E._E000(59467)) : (_ED3E._E000(243371) + text + _ED3E._E000(243414) + _ED3E._E000(243402).Localized() + _ED3E._E000(47210)));
		_offerStackCount.text = text;
		_itemCategory.text = string.Join(_ED3E._E000(197193), _E083.Category.Select((string x) => x.Localized()).ToArray());
		bool flag = _E354.BuyRestrictionMax > 0;
		_offersCount.gameObject.SetActive(flag);
		_E005();
		if (_E354.NotAvailable)
		{
			return;
		}
		if (flag)
		{
			_offersCount.text = string.Format(_ED3E._E000(243487), _ED3E._E000(243473).Localized(), _E354.CurrentItemCount);
		}
		List<RepairableComponent> source = item.GetItemComponentsInChildren<RepairableComponent>().ToList();
		if (source.Any())
		{
			float num = (float)Math.Round(source.Sum((RepairableComponent x) => x.Durability), 1);
			float num2 = (float)Math.Round(source.Sum((RepairableComponent x) => x.MaxDurability), 1);
			string textColor = ((num * 100f / num2 <= 10f) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.Durability, num, num2, textColor);
		}
		if (item.TryGetItemComponent<MedKitComponent>(out var component))
		{
			string textColor2 = ((component.HpResource < (float)component.MaxHpResource) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.HpResource, component.HpResource, component.MaxHpResource, textColor2);
		}
		if (item.TryGetItemComponent<FoodDrinkComponent>(out var component2))
		{
			string textColor3 = ((component2.HpPercent < component2.MaxResource) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.FoodResource, component2.HpPercent, component2.MaxResource, textColor3);
		}
		if (item.TryGetItemComponent<ResourceComponent>(out var component3))
		{
			string textColor4 = ((component3.Value < component3.MaxResource) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.FuelResource, component3.Value, component3.MaxResource, textColor4);
		}
		if (item.TryGetItemComponent<SideEffectComponent>(out var component4))
		{
			string textColor5 = ((component4.Value < component4.MaxResource) ? _ED3E._E000(250331) : _ED3E._E000(235968));
			_E004(EItemAttributeId.PoisonedWeapon, component4.Value, component4.MaxResource, textColor5);
		}
		if (item.TryGetItemComponent<KeyComponent>(out var component5))
		{
			int num3 = component5.Template.MaximumNumberOfUsage - component5.NumberOfUsages;
			string textColor6 = (((1f - (float)num3 / (float)component5.Template.MaximumNumberOfUsage) * 100f > 10f) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.Durability, num3, component5.Template.MaximumNumberOfUsage, textColor6);
		}
		if (item is _EA8D obj)
		{
			float resource = obj.Resource;
			int maxRepairResource = obj.MaxRepairResource;
			string textColor7 = ((resource < (float)maxRepairResource) ? _ED3E._E000(250331) : _ED3E._E000(243500));
			_E004(EItemAttributeId.RepairResource, resource, maxRepairResource, textColor7);
		}
	}

	private void _E002(_EAF6 eventArgs)
	{
		if (eventArgs.Item.TemplateId == _E354.Item.TemplateId)
		{
			_E001();
		}
	}

	public void UpdateItemStackCountLabel()
	{
		_offerItemView.UpdateInfo();
		_E001();
	}

	private void _E003()
	{
		_E2A3 = null;
	}

	private void _E004(EItemAttributeId iconType, float value, float maxValue, string textColor)
	{
		if (!(maxValue <= 1f))
		{
			OfferViewResourceCounter offerViewResourceCounter = UnityEngine.Object.Instantiate(_resourceCounterTemplate, _resourceCountersContainer, worldPositionStays: false);
			_E356.Add(offerViewResourceCounter);
			Sprite attributeIcon = EFTHardSettings.Instance.StaticIcons.GetAttributeIcon(iconType);
			string resource = string.Format(_ED3E._E000(243492), textColor, Mathf.FloorToInt(value), Mathf.RoundToInt(maxValue));
			offerViewResourceCounter.Show(attributeIcon, resource);
		}
	}

	private void _E005()
	{
		foreach (OfferViewResourceCounter item in _E356)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		_E356.Clear();
	}

	private void _E006()
	{
		UI.Dispose();
		_E003();
		_E005();
		if (_E02A != null && _E02A.gameObject.activeSelf)
		{
			_E02A.Close();
		}
		if (_E092 != null)
		{
			_E092.ExamineEvent -= _E002;
		}
		_offerItemView.IsStub = true;
		_offerItemView.Kill();
	}

	public override void Close()
	{
		_E006();
		base.Close();
	}
}
