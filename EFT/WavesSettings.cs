using System;
using EFT.Bots;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public struct WavesSettings
{
	[JsonProperty("botAmount")]
	public EBotAmount BotAmount;

	[JsonProperty("botDifficulty")]
	public EBotDifficulty BotDifficulty;

	[JsonProperty("isBosses")]
	public bool IsBosses;

	[JsonProperty("isTaggedAndCursed")]
	public bool IsTaggedAndCursed;

	public WavesSettings(EBotAmount botAmount, EBotDifficulty botDifficulty, bool isBosses, bool isTaggedAndCursed)
	{
		BotAmount = botAmount;
		BotDifficulty = botDifficulty;
		IsBosses = isBosses;
		IsTaggedAndCursed = isTaggedAndCursed;
	}
}
