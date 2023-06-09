using System;

namespace EFT;

[Serializable]
public class WildSpawnWave
{
	public int time_min;

	public int time_max;

	public int slots_min;

	public int slots_max;

	public string SpawnPoints;

	public EPlayerSide BotSide = EPlayerSide.Savage;

	public WildSpawnType WildSpawnType;

	public BotDifficulty _botDifficulty;

	public bool isPlayers;

	public string BotPreset = BotDifficulty.normal.ToString();

	public float ChanceGroup;

	public BotDifficulty BotDifficulty
	{
		get
		{
			return _botDifficulty;
		}
		set
		{
			_botDifficulty = value;
			BotPreset = _botDifficulty.ToString();
		}
	}

	public WildSpawnWave Copy()
	{
		return new WildSpawnWave
		{
			BotSide = BotSide,
			time_min = time_min,
			time_max = time_max,
			slots_min = slots_min,
			slots_max = slots_max,
			SpawnPoints = SpawnPoints,
			WildSpawnType = WildSpawnType,
			isPlayers = isPlayers,
			ChanceGroup = ChanceGroup
		};
	}

	public BotDifficulty GetDifficulty()
	{
		return (BotDifficulty)Enum.Parse(typeof(BotDifficulty), BotPreset);
	}
}
