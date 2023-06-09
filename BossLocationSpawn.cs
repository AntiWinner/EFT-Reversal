using System;
using System.Collections.Generic;
using EFT;
using UnityEngine;

[Serializable]
public class BossLocationSpawn
{
	public string BossName = "";

	public float BossChance = -1f;

	public string BossZone = "";

	public bool BossPlayer;

	public string BossDifficult;

	public string BossEscortDifficult;

	public string BossEscortType = "";

	public string BossEscortAmount = "";

	public float Time;

	public float Delay;

	public string TriggerId = "";

	public string TriggerName = "";

	public bool IgnoreMaxBots;

	public bool ForceSpawn;

	public WildSpawnSupports[] Supports;

	public _E2BB._E000 TimerForDelay;

	private List<BossLocationSpawnSubData> _subDatas;

	private List<BotZone> _possibleShuffledZones = new List<BotZone>();

	public WildSpawnType BossType { get; private set; }

	public WildSpawnType EscortType { get; private set; }

	public int EscortCount { get; private set; }

	public string BornZone { get; private set; }

	public bool ShallSpawn { get; private set; }

	public BotDifficulty BossDif { get; private set; } = BotDifficulty.normal;


	public BotDifficulty EscortDif { get; private set; } = BotDifficulty.normal;


	public SpawnTriggerType TriggerType { get; private set; }

	public bool Activated { get; set; }

	public bool IsStartWave()
	{
		if (Time < 0f)
		{
			return TriggerType == SpawnTriggerType.none;
		}
		return false;
	}

	public void ParseMainTypesTypes()
	{
		ShallSpawn = _E39D.IsTrue100(BossChance);
		Activated = false;
		if (!string.IsNullOrEmpty(TriggerName))
		{
			TriggerType = (SpawnTriggerType)Enum.Parse(typeof(SpawnTriggerType), TriggerName);
		}
		BossType = (WildSpawnType)Enum.Parse(typeof(WildSpawnType), BossName);
		EscortType = (WildSpawnType)Enum.Parse(typeof(WildSpawnType), BossEscortType);
		BossDif = (BotDifficulty)Enum.Parse(typeof(BotDifficulty), BossDifficult);
		EscortDif = (BotDifficulty)Enum.Parse(typeof(BotDifficulty), BossEscortDifficult);
	}

	public void Init()
	{
		ShallSpawn = _E39D.IsTrue100(BossChance);
		Activated = false;
		ParseMainTypesTypes();
		string[] array = BossEscortAmount.Split(',');
		List<int> list = new List<int>();
		string[] array2 = array;
		foreach (string s in array2)
		{
			list.Add(int.Parse(s));
		}
		int num = list.RandomElement();
		BornZone = BossZone.Split(',').RandomElement();
		if (Supports != null && Supports.Length != 0)
		{
			_subDatas = new List<BossLocationSpawnSubData>();
			WildSpawnSupports[] supports = Supports;
			foreach (WildSpawnSupports wildSpawnSupports in supports)
			{
				BotDifficulty difficulty = (BotDifficulty)Enum.Parse(typeof(BotDifficulty), wildSpawnSupports.BossEscortDifficult.RandomElement());
				_subDatas.Add(new BossLocationSpawnSubData(wildSpawnSupports.BossEscortAmount, wildSpawnSupports.BossEscortType, difficulty));
			}
		}
		if (_subDatas != null)
		{
			foreach (BossLocationSpawnSubData subData in _subDatas)
			{
				num += subData.BossEscortAmount;
			}
		}
		EscortCount = num;
	}

	public List<BossLocationSpawnSubData> GetEscors()
	{
		return _subDatas;
	}

	public BossLocationSpawn Copy()
	{
		BossLocationSpawn bossLocationSpawn = new BossLocationSpawn
		{
			BossName = BossName,
			BossChance = BossChance,
			BossZone = BossZone,
			BossPlayer = BossPlayer,
			BossDifficult = BossDifficult,
			BossEscortDifficult = BossEscortDifficult,
			BossEscortType = BossEscortType,
			BossEscortAmount = BossEscortAmount,
			TriggerName = TriggerName,
			TriggerId = TriggerId,
			Time = Time,
			Delay = Delay,
			IgnoreMaxBots = IgnoreMaxBots,
			ForceSpawn = ForceSpawn
		};
		if (Supports != null && Supports.Length != 0)
		{
			WildSpawnSupports[] array = new WildSpawnSupports[Supports.Length];
			for (int i = 0; i < Supports.Length; i++)
			{
				WildSpawnSupports wildSpawnSupports = Supports[i];
				array[i] = wildSpawnSupports.Copy();
			}
			bossLocationSpawn.Supports = array;
		}
		return bossLocationSpawn;
	}

	public List<_E624> GetUsingTypes()
	{
		List<_E624> list = new List<_E624>();
		list.Add(new _E624(1, BossType, BossDif));
		if (_subDatas != null)
		{
			foreach (BossLocationSpawnSubData subData in _subDatas)
			{
				list.Add(subData.GetTypesBotWave());
			}
			return list;
		}
		int num = EscortCount;
		if (EscortType == WildSpawnType.followerZryachiy)
		{
			num += 3;
		}
		list.Add(new _E624(num, EscortType, EscortDif));
		return list;
	}

	public string DebugInfo()
	{
		return string.Format(_ED3E._E000(13122), BossName, BossChance, BossZone, BossPlayer, BossDifficult, BossEscortDifficult, BossEscortType, BossEscortAmount);
	}

	public List<BotZone> GetPossibleZones(BotZone[] allZones, List<BotZone> markedBossZone)
	{
		if (_possibleShuffledZones.Count > 0)
		{
			return _possibleShuffledZones;
		}
		_possibleShuffledZones.Clear();
		if (BornZone.Length > 1)
		{
			string bornZone = BornZone;
			BotZone botZone = null;
			foreach (BotZone botZone2 in allZones)
			{
				if (botZone2.NameZone.Equals(bornZone))
				{
					botZone = botZone2;
				}
			}
			if (botZone == null)
			{
				Debug.LogError(_ED3E._E000(15380) + BornZone);
				_possibleShuffledZones = new List<BotZone> { markedBossZone.RandomElement() };
			}
			else
			{
				_possibleShuffledZones = new List<BotZone> { botZone };
			}
		}
		else
		{
			_possibleShuffledZones = _E39D.Shuffle(markedBossZone);
		}
		return _possibleShuffledZones;
	}
}
