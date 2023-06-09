using System;
using UnityEngine;

namespace EFT;

[Serializable]
public class FastGameInfo
{
	private const string SCENES_JSON_CONFIG_PATH = "Assets/Editor/ScenesJsonConfig.asset";

	private TarkovApplication _tarkovApplication;

	private string[] _mapsCollection;

	public bool IsAutoLoginEnabled;

	public bool UseLoginFromInspector;

	public string LoginForAutoLogin;

	public string PasswordForAutoLogin;

	[Header("Fast start enable settings")]
	public bool IsFastStartEnabled;

	public string FastGameMap;

	public bool IsLocalGame;

	public bool IsNoLootForLocalGame;

	public bool IsAIEnabledInLocalGame;

	public bool IsLocationGameSessionLimit;

	public bool IsCustomGameSessionLimit;

	public int CustomGameSessionTimeLimit = 100000;

	public string[] MapsCollection
	{
		get
		{
			return _mapsCollection;
		}
		set
		{
			_mapsCollection = value;
		}
	}

	private void _E000()
	{
		IsCustomGameSessionLimit = !IsLocationGameSessionLimit;
	}

	private void _E001()
	{
		IsLocationGameSessionLimit = !IsCustomGameSessionLimit;
	}
}
