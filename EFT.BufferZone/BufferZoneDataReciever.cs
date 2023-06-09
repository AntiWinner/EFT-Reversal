using UnityEngine;
using UnityEngine.Networking;

namespace EFT.BufferZone;

public class BufferZoneDataReciever : MonoBehaviour
{
	public void ParseBufferZoneData(byte[] data)
	{
		NetworkReader networkReader = new NetworkReader(data);
		EBufferZoneData eBufferZoneData = (EBufferZoneData)networkReader.ReadByte();
		switch (eBufferZoneData)
		{
		case EBufferZoneData.Availability:
		case EBufferZoneData.DisableByZryachiyDead:
		case EBufferZoneData.DisableByPlayerDead:
		{
			bool isZoneAvaliable = networkReader.ReadBoolean();
			_EBEB.Instance.SetInnerZoneAvailabilityStatus(isZoneAvaliable, eBufferZoneData);
			break;
		}
		case EBufferZoneData.PlayerAccessStatus:
		{
			string profileID2 = networkReader.ReadString();
			bool status = networkReader.ReadBoolean();
			_EBEB.Instance.SetPlayerAccessStatus(profileID2, status);
			break;
		}
		case EBufferZoneData.PlayerInZoneStatusChange:
		{
			string profileID = networkReader.ReadString();
			bool inZone = networkReader.ReadBoolean();
			_EBEB.Instance.SetPlayerInZoneStatus(profileID, inZone);
			break;
		}
		}
	}
}
