using System;
using Newtonsoft.Json;

namespace EFT.Bots;

[Serializable]
public struct BotControllerSettings
{
	[JsonIgnore]
	public bool IsEnabled;

	[JsonProperty("isScavWars")]
	public bool IsScavWars;

	[JsonProperty("botAmount")]
	public EBotAmount BotAmount;

	[JsonIgnore]
	public EBossType BossType;

	public BotControllerSettings(bool isEnabled, bool isScavWars, EBotAmount botAmount = EBotAmount.AsOnline, EBossType bossType = EBossType.AsOnline)
	{
		IsEnabled = isEnabled;
		IsScavWars = isScavWars;
		BotAmount = botAmount;
		BossType = bossType;
	}
}
