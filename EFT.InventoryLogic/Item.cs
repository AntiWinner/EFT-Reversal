using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using JetBrains.Annotations;
using JsonType;

namespace EFT.InventoryLogic;

public class Item
{
	private sealed class _E000 : IEqualityComparer<Item>
	{
		public bool Equals(Item x, Item y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null)
			{
				return false;
			}
			if (y == null)
			{
				return false;
			}
			return x.Compare(y);
		}

		public int GetHashCode(Item obj)
		{
			return 0;
		}
	}

	[CompilerGenerated]
	private sealed class _E001<_E099, _E0B1> where _E099 : Enum where _E0B1 : _E562
	{
		public string postfix;

		public EItemAttributeDisplayType displayType;

		public EItemAttributeLabelVariations variation;

		public Func<EItemAttributeDisplayType> _003C_003E9__3;

		internal _EB10 _E000(KeyValuePair<_E099, _E0B1> spec)
		{
			_E002<_E099, _E0B1> CS_0024_003C_003E8__locals0 = new _E002<_E099, _E0B1>
			{
				CS_0024_003C_003E8__locals1 = this,
				spec = spec
			};
			return new _EB10(CS_0024_003C_003E8__locals0.spec.Key)
			{
				Name = CS_0024_003C_003E8__locals0.spec.Key.ToString(),
				Base = () => CS_0024_003C_003E8__locals0.spec.Value.Value,
				StringValue = () => CS_0024_003C_003E8__locals0.spec.Value.GetStringValue(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.postfix),
				DisplayType = () => displayType,
				FullStringValue = () => CS_0024_003C_003E8__locals0.spec.Value.GetFullStringValue(CS_0024_003C_003E8__locals0.spec.Key.ToString()),
				LabelVariations = variation
			};
		}

		internal EItemAttributeDisplayType _E001()
		{
			return displayType;
		}
	}

	[CompilerGenerated]
	private sealed class _E002<_E099, _E0B1> where _E099 : Enum where _E0B1 : _E562
	{
		public KeyValuePair<_E099, _E0B1> spec;

		public _E001<_E099, _E0B1> CS_0024_003C_003E8__locals1;

		internal float _E000()
		{
			return spec.Value.Value;
		}

		internal string _E001()
		{
			return spec.Value.GetStringValue(CS_0024_003C_003E8__locals1.postfix);
		}

		internal string _E002()
		{
			return spec.Value.GetFullStringValue(spec.Key.ToString());
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Func<Item, IContainer, bool> weHaveToGoDeeper;

		public Item item;

		public Func<IContainer, bool> _003C_003E9__0;

		internal bool _E000(IContainer container)
		{
			return weHaveToGoDeeper(item, container);
		}
	}

	[CompilerGenerated]
	private sealed class _E004<_E099> where _E099 : Enum
	{
		public EItemAttributeDisplayType displayType;

		public Func<EItemAttributeDisplayType> _003C_003E9__0;

		internal EItemAttributeDisplayType _E000()
		{
			return displayType;
		}
	}

	[CompilerGenerated]
	private sealed class _E005<_E099> where _E099 : Enum
	{
		public _E560 spec;

		internal string _E000()
		{
			return spec.GetStringValue();
		}
	}

	[CompilerGenerated]
	private sealed class _E006<_E099> where _E099 : Enum
	{
		public string replacement;

		public _E005<_E099> CS_0024_003C_003E8__locals1;

		internal string _E000()
		{
			return CS_0024_003C_003E8__locals1.spec.GetFullStringValue(replacement);
		}
	}

	public bool UnlimitedCount;

	public int BuyRestrictionMax;

	[_E63C]
	[DefaultValue(0)]
	public int BuyRestrictionCurrent;

	[_E63C]
	[DefaultValue(1)]
	public int StackObjectsCount;

	public int Version;

	public string Id;

	[CanBeNull]
	public ItemAddress OriginalAddress;

	protected internal readonly List<IItemComponent> Components = new List<IItemComponent>();

	private string _toStringCache;

	[CanBeNull]
	public ItemAddress CurrentAddress;

	[NonSerialized]
	public readonly _ECED<Item> ChildrenChanged = new _ECED<Item>();

	[DefaultValue(false)]
	[_E63C]
	public bool SpawnedInSession;

	public List<_EB10> Attributes = new List<_EB10>();

	private static readonly Dictionary<Enum, string> Replacements;

	public string Name => Template.NameLocalizationKey;

	public string ShortName => Template.ShortNameLocalizationKey;

	public string Description => Template.DescriptionLocalizationKey;

	public float Weight => Template.Weight;

	public bool ExaminedByDefault
	{
		get
		{
			if (!Template.ExaminedByDefault)
			{
				return Template.QuestItem;
			}
			return true;
		}
	}

	public float ExamineTime => Template.ExamineTime;

	public bool QuestItem => Template.QuestItem;

	public TaxonomyColor BackgroundColor => Template.BackgroundColor;

	public int StackMaxSize => Template.StackMaxSize;

	public string ItemSound => Template.ItemSound;

	public EItemDropSoundType DropSoundType => Template.DropSoundType;

	public ResourceKey Prefab => Template.Prefab;

	public ResourceKey UsePrefab => Template.UsePrefab;

	public bool NotShownInSlot => Template.NotShownInSlot;

	public int LootExperience => Template.LootExperience;

	public bool HideEntrails => Template.HideEntrails;

	public int ExamineExperience => Template.ExamineExperience;

	public int RepairCost => Template.RepairCost;

	public int RepairSpeed => Template.RepairSpeed;

	public virtual bool MergesWithChildren => Template.MergesWithChildren;

	public virtual bool CanSellOnRagfair => Template.CanSellOnRagfair;

	public bool CanRequireOnRagfair => Template.CanRequireOnRagfair;

	public string[] ConflictingItems => Template.ConflictingItems;

	public bool IsUnremovable => Template.IsUnremovable;

	public bool IsSpecialSlotOnly => Template.IsSpecialSlotOnly;

	public int? DiscardLimit { get; }

	public bool CanSellOnRagfairRaidRelated => MarkedAsSpawnedInSession;

	public bool BuyRestrictionCheck
	{
		get
		{
			if (BuyRestrictionMax <= BuyRestrictionCurrent)
			{
				return BuyRestrictionMax == 0;
			}
			return true;
		}
	}

	public bool IsEmptyStack => StackObjectsCount <= 0;

	private static IEqualityComparer<Item> _E000 { get; }

	public ItemAddress Parent => CurrentAddress ?? throw new Exception(_ED3E._E000(227414) + ShortName.Localized());

	public IItemOwner Owner => CurrentAddress?.GetOwnerOrNull();

	public virtual ItemTemplate Template { get; }

	public bool IsContainer
	{
		get
		{
			if (!(this is _EAA2) && !(this is _EA91) && !(this is _EB0B) && !(this is _EA66) && !(this is _EA95))
			{
				return this is _EAA0;
			}
			return true;
		}
	}

	public string TemplateId => Template._id;

	public virtual bool MarkedAsSpawnedInSession => SpawnedInSession;

	public virtual bool ImportantForCheckSpawnedInSession => true;

	public bool LimitedDiscard
	{
		get
		{
			if (!SpawnedInSession)
			{
				return DiscardLimit.HasValue;
			}
			return false;
		}
	}

	public bool CanBeRagfairForbidden
	{
		get
		{
			if (GetItemComponent<RepairableComponent>() != null)
			{
				return false;
			}
			if (GetItemComponent<MedKitComponent>() != null)
			{
				return false;
			}
			if (GetItemComponent<FoodDrinkComponent>() != null)
			{
				return false;
			}
			return true;
		}
	}

	public virtual List<EItemInfoButton> ItemInteractionButtons
	{
		get
		{
			List<EItemInfoButton> list = new List<EItemInfoButton>
			{
				EItemInfoButton.Inspect,
				EItemInfoButton.Examine,
				EItemInfoButton.TopUp,
				EItemInfoButton.Insure,
				EItemInfoButton.Uncover,
				EItemInfoButton.Overlook,
				EItemInfoButton.FilterSearch,
				EItemInfoButton.LinkedSearch,
				EItemInfoButton.NeededSearch,
				EItemInfoButton.AddToWishlist,
				EItemInfoButton.RemoveFromWishlist,
				EItemInfoButton.Discard
			};
			if (GetItemComponent<RepairableComponent>() != null)
			{
				list.Add(EItemInfoButton.Repair);
			}
			if (GetItemComponent<MapComponent>() != null)
			{
				list.Add(EItemInfoButton.ViewMap);
			}
			if (this.GetItemComponentsInChildren<TogglableComponent>().Any() && (GetItemComponent<FaceShieldComponent>() == null || GetItemComponent<HelmetComponent>() != null))
			{
				list.Add(EItemInfoButton.TurnOn);
				list.Add(EItemInfoButton.TurnOff);
			}
			if (this.GetItemComponentsInChildren<FoldableComponent>().Any())
			{
				list.Add(EItemInfoButton.Fold);
				list.Add(EItemInfoButton.Unfold);
			}
			this.GetItemComponentsInChildren<RecodableComponent>().Any();
			return list;
		}
	}

	public Item(string id, ItemTemplate template)
	{
		Template = template;
		Id = id;
		if (Template.DiscardLimit >= 0 && Singleton<_E5CB>.Instance.DiscardLimitsEnabled)
		{
			DiscardLimit = Template.DiscardLimit;
		}
		StackObjectsCount = template.StackObjectsCount;
		if (template.Unlootable)
		{
			Components.Add(new UnlootableComponent(this, template));
		}
		if (template.AnimationVariantsNumber > 0)
		{
			Components.Add(new AnimationVariantsComponent(this, template));
		}
		if (!string.IsNullOrEmpty(_E003()))
		{
			Attributes.Add(new _EB10(EItemAttributeId.Size)
			{
				Name = EItemAttributeId.Size.GetName(),
				Base = () => 0f,
				StringValue = () => _E000(Template.ExtraSizeDown, Template.ExtraSizeUp, Template.ExtraSizeLeft, Template.ExtraSizeRight),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		if (DiscardLimit.HasValue)
		{
			Attributes.Add(new _EB10(EItemAttributeId.LimitedDiscard)
			{
				Name = EItemAttributeId.LimitedDiscard.GetName(),
				Base = () => DiscardLimit.Value,
				StringValue = () => (DiscardLimit != 0) ? (_ED3E._E000(227668) + DiscardLimit) : string.Empty,
				FullStringValue = () => (EItemAttributeId.LimitedDiscard.GetName() + _ED3E._E000(227671)).Localized(),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
	}

	public bool CheckAction([CanBeNull] ItemAddress location)
	{
		if (CurrentAddress == null)
		{
			return true;
		}
		if (!this.CheckForLockable())
		{
			return false;
		}
		if (location != null && !location.Container.ParentItem.CheckForLockable())
		{
			return false;
		}
		if (Parent != location && location != null)
		{
			IItemOwner owner = location.GetOwner();
			if (owner != null && !owner.CheckItemAction(this, location))
			{
				return false;
			}
		}
		return Parent.GetOwner().CheckItemAction(this, location);
	}

	private string _E000(int down, int up, int left, int right)
	{
		string text = string.Empty;
		if (left > 0)
		{
			text = text + _ED3E._E000(227379).Localized() + _ED3E._E000(29692) + left + _ED3E._E000(18502);
		}
		if (right > 0)
		{
			text = text + _ED3E._E000(227370).Localized() + _ED3E._E000(29692) + right + _ED3E._E000(18502);
		}
		if (down > 0)
		{
			text = text + _ED3E._E000(227362).Localized() + _ED3E._E000(29692) + down + _ED3E._E000(18502);
		}
		if (up > 0)
		{
			text = text + _ED3E._E000(227417).Localized() + _ED3E._E000(29692) + up + _ED3E._E000(18502);
		}
		return text;
	}

	public _E313 CalculateCellSize()
	{
		return CalculateExtraSize(ignoreSelf: true).Apply(Template.Width, Template.Height);
	}

	public _E313 CalculateRotatedSize(ItemRotation rotation)
	{
		_E313 obj = CalculateCellSize();
		return new _E313((rotation == ItemRotation.Horizontal) ? obj.X : obj.Y, (rotation == ItemRotation.Horizontal) ? obj.Y : obj.X);
	}

	public ExtraSize CalculateExtraSize(bool ignoreSelf, FoldableComponent overrideFoldable = null, bool overrideValue = false, Slot overrideSlot = null, Item overrideSlotContent = null)
	{
		if (!(this is _EA40))
		{
			if (!ignoreSelf)
			{
				return Template.ExtraSize;
			}
			return default(ExtraSize);
		}
		_EA40 obj = (_EA40)this;
		ExtraSize extraSize = default(ExtraSize);
		Stack<Item> stack = new Stack<Item>();
		stack.Push(obj);
		while (stack.Count > 0)
		{
			Item item = stack.Pop();
			FoldableComponent itemComponent = item.GetItemComponent<FoldableComponent>();
			int num;
			if (itemComponent != overrideFoldable)
			{
				if (itemComponent == null)
				{
					num = 0;
					goto IL_0071;
				}
				num = (itemComponent.Folded ? 1 : 0);
			}
			else
			{
				num = (overrideValue ? 1 : 0);
			}
			if (num == 0)
			{
				goto IL_0071;
			}
			object obj2 = itemComponent.FoldedSlot;
			goto IL_007b;
			IL_0071:
			obj2 = null;
			goto IL_007b;
			IL_007b:
			Slot slot = (Slot)obj2;
			ExtraSize op = ((ignoreSelf && item == this) ? default(ExtraSize) : item.Template.ExtraSize);
			if (num != 0)
			{
				if (item == obj)
				{
					op.ForcedRight -= itemComponent.SizeReduceRight;
				}
				else
				{
					op.Right -= itemComponent.SizeReduceRight;
				}
			}
			extraSize = ExtraSize.Merge(extraSize, op);
			if (!(item is ContainerCollection containerCollection) || !item.MergesWithChildren)
			{
				continue;
			}
			foreach (IContainer container in containerCollection.Containers)
			{
				if (!container.MergesWithChildren() || container == slot)
				{
					continue;
				}
				if (container == overrideSlot)
				{
					if (overrideSlotContent != null)
					{
						stack.Push(overrideSlotContent);
					}
					continue;
				}
				foreach (Item item2 in container.Items)
				{
					if (item2 != null)
					{
						stack.Push(item2);
					}
				}
			}
		}
		return extraSize;
	}

	public _E313 GetSizeAfterAttachment(ItemAddress location, Item attachedItem)
	{
		return CalculateExtraSize(ignoreSelf: true, null, overrideValue: false, location.Container as Slot, attachedItem).Apply(Template.Width, Template.Height);
	}

	public _E313 GetSizeAfterDetachment(ItemAddress location, Item detachedItem)
	{
		return CalculateExtraSize(ignoreSelf: true, null, overrideValue: false, location.Container as Slot).Apply(Template.Width, Template.Height);
	}

	public _E313 GetSizeAfterFolding(ItemAddress location, FoldableComponent foldedItem, bool folded)
	{
		return CalculateExtraSize(ignoreSelf: true, foldedItem, folded).Apply(Template.Width, Template.Height);
	}

	public bool IsArmorMod()
	{
		ArmorComponent itemComponent = GetItemComponent<ArmorComponent>();
		if (itemComponent != null && itemComponent.ArmorZone.Any((EBodyPart x) => x == EBodyPart.Head))
		{
			return GetItemComponent<HelmetComponent>() == null;
		}
		return false;
	}

	public void RaiseRefreshEvent(bool refreshIcon = false, bool checkMagazine = true)
	{
		IItemOwner owner = Owner;
		if (owner == null)
		{
			return;
		}
		owner.RaiseEvent(new _EAFF(this, Parent.Container, refreshIcon, checkMagazine));
		foreach (Item allParentItem in this.GetAllParentItems(onlyMerged: true))
		{
			owner.RaiseEvent(new _EAFF(allParentItem, allParentItem.Parent.Container, refreshIcon, checkMagazine));
		}
	}

	protected T GetTemplate<T>() where T : ItemTemplate
	{
		return (T)Template;
	}

	public bool TryGetItemComponent<T>(out T component) where T : class, IItemComponent
	{
		component = GetItemComponent<T>();
		return component != null;
	}

	private bool _E001<_E0B0>(Item item, Func<_E0B0, _E0B0, bool> comparator) where _E0B0 : class, IItemComponent
	{
		if (TryGetItemComponent<_E0B0>(out var component) != item.TryGetItemComponent<_E0B0>(out var component2))
		{
			return false;
		}
		if (component != null)
		{
			return comparator(component, component2);
		}
		return true;
	}

	[CanBeNull]
	public T GetItemComponent<T>() where T : class, IItemComponent
	{
		foreach (IItemComponent component in Components)
		{
			if (component is T result)
			{
				return result;
			}
		}
		foreach (IItemComponent readonlyComponent in Template.GetReadonlyComponents())
		{
			if (readonlyComponent is T result2)
			{
				return result2;
			}
		}
		return null;
	}

	public override string ToString()
	{
		if (_toStringCache == null)
		{
			_toStringCache = _ED3E._E000(227483) + Template._name + _ED3E._E000(227473) + Id + _ED3E._E000(27308);
		}
		return _toStringCache;
	}

	public string ToFullString()
	{
		string text = string.Format(_ED3E._E000(227464), Template._name, Template._id, CurrentAddress, Id);
		if (StackObjectsCount > 1)
		{
			text += string.Format(_ED3E._E000(227503), StackObjectsCount);
		}
		return text;
	}

	public virtual int GetHashSum()
	{
		return (16 + StackObjectsCount) * 23 + Id.GetHashCode();
	}

	[CanBeNull]
	public virtual _EA6A GetCurrentMagazine()
	{
		return null;
	}

	public virtual bool Compare(Item item)
	{
		if (this == item)
		{
			return true;
		}
		if (TemplateId != item.TemplateId)
		{
			return false;
		}
		if (!_E001(item, (DogtagComponent comp1, DogtagComponent comp2) => false))
		{
			return false;
		}
		if (!_E001(item, (RepairableComponent comp1, RepairableComponent comp2) => comp1.MaxDurability.ApproxEquals(comp2.MaxDurability) && comp1.Durability.ApproxEquals(comp2.Durability)))
		{
			return false;
		}
		if (!_E001(item, (MedKitComponent comp1, MedKitComponent comp2) => comp1.MaxHpResource == comp2.MaxHpResource && comp1.HpResource.ApproxEquals(comp2.HpResource)))
		{
			return false;
		}
		if (!_E001(item, (FoodDrinkComponent comp1, FoodDrinkComponent comp2) => comp1.MaxResource.ApproxEquals(comp2.MaxResource) && comp1.HpPercent.ApproxEquals(comp2.HpPercent)))
		{
			return false;
		}
		if (!_E001(item, (KeyComponent comp1, KeyComponent comp2) => comp1.NumberOfUsages == comp2.NumberOfUsages))
		{
			return false;
		}
		if (!_E001(item, (ResourceComponent comp1, ResourceComponent comp2) => comp1.Value.ApproxEquals(comp2.Value)))
		{
			return false;
		}
		if (!_E001(item, (SideEffectComponent comp1, SideEffectComponent comp2) => comp1.Value.ApproxEquals(comp2.Value)))
		{
			return false;
		}
		if (!_E001(item, (RepairKitComponent comp1, RepairKitComponent comp2) => comp1.Resource.ApproxEquals(comp2.Resource)))
		{
			return false;
		}
		if (!_E001(item, (BuffComponent comp1, BuffComponent comp2) => comp1.Value.ApproxEquals(comp2.Value) && comp1.BuffType == comp2.BuffType && comp1.Rarity == comp2.Rarity && comp1.ThresholdDurability.ApproxEquals(comp2.ThresholdDurability)))
		{
			return false;
		}
		if (!(this is ContainerCollection containerCollection))
		{
			return true;
		}
		if (!(item is ContainerCollection containerCollection2))
		{
			return false;
		}
		Item[] array = containerCollection.Containers.SelectMany((IContainer x) => x.Items).ToArray();
		Item[] array2 = containerCollection2.Containers.SelectMany((IContainer x) => x.Items).ToArray();
		if (array.Length != array2.Length)
		{
			return false;
		}
		int num = array.Intersect(array2, Item._E000).Count();
		return array.Length == num;
	}

	public float GetSingleItemTotalWeight()
	{
		if (!(this is ContainerCollection))
		{
			return Weight;
		}
		return ((ContainerCollection)this).GetAllItemsFromCollection().Sum((Item x) => x.Weight * (float)((x == this) ? 1 : x.StackObjectsCount));
	}

	public void UpdateAttributes()
	{
		foreach (_EB10 attribute in Attributes)
		{
			attribute.Update();
		}
	}

	public void CreateAttributesFromDictionary<TEnum, TSpecification>(Dictionary<TEnum, TSpecification> specifications, EItemAttributeDisplayType displayType, EItemAttributeLabelVariations variation, string postfix = "") where TEnum : Enum where TSpecification : _E562
	{
		Attributes.AddRange(specifications.Select((KeyValuePair<TEnum, TSpecification> spec) => new _EB10(spec.Key)
		{
			Name = spec.Key.ToString(),
			Base = () => spec.Value.Value,
			StringValue = () => spec.Value.GetStringValue(postfix),
			DisplayType = () => displayType,
			FullStringValue = () => spec.Value.GetFullStringValue(spec.Key.ToString()),
			LabelVariations = variation
		}));
	}

	public void SafelyAddAttributeToList(_EB10 itemAttribute)
	{
		if (itemAttribute.Base() != 0f)
		{
			Attributes.Add(itemAttribute);
		}
	}

	public IEnumerable<Item> GetAllVisibleItems()
	{
		List<Item> list = new List<Item>();
		_E002(this, list, (Item item, IContainer container) => !item.HideEntrails);
		return list;
	}

	private static void _E002(Item item, List<Item> list, Func<Item, IContainer, bool> weHaveToGoDeeper)
	{
		list.Add(item);
		if (!(item is ContainerCollection))
		{
			return;
		}
		foreach (Item item2 in ((ContainerCollection)item).Containers.Where((IContainer container) => weHaveToGoDeeper(item, container)).SelectMany((IContainer container) => container.Items))
		{
			_E002(item2, list, weHaveToGoDeeper);
		}
	}

	public void CreateAttributesFromDictionary<TEnum>(Dictionary<TEnum, _E560> specifications, EItemAttributeDisplayType displayType) where TEnum : Enum
	{
		foreach (KeyValuePair<TEnum, _E560> specification in specifications)
		{
			_E39D.Deconstruct(specification, out var key, out var value);
			TEnum val = key;
			_E560 spec = value;
			if (Replacements.TryGetValue(val, out var replacement))
			{
				Attributes.Add(new _EB10(val)
				{
					Name = replacement,
					DisplayType = () => displayType,
					StringValue = () => spec.GetStringValue(),
					FullStringValue = () => spec.GetFullStringValue(replacement)
				});
			}
		}
	}

	public IEnumerable<IContainer> GetPathOfContainers()
	{
		Stack<IContainer> stack = new Stack<IContainer>();
		stack.Push(Parent.Container);
		for (Item parentItem = Parent.Container.ParentItem; parentItem != null; parentItem = parentItem.Parent.Container.ParentItem)
		{
			stack.Push(parentItem.Parent.Container);
		}
		return stack;
	}

	public bool IsSameItem(Item other)
	{
		if (other.TemplateId == TemplateId && other.Id != Id)
		{
			return other.SpawnedInSession == SpawnedInSession;
		}
		return false;
	}

	static Item()
	{
		_003CItemNoHashComparer_003Ek__BackingField = new _E000();
		Replacements = new Dictionary<Enum, string>
		{
			{
				EDamageEffectType.LightBleeding,
				_ED3E._E000(227548)
			},
			{
				EDamageEffectType.HeavyBleeding,
				_ED3E._E000(227529)
			},
			{
				EDamageEffectType.Fracture,
				_ED3E._E000(227574)
			},
			{
				EDamageEffectType.Pain,
				_ED3E._E000(227559)
			},
			{
				EDamageEffectType.Contusion,
				_ED3E._E000(227604)
			},
			{
				EDamageEffectType.Intoxication,
				_ED3E._E000(227590)
			},
			{
				EDamageEffectType.RadExposure,
				_ED3E._E000(227633)
			}
		};
	}

	[CompilerGenerated]
	private string _E003()
	{
		return _E000(Template.ExtraSizeDown, Template.ExtraSizeUp, Template.ExtraSizeLeft, Template.ExtraSizeRight);
	}

	[CompilerGenerated]
	private float _E004()
	{
		return DiscardLimit.Value;
	}

	[CompilerGenerated]
	private string _E005()
	{
		if (DiscardLimit != 0)
		{
			return _ED3E._E000(227668) + DiscardLimit;
		}
		return string.Empty;
	}

	[CompilerGenerated]
	private float _E006(Item x)
	{
		return x.Weight * (float)((x == this) ? 1 : x.StackObjectsCount);
	}
}
