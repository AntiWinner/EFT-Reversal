using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using JetBrains.Annotations;
using JsonType;
using Newtonsoft.Json;

namespace EFT.InventoryLogic;

public class ItemTemplate : IUnlootableComponentTemplate, IAnimationVariantsComponentTemplate, IItemTemplate
{
	public string Name;

	public string ShortName;

	public string Description;

	public float Weight;

	public bool ExaminedByDefault;

	public float ExamineTime;

	public bool QuestItem;

	public TaxonomyColor BackgroundColor;

	public int Width;

	public int Height;

	public int ExtraSizeLeft;

	public int ExtraSizeRight;

	public int ExtraSizeUp;

	public int ExtraSizeDown;

	public bool ExtraSizeForceAdd;

	public int StackMaxSize;

	public int StackObjectsCount;

	public int CreditsPrice;

	public string ItemSound;

	public ResourceKey Prefab;

	public ResourceKey UsePrefab;

	public ELootRarity Rarity;

	public EItemDropSoundType DropSoundType;

	public float SpawnChance;

	public bool NotShownInSlot;

	public int LootExperience;

	public bool HideEntrails;

	public int ExamineExperience;

	public int RepairCost;

	public int RepairSpeed;

	public bool MergesWithChildren;

	public bool CanSellOnRagfair;

	public bool CanRequireOnRagfair;

	public string[] ConflictingItems;

	public int AnimationVariantsNumber;

	public float RagFairCommissionModifier = 1f;

	public bool IsAlwaysAvailableForInsurance;

	public bool InsuranceDisabled;

	public int DiscardLimit = -1;

	public bool Unlootable;

	public bool IsUnremovable;

	public bool IsSpecialSlotOnly;

	public string UnlootableFromSlot;

	public EPlayerSideMask UnlootableFromSide;

	public string _id;

	public string _name;

	public string _parent;

	public NodeType _type;

	public _E53A[] _items;

	protected List<IItemComponent> ReadonlyComponents;

	private List<ItemTemplate> _children;

	[JsonIgnore]
	private ItemTemplate[] _compatibleItems;

	public string ShortNameLocalizationKey => _id + _ED3E._E000(182596);

	public string NameLocalizationKey => _id + _ED3E._E000(70087);

	public string DescriptionLocalizationKey => _id + _ED3E._E000(114100);

	public virtual IEnumerable<ResourceKey> AllResources
	{
		get
		{
			if (!string.IsNullOrEmpty(Prefab.path))
			{
				yield return Prefab;
				if (!string.IsNullOrEmpty(UsePrefab.path))
				{
					yield return UsePrefab;
				}
			}
		}
	}

	public ExtraSize ExtraSize
	{
		get
		{
			ExtraSize result = default(ExtraSize);
			result.Left = ((!ExtraSizeForceAdd) ? ExtraSizeLeft : 0);
			result.Right = ((!ExtraSizeForceAdd) ? ExtraSizeRight : 0);
			result.Up = ((!ExtraSizeForceAdd) ? ExtraSizeUp : 0);
			result.Down = ((!ExtraSizeForceAdd) ? ExtraSizeDown : 0);
			result.ForcedLeft = (ExtraSizeForceAdd ? ExtraSizeLeft : 0);
			result.ForcedRight = (ExtraSizeForceAdd ? ExtraSizeRight : 0);
			result.ForcedUp = (ExtraSizeForceAdd ? ExtraSizeUp : 0);
			result.ForcedDown = (ExtraSizeForceAdd ? ExtraSizeDown : 0);
			return result;
		}
	}

	string IUnlootableComponentTemplate.SlotName => UnlootableFromSlot;

	EPlayerSideMask IUnlootableComponentTemplate.Side => UnlootableFromSide;

	int IAnimationVariantsComponentTemplate.AnimationVariantsNumber => AnimationVariantsNumber;

	[JsonIgnore]
	public ItemTemplate[] CompatibleItems
	{
		get
		{
			if (_compatibleItems != null)
			{
				return _compatibleItems;
			}
			Dictionary<ItemTemplate, ItemTemplate[]> allChildrenDict = new Dictionary<ItemTemplate, ItemTemplate[]>();
			IEnumerable<ItemTemplate> allCompatibleTemplates = _E54D.GetAllCompatibleTemplates(new ItemTemplate[1] { Singleton<_E63B>.Instance.ItemTemplates[_id] }, allChildrenDict);
			_compatibleItems = allCompatibleTemplates.ToArray();
			return _compatibleItems;
		}
	}

	[JsonIgnore]
	public IReadOnlyList<ItemTemplate> Children => _children;

	[JsonIgnore]
	public ItemTemplate Parent { get; private set; }

	public virtual void OnInit()
	{
	}

	public void AddChild(ItemTemplate template)
	{
		if (_children == null)
		{
			_children = new List<ItemTemplate>(3);
		}
		_children.Add(template);
		template.Parent = this;
	}

	public bool IsChildOf(string parentTemplateId)
	{
		for (ItemTemplate parent = Parent; parent != null; parent = parent.Parent)
		{
			if (parent._id == parentTemplateId)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual List<IItemComponent> CreateReadonlyComponentsCollection()
	{
		return new List<IItemComponent>();
	}

	[NotNull]
	public List<IItemComponent> GetReadonlyComponents()
	{
		return ReadonlyComponents ?? (ReadonlyComponents = CreateReadonlyComponentsCollection());
	}
}
