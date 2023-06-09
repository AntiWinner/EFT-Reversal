using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT;
using EFT.Counters;
using EFT.Interactive;
using UnityEngine;

namespace CommonAssets.Scripts.Game;

public sealed class EndByExitTrigerScenario : MonoBehaviour
{
	private sealed class _E000
	{
		public readonly Player Player;

		public readonly ExfiltrationPoint Trigger;

		public readonly float StartTime;

		public _E000(Player player, ExfiltrationPoint trigger, float startTime)
		{
			Player = player;
			Trigger = trigger;
			StartTime = startTime;
		}
	}

	public interface _E001 : _E61E
	{
		void StopSession(string profileId, ExitStatus exitStatus, string exitName);
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Player player;

		internal bool _E000(_E000 x)
		{
			return x.Player != player;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Player player;

		internal bool _E000(_E000 x)
		{
			return x.Player == player;
		}
	}

	private _E001 m__E000;

	private ExfiltrationPoint[] m__E001;

	private readonly List<_E000> m__E002 = new List<_E000>();

	private readonly List<(Player, string)> m__E003 = new List<(Player, string)>();

	private readonly List<ExfiltrationPoint> _E004 = new List<ExfiltrationPoint>();

	public static EndByExitTrigerScenario Create(_E001 game)
	{
		EndByExitTrigerScenario endByExitTrigerScenario = game.gameObject.AddComponent<EndByExitTrigerScenario>();
		endByExitTrigerScenario.m__E000 = game;
		return endByExitTrigerScenario;
	}

	public void Run()
	{
		_E57A instance = _E57A.Instance;
		this.m__E001 = instance.ExfiltrationPoints.Concat(instance.ScavExfiltrationPoints).ToArray();
		_E57A.Instance.LogDebug(_ED3E._E000(125053), this.m__E001.Length);
		ExfiltrationPoint[] array = this.m__E001;
		foreach (ExfiltrationPoint obj in array)
		{
			obj.OnStartExtraction += _E000;
			obj.OnCancelExtraction += _E001;
			obj.OnStatusChanged += _E002;
		}
	}

	public void Stop()
	{
		_E57A.Instance.LogDebug(_ED3E._E000(125068));
		ExfiltrationPoint[] array = Interlocked.Exchange(ref this.m__E001, null);
		if (array != null)
		{
			_E004.Clear();
			ExfiltrationPoint[] array2 = array;
			foreach (ExfiltrationPoint obj in array2)
			{
				obj.OnStartExtraction -= _E000;
				obj.OnCancelExtraction -= _E001;
				obj.OnStatusChanged -= _E002;
				obj.Disable();
			}
		}
	}

	private void _E000(ExfiltrationPoint trigger, Player player)
	{
		if (this.m__E000.Status != GameStatus.Started && this.m__E000.Status != GameStatus.SoftStopping)
		{
			_E57A.Instance.LogDebug(_ED3E._E000(125105), this.m__E000.Status);
		}
		else if (this.m__E002.All((_E000 x) => x.Player != player))
		{
			this.m__E002.Add(new _E000(player, trigger, this.m__E000.PastTime));
			_E57A.Instance.LogDebug(_ED3E._E000(125126), player.Profile.Nickname, player.Profile.Info.EntryPoint, trigger.Settings.Name, this.m__E000.PastTime);
		}
	}

	private void _E001(ExfiltrationPoint trigger, Player player)
	{
		if (this.m__E000.Status == GameStatus.Started || this.m__E000.Status == GameStatus.SoftStopping)
		{
			_E000 obj = this.m__E002.FirstOrDefault((_E000 x) => x.Player == player);
			if (obj != null)
			{
				this.m__E002.Remove(obj);
			}
		}
	}

	private void _E002(ExfiltrationPoint point, EExfiltrationStatus prevStatus)
	{
		bool num = _E004.Contains(point);
		if (num && point.Status != EExfiltrationStatus.Countdown)
		{
			point.ExfiltrationStartTime = -1E-45f;
			_E004.Remove(point);
		}
		if (!num && point.Status == EExfiltrationStatus.Countdown)
		{
			if (point.ExfiltrationStartTime <= 0f)
			{
				point.ExfiltrationStartTime = this.m__E000.PastTime;
			}
			_E004.Add(point);
		}
	}

	private void Update()
	{
		if (this.m__E001 == null)
		{
			return;
		}
		for (int num = this.m__E002.Count - 1; num > -1; num--)
		{
			_E000 obj = this.m__E002[num];
			if (!(obj.StartTime + obj.Trigger.Settings.ExfiltrationTime - this.m__E000.PastTime > 0f))
			{
				this.m__E002.Remove(obj);
				this.m__E003.Add((obj.Player, obj.Trigger.Settings.Name));
			}
		}
		for (int num2 = _E004.Count - 1; num2 >= 0; num2--)
		{
			ExfiltrationPoint exfiltrationPoint = _E004[num2];
			if (!(this.m__E000.PastTime - exfiltrationPoint.ExfiltrationStartTime <= exfiltrationPoint.Settings.ExfiltrationTime))
			{
				Player[] array = exfiltrationPoint.Entered.ToArray();
				foreach (Player player in array)
				{
					bool flag = !exfiltrationPoint.UnmetRequirements(player).Any();
					if (player != null && player.HealthController.IsAlive && flag)
					{
						this.m__E003.Add((player, exfiltrationPoint.Settings.Name));
					}
				}
				exfiltrationPoint.SetStatusLogged((!exfiltrationPoint.Reusable) ? EExfiltrationStatus.NotPresent : EExfiltrationStatus.UncompleteRequirements, _ED3E._E000(125195));
			}
		}
		_E5CB._E023._E004 matchEnd = Singleton<_E5CB>.Instance.Experience.MatchEnd;
		foreach (var item3 in this.m__E003)
		{
			Player item = item3.Item1;
			string item2 = item3.Item2;
			ExitStatus exitStatus = ((item.Profile.Stats.SessionCounters.GetAllInt(CounterTag.Exp) <= matchEnd.SurvivedExpRequirement && !(this.m__E000.PastTime > (float)matchEnd.SurvivedTimeRequirement)) ? ExitStatus.Runner : ExitStatus.Survived);
			this.m__E000.StopSession(item.ProfileId, exitStatus, item2);
		}
		this.m__E003.Clear();
	}
}
