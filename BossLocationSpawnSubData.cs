using EFT;
using UnityEngine;

public class BossLocationSpawnSubData : MonoBehaviour
{
	public int BossEscortAmount;

	public WildSpawnType BossEscortType;

	public BotDifficulty EscortDifficulty;

	public BossLocationSpawnSubData(int v, WildSpawnType escortType, BotDifficulty difficulty)
	{
		BossEscortAmount = v;
		BossEscortType = escortType;
		EscortDifficulty = difficulty;
	}

	public _E624 GetTypesBotWave()
	{
		return new _E624(BossEscortAmount, BossEscortType, EscortDifficulty);
	}
}
