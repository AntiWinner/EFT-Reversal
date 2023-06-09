#define BATTLEYE_ANTICHEAT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattlEye;
using Comfort.Common;
using Diz.Jobs;
using EFT.Interactive;
using EFT.SynchronizableObjects;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal abstract class World : MonoBehaviour
{
	protected readonly int[] _E000 = new int[3] { 4, 8, 10 };

	protected const int _E001 = 2047;

	public const int SERIALIZATION_BUFFER_SIZE = 65536;

	private readonly List<WindowBreaker> m__E002 = new List<WindowBreaker>(256);

	private readonly List<_E335> m__E003 = new List<_E335>(8);

	protected readonly List<WorldInteractiveObject._E000> _E004 = new List<WorldInteractiveObject._E000>(20);

	private List<SynchronizableObject> m__E005 = new List<SynchronizableObject>(20);

	public IDictionary<string, int> Interactables;

	protected WorldInteractiveObject[] _E006;

	private Dictionary<string, WorldInteractiveObject> m__E007;

	private IDictionary<NetworkConnection, ChannelCombined> m__E008;

	protected readonly List<SubWorld> _E009 = new List<SubWorld>();

	private int m__E00A;

	private int m__E00B;

	private readonly _E548 m__E00C = new _E548();

	private readonly List<NetworkConnection> m__E00D = new List<NetworkConnection>();

	protected _E6C3 _E00E = _E6C3._E001();

	public _E5BC _previousPacket = _E5BC.Create();

	public _E5BC _packet = _E5BC.Create();

	protected GameWorld _E00F;

	private bool _E010;

	private readonly TaskCompletionSource<object> _E011 = new TaskCompletionSource<object>();

	private readonly Queue<Task<Func<TimeSpan>>> _E012 = new Queue<Task<Func<TimeSpan>>>();

	private Task _E013;

	private int _E014;

	private List<TaskCompletionSource<object>> _E015;

	private readonly Queue<byte[]> m__E016 = new Queue<byte[]>();

	private Task _E017;

	internal static NetworkHash128 _E018 => NetworkHash128.Parse(_ED3E._E000(47791));

	internal static World _E019 => Singleton<GameWorld>.Instance.GetComponent<World>();

	internal static _E077 _E000<_E077>(IDictionary<string, int> interactables, IDictionary<NetworkConnection, ChannelCombined> channels, bool crypted) where _E077 : World
	{
		Debug.Log(_ED3E._E000(110263));
		GameWorld instance = Singleton<GameWorld>.Instance;
		_E077 val = instance.gameObject.AddComponent<_E077>();
		val._E00F = instance;
		((World)val).m__E008 = channels;
		val._E010 = crypted;
		val.RegisterNetworkInteractionObjects(interactables);
		instance._E012(val);
		val._E016();
		return val;
	}

	protected virtual void _E016()
	{
		for (int i = 0; i < SubWorld._E003; i++)
		{
			SubWorld item = SubWorld._E000(_E00F, this, i, null);
			this._E009.Add(item);
		}
	}

	internal NetworkReader _E001(NetworkReader reader)
	{
		if (!_E010)
		{
			return reader;
		}
		byte[] array = reader.ReadBytesAndSize();
		byte[] array2 = new byte[65535];
		int decryptedPacketLength = array2.Length;
		BEClient.DecryptPacket(array, 0, array.Length, array2, 0, ref decryptedPacketLength);
		reader = new NetworkReader(array2);
		reader.ReadInt32();
		return reader;
	}

	internal void _E002(NetworkConnection connection)
	{
		this.m__E00D.Remove(connection);
	}

	internal void _E003(NetworkReader reader)
	{
		int index = this.m__E00A++;
		this._E009[index].OnDeserialize(reader);
	}

	internal static void _E004()
	{
		Debug.Log(_ED3E._E000(110244));
		UnityEngine.Object.Destroy(Singleton<GameWorld>.Instance.GetComponent<World>());
	}

	protected virtual void OnDestroy()
	{
		_previousPacket.Dispose();
		_packet.Dispose();
	}

	internal void _E005(_E548 loot)
	{
		if (loot != null)
		{
			this.m__E00C.AddRange(loot);
		}
		this.m__E00B++;
		if (this.m__E00B == SubWorld._E003)
		{
			Singleton<GameWorld>.Instance._E008(this.m__E00C);
		}
	}

	public void AddLampChangeStatePacket(int netId, Turnable.EState state)
	{
		_E5C1 nested = default(_E5C1);
		nested.NetId = netId;
		nested.State = state;
		nested.AttachTo(ref _packet.LampChangeStatePacket);
	}

	public void AddWindowBreakPacket(int netId, in Vector3 hitPosition)
	{
		_E5C2 nested = default(_E5C2);
		nested.NetId = netId;
		nested.HitPosition = hitPosition;
		nested.AttachTo(ref _packet.WindowHitPacket);
	}

	public void AddInteractiveObjectsStatusPacket(ArraySegment<byte> segment)
	{
		_packet.InteractiveObjectsStatusPacket = new _E5BD
		{
			Segment = segment
		};
	}

	public void AddUpdateExfiltrationPointPacket(string pointName, EExfiltrationStatus command)
	{
		_E5C0 nested = default(_E5C0);
		nested.PointName = pointName;
		nested.Command = command;
		nested.AttachTo(ref _packet.UpdateExfiltrationPointPacket);
	}

	private async Task _E006()
	{
		while (this.m__E016.Count > 0)
		{
			if (!_packet.SpawnQuestLootPacket.HasValue)
			{
				byte[] item = this.m__E016.Dequeue();
				_packet.SpawnQuestLootPacket = new _E5BF
				{
					Item = item
				};
			}
			await JobScheduler.Yield();
		}
		_E017 = null;
	}

	public void AddSpawnQuestLootPacket(byte[] item)
	{
		if (!_packet.SpawnQuestLootPacket.HasValue)
		{
			_packet.SpawnQuestLootPacket = new _E5BF
			{
				Item = item
			};
			return;
		}
		this.m__E016.Enqueue(item);
		if (_E017 == null)
		{
			_E017 = _E006();
		}
	}

	public void OnSerialize(NetworkWriter writer)
	{
		short position = writer.Position;
		_E57A.Instance.WriteStates(writer);
		_EBEB.Instance.WriteStates(writer);
		_E00B(writer);
		_E00A(writer);
		_E00C(writer);
		_E00D(writer);
		_E00E(writer);
		Debug.Log(_ED3E._E000(110298) + (writer.Position - position));
	}

	public virtual Task OnDeserialize(NetworkReader reader)
	{
		return _E011.Task;
	}

	public void RegisterNetworkInteractionObjects(IDictionary<string, int> interactables = null)
	{
		WorldInteractiveObject[] array = LocationScene.GetAllObjects<WorldInteractiveObject>().ToArray();
		WorldInteractiveObject[] array2;
		if (interactables == null)
		{
			Interactables = new Dictionary<string, int>();
			array2 = array;
			foreach (WorldInteractiveObject worldInteractiveObject in array2)
			{
				if (Interactables.ContainsKey(worldInteractiveObject.Id))
				{
					Debug.LogError(_ED3E._E000(110326) + worldInteractiveObject.Id, worldInteractiveObject);
				}
				else
				{
					Interactables.Add(worldInteractiveObject.Id, worldInteractiveObject.NetId);
				}
			}
		}
		else
		{
			array2 = array;
			foreach (WorldInteractiveObject worldInteractiveObject2 in array2)
			{
				if (interactables.ContainsKey(worldInteractiveObject2.Id))
				{
					worldInteractiveObject2.NetId = interactables[worldInteractiveObject2.Id];
				}
				else
				{
					Debug.LogError(_ED3E._E000(110348) + worldInteractiveObject2.Id + _ED3E._E000(11164));
				}
			}
		}
		this._E006 = array.Where((WorldInteractiveObject x) => !(x is LootableContainer)).ToArray();
		this.m__E007 = new Dictionary<string, WorldInteractiveObject>();
		array2 = array;
		foreach (WorldInteractiveObject worldInteractiveObject3 in array2)
		{
			RegisterWorldInteractionObject(worldInteractiveObject3);
		}
	}

	public void RegisterWorldInteractionObject(WorldInteractiveObject worldInteractiveObject)
	{
		if (this.m__E007.ContainsKey(worldInteractiveObject.Id))
		{
			Debug.LogErrorFormat(this.m__E007[worldInteractiveObject.Id], _ED3E._E000(110377), worldInteractiveObject.Id);
			Debug.LogErrorFormat(worldInteractiveObject, _ED3E._E000(110444), worldInteractiveObject.Id);
		}
		else
		{
			this.m__E007.Add(worldInteractiveObject.Id, worldInteractiveObject);
		}
	}

	protected void _E007(in ArraySegment<byte> segment, PacketPriority priority)
	{
		for (int i = 0; i < this.m__E00D.Count; i++)
		{
			NetworkConnection key = this.m__E00D[i];
			if (this.m__E008.TryGetValue(key, out var value))
			{
				value._E005(_E6A8._E005, in segment, priority);
			}
			else
			{
				Debug.LogError(_ED3E._E000(110470));
			}
		}
	}

	public WorldInteractiveObject FindDoor(string doorId)
	{
		if (string.IsNullOrEmpty(doorId))
		{
			return null;
		}
		if (!this.m__E007.TryGetValue(doorId, out var value))
		{
			Debug.LogError(_ED3E._E000(110510) + doorId + _ED3E._E000(110552));
		}
		return value;
	}

	public void OnExfiltrationPointUpdate(ExfiltrationPoint point)
	{
		AddUpdateExfiltrationPointPacket(point.Settings.Name, point.Status);
	}

	public virtual void SubscribeToBorderZones(BorderZone[] zones)
	{
	}

	public void EmptySubWorld()
	{
		Task<Func<TimeSpan>> task = Task.FromResult<Func<TimeSpan>>(() => TimeSpan.Zero);
		SpawnSubWorld(task);
	}

	public void SpawnSubWorld(Task<Func<TimeSpan>> task)
	{
		_E012.Enqueue(task);
		if (_E013 == null)
		{
			_E013 = _E008();
		}
	}

	private async Task _E008()
	{
		int frameCount = Time.frameCount;
		while (_E012.Count > 0)
		{
			try
			{
				Func<TimeSpan> func = await _E012.Dequeue();
				if (frameCount == Time.frameCount)
				{
					await Task.Yield();
				}
				if ((int)func().TotalMilliseconds > 0)
				{
					frameCount = Time.frameCount;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				_E014++;
			}
		}
		if (_E014 == SubWorld._E003)
		{
			await _E009();
		}
		_E013 = null;
	}

	private async Task _E009()
	{
		Debug.Log(string.Format(_ED3E._E000(110540), _E014));
		_E011.TrySetResult(true);
		List<TaskCompletionSource<object>> list = _E015;
		_E015 = null;
		if (list == null)
		{
			return;
		}
		foreach (TaskCompletionSource<object> item in list)
		{
			item.TrySetResult(true);
			await Task.Yield();
		}
	}

	public Task AwaitSpawnCompletion()
	{
		Task<object> task = _E011.Task;
		if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
		{
			return Task.CompletedTask;
		}
		if (_E015 == null)
		{
			_E015 = new List<TaskCompletionSource<object>>();
		}
		TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
		_E015.Add(taskCompletionSource);
		return taskCompletionSource.Task;
	}

	private void _E00A(NetworkWriter writer)
	{
		this._E004.Clear();
		WorldInteractiveObject[] array = this._E006;
		foreach (WorldInteractiveObject worldInteractiveObject in array)
		{
			if ((worldInteractiveObject.DoorState != worldInteractiveObject.InitialDoorState && worldInteractiveObject.DoorState != EDoorState.Interacting) || (worldInteractiveObject is Door door && door.IsBroken))
			{
				this._E004.Add(worldInteractiveObject.GetStatusInfo(solidState: true));
			}
		}
		writer.Write(this._E004.Count);
		foreach (WorldInteractiveObject._E000 item in this._E004)
		{
			writer.Write((short)item.NetId);
			byte b = item.State;
			if (item.IsBroken)
			{
				b = (byte)(b | 0x10u);
			}
			writer.Write(b);
		}
	}

	private void _E00B(NetworkWriter writer)
	{
		this.m__E003.Clear();
		foreach (Throwable item in _E00F.Grenades.GetValuesEnumerator())
		{
			if (item is SmokeGrenade smokeGrenade)
			{
				this.m__E003.Add(smokeGrenade.NetworkData);
			}
		}
		writer.Write(this.m__E003.Count);
		foreach (_E335 item2 in this.m__E003)
		{
			item2.Write(writer);
		}
	}

	private static void _E00C(NetworkWriter writer)
	{
		IEnumerable<LampController> allObjects = LocationScene.GetAllObjects<LampController>();
		writer.Write(allObjects.Count());
		foreach (LampController item in allObjects)
		{
			writer.Write(item.NetId);
			writer.Write((byte)item.LampState);
		}
	}

	private void _E00D(NetworkWriter writer)
	{
		this.m__E002.Clear();
		foreach (WindowBreaker item in _E00F.Windows.GetValuesEnumerator())
		{
			if (item.AvailableToSync && item.IsDamaged)
			{
				this.m__E002.Add(item);
			}
		}
		writer.Write(this.m__E002.Count);
		if (this.m__E002.Count <= 0)
		{
			return;
		}
		foreach (WindowBreaker item2 in this.m__E002)
		{
			writer.Write(item2.NetId);
			writer.Write(item2.FirstHitPosition.Value);
		}
	}

	private void _E00E(NetworkWriter writer)
	{
		Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor.GetSynchronizableObjects(ref this.m__E005);
		writer.Write((ushort)this.m__E005.Count);
		if (this.m__E005.Count <= 0)
		{
			return;
		}
		foreach (SynchronizableObject item in this.m__E005)
		{
			writer.Write((byte)item.Type);
			item.Serialize(writer);
		}
		this.m__E005.Clear();
	}
}
