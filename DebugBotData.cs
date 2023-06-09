using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT;
using UnityEngine;

public class DebugBotData : ScriptableObject
{
	private class _E000
	{
		public BotZone Zone;

		public int PosibleSlots;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public BotZone botZone;

		internal bool _E000(_E000 x)
		{
			return x.Zone == botZone;
		}
	}

	private const string m__E000 = "DebugBotData.txt";

	private const float m__E001 = 1f;

	private static DebugBotData _E002;

	private static bool _E003;

	[SerializeField]
	public List<WildSpawnWave> StartWaves = new List<WildSpawnWave>();

	[SerializeField]
	public List<BossLocationSpawn> BossStartWaves = new List<BossLocationSpawn>();

	public GizmosConfig Gizmos;

	public bool autoRespawn;

	public bool evenlyDistribute;

	public bool spawnInstantly;

	public bool localProfiles;

	public bool AnySideBornUse;

	public bool DebugBrain;

	public DebugBotDesition DebugBotDesition;

	public DebugBotTactic DebugBotTactic;

	public bool BadShooting;

	public bool UseDebugWawes;

	public int respawnCount;

	public int MaxBotsCount;

	public int SummonSavages;

	public bool BadVision;

	public bool BadHearing;

	public bool NoShoot;

	public bool BadShootingAsCount;

	public bool MaxAgression;

	public bool NoMinMax;

	public bool DebugCoverLogs;

	public bool NoReciol;

	public bool FreeForAll;

	public bool FreeForAllOverride;

	public bool DrawColorCanShoot;

	public bool DrawColorLinesWeight;

	public bool NoArmorToPower;

	public bool GoodShooting;

	public bool NoSleepMode;

	public bool LoadSettingsFromCode;

	public bool DebugMalfunctions;

	public bool AlwaysSprint;

	public bool SprintOverride;

	public float SprintSpeed = 1f;

	public bool useThisData;

	[HideInInspector]
	public List<WildSpawnWave> waves = new List<WildSpawnWave>();

	[HideInInspector]
	public List<BossLocationSpawn> bossWaves = new List<BossLocationSpawn>();

	public bool NoZoneBlocks;

	public bool NoRoleBlocks = true;

	public bool NoGrenadeOffset;

	public bool NoGrenadeThrow;

	public bool TrueAim;

	public DebugLook DebugRaysSystem;

	public bool NoStopServer;

	public bool AlwaysLayWhenDanger;

	public bool IgnoreBotLimits;

	public bool ActivatedStatus = true;

	private _E628 _E004;

	private _E263 _E005;

	private float _E006;

	public float BadShootOffset => 3f;

	public static DebugBotData Instance => _E002 ?? (_E002 = Load());

	public static bool UseDebugData
	{
		get
		{
			if (_E003)
			{
				return false;
			}
			DebugBotData instance = Instance;
			int num;
			if (instance != null)
			{
				num = (instance.useThisData ? 1 : 0);
				if (num != 0)
				{
					goto IL_0032;
				}
			}
			else
			{
				num = 0;
			}
			_E003 = true;
			goto IL_0032;
			IL_0032:
			return (byte)num != 0;
		}
	}

	public static DebugBotData Load()
	{
		try
		{
			if (File.Exists(_ED3E._E000(26353)))
			{
				_E105 data = JsonUtility.FromJson<_E105>(File.ReadAllText(_ED3E._E000(26353)));
				DebugBotData debugBotData = ScriptableObject.CreateInstance<DebugBotData>();
				debugBotData.SetData(data);
				return debugBotData;
			}
		}
		catch (Exception ex)
		{
			Debug.Log(_ED3E._E000(26338) + ex);
		}
		return null;
	}

	public void StartUseAutoRespawn(_E628 botSpawner, _E2F5 bots, _E263 groups)
	{
		_E004 = botSpawner;
		_E005 = groups;
	}

	public void InitMessage()
	{
		if (useThisData)
		{
			string message = _ED3E._E000(26387);
			Debug.LogWarning(message);
			Debug.LogError(message);
			Debug.Log(message);
		}
	}

	public void Init(BotZone[] botZones = null)
	{
		if (!useThisData)
		{
			return;
		}
		waves = new List<WildSpawnWave>();
		bossWaves = new List<BossLocationSpawn>();
		if (autoRespawn)
		{
			foreach (WildSpawnWave startWave in StartWaves)
			{
				WildSpawnWave wildSpawnWave = startWave.Copy();
				waves.Add(wildSpawnWave);
				wildSpawnWave.slots_min = respawnCount;
				wildSpawnWave.slots_max = respawnCount;
			}
			StaticManager.Instance.StaticUpdate += _E001;
		}
		else
		{
			foreach (WildSpawnWave startWave2 in StartWaves)
			{
				WildSpawnWave item = startWave2.Copy();
				waves.Add(item);
			}
			foreach (BossLocationSpawn bossStartWave in BossStartWaves)
			{
				BossLocationSpawn item2 = bossStartWave.Copy();
				bossWaves.Add(item2);
			}
		}
		if (!evenlyDistribute)
		{
			return;
		}
		if (botZones != null && botZones.Length != 0)
		{
			List<_E000> list = new List<_E000>();
			foreach (BotZone botZone in botZones)
			{
				_E000 item3 = new _E000
				{
					Zone = botZone,
					PosibleSlots = botZone.MaxPersonsOnPatrol
				};
				list.Add(item3);
			}
			waves = _E000(list, waves);
		}
		else
		{
			Debug.LogError(_ED3E._E000(24873));
		}
	}

