using System;
using UnityEngine;

namespace Audio.SpatialSystem;

[Serializable]
public class RoomPair : IEquatable<RoomPair>
{
	[Serializable]
	public struct RoutesIdGroup
	{
		public int GroupIndex;

		public int[] IDs;
	}

	[SerializeField]
	private int _firstRoomID;

	[SerializeField]
	private int SecondRoom;

	public RoutesIdGroup[] RoutesIdGroups;

	public RoomPair(SpatialAudioRoom firstRoom, SpatialAudioRoom secondRoom, RoutesIdGroup[] routesIdGroups)
	{
		_firstRoomID = firstRoom.ID;
		SecondRoom = secondRoom.ID;
		RoutesIdGroups = routesIdGroups;
	}

	public bool IsMatch(SpatialAudioRoom firstRoom, SpatialAudioRoom secondRoom)
	{
		if (_firstRoomID.Equals(firstRoom.ID))
		{
			return SecondRoom.Equals(secondRoom.ID);
		}
		return false;
	}

	public bool Equals(RoomPair other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(_firstRoomID, other._firstRoomID))
		{
			return object.Equals(SecondRoom, other.SecondRoom);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (this != obj)
		{
			if (obj is RoomPair other)
			{
				return Equals(other);
			}
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		_ = _firstRoomID;
		int num = _firstRoomID.GetHashCode() * 397;
		_ = SecondRoom;
		return num ^ SecondRoom.GetHashCode();
	}
}
