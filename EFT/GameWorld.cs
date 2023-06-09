using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Systems.Effects;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using Diz.Utils;
using EFT.AssetsManager;
using EFT.Ballistics;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.MovingPlatforms;
using EFT.Quests;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

[_E2E2(-2000)]
public class GameWorld : MonoBehaviour, _E5CE, _EBC9, IEnumerable<_E5B4>, IEnumerable, IDisposable
{
	public struct _E000
	{
		public Transform Transform;

		public bool DoNotPerformPickUpValidation;

		public IItemOwner ItemOwner;
	}

	public class _E001 : _ECD1
	{
		public readonly string ItemOwnerId;

		public _E001(string itemOwnerId)
		{
			ItemOwnerId = itemOwnerId;
		}

		public override string ToString()
		{
			return _ED3E._E000(111120) + ItemOwnerId;
		}
	}

	public class _E002 : _ECD1
	{
		public readonly string ItemAddressId;

		public _E002(string itemAddressId)
		{
			ItemAddressId = itemAddressId;
		}

		public override string ToString()
		{
			return _ED3E._E000(111148) + ItemAddressId;
		}
	}

	public class _E003 : _ECD1
	{
		public readonly string ItemId;

		public _E003(string itemId)
		{
			ItemId = itemId;
		}

		public override string ToString()
		{
			return _ED3E._E000(111181) + ItemId;
		}
	}

	public sealed class _E004 : _ECD1
	{
		public readonly Item Item;

		public readonly Vector3 PlayerPosition;

		public readonly Vector3 ItemPosition;

