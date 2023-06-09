using System;

namespace EFT;

[Serializable]
public class WildSpawnSupports
{
	public WildSpawnType BossEscortType;

	public int BossEscortAmount;

	public string[] BossEscortDifficult;

	public WildSpawnSupports Copy()
	{
		WildSpawnSupports wildSpawnSupports = new WildSpawnSupports();
		wildSpawnSupports.BossEscortType = BossEscortType;
		wildSpawnSupports.BossEscortAmount = BossEscortAmount;
		string[] array = new string[BossEscortDifficult.Length];
		for (int i = 0; i < BossEscortDifficult.Length; i++)
		{
			string text = BossEscortDifficult[i];
			array[i] = text;
		}
		wildSpawnSupports.BossEscortDifficult = array;
		return wildSpawnSupports;
	}
}
