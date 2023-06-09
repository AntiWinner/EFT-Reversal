using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT;

public sealed class WavesSpawnScenario : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public WildSpawnType wildSpawnType;

		internal bool _E000(MinMaxBots x)
		{
			return x.WildSpawnType == wildSpawnType;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public WildSpawnType type;

		internal int _E000(WildSpawnWave x)
		{
			if (x.WildSpawnType != type)
			{
				return 0;
			}
			return x.slots_min;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _E625 spawnWave;

		public WavesSpawnScenario _003C_003E4__this;
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _E2BB._E000 timer;

		public _E002 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000.Remove(timer);
			CS_0024_003C_003E8__locals1._003C_003E4__this.m__E001(CS_0024_003C_003E8__locals1.spawnWave);
		}
	}

	public BotLocationModifier BotLocationModifier;

	private readonly List<_E2BB._E000> m__E000 = new List<_E2BB._E000>();

	private Action<_E625> m__E001;

	private Dictionary<WildSpawnType, int> m__E002 = new Dictionary<WildSpawnType, int>();

	public readonly List<_E624> BotsCountProfiles = new List<_E624>();

	[CompilerGenerated]
	private _E625[] m__E003;

	[CompilerGenerated]
	private bool _E004;

	public _E625[] SpawnWaves
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	public bool Enabled
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		private set
		{
			_E004 = value;
		}
	}

	internal static WavesSpawnScenario _E000(GameObject game, WildSpawnWave[] waves, Action<_E625> spawnAction, _E554.Location location = null)
	{
		MinMaxBots[] minMaxBots = ((location != null) ? location.MinMaxBots : new MinMaxBots[0]);
		BotLocationModifier botLocationModifier = ((location != null) ? location.BotLocationModifier : new BotLocationModifier());
		WavesSpawnScenario wavesSpawnScenario = game.gameObject.AddComponent<WavesSpawnScenario>();
		_E001(wavesSpawnScenario.m__E002, minMaxBots);
		bool flag = location?.OldSpawn ?? false;
		if (DebugBotData.UseDebugData && DebugBotData.Instance.waves.Count > 0)
		{
			flag = true;
		}
		if (flag)
		{
			wavesSpawnScenario.Init(waves);
		}
		wavesSpawnScenario.m__E001 = spawnAction;
		wavesSpawnScenario.BotLocationModifier = botLocationModifier;
		if (waves.Sum((WildSpawnWave x) => x.slots_max) == 0)
		{
			_ = waves.LongLength;
		}
		return wavesSpawnScenario;
	}

	private static void _E001(Dictionary<WildSpawnType, int> minCounts, MinMaxBots[] minMaxBots)
	{
		WildSpawnType[] allTypes = _E620.AllTypes;
		foreach (WildSpawnType wildSpawnType in allTypes)
		{
			if (minMaxBots == null)
			{
				minCounts.Add(wildSpawnType, 0);
				continue;
			}
			MinMaxBots minMaxBots2 = minMaxBots.FirstOrDefault((MinMaxBots x) => x.WildSpawnType == wildSpawnType);
			if (minMaxBots2 == null)
			{
				minCounts.Add(wildSpawnType, 0);
			}
			else
			{
				minCounts.Add(wildSpawnType, minMaxBots2.min);
			}
		}
	}

	public void Init(WildSpawnWave[] waves)
	{
		Enabled = true;
		foreach (WildSpawnWave wildSpawnWave in waves)
		{
			if (wildSpawnWave.WildSpawnType != WildSpawnType.marksman && wildSpawnWave.WildSpawnType != WildSpawnType.assault)
			{
				_ = wildSpawnWave.WildSpawnType;
				_ = 1024;
			}
		}
		_E002(waves, WildSpawnType.marksman);
		_E002(waves, WildSpawnType.assault);
		SpawnWaves = ((waves == null) ? new _E625[0] : (from wave in waves.Select(delegate(WildSpawnWave wave)
			{
				int botsCount = _E39D.RandomInclude(wave.slots_min, wave.slots_max);
				_E625 obj = new _E625
				{
					Time = UnityEngine.Random.Range(wave.time_min, wave.time_max),
					BotsCount = botsCount,
					Difficulty = wave.GetDifficulty(),
					WildSpawnType = wave.WildSpawnType,
					SpawnAreaName = wave.SpawnPoints,
					Side = wave.BotSide,
					IsPlayers = wave.isPlayers,
					ChanceGroup = wave.ChanceGroup
				};
				_E624 item = new _E624(obj.BotsCount, obj.WildSpawnType, obj.Difficulty);
				BotsCountProfiles.Add(item);
				return obj;
			})
			orderby wave.Time
			select wave).ToArray());
		_E625[] spawnWaves = SpawnWaves;
		for (int i = 0; i < spawnWaves.Length; i++)
		{
			_ = spawnWaves[i];
		}
	}

	private void _E002(WildSpawnWave[] waves, WildSpawnType type)
	{
		if (DebugBotData.UseDebugData && DebugBotData.Instance.NoMinMax)
		{
			return;
		}
		int num = waves.Sum((WildSpawnWave x) => (x.WildSpawnType == type) ? x.slots_min : 0);
		int num2 = this.m__E002[type];
		if (num >= num2)
		{
			return;
		}
		int num3 = num2 - num;
		if (waves.Length <= num3)
		{
			foreach (WildSpawnWave item in waves.RandomElement(num3))
			{
				item.slots_min++;
				if (item.slots_max < item.slots_min)
				{
					item.slots_max = item.slots_min;
				}
			}
			return;
		}
		int num4 = num3 / waves.Length;
		foreach (WildSpawnWave wildSpawnWave in waves)
		{
			wildSpawnWave.slots_min += num4;
			if (wildSpawnWave.slots_max < wildSpawnWave.slots_min)
			{
				wildSpawnWave.slots_max = wildSpawnWave.slots_min;
			}
		}
	}

	public void Run(EBotsSpawnMode spawnMode = EBotsSpawnMode.Anyway)
	{
		if (!Enabled)
		{
			return;
		}
		_E625[] spawnWaves = SpawnWaves;
		foreach (_E625 spawnWave in spawnWaves)
		{
			switch (spawnMode)
			{
			case EBotsSpawnMode.AfterGameStarted:
				if (spawnWave.Time < 0f)
				{
					continue;
				}
				break;
			case EBotsSpawnMode.BeforeGameStarted:
				if (spawnWave.Time < 0f)
				{
					this.m__E001(spawnWave);
				}
				continue;
			}
			_E2BB._E000 timer = StaticManager.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(spawnWave.Time));
			this.m__E000.Add(timer);
			timer.OnTimer += delegate
			{
				this.m__E000.Remove(timer);
				this.m__E001(spawnWave);
			};
		}
	}

	public void Stop()
	{
		foreach (_E2BB._E000 item in this.m__E000)
		{
			item.Stop();
		}
	}

	[CompilerGenerated]
	private _E625 _E003(WildSpawnWave wave)
	{
		int botsCount = _E39D.RandomInclude(wave.slots_min, wave.slots_max);
		_E625 obj = new _E625
		{
			Time = UnityEngine.Random.Range(wave.time_min, wave.time_max),
			BotsCount = botsCount,
			Difficulty = wave.GetDifficulty(),
			WildSpawnType = wave.WildSpawnType,
			SpawnAreaName = wave.SpawnPoints,
			Side = wave.BotSide,
			IsPlayers = wave.isPlayers,
			ChanceGroup = wave.ChanceGroup
		};
		_E624 item = new _E624(obj.BotsCount, obj.WildSpawnType, obj.Difficulty);
		BotsCountProfiles.Add(item);
		return obj;
	}
}
