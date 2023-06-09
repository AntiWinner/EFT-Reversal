using System;
using EFT.Bots;
using JsonType;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public sealed class RaidSettings
{
	[JsonProperty("keyId")]
	public string KeyId;

	[JsonProperty("side")]
	public ESideType Side;

	[JsonProperty("location")]
	public string LocationId;

	[JsonProperty("timeVariant")]
	public EDateTime SelectedDateTime;

	[JsonProperty("raidMode")]
	public ERaidMode RaidMode;

	[JsonProperty("metabolismDisabled")]
	public bool MetabolismDisabled;

	[JsonProperty("playersSpawnPlace")]
	public EPlayersSpawnPlace PlayersSpawnPlace;

	[JsonProperty("timeAndWeatherSettings")]
	public TimeAndWeatherSettings TimeAndWeatherSettings;

	[JsonProperty("botSettings")]
	public BotControllerSettings BotSettings;

	[JsonProperty("wavesSettings")]
	public WavesSettings WavesSettings;

	[JsonIgnore]
	private _E554.Location _selectedLocation;

	[JsonIgnore]
	public _E554.Location SelectedLocation
	{
		get
		{
			return _selectedLocation;
		}
		set
		{
			_selectedLocation = value;
			LocationId = _selectedLocation?.Id;
		}
	}

	[JsonIgnore]
	public bool Local => RaidMode == ERaidMode.Local;

	[JsonIgnore]
	public bool IsPmc => Side == ESideType.Pmc;

	[JsonIgnore]
	public bool IsScav => Side == ESideType.Savage;

	public void Apply(RaidSettings raidSettings)
	{
		Side = raidSettings.Side;
		SelectedLocation = raidSettings.SelectedLocation;
		SelectedDateTime = raidSettings.SelectedDateTime;
		KeyId = raidSettings.KeyId;
		RaidMode = raidSettings.RaidMode;
		MetabolismDisabled = raidSettings.MetabolismDisabled;
		PlayersSpawnPlace = raidSettings.PlayersSpawnPlace;
		TimeAndWeatherSettings = raidSettings.TimeAndWeatherSettings;
		BotSettings = raidSettings.BotSettings;
		WavesSettings = raidSettings.WavesSettings;
	}

	public RaidSettings Clone()
	{
		return new RaidSettings
		{
			Side = Side,
			SelectedLocation = SelectedLocation,
			SelectedDateTime = SelectedDateTime,
			KeyId = KeyId,
			RaidMode = RaidMode,
			MetabolismDisabled = MetabolismDisabled,
			PlayersSpawnPlace = PlayersSpawnPlace,
			TimeAndWeatherSettings = TimeAndWeatherSettings,
			BotSettings = BotSettings,
			WavesSettings = WavesSettings
		};
	}
}
