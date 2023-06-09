using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT;

public sealed class NonWavesSpawnScenario : MonoBehaviour
{
	private AbstractGame m__E000;

	private _E554.Location m__E001;

	private _E620 _E002;

	private _E623<BotDifficulty> _E003;

	private _E623<WildSpawnType> _E004;

	private bool _E005;

	private bool _E006;

	private float? _E007;

	private float _E008;

	[CompilerGenerated]
	private _E624[] _E009;

	[CompilerGenerated]
	private bool _E00A;

	internal _E624[] _E00B
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		private set
		{
			_E009 = value;
		}
	}

	public bool Enabled
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		private set
		{
			_E00A = value;
		}
	}

	internal static NonWavesSpawnScenario _E000(AbstractGame game, _E554.Location location, _E620 botsController)
	{
		NonWavesSpawnScenario nonWavesSpawnScenario = game.gameObject.AddComponent<NonWavesSpawnScenario>();
		nonWavesSpawnScenario.m__E000 = game;
		nonWavesSpawnScenario.m__E001 = location;
		nonWavesSpawnScenario._E002 = botsController;
		if (location.NewSpawn)
		{
			nonWavesSpawnScenario._E001();
		}
		return nonWavesSpawnScenario;
	}

	private void _E001()
	{
		Enabled = true;
		_E003 = new _E623<BotDifficulty>(new KeyValuePair<BotDifficulty, float>(BotDifficulty.easy, this.m__E001.BotEasy), new KeyValuePair<BotDifficulty, float>(BotDifficulty.normal, this.m__E001.BotNormal), new KeyValuePair<BotDifficulty, float>(BotDifficulty.hard, this.m__E001.BotHard), new KeyValuePair<BotDifficulty, float>(BotDifficulty.impossible, this.m__E001.BotImpossible));
		_E004 = new _E623<WildSpawnType>(new KeyValuePair<WildSpawnType, float>(WildSpawnType.assault, this.m__E001.BotAssault), new KeyValuePair<WildSpawnType, float>(WildSpawnType.marksman, this.m__E001.BotMarksman));
		if (_E004.Count == 0)
		{
			_E004.Add(1f, WildSpawnType.assault);
		}
		_ = this.m__E001.BotMax;
		_ = 0;
		_ = this.m__E001.BotStart;
		_ = 0;
		_ = this.m__E001.BotStop;
		_ = 0;
		List<_E624> list = new List<_E624>();
		for (int i = 0; i < this.m__E001.BotMax; i++)
		{
			_E624 item = new _E624(1, _E004.Random(), _E003.Random());
			list.Add(item);
		}
		_E00B = list.ToArray();
	}

	public void Run()
	{
		if (this.m__E001.NewSpawn && _E002 != null)
		{
			_E005 = true;
		}
	}

	public void Stop()
	{
		_E005 = false;
	}

	public void Update()
	{
		if (!_E005 || this.m__E000.PastTime < (float)this.m__E001.BotStart || this.m__E000.PastTime > (float)this.m__E001.BotStop)
		{
			return;
		}
		if (!_E007.HasValue || _E007.Value <= this.m__E000.PastTime)
		{
			_E006 = !_E006;
			_E007 = this.m__E000.PastTime + (float)(_E006 ? Random.Range(this.m__E001.BotSpawnTimeOnMin, this.m__E001.BotSpawnTimeOnMax) : Random.Range(this.m__E001.BotSpawnTimeOffMin, this.m__E001.BotSpawnTimeOffMax));
		}
		if (!_E006 || this.m__E000.PastTime - _E008 < 5f)
		{
			return;
		}
		_E008 = this.m__E000.PastTime;
		int num = this.m__E001.BotMax - _E002.BotsCountWithDelayed;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				WildSpawnType type = _E004.Random();
				BotDifficulty botDifficulty = _E003.Random();
				_E2FE data = new _E2FE(EPlayerSide.Savage, type, botDifficulty, 0f);
				_E002.ActivateBotsWithoutWave(1, data);
			}
		}
	}
}
