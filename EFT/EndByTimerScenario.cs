using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT;

internal sealed class EndByTimerScenario : MonoBehaviour
{
	internal interface _E000 : _E61E
	{
		void StopGame();
	}

	private _E000 m__E000;

	private _E62B _E001;

	[CompilerGenerated]
	private GameStatus _E002 = GameStatus.Started;

	internal GameStatus _E003
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	internal static EndByTimerScenario _E000(_E000 game)
	{
		EndByTimerScenario endByTimerScenario = game.gameObject.AddComponent<EndByTimerScenario>();
		endByTimerScenario.m__E000 = game;
		endByTimerScenario._E001 = game.GameTimer;
		return endByTimerScenario;
	}

	private void Update()
	{
		if (this.m__E000.Status == _E003 && _E001.SessionTime.HasValue)
		{
			TimeSpan pastTime = _E001.PastTime;
			TimeSpan? sessionTime = _E001.SessionTime;
			if (pastTime >= sessionTime)
			{
				Debug.Log(string.Format(_ED3E._E000(148294), this.m__E000.Status, this.m__E000.PastTime, _E001.SessionTime));
				this.m__E000.StopGame();
				base.enabled = false;
			}
		}
	}
}
