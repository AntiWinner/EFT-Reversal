using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using Diz.Jobs;
using Diz.Utils;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using Interpolation;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

namespace EFT;

public sealed class ObservedPlayer : NetworkPlayer
{
	public new delegate _E001 _E000(ObservedPlayer player);

	public new interface _E001
	{
		ulong BytesReceived { get; }

		event _E002._E000 ReceiveEvent;

		void Receive(_E524 reader, _E3D5 framesInfoLoopedQueue);
	}

	public new class _E002 : _E001
	{
		public delegate void _E000(ObservedPlayer player, int bytes, ulong totalBytes);

		private readonly ObservedPlayer _E001;

		private ulong m__E002;

		private _E733 _E003;

		[CompilerGenerated]
		private _E000 _E004;

		ulong _E001.BytesReceived => m__E002;

		private event _E000 _E000
		{
			[CompilerGenerated]
			add
			{
				_E000 obj = _E004;
				_E000 obj2;
				do
				{
					obj2 = obj;
					_E000 value2 = (_E000)Delegate.Combine(obj2, value);
					obj = Interlocked.CompareExchange(ref _E004, value2, obj2);
				}
				while ((object)obj != obj2);
			}
			[CompilerGenerated]
			remove
			{
				_E000 obj = _E004;
				_E000 obj2;
				do
				{
					obj2 = obj;
					_E000 value2 = (_E000)Delegate.Remove(obj2, value);
					obj = Interlocked.CompareExchange(ref _E004, value2, obj2);
				}
				while ((object)obj != obj2);
			}
		}

		event _E000 _E001.ReceiveEvent
		{
			add
			{
				this._E000 += value;
			}
			remove
			{
				this._E000 -= value;
			}
		}

		public _E002(ObservedPlayer player)
		{
			_E001 = player;
		}

		void _E001.Receive(_E524 reader, _E3D5 destinationFramesInfoQueue)
		{
			int bytesRead = reader.BytesRead;
			if (reader.ReadBool())
			{
				ulong num = (ulong)reader.ReadLimitedInt32(1, 5);
				_E733 obj = default(_E733);
				obj.FrameIndex = _E003.FrameIndex + num;
				_E733 current = obj;
				_E733.DeserializeDiffUsing(reader, ref current, _E003);
				if (_E003.FrameIndex != 0L)
				{
					destinationFramesInfoQueue.Add(current.ClientTime, current);
				}
			}
			else
			{
				ulong frameIndex = (ulong)reader.ReadLimitedInt32(0, 2097151);
				_E733 obj = default(_E733);
				obj.FrameIndex = frameIndex;
				_E733 current2 = obj;
				_E733.DeserializeDiffUsing(reader, ref current2, _E733.INITIAL_FRAME_INFO);
				if (m__E002 == 0L)
				{
					_E733 iNITIAL_FRAME_INFO = _E733.INITIAL_FRAME_INFO;
					iNITIAL_FRAME_INFO.SetDeathPacket(_E94B.ALIVE_PACKET);
					destinationFramesInfoQueue.Add(current2.ClientTime - 0.001f, iNITIAL_FRAME_INFO);
				}
				destinationFramesInfoQueue.Add(current2.ClientTime, current2);
				if (_E003.FrameIndex > current2.FrameIndex)
				{
					Debug.LogWarningFormat(_ED3E._E000(195344), _E003.FrameIndex, current2.FrameIndex, _E001.ProfileId);
				}
				_E003 = current2;
			}
			m__E002 += (ulong)bytesRead;
			_E004?.Invoke(_E001, bytesRead, m__E002);
		}
	}

	internal new sealed class _E003 : PlayerInventoryController
	{
		[CompilerGenerated]
		private new sealed class _E000
		{
			public _EA91 item;

			internal bool _E000(_EB94 x)
			{
				return x._E016 == item;
			}
		}

		protected override bool HasDiscardLimits => false;

		internal _E003(ObservedPlayer player, Profile profile, MongoID firstId)
			: base(player, profile, examined: true)
		{
			_E027 = firstId;
		}

		public override void StrictCheckMagazine(_EA6A magazine, bool status, int skill = 0, bool notify = false, bool useOperation = true)
		{
		}

		public override void OnAmmoLoadedCall(int count)
		{
		}

		public override void OnAmmoUnloadedCall(int count)
		{
		}

		public override void OnMagazineCheckCall()
		{
		}

		public override bool IsInventoryBlocked()
		{
			return false;
		}

		public override void StartSearchingAction(_EA91 item)
		{
			_EB94 newItem = new _EB94(0, this, item);
			SearchOperations.Add(newItem);
			item.SearchOperations.Add(newItem);
			base.StartSearchingAction(item);
		}

		public override void StopSearchingAction(_EA91 item)
		{
			_EB94 obj = SearchOperations.FirstOrDefault((_EB94 x) => x._E016 == item);
			if (obj == null)
			{
				Logger.LogError(_ED3E._E000(195423) + item);
				return;
			}
			SearchOperations.Remove(obj);
			item.SearchOperations.Remove(obj);
			base.StopSearchingAction(item);
		}

		public override bool CheckOverLimit(IEnumerable<Item> items, ItemAddress to, bool useItemCountInEquipment, out _EB29._E00D error)
		{
			error = null;
			return true;
		}

		public override bool IsLimitedAtAddress(string templateId, ItemAddress address, out int limit)
		{
			limit = -1;
			return false;
		}
	}

	public new interface _E004
	{
		void ProcessPlayerPacket(_E733 framePlayerInfo);

		bool IsInIdleState();
	}

	public new delegate Task _E005(int playerId);

	[CompilerGenerated]
	private new sealed class _E006
	{
		public ObservedPlayer _003C_003E4__this;

		public Weapon weapon;

		internal FirearmController _E000()
		{
			return ObservedFirearmController._E000(_003C_003E4__this, weapon);
		}
	}

	[CompilerGenerated]
	private new sealed class _E007
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF throwWeap;

