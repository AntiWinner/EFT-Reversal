using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT;
using UnityEngine;
using UnityEngine.Timeline;

namespace Cutscene;

public class BaseCutsceneTrigger : MonoBehaviour
{
	[CompilerGenerated]
	private Action<BaseCutsceneTrigger, Player> _E000;

	[CompilerGenerated]
	private Action<Player> _E001;

	[_E44A]
	[SerializeField]
	private int _cutsceneID = -1;

	[_E44A]
	[SerializeField]
	private Vector3 _startPosition;

	[_E44A]
	[SerializeField]
	private Vector3 _startViewDirection;

	[_E44A]
	[SerializeField]
	private float _startPlayerPosLevel;

	[SerializeField]
	[_E44A]
	private bool _needToProneAtStart;

	[SerializeField]
	[_E44A]
	private Vector3 _cutsceneEndPlayerPosition;

	public CutsceneFakePlayerSteps fakePlayerSteps;

	[SerializeField]
	private TimelineAsset _timelineAsset;

	[SerializeField]
	private bool _callServerTeleportOnEnd;

	private Player _E002;

	public int CutsceneID => _cutsceneID;

	public Vector3 StartPosition => _startPosition;

	public Vector3 StartViewDirection => _startViewDirection;

	public float StartPlayerPosLevel => _startPlayerPosLevel;

	public bool NeedToProneAtStart => _needToProneAtStart;

	public Vector3 CutsceneEndPlayerPosition => _cutsceneEndPlayerPosition;

	public TimelineAsset TimeLineAsset => _timelineAsset;

	public event Action<BaseCutsceneTrigger, Player> OnPlayerCausesCutscene
	{
		[CompilerGenerated]
		add
		{
			Action<BaseCutsceneTrigger, Player> action = _E000;
			Action<BaseCutsceneTrigger, Player> action2;
			do
			{
				action2 = action;
				Action<BaseCutsceneTrigger, Player> value2 = (Action<BaseCutsceneTrigger, Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<BaseCutsceneTrigger, Player> action = _E000;
			Action<BaseCutsceneTrigger, Player> action2;
			do
			{
				action2 = action;
				Action<BaseCutsceneTrigger, Player> value2 = (Action<BaseCutsceneTrigger, Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Player> OnCutsceneEnded
	{
		[CompilerGenerated]
		add
		{
			Action<Player> action = _E001;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player> action = _E001;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected virtual void Awake()
	{
		_E454.Instance.AddActiveCutsceneTrigger(this);
		CutsceneTriggerStartInfoSO.StartPlayerValues playerStartInfo = CutsceneTriggerStartInfoSO.Instance.GetPlayerStartInfo(base.gameObject.scene.name, _cutsceneID);
		if (playerStartInfo != null)
		{
			_startPosition = playerStartInfo.startPosition;
			_startViewDirection = playerStartInfo.startViewDirection;
			_startPlayerPosLevel = playerStartInfo.startPlayerPosLevel;
			_needToProneAtStart = playerStartInfo.needToProneAtStart;
			_cutsceneEndPlayerPosition = playerStartInfo.cutsceneEndPlayerPos;
		}
	}

	protected void CallStartCutscene(Player player)
	{
		if (player.IsYourPlayer)
		{
			_E002 = player;
			_E000?.Invoke(this, player);
		}
	}

	public void CallEndCutscene()
	{
		_E001?.Invoke(_E002);
		_E002 = null;
		if (_callServerTeleportOnEnd)
		{
			_EBAF.Instance.CreateCommonEvent<_EBC2>().Invoke(base.gameObject.scene.name, CutsceneID);
		}
	}
}
