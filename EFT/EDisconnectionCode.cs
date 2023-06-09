namespace EFT;

public enum EDisconnectionCode
{
	Unknown,
	SABER_ANTICHEAT_AnticheatConnectionFailed,
	BATTLEYE_ANTICHEAT_ClientNotResponding,
	BATTLEYE_ANTICHEAT_QueryTimeout,
	BATTLEYE_ANTICHEAT_GameRestartRequired,
	BATTLEYE_ANTICHEAT_BadServiceVersion,
	BATTLEYE_ANTICHEAT_DisallowedProgram,
	BATTLEYE_ANTICHEAT_CorruptedMemory,
	BATTLEYE_ANTICHEAT_CorruptedData,
	BATTLEYE_ANTICHEAT_WinAPIFailure,
	BATTLEYE_ANTICHEAT_GlobalBan,
	BAD_RTT,
	HIGH_PACKETS_LOSS,
	TIME_MISMATCH
}
