using System;
using System.Collections.Generic;
using System.Linq;
using Audio.SpatialSystem;
using EFT;
using UnityEngine;

[Serializable]
public class SpatialAudioRoom : MonoBehaviour
{
	[Serializable]
	public class RoomConnection
	{
		public SpatialAudioRoom connectedRoom;

		public SpatialAudioPortal[] connectingPortals;
	}

	private readonly _E48C _logger = new _E48C();

	[Tooltip("Optionally give the room a unique name for more informative debug messages.")]
	public string roomName;

	public List<AudioTriggerArea> Areas;

	public bool Outdoor;

	public bool Isolated;

	[Tooltip("experimental options to fit room in scene geometry")]
	public bool FitToGeometry;

	public List<RoomConnection> roomConnections = new List<RoomConnection>();

	[SerializeField]
	private int _iD;

	private uint _enteredAreasCount;

	private readonly Dictionary<int, uint> _playersEnteredAreasCount = new Dictionary<int, uint>();

	public BoxCollider[] Colliders { get; private set; }

	public bool IsInitialized { get; private set; }

	public int ID => _iD;

	protected virtual void Awake()
	{
		IsInitialized = false;
	}

	private void _E000()
	{
		foreach (AudioTriggerArea area in Areas)
		{
			area.OnTriggerArea += _E005;
		}
	}

	public void Initialize()
	{
		_E004();
		if (_E002())
		{
			_E001();
			_E003();
			_E000();
			IsInitialized = true;
		}
	}

	private void _E001()
	{
		if (Areas.Count == 0)
		{
			return;
		}
		try
		{
			Colliders = new BoxCollider[Areas.Count];
			for (int i = 0; i < Areas.Count; i++)
			{
				Colliders[i] = Areas[i].GetCollider();
			}
		}
		catch (Exception)
		{
			_logger.LogError(ESpatialAudioRoomError.NoValidTriggerCollider, roomName);
		}
	}

	private bool _E002()
	{
		if (Areas == null)
		{
			_logger.LogError(ESpatialAudioRoomError.AudioTriggerAreaIsNull, roomName);
			return false;
		}
		foreach (RoomConnection roomConnection in roomConnections)
		{
			if (roomConnection == null)
			{
				_logger.LogError(ESpatialAudioRoomError.RoomConnectionIsNull, roomName);
				return false;
			}
			if ((object)roomConnection.connectedRoom == null)
			{
				_logger.LogError(ESpatialAudioRoomError.ConnectedRoomIsNull, roomName);
				return false;
			}
			if (roomConnection.connectingPortals == null)
			{
				_logger.LogError(ESpatialAudioRoomError.ConnectedPortalIsNull, roomName);
				return false;
			}
			if (!roomConnection.connectingPortals.Any((SpatialAudioPortal connectingPortal) => (object)connectingPortal == null))
			{
				continue;
			}
			_logger.LogError(ESpatialAudioRoomError.ConnectedPortalIsNull, roomName);
			return false;
		}
		return true;
	}

	private void _E003()
	{
		foreach (RoomConnection roomConnection in roomConnections)
		{
			SpatialAudioPortal[] connectingPortals = roomConnection.connectingPortals;
			for (int i = 0; i < connectingPortals.Length; i++)
			{
				connectingPortals[i].SetConnectedRoom(this);
			}
		}
	}

	private void _E004()
	{
		roomName = (string.IsNullOrEmpty(roomName) ? base.gameObject.name : roomName);
	}

	private void _E005(object sender, _E47F eventArgs)
	{
		if (!MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
		{
			return;
		}
		Player player = eventArgs.Player;
		if (player == null || !player.HealthController.IsAlive)
		{
			return;
		}
		int id = player.Id;
		if (eventArgs.TriggerEventType == _E47F.ETriggerEventType.TriggerEnter)
		{
			if (_playersEnteredAreasCount.TryGetValue(id, out var value))
			{
				if (value == 0)
				{
					MonoBehaviourSingleton<SpatialAudioSystem>.Instance.AddPlayerCurrentRoom(this, player);
				}
				_playersEnteredAreasCount[id]++;
			}
			else
			{
				_playersEnteredAreasCount.Add(id, 1u);
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.AddPlayerCurrentRoom(this, player);
			}
		}
		if (eventArgs.TriggerEventType == _E47F.ETriggerEventType.TriggerExit && _playersEnteredAreasCount.TryGetValue(id, out var value2))
		{
			value2--;
			_playersEnteredAreasCount[id] = value2;
			if (value2 == 0)
			{
				MonoBehaviourSingleton<SpatialAudioSystem>.Instance.RemovePlayerCurrentRoom(this, player);
				_playersEnteredAreasCount.Remove(id);
			}
		}
	}

	private void OnDestroy()
	{
		_E006();
	}

	private void _E006()
	{
		foreach (AudioTriggerArea area in Areas)
		{
			area.OnTriggerArea -= _E005;
		}
	}
}
