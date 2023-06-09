using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Interactive;
using EFT.UI;
using EFT.UI.BattleTimer;
using EFT.UI.Matchmaker;
using UnityEngine;

namespace EFT;

[_E2E2(-500)]
public abstract class AbstractGame : MonoBehaviour, _E61E, IDisposable
{
	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	[CompilerGenerated]
	private GameStatus m__E000;

	[CompilerGenerated]
	private _E62B m__E001;

	[CompilerGenerated]
	private EUpdateQueue _E002;

	public const float MAX_FIXED_DELTA_TIME = 0.05f;

	public GameStatus Status
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		protected set
		{
			this.m__E000 = value;
		}
	}

	public _E62B GameTimer
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public float PastTime => GameTimer.PastTimeSeconds();

	public EUpdateQueue UpdateQueue
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public float FixedDeltaTime
	{
		get
		{
			return Time.fixedDeltaTime;
		}
		set
		{
			if (value <= 0.05f)
			{
				Time.fixedDeltaTime = value;
				Debug.LogFormat(_ED3E._E000(147805), FixedDeltaTime);
			}
			else
			{
				Debug.LogErrorFormat(_ED3E._E000(147788), value);
			}
		}
	}

	public abstract string LocationObjectId { get; }

	public bool InRaid
	{
		get
		{
			if (!(this is _E7A6))
			{
				return this is LocalGame;
			}
			return true;
		}
	}

	protected abstract GameUI GameUi { get; }

	protected abstract string ProfileId { get; }

	protected static TGame Create<TGame>(EUpdateQueue updateQueue, TimeSpan? sessionTime = null, string goName = "GAME") where TGame : AbstractGame
	{
		TGame val = new GameObject(goName).AddComponent<TGame>();
		val.GameTimer = new _E62B(sessionTime);
		val.UpdateQueue = updateQueue;
		return val;
	}

	public virtual void Dispose()
	{
		CompositeDisposable.Dispose();
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	protected void UpdateExfiltrationUi(ExfiltrationPoint point, bool contains, bool initial = false)
	{
		ExitTriggerSettings settings = point.Settings;
		bool flag = false;
		if (contains)
		{
			flag = point.HasMetRequirements(ProfileId);
		}
		float num = ((point.Status == EExfiltrationStatus.Countdown) ? (settings.ExfiltrationTime - ((point.ExfiltrationStartTime > 0f) ? (PastTime - point.ExfiltrationStartTime) : 0f)) : (flag ? settings.ExfiltrationTime : (0f - PastTime)));
		if (contains && point.Status != EExfiltrationStatus.NotPresent)
		{
			if (settings.PlayersCount > 0)
			{
				GameUi.BattleUiPmcCount.Show(settings.PlayersCount, point.QueuedPlayers.Count);
			}
			if (flag)
			{
				GameUi.BattleUiPanelExitTrigger.Show(num);
			}
			else
			{
				GameUi.BattleUiPanelExitTrigger.Close();
			}
		}
		GameUi.TimerPanel.UpdateExfiltrationTimers(point, flag, contains, initial, num, point.Status);
		GameUi.TimerPanel.SetMainTimerState(point.Settings.Name, flag ? EMainTimerState.StayInEpInside : EMainTimerState.FindEp);
	}

	protected void OnEpInteraction(ExfiltrationPoint point, bool entered)
	{
		ExitTriggerSettings settings = point.Settings;
		bool flag = point.QueuedPlayers.Contains(ProfileId);
		if (entered)
		{
			UpdateExfiltrationUi(point, point.Entered.Any((Player x) => x.ProfileId == ProfileId));
			GameUi.TimerPanel.SwitchTimers(point, showOnePoint: true);
			return;
		}
		GameUi.BattleUiPmcCount.Close();
		GameUi.BattleUiPanelExitTrigger.Close();
		if (flag)
		{
			GameUi.TimerPanel.ShowTimer(showExits: true);
		}
		if (string.IsNullOrEmpty(settings.Name))
		{
			return;
		}
		if (point.Status == EExfiltrationStatus.RegularMode)
		{
			GameUi.TimerPanel.UpdateExfiltrationTimers(point, availability: true, point.Entered.Any((Player x) => x.ProfileId == ProfileId), initial: false, settings.MaxTime * 60f - PastTime, point.Status);
		}
		GameUi.TimerPanel.SetMainTimerState(point.Settings.Name, flag ? EMainTimerState.StayInEpOutside : EMainTimerState.FindEp);
	}

	protected void SetMatchmakerStatus(string status, float? progress = null)
	{
		if (_EC92.Instance.CurrentScreenController is MatchmakerTimeHasCome._E000 obj)
		{
			obj.ChangeStatus(status, progress);
		}
	}

	[SpecialName]
	GameObject _E61E.get_gameObject()
	{
		return base.gameObject;
	}

	[CompilerGenerated]
	private bool _E000(Player x)
	{
		return x.ProfileId == ProfileId;
	}

	[CompilerGenerated]
	private bool _E001(Player x)
	{
		return x.ProfileId == ProfileId;
	}
}
