using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Bots;
using EFT.UI;
using EFT.Weather;
using JetBrains.Annotations;
using JsonType;
using UnityEngine;

namespace EFT;

internal sealed class LocalGame : BaseLocalGame<GamePlayerOwner>, _E2FA
{
	[CompilerGenerated]
	private new sealed class _E000
	{
		public LocalGame game;

		internal void _E000(_E625 wave)
		{
			((BaseLocalGame<GamePlayerOwner>)game)._E017.ActivateBotsByWave(wave);
		}

		internal void _E001(BossLocationSpawn bossWave)
		{
			((BaseLocalGame<GamePlayerOwner>)game)._E017.ActivateBotsByWave(bossWave);
		}
	}

	private new _E2F0 m__E000;

	private new WavesSpawnScenario m__E001;

	private new NonWavesSpawnScenario m__E002;

	private readonly Dictionary<string, Player> m__E003 = new Dictionary<string, Player>();

	_E620 _E2FA.BotsController => base._E017;

	public IWeatherCurve WeatherCurve => WeatherController.Instance.WeatherCurve;

	internal new static LocalGame _E000(_E7FD inputTree, Profile profile, _E629 backendDateTime, _ECB1 insurance, MenuUI menuUI, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E554.Location location, TimeAndWeatherSettings timeAndWeather, WavesSettings wavesSettings, EDateTime dateTime, Callback<ExitStatus, TimeSpan, _E907> callback, float fixedDeltaTime, EUpdateQueue updateQueue, _E796 backEndSession, TimeSpan sessionTime)
	{
		Singleton<_E7DE>.Instance.Sound.Controller.VoipDisabledInLocalGame();
		LocalGame game = BaseLocalGame<GamePlayerOwner>._E000<LocalGame>(inputTree, profile, backendDateTime, insurance, menuUI, commonUI, preloaderUI, gameUI, location, timeAndWeather, wavesSettings, dateTime, callback, fixedDeltaTime, updateQueue, backEndSession, sessionTime);
		WildSpawnWave[] waves = _E001(wavesSettings, location.waves);
		game.m__E002 = NonWavesSpawnScenario._E000(game, location, ((BaseLocalGame<GamePlayerOwner>)game)._E017);
		game.m__E001 = WavesSpawnScenario._E000(game.gameObject, waves, delegate(_E625 wave)
		{
			((BaseLocalGame<GamePlayerOwner>)game)._E017.ActivateBotsByWave(wave);
		}, location);
		BossLocationSpawn[] bossWaves = _E002(wavesSettings, location.BossLocationSpawn);
		game.m__E000 = _E2F0._E000(bossWaves, delegate(BossLocationSpawn bossWave)
		{
			((BaseLocalGame<GamePlayerOwner>)game)._E017.ActivateBotsByWave(bossWave);
		});
		return game;
	}

	protected override IEnumerator _E01C(float startDelay, BotControllerSettings controllerSettings, _EBCC spawnSystem, Callback runCallback)
	{
		_E2FD profileCreator = new _E2FD(_E008, this.m__E001.SpawnWaves, this.m__E000.BossSpawnWaves, this.m__E002._E00B);
		_E3D1 botCreator = new _E3D1(this, profileCreator, _E003);
		BotZone[] botZones = LocationScene.GetAllObjects<BotZone>().ToArray();
		bool enableWaveControl = controllerSettings.BotAmount == EBotAmount.Horde;
		base._E017.Init(this, botCreator, botZones, spawnSystem, this.m__E001.BotLocationModifier, controllerSettings.IsEnabled, controllerSettings.IsScavWars, enableWaveControl, online: false, this.m__E000.HaveSectants, Singleton<GameWorld>.Instance, base._E002.OpenZones);
		int maxCount = controllerSettings.BotAmount switch
		{
			EBotAmount.Low => 15, 
			EBotAmount.Medium => 20, 
			EBotAmount.High => 25, 
			EBotAmount.Horde => 35, 
			EBotAmount.AsOnline => 20, 
			_ => 15, 
		};
		base._E017.SetSettings(maxCount, _E008.BackEndConfig.BotPresets, _E008.BackEndConfig.BotWeaponScatterings);
		base._E017.AddActivePLayer(base._E00E.Player);
		yield return new WaitForSeconds(startDelay);
		if (!base._E002.NewSpawn)
		{
			if (this.m__E001.SpawnWaves != null && this.m__E001.SpawnWaves.Length != 0)
			{
				this.m__E001.Run();
			}
			else
			{
				this.m__E002.Run();
			}
		}
		this.m__E000.Run();
		yield return base._E01C(startDelay, controllerSettings, spawnSystem, runCallback);
	}

