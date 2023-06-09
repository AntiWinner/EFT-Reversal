using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Interactive;
using EFT.MovingPlatforms;
using EFT.SynchronizableObjects;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal class ClientWorld : World
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public _E5C7 packet;

		internal bool _E000(Player x)
		{
			return x.PlayerId == packet.PlayerId;
		}
	}

	private new Dictionary<int, WorldInteractiveObject._E000> _E018;

	[CompilerGenerated]
	private new Action<int, Turnable.EState> _E019;

	[CompilerGenerated]
	private Action<int, Vector3> _E01A;

	private readonly List<_E5C3> _E01B = new List<_E5C3>(8);

	private readonly List<_E358> _E01C = new List<_E358>(16);

	private readonly ArrayPool<byte> _E01D = ArrayPool<byte>.Shared;

	public event Action<int, Turnable.EState> OnRpcLampStateChanged
	{
		[CompilerGenerated]
		add
		{
			Action<int, Turnable.EState> action = _E019;
			Action<int, Turnable.EState> action2;
			do
			{
				action2 = action;
				Action<int, Turnable.EState> value2 = (Action<int, Turnable.EState>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E019, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, Turnable.EState> action = _E019;
			Action<int, Turnable.EState> action2;
			do
			{
				action2 = action;
				Action<int, Turnable.EState> value2 = (Action<int, Turnable.EState>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E019, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<int, Vector3> OnRpcWindowHit
	{
		[CompilerGenerated]
		add
		{
			Action<int, Vector3> action = _E01A;
			Action<int, Vector3> action2;
			do
			{
				action2 = action;
				Action<int, Vector3> value2 = (Action<int, Vector3>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E01A, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, Vector3> action = _E01A;
			Action<int, Vector3> action2;
			do
			{
				action2 = action;
				Action<int, Vector3> value2 = (Action<int, Vector3>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E01A, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	internal new void _E000(_E524 reader, PacketPriority priority)
	{
		if (base._E00E == null)
		{
			_E001(reader, priority);
		}
		else if (priority == PacketPriority.Critical)
		{
			base._E00E._E002(reader.Buffer);
		}
	}

	protected override void _E016()
	{
		for (int i = 0; i < SubWorld._E003; i++)
		{
			SubWorld item = SubWorld._E000(base._E00F, this, i, _E01D);
			base._E009.Add(item);
		}
	}

	private new void _E001(_E524 reader, PacketPriority priority)
	{
		_packet = reader.DeserializeGameWorldPacket(_previousPacket);
		_E003(ref _packet);
		if (priority == PacketPriority.Critical)
		{
			_previousPacket.Dispose();
			_previousPacket = _packet.Clone();
		}
		_packet.Dispose();
	}

	internal void _E002()
	{
		base._E00E._E003(delegate(_E524 reader)
		{
			_E001(reader, PacketPriority.Critical);
		});
		base._E00E = null;
	}

	public override void SubscribeToBorderZones(BorderZone[] zones)
	{
		for (int i = 0; i < zones.Length; i++)
		{
			zones[i].RemoveAuthority();
		}
	}

	private void _E003(ref _E5BC gameWorldPacket)
	{
		_E5BD? interactiveObjectsStatusPacket = gameWorldPacket.InteractiveObjectsStatusPacket;
		if (interactiveObjectsStatusPacket.HasValue)
		{
			_E518 reader = new _E518(interactiveObjectsStatusPacket.GetValueOrDefault().Segment.Array);
			_E005(reader);
		}
		_E5BF? spawnQuestLootPacket = gameWorldPacket.SpawnQuestLootPacket;
		if (spawnQuestLootPacket.HasValue)
		{
			_E5BF valueOrDefault = spawnQuestLootPacket.GetValueOrDefault();
			Singleton<GameWorld>.Instance._E009(valueOrDefault.Item);
		}
		_E5C0? obj = gameWorldPacket.UpdateExfiltrationPointPacket;
		while (obj.HasValue)
		{
			_E5C0 valueOrDefault2 = obj.GetValueOrDefault();
			_E57A.Instance.UpdatePoint(valueOrDefault2.PointName, valueOrDefault2.Command);
			obj = valueOrDefault2.GetNested();
		}
		if (WeatherEventController.instance != null)
		{
			_E5BE halloweenEventPacket = gameWorldPacket.HalloweenEventPacket;
			WeatherEventController.instance.SetWeather(halloweenEventPacket.Hours, halloweenEventPacket.Minutes, halloweenEventPacket.Cloudness, halloweenEventPacket.WindDirection, halloweenEventPacket.Wind, halloweenEventPacket.Rain, halloweenEventPacket.RainRandomness, halloweenEventPacket.ScaterringFogDensity, halloweenEventPacket.TopWindDirection);
			WeatherEventController.instance.Activate(halloweenEventPacket.active, halloweenEventPacket.timeChangePercentage);
		}
		_E5C1? obj2 = gameWorldPacket.LampChangeStatePacket;
		while (obj2.HasValue)
		{
			_E5C1 valueOrDefault3 = obj2.GetValueOrDefault();
			_E019?.Invoke(valueOrDefault3.NetId, valueOrDefault3.State);
			obj2 = valueOrDefault3.GetNested();
		}
		_E5C2? obj3 = gameWorldPacket.WindowHitPacket;
		while (obj3.HasValue)
		{
			_E5C2 valueOrDefault4 = obj3.GetValueOrDefault();
			_E01A?.Invoke(valueOrDefault4.NetId, valueOrDefault4.HitPosition);
			obj3 = valueOrDefault4.GetNested();
		}
		List<_E5C3> lootSyncPackets = gameWorldPacket.LootSyncPackets;
		if (lootSyncPackets.Count > 0)
		{
			_E007(lootSyncPackets);
		}
		List<_E358> synchronizableObjectPackets = gameWorldPacket.SynchronizableObjectPackets;
		if (synchronizableObjectPackets.Count > 0)
		{
			_E009(synchronizableObjectPackets);
		}
		List<_E5C4> corpseSyncPackets = gameWorldPacket.CorpseSyncPackets;
		if (corpseSyncPackets.Count > 0)
		{
			_E00C(corpseSyncPackets, Singleton<GameWorld>.Instance.LootItems);
		}
		List<_E5C5> grenadeSyncPackets = gameWorldPacket.GrenadeSyncPackets;
		if (grenadeSyncPackets.Count > 0)
		{
			_E00D(grenadeSyncPackets, Singleton<GameWorld>.Instance.Grenades);
		}
		List<_E5C6> platformSyncPackets = gameWorldPacket.PlatformSyncPackets;
		if (platformSyncPackets.Count > 0)
		{
			_E00E(platformSyncPackets, Singleton<GameWorld>.Instance.PlatformAdapters);
		}
		List<_E5C7> borderZonePackets = gameWorldPacket.BorderZonePackets;
		if (borderZonePackets.Count > 0)
		{
			_E004(borderZonePackets);
		}
		(base._E00F as ClientGameWorld)?.ProcessWordPacket(gameWorldPacket);
	}

	private new void _E004(List<_E5C7> packets)
	{
		foreach (_E5C7 packet in packets)
		{
			Player player = Singleton<GameWorld>.Instance.RegisteredPlayers.FirstOrDefault((Player x) => x.PlayerId == packet.PlayerId);
			Singleton<GameWorld>.Instance.BorderZones[packet.Id].ProcessIncomingPacket(player, packet.WillHit);
		}
	}

	private void _E005(_E524 reader)
	{
		_E00F();
		uint num = reader.ReadUInt32UsingBitRange(base._E000);
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadLimitedInt32(0, 2047);
			EDoorState eDoorState = (EDoorState)reader.ReadBits(5);
			string id = string.Empty;
			if (_E018.TryGetValue(num2, out var value))
			{
				id = value.Id;
			}
			else
			{
				Debug.LogErrorFormat(_ED3E._E000(133752), num2);
			}
			float angle = float.NaN;
			if ((eDoorState & EDoorState.Interacting) != 0)
			{
				angle = (float)(reader.ReadBits(5) * 15) - 180f;
			}
			_E018[num2] = new WorldInteractiveObject._E000(id, eDoorState, angle)
			{
				NetId = num2,
				Updated = true
			};
		}
		_E006(_E018, init: false);
	}

	private new void _E006(Dictionary<int, WorldInteractiveObject._E000> infos, bool init)
	{
		WorldInteractiveObject[] array = base._E006;
		foreach (WorldInteractiveObject worldInteractiveObject in array)
		{
			if (infos.TryGetValue(worldInteractiveObject.NetId, out var value) && value.Updated)
			{
				if (init)
				{
					worldInteractiveObject.SetInitialSyncState(value);
				}
				else
				{
					worldInteractiveObject.SyncInteractState(value);
				}
			}
		}
	}

	private void _E007(List<_E5C3> packets)
	{
		foreach (_E5C3 packet in packets)
		{
			_E008(packet);
		}
	}

	private void _E008(_E5C3 packet)
	{
		for (int i = 0; i < _E01B.Count; i++)
		{
			if (_E01B[i].Id == packet.Id)
			{
				_E01B[i] = packet;
				return;
			}
		}
		_E01B.Add(packet);
	}

	private new void _E009(List<_E358> packets)
	{
		foreach (_E358 packet in packets)
		{
			_E00A(packet);
		}
	}

	private void _E00A(_E358 packet)
	{
		for (int i = 0; i < _E01C.Count; i++)
		{
			if (_E01C[i].ObjectId == packet.ObjectId)
			{
				_E01C[i] = packet;
				return;
			}
		}
		_E01C.Add(packet);
	}

	private void _E00B(_E373<int, LootItem> lootItems)
	{
		for (int num = _E01B.Count - 1; num >= 0; num--)
		{
			_E5C3 packet = _E01B[num];
			if (lootItems.TryGetByKey(packet.Id, out var value))
			{
				if (value is ObservedLootItem observedLootItem)
				{
					observedLootItem.ApplyNetPacket(packet);
				}
				_E01B.RemoveAt(num);
			}
		}
	}

	private void _E00C(List<_E5C4> packets, _E373<int, LootItem> lootItems)
	{
		foreach (_E5C4 packet in packets)
		{
			if (lootItems.TryGetByKey(packet.Id, out var value) && value is ObservedCorpse observedCorpse && observedCorpse.HasRagdoll)
			{
				try
				{
					observedCorpse.ApplyNetPacket(packet);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
	}

	private void _E00D(List<_E5C5> packets, _E373<int, Throwable> grenades)
	{
		foreach (_E5C5 packet in packets)
		{
			if (grenades.TryGetByKey(packet.Id, out var value))
			{
				value.ApplyNetPacket(packet);
			}
		}
	}

	private new void _E00E(List<_E5C6> packets, MovingPlatform._E001[] adapters)
	{
		for (int i = 0; i < packets.Count; i++)
		{
			adapters[packets[i].Id].StoreNetPacket(packets[i]);
		}
	}

	private void Update()
	{
		_E00B(Singleton<GameWorld>.Instance.LootItems);
		Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor.ProcessSyncObjectPackets(_E01C);
	}

	private new void _E00F()
	{
		if (_E018 == null)
		{
			_E018 = new Dictionary<int, WorldInteractiveObject._E000>(200);
		}
		WorldInteractiveObject[] array = base._E006;
		foreach (WorldInteractiveObject worldInteractiveObject in array)
		{
			_E018[worldInteractiveObject.NetId] = new WorldInteractiveObject._E000(worldInteractiveObject.Id, worldInteractiveObject.DoorState, worldInteractiveObject.CurrentAngle);
		}
	}

	public override Task OnDeserialize(NetworkReader reader)
	{
		reader = _E001(reader);
		uint position = reader.Position;
		_E57A.Instance.ReadStates(reader);
		_EBEB.Instance.ReadStates(reader);
		_E010(reader);
		_E011(reader);
		_E012(reader);
		_E013(reader);
		_E014(reader);
		Debug.Log(string.Format(_ED3E._E000(133763), reader.Position - position));
		return base.OnDeserialize(reader);
	}

	private void _E010(NetworkReader reader)
	{
		int num = reader.ReadInt32();
		if (num > 0)
		{
			List<_E335> list = new List<_E335>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(_E335.Read(reader));
			}
			base._E00F.OnSmokeGrenadesDeserialized(list);
		}
	}

	private void _E011(NetworkReader reader)
	{
		_E00F();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			short num2 = reader.ReadInt16();
			byte num3 = reader.ReadByte();
			EDoorState state = (EDoorState)(num3 & 0xEF);
			bool isBroken = (num3 & 0x10) != 0;
			string id = string.Empty;
			if (_E018.TryGetValue(num2, out var value))
			{
				id = value.Id;
			}
			else
			{
				Debug.LogErrorFormat(_ED3E._E000(133752), num2);
			}
			_E018[num2] = new WorldInteractiveObject._E000(id, state, float.NaN)
			{
				NetId = num2,
				Updated = true,
				IsBroken = isBroken
			};
		}
		_E006(_E018, init: true);
	}

	private void _E012(NetworkReader reader)
	{
		int num = reader.ReadInt32();
		if (num <= 0)
		{
			return;
		}
		Dictionary<int, LampController> dictionary = (from x in LocationScene.GetAllObjects<LampController>(withDisabled: true)
			where x.NetId != 0
			select x).ToDictionary((LampController x) => x.NetId);
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadInt32();
			Turnable.EState eState = (Turnable.EState)reader.ReadByte();
			if (dictionary.TryGetValue(num2, out var value))
			{
				if (value.LampState != eState)
				{
					value.Switch(eState);
				}
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(133849), num2));
			}
		}
	}

	private void _E013(NetworkReader reader)
	{
		int num = reader.ReadInt32();
		if (num <= 0)
		{
			return;
		}
		Dictionary<int, WindowBreaker> dictionary = new Dictionary<int, WindowBreaker>(num);
		foreach (WindowBreaker item in from x in LocationScene.GetAllObjects<WindowBreaker>(withDisabled: true)
			where x.AvailableToSync
			select x)
		{
			if (dictionary.ContainsKey(item.NetId))
			{
				Debug.LogErrorFormat(_ED3E._E000(133880), item.NetId);
			}
			else
			{
				dictionary.Add(item.NetId, item);
			}
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadInt32();
			Vector3 hitPoint = reader.ReadVector3();
			if (dictionary.TryGetValue(num2, out var value))
			{
				try
				{
					WindowBreaker windowBreaker = value;
					_EC23 hitInfo = new _EC23
					{
						HitPoint = hitPoint
					};
					windowBreaker.MakeHit(in hitInfo, instantFall: true);
				}
				catch (Exception exception)
				{
					if (value != null && value.transform != null)
					{
						Debug.LogErrorFormat(_ED3E._E000(133888), value.transform.GetFullPath(withSceneName: true));
					}
					Debug.LogException(exception);
				}
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(133928), num2));
			}
		}
	}

	private void _E014(NetworkReader reader)
	{
		ushort num = reader.ReadUInt16();
		if (num > 0)
		{
			_E90E synchronizableObjectLogicProcessor = Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor;
			for (int i = 0; i < num; i++)
			{
				byte type = reader.ReadByte();
				synchronizableObjectLogicProcessor.TakeFromPool((SynchronizableObjectType)type).Deserialize(reader);
			}
		}
	}

	[CompilerGenerated]
	private void _E015(_E524 reader)
	{
		_E001(reader, PacketPriority.Critical);
	}
}
