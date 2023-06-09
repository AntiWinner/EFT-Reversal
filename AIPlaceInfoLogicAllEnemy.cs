using System.Collections.Generic;
using EFT;

public class AIPlaceInfoLogicAllEnemy : AIPlaceInfoLogic
{
	public List<WildSpawnType> EnemyList = new List<WildSpawnType>();

	private bool m__E000;

	private readonly HashSet<WildSpawnType> m__E001 = new HashSet<WildSpawnType>();

	private AIPlaceInfo m__E002;

	private _E620 _E003;

	private readonly List<BotOwner> _E004 = new List<BotOwner>();

	public override void Init(AIPlaceInfo aiPlaceInfo, _E620 botsController)
	{
		_E003 = botsController;
		this.m__E002 = aiPlaceInfo;
		this.m__E000 = EnemyList.Count > 0;
		foreach (WildSpawnType enemy in EnemyList)
		{
			this.m__E001.Add(enemy);
		}
		foreach (BotOwner botOwner in _E003.Bots.BotOwners)
		{
			if (this.m__E001.Contains(botOwner.Profile.Info.Settings.Role))
			{
				_E004.Add(botOwner);
			}
		}
		this.m__E002.OnPlayerEnter += _E002;
		_E003.Bots.OnBotAdd += _E001;
		_E003.Bots.OnBotRemove += _E000;
		base.Init(aiPlaceInfo, botsController);
	}

	private void _E000(BotOwner botOwner)
	{
		_E004.Remove(botOwner);
	}

	private void _E001(BotOwner botOwner)
	{
		if (this.m__E001.Contains(botOwner.Profile.Info.Settings.Role))
		{
			_E004.Add(botOwner);
		}
	}

	private void _E002(Player player)
	{
		if (!this.m__E000)
		{
			return;
		}
		foreach (BotOwner item in _E004)
		{
			item.BotsGroup.CheckAndAddEnemy(player);
		}
	}

	public new void Dispose()
	{
		if (this.m__E002 != null)
		{
			this.m__E002.OnPlayerEnter -= _E002;
		}
		if (_E003 != null)
		{
			_E003.Bots.OnBotAdd -= _E001;
			_E003.Bots.OnBotRemove -= _E000;
		}
	}
}