	public void Serialize(string path = "DebugBotData.txt")
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		File.Create(path).Dispose();
		string value = JsonUtility.ToJson(new _E105(this), prettyPrint: true);
		StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.Write(value);
		streamWriter.Flush();
		streamWriter.Close();
	}

	public void DebugActivateAllBots(bool val = true)
	{
		_E620 obj = _E620.FindBotControllerEditorOnly();
		ActivatedStatus = val;
		if (obj == null)
		{
			return;
		}
		foreach (BotOwner botOwner in obj.Bots.BotOwners)
		{
			botOwner.StandBy.DebugActivate(ActivatedStatus);
		}
	}

	public void SetData(_E105 result)
	{
		autoRespawn = result.autoRespawn;
		respawnCount = result.respawnCount;
		StartWaves = result.StartWaves;
		BossStartWaves = result.BossStartWaves;
		useThisData = result.useThisData;
		evenlyDistribute = result.evenlyDistribute;
		UseDebugWawes = result.useStartWaves;
		BadHearing = result.badHearing;
		BadVision = result.badVision;
		spawnInstantly = result.spawnInstantly;
		NoMinMax = result.NoMinMax;
		NoSleepMode = result.NoSleepMode;
		TrueAim = result.TrueAim;
		FreeForAllOverride = result.FreeForAllOverride;
		FreeForAll = result.FreeForAll;
		DebugBrain = result.UseDebugBrain;
		DebugBotDesition = result.DebugBotDesition;
		SummonSavages = result.SummonSavages;
	}

	private List<WildSpawnWave> _E000(List<_E000> zonesSpace, List<WildSpawnWave> wavesToCheck)
	{
		List<WildSpawnWave> list = new List<WildSpawnWave>();
		int num = zonesSpace.Sum((_E000 x) => x.PosibleSlots);
		foreach (WildSpawnWave item in wavesToCheck)
		{
			int slots_max = item.slots_max;
			_E000 obj = null;
			foreach (_E000 item2 in zonesSpace)
			{
				if (item2.PosibleSlots > slots_max)
				{
					obj = item2;
					break;
				}
			}
			if (obj != null)
			{
				obj.PosibleSlots -= slots_max;
				item.SpawnPoints = obj.Zone.name;
				list.Add(item);
				continue;
			}
			int num2 = item.slots_max;
			List<BotZone> list2 = new List<BotZone>();
			foreach (_E000 item3 in zonesSpace)
			{
				if (num2 == 0)
				{
					break;
				}
				WildSpawnWave wildSpawnWave = item.Copy();
				list.Add(wildSpawnWave);
				wildSpawnWave.SpawnPoints = item3.Zone.name;
				if (item3.PosibleSlots > num2)
				{
					wildSpawnWave.slots_min = (wildSpawnWave.slots_max = num2);
					item3.PosibleSlots -= num2;
					num2 = 0;
					break;
				}
				num2 -= item3.PosibleSlots;
				wildSpawnWave.slots_max = (wildSpawnWave.slots_min = item3.PosibleSlots);
				item3.PosibleSlots = 0;
				list2.Add(item3.Zone);
			}
			foreach (BotZone botZone in list2)
			{
				zonesSpace.RemoveAll((_E000 x) => x.Zone == botZone);
			}
			if (num2 > 0)
			{
				for (int i = 0; i < num2; i++)
				{
					WildSpawnWave wildSpawnWave2 = list.RandomElement();
					wildSpawnWave2.slots_min = ++wildSpawnWave2.slots_max;
				}
				int num3 = wavesToCheck.Sum((WildSpawnWave x) => x.slots_max);
				Debug.LogError(string.Format(_ED3E._E000(24914), num, num3) + string.Format(_ED3E._E000(25066), num2));
			}
		}
		return list;
	}

	private void _E001()
	{
		if (!(_E006 > Time.time))
		{
			return;
		}
		_E006 = 1f + Time.time;
		foreach (KeyValuePair<BotZone, _E262> item in _E005)
		{
			foreach (_E267 item2 in item.Value.GetAllGroupsDebug())
			{
				if (item2 != null && item2.MembersCount < respawnCount && item2.MembersCount > 0)
				{
					_E004._E004(item2.Side, item.Key);
				}
			}
		}
	}
}