		public _E004(Item item, Vector3 playerPosition, Vector3 itemPosition)
		{
			Item = item;
			PlayerPosition = playerPosition;
			ItemPosition = itemPosition;
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(111219), Item, ItemPosition, PlayerPosition);
		}
	}

	public sealed class _E005 : _ECD1
	{
		public readonly Item Item;

		public readonly Player CurrentPlayer;

		[CanBeNull]
		public readonly Player FromPlayer;

		[CanBeNull]
		public readonly Player ToPlayer;

		public readonly bool KnownDirection;

		public _E005(Player currentPlayer, Item item, [CanBeNull] Player fromPlayer, [CanBeNull] Player toPlayer)
		{
			Item = item;
			CurrentPlayer = currentPlayer;
			FromPlayer = fromPlayer;
			ToPlayer = toPlayer;
			KnownDirection = true;
		}

		public _E005(Player currentPlayer, Player anotherPlayer, Item item)
		{
			Item = item;
			CurrentPlayer = currentPlayer;
			ToPlayer = anotherPlayer;
			KnownDirection = false;
		}

		public override string ToString()
		{
			if (!KnownDirection)
			{
				return string.Format(_ED3E._E000(111247), CurrentPlayer.FullIdInfo, Item, _E000(ToPlayer));
			}
			return string.Format(_ED3E._E000(111308), CurrentPlayer.FullIdInfo, Item, _E000(FromPlayer), _E000(ToPlayer));
		}

		[CompilerGenerated]
		private string _E000(Player player)
		{
			if (!(player == CurrentPlayer))
			{
				if (!(player == null))
				{
					return _ED3E._E000(111334) + player.FullIdInfo;
				}
				return _ED3E._E000(111380);
			}
			return _ED3E._E000(111367);
		}
	}

	public sealed class _E006 : _ECD1
	{
		public readonly Player Player;

		public _E006(Player player)
		{
			Player = player;
		}

		public override string ToString()
		{
			return _ED3E._E000(111422) + Player.FullIdInfo + _ED3E._E000(111414);
		}
	}

	public sealed class _E007 : _ECD1
	{
		public readonly Player Player;

		public _E007(Player player)
		{
			Player = player;
		}

		public override string ToString()
		{
			return _ED3E._E000(111422) + Player.FullIdInfo + _ED3E._E000(111434);
		}
	}

	public class _E008 : _ECD1
	{
		public readonly IItemOwner ItemOwner;

		public readonly Item Item;

		public readonly Vector3 OwnerPosition;

		public readonly Vector3 ItemPosition;

		public _E008(IItemOwner itemOwner, Item item, Vector3 ownerPosition, Vector3 itemPosition)
		{
			ItemOwner = itemOwner;
			Item = item;
			OwnerPosition = ownerPosition;
			ItemPosition = itemPosition;
		}

		public override string ToString()
		{
			return string.Concat(Item, _ED3E._E000(97243), ItemPosition, _ED3E._E000(111463), ItemOwner, _ED3E._E000(97243), OwnerPosition);
		}
	}

	public class _E009 : _ECD1
	{
		public readonly Item Item;

		public _E009(Item item)
		{
			Item = item;
		}

		public override string ToString()
		{
			return string.Concat(_ED3E._E000(111502), Item, _ED3E._E000(111488));
		}
	}

	[CompilerGenerated]
	private sealed class _E00C
	{
		public string id;

		internal bool _E000(LootItem x)
		{
			return x.ItemId.Equals(id);
		}
	}

	[CompilerGenerated]
	private sealed class _E00D
	{
		public string lootId;

		internal bool _E000(_E354 x)
		{
			return x.Id == lootId;
		}
	}

	[CompilerGenerated]
	private sealed class _E00E
	{
		public _E354 staticLootSpawn;

		internal bool _E000(_E545 lootItem)
		{
			return lootItem.Id != staticLootSpawn.Id;
		}
	}

	[CompilerGenerated]
	private sealed class _E010
	{
		public _E354 staticLootSpawn;

		internal bool _E000(_E545 lootItem)
		{
			return lootItem.Id != staticLootSpawn.Id;
		}
	}

	[CompilerGenerated]
	private sealed class _E012
	{
		public byte[] bytes;

		public _E545 lootItem;

		internal void _E000()
		{
			using MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(bytes));
			using BinaryReader reader = new BinaryReader(input);
			lootItem = _E672.DeserializeJsonLootItem(Singleton<_E63B>.Instance, new Dictionary<string, Item>(), reader.ReadPolymorph<_E66F>());
		}
	}

	[CompilerGenerated]
	private sealed class _E014
	{
		public string id;

		internal bool _E000(StationaryWeapon x)
		{
			return x.Id == id;
		}
	}

	[CompilerGenerated]
	private sealed class _E015
	{
		public string itemId;

		internal bool _E000(StationaryWeapon x)
		{
			return x.Item.Id == itemId;
		}
	}

	[CompilerGenerated]
	private sealed class _E016
	{
		public _E545 jsonLootItem;

		internal byte[] _E000()
		{
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(memoryStream);
			writer.WritePolymorph(_E672.SerializeJsonLootItem(jsonLootItem));
			byte[] array = memoryStream.ToArray();
			return SimpleZlib.CompressToBytes(array, array.Length, 9);
		}
	}

	[CompilerGenerated]
	private sealed class _E018
	{
		public string ownerId;

		internal bool _E000(KeyValuePair<IItemOwner, _E000> x)
		{
			return x.Key.ID == ownerId;
		}
	}

	[CompilerGenerated]
	private static Action m__E000;

	[CompilerGenerated]
	private _E57A m__E001;

	[CompilerGenerated]
	private _EBEB m__E002;

	[CompilerGenerated]
	private string m__E003;

	public _E629 GameDateTime;

	public GameWorldUnityTickListener UnityTickListener;

	public bool SpeedLimitsEnabled;

	public _E717.Config SpeedLimits = _E717.DefaultSpeedLimits;

	public string CurrentProfileId;

	[CompilerGenerated]
	private Action m__E004;

	[CompilerGenerated]
	private Action<_E5B4> m__E005;

	private readonly _E37A m__E006 = new _E37A();

	public AudioSourceCulling AudioSourceCulling;

	[CompilerGenerated]
	private _EC1E m__E007;

	public BallisticsCalculator _sharedBallisticsCalculator;

	public readonly List<_E2B8> LootList = new List<_E2B8>(1000);

	public readonly Dictionary<IItemOwner, _E000> ItemOwners = new Dictionary<IItemOwner, _E000>(100);

	public readonly _E373<int, LootItem> LootItems = new _E373<int, LootItem>(1000);

	public readonly List<_E545> AllLoot = new List<_E545>();

	public readonly List<Player> RegisteredPlayers = new List<Player>(40);

	public MovingPlatform._E001[] PlatformAdapters = Array.Empty<MovingPlatform._E001>();

	public BorderZone[] BorderZones;

	private MovingPlatform[] m__E008;

	public Player MainPlayer;

	[CompilerGenerated]
	private readonly List<Player> m__E009 = new List<Player>(40);

	[CompilerGenerated]
	private readonly List<Player> m__E00A = new List<Player>(40);

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	[CompilerGenerated]
	private readonly Dictionary<Collider, Player> m__E00B = new Dictionary<Collider, Player>(40);

	private readonly List<AmmoPoolObject> m__E00C = new List<AmmoPoolObject>(100);

	private readonly List<_E545> m__E00D = new List<_E545>();

	private static int m__E00E;

	private static int m__E00F;

	private static int m__E010;

	private static int m__E011;

	public SpeakerManager SpeakerManager;

	private static int m__E012;

	private EUpdateQueue _E013;

	public _E90E SynchronizableObjectLogicProcessor;

	public _E310 MineManager;

	[CompilerGenerated]
	private Action<_E2B8> m__E014;

	protected readonly Dictionary<int, Turnable> Turnables = new Dictionary<int, Turnable>(512);

	public readonly _E373<int, WindowBreaker> Windows = new _E373<int, WindowBreaker>(512);

	public readonly _E373<int, Throwable> Grenades = new _E373<int, Throwable>();

	public List<_E5C5> GrenadesCriticalStates = new List<_E5C5>();

	private _E334 m__E015;

	private bool m__E016;

	private _ED0E<_ED08>._E002 _E017;

	public _E307._E004 OnThrowItem;

	public _E307._E005 OnTakeItem;

	protected EffectsCommutator _effectsCommutator;

	private BallisticCollider m__E018;

	internal World _E019;

	private World _E01A;

	protected _E760 ObjectsFactory;

	private readonly Queue<byte[]> _E01B = new Queue<byte[]>();

	private Task _E01C;

	private List<WorldInteractiveObject> _E01D = new List<WorldInteractiveObject>();

	public _E57A ExfiltrationController
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		set
		{
			this.m__E001 = value;
		}
	}

	public _EBEB BufferZoneController
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		set
		{
			this.m__E002 = value;
		}
	}

	public string LocationId
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		set
		{
			this.m__E003 = value;
		}
	}

	public float DeltaTime
	{
		get
		{
			float num = this.m__E006.DeltaTime;
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}
	}

	public _EC1E ClientBallisticCalculator
	{
		[CompilerGenerated]
		get
		{
			return this.m__E007;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E007 = value;
		}
	}

	public _EC1E SharedBallisticsCalculator
	{
		get
		{
			return _sharedBallisticsCalculator;
		}
		private set
		{
			_sharedBallisticsCalculator = value as BallisticsCalculator;
		}
	}

	public MovingPlatform[] Platforms
	{
		get
		{
			return this.m__E008 ?? (this.m__E008 = LocationScene.GetAllObjectsAndWhenISayAllIActuallyMeanIt<MovingPlatform>().ToArray());
		}
		set
		{
			this.m__E008 = value;
		}
	}

	public List<Player> AllPlayers
	{
		[CompilerGenerated]
		get
		{
			return this.m__E009;
		}
	}

	public List<Player> AllPlayersObjects
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00A;
		}
	}

	protected Dictionary<Collider, Player> PlayersColliders
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00B;
		}
	}

	public EUpdateQueue UpdateQueue => this._E013;

	public _E334 GrenadeFactory => this.m__E015;

	private World _E01E => _E01A ?? (_E01A = GetComponent<World>());

	public virtual ulong TotalOutgoingBytes => 0uL;

	public virtual ulong TotalIngoingBytes => 0uL;

	public static event Action OnDispose
	{
		[CompilerGenerated]
		add
		{
			Action action = GameWorld.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref GameWorld.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = GameWorld.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref GameWorld.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action AfterGameStarted
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E004, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E5B4> OnPersonAdd
	{
		[CompilerGenerated]
		add
		{
			Action<_E5B4> action = this.m__E005;
			Action<_E5B4> action2;
			do
			{
				action2 = action;
				Action<_E5B4> value2 = (Action<_E5B4>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E005, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E5B4> action = this.m__E005;
			Action<_E5B4> action2;
			do
			{
				action2 = action;
				Action<_E5B4> value2 = (Action<_E5B4>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E005, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E2B8> OnLootItemDestroyed
	{
		[CompilerGenerated]
		add
		{
			Action<_E2B8> action = this.m__E014;
			Action<_E2B8> action2;
			do
			{
				action2 = action;
				Action<_E2B8> value2 = (Action<_E2B8>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E014, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E2B8> action = this.m__E014;
			Action<_E2B8> action2;
			do
			{
				action2 = action;
				Action<_E2B8> value2 = (Action<_E2B8>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E014, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static TGameWorld Create<TGameWorld>(GameObject gameObject, _E760 objectsFactory, EUpdateQueue updateQueue, string currentProfileId) where TGameWorld : GameWorld
	{
		TGameWorld val = gameObject.AddComponent<TGameWorld>();
		val.ObjectsFactory = objectsFactory;
		val._E013 = updateQueue;
		val.SpeakerManager = gameObject.AddComponent<SpeakerManager>();
		val.ExfiltrationController = new _E57A();
		val.BufferZoneController = new _EBEB();
		val.CurrentProfileId = currentProfileId;
		val.UnityTickListener = GameWorldUnityTickListener.Create(gameObject, val);
		return val;
	}

	public IEnumerator<_E5B4> GetEnumerator()
	{
		return RegisteredPlayers.Where((Player x) => x.HealthController.IsAlive).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[CanBeNull]
	public MovingPlatform GetPlatformAtPosition(Vector3 position)
	{
		MovingPlatform[] platforms = Singleton<GameWorld>.Instance.Platforms;
		foreach (MovingPlatform movingPlatform in platforms)
		{
			if (movingPlatform.Area != null && movingPlatform.Area.ContainsPointConsiderCenter(position))
			{
				return movingPlatform;
			}
		}
		return null;
	}

	public void BoardIfOnPlatform(MovingPlatform._E000 transportee, Vector3 position)
	{
		MovingPlatform platformAtPosition = GetPlatformAtPosition(position);
		if ((bool)platformAtPosition)
		{
			transportee.Board(platformAtPosition);
		}
	}

	public AmmoPoolObject SpawnShellInTheWorld(ref AmmoPoolObject shell)
	{
		AmmoPoolObject ammoPoolObject = shell;
		shell = null;
		this.m__E00C.Add(ammoPoolObject);
		return ammoPoolObject;
	}

	_EC1E _E5CE.CreateBallisticCalculator(int seed)
	{
		return SharedBallisticsCalculator;
	}

	public void RemoveBallisticCalculator(Item weapon)
	{
	}

	public _EC1E CreateClientBallisticsCalculator()
	{
		if (ClientBallisticCalculator == null)
		{
			ClientBallisticCalculator = BallisticsCalculator.Create(base.gameObject, 0, ShotDelegate);
		}
		return ClientBallisticCalculator;
	}

	public void RemoveClientBallisticsCalculator()
	{
	}

	public void RegisterGrenade(Throwable grenade)
	{
		try
		{
			Grenades.Add(grenade.Id, grenade);
			grenade.DestroyEvent += _E001;
		}
		catch (Exception)
		{
			Debug.LogError(string.Format(_ED3E._E000(112214), grenade.Id));
			if (Grenades.TryGetByKey(grenade.Id, out var value))
			{
				Debug.LogError(string.Format(_ED3E._E000(112284), value.Id));
			}
		}
	}

	private void _E000(Throwable grenade)
	{
		grenade.DestroyEvent -= _E001;
		_E002(grenade);
		Grenades.Remove(grenade.Id);
	}

	private void _E001(Throwable grenade)
	{
		_E000(grenade);
	}

	private void _E002(Throwable grenade)
	{
		if (grenade.HasNetData)
		{
			_E5C5 netPacket = grenade.GetNetPacket();
			GrenadesCriticalStates.Add(netPacket);
		}
	}

	protected virtual void Awake()
	{
		Grenade.GrenadeRandoms = new _EC17(128, 1337);
		this.m__E015 = CreateGrenadeFactory();
		foreach (WorldInteractiveObject allObject in LocationScene.GetAllObjects<WorldInteractiveObject>())
		{
			allObject.OnDoorStateChanged += _E010;
		}
	}

	protected virtual _E334 CreateGrenadeFactory()
	{
		return new _E334();
	}

	internal virtual void _E012(World world)
	{
		_E019 = world;
		ExfiltrationController.StatusChanged += _E019.OnExfiltrationPointUpdate;
	}

	public void RegisterInteractiveObject(Turnable t)
	{
		if (!Turnables.ContainsKey(t.NetId))
		{
			Turnables.Add(t.NetId, t);
		}
		else
		{
			Debug.LogError(string.Format(_ED3E._E000(112258), t.NetId));
		}
	}

	public void ChangeLampState(Turnable turnable, Turnable.EState state)
	{
		turnable.Switch(state);
	}

	public void RegisterWindow(WindowBreaker window)
	{
		if (!Windows.TryGetByKey(window.NetId, out var value))
		{
			Windows.Add(window.NetId, window);
			return;
		}
		Debug.LogError(_ED3E._E000(112337) + window.Id + _ED3E._E000(112360) + value.gameObject.scene.name + _ED3E._E000(112352) + value.transform.GetFullPath(), window);
	}

	protected virtual bool IsLocalGame()
	{
		return false;
	}

	public virtual void InitAirdrop(bool takeNearbyPoint = false, Vector3 position = default(Vector3))
	{
	}

	public void RegisterWorldInteractionObject(WorldInteractiveObject worldInteractiveObject)
	{
		_E01E.RegisterWorldInteractionObject(worldInteractiveObject);
	}

	public virtual async Task InitLevel(_E63B itemFactory, _E761 config, bool loadBundlesAndCreatePools = true, List<ResourceKey> resources = null, IProgress<_E5BB> progress = null, CancellationToken ct = default(CancellationToken))
	{
		_E2BA.LogMemoryConsumption(_ED3E._E000(111545));
		_EC26.FillPool();
		if (base.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		await PreloadAdditionalData();
		await EFTHardSettings.Load();
		_E8EE.Int();
		using (_E069.StartWithToken(_ED3E._E000(111533)))
		{
			await _E003();
		}
		this.m__E016 = loadBundlesAndCreatePools;
		if (this.m__E016)
		{
			using (_E069.StartWithToken(_ED3E._E000(111583)))
			{
				if (resources == null)
				{
					resources = new List<ResourceKey>();
				}
				resources.AddRange(_E5D2.BUNDLES_TO_PRELOAD.Select((string x) => new ResourceKey
				{
					path = x
				}));
				await ObjectsFactory.RegisterLoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, base.transform, config, (!IsLocalGame()) ? _E760.AssemblyType.Online : _E760.AssemblyType.Local, resources.ToArray(), _ECE3.General, progress, ct);
			}
		}
		SharedBallisticsCalculator = BallisticsCalculator.Create(base.gameObject, 0, ShotDelegate);
		this.m__E018 = SharedBallisticsCalculator.DefaultHitBody;
		this.m__E018.transform.SetParent(base.transform, worldPositionStays: true);
		Singleton<BetterAudio>.Instance.LoadSoundBundles();
		_E2BA.LogMemoryConsumption(_ED3E._E000(111609));
		try
		{
			BallisticCalculatorPrewarmer ballisticCalculatorPrewarmer = base.gameObject.AddComponent<BallisticCalculatorPrewarmer>();
			_EC26 obj2 = ballisticCalculatorPrewarmer.SimulateShot();
			ShotDelegate(obj2);
			UnityEngine.Object.Destroy(ballisticCalculatorPrewarmer);
			_EC26.Release(obj2);
		}
		catch (Exception ex)
		{
			Debug.LogWarning(_ED3E._E000(111597) + ex);
		}
		MineManager = new _E310();
	}

	private async Task _E003()
	{
		this._E017 = Singleton<_ED0A>.Instance.Retain(new string[1] { _ED3E._E000(113675) });
		await _E612.LoadBundles(this._E017);
		GameObject obj = Singleton<_ED0A>.Instance.InstantiateAsset<GameObject>(_ED3E._E000(113675));
		obj.transform.SetParent(base.transform, worldPositionStays: true);
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		if (Singleton<Effects>.Instantiated)
		{
			if (Singleton<Effects>.Instance.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(Singleton<Effects>.Instance.gameObject);
			}
			Singleton<Effects>.Release(Singleton<Effects>.Instance);
		}
		Singleton<Effects>.Create(obj.GetComponent<Effects>());
		Singleton<Effects>.Instance.CacheEffects();
	}

	public void RegisterLoot<T>(T loot) where T : InteractableObject
	{
		LootList.Add(loot);
		if (loot is LootItem lootItem)
		{
			ItemOwners.Add(lootItem.ItemOwner, new _E000
			{
				Transform = loot.TrackableTransform,
				DoNotPerformPickUpValidation = !lootItem.PerformPickUpValidation,
				ItemOwner = lootItem.ItemOwner
			});
			int netId = lootItem.GetNetId();
			try
			{
				LootItems.Add(netId, lootItem);
			}
			catch (Exception)
			{
				Debug.LogError(string.Format(_ED3E._E000(112415), lootItem.ItemId, lootItem.Item.Id, netId, lootItem.ItemOwner, lootItem));
				if (LootItems.TryGetByKey(netId, out var value))
				{
					Debug.LogError(string.Format(_ED3E._E000(112490), value.ItemId, value.Item.Id, value.GetNetId(), value.ItemOwner, value));
				}
			}
		}
		if (loot is LootableContainer lootableContainer)
		{
			ItemOwners.Add(lootableContainer.ItemOwner, new _E000
			{
				Transform = lootableContainer.TrackableTransform
			});
		}
	}

	public void DestroyAllLoot()
	{
		LootItem[] array = LootList.OfType<LootItem>().ToArray();
		foreach (LootItem loot in array)
		{
			DestroyLoot(loot);
		}
	}

	public void DestroyLoot(string id)
	{
		LootItem lootItem = LootList.OfType<LootItem>().FirstOrDefault((LootItem x) => x.ItemId.Equals(id));
		if (lootItem != null)
		{
			DestroyLoot(lootItem);
		}
		else
		{
			Debug.LogError(_ED3E._E000(112601) + id + _ED3E._E000(112581));
		}
	}

	public void DestroyLoot(_E2B8 loot)
	{
		if (this.m__E014 != null)
		{
			this.m__E014(loot);
		}
		LootList.Remove(loot);
		LootItem lootItem = loot as LootItem;
		if (lootItem != null)
		{
			ItemOwners.Remove(lootItem.ItemOwner);
			LootItems.Remove(lootItem.GetNetId());
			OnTakeItem?.Invoke(lootItem);
		}
		if (loot is LootableContainer && ((LootableContainer)loot).ItemOwner != null)
		{
			ItemOwners.Remove(((LootableContainer)loot).ItemOwner);
		}
		foreach (Player registeredPlayer in RegisteredPlayers)
		{
			registeredPlayer.ResetInteractionRaycast(loot);
		}
		loot.Kill();
	}

	internal _E548 _E004(_E548 lootItems)
	{
		_E548 obj = new _E548();
		foreach (_E545 lootItem in lootItems)
		{
			if (lootItem.Item.QuestItem)
			{
				this.m__E00D.Add(lootItem);
			}
			else
			{
				obj.Add(lootItem);
			}
		}
		return obj;
	}

	protected virtual LootItem CreateStaticLoot(GameObject lootObject, Item item, string lootName, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, string staticId = null, Vector3 shift = default(Vector3))
	{
		return LootItem.CreateStaticLoot<LootItem>(lootObject, item, lootName, gameWorld, randomRotation, validProfiles, staticId, performPickUpValidation: true, shift);
	}

	internal void _E005(_E548 lootItems, bool initial)
	{
		Platforms = LocationScene.GetAllObjectsAndWhenISayAllIActuallyMeanIt<MovingPlatform>().ToArray();
		Locomotive[] array = Platforms.OfType<Locomotive>().ToArray();
		PlatformAdapters = new MovingPlatform._E001[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			PlatformAdapters[i] = new MovingPlatform._E001
			{
				Platform = array[i],
				Id = (byte)i
			};
		}
		_E354[] array2 = LocationScene.GetAllObjects<StaticLoot>().Cast<_E354>().Concat(LocationScene.GetAllObjects<LootableContainer>().Cast<_E354>())
			.Concat(LocationScene.GetAllObjects<StationaryWeapon>().Cast<_E354>())
			.ToArray();
		if (initial)
		{
			array2 = _E006(lootItems, array2);
		}
		foreach (_E545 lootItem2 in lootItems)
		{
			string lootId = lootItem2.Id;
			Item item = lootItem2.Item;
			string shortName = item.ShortName;
			AllLoot.Add(lootItem2);
			if (initial)
			{
				item.SpawnedInSession = true;
				if (item is ContainerCollection topLevelCollection)
				{
					foreach (Item item2 in topLevelCollection.GetAllItemsFromCollection())
					{
						item2.SpawnedInSession = true;
					}
				}
			}
			if (lootItem2.IsStatic)
			{
				_E354 obj = array2.FirstOrDefault((_E354 x) => x.Id == lootId);
				if (obj != null)
				{
					if (!(obj is StaticLoot staticLoot))
					{
						if (!(obj is LootableContainer lc))
						{
							if (obj is StationaryWeapon stationaryWeapon)
							{
								LootItem.CreateStationaryWeapon(stationaryWeapon, item, shortName, this, lootId);
							}
						}
						else
						{
							LootItem.CreateLootContainer(lc, item, shortName, this, lootId);
						}
					}
					else
					{
						StaticLoot staticLoot2 = staticLoot;
						new _EB1E(item, item.Id, shortName);
						CreateStaticLoot(staticLoot2.gameObject, item, shortName, this, lootItem2.randomRotation, lootItem2.ValidProfiles, lootId, lootItem2.Shift);
					}
				}
				else
				{
					Debug.LogError(lootId + _ED3E._E000(112610));
				}
			}
			else if (lootItem2 is _E546 lootItem)
			{
				SpawnLootCorpse(lootItem);
			}
			else
			{
				if (lootItem2.PlatformId > -1)
				{
					Debug.Log(_ED3E._E000(110614).Yellow());
				}
				_E00C(lootItem2, initial, null, (lootItem2.PlatformId > -1) ? Platforms[lootItem2.PlatformId] : null);
			}
		}
	}

	protected virtual void SpawnLootCorpse(_E546 lootItem)
	{
		SpawnLootCorpse<Corpse>(lootItem);
	}

	protected void SpawnLootCorpse<T>(_E546 corpseJson) where T : Corpse
	{
		Corpse.CreateStillCorpse<T>(Singleton<GameWorld>.Instance, corpseJson, (corpseJson.PlatformId > -1) ? Platforms[corpseJson.PlatformId] : null);
	}

	private _E354[] _E006(_E548 lootItems, _E354[] staticLootSpawns)
	{
		foreach (_E354 staticLootSpawn in staticLootSpawns)
		{
			if (lootItems.All((_E545 lootItem) => lootItem.Id != staticLootSpawn.Id))
			{
				GameObject gameObject = ((MonoBehaviour)staticLootSpawn).gameObject;
				string text = string.Join(_ED3E._E000(30703), Enumerable.Repeat(gameObject.scene.name, 1).Concat(from x in _E007(gameObject.transform).Reverse()
					select x.name).ToArray());
				Debug.LogError(_ED3E._E000(110644) + staticLootSpawn.Id + _ED3E._E000(110633) + text + _ED3E._E000(110679), gameObject);
				UnityEngine.Object.DestroyImmediate((MonoBehaviour)staticLootSpawn);
			}
		}
		return staticLootSpawns.Where((_E354 x) => x != null).ToArray();
	}

	private static IEnumerable<Transform> _E007(Transform transform)
	{
		while (transform != null)
		{
			yield return transform;
			transform = transform.parent;
		}
	}

	internal void _E008(_E548 lootItems)
	{
		_E354[] array = LocationScene.GetAllObjects<StaticLoot>().Concat(LocationScene.GetAllObjects<LootableContainer>().Cast<_E354>()).ToArray();
		foreach (_E354 staticLootSpawn in array)
		{
			if (lootItems.All((_E545 lootItem) => lootItem.Id != staticLootSpawn.Id))
			{
				GameObject gameObject = ((MonoBehaviour)staticLootSpawn).gameObject;
				string text = string.Join(_ED3E._E000(30703), Enumerable.Repeat(gameObject.scene.name, 1).Concat(from x in _E007(gameObject.transform).Reverse()
					select x.name).ToArray());
				Debug.LogError(_ED3E._E000(110644) + staticLootSpawn.Id + _ED3E._E000(110633) + text + _ED3E._E000(110679), gameObject);
				UnityEngine.Object.DestroyImmediate((MonoBehaviour)staticLootSpawn);
			}
		}
	}

	public virtual LootItem CreateLootWithRigidbody(GameObject lootObject, Item item, string objectName, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, out BoxCollider objectCollider, bool forceLayerSetup = false, bool syncable = true, bool performPickUpValidation = true, float makeVisibleAfterDelay = 0f)
	{
		return LootItem.CreateLootWithRigidbody<LootItem>(lootObject, item, objectName, gameWorld, randomRotation, validProfiles, out objectCollider, forceLayerSetup, performPickUpValidation, makeVisibleAfterDelay);
	}

	internal void _E009(byte[] bytes)
	{
		_E01B.Enqueue(bytes);
		_E00A();
	}

	private async void _E00A()
	{
		try
		{
			if (_E01C == null)
			{
				_E01C = _E00B();
				await _E01C;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			_E01C = null;
			_E00A();
		}
	}

	private async Task _E00B()
	{
		while (_E01B.Count > 0)
		{
			byte[] bytes = _E01B.Dequeue();
			_E545 lootItem = null;
			await AsyncWorker.RunOnBackgroundThread(delegate
			{
				using MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(bytes));
				using BinaryReader reader = new BinaryReader(input);
				lootItem = _E672.DeserializeJsonLootItem(Singleton<_E63B>.Instance, new Dictionary<string, Item>(), reader.ReadPolymorph<_E66F>());
			});
			if (lootItem != null)
			{
				_E00C(lootItem, initial: true);
			}
			await Task.Yield();
		}
		_E01C = null;
	}

	[CanBeNull]
	private LootItem _E00C(_E545 lootItem, bool initial, Player questPlayer = null, MovingPlatform platform = null)
	{
		Item item = ((questPlayer == null) ? lootItem.Item : lootItem.Item.CloneItem());
		new _EB1E(item, item.Id, item.ShortName);
		GameObject gameObject = Singleton<_E760>.Instance.CreateLootPrefab(item);
		gameObject.SetActive(value: true);
		if (platform != null)
		{
			gameObject.transform.position = platform.transform.TransformPoint(lootItem.Position);
		}
		else
		{
			gameObject.transform.position = lootItem.Position;
			gameObject.transform.rotation = Quaternion.Euler(lootItem.Rotation);
		}
		BoxCollider objectCollider;
		LootItem lootItem2 = (lootItem.useGravity ? CreateLootWithRigidbody(gameObject, item, item.ShortName, this, lootItem.randomRotation, lootItem.ValidProfiles, out objectCollider, forceLayerSetup: false, syncable: false) : CreateStaticLoot(gameObject, item, item.ShortName, this, lootItem.randomRotation, lootItem.ValidProfiles, null, lootItem.Shift));
		if (platform != null)
		{
			lootItem2.Board(platform);
			gameObject.transform.localRotation = Quaternion.Euler(lootItem.Rotation);
			return lootItem2;
		}
		PreviewPivot component = gameObject.GetComponent<PreviewPivot>();
		if (component == null || component.SpawnPosition == Vector3.zero)
		{
			return lootItem2;
		}
		GameObject gameObject2 = new GameObject(_ED3E._E000(110706));
		gameObject2.transform.position = lootItem.Position;
		gameObject2.transform.rotation = Quaternion.Euler(lootItem.Rotation);
		Transform transform = lootItem2.transform;
		transform.SetParent(gameObject2.transform);
		transform.localPosition = (initial ? (-component.SpawnPosition) : Vector3.zero);
		if (lootItem.randomRotation)
		{
			transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0, 360), 0f));
		}
		transform.localScale = Vector3.one;
		return lootItem2;
	}

	protected virtual Task PreloadBundles()
	{
		return Task.CompletedTask;
	}

	protected virtual Task PreloadAdditionalData()
	{
		return Task.CompletedTask;
	}

	public LootItem ThrowItem(Item item, Player player, Vector3? direction)
	{
		Vector3 vector = direction ?? (Quaternion.Euler(Mathf.Clamp(player.MovementContext.Pitch, -90f, 45f), player.MovementContext.Yaw, 0f) * new Vector3(0f, 1f, 1f));
		vector *= 2f;
		Vector3 position = player.MovementContext.PlayerColliderPointOnCenterAxis(0.65f) + player.Velocity * Time.deltaTime;
		Quaternion rotation = player.PlayerBones.WeaponRoot.rotation * Quaternion.Euler(90f, 0f, 0f);
		return ThrowItem(angularVelocity: new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 2f * Mathf.Sign(UnityEngine.Random.Range(-1, 2))), item: item, player: player, position: position, rotation: rotation, velocity: vector + player.Velocity / 2f, syncable: true, performPickUpValidation: true, makeVisibleAfterDelay: EFTHardSettings.Instance.ThrowLootMakeVisibleDelay);
	}

	public LootItem ThrowItem(Item item, Player player, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, bool syncable = true, bool performPickUpValidation = true, float makeVisibleAfterDelay = 0f)
	{
		try
		{
			if (item == null)
			{
				Debug.LogError(_ED3E._E000(110748));
			}
			new _EB1E(item, item?.Id, item?.ShortName);
			if (Singleton<_E760>.Instance == null)
			{
				Debug.LogError(_ED3E._E000(110737));
			}
			GameObject gameObject = Singleton<_E760>.Instance.CreateLootPrefab(item);
			if (gameObject == null)
			{
				Debug.LogError(_ED3E._E000(110756));
			}
			gameObject.SetActive(value: true);
			BoxCollider objectCollider;
			LootItem lootItem = CreateLootWithRigidbody(gameObject, item, item?.ShortName, this, randomRotation: false, null, out objectCollider, forceLayerSetup: true, syncable, performPickUpValidation, makeVisibleAfterDelay);
			try
			{
				lootItem.transform.SetPositionAndRotation(position, rotation);
				lootItem.RigidBody.velocity = velocity;
				lootItem.RigidBody.angularVelocity = angularVelocity;
				lootItem.LastOwner = player;
				lootItem.CacheParameters();
				OnThrowItem?.Invoke(lootItem);
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format(_ED3E._E000(110811), item?.Name, item?.Id, lootItem, ex.Message, ex.StackTrace));
			}
			return lootItem;
		}
		catch (Exception ex2)
		{
			Debug.LogError(item?.Name + _ED3E._E000(18502) + item?.Id + _ED3E._E000(29184) + ex2.Message + _ED3E._E000(110840) + ex2.StackTrace);
		}
		return null;
	}

	public LootItem SetupItem(Item item, Player player, Vector3 position, Quaternion rotation)
	{
		new _EB1E(item, item.Id, item.ShortName);
		GameObject gameObject = Singleton<_E760>.Instance.CreateLootPrefab(item);
		gameObject.SetActive(value: true);
		BoxCollider objectCollider;
		LootItem lootItem = CreateLootWithRigidbody(gameObject, item, item.ShortName, this, randomRotation: false, null, out objectCollider);
		lootItem.GetComponent<Rigidbody>().isKinematic = true;
		lootItem.transform.SetPositionAndRotation(position + rotation * lootItem.Shift, rotation);
		lootItem.LastOwner = player;
		return lootItem;
	}

	protected virtual void Start()
	{
		GameWorld.m__E00E = LayerMask.GetMask(_ED3E._E000(60775), _ED3E._E000(55338), _ED3E._E000(60863));
		GameWorld.m__E00F = LayerMask.GetMask(_ED3E._E000(60775), _ED3E._E000(55338), _ED3E._E000(60679), _ED3E._E000(60863));
		GameWorld.m__E010 = LayerMask.GetMask(_ED3E._E000(60679));
		GameWorld.m__E011 = LayerMask.GetMask(_ED3E._E000(25428), _ED3E._E000(60852));
	}

	public virtual void ShotDelegate(_EC26 shotResult)
	{
		if (shotResult.IsFlyingOutOfTime)
		{
			return;
		}
		_EC23 damageInfo = new _EC23(EDamageType.Bullet, shotResult);
		_EC22 shotID = new _EC22(shotResult.Ammo.Id, shotResult.FragmentIndex);
		_E6FF playerHitInfo = shotResult.HittedBallisticCollider.ApplyHit(damageInfo, shotID);
		shotResult.AddClientHitPosition(playerHitInfo);
		_E9D6 itemComponent = shotResult.Ammo.GetItemComponent<_E9D6>();
		if (itemComponent != null && shotResult.TimeSinceShot >= itemComponent.Template.FuzeArmTimeSec)
		{
			string explosionType = itemComponent.Template.ExplosionType;
			if (!string.IsNullOrEmpty(explosionType) && shotResult.IsFirstHit)
			{
				Singleton<Effects>.Instance.EmitGrenade(explosionType, shotResult.HitPoint, shotResult.HitNormal, shotResult.IsForwardHit ? 1 : 0);
			}
			Grenade.Explosion(null, itemComponent, shotResult.HitPoint, shotResult.Player, SharedBallisticsCalculator, shotResult.Weapon, shotResult.HitNormal * 0.08f);
		}
	}

	public virtual void NetworkWorldOnGrenadeHit(Vector3 pos)
	{
	}

	public virtual _E6FF HackShot(_EC23 damageInfo)
	{
		if (null == damageInfo.HittedBallisticCollider)
		{
			damageInfo.HittedBallisticCollider = this.m__E018;
		}
		return damageInfo.HittedBallisticCollider.ApplyHit(damageInfo, _EC22.EMPTY_SHOT_ID);
	}

	protected void Update()
	{
		this.m__E006.RecordDeltaTime();
		if (_E8A8.Instance.Camera == null)
		{
			return;
		}
		for (int num = this.m__E00C.Count - 1; num >= 0; num--)
		{
			AmmoPoolObject ammoPoolObject = this.m__E00C[num];
			if (ammoPoolObject.ShouldBeDestroyed)
			{
				this.m__E00C.RemoveAt(num);
				AssetPoolObject.ReturnToPool(ammoPoolObject.gameObject);
			}
		}
	}

	public void DoWorldTick(float dt)
	{
		BeforeWorldTick(dt);
		BeforePlayerTick(dt);
		PlayerTick(dt);
		BallisticsTick(dt);
		AfterPlayerTick(dt);
		OtherElseWorldTick(dt);
		AfterWorldTick(dt);
	}

	public void DoOtherWorldTick(float dt)
	{
		SafePlayerOperation(delegate(Player player)
		{
			if (player.UpdateQueue == EUpdateQueue.Update)
			{
				player.FixedUpdateTick();
			}
			else if (player.UpdateQueue == EUpdateQueue.FixedUpdate)
			{
				player.UpdateTick();
			}
		});
	}

	protected virtual void BeforeWorldTick(float dt)
	{
	}

	protected virtual void BallisticsTick(float dt)
	{
		SharedBallisticsCalculator?.ManualUpdate(dt);
	}

	protected virtual void BeforePlayerTick(float dt)
	{
	}

	protected virtual void PlayerTick(float dt)
	{
		SafePlayerOperation(delegate(Player player)
		{
			if (player.UpdateQueue == EUpdateQueue.Update)
			{
				player.UpdateTick();
			}
			else if (player.UpdateQueue == EUpdateQueue.FixedUpdate)
			{
				player.FixedUpdateTick();
			}
		});
	}

	protected virtual void AfterPlayerTick(float dt)
	{
		SafePlayerOperation(delegate(Player player)
		{
			player.AfterMainTick();
		});
	}

	protected virtual void OtherElseWorldTick(float dt)
	{
		SpeakerManager.ManualUpdate(dt);
	}

	protected virtual void AfterWorldTick(float dt)
	{
	}

	protected void SafePlayerOperation(Action<Player> operation)
	{
		foreach (Player allPlayersObject in AllPlayersObjects)
		{
			try
			{
				operation(allPlayersObject);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat(_ED3E._E000(110836), allPlayersObject.FullIdInfo, ex);
			}
		}
	}

	protected virtual void OnDestroy()
	{
	}

	public virtual void Dispose()
	{
		using (_E069.StartWithToken(_ED3E._E000(110864)))
		{
			for (int num = LootList.Count - 1; num >= 0; num--)
			{
				try
				{
					_E2B8 obj = LootList[num];
					if ((UnityEngine.Object)obj != null)
					{
						obj.Kill();
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
		foreach (AmmoPoolObject item in this.m__E00C)
		{
			AssetPoolObject.ReturnToPool(item.gameObject);
		}
		this.m__E00C.Clear();
		LootList.Clear();
		LootItems.Clear();
		if (this.m__E016)
		{
			using (_E069.StartWithToken(_ED3E._E000(110858)))
			{
				if (ObjectsFactory != null)
				{
					ObjectsFactory.UnloadTemporaryPools(cleanUselessPools: true);
					ObjectsFactory.UnloadBundles();
				}
			}
		}
		if (this._E017 != null)
		{
			this._E017.Release();
			this._E017 = null;
		}
		CompositeDisposable.Dispose();
		ExfiltrationController.Dispose();
		BufferZoneController.Dispose();
		_EBDB.Instance.Dispose();
		_E019 = null;
		GameWorld.m__E000?.Invoke();
		GameWorld.m__E000 = null;
	}

	public static bool InteractionSense(Vector3 origin, Vector3 direction, float radius, float distance)
	{
		RaycastHit hitInfo;
		bool num = Physics.SphereCast(origin, radius, direction, out hitInfo, distance, GameWorld.m__E00E);
		LootItem lootItem = null;
		Switch @switch = null;
		if (num)
		{
			lootItem = hitInfo.collider.gameObject.GetComponentInParent<LootItem>();
			if (lootItem == null)
			{
				Switch component = hitInfo.collider.gameObject.GetComponent<Switch>();
				if (component != null && component.Operatable && component.DoorState == EDoorState.Shut)
				{
					@switch = component;
				}
			}
		}
		if (!(lootItem != null) || lootItem is Corpse || (lootItem.Item is Weapon weapon && weapon.IsOneOff && weapon.Repairable.Durability == 0f) || !lootItem.enabled)
		{
			return @switch != null;
		}
		return true;
	}

	[CanBeNull]
	public static GameObject FindInteractable(Ray ray, out RaycastHit hit)
	{
		GameObject gameObject = (_E320.Raycast(ray, out hit, Mathf.Max(EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE, EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST), GameWorld.m__E00F) ? hit.collider.gameObject : null);
		if ((bool)gameObject && !Physics.Linecast(ray.origin, hit.point, GameWorld.m__E011))
		{
			return gameObject;
		}
		return null;
	}

	public static Player FindInteractablePlayer(Ray ray, out RaycastHit hit)
	{
		GameObject gameObject = (Physics.Raycast(ray, out hit, EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE, GameWorld.m__E010) ? hit.collider.gameObject : null);
		if ((bool)gameObject && !Physics.Linecast(ray.origin, hit.point, GameWorld.m__E011))
		{
			return gameObject.GetComponent<Player>();
		}
		return null;
	}

	public _E545[] GetJsonLootItems()
	{
		try
		{
			return _E00D();
		}
		catch (NullReferenceException exception)
		{
			Debug.LogException(exception);
			for (int num = LootList.Count - 1; num >= 0; num--)
			{
				if (LootList[num] == null)
				{
					LootList.RemoveAt(num);
				}
			}
			return _E00D();
		}
	}

	private _E545[] _E00D()
	{
		List<_E545> list = new List<_E545>(LootList.Count);
		foreach (_E2B8 loot in LootList)
		{
			if (loot is LootItem lootItem)
			{
				list.Add(SerializeLootItem(lootItem));
			}
		}
		foreach (LootableContainer allObject in LocationScene.GetAllObjects<LootableContainer>())
		{
			if (allObject.ItemOwner != null)
			{
				list.Add(new _E545
				{
					Position = allObject.transform.position,
					Rotation = allObject.transform.rotation.eulerAngles,
					Item = allObject.ItemOwner.RootItem,
					ValidProfiles = null,
					Id = allObject.Id,
					IsStatic = true
				});
			}
		}
		foreach (StationaryWeapon allObject2 in LocationScene.GetAllObjects<StationaryWeapon>())
		{
			if (!(allObject2 == null) && allObject2.ItemController != null)
			{
				list.Add(new _E545
				{
					Position = allObject2.transform.position,
					Rotation = allObject2.transform.rotation.eulerAngles,
					Item = allObject2.ItemController.RootItem,
					ValidProfiles = null,
					Id = allObject2.Id,
					IsStatic = true
				});
			}
		}
		_E545[] array = list.ToArray();
		Array.Sort(array, (_E545 a, _E545 b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
		return array;
	}

	public _E545 SerializeLootItem(LootItem lootItem)
	{
		short num = -1;
		if (Platforms.Length != 0 && lootItem.Platform != null)
		{
			num = (short)Array.IndexOf(Platforms, lootItem.Platform);
		}
		_E545 obj = ((!(lootItem is Corpse corpse)) ? new _E545() : new _E546
		{
			Customization = corpse.Customization,
			Side = corpse.Side,
			Bones = ((num > -1) ? corpse.TransformSyncsRelativeToPlatform : corpse.TransformSyncs)
		});
		Transform transform = lootItem.transform;
		obj.Position = ((num > -1) ? transform.localPosition : transform.position);
		obj.Rotation = ((num > -1) ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles);
		obj.Item = lootItem.ItemOwner.RootItem;
		obj.ValidProfiles = lootItem.ValidProfiles;
		obj.Id = lootItem.StaticId;
		obj.IsStatic = lootItem.StaticId != null;
		obj.Shift = lootItem.Shift;
		obj.PlatformId = num;
		return obj;
	}

	[CanBeNull]
	public static LootItem FindLootItem(string itemId, Vector3 position)
	{
		Collider[] array = Physics.OverlapSphere(position, 100f, GameWorld.m__E00E, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array.Length; i++)
		{
			LootItem componentInParent = array[i].gameObject.GetComponentInParent<LootItem>();
			if (componentInParent != null && componentInParent.enabled && componentInParent.ItemOwner.RootItem.Id == itemId)
			{
				return componentInParent;
			}
		}
		Debug.LogError(_ED3E._E000(110909) + itemId + _ED3E._E000(110881) + position);
		return DevelopFindLootItem(itemId);
	}

	[CanBeNull]
	public static IItemOwner FindLootOrContainer(string itemId, Vector3 position)
	{
		Collider[] array = Physics.OverlapSphere(position, 100f, GameWorld.m__E00E, QueryTriggerInteraction.Collide);
		foreach (Collider collider in array)
		{
			LootItem componentInParent = collider.gameObject.GetComponentInParent<LootItem>();
			if (componentInParent != null && componentInParent.enabled)
			{
				if (componentInParent.ItemOwner == null)
				{
					Debug.LogErrorFormat(componentInParent, _ED3E._E000(110935), componentInParent);
					continue;
				}
				if (componentInParent.ItemOwner.RootItem.Id == itemId)
				{
					return componentInParent.ItemOwner;
				}
			}
			LootableContainer componentInParent2 = collider.gameObject.GetComponentInParent<LootableContainer>();
			bool flag = componentInParent2 != null && componentInParent2.DoorState == EDoorState.Locked;
			if (componentInParent2 != null && componentInParent2.enabled && !flag)
			{
				if (componentInParent2.ItemOwner == null)
				{
					Debug.LogErrorFormat(componentInParent2, _ED3E._E000(110958), componentInParent2);
				}
				else if (componentInParent2.ItemOwner.RootItem.Id == itemId)
				{
					return componentInParent2.ItemOwner;
				}
			}
		}
		Debug.LogError(_ED3E._E000(110909) + itemId + _ED3E._E000(110881) + position);
		return null;
	}

	[CanBeNull]
	public static LootItem DevelopFindLootItem(string itemId)
	{
		LootItem[] array = _E3AA.FindUnityObjectsOfType<LootItem>();
		foreach (LootItem lootItem in array)
		{
			if (lootItem.enabled && lootItem.ItemOwner.RootItem.Id == itemId)
			{
				return lootItem;
			}
		}
		Debug.LogError(_ED3E._E000(110978) + itemId);
		return null;
	}

	public WorldInteractiveObject FindDoor(string doorId)
	{
		return _E01E.FindDoor(doorId);
	}

	public StationaryWeapon FindStationaryWeapon(string id)
	{
		return LocationScene.GetAllObjects<StationaryWeapon>().FirstOrDefault((StationaryWeapon x) => x.Id == id);
	}

	public StationaryWeapon FindStationaryWeaponByItemId(string itemId)
	{
		return LocationScene.GetAllObjects<StationaryWeapon>().FirstOrDefault((StationaryWeapon x) => x.Item.Id == itemId);
	}

	public virtual void RegisterPlayer(Player player)
	{
		RegisteredPlayers.Add(player);
		if (player.IsYourPlayer)
		{
			MainPlayer = player;
		}
		Transform transform = (_E2B6.Config.UseSpiritPlayer ? player.Spirit.GetActiveTransform() : player.gameObject.transform);
		ItemOwners.Add(player._E0DE, new _E000
		{
			Transform = transform
		});
		Collider collider = player.CharacterControllerCommon.GetCollider();
		PlayersColliders[collider] = player;
		if (!AllPlayers.Contains(player))
		{
			AllPlayers.Add(player);
			this.m__E005?.Invoke(player);
		}
		if (!AllPlayersObjects.Contains(player))
		{
			AllPlayersObjects.Add(player);
		}
		if (_E2B6.Config.UseSpiritPlayer)
		{
			Collider collider2 = player.Spirit.CharacterController.GetCollider();
			PlayersColliders[collider2] = player;
			Collider collider3 = player.Spirit.PlayerSpiritAura.GetCollider();
			PlayersColliders[collider3] = player;
		}
		player.SetDeltaTimeDelegate(() => DeltaTime);
	}

	public virtual void UnregisterPlayer(Player player)
	{
		if (RegisteredPlayers.Contains(player))
		{
			RegisteredPlayers.Remove(player);
			ItemOwners.Remove(player._E0DE);
			Collider collider = player.CharacterControllerCommon.GetCollider();
			PlayersColliders.Remove(collider);
			if (AllPlayers.Contains(player))
			{
				AllPlayers.Remove(player);
			}
			if (_E2B6.Config.UseSpiritPlayer)
			{
				Collider collider2 = player.Spirit.CharacterController.GetCollider();
				PlayersColliders.Remove(collider2);
				Collider collider3 = player.Spirit.PlayerSpiritAura.GetCollider();
				PlayersColliders.Remove(collider3);
			}
			player.SetDeltaTimeDelegate(null);
		}
	}

	public virtual void RemovePlayerObject(Player player)
	{
		if (AllPlayersObjects.Contains(player))
		{
			AllPlayersObjects.Remove(player);
		}
	}

	public virtual void RegisterPlayerCollider(Player player, Collider playerCollider)
	{
		PlayersColliders[playerCollider] = player;
	}

	public virtual void UnregisterPlayerCollider(Collider playerCollider)
	{
		PlayersColliders.Remove(playerCollider);
	}

	internal async Task _E00E(Player player)
	{
		List<Task<byte[]>> list = new List<Task<byte[]>>(this.m__E00D.Count);
		foreach (_E545 item3 in this.m__E00D)
		{
			IEnumerable<Item> questItems = player.Profile.Inventory.QuestItems;
			if (questItems == null)
			{
				Debug.LogError(_ED3E._E000(113745) + player.Profile.Nickname + _ED3E._E000(113777));
				return;
			}
			item3.ValidProfiles = new string[20];
			int num = 0;
			if (questItems.Select((Item x) => x.TemplateId).Contains(item3.Item.TemplateId))
			{
				continue;
			}
			foreach (_E933 item4 in player._E0DC.Quests.Where((_E933 quest) => quest.QuestStatus == EQuestStatus.Started))
			{
				try
				{
					foreach (ConditionFindItem condition in item4.GetConditions<ConditionFindItem>(EQuestStatus.AvailableForFinish))
					{
						if (condition.target.Contains(item3.Item.TemplateId) && !item4.ConditionHandlers[condition].Test() && !item4.CompletedConditions.Contains(condition.id))
						{
							item3.ValidProfiles[num] = player.ProfileId;
							num++;
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(_ED3E._E000(113773) + item4.Id + _ED3E._E000(113797) + ex);
				}
			}
			if (num == 0)
			{
				item3.ValidProfiles = null;
			}
			LootItem lootItem = _E00C(item3, initial: true, player);
			if (lootItem == null || _E01E == null)
			{
				continue;
			}
			_E545 jsonLootItem = SerializeLootItem(lootItem);
			Task<byte[]> item = AsyncWorker.RunOnBackgroundThread(delegate
			{
				using MemoryStream memoryStream = new MemoryStream();
				using BinaryWriter writer = new BinaryWriter(memoryStream);
				writer.WritePolymorph(_E672.SerializeJsonLootItem(jsonLootItem));
				byte[] array2 = memoryStream.ToArray();
				return SimpleZlib.CompressToBytes(array2, array2.Length, 9);
			});
			list.Add(item);
		}
		if (list.Count > 0)
		{
			byte[][] array = await Task.WhenAll(list);
			foreach (byte[] item2 in array)
			{
				_E01E.AddSpawnQuestLootPacket(item2);
			}
		}
	}

	[CanBeNull]
	public Player GetPlayerByCollider(Collider col)
	{
		PlayersColliders.TryGetValue(col, out var value);
		return value;
	}

	public _ECD9<ItemAddress> ToItemAddress(_E673 descriptor)
	{
		IItemOwner itemOwner;
		if (descriptor is _E678)
		{
			itemOwner = FindOwnerById(descriptor.Container.ParentId);
			if (itemOwner == null)
			{
				return new _E001(descriptor.Container.ParentId);
			}
		}
		else
		{
			_ECD9<Item> obj = FindItemById(descriptor.Container.ParentId);
			if (obj.Failed)
			{
				return obj.Error;
			}
			itemOwner = obj.Value.Parent.GetOwnerOrNull();
			if (itemOwner == null)
			{
				return new _E009(obj.Value);
			}
		}
		ItemAddress itemAddress = itemOwner.ToItemAddress(descriptor);
		if (itemAddress == null)
		{
			return new _E002(descriptor.Container.ContainerId);
		}
		return itemAddress;
	}

	[CanBeNull]
	public IItemOwner FindOwnerById(string ownerId)
	{
		return FindOwnerWithWorldData(ownerId).Key;
	}

	public KeyValuePair<IItemOwner, _E000> FindOwnerWithWorldData(string ownerId)
	{
		return ItemOwners.FirstOrDefault((KeyValuePair<IItemOwner, _E000> x) => x.Key.ID == ownerId);
	}

	public _ECD9<Item> FindItemById(string itemId)
	{
		return from e in FindItemWithWorldData(itemId)
			select e.item;
	}

	public _ECD9<(Item item, _E000 data)> FindItemWithWorldData(string itemId)
	{
		using (_ECC9.BeginSampleWithToken(_ED3E._E000(111069), _ED3E._E000(111043)))
		{
			if (Singleton<_E63E>.Instance.TryGetById(itemId, out var item))
			{
				IItemOwner owner = item.Owner;
				if (owner != null && ItemOwners.TryGetValue(owner, out var value) && _E00F(itemId, owner, out var item2))
				{
					_E000 item3 = value;
					item3.ItemOwner = owner;
					return (item2, item3);
				}
			}
		}
		using (_ECC9.BeginSampleWithToken(_ED3E._E000(111081), _ED3E._E000(111043)))
		{
			foreach (var (itemOwner2, obj3) in ItemOwners)
			{
				if (_E00F(itemId, itemOwner2, out var item4))
				{
					_E000 item5 = obj3;
					item5.ItemOwner = itemOwner2;
					return (item4, item5);
				}
			}
		}
		return new _E003(itemId);
	}

	private bool _E00F(string itemId, IItemOwner inventoryController, out Item item)
	{
		try
		{
			item = inventoryController?.FindItem(itemId);
			return item != null;
		}
		catch (Exception)
		{
			item = null;
			return false;
		}
	}

	[CanBeNull]
	public _EB1E FindControllerById(string id)
	{
		foreach (IItemOwner key in ItemOwners.Keys)
		{
			if (key is _EB1E obj && obj.ID == id)
			{
				return obj;
			}
		}
		return null;
	}

	public int CulledPlayersCount()
	{
		int num = 0;
		foreach (Player registeredPlayer in RegisteredPlayers)
		{
			ObservedPlayer observedPlayer = registeredPlayer as ObservedPlayer;
			if (observedPlayer != null)
			{
				num += (observedPlayer.IsVisible ? 1 : 0);
			}
		}
		return num;
	}

	public int TotalPlayersCountToCull()
	{
		int num = 0;
		foreach (Player registeredPlayer in RegisteredPlayers)
		{
			if (registeredPlayer as ObservedPlayer != null)
			{
				num++;
			}
		}
		return num;
	}

	public void OnSmokeGrenadesDeserialized(List<_E335> netGrenadeData)
	{
		foreach (_E335 netGrenadeDatum in netGrenadeData)
		{
			SmokeGrenade grenade = this.m__E015.CreateStillSmokeGrenade(netGrenadeDatum.Id, netGrenadeDatum.Template, netGrenadeDatum.Position, netGrenadeDatum.Orientation, netGrenadeDatum.Time, (netGrenadeDatum.PlatformId > -1) ? Platforms[netGrenadeDatum.PlatformId] : null);
			RegisterGrenade(grenade);
		}
	}

	private void _E010(WorldInteractiveObject door, EDoorState stateBefore, EDoorState stateAfter)
	{
		if (stateAfter == EDoorState.Interacting || stateAfter == EDoorState.Breaching)
		{
			_E01D.Add(door);
		}
		else
		{
			_E01D.Remove(door);
		}
	}

	public int CanPlayerSeepThrough(Collider collider)
	{
		if (collider.gameObject.layer == _E37B.PlayerLayer)
		{
			foreach (Player registeredPlayer in RegisteredPlayers)
			{
				if (registeredPlayer.CharacterControllerCommon.GetCollider() == collider)
				{
					return 0;
				}
			}
		}
		else if (collider.gameObject.layer == _E37B.DoorLayer)
		{
			for (int i = 0; i < _E01D.Count; i++)
			{
				if (_E01D[i].Collider == collider)
				{
					return 1;
				}
			}
		}
		return -1;
	}

	public void RegisterBorderZones()
	{
		BorderZones = LocationScene.GetAllObjects<BorderZone>().ToArray();
		for (int i = 0; i < BorderZones.Length; i++)
		{
			BorderZones[i].Id = i;
		}
		_E01E.SubscribeToBorderZones(BorderZones);
	}

	public void RegisterRestrictableZones()
	{
	}

	public virtual void OnGameStarted()
	{
		this.m__E004?.Invoke();
	}

	[CompilerGenerated]
	private float _E011()
	{
		return DeltaTime;
	}
}