	private new static WildSpawnWave[] _E001(WavesSettings wavesSettings, WildSpawnWave[] waves)
	{
		foreach (WildSpawnWave wildSpawnWave in waves)
		{
			wildSpawnWave.slots_min = (wildSpawnWave.slots_max = wavesSettings.BotAmount.ToBotAmountSlots(wildSpawnWave.slots_min, wildSpawnWave.slots_max));
			if (wavesSettings.IsTaggedAndCursed && wildSpawnWave.WildSpawnType == WildSpawnType.assault)
			{
				wildSpawnWave.WildSpawnType = WildSpawnType.cursedAssault;
			}
			if (wavesSettings.IsBosses)
			{
				wildSpawnWave.time_min += 35;
				wildSpawnWave.time_max += 35;
			}
			wildSpawnWave.BotDifficulty = wavesSettings.BotDifficulty.ToBotDifficulty();
		}
		return waves;
	}

	private new static BossLocationSpawn[] _E002(WavesSettings wavesSettings, BossLocationSpawn[] bossLocationSpawn)
	{
		if (!wavesSettings.IsBosses)
		{
			return new BossLocationSpawn[0];
		}
		foreach (BossLocationSpawn bossLocationSpawn2 in bossLocationSpawn)
		{
			List<int> source;
			try
			{
				source = bossLocationSpawn2.BossEscortAmount.Split(',').Select(int.Parse).ToList();
				bossLocationSpawn2.ParseMainTypesTypes();
			}
			catch (Exception)
			{
				continue;
			}
			float bossChance = ((bossLocationSpawn2.BossChance > 0f) ? 100f : (-1f));
			if (bossLocationSpawn2.BossType == WildSpawnType.sectantPriest || bossLocationSpawn2.BossType == WildSpawnType.sectantWarrior || bossLocationSpawn2.BossType == WildSpawnType.bossZryachiy || bossLocationSpawn2.BossType == WildSpawnType.followerZryachiy)
			{
				bossChance = -1f;
			}
			bossLocationSpawn2.BossChance = bossChance;
			switch (wavesSettings.BotAmount)
			{
			case EBotAmount.Low:
				bossLocationSpawn2.BossEscortAmount = source.Min((int x) => x).ToString();
				break;
			case EBotAmount.Medium:
			{
				int num = source.Max((int x) => x);
				int num2 = source.Min((int x) => x);
				bossLocationSpawn2.BossEscortAmount = ((num - num2) / 2).ToString();
				break;
			}
			case EBotAmount.High:
			case EBotAmount.Horde:
				bossLocationSpawn2.BossEscortAmount = source.Max((int x) => x).ToString();
				break;
			}
		}
		return bossLocationSpawn;
	}

	[CanBeNull]
	private async Task<LocalPlayer> _E003(Profile profile, Vector3 position)
	{
		if (!base.Status.IsRunned())
		{
			return null;
		}
		if (this.m__E003.ContainsKey(profile.Id))
		{
			_E014?.LogError(_ED3E._E000(180285) + profile.Id + _ED3E._E000(180312) + string.Join(_ED3E._E000(10270), this.m__E003.Keys));
			return null;
		}
		int playerId = _E00D();
		Player.EUpdateMode armsUpdateMode = Player.EUpdateMode.Auto;
		profile.SetSpawnedInSession(profile.Info.Side == EPlayerSide.Savage);
		LocalPlayer localPlayer = await LocalPlayer.Create(playerId, position, Quaternion.identity, _ED3E._E000(60679), "", EPointOfView.ThirdPerson, profile, aiControl: true, base.UpdateQueue, armsUpdateMode, Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.BotPlayerMode, () => 1f, () => 1f, new _E757(), _E611.Default, null, isYourPlayer: false);
		localPlayer.Location = base._E002.Id;
		if (this.m__E003.ContainsKey(localPlayer.ProfileId))
		{
			_E014?.LogError(_ED3E._E000(180311) + localPlayer.ProfileId + _ED3E._E000(180312) + string.Join(_ED3E._E000(10270), this.m__E003.Keys));
		}
		else
		{
			this.m__E003.Add(localPlayer.ProfileId, localPlayer);
		}
		return localPlayer;
	}

	protected override void _E017()
	{
		_E014 = new _E626(LoggerMode.None, _E013, this.m__E003);
	}

	protected override void Stop(string profileId, ExitStatus exitStatus, string exitName, float delay = 0f)
	{
		this.m__E000.Stop();
		this.m__E002.Stop();
		this.m__E001.Stop();
		base.Stop(profileId, exitStatus, exitName, delay);
	}

	public override void CleanUp()
	{
		base.CleanUp();
		BaseLocalGame<GamePlayerOwner>._E00C(this.m__E003);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _E004(float startDelay, BotControllerSettings controllerSettings, _EBCC spawnSystem, Callback runCallback)
	{
		return base._E01C(startDelay, controllerSettings, spawnSystem, runCallback);
	}
}
