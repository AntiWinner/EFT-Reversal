using System;
using UnityEngine;

namespace Audio.SpatialSystem;

[Serializable]
public sealed class AudioRoutesBakeData : ScriptableObject
{
	[Serializable]
	public struct RoomPairItem
	{
		public uint Key;

		public RoomPair Value;
	}

	[Serializable]
	public struct RoutesAwareRoomItem
	{
		public int Key;

		public uint[] Value;
	}

	public RoomPairItem[] RoomPairsItems;

	public RoutesAwareRoomItem[] RoutesAwareRoomItems;

	public uint MaxPropagationDepth;
}
