using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[UsedImplicitly]
public sealed class ItemSpecificationPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E750 mastered;

		public ItemSpecificationPanel _003C_003E4__this;

		internal float _E000()
		{
			return _003C_003E4__this._E000 ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public float maxDurability;

		internal float _E000()
		{
			return maxDurability;
		}

		internal string _E001()
		{
			return Math.Round(maxDurability, 1).ToString(CultureInfo.InvariantCulture);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string[] weaponNames;

		internal string _E000()
		{
			if (weaponNames.Length == 0)
			{
				return _ED3E._E000(247332).Localized();
			}
			return weaponNames.CastToStringValue(_ED3E._E000(2540), localized: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E003<_E0B7> where _E0B7 : CompactCharacteristicPanel
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 attr)
		{
			return attribute.Id.Equals(attr.Id);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public float cellSize;

		internal bool _E000(KeyValuePair<_EB10, CompactCharacteristicPanel> view)
		{
			return 30f + view.Value.TextWidth > cellSize;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public Slot slot;

		public ItemSpecificationPanel _003C_003E4__this;

		internal void _E000(_EB69 dragItemContext)
		{
			if (dragItemContext != null)
			{
				_003C_003E4__this._E010(slot, dragItemContext);
			}
			else if (_003C_003E4__this._E141 != null)
			{
				_003C_003E4__this._E010(_003C_003E4__this._E141, null);
			}
			else
			{
				_003C_003E4__this._E00F();
			}
		}

		internal void _E001()
		{
			_003C_003E4__this._E141 = slot;
			_003C_003E4__this._E010(slot, null);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public ModSlotView modSlotView;

		public _E005 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			modSlotView.OnItemHover -= delegate(_EB69 dragItemContext)
			{
				if (dragItemContext != null)
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this._E010(CS_0024_003C_003E8__locals1.slot, dragItemContext);
				}
				else if (CS_0024_003C_003E8__locals1._003C_003E4__this._E141 != null)
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this._E010(CS_0024_003C_003E8__locals1._003C_003E4__this._E141, null);
				}
				else
				{
					CS_0024_003C_003E8__locals1._003C_003E4__this._E00F();
				}
			};
			modSlotView.OnDragStarted -= delegate
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this._E141 = CS_0024_003C_003E8__locals1.slot;
				CS_0024_003C_003E8__locals1._003C_003E4__this._E010(CS_0024_003C_003E8__locals1.slot, null);
			};
			modSlotView.OnDragCancelled -= CS_0024_003C_003E8__locals1._003C_003E4__this._E00F;
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public ItemSpecificationPanel _003C_003E4__this;

		public List<_EB10> newAttributes;

		internal bool _E000(_EB10 attribute)
		{
			_E008 CS_0024_003C_003E8__locals0 = new _E008
			{
				attribute = attribute
			};
			return _003C_003E4__this._E085.Attributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null;
		}

		internal bool _E001(_EB10 attribute)
		{
			_E009 CS_0024_003C_003E8__locals0 = new _E009
			{
				attribute = attribute
			};
			if (_003C_003E4__this._E085.Attributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null)
			{
				return newAttributes.Find((_EB10 a) => a.Id.Equals(CS_0024_003C_003E8__locals0.attribute.Id)) == null;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public _EB10 attribute;

		internal bool _E000(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}

		internal bool _E001(_EB10 a)
		{
			return a.Id.Equals(attribute.Id);
		}
	}

	private const int _E131 = 6;

	private const int _E132 = 3;

	[SerializeField]
	private ItemInfoWindowLabels _itemLabels;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private CameraImage _cameraImage;

	[SerializeField]
	private QuestItemViewPanel _questItemViewPanel;

	[SerializeField]
	private Scrollbar _modsScrollBar;

	[SerializeField]
	private Transform _modsPanel;

	[SerializeField]
	private RectTransform _modsContainer;

	[SerializeField]
	private GameObject _masteringPanel;

	[SerializeField]
	private TextMeshProUGUI _masteringText;

	[SerializeField]
	private Image _masteringBg;

	[SerializeField]
	private CharacteristicPanel _charPanelTemplate;

	[SerializeField]
	private CharacteristicPanel _charDurability;

	[SerializeField]
	private CharacteristicPanel _charMastering;

	[SerializeField]
	private InteractionButtonsContainer _interactionButtonsContainer;

	[SerializeField]
	private CompactCharacteristicPanel _compactCharTemplate;

	[SerializeField]
	private CompactCharacteristicDropdownPanel _compactCharDropdownTemplate;

	[SerializeField]
	private ExamineCharacteristicPanel _examineCharacteristicPanel;

	[SerializeField]
	private Transform _charPanelContainer;

	[SerializeField]
	private ModSlotView _modSlotViewPrefab;

	[SerializeField]
	private Transform _compactPanel;

	private _EB68 _E133;

	private Item _E085;

	private _EBAB _E134;

	private _EAED _E092;

	private ItemUiContext _E089;

	private _E74F _E135;

	private IItemOwner _E136;

	private SimpleTooltip _E137;

	private GridLayoutGroup _E138;

	private List<string> _E139;

	private _EA40[] _E13A;

	private _EC79<_EB10, CompactCharacteristicPanel> _E13B;

	private _EC79<_EB10, CharacteristicPanel> _E13C;

	private _EC79<_EB10, CompactCharacteristicDropdownPanel> _E13D;

	private WeaponPreviewPool _E13E;

	private WeaponPreview _E130;

	private float _E13F;

	private bool _E140;

	private Slot _E141;

	private readonly _E3A4 _E142 = new _E3A4();

	private LayoutElement _E143;

	private bool _E000
	{
		get
		{
			if (_E140)
			{
				return _E092.Examined(_E085);
			}
			return true;
		}
	}

	private string _E001
	{
		get
		{
			if (!this._E000)
			{
				return _ED3E._E000(193009).Localized();
			}
			return Singleton<_E63B>.Instance.BriefItemName(_E085, _E085.Name.Localized());
		}
	}

	[UsedImplicitly]
	private void Awake()
	{
		_E138 = _compactPanel.GetComponent<GridLayoutGroup>();
		_E143 = GetComponent<LayoutElement>();
	}

	[UsedImplicitly]
	private void Update()
	{
		float width = ((RectTransform)_compactPanel).rect.width;
		float num = width - _E13F;
		if (Mathf.Abs(num) > 1f)
		{
			_E13F = width;
			_E00B(num);
		}
	}

	public void Show(_EB68 itemContext, Profile profile, _EAED inventoryController, _EA40[] playerCollections, _E74F skills, WeaponPreviewPool weaponPreviewPool, ItemUiContext itemUiContext, _EC4E<EItemInfoButton> contextInteractions, SimpleTooltip secondaryTooltip)
	{
		ShowGameObject();
		_loader.SetActive(value: false);
		_E13E = weaponPreviewPool;
		_E130 = _E13E.GetWeaponPreview();
		_E133 = itemContext;
		_E085 = itemContext.Item;
		_EBA8 instance = Singleton<_EBA8>.Instance;
		if (instance?.StructuredItems != null)
		{
			_E134 = instance.StructuredItems[_E085.TemplateId];
		}
		_E139 = profile.CheckedChambers;
		_E092 = inventoryController;
		_E140 = contextInteractions.ExaminationRequired;
		if (Singleton<_EBA8>.Instantiated)
		{
			_E134 = Singleton<_EBA8>.Instance.StructuredItems[_E085.TemplateId];
		}
		_E136 = _E085.Owner;
		_E089 = itemUiContext;
		_E13A = playerCollections;
		_E135 = skills;
		_E137 = secondaryTooltip;
		_cameraImage.InitCamera(_E130.WeaponPreviewCamera);
		_itemLabels.Close();
		_itemLabels.Show(_E130);
		if (_E136 != null)
		{
			_E136.RefreshItemEvent += _E00A;
		}
		_E092.OnChamberCheck += OnRefreshItemEvent;
		_E092.ExamineEvent += OnItemExaminedEvent;
		_E007();
		_E130.ResetRotator(-1.5f);
		_E001();
		_E000();
		_E004(contextInteractions);
	}

	private void _E000()
	{
		_itemLabels._E000(this._E001);
	}

	private void _E001()
	{
		_modsScrollBar.value = 0f;
		string text = string.Join(_ED3E._E000(197193), _E134?.Category.Select((string x) => x.Localized()).ToArray() ?? new string[0]);
		Weapon weapon = _E085 as Weapon;
		_EA40 obj = _E085 as _EA40;
		bool flag = (obj != null && (obj.Slots.Length != 0 || (weapon != null && weapon.Chambers.Length != 0))) || _E085 is _EA62;
		_itemLabels._E001(this._E000 ? text : _ED3E._E000(91186));
		_itemLabels._E002(this._E000 ? _E085.Description.Localized() : string.Empty);
		_itemLabels._E003(_E085.GetTruncatedWeightString() + _ED3E._E000(214011).Localized());
		_examineCharacteristicPanel.Show(_E085, _E092);
		_E005();
		_modsPanel.gameObject.SetActive(flag);
		_masteringBg.gameObject.SetActive(weapon != null);
		_masteringPanel.gameObject.SetActive(weapon != null);
		_E002();
		_charMastering.Dispose();
		_charDurability.Dispose();
		if (weapon != null && _E135 != null)
		{
			_E750 mastered = _E135.GetMastering(weapon.TemplateId);
			_EB11 obj2 = new _EB11(EItemAttributeId.Mastering);
			string format = _ED3E._E000(247107);
			_E750 obj3 = mastered;
			obj2.Name = string.Format(format, (obj3 == null) ? 1 : (obj3.Level + 1));
			_EB11 obj4 = obj2;
			MasteringCharacteristicPanel masteringCharacteristicPanel = _charMastering as MasteringCharacteristicPanel;
			if (mastered != null)
			{
				obj4.Base = () => this._E000 ? ((int)(mastered.LevelProgress * (float)mastered.LevelingThreshold)) : 0;
				if (masteringCharacteristicPanel != null)
				{
					masteringCharacteristicPanel.MaxVal = mastered.LevelingThreshold;
					obj4.Range = new Vector2(0f, mastered.LevelingThreshold);
				}
			}
			else
			{
				obj4.Base = () => 0f;
				if (masteringCharacteristicPanel != null)
				{
					obj4.Range = new Vector2(0f, masteringCharacteristicPanel.MaxVal);
				}
			}
			UI.AddDisposable(_charMastering);
			_charMastering.Show(obj4, _E137, this._E000);
			_charMastering.SetValues();
			TextMeshProUGUI masteringText = _masteringText;
			string format2 = _ED3E._E000(247147);
			_E750 obj5 = mastered;
			masteringText.text = string.Format(format2, (obj5 == null) ? 1 : (obj5.Level + 1));
			UI.AddDisposable(_charDurability);
			_charDurability.Show(weapon.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability)), _E137, this._E000, weapon.Repairable.TemplateDurability);
			weapon.UpdateAttributes();
			_charDurability.gameObject.SetActive(this._E000);
			_charMastering.gameObject.SetActive(this._E000);
		}
		if (flag)
		{
			_E008();
			_E00E(obj);
			Vector3 localPosition = _modsContainer.localPosition;
			localPosition = new Vector3(0f, localPosition.y, localPosition.z);
			_modsContainer.localPosition = localPosition;
		}
		_examineCharacteristicPanel.gameObject.SetActive(!this._E000);
		_cameraImage.SetRawImageColor(this._E000 ? ((Color32)Color.white) : new Color32(0, 0, 0, byte.MaxValue));
	}

	private void _E002()
	{
		if (_questItemViewPanel != null)
		{
			_questItemViewPanel.Show(_E092.Profile, _E085, (_E089 != null) ? _E089.Tooltip : null);
		}
	}

	private void _E003(object sender, EventArgs e)
	{
		CompactCharacteristicPanel compactCharacteristicPanel = sender as CompactCharacteristicPanel;
		if (!(compactCharacteristicPanel == null))
		{
			compactCharacteristicPanel.OnTextWidthCalculated -= _E003;
			_E00B(0f);
		}
	}

	private void _E004([CanBeNull] _EC4E<EItemInfoButton> contextInteractions)
	{
		_interactionButtonsContainer.SubMenuContainer = base.transform;
		_interactionButtonsContainer.Show(contextInteractions, null, new List<EItemInfoButton> { EItemInfoButton.Inspect }, _E085, null, autoClose: false);
	}

	private void _E005()
	{
		_E13C?.Dispose();
		_E13B?.Dispose();
		_E13D?.Dispose();
		List<_EB10> items = _E085.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.FullBar).ToList();
		List<_EB10> list = _E085.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.Compact).ToList();
		List<_EB10> list2 = _E085.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.CompactWithTooltip).ToList();
		ArmorComponent[] source = (from x in _E085.GetItemComponentsInChildren<ArmorComponent>()
			where x.ArmorClass > 0
			select x).ToArray();
		if (source.Any())
		{
			float maxDurability = source.Sum((ArmorComponent x) => x.Repairable.Durability);
			list.Insert(0, new _EB10(EItemAttributeId.ArmorPoints)
			{
				Name = EItemAttributeId.ArmorPoints.GetName(),
				Base = () => maxDurability,
				StringValue = () => Math.Round(maxDurability, 1).ToString(CultureInfo.InvariantCulture),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		if (_E085 is Mod mod)
		{
			Weapon[] source2 = mod.GetSuitableWeapons(_E13A).ToArray();
			string[] weaponNames = source2.Select((Weapon item) => item.ShortName.Localized() + _ED3E._E000(54246) + _E009(item) + _ED3E._E000(27308)).ToArray();
			list2.Add(new _EB10(EItemAttributeId.CompatibleWith)
			{
				Name = EItemAttributeId.CompatibleWith.GetName(),
				StringValue = () => (weaponNames.Length == 0) ? _ED3E._E000(247332).Localized() : weaponNames.CastToStringValue(_ED3E._E000(2540), localized: false),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		_E13C = new _EC79<_EB10, CharacteristicPanel>(items, _charPanelTemplate, _charPanelContainer, delegate(_EB10 attribute, CharacteristicPanel viewer)
		{
			viewer.Show(attribute, _E137, this._E000);
		});
		_E13B = new _EC79<_EB10, CompactCharacteristicPanel>(list, _compactCharTemplate, _compactPanel, delegate(_EB10 attribute, CompactCharacteristicPanel viewer)
		{
			viewer.Show(attribute, _E137, this._E000);
		});
		_E13D = new _EC79<_EB10, CompactCharacteristicDropdownPanel>(list2, _compactCharDropdownTemplate, _compactPanel, delegate(_EB10 attribute, CompactCharacteristicDropdownPanel viewer)
		{
			viewer.Show(attribute, _E137, this._E000);
		});
		if (_E13B.Any())
		{
			_E13B.Last().Value.OnTextWidthCalculated += _E003;
		}
		_E00B(0f);
		_E006(null);
	}

	private void _E006(Item compareItem)
	{
		List<_EB10> changedList = compareItem?.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.FullBar).ToList();
		List<_EB10> changedList2 = compareItem?.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.Compact).ToList();
		List<_EB10> changedList3 = compareItem?.Attributes.Where((_EB10 x) => x.DisplayType() == EItemAttributeDisplayType.CompactWithTooltip).ToList();
		_E018(_E13C, changedList);
		_E018(_E13B, changedList2);
		_E018(_E13D, changedList3);
		if (compareItem is Weapon weapon)
		{
			_charDurability.CompareWith(weapon.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability)));
		}
		else if (compareItem == null)
		{
			_charDurability.CompareWith(null);
		}
	}

	private void _E007()
	{
		_E130.SetupItemPreview(_E085, delegate
		{
			_cameraImage.gameObject.SetActive(value: false);
			_loader.SetActive(value: true);
		}, delegate
		{
			_loader.SetActive(value: false);
			_cameraImage.gameObject.SetActive(value: true);
		}, null, setAsClosest: true, null, enableWeaponLights: false);
	}

	private void _E008()
	{
		_E142.Dispose();
		for (int num = _modsContainer.childCount - 1; num >= 0; num--)
		{
			Transform child = _modsContainer.GetChild(num);
			child.GetComponent<SlotView>().Hide();
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		_E00F();
	}

	private static string _E009(Item item)
	{
		Item parentItem = item.Parent.Container.ParentItem;
		if (parentItem is _EAA0)
		{
			return _ED3E._E000(247136).Localized();
		}
		if (parentItem is _EB0B)
		{
			return _ED3E._E000(247198).Localized();
		}
		if (parentItem.IsContainer)
		{
			return _ED3E._E000(247191).Localized();
		}
		return string.Empty;
	}

	public void OnRemoveFromSlotEvent(_EAF3 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed && _E014(eventArgs.From))
		{
			_E00C();
			_E00D(eventArgs.From);
			_E000();
		}
	}

	private void _E00A(_EAFF eventArgs)
	{
		if (_E085 is _EA6A && eventArgs.Item == _E085)
		{
			_E00C();
		}
	}

	public void OnItemExaminedEvent(_EAF6 eventArgs)
	{
		if (eventArgs.Item.TemplateId == _E085.TemplateId && eventArgs.Status == CommandStatus.Succeed)
		{
			_E001();
			_cameraImage.SetRawImageColor(Color.white);
		}
		_examineCharacteristicPanel.Show(_E085, _E092);
		_E000();
	}

	public void OnItemAddedEvent(_EAF2 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed && _E014(eventArgs.To))
		{
			_E00C();
			_E00D(eventArgs.To);
			_E000();
		}
	}

	public void OnRefreshItemEvent(Weapon weapon)
	{
		if (_E085 == weapon)
		{
			_E008();
			_E00E((_EA40)_E085);
		}
	}

	private void _E00B(float deltaX)
	{
		if (_E138 == null)
		{
			return;
		}
		float cellSize = _E138.cellSize.x + deltaX / 2f;
		if (_E138.constraintCount < 2)
		{
			cellSize = cellSize / 2f - _E138.spacing.x / 2f;
		}
		if (_E13B.Any((KeyValuePair<_EB10, CompactCharacteristicPanel> view) => 30f + view.Value.TextWidth > cellSize))
		{
			if (_E138.constraintCount != 1)
			{
				_E138.constraintCount = 1;
			}
		}
		else if (_E138.constraintCount < 2)
		{
			_E138.constraintCount = 2;
		}
	}

	private void _E00C()
	{
		_E007();
		if (_E085 is Weapon weapon)
		{
			weapon.UpdateAttributes();
		}
		if (_E085 is _EA40 compoundItem)
		{
			_E008();
			_E00E(compoundItem);
		}
		_E002();
		_itemLabels._E003(_E085.GetTruncatedWeightString() + _ED3E._E000(214011).Localized());
	}

	private void _E00D([CanBeNull] ItemAddress ownerLocation)
	{
		if (ownerLocation != null && ownerLocation.GetAllParentItems().Contains(_E085))
		{
			_E005();
		}
	}

	private void _E00E(_EA40 compoundItem)
	{
		if (!this._E000)
		{
			_modsPanel.gameObject.SetActive(value: false);
			return;
		}
		int num = 0;
		foreach (Slot slot in compoundItem.AllSlots)
		{
			num++;
			ModSlotView modSlotView = UnityEngine.Object.Instantiate(_modSlotViewPrefab, _modsContainer, worldPositionStays: true);
			Transform obj = modSlotView.transform;
			obj.localPosition = Vector3.zero;
			obj.localScale = Vector3.one;
			modSlotView.Show(slot, _E133, _E092, _E089);
			modSlotView.SetLocked(_E012(slot), _E085);
			modSlotView.OnItemHover += delegate(_EB69 dragItemContext)
			{
				if (dragItemContext != null)
				{
					_E010(slot, dragItemContext);
				}
				else if (_E141 != null)
				{
					_E010(_E141, null);
				}
				else
				{
					_E00F();
				}
			};
			modSlotView.OnDragStarted += delegate
			{
				_E141 = slot;
				_E010(slot, null);
			};
			modSlotView.OnDragCancelled += _E00F;
			_E142.AddDisposable(delegate
			{
				modSlotView.OnItemHover -= delegate(_EB69 dragItemContext)
				{
					if (dragItemContext != null)
					{
						_E010(slot, dragItemContext);
					}
					else if (_E141 != null)
					{
						_E010(_E141, null);
					}
					else
					{
						_E00F();
					}
				};
				modSlotView.OnDragStarted -= delegate
				{
					_E141 = slot;
					_E010(slot, null);
				};
				modSlotView.OnDragCancelled -= _E00F;
			});
		}
		int num2 = 0;
		RectTransform rectTransform = _modsContainer;
		RectTransform rectTransform2 = base.gameObject.RectTransform();
		while (rectTransform != rectTransform2 && !(rectTransform == null))
		{
			LayoutGroup component = rectTransform.GetComponent<LayoutGroup>();
			if (component != null)
			{
				RectOffset padding = component.padding;
				num2 += padding.left + padding.right;
			}
			rectTransform = rectTransform.parent.gameObject.RectTransform();
		}
		int b = Mathf.CeilToInt((float)num / 3f);
		int num3 = Mathf.Max(6, b);
		GridLayoutGroup component2 = _modsContainer.GetComponent<GridLayoutGroup>();
		float minWidth = (float)num2 + component2.cellSize.x * (float)num3 + component2.spacing.x * (float)(num3 - 1);
		_E143.minWidth = minWidth;
	}

	private void _E00F()
	{
		if (_E085 != null)
		{
			_E141 = null;
			_E011(_E085);
			_E006(null);
			_itemLabels._E003(_E085.GetTruncatedWeightString() + _ED3E._E000(214011).Localized());
		}
	}

	private void _E010(Slot slot, _EB69 itemContext)
	{
		Item item = null;
		float num = 0f;
		_ECD7 operation;
		if (itemContext == null)
		{
			Item containedItem = slot.ContainedItem;
			if (slot.RemoveItem().Succeeded)
			{
				item = _E085.CloneItem();
				num = item.GetSingleItemTotalWeight();
			}
			slot.Add(containedItem, simulate: false);
		}
		else if (itemContext.CanAccept(slot, _E092, out operation, simulate: false))
		{
			item = _E085.CloneItem();
			num = item.GetSingleItemTotalWeight();
			operation.Value.RollBack();
		}
		if (item != null)
		{
			_E011(item);
			_E006(item);
			float singleItemTotalWeight = _E085.GetSingleItemTotalWeight();
			float num2 = num - singleItemTotalWeight;
			_itemLabels._E003(string.Format(_ED3E._E000(247172), _E085.GetTruncatedWeightString(), (num2 > 0f) ? _ED3E._E000(29692) : string.Empty, num2, _ED3E._E000(214011).Localized()));
		}
	}

	private void _E011(Item compareItem)
	{
		List<_EB10> newAttributes = compareItem.Attributes.FindAll((_EB10 attribute) => _E085.Attributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null);
		foreach (_EB10 item in newAttributes)
		{
			if (item.DisplayType() == EItemAttributeDisplayType.FullBar)
			{
				_E13C.Add(item.Clone());
			}
		}
		foreach (_EB10 item2 in _E13C.Keys.FindAll((_EB10 attribute) => _E085.Attributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null && newAttributes.Find((_EB10 a) => a.Id.Equals(attribute.Id)) == null))
		{
			_E13C.Remove(item2);
		}
	}

	private KeyValuePair<EModLockedState, string> _E012(Slot slot)
	{
		string text = ((slot.ContainedItem != null) ? slot.ContainedItem.Name.Localized() : string.Empty);
		if (_E7A3.InRaid && ((slot.ContainedItem is Mod mod && !mod.RaidModdable) || ((_EA40)_E085).VitalParts.Contains(slot)))
		{
			return new KeyValuePair<EModLockedState, string>(EModLockedState.RaidLock, _ED3E._E000(103088) + _ED3E._E000(247222).Localized() + _ED3E._E000(247208) + text);
		}
		if (!_E7A3.InRaid && ((_EA40)_E085).VitalParts.Contains(slot) && _E092.IsItemEquipped(_E085))
		{
			return new KeyValuePair<EModLockedState, string>(EModLockedState.RaidLock, _ED3E._E000(103088) + _ED3E._E000(247203).Localized() + _ED3E._E000(247208) + text);
		}
		if (!_E013(slot))
		{
			return new KeyValuePair<EModLockedState, string>(EModLockedState.Unlocked, text);
		}
		if (slot.ID.StartsWith(_ED3E._E000(247237)))
		{
			return new KeyValuePair<EModLockedState, string>(EModLockedState.ChamberUnchecked, _ED3E._E000(247292) + _ED3E._E000(247276).Localized() + _ED3E._E000(59467));
		}
		return new KeyValuePair<EModLockedState, string>(EModLockedState.ChamberUnchecked, _ED3E._E000(247292) + _ED3E._E000(247304).Localized() + _ED3E._E000(59467));
	}

	private bool _E013(Slot slot)
	{
		if (!_E7A3.InRaid)
		{
			return false;
		}
		if ((slot.ID.StartsWith(_ED3E._E000(225081)) || slot.ID.StartsWith(_ED3E._E000(155651))) && _E085 is Weapon)
		{
			return !_E139.Contains(_E085.Id);
		}
		if (slot.ID.StartsWith(_ED3E._E000(247237)) && _E085 is Weapon weapon && weapon.GetCurrentMagazine() is _EB13)
		{
			return !_E139.Contains(_E085.Id);
		}
		return false;
	}

	public override void Close()
	{
		if (_E136 != null)
		{
			_E136.RefreshItemEvent -= _E00A;
		}
		if (_E092 != null)
		{
			_E092.OnChamberCheck -= OnRefreshItemEvent;
			_E092.ExamineEvent -= OnItemExaminedEvent;
		}
		_interactionButtonsContainer.Close();
		_cameraImage.InitCamera(null);
		_E008();
		if (_E130 != null)
		{
			_E130.Hide();
			_E13E.ReturnToPool(_E130);
			_E130 = null;
		}
		base.Close();
		_E092 = null;
		_E13A = null;
		_E137 = null;
		_E089 = null;
		_E133 = null;
		_E141 = null;
		_E136 = null;
		_E135 = null;
		_E085 = null;
	}

	private bool _E014(ItemAddress address)
	{
		return address.IsChildOf(_E085, notMergedWithThisItem: false);
	}

	[CompilerGenerated]
	private void _E015(_EB10 attribute, CharacteristicPanel viewer)
	{
		viewer.Show(attribute, _E137, this._E000);
	}

	[CompilerGenerated]
	private void _E016(_EB10 attribute, CompactCharacteristicPanel viewer)
	{
		viewer.Show(attribute, _E137, this._E000);
	}

	[CompilerGenerated]
	private void _E017(_EB10 attribute, CompactCharacteristicDropdownPanel viewer)
	{
		viewer.Show(attribute, _E137, this._E000);
	}

	[CompilerGenerated]
	internal static void _E018<_E0B7>(_EC79<_EB10, _E0B7> viewList, IReadOnlyCollection<_EB10> changedList) where _E0B7 : CompactCharacteristicPanel
	{
		foreach (KeyValuePair<_EB10, _E0B7> view in viewList)
		{
			_E39D.Deconstruct(view, out var key, out var value);
			_EB10 attribute = key;
			_E0B7 val = value;
			_EB10 attribute2 = changedList?.FirstOrDefault((_EB10 attr) => attribute.Id.Equals(attr.Id));
			val.CompareWith(attribute2);
		}
	}

	[CompilerGenerated]
	private void _E019()
	{
		_cameraImage.gameObject.SetActive(value: false);
		_loader.SetActive(value: true);
	}

	[CompilerGenerated]
	private void _E01A()
	{
		_loader.SetActive(value: false);
		_cameraImage.gameObject.SetActive(value: true);
	}
}