		internal GrenadeController _E000()
		{
			return ObservedGrenadeController._E000(_003C_003E4__this, throwWeap);
		}
	}

	[CompilerGenerated]
	private new sealed class _E008
	{
		public ObservedPlayer _003C_003E4__this;

		public _EA72 meds;

		public EBodyPart bodyPart;

		public int animationVariant;

		internal MedsController _E000()
		{
			return ObservedMedsController._E000(_003C_003E4__this, meds, bodyPart, 1f, animationVariant);
		}
	}

	[CompilerGenerated]
	private new sealed class _E009
	{
		public ObservedPlayer _003C_003E4__this;

		public _EA48 foodDrink;

		public float amount;

		public int animationVariant;

		internal MedsController _E000()
		{
			return ObservedMedsController._E000(_003C_003E4__this, foodDrink, EBodyPart.Head, amount, animationVariant);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00A
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal KnifeController _E000()
		{
			return ObservedKnifeController._E000(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00B<_E077> where _E077 : UsableItemController
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal _E077 _E000()
		{
			return UsableItemController._E000<_E077>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00C
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF throwWeap;

		internal QuickGrenadeThrowController _E000()
		{
			return ObservedQuickGrenadeThrowController._E000(_003C_003E4__this, throwWeap);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00D
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal QuickKnifeKickController _E000()
		{
			return ObservedQuickKnifeKickController._E000(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private new sealed class _E00E
	{
		public Player._E012 setInHandsOperation;

		internal void _E000()
		{
			setInHandsOperation?.Dispose();
		}
	}

	[CompilerGenerated]
	private new sealed class _E00F<_E077> where _E077 : AbstractHandsController
	{
		public Player._E012 setInHandsOperation;

		internal void _E000()
		{
			setInHandsOperation?.Dispose();
		}
	}

	[CompilerGenerated]
	private new sealed class _E011
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal AbstractHandsController _E000()
		{
			return ObservedFirearmController._E000(_003C_003E4__this, (Weapon)item);
		}
	}

	[CompilerGenerated]
	private new sealed class _E012
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal Task<ObservedFirearmController> _E000()
		{
			return ObservedFirearmController._E001(_003C_003E4__this, (Weapon)item);
		}
	}

	[CompilerGenerated]
	private new sealed class _E014
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF grenade;

		internal AbstractHandsController _E000()
		{
			return ObservedGrenadeController._E000(_003C_003E4__this, grenade);
		}
	}

	[CompilerGenerated]
	private new sealed class _E015
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF grenade;

		internal Task<ObservedGrenadeController> _E000()
		{
			return ObservedGrenadeController._E001(_003C_003E4__this, grenade);
		}
	}

	[CompilerGenerated]
	private sealed class _E017
	{
		public ObservedPlayer _003C_003E4__this;

		public _ECD9<Item> item;

		public EBodyPart bodyPart;

		public float amount;

		public int animationVariant;

		internal AbstractHandsController _E000()
		{
			return ObservedMedsController._E000(_003C_003E4__this, item.Value, bodyPart, amount, animationVariant);
		}
	}

	[CompilerGenerated]
	private sealed class _E018
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		public EBodyPart bodyPart;

		public float amount;

		public int animationVariant;

		internal Task<ObservedMedsController> _E000()
		{
			return ObservedMedsController._E001(_003C_003E4__this, item, bodyPart, amount, animationVariant);
		}
	}

	[CompilerGenerated]
	private sealed class _E01A
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal AbstractHandsController _E000()
		{
			return ObservedKnifeController._E000(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E01B
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal Task<ObservedKnifeController> _E000()
		{
			return ObservedKnifeController._E001(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E01D
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal AbstractHandsController _E000()
		{
			return UsableItemController._E000<ObservedPortableRangeFinderController>(_003C_003E4__this, item);
		}

		internal AbstractHandsController _E001()
		{
			return UsableItemController._E000<ObservedRadioTransmitterController>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E01E
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal Task<ObservedPortableRangeFinderController> _E000()
		{
			return UsableItemController._E001<ObservedPortableRangeFinderController>(_003C_003E4__this, item);
		}

		internal Task<ObservedRadioTransmitterController> _E001()
		{
			return UsableItemController._E001<ObservedRadioTransmitterController>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E020
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF grenade;

		internal AbstractHandsController _E000()
		{
			return ObservedQuickGrenadeThrowController._E000(_003C_003E4__this, grenade);
		}
	}

	[CompilerGenerated]
	private sealed class _E021
	{
		public ObservedPlayer _003C_003E4__this;

		public _EADF grenade;

		internal Task<ObservedQuickGrenadeThrowController> _E000()
		{
			return ObservedQuickGrenadeThrowController._E001(_003C_003E4__this, grenade);
		}
	}

	[CompilerGenerated]
	private sealed class _E023
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal AbstractHandsController _E000()
		{
			return ObservedQuickKnifeKickController._E000(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E024
	{
		public ObservedPlayer _003C_003E4__this;

		public KnifeComponent knife;

		internal Task<ObservedQuickKnifeKickController> _E000()
		{
			return ObservedQuickKnifeKickController._E001(_003C_003E4__this, knife);
		}
	}

	[CompilerGenerated]
	private sealed class _E026
	{
		public ObservedPlayer _003C_003E4__this;

		public Item item;

		internal AbstractHandsController _E000()
		{
			return QuickUseItemController._E000<QuickUseItemController>(_003C_003E4__this, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E027
	{
		public ObservedPlayer _003C_003E4__this;

		public Callback callback;

		internal void _E000(Result<_E6C9> result)
		{
			if ((object)_003C_003E4__this._removeFromHandsCallback == callback)
			{
				_003C_003E4__this._removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}
	}

	[CompilerGenerated]
	private sealed class _E028
	{
		public _ECD5 operation;

		public ObservedPlayer _003C_003E4__this;

		internal void _E000(IResult executeResult)
		{
			if (!executeResult.Succeed)
			{
				_003C_003E4__this._E0DE.Logger.LogError(_ED3E._E000(189653), Time.frameCount, _003C_003E4__this.ProfileId, operation.Value.Id, operation.Value, executeResult.Error);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E02B
	{
		public byte[] bytes;

		public byte[] profileZip;

		public byte[] inventoryZip;

		internal Profile _E000()
		{
			_E5D6 obj = _E5D5.Deserialize(new _E518(bytes));
			Profile profile = SimpleZlib.Decompress(profileZip).ParseJsonTo<Profile>(Array.Empty<JsonConverter>());
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(inventoryZip)))
			{
				using BinaryReader reader = new BinaryReader(input);
				profile.Inventory = _E672.DeserializeInventory(Singleton<_E63B>.Instance, reader.ReadEFTInventoryDescriptor());
			}
			_E5D5.FillSearchInfo(items: profile.Inventory.NonQuestItems.OfType<_EA91>().ToArray(), searchableItemInfos: obj.Data);
			return profile;
		}
	}

	private const int _E10B = 2000;

	private byte _E0E6;

	private _E7C6 _E10C;

	private _E001 _E10D;

	private _E3D5 _E10E;

	private ObservedPlayerInterpolationSettings.RttDTMultiplier[] _E10F;

	private static bool _E110;

	[CompilerGenerated]
	private int _E111;

	private float _E112;

	public bool NeverInited = true;

	private bool _E113;

	private bool _E114;

	private _E951 _E115;

	private Renderer[] _E116;

	private Collider[] _E117;

	private _E33A _E118;

	private _E6C3 _E0ED;

	private _E4E2 _E119 = _E4DF.NOT_VALID_TIME_BOUND;

	private _E4E2 _E11A = _E4DF.NOT_VALID_TIME_BOUND;

	[CompilerGenerated]
	private ulong _E11B;

	private Player _E11C;

	private float _E11D;

	private const float _E11E = 5f;

	private bool _E11F;

	private _E94B _E120;

	private bool _E121;

	private readonly float _E122 = 0.65f;

	private readonly float _E123 = 1.538f;

	private readonly float _E124 = 0.25f;

	private readonly float _E125 = 0.35f;

	private readonly float _E126 = 0.8f;

	private readonly float _E127 = 1.2f;

	private readonly float _E128 = 0.03f;

	private readonly float _E129 = 0.15f;

	private readonly float _E12A = 0.5f;

	private readonly float _E12B = 1f;

	private const float _E12C = 0.125f;

	private readonly _E35F _E12D = new _E35F(30);

	private readonly _E35F _E12E = new _E35F(5);

	private readonly _E360 _E12F = new _E360(0.1f, 0.001f, 0.125f);

	private readonly _E360 _E130 = new _E360(1f, 0.01f, 0.125f);

	private float _E131 = 1f;

	private int? _E132;

	private float _E133;

	private _E4D5<_E4D7<_E733>> _E134 = new _E4D5<_E4D7<_E733>>(64, 256);

	[CompilerGenerated]
	private bool _E135;

	private float _E136;

	private FollowerCullingObject _E137;

	private List<DisablerCullingObject> _E138 = new List<DisablerCullingObject>();

	private bool _E139;

	protected override float LandingThreshold => 0.14f;

	protected override float MINStepSoundSpeedFactor
	{
		get
		{
			if (base.Pose != EPlayerPose.Stand)
			{
				return 0f;
			}
			return 0.1f;
		}
	}

	public override byte ChannelIndex => _E0E6;

	public override bool CanBeSnapped => false;

	private new int _E000
	{
		[CompilerGenerated]
		get
		{
			return _E111;
		}
		[CompilerGenerated]
		set
		{
			_E111 = value;
		}
	}

	public ulong LastServerFrameId
	{
		[CompilerGenerated]
		get
		{
			return _E11B;
		}
		[CompilerGenerated]
		private set
		{
			_E11B = value;
		}
	}

	public float LastServerTimeStamp => _E119.TimeStamp + _E10C.ExtrapolationDeltaTime;

	internal new bool _E001
	{
		[CompilerGenerated]
		get
		{
			return _E135;
		}
		[CompilerGenerated]
		private set
		{
			_E135 = value;
		}
	}

	public override EPointOfView PointOfView
	{
		get
		{
			if (_playerBody == null || _playerBody.PointOfView == null)
			{
				Debug.LogError(((_playerBody == null) ? _ED3E._E000(196260) : _ED3E._E000(196284)) + _ED3E._E000(196312));
			}
			return EPointOfView.ThirdPerson;
		}
	}

	public override AbstractHandsController HandsController
	{
		get
		{
			return base.HandsController;
		}
		protected set
		{
			base.HandsController = value;
			_E712.EWeaponAnimationType weaponAnimationType = GetWeaponAnimationType(_handsController);
			base.MovementContext.PlayerAnimatorSetWeaponId(weaponAnimationType);
		}
	}

	protected override Ray InteractionRay
	{
		get
		{
			Vector3 direction = base.HandsRotation * Vector3.forward;
			return new Ray(_playerLookRaycastTransform.position, direction);
		}
	}

	public _E6C3 DeferredData
	{
		get
		{
			return _E0ED;
		}
		set
		{
			_E0ED = value;
		}
	}

	private new bool _E002
	{
		get
		{
			if (_E138.Count > 0)
			{
				for (int i = 0; i < _E138.Count; i++)
				{
					if (_E138[i].HasEntered)
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
	}

	public override bool IsVisible
	{
		get
		{
			if (_E137 == null || !_E137.IsVisible)
			{
				return false;
			}
			return this._E002;
		}
	}

	public override float SqrCameraDistance
	{
		get
		{
			if (_E137 != null)
			{
				return _E137.SqrCameraDistance;
			}
			return base.SqrCameraDistance;
		}
	}

	public override float ProtagonistHearing => Mathf.Max(1f, Singleton<BetterAudio>.Instance.ProtagonistHearing + 1f);

	private new _E4E2 _E003
	{
		get
		{
			return _E119;
		}
		set
		{
			_E119 = value;
		}
	}

	public event _E002._E000 OnPacketReceived
	{
		add
		{
			_E10D.ReceiveEvent += value;
		}
		remove
		{
			_E10D.ReceiveEvent -= value;
		}
	}

	private void _E000(_E743 viewPacket)
	{
		UpdateSurfaceData(viewPacket.HasSurface, viewPacket.IsSurfaceTerrain, viewPacket.SurfaceSound);
	}

	protected override void PlayGroundedSound(float fallHeight, float jumpHeight)
	{
		var (flag, hitOnTerrain, eSurfaceSound) = CalculateMovementSurface();
		if (flag)
		{
			UpdateSurfaceData(flag, hitOnTerrain, eSurfaceSound ?? BaseBallistic.ESurfaceSound.Concrete);
		}
		base.PlayGroundedSound(fallHeight, jumpHeight);
	}

	protected override bool CheckSurface(float range, float delayToNextCheck = 1f)
	{
		return !OutOfProtagonistsRange(range);
	}

	protected override void UpdateVoipOcclusion(AudioMixerGroup group)
	{
		base.UpdateVoipOcclusion(group);
		if (!(base.VoipAudioSource == null))
		{
			base.VoipAudioSource.SyncOcclusionParameters(base.SpeechSource.LowPassFilterFrequency, base.SpeechSource.source1.volume, base.SpeechSource.OcclusionVolumeFactor);
		}
	}

	protected override void Proceed(bool withNetwork, Callback<_E6C9> callback, bool scheduled = true)
	{
		Func<EmptyHandsController> controllerFactory = () => ObservedEmptyHandsController._E000(this);
		new Process<EmptyHandsController, _E6C9>(this, controllerFactory, null)._E000(null, callback, scheduled);
	}

	protected override void Proceed(Weapon weapon, Callback<_E6CB> callback, bool scheduled = true)
	{
		Func<FirearmController> controllerFactory = () => ObservedFirearmController._E000(this, weapon);
		new Process<FirearmController, _E6CB>(this, controllerFactory, weapon)._E000(null, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6CC> callback, bool scheduled = true)
	{
		Func<GrenadeController> controllerFactory = () => ObservedGrenadeController._E000(this, throwWeap);
		new Process<GrenadeController, _E6CC>(this, controllerFactory, throwWeap)._E000(null, callback, scheduled);
	}

	protected override void Proceed(_EA72 meds, EBodyPart bodyPart, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ObservedMedsController._E000(this, meds, bodyPart, 1f, animationVariant);
		new Process<MedsController, _E6CF>(this, controllerFactory, meds)._E000(null, callback, scheduled);
	}

	protected override void Proceed(_EA48 foodDrink, float amount, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ObservedMedsController._E000(this, foodDrink, EBodyPart.Head, amount, animationVariant);
		new Process<MedsController, _E6CF>(this, controllerFactory, foodDrink)._E000(null, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6CD> callback, bool scheduled = true)
	{
		Func<KnifeController> controllerFactory = () => ObservedKnifeController._E000(this, knife);
		new Process<KnifeController, _E6CD>(this, controllerFactory, knife.Item)._E000(null, callback, scheduled);
	}

	internal override void Proceed<T>(Item item, Callback<_E6CE> callback, bool scheduled = true)
	{
		Func<T> controllerFactory = () => UsableItemController._E000<T>(this, item);
		new Process<T, _E6CE>(this, controllerFactory, item)._E000(null, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6D2> callback, bool scheduled = true)
	{
		Func<QuickGrenadeThrowController> controllerFactory = () => ObservedQuickGrenadeThrowController._E000(this, throwWeap);
		new Process<QuickGrenadeThrowController, _E6D2>(this, controllerFactory, throwWeap, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false)._E000(null, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6D3> callback, bool scheduled = true)
	{
		Func<QuickKnifeKickController> controllerFactory = () => ObservedQuickKnifeKickController._E000(this, knife);
		new Process<QuickKnifeKickController, _E6D3>(this, controllerFactory, knife.Item, fastHide: true, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false)._E000(null, callback, scheduled);
	}

	private void _E001(Func<AbstractHandsController> controllerFactory, Item item)
	{
		Player._E012 setInHandsOperation = ((item != null) ? _E03A(item) : null);
		if (setInHandsOperation != null)
		{
			setInHandsOperation.Confirm();
		}
		if (HandsController != null)
		{
			AbstractHandsController handsController = HandsController;
			HandsController.FastForwardCurrentState();
			if (HandsController != handsController && HandsController != null)
			{
				HandsController.FastForwardCurrentState();
			}
			HandsController.Destroy();
			HandsController = null;
		}
		SpawnController(controllerFactory(), delegate
		{
			setInHandsOperation?.Dispose();
		});
	}

	private async Task _E002<_E077>(Func<Task<_E077>> controllerFactory, Item item) where _E077 : AbstractHandsController
	{
		Player._E012 setInHandsOperation = ((item != null) ? _E03A(item) : null);
		setInHandsOperation?.Confirm();
		if (HandsController != null)
		{
			AbstractHandsController handsController = HandsController;
			HandsController.FastForwardCurrentState();
			if (HandsController != handsController && HandsController != null)
			{
				HandsController.FastForwardCurrentState();
			}
			HandsController.Destroy();
			HandsController = null;
		}
		SpawnController(await controllerFactory(), delegate
		{
			setInHandsOperation?.Dispose();
		});
	}

	private void _E003()
	{
		_E001(() => ObservedEmptyHandsController._E000(this), null);
	}

	private Task _E004()
	{
		return _E002(() => ObservedEmptyHandsController._E001(this), null);
	}

	private void _E005(string itemId)
	{
		Item item;
		if (base.MovementContext.StationaryWeapon != null && base.MovementContext.StationaryWeapon.Item.Id == itemId)
		{
			item = base.MovementContext.StationaryWeapon.Item;
		}
		else
		{
			item = _E006(itemId, _ED3E._E000(195593));
		}
		if (item != null)
		{
			_E001(() => ObservedFirearmController._E000(this, (Weapon)item), item);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(195625), itemId, _ED3E._E000(195593));
		}
	}

	private Item _E006(string itemId, [CallerMemberName] string methodName = null)
	{
		Item item = _E0DE.Inventory.Equipment.FindItem(itemId);
		if (item != null)
		{
			return item;
		}
		Debug.LogErrorFormat(_ED3E._E000(195664), itemId, base.ProfileId, methodName);
		_ECD9<Item> obj = FindItemById(itemId);
		if (obj.Failed)
		{
			Debug.LogErrorFormat(_ED3E._E000(195741), itemId, methodName);
		}
		else
		{
			item = obj.Value;
			IEnumerable<ItemAddress> allParentLocations = item.GetAllParentLocations();
			string text = string.Join(_ED3E._E000(30703), allParentLocations.Select((ItemAddress x) => string.Concat(x.Container, _ED3E._E000(18502), x.ContainerName)).ToArray());
			Debug.LogErrorFormat(_ED3E._E000(195755), item, item.TemplateId, text, methodName);
		}
		return item;
	}

	private async Task _E007(string itemId, bool isStationaryWeapon)
	{
		if (string.IsNullOrEmpty(itemId))
		{
			Debug.LogErrorFormat(_ED3E._E000(195478), _ED3E._E000(195459));
		}
		Item item = null;
		if (isStationaryWeapon)
		{
			item = Singleton<GameWorld>.Instance.FindStationaryWeaponByItemId(itemId).Item;
		}
		else
		{
			item = _E006(itemId, _ED3E._E000(195459));
		}
		if (item != null)
		{
			await _E002(() => ObservedFirearmController._E001(this, (Weapon)item), item);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(195625), itemId, _ED3E._E000(195459));
		}
	}

	private void _E008(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(195833));
		_EADF grenade;
		if ((grenade = item as _EADF) != null)
		{
			_E001(() => ObservedGrenadeController._E000(this, grenade), grenade);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(195625), itemId, _ED3E._E000(195833));
		}
	}

	private async Task _E009(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(195544));
		_EADF grenade;
		if ((grenade = item as _EADF) != null)
		{
			await _E002(() => ObservedGrenadeController._E001(this, grenade), grenade);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(195625), itemId, _ED3E._E000(195544));
		}
	}

	private void _E00A(string itemId, EBodyPart bodyPart, float amount, int animationVariant)
	{
		_ECD9<Item> item = FindItemById(itemId);
		if (item.Succeeded)
		{
			_E001(() => ObservedMedsController._E000(this, item.Value, bodyPart, amount, animationVariant), item.Value);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195865), itemId, item.Error, _ED3E._E000(195899));
	}

	private async Task _E00B(string itemId, EBodyPart bodyPart, float amount, int animationVariant)
	{
		Item item = _E006(itemId, _ED3E._E000(195573));
		if (item != null)
		{
			await _E002(() => ObservedMedsController._E001(this, item, bodyPart, amount, animationVariant), item);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(189463), itemId, _ED3E._E000(195573));
		}
	}

	private void _E00C(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(195928));
		KnifeComponent knife = item?.GetItemComponent<KnifeComponent>();
		if (knife != null)
		{
			_E001(() => ObservedKnifeController._E000(this, knife), knife.Item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(195928), item);
	}

	private async Task _E00D(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(189503));
		KnifeComponent knife = item?.GetItemComponent<KnifeComponent>();
		if (knife != null)
		{
			await _E002(() => ObservedKnifeController._E001(this, knife), knife.Item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(195928), item);
	}

	private void _E00E(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(195986));
		if (item is _EA82)
		{
			_E001(() => UsableItemController._E000<ObservedPortableRangeFinderController>(this, item), item);
			return;
		}
		if (item is _EA87)
		{
			_E001(() => UsableItemController._E000<ObservedRadioTransmitterController>(this, item), item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(196013), item);
	}

	private async Task _E00F(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(189530));
		if (item is _EA82)
		{
			await _E002(() => UsableItemController._E001<ObservedPortableRangeFinderController>(this, item), item);
			return;
		}
		if (item is _EA87)
		{
			await _E002(() => UsableItemController._E001<ObservedRadioTransmitterController>(this, item), item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(196013), item);
	}

	private void _E010(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(196050));
		_EADF grenade;
		if ((grenade = item as _EADF) != null)
		{
			_E001(() => ObservedQuickGrenadeThrowController._E000(this, grenade), grenade);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(196068), itemId, _ED3E._E000(196050));
		}
	}

	private async Task _E011(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(189554));
		_EADF grenade;
		if ((grenade = item as _EADF) != null)
		{
			await _E002(() => ObservedQuickGrenadeThrowController._E001(this, grenade), grenade);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(196068), itemId, _ED3E._E000(189554));
		}
	}

	private void _E012(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(196114));
		KnifeComponent knife = item?.GetItemComponent<KnifeComponent>();
		if (knife != null)
		{
			_E001(() => ObservedQuickKnifeKickController._E000(this, knife), knife.Item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(196114), item);
	}

	private async Task _E013(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(189569));
		KnifeComponent knife = item?.GetItemComponent<KnifeComponent>();
		if (knife != null)
		{
			await _E002(() => ObservedQuickKnifeKickController._E001(this, knife), knife.Item);
			return;
		}
		Debug.LogErrorFormat(_ED3E._E000(195966), itemId, _ED3E._E000(189569), item);
	}

	private void _E014(string itemId)
	{
		Item item = _E006(itemId, _ED3E._E000(196137));
		if (item != null)
		{
			_E001(() => QuickUseItemController._E000<QuickUseItemController>(this, item), item);
		}
		else
		{
			Debug.LogErrorFormat(_ED3E._E000(195625), itemId, _ED3E._E000(196137));
		}
	}

	protected override void SetControllerInsteadRemovedOne(Item removingItem, Callback callback)
	{
		_removeFromHandsCallback = callback;
		Proceed(withNetwork: false, delegate(Result<_E6C9> result)
		{
			if ((object)_removeFromHandsCallback == callback)
			{
				_removeFromHandsCallback = null;
			}
			callback.Invoke(result);
		}, scheduled: false);
	}

	private void _E015(byte[] bytes)
	{
		using MemoryStream input = new MemoryStream(bytes);
		using BinaryReader reader = new BinaryReader(input);
		try
		{
			_ECD5 operation = ToInventoryOperation(reader.ReadPolymorph<_E67A>());
			if (operation.Failed)
			{
				_E0DE.Logger.LogError(_ED3E._E000(196166), Time.frameCount, base.ProfileId, operation.Error);
				return;
			}
			if (!_E0DE._E01E(operation.Value))
			{
				HandsController.FastForwardCurrentState();
			}
			operation.Value._E02A(delegate(IResult executeResult)
			{
				if (!executeResult.Succeed)
				{
					_E0DE.Logger.LogError(_ED3E._E000(189653), Time.frameCount, base.ProfileId, operation.Value.Id, operation.Value, executeResult.Error);
				}
			});
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	internal static ObservedPlayer _E016(int playerId, Vector3 position, _E62D frameIndexer, EUpdateQueue updateQueue, bool voipEnabled)
	{
		return _E017(playerId, position, frameIndexer, updateQueue, (ObservedPlayer player) => new _E002(player), voipEnabled);
	}

	internal static ObservedPlayer _E017(int playerId, Vector3 position, _E62D frameIndexer, EUpdateQueue updateQueue, _E000 dataReceiverFactory, bool voipEnabled)
	{
		ObservedPlayer observedPlayer = NetworkPlayer._E000<ObservedPlayer>(_E5D2.PLAYER_BUNDLE_NAME, playerId, position, _ED3E._E000(196234), frameIndexer, updateQueue, EUpdateMode.Auto, _E2B6.Config.UseBodyFastAnimator ? EUpdateMode.Manual : EUpdateMode.Auto, _E2B6.Config.CharacterController.ObservedPlayerMode, () => 1f, () => 1f, isThirdPerson: true);
		observedPlayer.EnabledAnimators = (EAnimatorMask)0;
		if (observedPlayer._triggerColliderSearcher != null)
		{
			observedPlayer._triggerColliderSearcher.OnEnter += observedPlayer._E02E;
			observedPlayer._triggerColliderSearcher.OnExit += observedPlayer._E02F;
		}
		observedPlayer._armsUpdateQueue = EUpdateQueue.Update;
		observedPlayer._E10F = ObservedPlayerInterpolationSettings.Instance.RttDtMultipliers;
		observedPlayer._E10D = dataReceiverFactory(observedPlayer);
		observedPlayer._E019();
		observedPlayer._E001 = voipEnabled;
		observedPlayer.CurrentLeanType = LeanType.SlowLean;
		return observedPlayer;
	}

	public override _E338 CreatePhysical()
	{
		return new _E338
		{
			EncumberDisabled = _E139
		};
	}

	internal void _E018(byte channelIndex)
	{
		_E0E6 = channelIndex;
	}

	internal void _E019()
	{
		_E030();
		if (_E2B6.Config.UseSpiritPlayer)
		{
			Spirit.HideUnusedPart();
		}
	}

	protected override void CreateMovementContext()
	{
		LayerMask mOVEMENT_MASK = EFTHardSettings.Instance.MOVEMENT_MASK;
		_E10C = _E7C6.Create(this, base.GetBodyAnimatorCommon, base.GetCharacterControllerCommon, mOVEMENT_MASK);
		base.MovementContext = _E10C;
	}

	protected override void UpdateSpeedLimitByHealth()
	{
	}

	protected override void UpdatePhones()
	{
	}

	public override void UpdateTick()
	{
		float deltaTime = base.DeltaTime;
		_fixedTime += deltaTime;
		if (_E0ED == null && base.UpdateQueue == EUpdateQueue.Update)
		{
			ManualUpdate(deltaTime, _nFixedFrames);
		}
	}

	protected override void LateUpdate()
	{
		DistanceDirty = true;
		OcclusionDirty = true;
		if (base.UpdateQueue == EUpdateQueue.FixedUpdate && !_manuallyUpdated)
		{
			return;
		}
		_manuallyUpdated = false;
		if (_E0ED == null && base.HealthController != null && base.HealthController.IsAlive)
		{
			Physical.LateUpdate();
			VisualPass();
			PropUpdate();
			_armsupdated = false;
			_bodyupdated = false;
			if (_nFixedFrames > 0)
			{
				_nFixedFrames = 0;
				_fixedTime = 0f;
			}
			float fixedTime = Time.fixedTime;
			if (fixedTime - _E136 > EFTHardSettings.Instance.SoundOcclusionUpdateInterval)
			{
				_E136 = fixedTime;
				OcclusionDirty = true;
				UpdateOcclusion();
			}
		}
	}

	public override void UpdateArmsCondition()
	{
	}

	public override void OnHealthEffectAdded(_E992 effect)
	{
		if (effect is _E9A2 && FractureSound != null && Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.Position, FractureSound, _E8A8.Instance.Distance(base.Position), BetterAudio.AudioSourceGroupType.Impacts, 15, 0.7f, EOcclusionTest.Fast);
		}
	}

	public override void OnHealthEffectRemoved(_E992 effect)
	{
	}

	public override void ExecuteSkill(Action action)
	{
	}

	public override void ExecuteShotSkill(Item weapon)
	{
	}

	public override void Say(EPhraseTrigger @event, bool demand = false, float delay = 0f, ETagStatus mask = (ETagStatus)0, int probability = 100, bool aggressive = false)
	{
		if (NetworkPlayer.LocalPhrases.Contains(@event))
		{
			base.Say(@event, demand, delay, mask, 100, aggressive);
		}
	}

	internal override void _E073(EGesture gesture)
	{
		if (gesture == EGesture.Hello)
		{
			InteractionRaycast();
			if (base.InteractablePlayer != null && (_E11C != base.InteractablePlayer || Time.time > _E11D))
			{
				base.InteractablePlayer.ShowHelloNotification(base.Profile.Nickname);
				_E11C = base.InteractablePlayer;
				_E11D = Time.time + 5f;
			}
		}
		base._E073(gesture);
	}

	public override void ShowHelloNotification(string message)
	{
	}

	public override void MouseLook(bool forceApplyToOriginalRibcage = false)
	{
		if (!_E2B6.Config.UseSpiritPlayer || !Spirit.IsActive || forceApplyToOriginalRibcage)
		{
			base.MovementContext.RotationAction?.Invoke(this);
		}
	}

	protected override void ManageAggressor(_EC23 damageInfo, EBodyPart bodyPart, EHeadSegment? headSegment)
	{
		if (!_isDeadAlready)
		{
			if (!base.HealthController.IsAlive)
			{
				_isDeadAlready = true;
			}
			Player player = damageInfo.Player;
			if ((object)player != this && !(player == null) && !player.Profile.Info.GroupId.EqualsAndNotNull(base.Profile.Info.GroupId) && damageInfo.Weapon != null)
			{
				player.ExecuteShotSkill(damageInfo.Weapon);
			}
		}
	}

	public override void ManualUpdate(float deltaTime, int loop = 1)
	{
		if (_E0ED != null)
		{
			return;
		}
		_bodyupdated = true;
		_bodyTime = deltaTime;
		if (base.HealthController == null || !base.HealthController.IsAlive)
		{
			return;
		}
		if (_E2B6.Config.UseBodyFastAnimator)
		{
			base.BodyAnimatorCommon.Update(deltaTime);
			PlayerBones.PlayableAnimator.Process(IsVisible, deltaTime);
		}
		int valuesCount = _E10E.GetValuesCount();
		bool hasDeathPacket = _E10E.HasDeathPacket;
		_E4E2 start = _E4DF.NOT_VALID_TIME_BOUND;
		_E4E2 end = _E4DF.NOT_VALID_TIME_BOUND;
		if (hasDeathPacket)
		{
			if (!_E11A.IsValid())
			{
				float timeStamp = _E10E.GetTimeStamp(0);
				_E11A = new _E4E2(timeStamp, EBoundType.GreaterOrEqual);
			}
			float timeStamp2 = _E10E.GetTimeStamp(valuesCount - 1);
			end = new _E4E2(timeStamp2, EBoundType.Equals);
			start = new _E4E2(_E11A.TimeStamp, EBoundType.Greater);
		}
		else if (valuesCount > 0)
		{
			if (!_E11A.IsValid())
			{
				float timeStamp3 = _E10E.GetTimeStamp(0);
				_E11A = new _E4E2(timeStamp3, EBoundType.GreaterOrEqual);
			}
			start = new _E4E2(_E11A.TimeStamp, EBoundType.GreaterOrEqual);
			float leftTime = _E10E.GetLeftTime(_E11A.TimeStamp);
			deltaTime = _E01A(deltaTime, leftTime);
			if (deltaTime > leftTime)
			{
				deltaTime = leftTime;
			}
			float timeStamp4 = _E11A.TimeStamp + deltaTime;
			end = new _E4E2(timeStamp4, EBoundType.LessOrEqual);
		}
		else
		{
			_E112 += deltaTime;
		}
		if (valuesCount > 0 || hasDeathPacket)
		{
			if (!_E10E.GetInterpolatorSpan(in start, in end, out var interpolatorSpan))
			{
				return;
			}
			_E01B(interpolatorSpan, deltaTime, hasDeathPacket);
			_E11A = end;
			_E10E.RemoveDataUntil(in _E11A);
		}
		else if (base.HealthController.IsAlive && _E11A.IsValid())
		{
			_E10C.Extrapolate(_E112);
			_E112 = 0f;
		}
		UpdateTriggerColliderSearcher(deltaTime, SqrCameraDistance < 1600f);
	}

	private float _E01A(float deltaTime, float leftTime)
	{
		float result = deltaTime;
		float num = leftTime - deltaTime;
		float value = _E12D.Value;
		float num2 = _E131;
		float num3 = Mathf.Lerp(_E12A, _E12B, 1f - Mathf.InverseLerp(_E128, _E129, value));
		float num4 = value * num3 + _E124;
		if (num > num4)
		{
			return leftTime - num4;
		}
		if (leftTime >= _E125)
		{
			return leftTime - _E125;
		}
		_E12F.Calculate(value * num3 * num2);
		float targetValue = Mathf.Clamp(num / _E12F.Value, _E126, _E127);
		_E130.Calculate(targetValue);
		if (Mathf.Abs(_E130.Value - 1f) > 0.01f)
		{
			result = deltaTime * _E130.Value;
		}
		return result;
	}

	private void _E01B(_E4E0<_E733, _E3D5, _E3D3> interpolationSpan, float deltaTime, bool deathIsClose)
	{
		int num = interpolationSpan.Interpolate(_E134);
		for (int i = 0; i < num; i++)
		{
			_E4D7<_E733> obj = _E134[i];
			float num2 = obj.DeltaTime;
			if (num2 <= 0f)
			{
				num2 = deltaTime;
			}
			_E01C(num2, obj.Frame, deathIsClose);
		}
	}

	private void _E01C(float deltaTime, _E733 framePlayerInfo, bool deathIsClose)
	{
		LastServerFrameId = framePlayerInfo.FrameIndex;
		this._E003 = new _E4E2(framePlayerInfo.Time, EBoundType.Equals);
		bool flag = _E01F(framePlayerInfo);
		if (flag)
		{
			try
			{
				_E025(framePlayerInfo.HandsChangePacket);
			}
			catch (Exception ex)
			{
				_E7A8.TraceError(ETraceCode.ObserverProcessHandsChangePacketException, base.TryGetId, ex);
				Debug.LogException(ex);
			}
		}
		if (framePlayerInfo.HandsTypePacket != 0)
		{
			try
			{
				((HandsController as _E004) ?? throw new InvalidOperationException(_ED3E._E000(196401) + base.Profile.Nickname + _ED3E._E000(18502) + string.Format(_ED3E._E000(196387), framePlayerInfo.HandsTypePacket, HandsController))).ProcessPlayerPacket(framePlayerInfo);
			}
			catch (Exception ex2)
			{
				_E7A8.TraceError(ETraceCode.ObserverProcessPlayerPacketException, base.TryGetId, ex2);
				Debug.LogException(ex2);
			}
		}
		try
		{
			if (_handsController != null)
			{
				_handsController.ManualUpdate(deltaTime);
				if (deathIsClose)
				{
					_handsController.FastForwardCurrentState();
				}
			}
			if ((EnabledAnimators & EAnimatorMask.Arms) != 0 && base.ArmsUpdateMode != 0)
			{
				base.ArmsAnimatorCommon.Update(deltaTime);
			}
			_armsupdated = true;
			_armsTime = deltaTime;
			if (_handsController != null)
			{
				_handsController.EmitEvents();
			}
		}
		catch (Exception ex3)
		{
			_E7A8.TraceError(ETraceCode.ObserverProcessInterpolationSpanException, base.TryGetId, ex3);
			Debug.LogException(ex3);
		}
		if (!flag)
		{
			try
			{
				_E025(framePlayerInfo.HandsChangePacket);
			}
			catch (Exception ex4)
			{
				_E7A8.TraceError(ETraceCode.ObserverProcessHandsChangePacketException, base.TryGetId, ex4);
				Debug.LogException(ex4);
			}
		}
		if (framePlayerInfo.InventoryCommandPackets != null)
		{
			for (int i = 0; i < framePlayerInfo.InventoryCommandPackets.Count; i++)
			{
				_E6F6 obj = framePlayerInfo.InventoryCommandPackets[i];
				try
				{
					switch (obj.Tag)
					{
					case _E6F6.ETag.Command:
						_E015(obj.CommandPacket.CommandBytes);
						break;
					case _E6F6.ETag.Status:
					{
						_E957 statusPacket = obj.StatusPacket;
						_E033(statusPacket.Id, statusPacket.Status, statusPacket.Error, 0, badBeforeExecuting: false);
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception ex5)
				{
					_E7A8.TraceError(ETraceCode.ObserverInventoryCommandPacketException, base.TryGetId, ex5);
					Debug.LogException(ex5);
				}
			}
		}
		if (framePlayerInfo.PhraseCommandPacket.HasPhrase)
		{
			try
			{
				Speaker.PlayDirect(framePlayerInfo.PhraseCommandPacket.PhraseCommand, framePlayerInfo.PhraseCommandPacket.PhraseId);
			}
			catch (Exception ex6)
			{
				_E7A8.TraceError(ETraceCode.ObserverSpeakerPlayDirectException, base.TryGetId, ex6);
				Debug.LogException(ex6);
			}
		}
		if (framePlayerInfo.CommonPacket != null)
		{
			ProcessCommonPacket(framePlayerInfo.CommonPacket);
		}
		_E120 = framePlayerInfo.DeathPacket;
		_E9C7? obj2 = framePlayerInfo.SyncHealthPacket;
		while (obj2.HasValue)
		{
			base.NetworkHealthController.HandleSyncPacket(obj2.Value);
			obj2 = obj2.Value.GetNested();
		}
		if (!framePlayerInfo.DeathPacket.IsAlive && !_E121)
		{
			_E121 = true;
			_E01D(framePlayerInfo.DeathPacket);
		}
		if (framePlayerInfo.DeathInventorySyncPacket.HasValue)
		{
			try
			{
				_E022(framePlayerInfo.DeathInventorySyncPacket.GetValueOrDefault().Bytes);
			}
			catch (Exception ex7)
			{
				_E7A8.TraceError(ETraceCode.ObserverProcessDeathInventorySyncException, base.TryGetId, ex7);
				Debug.LogException(ex7);
			}
		}
		if (framePlayerInfo.ObservedRadioTransmitterStatusPacket.HasValue)
		{
			try
			{
				_E024(framePlayerInfo.ObservedRadioTransmitterStatusPacket);
			}
			catch (Exception ex8)
			{
				_E7A8.TraceError(ETraceCode.ObserverRadioTransmitterStateException, base.TryGetId, ex8);
				Debug.LogException(ex8);
			}
		}
		if (framePlayerInfo.VoipState != 0)
		{
			base.VoipState = framePlayerInfo.VoipState;
		}
		if (_handsController is FirearmController firearmController)
		{
			firearmController.SetWeaponOverlapValue(framePlayerInfo.ViewPacket.WeaponOverlap);
		}
		_E000(framePlayerInfo.ViewPacket);
		try
		{
			_E10C.ProcessFrame(framePlayerInfo, deltaTime);
		}
		catch (Exception ex9)
		{
			_E7A8.TraceError(ETraceCode.ObserverProcessFrameException, base.TryGetId, ex9);
			Debug.LogException(ex9);
		}
		bool isDisconnected = framePlayerInfo.IsDisconnected;
		if (isDisconnected != _E11F)
		{
			_E11F = isDisconnected;
			if (_E11F)
			{
				_E035();
			}
			else
			{
				_E036();
			}
		}
	}

	public override void OperateStationaryWeapon(StationaryWeapon stationaryWeapon, _E6DB.EStationaryCommand command)
	{
		switch (command)
		{
		case _E6DB.EStationaryCommand.Occupy:
			stationaryWeapon.SetOperator(base.TryGetId);
			base.MovementContext.StationaryWeapon = stationaryWeapon;
			base.MovementContext.InteractionParameters = stationaryWeapon.GetInteractionParameters();
			base.MovementContext.PlayerAnimatorSetApproached(b: false);
			base.MovementContext.PlayerAnimatorSetStationary(b: true);
			base.MovementContext.PlayerAnimatorSetStationaryAnimation((int)stationaryWeapon.Animation);
			break;
		case _E6DB.EStationaryCommand.Denied:
			base.MovementContext.PlayerAnimatorSetStationary(b: false);
			if (base.MovementContext.StationaryWeapon != null)
			{
				base.MovementContext.StationaryWeapon.Unlock(base.TryGetId);
			}
			break;
		}
	}

	private void _E01D(_E94B deathPacket)
	{
		try
		{
			SetDogtagInfo(deathPacket);
		}
		catch (Exception ex)
		{
			_E7A8.TraceError(ETraceCode.ObserverSetDogtagInfoException, base.TryGetId, ex);
			Debug.LogException(ex);
		}
		try
		{
			ProcessNetworkDeath(string.Empty, string.Empty, EPlayerSide.Bear, EBodyPart.Chest, string.Empty, EMemberCategory.Default);
		}
		catch (Exception ex2)
		{
			_E7A8.TraceError(ETraceCode.ObserverProcessNetworkDeathException, base.TryGetId, ex2);
			Debug.LogException(ex2);
		}
		_E01E(deathPacket);
	}

	private async void _E01E(_E94B deathPacket)
	{
		Task<int> task = AsyncWorker.RunOnBackgroundThread(() => _E0DE.Inventory.CreateInventoryHashSum(new EquipmentSlot[1] { EquipmentSlot.SecuredContainer }));
		await task;
		try
		{
			_E020(deathPacket, task.Result);
		}
		catch (Exception ex)
		{
			_E7A8.TraceError(ETraceCode.ObserverCheckInvetorySyncException, base.TryGetId, ex);
			Debug.LogException(ex);
		}
	}

	private static bool _E01F(_E733 framePlayerInfo)
	{
		EHandsTypePacket handsTypePacket = framePlayerInfo.HandsTypePacket;
		_E6E5.EOperationType operationType = framePlayerInfo.HandsChangePacket.OperationType;
		if ((handsTypePacket != EHandsTypePacket.Firearm || operationType != _E6E5.EOperationType.CreateFirearm) && (handsTypePacket != EHandsTypePacket.Grenade || (operationType != _E6E5.EOperationType.CreateGrenade && operationType != _E6E5.EOperationType.CreateQuickGrenadeThrow)) && (handsTypePacket != EHandsTypePacket.Knife || (operationType != _E6E5.EOperationType.CreateKnife && operationType != _E6E5.EOperationType.CreateQuickKnifeKick)) && (handsTypePacket != EHandsTypePacket.EmptyHand || operationType != _E6E5.EOperationType.CreateEmptyHands))
		{
			if (handsTypePacket == EHandsTypePacket.UsableItem)
			{
				return operationType == _E6E5.EOperationType.CreateUsableItem;
			}
			return false;
		}
		return true;
	}

	private void _E020(_E94B deathPacket, int localInventoryHashSum)
	{
		_EAE7 inventory = _E0DE.Inventory;
		if (localInventoryHashSum != deathPacket.InventoryHashSum)
		{
			Logger.LogError(_ED3E._E000(196475), base.Profile.Id, base.Profile.Nickname, deathPacket.InventoryHashSum, localInventoryHashSum);
			if (_E114)
			{
				Logger.LogDebug(_ED3E._E000(196485));
				_EB0B equipment = inventory.Equipment;
				{
					foreach (EquipmentSlot value in _E3A5<EquipmentSlot>.Values)
					{
						_E021(_E115, equipment, value);
					}
					return;
				}
			}
			_E113 = true;
		}
		else
		{
			Logger.LogTrace(_ED3E._E000(196519), base.Profile.Id, base.Profile.Nickname);
		}
	}

	private void _E021(_E951 deathPacket, _EB0B equipment, EquipmentSlot equipmentSlot)
	{
		Slot slot = equipment.GetSlot(equipmentSlot);
		Item containedItem = slot.ContainedItem;
		_E950 slotItemInfo = deathPacket.GetSlotItemInfo(equipmentSlot);
		if (containedItem != null)
		{
			if (slotItemInfo.ItemHashSum == 0)
			{
				_EB29.Discard(slot.ContainedItem, _E0DE, simulate: false, ignoreRestrictions: true);
				Logger.LogError(_ED3E._E000(196558), base.Profile.Id, base.Profile.Nickname.Transliterate(_ED3E._E000(36786)), equipmentSlot, containedItem);
				return;
			}
			_E53A[] array = slotItemInfo.ItemJson.ParseJsonTo<_E53A[]>(Array.Empty<JsonConverter>());
			if (array == null)
			{
				Debug.LogErrorFormat(_ED3E._E000(194571), equipmentSlot, slotItemInfo.ItemJson);
				return;
			}
			Item item = Singleton<_E63B>.Instance.FlatItemsToTree(array).Items[containedItem.Id];
			if (containedItem.GetHashSum() != slotItemInfo.ItemHashSum)
			{
				string text = Singleton<_E63B>.Instance.TreeToFlatItems(containedItem).ToPrettyJson();
				Logger.LogError(_ED3E._E000(194642), base.Profile.Id, base.Profile.Nickname.Transliterate(_ED3E._E000(36786)), equipmentSlot, slotItemInfo.ItemJson, text);
				Corpse.SetItemInHandsLootedCallback(null);
				_EB29.Discard(slot.ContainedItem, _E0DE, simulate: false, ignoreRestrictions: true);
				if (Corpse != null && slot.ContainedItem == Corpse.ItemInHands.Value)
				{
					Corpse.ItemInHands.Value = item;
				}
				Corpse.SetItemInHandsLootedCallback(base.ReleaseHand);
				_EB29.AddWithoutRestrictions(item, new _EB20(slot), _E0DE);
			}
		}
		else
		{
			Logger.LogTrace(_ED3E._E000(194663), base.Profile.Id, base.Profile.Nickname, equipmentSlot, slotItemInfo.ItemJson);
		}
	}

	internal void _E022(byte[] bytes)
	{
		Logger.LogDebug(_ED3E._E000(194747), bytes.Length);
		_E976.Serialize(new _E518(bytes), ref _E115);
		_E114 = true;
		if (!_E113)
		{
			return;
		}
		Logger.LogDebug(_ED3E._E000(194773), bytes.Length);
		_EB0B equipment = _E0DE.Inventory.Equipment;
		foreach (EquipmentSlot value in _E3A5<EquipmentSlot>.Values)
		{
			_E021(_E115, equipment, value);
		}
	}

	private void _E023(_E733 framePlayerInfo)
	{
		_E6D9 movementInfo = framePlayerInfo.MovementInfo;
		EPlayerState ePlayerState = base.MovementContext.CurrentState.Name;
		EPlayerState ePlayerState2 = movementInfo.EPlayerState;
		if (ePlayerState == EPlayerState.Jump && ePlayerState2 != EPlayerState.Jump)
		{
			_E10C.ResetJumpToIdle();
		}
		if ((ePlayerState == EPlayerState.ProneIdle || ePlayerState == EPlayerState.ProneMove) && ePlayerState2 != EPlayerState.ProneMove && ePlayerState2 != EPlayerState.Transit2Prone && ePlayerState2 != EPlayerState.ProneIdle)
		{
			_E10C.ResetProneToIdle();
		}
		if ((ePlayerState2 == EPlayerState.ProneIdle || ePlayerState2 == EPlayerState.ProneMove) && ePlayerState != EPlayerState.ProneMove && ePlayerState != EPlayerState.Prone2Stand && ePlayerState != EPlayerState.Transit2Prone && ePlayerState != EPlayerState.ProneIdle)
		{
			_E10C.ResetIdleToProne();
		}
	}

	private void _E024(_E744? packet)
	{
		if (packet.HasValue && base.RecodableItemsHandler.TryToGetRecodableComponent<RadioTransmitterRecodableComponent>(out var component))
		{
			component.SetStatus(packet.Value.Status);
		}
	}

	private void _E025(_E6E5 handsChangePacket)
	{
		if (handsChangePacket.OperationType == _E6E5.EOperationType.None)
		{
			return;
		}
		switch (handsChangePacket.OperationType)
		{
		case _E6E5.EOperationType.CreateEmptyHands:
		case _E6E5.EOperationType.DropAndCreateEmptyHands:
			_E003();
			break;
		case _E6E5.EOperationType.CreateKnife:
		case _E6E5.EOperationType.DropAndCreateKnife:
			_E00C(handsChangePacket.ItemId);
			break;
		case _E6E5.EOperationType.CreateUsableItem:
		case _E6E5.EOperationType.DropAndCreateUsableItem:
			_E00E(handsChangePacket.ItemId);
			break;
		case _E6E5.EOperationType.CreateFirearm:
		case _E6E5.EOperationType.DropAndCreateFirearm:
			if (HandsController == null || HandsController.Item.Id != handsChangePacket.ItemId)
			{
				_E005(handsChangePacket.ItemId);
			}
			break;
		case _E6E5.EOperationType.CreateGrenade:
		case _E6E5.EOperationType.DropAndCreateGrenade:
			_E008(handsChangePacket.ItemId);
			break;
		case _E6E5.EOperationType.CreateMeds:
		case _E6E5.EOperationType.DropAndCreateMeds:
			_E00A(handsChangePacket.ItemId, handsChangePacket.MedsBodyPart, handsChangePacket.MedsAmount, handsChangePacket.AnimationVariant);
			break;
		case _E6E5.EOperationType.CreateQuickGrenadeThrow:
		case _E6E5.EOperationType.DropAndCreateQuickGrenadeThrow:
			_E010(handsChangePacket.ItemId);
			break;
		case _E6E5.EOperationType.CreateQuickKnifeKick:
		case _E6E5.EOperationType.DropAndCreateQuickKnifeKick:
			_E012(handsChangePacket.ItemId);
			break;
		case _E6E5.EOperationType.CreateQuickUseItem:
		case _E6E5.EOperationType.DropAndCreateQuickUseItem:
			_E014(handsChangePacket.ItemId);
			break;
		default:
			throw new NotImplementedException(handsChangePacket.OperationType.ToString());
		case _E6E5.EOperationType.Drop:
		case _E6E5.EOperationType.FastDrop:
			break;
		}
	}

	protected override void OnFovUpdatedEvent(int fov)
	{
	}

	protected override void OnDestroy()
	{
		if (NeverInited)
		{
			Debug.LogWarning(_ED3E._E000(194789));
		}
		if (HandsController != null)
		{
			AbstractHandsController handsController = HandsController;
			if (handsController != null && handsController.ControllerGameObject != null)
			{
				HandsController.OnGameSessionEnd();
				HandsController.Destroy();
			}
		}
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.ProtagonistHearingChanged -= _E034;
		}
		base.OnDestroy();
	}

	internal async Task _E026(NetworkReader reader, _E005 spawnSync, Callback callback)
	{
		try
		{
			await _E027(reader, spawnSync);
		}
		catch (OperationCanceledException)
		{
			Logger.LogError(_ED3E._E000(189706) + base.Profile.Nickname + _ED3E._E000(64014) + base.Profile.AccountId + _ED3E._E000(64014) + base.Profile.Id + _ED3E._E000(189755));
		}
		catch (Exception e)
		{
			_E7A8.TraceError(ETraceCode.ObserverOnDeserializeInitialStateJobException, base.TryGetId, e);
			Logger.LogException(e);
		}
		_E6C3 obj = Interlocked.Exchange(ref _E0ED, null);
		try
		{
			obj._E003(_E02C);
		}
		catch (Exception e2)
		{
			_E7A8.TraceError(ETraceCode.ObserverDeserializeFromServerException, base.TryGetId, e2);
			Logger.LogException(e2);
		}
		if (NeverInited)
		{
			_E02D();
		}
		callback.Succeed();
	}

	private async Task _E027(NetworkReader reader, _E005 spawnSync)
	{
		bool flag = reader.ReadBoolean();
		Vector3 position = reader.ReadVector3();
		Quaternion rotation = reader.ReadQuaternion();
		bool isInPronePose = reader.ReadBoolean();
		float poseLevel = reader.ReadSingle();
		EVoipState voipState = (EVoipState)reader.ReadByte();
		if (!this._E001)
		{
			voipState = EVoipState.NotAvailable;
		}
		bool isInBufferZone = reader.ReadBoolean();
		int bufferZoneUsageTimeLeft = reader.ReadInt32();
		base.MalfRandoms.Deserialize(reader);
		base.Transform.position = position;
		byte[] healthState = null;
		EController eController = EController.None;
		bool isInSpawnOperation = true;
		string[] array = null;
		string itemId = null;
		bool flag2 = false;
		Vector2 stationaryRotation = Vector2.zero;
		Quaternion identity = Quaternion.identity;
		int animationVariant = 0;
		byte[] inventoryZip = reader.SafeReadSizeAndBytes();
		byte[] profileZip = reader.SafeReadSizeAndBytes();
		byte[] bytes = reader.SafeReadSizeAndBytes();
		MongoID firstId = reader.ReadMongoId();
		if (flag)
		{
			_E139 = reader.ReadBoolean();
			reader.ReadInt32();
			healthState = reader.SafeReadSizeAndBytes();
			animationVariant = reader.ReadInt32();
			eController = (EController)reader.ReadByte();
			if (reader.ReadBoolean())
			{
				itemId = reader.ReadString();
			}
			if (eController == EController.Firearm)
			{
				isInSpawnOperation = reader.ReadBoolean();
				flag2 = reader.ReadBoolean();
				if (flag2)
				{
					stationaryRotation = reader.ReadVector2();
					identity.y = reader.ReadSingle();
					identity.w = reader.ReadSingle();
				}
			}
			byte b = reader.ReadByte();
			array = new string[b];
			for (int i = 0; i < b; i++)
			{
				array[i] = reader.ReadString();
			}
		}
		Profile profile = await AsyncWorker.RunOnBackgroundThread(delegate
		{
			_E5D6 obj = _E5D5.Deserialize(new _E518(bytes));
			Profile profile2 = SimpleZlib.Decompress(profileZip).ParseJsonTo<Profile>(Array.Empty<JsonConverter>());
			using (MemoryStream input = new MemoryStream(SimpleZlib.DecompressToBytes(inventoryZip)))
			{
				using BinaryReader reader2 = new BinaryReader(input);
				profile2.Inventory = _E672.DeserializeInventory(Singleton<_E63B>.Instance, reader2.ReadEFTInventoryDescriptor());
			}
			_E5D5.FillSearchInfo(items: profile2.Inventory.NonQuestItems.OfType<_EA91>().ToArray(), searchableItemInfos: obj.Data);
			return profile2;
		});
		if (eController == EController.None)
		{
			Debug.LogError(_ED3E._E000(147704) + base.Profile.Nickname + _ED3E._E000(64014) + base.Profile.AccountId + _ED3E._E000(64014) + base.Profile.Id + _ED3E._E000(189790));
			eController = EController.Empty;
		}
		try
		{
			await spawnSync(base.Id);
		}
		catch (OperationCanceledException)
		{
			Logger.LogError(_ED3E._E000(189706) + base.Profile.Nickname + _ED3E._E000(64014) + base.Profile.AccountId + _ED3E._E000(64014) + base.Profile.Id + _ED3E._E000(189817));
			throw;
		}
		if (!flag)
		{
			_E7A8.TraceError(ETraceCode.ObserverSpawnDead, base.TryGetId);
		}
		await _E028(profile, firstId, rotation, flag, eController, isInSpawnOperation, itemId, flag2, stationaryRotation, identity, healthState, poseLevel, isInPronePose, animationVariant, isInBufferZone, bufferZoneUsageTimeLeft, array?.Where((string s) => !string.IsNullOrEmpty(s)).ToArray(), voipState);
	}

	private async Task _E028(Profile profile, MongoID firstId, Quaternion rotation, bool isAlive, EController type, bool isInSpawnOperation, string itemId, bool isStationary, Vector2 stationaryRotation, Quaternion stationaryPlayerRotation, byte[] healthState, float poseLevel, bool isInPronePose, int animationVariant, bool isInBufferZone, int bufferZoneUsageTimeLeft, [CanBeNull] string[] searchItemIds, EVoipState voipState)
	{
		if (_E0ED == null)
		{
			_E0ED = _E6C3._E000(0);
		}
		Vector3 position = base.Transform.position;
		base.Transform.position = new Vector3(position.x, -10000f, position.z);
		_E003 inventoryController = new _E003(this, profile, firstId);
		await Init(rotation, _ED3E._E000(60679), EPointOfView.ThirdPerson, profile, inventoryController, new _E9CC(healthState, this, inventoryController, profile.Skills), new _E757(), null, _E611.Default, voipState);
		profile.Skills.StartClientMode();
		if (searchItemIds != null)
		{
			foreach (string itemId2 in searchItemIds)
			{
				ProcessStartSearchingAction(itemId2);
			}
		}
		_E10E = new _E3D5(2000);
		_E029();
		await JobScheduler.Yield();
		if (_E2B6.Config.UseSpiritPlayer)
		{
			Spirit.ConnectToPlayerEvents();
		}
		if (isAlive)
		{
			switch (type)
			{
			case EController.Empty:
				await _E004();
				break;
			case EController.Firearm:
				await _E007(itemId, isStationary);
				if (!isInSpawnOperation)
				{
					Weapon obj = HandsController.Item as Weapon;
					Weapon.EMalfunctionState state = obj.MalfState.State;
					obj.MalfState.ChangeStateSilent(Weapon.EMalfunctionState.None);
					HandsController.FastForwardCurrentState();
					obj.MalfState.ChangeStateSilent(state);
					HandsController.FastForwardCurrentState();
				}
				break;
			case EController.Meds:
				await _E00B(itemId, EBodyPart.Head, 1f, animationVariant);
				break;
			case EController.Grenade:
				await _E009(itemId);
				break;
			case EController.Knife:
				await _E00D(itemId);
				break;
			case EController.UsableItem:
				await _E00F(itemId);
				break;
			case EController.QuickGrenade:
				await _E011(itemId);
				break;
			case EController.QuickKnife:
				await _E013(itemId);
				break;
			case EController.None:
				Debug.LogError(_ED3E._E000(192561));
				return;
			default:
				throw new Exception(_ED3E._E000(192606) + type);
			}
			if (isStationary)
			{
				FastForwardToStationaryWeapon(HandsController.Item, stationaryRotation, rotation, stationaryPlayerRotation);
				base.Transform.position = position;
			}
		}
		else
		{
			await _E004();
			OnDead(EDamageType.Undefined);
		}
		await JobScheduler.Yield();
		if (isAlive)
		{
			base.MovementContext.SetPoseLevel(poseLevel, force: true);
			base.MovementContext.SmoothedPoseLevel = poseLevel;
			base.MovementContext.IsInPronePose = isInPronePose;
		}
		Teleport(position);
		TrackPlayerPosition();
		_EBEB.Instance.HandleReconnectedPlayer(isInBufferZone, this, bufferZoneUsageTimeLeft, Task.CompletedTask);
	}

	private void _E029()
	{
		_E117 = (from bodyPartCollider in _hitColliders
			select bodyPartCollider.Collider into cldr
			where cldr != null
			select cldr).ToArray();
		Collider[] array = _E117;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		_E116 = (from x in GetComponentsInChildren<Renderer>(includeInactive: true)
			where x.enabled
			select x).ToArray();
		Renderer[] array2 = _E116;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].enabled = false;
		}
		_E118 = GetCharacterControllerCommon();
		if (_E118.isEnabled)
		{
			_E118.isEnabled = false;
		}
		else
		{
			_E118 = null;
		}
		if (base.ArmsUpdateMode == EUpdateMode.Auto && base.ArmsAnimatorCommon != null)
		{
			base.ArmsAnimatorCommon.enabled = false;
		}
	}

	private static string _E02A(object obj)
	{
		if (obj == null)
		{
			return _ED3E._E000(194875);
		}
		try
		{
			return obj.ToString();
		}
		catch (Exception arg)
		{
			string text = string.Format(_ED3E._E000(194867), arg);
			Debug.LogErrorFormat(text);
			return text;
		}
	}

	private void _E02B()
	{
		Collider[] array = Interlocked.Exchange(ref _E117, null);
		if (array != null)
		{
			try
			{
				Collider[] array2 = array;
				foreach (Collider collider in array2)
				{
					try
					{
						collider.enabled = true;
					}
					catch (Exception ex)
					{
						_E7A8.TraceError(ETraceCode.ObserverCldrEnabledException, base.TryGetId, ex);
						string text = _E02A(collider);
						Debug.LogErrorFormat(_ED3E._E000(194890), text, ex);
					}
				}
			}
			catch (Exception ex2)
			{
				_E7A8.TraceError(ETraceCode.ObserverTemporaryCollidersEnabledException, base.TryGetId, ex2);
				Debug.LogError(ex2);
			}
		}
		Renderer[] array3 = Interlocked.Exchange(ref _E116, null);
		if (array3 != null)
		{
			try
			{
				Renderer[] array4 = array3;
				foreach (Renderer renderer in array4)
				{
					try
					{
						renderer.enabled = true;
					}
					catch (Exception ex3)
					{
						_E7A8.TraceError(ETraceCode.ObserverRenderEnabledException, base.TryGetId, ex3);
						string text2 = _E02A(renderer);
						Debug.LogErrorFormat(_ED3E._E000(194964), text2, ex3);
					}
				}
			}
			catch (Exception ex4)
			{
				_E7A8.TraceError(ETraceCode.ObserverTemporaryRenderersEnabledException, base.TryGetId, ex4);
				Debug.LogError(ex4);
			}
		}
		_E33A obj = Interlocked.Exchange(ref _E118, null);
		if (obj != null)
		{
			try
			{
				obj.isEnabled = true;
			}
			catch (Exception ex5)
			{
				_E7A8.TraceError(ETraceCode.ObserverTemporaryCharacterControllerException, base.TryGetId, ex5);
				string text3 = _E02A(obj);
				Debug.LogErrorFormat(_ED3E._E000(194982), text3, ex5);
			}
		}
		try
		{
			EnabledAnimators = EAnimatorMask.Thirdperson | EAnimatorMask.Arms | EAnimatorMask.Procedural | EAnimatorMask.FBBIK | EAnimatorMask.IK;
		}
		catch (Exception ex6)
		{
			Debug.LogErrorFormat(_ED3E._E000(195100), EAnimatorMask.Thirdperson | EAnimatorMask.Arms | EAnimatorMask.Procedural | EAnimatorMask.FBBIK | EAnimatorMask.IK, ex6);
		}
		try
		{
			if (base.ArmsAnimatorCommon != null)
			{
				base.ArmsAnimatorCommon.enabled = base.ArmsUpdateMode == EUpdateMode.Auto;
			}
		}
		catch (Exception ex7)
		{
			_E7A8.TraceError(ETraceCode.ObserverArmsAnimatorCommonEnabledException, base.TryGetId, ex7);
			Debug.LogErrorFormat(_ED3E._E000(195152), base.ArmsUpdateMode, ex7);
		}
		try
		{
			base.BodyAnimatorCommon.enabled = base.BodyUpdateMode == EUpdateMode.Auto;
		}
		catch (Exception ex8)
		{
			_E7A8.TraceError(ETraceCode.ObserverBodyAnimatorCommonEnabledException, base.TryGetId, ex8);
			Debug.LogErrorFormat(_ED3E._E000(195219), base.BodyUpdateMode, ex8);
		}
	}

	public override void OnDeserializeFromServer(byte channelId, _E524 reader, int rtt)
	{
		int? num = _E132;
		int valueOrDefault = num.GetValueOrDefault();
		if (num.HasValue)
		{
			if (valueOrDefault != Time.frameCount)
			{
				float num2 = Time.time - _E133;
				_E132 = Time.frameCount;
				_E133 = Time.time;
				this._E000 = Mathf.CeilToInt(Mathf.Max(num2 * 1000f, rtt));
			}
		}
		else
		{
			this._E000 = rtt;
			_E132 = Time.frameCount;
			_E133 = Time.time;
		}
		float num3 = (float)this._E000 / 1000f;
		_E131 = Mathf.Clamp(_E12E.NextValue(num3 / _E12D.NextValue(num3)), _E122, _E123);
		if (_E0ED == null)
		{
			_E02C(reader);
		}
		else if ((int)channelId % 2 != 0)
		{
			_E0ED._E002(reader.Buffer);
		}
	}

	private void _E02C(_E524 reader)
	{
		if (NeverInited)
		{
			_E02D();
		}
		try
		{
			_E10D.Receive(reader, _E10E);
		}
		catch (Exception innerException)
		{
			Debug.LogException(new Exception(_ED3E._E000(147704) + FullIdInfo + _ED3E._E000(195278), innerException));
		}
	}

	private void _E02D()
	{
		NeverInited = false;
		if (Destroyed)
		{
			Debug.LogWarning(_ED3E._E000(194789));
			return;
		}
		try
		{
			_E02B();
		}
		catch (Exception ex)
		{
			_E7A8.TraceError(ETraceCode.ObserverBodyShowException, base.TryGetId, ex);
			Debug.LogErrorFormat(_ED3E._E000(195305), ex);
		}
	}

	protected override bool UpdateGrenadeAnimatorDuePoV()
	{
		return true;
	}

	protected override void OnDead(EDamageType damageType)
	{
		base.OnDead(damageType);
		Singleton<BetterAudio>.Instance.ProtagonistHearingChanged -= _E034;
		UnregisterCulling();
	}

	private void _E02E(IPhysicsTrigger trigger)
	{
		ColliderReporter colliderReporter = trigger as ColliderReporter;
		if (!(colliderReporter != null))
		{
			return;
		}
		for (int i = 0; i < colliderReporter.Owners.Count; i++)
		{
			DisablerCullingObject disablerCullingObject = colliderReporter.Owners[i] as DisablerCullingObject;
			if (disablerCullingObject != null)
			{
				_E138.Add(disablerCullingObject);
			}
		}
	}

	private void _E02F(IPhysicsTrigger trigger)
	{
		ColliderReporter colliderReporter = trigger as ColliderReporter;
		if (!(colliderReporter != null))
		{
			return;
		}
		for (int i = 0; i < colliderReporter.Owners.Count; i++)
		{
			DisablerCullingObject disablerCullingObject = colliderReporter.Owners[i] as DisablerCullingObject;
			if (disablerCullingObject != null)
			{
				_E138.Remove(disablerCullingObject);
			}
		}
	}

	protected override bool IsVisibleByCullingObject(float cullingDistance)
	{
		if (_E137.IsVisible)
		{
			return _E137.SqrCameraDistance < cullingDistance * cullingDistance;
		}
		return false;
	}

	private void _E030()
	{
		_E137 = base.gameObject.GetOrAddComponent<FollowerCullingObject>();
		_E137.enabled = true;
		_E137.CullByDistanceOnly = false;
		if (_E2B6.Config.UseSpiritPlayer)
		{
			_E137.Init(Spirit.GetActiveTransform);
		}
		else
		{
			_E137.Init(() => PlayerBones.BodyTransform.Original);
		}
		_E137.SetParams(EFTHardSettings.Instance.CULLING_PLAYER_SPHERE_RADIUS, EFTHardSettings.Instance.CULLING_PLAYER_SPHERE_SHIFT, EFTHardSettings.Instance.CULLING_PLAYER_DISTANCE);
		_E137.OnVisibilityChanged += _E031;
	}

	public void UnregisterCulling()
	{
		if (!(_E137 == null))
		{
			_E137.OnVisibilityChanged -= _E031;
			_E137.enabled = false;
		}
	}

	public override void Dispose()
	{
		UnregisterCulling();
		base.Dispose();
	}

	private void _E031(bool isVisible)
	{
		if (_E2B6.Config.UseSpiritPlayer && Spirit.IsActive == isVisible)
		{
			Spirit.Switch(!isVisible);
		}
	}

	public static void ShowCullingPlayersInfo()
	{
		_E110 = !_E110;
	}

	private void _E032()
	{
		_ = Singleton<GameWorld>.Instance;
	}

	public ulong IngoingPacketsBytes()
	{
		return _E10D.BytesReceived;
	}

	public override void AddStateSpeedLimit(float speedDelta, ESpeedLimit cause)
	{
	}

	public override void UpdateSpeedLimit(float speedDelta, ESpeedLimit cause)
	{
	}

	private void _E033(uint operationId, EOperationStatus status, string error, int inventoryHashSum, bool badBeforeExecuting)
	{
	}

	protected override void StartInflictSelfDamageCoroutine()
	{
	}

	public override void FaceshieldMarkOperation(FaceShieldComponent armor, bool hasServerOrigin)
	{
	}

	public override void SetAudioProtagonist()
	{
	}

	protected override void InitAudioController()
	{
		base.InitAudioController();
		Singleton<BetterAudio>.Instance.ProtagonistHearingChanged += _E034;
	}

	private void _E034()
	{
		if (NestedStepSoundSource != null)
		{
			NestedStepSoundSource.SetRolloff(60f * ProtagonistHearing);
		}
		if (base.VoipAudioSource != null)
		{
			base.VoipAudioSource.Source.maxDistance = 60f * ProtagonistHearing;
		}
	}

	protected override Corpse CreateCorpse()
	{
		return CreateCorpse<ObservedCorpse>(_E120.CorpseImpulse.OverallVelocity);
	}

	public void ResetInterpolator()
	{
		_E10E.Clear();
		_E119 = _E4DF.NOT_VALID_TIME_BOUND;
	}

	private void _E035()
	{
		base.CharacterControllerCommon.GetCollider().enabled = false;
	}

	private void _E036()
	{
		base.CharacterControllerCommon.GetCollider().enabled = true;
	}

	protected override void InitialProfileExamineAll()
	{
	}

	protected override void ApplyCorpseImpulse()
	{
		if (!_E120.IsAlive && _E120.CorpseImpulse.BodyPartColliderType != EBodyPartColliderType.None)
		{
			Collider collider = PlayerBones.BodyPartCollidersDictionary[_E120.CorpseImpulse.BodyPartColliderType].Collider;
			Corpse.Ragdoll.ApplyImpulse(collider, _E120.CorpseImpulse.Direction, _E120.CorpseImpulse.Point, _E120.CorpseImpulse.Force);
		}
	}

	internal override void _E072(Item item, string zone)
	{
	}

	[CompilerGenerated]
	private EmptyHandsController _E037()
	{
		return ObservedEmptyHandsController._E000(this);
	}

	[CompilerGenerated]
	private AbstractHandsController _E038()
	{
		return ObservedEmptyHandsController._E000(this);
	}

	[CompilerGenerated]
	private Task<ObservedEmptyHandsController> _E039()
	{
		return ObservedEmptyHandsController._E001(this);
	}

	[CompilerGenerated]
	private int _E03A()
	{
		return _E0DE.Inventory.CreateInventoryHashSum(new EquipmentSlot[1] { EquipmentSlot.SecuredContainer });
	}

	[CompilerGenerated]
	private Transform _E03B()
	{
		return PlayerBones.BodyTransform.Original;
	}
}
