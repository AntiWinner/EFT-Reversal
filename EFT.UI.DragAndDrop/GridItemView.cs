using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class GridItemView : ItemView, _E640, _E63F, _E641, _E642, _E643, _E644, _E646, _E647, _E649, _E64A, _E64B, _E645
{
	protected enum EItemValueFormat
	{
		OneValue,
		TwoValues,
		Other
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public GridItemView _003C_003E4__this;

		public Weapon weapon;

		public FilterPanel filterPanel;

		internal void _E000()
		{
			_003C_003E4__this.HoverTrigger.OnHoverStart -= _003C_003E4__this._E006;
			_003C_003E4__this.HoverTrigger.OnHoverEnd -= _003C_003E4__this._E005;
		}

		internal void _E001()
		{
			_003C_003E4__this.ItemContext.OnInventoryError -= _003C_003E4__this._E001;
		}

		internal void _E002()
		{
			weapon.MalfState.OnStateChanged -= _003C_003E4__this.UpdateInfo;
		}

		internal void _E003()
		{
			_003C_003E4__this.ItemOwner.UnregisterView(_003C_003E4__this);
		}

		internal void _E004()
		{
			_003C_003E4__this.ItemController.UnregisterView(_003C_003E4__this);
		}

		internal void _E005()
		{
			_003C_003E4__this.ItemController.ExamineEvent -= _003C_003E4__this._E00A;
		}

		internal void _E006()
		{
			filterPanel.CurrentFilterChanged -= _003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public RectTransform customObject;

		internal void _E000()
		{
			UnityEngine.Object.Destroy(customObject.gameObject);
		}
	}

	private const float _E02E = 40f;

	private const float _E02F = 12f;

	private static Color _E030 = Color.green;

	private static Color _E031 = Color.yellow;

	[SerializeField]
	private Image _unsearchedBackground;

	[SerializeField]
	private Image _secureIcon;

	[SerializeField]
	private Image _lockedIcon;

	[SerializeField]
	private Image _togglableIcon;

	[SerializeField]
	private Image _tagColor;

	[SerializeField]
	private ItemViewStats _itemViewStats;

	[SerializeField]
	protected TextMeshProUGUI ItemInscription;

	[SerializeField]
	protected TextMeshProUGUI ItemValue;

	[SerializeField]
	protected TextMeshProUGUI Caption;

	[SerializeField]
	private TextMeshProUGUI TagName;

	[SerializeField]
	private BindPanel _bindPanel;

	[SerializeField]
	private RectTransform _infoPanel;

	[SerializeField]
	private GameObject _resizeRectPanelTemplate;

	private RectTransform _E032;

	private FilterPanel _E033;

	private _ECB1 _E034;

	private bool _E035;

	private bool _E036;

	protected virtual string ValueFormat => _ED3E._E000(235707);

	private _ECB4 _E000 => _ECB4.FindOrCreate(base.Item);

	protected Image SecureIcon => _secureIcon;

	protected Image LockedIcon => _lockedIcon;

	protected Image TogglableIcon => _togglableIcon;

	protected override bool IsInteractable => true;

	protected override void OnBeingExaminedChanged(bool isBeingExamined)
	{
		base.OnBeingExaminedChanged(isBeingExamined);
		ItemInscription.gameObject.SetActive(!isBeingExamined);
		ItemValue.gameObject.SetActive(!isBeingExamined);
		Caption.gameObject.SetActive(!isBeingExamined);
	}

	public static GridItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, [CanBeNull] ItemUiContext itemUiContext, _ECB1 insurance, bool isSearched)
	{
		GridItemView gridItemView = ItemViewFactory.CreateFromPool<GridItemView>(_ED3E._E000(235742));
		gridItemView.NewGridItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance, isSearched);
		gridItemView.Init();
		return gridItemView;
	}

	protected GridItemView NewGridItemView(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, [CanBeNull] ItemUiContext itemUiContext, _ECB1 insurance, bool isSearched = true)
	{
		_E036 = _E7A3.InRaid;
		NewItemView(item, sourceContext, rotation, itemController, container, itemOwner, itemUiContext);
		base.IsSearched = isSearched;
		IsConflicting = false;
		base.RectTransform.anchorMin = new Vector2(0f, 0f);
		base.RectTransform.anchorMax = new Vector2(0f, 0f);
		_E034 = insurance;
		_E033 = filterPanel;
		if (_E034 != null)
		{
			CompositeDisposable.SubscribeEvent(insurance.OnItemInsured, ChangeInsuredStatus);
			ChangeInsuredStatus(this._E000);
			if (insurance.Insured(base.Item.Id))
			{
				if (this._E000.InsurerId != null)
				{
					HoverTrigger.OnHoverStart += _E006;
					HoverTrigger.OnHoverEnd += _E005;
					CompositeDisposable.AddDisposable(delegate
					{
						HoverTrigger.OnHoverStart -= _E006;
						HoverTrigger.OnHoverEnd -= _E005;
					});
				}
				else
				{
					insurance.Logger.LogError(_ED3E._E000(235730), base.Item.ShortName.Localized());
				}
			}
		}
		ChangeRepairBuffStatus();
		base.ItemContext.OnInventoryError += _E001;
		CompositeDisposable.AddDisposable(delegate
		{
			base.ItemContext.OnInventoryError -= _E001;
		});
		if (_E036 && _E00B(item, out var weapon))
		{
			weapon.MalfState.OnStateChanged += UpdateInfo;
			CompositeDisposable.AddDisposable(delegate
			{
				weapon.MalfState.OnStateChanged -= UpdateInfo;
			});
		}
		if (ItemOwner != null)
		{
			ItemOwner.RegisterView(this);
			CompositeDisposable.AddDisposable(delegate
			{
				ItemOwner.UnregisterView(this);
			});
		}
		if (ItemController != ItemOwner && ItemController != null)
		{
			ItemController.RegisterView(this);
			CompositeDisposable.AddDisposable(delegate
			{
				ItemController.UnregisterView(this);
			});
		}
		if (ItemController != null)
		{
			ItemController.ExamineEvent += _E00A;
			CompositeDisposable.AddDisposable(delegate
			{
				ItemController.ExamineEvent -= _E00A;
			});
		}
		SetItemBinding(ItemView.GetBindingForItem(ItemController, base.Item));
		CompositeDisposable.BindState(base.IsBeingDragged, _E004);
		_E007();
		if (filterPanel == null)
		{
			return this;
		}
		_E000();
		filterPanel.CurrentFilterChanged += _E000;
		CompositeDisposable.AddDisposable(delegate
		{
			filterPanel.CurrentFilterChanged -= _E000;
		});
		return this;
	}

	private void _E000()
	{
		base.IsFilteredOut.Value = !_E033.IsFilteredSingleItem(base.Item);
	}

	private void _E001(InventoryError error)
	{
		_EB29._E00B obj = error as _EB29._E00B;
		bool flag = obj != null && obj.ConflictingItem == base.Item;
		if (_E035 == flag)
		{
			return;
		}
		_E035 = flag;
		if (_E035)
		{
			if (_E032 == null)
			{
				_E032 = (RectTransform)UnityEngine.Object.Instantiate(_resizeRectPanelTemplate, base.transform, worldPositionStays: false).transform;
				_E032.gameObject.SetActive(_E035);
			}
			_E032.sizeDelta = ItemViewFactory.GetCellPixelSize(((_EB22)obj.ConflictingItem.Parent).LocationInGrid.r.Rotate(obj.NewSize));
			base.transform.SetAsLastSibling();
		}
		else if (_E032 != null)
		{
			UnityEngine.Object.Destroy(_E032.gameObject);
		}
		UpdateInfo();
	}

	private void _E002(_EB69 dragItemContext)
	{
		if (Container.CanAccept(dragItemContext, base.ItemContext, out var _))
		{
			if (base.Item is _EA40 obj && obj.Grids.Any())
			{
				SelectedColor = _E031;
			}
			else if (base.Item.StackMaxSize > 1)
			{
				SelectedColor = _E031;
			}
			else
			{
				SelectedColor = _E030;
			}
		}
		else
		{
			if (!CanInteract(dragItemContext))
			{
				_E003();
				return;
			}
			SelectedColor = _E030;
		}
		SelectedColor.a = 10f / 51f;
		HighlightedGlobally = true;
		UpdateColor();
	}

	private void _E003()
	{
		SelectedColor = ItemView.DefaultSelectedColor;
		HighlightedGlobally = false;
		UpdateColor();
	}

	protected override void CheckAcceptHandler(_EB69 dragItemContext)
	{
		base.CheckAcceptHandler(dragItemContext);
		if (Container != null && base.ItemContext.IsPreviewHighlightAvailable)
		{
			if (dragItemContext != null)
			{
				_E002(dragItemContext);
			}
			else
			{
				_E003();
			}
		}
	}

	private void _E004(bool dragged)
	{
		Animator.SetDragState(dragged);
	}

	private void _E005(PointerEventData eventData)
	{
		ItemUiContext.Tooltip.Close();
		ShowTooltip();
	}

	private void _E006(PointerEventData eventData)
	{
		HideTooltip();
		ItemUiContext.Tooltip.Show(_ED3E._E000(235750) + _ED3E._E000(235793).Localized() + _ED3E._E000(235780) + Singleton<_E5CB>.Instance.TradersSettings[this._E000.InsurerId].Nickname.Localized() + _ED3E._E000(59467));
	}

	protected void ChangeInsuredStatus(_ECB4 item)
	{
		if (!(item.Id != this._E000.Id))
		{
			if (InsuredItemBorder != null)
			{
				InsuredItemBorder.SetActive(_E034.Insured(base.Item.Id));
			}
			if (InsuredIcon != null)
			{
				InsuredIcon.SetActive(_E034.Insured(base.Item.Id));
			}
		}
	}

	protected void ChangeRepairBuffStatus()
	{
		if (!(RepairBuffIcon == null))
		{
			BuffComponent itemComponent = base.Item.GetItemComponent<BuffComponent>();
			if (itemComponent != null && itemComponent.IsActive && Examined)
			{
				RepairBuffIcon.sprite = EFTHardSettings.Instance.StaticIcons.GetAttributeIcon(itemComponent.BuffAttributeId);
				RepairBuffIcon.SetNativeSize();
				RepairBuffIcon.gameObject.SetActive(value: true);
			}
			else
			{
				RepairBuffIcon.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnRefreshItem(_EAFF eventArgs)
	{
		if (eventArgs.Item == base.Item && !(MainImage == null))
		{
			ItemRotation = ItemRotation;
			base.IsBeingDragged.Value = false;
			UpdateInfo();
			if (eventArgs.RefreshIcon)
			{
				RefreshIcon();
			}
			_E007();
		}
	}

	private void _E007()
	{
		if (_tagColor == null || TagName == null)
		{
			return;
		}
		TagComponent itemComponent = base.Item.GetItemComponent<TagComponent>();
		if (itemComponent == null)
		{
			_tagColor.gameObject.SetActive(value: false);
			return;
		}
		string text = itemComponent.Name;
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text.Trim()))
		{
			_tagColor.gameObject.SetActive(value: false);
			return;
		}
		_tagColor.gameObject.SetActive(value: true);
		_tagColor.color = EditTagWindow.GetColor(itemComponent.Color);
		TagName.text = text;
		_E008().HandleExceptions();
	}

	private async Task _E008()
	{
		TagName.gameObject.SetActive(value: false);
		await Task.Yield();
		RectTransform rectTransform = _tagColor.rectTransform;
		float num = base.RectTransform.sizeDelta.x - Caption.renderedWidth - 2f;
		if (num < 40f)
		{
			rectTransform.sizeDelta = new Vector2(base.RectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
			return;
		}
		TagName.gameObject.SetActive(value: true);
		float x = Mathf.Clamp(TagName.preferredWidth + 12f, 40f, num);
		rectTransform.sizeDelta = new Vector2(x, _tagColor.rectTransform.sizeDelta.y);
	}

	public virtual void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			_E009(eventArgs.To);
		}
	}

	public virtual void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			_E009(eventArgs.From);
		}
	}

	public void OnMagazineChange(_EA6A magazine)
	{
		if (magazine == base.Item)
		{
			UpdateInfo();
		}
	}

	public void OnInventoryMagazineCheck(_EA6A magazine, float speed, bool status)
	{
		if (magazine == base.Item)
		{
			SetInventoryCheckMagazineStatus(speed, status);
		}
	}

	private void _E009([CanBeNull] ItemAddress location)
	{
		if (!(this == null) && location.IsChildOf(base.Item, notMergedWithThisItem: false))
		{
			RefreshIcon();
			UpdateInfo();
			_E007();
		}
	}

	public void OnUnbindItem(_EAFD eventArgs)
	{
		if (eventArgs.Item == base.Item)
		{
			SetItemBinding(null);
		}
	}

	public void OnBindItem(_EAFC eventArgs)
	{
		if (eventArgs.Item == base.Item)
		{
			SetItemBinding(eventArgs.Index);
		}
	}

	public void OnDrain(_EB01 eventArgs)
	{
		if (eventArgs.Item == base.Item)
		{
			base.IsBeingDrained.Value = eventArgs.Status == CommandStatus.Begin;
		}
	}

	private void _E00A(_EAF6 eventArgs)
	{
		if (eventArgs.Item == base.Item)
		{
			SetBeingExaminedState(eventArgs);
			UpdateStaticInfo();
			UpdateInfo();
		}
		else if (!(eventArgs.Item.TemplateId != base.Item.TemplateId))
		{
			UpdateStaticInfo();
			UpdateInfo();
		}
	}

	public void OnLoadMagazine(_EAF7 eventArgs)
	{
		if (eventArgs.TargetItem.GetRootMergedItem() == base.Item)
		{
			SetLoadMagazineStatus(eventArgs);
		}
		if (eventArgs.Item == base.Item)
		{
			SetLoadAmmoStatus(eventArgs);
		}
	}

	public void OnUnloadMagazine(_EAF8 eventArgs)
	{
		if (eventArgs.FromItem == base.Item)
		{
			SetUnloadMagazineStatus(eventArgs);
		}
		if (eventArgs.TargetItem == base.Item)
		{
			SetLoadAmmoStatus(eventArgs);
		}
	}

	public void OnMerge(_EB09 eventArgs)
	{
		if (eventArgs.Item == base.Item)
		{
			SetLoadAmmoStatus(eventArgs);
		}
	}

	[CanBeNull]
	protected virtual string GetErrorText()
	{
		if (base.RemoveError.Value != null)
		{
			string text = ((base.RemoveError.Value is InventoryError inventoryError) ? inventoryError.GetLocalizedDescription() : base.RemoveError.Value.ToString());
			return _ED3E._E000(103088) + text + _ED3E._E000(59467);
		}
		return null;
	}

	protected virtual void ShowTooltip()
	{
		string errorText = GetErrorText();
		if (errorText != null)
		{
			ItemUiContext.Tooltip.Show(_ED3E._E000(103088) + errorText + _ED3E._E000(59467));
			return;
		}
		if (base.Item is Weapon weapon)
		{
			if (ItemController.HasKnownMalfunction(weapon))
			{
				if (!ItemController.HasKnownMalfType(weapon))
				{
					ItemUiContext.Tooltip.Show(_ED3E._E000(103088) + _E893.GetLocalizedDescription(withKey: false) + _ED3E._E000(59467));
				}
				else
				{
					ItemUiContext.Tooltip.Show(_ED3E._E000(103088) + _E892.GetLocalizedDescription(weapon.MalfState.State, withKey: false) + _ED3E._E000(59467));
				}
				return;
			}
			if (!weapon.CompatibleAmmo)
			{
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				ItemUiContext.Tooltip.Show(_ED3E._E000(103088) + string.Format(_ED3E._E000(162442).Localized() + _ED3E._E000(59467), _ED3E._E000(235823) + ((currentMagazine != null) ? currentMagazine.Cartridges.Last.Name.Localized() : string.Empty) + _ED3E._E000(59467), _ED3E._E000(235823) + weapon.AmmoCaliber + _ED3E._E000(59467)));
				return;
			}
		}
		if (IsTeammateDogtag)
		{
			ItemUiContext.Tooltip.Show(_ED3E._E000(235869).Localized());
			return;
		}
		string text = (Examined ? Singleton<_E63B>.Instance.BriefItemName(base.Item, base.Item.Name.Localized()) : _ED3E._E000(193009).Localized());
		if (base.Item is _EA97 && _E036)
		{
			text = text + _ED3E._E000(235872) + _ED3E._E000(235926).Localized() + _ED3E._E000(59467);
		}
		ItemUiContext.Tooltip.Show(text, null, 1.5f);
	}

	protected virtual void HideTooltip()
	{
		if (ItemUiContext.Tooltip.Displayed)
		{
			if (ItemUiContext.Tooltip != null)
			{
				ItemUiContext.Tooltip.Close();
			}
			else
			{
				Debug.LogWarning(_ED3E._E000(235956) + base.name + _ED3E._E000(261694));
			}
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (base.IsSearched)
		{
			ShowTooltip();
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.ItemContext.InventoryError = null;
		base.OnPointerExit(eventData);
		HideTooltip();
	}

	protected override void UpdateStaticInfo()
	{
		base.UpdateStaticInfo();
		ItemInscription.text = "";
		_itemViewStats.SetStaticInfo(base.Item, Examined);
	}

	protected static string GetStackColor(Item item)
	{
		string result = _ED3E._E000(235944);
		if (item != null)
		{
			if (!(item is _EA76))
			{
				if (item is _EA12)
				{
					result = _ED3E._E000(235992);
				}
			}
			else
			{
				result = _ED3E._E000(235936);
			}
		}
		return result;
	}

	public override void UpdateInfo()
	{
		if (!base.IsSearched)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			_unsearchedBackground.gameObject.SetActive(value: true);
			return;
		}
		if ((object)_unsearchedBackground != null)
		{
			_unsearchedBackground.gameObject.SetActive(value: false);
		}
		Weapon weapon = base.Item as Weapon;
		_EA6A obj = base.Item as _EA6A;
		AmmoBox ammoBox = base.Item as AmmoBox;
		MedKitComponent itemComponent = base.Item.GetItemComponent<MedKitComponent>();
		FoodDrinkComponent itemComponent2 = base.Item.GetItemComponent<FoodDrinkComponent>();
		ArmorComponent itemComponent3 = base.Item.GetItemComponent<ArmorComponent>();
		DogtagComponent itemComponent4 = base.Item.GetItemComponent<DogtagComponent>();
		ResourceComponent itemComponent5 = base.Item.GetItemComponent<ResourceComponent>();
		KeyComponent itemComponent6 = base.Item.GetItemComponent<KeyComponent>();
		SideEffectComponent itemComponent7 = base.Item.GetItemComponent<SideEffectComponent>();
		_EA8D obj2 = base.Item as _EA8D;
		bool examined = Examined;
		string text = ((itemComponent4 == null || string.IsNullOrEmpty(itemComponent4.Nickname)) ? (examined ? Singleton<_E63B>.Instance.BriefItemName(base.Item, base.Item.ShortName.Localized()) : _ED3E._E000(252232)) : (examined ? itemComponent4.Nickname.SubstringIfNecessary(20) : _ED3E._E000(252232)));
		Caption.text = _ED3E._E000(235984) + text + _ED3E._E000(59467);
		ItemValue.text = string.Empty;
		SetCountValue();
		SetQuestItemViewPanel();
		LockableComponent itemComponent8 = base.Item.GetItemComponent<LockableComponent>();
		_lockedIcon.gameObject.SetActive(itemComponent8?.Locked ?? false);
		TogglableComponent itemComponent9 = base.Item.GetItemComponent<TogglableComponent>();
		_togglableIcon.gameObject.SetActive(itemComponent9 != null);
		if (itemComponent9 != null)
		{
			_togglableIcon.sprite = (itemComponent9.On ? EFTHardSettings.Instance.StaticIcons.TogglableOn : EFTHardSettings.Instance.StaticIcons.TogglableOff);
		}
		if (ItemViewFactory.IsSecureContainer(base.Item) && !base.IsBeingExamined.Value)
		{
			_secureIcon.gameObject.SetActive(value: true);
			_lockedIcon.gameObject.SetActive(value: false);
		}
		else
		{
			_secureIcon.gameObject.SetActive(value: false);
		}
		if (weapon != null)
		{
			_EA6A currentMagazine = weapon.GetCurrentMagazine();
			bool flag = currentMagazine?.IsAmmoCompatible(weapon.Chambers) ?? false;
			weapon.CompatibleAmmo = flag || currentMagazine == null;
			string text2 = (examined ? weapon.AmmoCaliber : _ED3E._E000(91186));
			ItemInscription.text = text2;
			ItemInscription.gameObject.SetActive(text2 != null);
			int num = 0;
			int num2 = 0;
			EItemValueFormat format = EItemValueFormat.TwoValues;
			string color = _ED3E._E000(29694);
			if (currentMagazine != null)
			{
				num = currentMagazine.Count;
				num2 = currentMagazine.MaxCount;
			}
			else if (weapon.ReloadMode == Weapon.EReloadMode.OnlyBarrel)
			{
				num = weapon.Chambers.Select((Slot x) => x.ContainedItem).OfType<_EA12>().Count();
				num2 = weapon.Chambers.Length;
			}
			if (num2 > 0)
			{
				color = (((float)num / (float)num2 < 0.15f) ? _ED3E._E000(29694) : _ED3E._E000(235992));
				format = EItemValueFormat.Other;
			}
			SetItemValue(format, examined, color, num, num2);
		}
		else if (itemComponent7 != null && itemComponent7.Value.Positive())
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, (itemComponent7.Value / itemComponent7.MaxResource < 0.15f) ? _ED3E._E000(29694) : _ED3E._E000(235968), (int)itemComponent7.Value, itemComponent7.MaxResource, _ED3E._E000(235968));
		}
		else if (obj != null)
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(235992), obj.Count, obj.MaxCount);
		}
		else if (ammoBox != null)
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(235992), ammoBox.Count, ammoBox.MaxCount);
		}
		else if (itemComponent != null)
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(236024), Mathf.RoundToInt(itemComponent.HpResource), Mathf.RoundToInt(itemComponent.MaxHpResource));
		}
		else if (itemComponent2 != null && itemComponent2.MaxResource > 1f)
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(236024), Mathf.RoundToInt(itemComponent2.HpPercent), Mathf.RoundToInt(itemComponent2.MaxResource));
		}
		else if (itemComponent3 != null)
		{
			List<RepairableComponent> list = (from x in itemComponent3.Item.GetItemComponentsInChildren<ArmorComponent>()
				where x.ArmorClass > 0
				select x.Repairable).ToList();
			if (list.Count > 0)
			{
				float num3 = 0f;
				float num4 = 0f;
				foreach (RepairableComponent item in list)
				{
					num3 += item.Durability;
					num4 += item.MaxDurability;
				}
				SetItemValue(EItemValueFormat.TwoValues, examined, (num3 <= 0f) ? _ED3E._E000(236016) : _ED3E._E000(245824), Math.Round(num3, 1), Math.Round(num4, 1));
			}
		}
		else if (itemComponent4 != null)
		{
			ItemInscription.text = itemComponent4.Level.ToString();
		}
		else if (itemComponent5 != null && itemComponent5.MaxResource > 0f)
		{
			SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(235992), (int)itemComponent5.Value, itemComponent5.MaxResource);
		}
		else if (itemComponent6 != null)
		{
			int maximumNumberOfUsage = itemComponent6.Template.MaximumNumberOfUsage;
			if (maximumNumberOfUsage > 0)
			{
				int num5 = maximumNumberOfUsage - itemComponent6.NumberOfUsages;
				SetItemValue(EItemValueFormat.TwoValues, examined, _ED3E._E000(236024), Mathf.RoundToInt(num5), Mathf.RoundToInt(maximumNumberOfUsage));
			}
		}
		else if (obj2 != null)
		{
			float resource = obj2.Resource;
			float num6 = (float)obj2.MaxRepairResource / 2f;
			SetItemValue(EItemValueFormat.TwoValues, examined, (Math.Ceiling(resource) <= 0.0) ? _ED3E._E000(236016) : _ED3E._E000(245824), (resource > num6) ? Math.Floor(resource) : Math.Ceiling(resource), obj2.MaxRepairResource);
		}
		bool flag2 = (base.Item is _EA40 obj3 && obj3.MissingVitalParts.Any()) || (_E036 && ((base.Item is Weapon weapon2 && (!weapon2.CompatibleAmmo || ItemController.HasKnownMalfunction(weapon2))) || _E00C(base.Item) || _E00D(base.Item)));
		if (!base.IsBeingLoadedAmmo.Value && !base.IsBeingLoadedMagazine.Value && !base.IsBeingUnloadedMagazine.Value)
		{
			if (!examined)
			{
				MainImage.color = new Color(0f, 0f, 0f, 0.75f);
				BackgroundColor = new Color32(0, 0, 0, 100);
			}
			else if (flag2 || _E035 || IsConflicting)
			{
				MainImage.color = Color.red;
				BackgroundColor = new Color32(83, 0, 0, 63);
			}
			else
			{
				BuffComponent itemComponent10 = base.Item.GetItemComponent<BuffComponent>();
				if (itemComponent10 != null && itemComponent10.IsActive)
				{
					MainImage.color = Color.white;
					BackgroundColor = new Color32(209, 153, 45, 32);
				}
				else
				{
					MainImage.color = Color.white;
					BackgroundColor = OriginalBackgroundColor;
				}
				ChangeRepairBuffStatus();
			}
		}
		Animator.SetExaminedState(examined);
		ItemValue.gameObject.SetActive(ItemValue.text.Length > 0);
		if (!base.IsBeingLoadedMagazine.Value && !base.IsBeingUnloadedMagazine.Value)
		{
			UpdateColor();
		}
	}

	protected virtual void SetCountValue()
	{
		if (base.Item.UnlimitedCount)
		{
			SetItemValue(EItemValueFormat.OneValue, display: true, GetStackColor(base.Item), _ED3E._E000(254994).Localized());
		}
		else if (base.Item.StackObjectsCount != 1)
		{
			SetItemValue(EItemValueFormat.OneValue, display: true, GetStackColor(base.Item), base.Item.StackObjectsCount);
		}
	}

	private bool _E00B(Item item, out Weapon weapon)
	{
		if (item != null)
		{
			if (item is Weapon weapon2)
			{
				Weapon weapon3 = weapon2;
				weapon = weapon3;
				return true;
			}
			if (item is _EA6A obj)
			{
				_EA6A item2 = obj;
				weapon = item2.GetRootItem() as Weapon;
				return weapon != null;
			}
			if (item is _EA12 obj2)
			{
				_EA12 obj3 = obj2;
				weapon = obj3.CurrentAddress?.GetRootItem() as Weapon;
				return weapon != null;
			}
		}
		weapon = null;
		return false;
	}

	private bool _E00C(Item item)
	{
		if (!(item is _EA6A) || !(base.Item.GetRootItem() is Weapon weapon))
		{
			return false;
		}
		if (weapon.MalfState.State == Weapon.EMalfunctionState.Feed)
		{
			return ItemController.HasKnownMalfunction(weapon);
		}
		return false;
	}

	private bool _E00D(Item item)
	{
		if (!(item is _EA12) || !(base.Item.CurrentAddress?.GetRootItem() is Weapon weapon))
		{
			return false;
		}
		if (weapon.MalfState.State != 0)
		{
			return ItemController.HasKnownMalfunction(weapon);
		}
		return false;
	}

	protected virtual void SetItemValue(EItemValueFormat format, bool display, string color, object arg1, [CanBeNull] object arg2 = null, [CanBeNull] string color2 = null)
	{
		if (base.Item is Weapon && arg2 != null && (int)arg2 == 0)
		{
			ItemValue.text = string.Empty;
			return;
		}
		if (!display)
		{
			arg1 = _ED3E._E000(91186);
			arg2 = _ED3E._E000(91186);
		}
		else if (ItemController is Player.PlayerInventoryController playerInventoryController)
		{
			_EA6A currentMagazine = base.Item.GetCurrentMagazine();
			if (currentMagazine != null)
			{
				Profile profile = playerInventoryController.Profile;
				int skill = Mathf.Max(profile.MagDrillsMastering, profile.CheckedMagazineSkillLevel(currentMagazine.Id), currentMagazine.CheckOverride);
				ItemValue.text = currentMagazine.GetAmmoCountByLevel((int)arg1, (int)arg2, skill, color, playerInventoryController.CheckedMagazine(currentMagazine), playerInventoryController.IsItemEquipped(base.Item), ValueFormat);
				return;
			}
		}
		switch (format)
		{
		case EItemValueFormat.OneValue:
			ItemValue.text = string.Format(_ED3E._E000(236008), color, arg1);
			break;
		case EItemValueFormat.TwoValues:
			ItemValue.text = string.Format(_ED3E._E000(245880), color, arg1, color2 ?? color, arg2);
			break;
		case EItemValueFormat.Other:
			ItemValue.text = string.Format(ValueFormat, arg1, arg2, color);
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(236055), format, null);
		}
	}

	protected override void UpdateInfoVisibility(bool isVisible)
	{
		base.UpdateInfoVisibility(isVisible);
		_infoPanel.gameObject.SetActive(isVisible);
	}

	protected void SetItemBinding(EBoundItem? slotName)
	{
		if (slotName.HasValue)
		{
			_bindPanel.Show(slotName.Value);
		}
		else
		{
			_bindPanel.Hide();
		}
	}

	public void AddCustomObjectToInfoPanel(RectTransform customObject, Vector2 anchoredPosition)
	{
		customObject.SetParent(_infoPanel, worldPositionStays: false);
		customObject.anchoredPosition = anchoredPosition;
		CompositeDisposable.AddDisposable(delegate
		{
			UnityEngine.Object.Destroy(customObject.gameObject);
		});
	}

	public override void Kill()
	{
		_E003();
		if (ItemUiContext != null && ItemUiContext.Tooltip != null)
		{
			ItemUiContext.Tooltip.Close();
		}
		base.Kill();
	}
}
