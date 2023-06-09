using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using Comfort.Common;
using EFT.Counters;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public class Profile : IProfileDataContainer
{
	public class _E000
	{
		public class ValueInfo
		{
			public float Current;

			public float Minimum;

			public float Maximum;

			public float OverDamageReceivedMultiplier;
		}

		public class _E000
		{
			public float Time = -1f;

			[JsonProperty(TypeNameHandling = TypeNameHandling.All)]
			public object ExtraData;
		}

		public class _E001
		{
			public ValueInfo Health;

			public Dictionary<string, _E000> Effects = new Dictionary<string, _E000>();
		}

		public Dictionary<EBodyPart, _E001> BodyParts;

		public ValueInfo Energy;

		public ValueInfo Hydration;

		public ValueInfo Temperature = Singleton<_E5CB>.Instance.Health.ProfileHealthSettings.HealthFactorsSettings[EHealthFactorType.Temperature].ValueInfo;

		public ValueInfo Poison = new ValueInfo
		{
			Current = 0f,
			Minimum = 0f,
			Maximum = 100f
		};

		public int? UpdateTime;
	}

	public class _E001 : _E5CB._E021
	{
		private const int m__E000 = 4;

		[CompilerGenerated]
		private Action m__E001;

		[CompilerGenerated]
		private Action m__E002;

		[CompilerGenerated]
		private Action m__E003;

		[CompilerGenerated]
		private Action _E004;

		[CompilerGenerated]
		private Action _E005;

		[CompilerGenerated]
		private int _E006;

		[CompilerGenerated]
		private long _E007;

		[CompilerGenerated]
		private double _E008;

		[CompilerGenerated]
		private int _E009;

		[JsonProperty("unlocked")]
		public bool Unlocked;

		[CompilerGenerated]
		private bool _E00A;

		private _E72F _E00B;

		private bool _E00C;

		[CompilerGenerated]
		private string _E00D;

		[CompilerGenerated]
		private _E5CB.TraderSettings _E00E;

		[CompilerGenerated]
		private _E5CB.TraderLoyaltyLevel _E00F;

		[JsonProperty("loyaltyLevel")]
		public int LoyaltyLevel
		{
			[CompilerGenerated]
			get
			{
				return _E006;
			}
			[CompilerGenerated]
			private set
			{
				_E006 = value;
			}
		}

		[JsonProperty("salesSum")]
		public long SalesSum
		{
			[CompilerGenerated]
			get
			{
				return _E007;
			}
			[CompilerGenerated]
			protected set
			{
				_E007 = value;
			}
		}

		[JsonProperty("standing")]
		public double Standing
		{
			[CompilerGenerated]
			get
			{
				return _E008;
			}
			[CompilerGenerated]
			protected set
			{
				_E008 = value;
			}
		}

		[JsonProperty("nextResupply")]
		public int NextResupply
		{
			[CompilerGenerated]
			get
			{
				return _E009;
			}
			[CompilerGenerated]
			protected set
			{
				_E009 = value;
			}
		}

		[JsonProperty("disabled")]
		public bool Disabled
		{
			[CompilerGenerated]
			get
			{
				return _E00A;
			}
			[CompilerGenerated]
			protected set
			{
				_E00A = value;
			}
		}

		[JsonIgnore]
		public bool Available
		{
			get
			{
				if (!Disabled && Unlocked)
				{
					return !_E00C;
				}
				return false;
			}
		}

		[JsonIgnore]
		public string Id
		{
			[CompilerGenerated]
			get
			{
				return _E00D;
			}
			[CompilerGenerated]
			private set
			{
				_E00D = value;
			}
		}

		[JsonIgnore]
		public int ProfileLevel => _E00B.Level;

		[JsonIgnore]
		public _E5CB.TraderSettings Settings
		{
			[CompilerGenerated]
			get
			{
				return _E00E;
			}
			[CompilerGenerated]
			private set
			{
				_E00E = value;
			}
		}

		[JsonIgnore]
		public _E5CB.TraderLoyaltyLevel CurrentLoyalty
		{
			[CompilerGenerated]
			get
			{
				return _E00F;
			}
			[CompilerGenerated]
			private set
			{
				_E00F = value;
			}
		}

		[JsonIgnore]
		public int MaxLoyaltyLevel => Settings?.LoyaltyLevels?.Length ?? 4;

		[JsonIgnore]
		public virtual double SellToTraderPriceModifier => 1.0 - CurrentLoyalty.SellToTraderPriceCoef / 100.0;

		public event Action OnStandingChanged
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnSalesSumChanged
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E002;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E002;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnLoyaltyChanged
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E003;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E003, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E003;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E003, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAvailabilityChanged
		{
			[CompilerGenerated]
			add
			{
				Action action = _E004;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E004, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = _E004;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E004, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnResupplyTimeChanged
		{
			[CompilerGenerated]
			add
			{
				Action action = _E005;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E005, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = _E005;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E005, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public virtual double ApplyPriceModifier(double basePrice)
		{
			return Math.Floor(basePrice * SellToTraderPriceModifier);
		}

		public void Init(string traderId, _E72F profileInfo)
		{
			Id = traderId;
			_E00B = profileInfo;
			Settings = Singleton<_E5CB>.Instance.TradersSettings[Id];
			Unlocked |= Settings.UnlockedByDefault;
			NextResupply = Settings.NextResupply;
			profileInfo.OnLevelChanged += delegate
			{
				UpdateLevel();
			};
			profileInfo.OnBanChanged += _E001;
			_E001();
			UpdateLevel();
		}

		public bool CanBuyItem(Item item)
		{
			return (from subItem in item.GetAllItems()
				where !(subItem is _EAA0)
				select subItem).All((Item subItem) => _E000(subItem.Template));
		}

		private bool _E000(ItemTemplate itemTemplate)
		{
			if (!Settings.DoesNotBuyItems.Contains(itemTemplate))
			{
				return Settings.BuysItems.Contains(itemTemplate);
			}
			return false;
		}

		public bool CanBuyRootItem(Item item, out IReadOnlyList<Item> unsellableParts)
		{
			List<Item> list = (List<Item>)(unsellableParts = new List<Item>());
			bool flag = false;
			foreach (Item allItem in item.GetAllItems())
			{
				bool flag2 = _E000(allItem.Template);
				if (!flag && !flag2)
				{
					return false;
				}
				if (!flag2)
				{
					list.Add(allItem);
				}
				flag = true;
			}
			return true;
		}

		public bool TryGetNextLoyalty(out _E5CB.TraderLoyaltyLevel nextLoyalty)
		{
			_E5CB.TraderLoyaltyLevel? traderLoyaltyLevel = Settings?.GetLoyaltyLevelSettings(LoyaltyLevel + 1);
			nextLoyalty = traderLoyaltyLevel ?? default(_E5CB.TraderLoyaltyLevel);
			return traderLoyaltyLevel.HasValue;
		}

		protected virtual void UpdateLevel()
		{
			if (Settings == null)
			{
				return;
			}
			int loyaltyLevel = Settings.GetLoyaltyLevel(this);
			if (loyaltyLevel != LoyaltyLevel)
			{
				LoyaltyLevel = loyaltyLevel;
				_E5CB.TraderLoyaltyLevel? loyaltyLevelSettings = Settings.GetLoyaltyLevelSettings(LoyaltyLevel);
				if (!loyaltyLevelSettings.HasValue)
				{
					throw new IndexOutOfRangeException(string.Format(_ED3E._E000(138205), LoyaltyLevel));
				}
				CurrentLoyalty = loyaltyLevelSettings.Value;
				this.m__E003?.Invoke();
			}
		}

		private void _E001(EBanType banType = EBanType.Trading)
		{
			if (banType == EBanType.Trading)
			{
				bool flag = _E00B.GetBan(EBanType.Trading) != null;
				if (_E00C != flag)
				{
					_E00C = flag;
					_E004?.Invoke();
				}
			}
		}

		public virtual void SetStanding(double value)
		{
			if (!Standing.ApproxEquals(value))
			{
				Standing = value;
				UpdateLevel();
				this.m__E001?.Invoke();
			}
		}

		public void SetSalesSum(long value)
		{
			if (SalesSum != value)
			{
				SalesSum = value;
				UpdateLevel();
				this.m__E002?.Invoke();
			}
		}

		public void SetUnlocked(bool unlocked)
		{
			unlocked |= Settings.UnlockedByDefault;
			if (Unlocked != unlocked)
			{
				Unlocked = unlocked;
				_E004?.Invoke();
			}
		}

		public void SetDisabled(bool disabled)
		{
			if (Disabled != disabled)
			{
				Disabled = disabled;
				_E004?.Invoke();
			}
		}

		public void SetResupplyTime(int resupplyTime)
		{
			if (NextResupply != resupplyTime)
			{
				NextResupply = resupplyTime;
				_E005?.Invoke();
			}
		}

		[CompilerGenerated]
		private void _E002(int _, int __)
		{
			UpdateLevel();
		}

		[CompilerGenerated]
		private bool _E003(Item subItem)
		{
			return _E000(subItem.Template);
		}
	}

	public sealed class FenceTraderInfo : _E001
	{
		private readonly _E5CB.FenceGlobalSettings _fenceSettings;

		private readonly _E945 _sessionCounters;

		private readonly float _charismaFenceDiscount;

		[JsonIgnore]
		public _E5CB.FenceLoyaltyLevel FenceLoyalty { get; private set; }

		public int AvailableExitsCount => FenceLoyalty.AvailableExits;

		public double FenceLoyaltyPriceModifier => FenceLoyalty.PriceModifier;

		public override double ApplyPriceModifier(double basePrice)
		{
			basePrice = Math.Floor(basePrice * Math.Round(SellToTraderPriceModifier, 3));
			return Math.Floor(basePrice / Math.Round(FenceLoyaltyPriceModifier, 3));
		}

		public FenceTraderInfo(_E001 traderInfo, Profile profile)
		{
			base.SalesSum = traderInfo.SalesSum;
			base.Standing = traderInfo.Standing;
			Unlocked = traderInfo.Unlocked;
			base.Disabled = traderInfo.Disabled;
			base.NextResupply = traderInfo.NextResupply;
			_fenceSettings = Singleton<_E5CB>.Instance.FenceSettings;
			_sessionCounters = profile.Stats.SessionCounters;
			_charismaFenceDiscount = profile.Skills.CharismaEliteFenceRepPenaltyReduction;
			Init(traderInfo.Id, profile.Info);
		}

		protected override void UpdateLevel()
		{
			FenceLoyalty = _fenceSettings.GetSettings(base.Standing);
			base.UpdateLevel();
		}

		public int NewExfiltrationPrice(int price)
		{
			return (int)(FenceLoyalty.ExfiltrationPriceModifier * (float)price);
		}

		public void AddStanding(double dif, EFenceStandingSource standingSource)
		{
			if ((standingSource == EFenceStandingSource.ScavKill || standingSource == EFenceStandingSource.BossKill) && dif.Negative())
			{
				dif *= (double)(1f - _charismaFenceDiscount);
				dif = Math.Floor(dif * 100.0) / 100.0;
			}
			base.SetStanding(base.Standing + dif);
			_sessionCounters.AddDouble(dif, CounterTag.FenceStanding, standingSource);
		}

		public override void SetStanding(double value)
		{
			base.SetStanding(value);
		}
	}

	[Serializable]
	public class UnlockedInfo
	{
		[JsonProperty("unlockedProductionRecipe")]
		public List<string> unlockedSchemeList = new List<string>();
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string fenceId;

		public Profile _003C_003E4__this;

		internal _E001 _E000(KeyValuePair<string, _E001> traderInfo)
		{
			if (!(traderInfo.Value.Id == fenceId))
			{
				return traderInfo.Value;
			}
			return new FenceTraderInfo(traderInfo.Value, _003C_003E4__this);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _E001 traderInfo;

		public _E002 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.OnTraderLoyaltyChanged?.Invoke(traderInfo);
		}

		internal void _E001()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.OnTraderStandingChanged?.Invoke(traderInfo);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public bool allCustomization;

		public _E60E customizationSolver;

		internal bool _E000(KeyValuePair<EBodyModelPart, string> x)
		{
			if (!allCustomization)
			{
				return x.Key != EBodyModelPart.Hands;
			}
			return true;
		}

		internal ResourceKey _E001(KeyValuePair<EBodyModelPart, string> x)
		{
			return customizationSolver.GetBundle(x.Value);
		}
	}

	[JsonProperty("_id")]
	public string Id = "";

	[JsonProperty("aid")]
	public string AccountId = "";

	[JsonProperty("savage")]
	public string PetId;

	public _E72F Info;

	public _E72D Customization;

	public Dictionary<string, bool> Encyclopedia;

	public _E000 Health;

	public _EAE7 Inventory;

	public _E53C[] QuestItems;

	public _E541[] InsuredItems;

	public _E74F Skills;

	public _E9CE Notes = new _E9CE();

	[JsonProperty("Quests")]
	public List<_E932> QuestsData = new List<_E932>();

	public _E91E ConditionCounters = new _E91E();

	public Dictionary<string, _E917> BackendCounters = new Dictionary<string, _E917>();

	public Dictionary<string, _E001> TradersInfo = new Dictionary<string, _E001>();

	[JsonProperty("UnlockedInfo")]
	public UnlockedInfo UnlockedRecipeInfo = new UnlockedInfo();

	public _E5EA[] Bonuses;

	public HideoutInfo Hideout;

	public _E7D5 RagfairInfo;

	public readonly IdGenerator IdGenerator = new IdGenerator();

	public string[] WishList = new string[0];

	private readonly ResourceKey[] _emptyResourceCollection = new ResourceKey[0];

	public _E34D Stats = new _E34D();

	public Dictionary<string, int> CheckedMagazines = new Dictionary<string, int>();

	public List<string> CheckedChambers = new List<string>();

	public BonusController BonusController { get; private set; }

	public string Nickname => Info.Nickname;

	public EPlayerSide Side => Info.Side;

	string IProfileDataContainer.ProfileId => Id;

	[JsonIgnore]
	public FenceTraderInfo FenceInfo { get; private set; }

	public int MagDrillsMastering => Skills.MagDrills.Level / 10;

	public _E34D.ESurvivorClass SurvivorClass
	{
		get
		{
			if (Info.Level >= 20)
			{
				return Stats.SurvivorClass;
			}
			return _E34D.ESurvivorClass.Unknown;
		}
	}

	public event Action<string, string> OnItemZoneDropped;

	public event Action<_E001> OnTraderStandingChanged;

	public event Action<_E001> OnTraderLoyaltyChanged;

	[OnDeserialized]
	internal void _E000(StreamingContext context)
	{
		BonusController = new BonusController();
		if (Skills != null)
		{
			BonusController.InitSkillManager(Skills);
			Skills.Init(BonusController);
		}
		Dictionary<string, _E001> tradersInfo = TradersInfo;
		TradersInfo = new Dictionary<string, _E001>();
		foreach (string key2 in Singleton<_E5CB>.Instance.TradersSettings.Keys)
		{
			if (!tradersInfo.TryGetValue(key2, out var value))
			{
				value = new _E001();
			}
			TradersInfo[key2] = value;
		}
		if (Stats.SessionCounters == null)
		{
			Stats.SessionCounters = new _E945();
		}
		string key;
		_E001 value2;
		foreach (KeyValuePair<string, _E001> item in TradersInfo)
		{
			_E39D.Deconstruct(item, out key, out value2);
			string traderId = key;
			value2.Init(traderId, Info);
		}
		string fenceId = Singleton<_E5CB>.Instance.FenceSettings.FenceId;
		TradersInfo = TradersInfo.ToDictionary((KeyValuePair<string, _E001> traderInfo) => traderInfo.Value.Id, (KeyValuePair<string, _E001> traderInfo) => (!(traderInfo.Value.Id == fenceId)) ? traderInfo.Value : new FenceTraderInfo(traderInfo.Value, this));
		foreach (KeyValuePair<string, _E001> item2 in TradersInfo)
		{
			_E39D.Deconstruct(item2, out key, out value2);
			_E001 traderInfo2 = value2;
			traderInfo2.OnLoyaltyChanged += delegate
			{
				this.OnTraderLoyaltyChanged?.Invoke(traderInfo2);
			};
			traderInfo2.OnStandingChanged += delegate
			{
				this.OnTraderStandingChanged?.Invoke(traderInfo2);
			};
		}
		FenceInfo = TradersInfo.Values.OfType<FenceTraderInfo>().FirstOrDefault();
		if (Bonuses != null)
		{
			_E5EA[] bonuses = Bonuses;
			foreach (_E5EA bonus in bonuses)
			{
				BonusController.AddBonus(bonus);
			}
		}
	}

	public PlayerVisualRepresentation GetVisualEquipmentState(bool clone = true)
	{
		return new PlayerVisualRepresentation
		{
			Info = new _E54F
			{
				Level = Info.Level,
				MemberCategory = ((Info.Side != EPlayerSide.Savage) ? Info.MemberCategory : EMemberCategory.Default),
				Nickname = Info.Nickname,
				Side = Info.Side
			},
			Customization = Customization,
			Equipment = (clone ? Inventory.Equipment.CloneVisibleItem() : Inventory.Equipment)
		};
	}

	public bool TryGetTraderInfo(string traderId, out _E001 traderInfo)
	{
		return TradersInfo.TryGetValue(traderId, out traderInfo);
	}

	public double GetTraderStanding(string traderId)
	{
		if (TryGetTraderInfo(traderId, out var traderInfo))
		{
			return traderInfo.Standing;
		}
		return -1.0;
	}

	public int GetTraderLoyalty(string traderId)
	{
		if (TryGetTraderInfo(traderId, out var traderInfo))
		{
			return traderInfo.LoyaltyLevel;
		}
		return -1;
	}

	public int GetExfiltrationPrice(int price)
	{
		return (int)(FenceInfo.FenceLoyalty.ExfiltrationPriceModifier * (float)price * (1f - (float)Skills.CharismaExfiltrationDiscount));
	}

	public _E917 GetBackendCounter(string id)
	{
		if (!BackendCounters.ContainsKey(id))
		{
			BackendCounters[id] = new _E917();
		}
		return BackendCounters[id];
	}

	public int CheckedMagazineSkillLevel(string id)
	{
		if (!CheckedMagazines.ContainsKey(id))
		{
			return 0;
		}
		return CheckedMagazines[id];
	}

	public void AddToCarriedQuestItems(string id)
	{
		if (!Stats.CarriedQuestItems.Contains(id))
		{
			Stats.CarriedQuestItems.Add(id);
		}
	}

	public void UncoverAll(string profileId = null)
	{
		profileId = profileId ?? Id;
		IEnumerable<_EA91> enumerable = Inventory.NonQuestItems.OfType<_EA91>();
		if (string.IsNullOrEmpty(Id))
		{
			Id = Guid.NewGuid().ToString();
		}
		foreach (_EA91 item in enumerable)
		{
			item.UncoverAll(profileId);
		}
	}

	public bool Examined(Item item)
	{
		if (Encyclopedia == null)
		{
			return true;
		}
		if (!Encyclopedia.ContainsKey(item.TemplateId))
		{
			return item.ExaminedByDefault;
		}
		return true;
	}

	public bool Examined(string templateId)
	{
		if (Encyclopedia != null)
		{
			return Encyclopedia.ContainsKey(templateId);
		}
		return true;
	}

	public void LearnAll()
	{
		foreach (Item item in Inventory.NonQuestItems.Where((Item item) => !Encyclopedia.ContainsKey(item.TemplateId)))
		{
			Encyclopedia.Add(item.TemplateId, value: false);
		}
	}

	public void ItemDroppedAtPlace(string itemId, string zoneId)
	{
		this.OnItemZoneDropped?.Invoke(itemId, zoneId);
	}

	public IEnumerable<ResourceKey> GetAllPrefabPaths(bool allCustomization = true)
	{
		_E60E customizationSolver = Singleton<_E60E>.Instance;
		IEnumerable<ResourceKey> enumerable;
		if (customizationSolver == null)
		{
			IEnumerable<ResourceKey> emptyResourceCollection = _emptyResourceCollection;
			enumerable = emptyResourceCollection;
		}
		else
		{
			enumerable = from x in Customization
				where allCustomization || x.Key != EBodyModelPart.Hands
				select customizationSolver.GetBundle(x.Value);
		}
		IEnumerable<ResourceKey> first = enumerable;
		ResourceKey resourceKey = customizationSolver?.GetWatchBundle(Customization[EBodyModelPart.Hands]).WatchPrefab;
		ResourceKey[] second = _emptyResourceCollection;
		if (resourceKey != null && !string.IsNullOrEmpty(resourceKey.path))
		{
			second = new ResourceKey[1] { resourceKey };
		}
		IEnumerable<ResourceKey> second2 = Inventory.Equipment.GetAllItemsFromCollection().SelectMany((Item x) => x.Template.AllResources);
		return first.Concat(second2).Concat(second).Append(new ResourceKey
		{
			path = _E5D2.TakePhrasePath(Info.Voice),
			rcid = Info.Voice
		});
	}

	public void SetSpawnedInSession(bool value)
	{
		foreach (Item allPlayerItem in Inventory.AllPlayerItems)
		{
			allPlayerItem.SpawnedInSession = value;
		}
	}

	public Profile Clone()
	{
		return this.ToJson().ParseJsonTo<Profile>(Array.Empty<JsonConverter>());
	}

	public void SetDefaultFenceInfo()
	{
		FenceInfo = new FenceTraderInfo(new _E001(), this);
	}

	[CompilerGenerated]
	private bool _E001(Item item)
	{
		return !Encyclopedia.ContainsKey(item.TemplateId);
	}
}
