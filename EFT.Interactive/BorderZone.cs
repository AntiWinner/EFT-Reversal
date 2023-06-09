using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public abstract class BorderZone : MonoBehaviour, IPhysicsTrigger
{
	protected const float DEEPENING_UPDATE_FREQUENCY = 1f;

	[SerializeField]
	private BoxCollider Collider;

	[SerializeField]
	protected Vector3 _extents;

	[SerializeField]
	protected Vector4 _triggerZoneSettings = new Vector4(1f, 1f, 1f, 1f);

	private float m__E000;

	protected List<Player> TargetedPlayers = new List<Player>();

	protected List<Player> NotTargetedPlayers = new List<Player>();

	[CompilerGenerated]
	private int m__E001;

	[CompilerGenerated]
	private readonly string _E002 = _ED3E._E000(212450);

	[CompilerGenerated]
	private Action<Player, BorderZone, float, bool> _E003;

	public int Id
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		set
		{
			this.m__E001 = value;
		}
	}

	public virtual float Delay => 0f;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public Player[] GetAllPlayersInArea => TargetedPlayers.Concat(NotTargetedPlayers).ToArray();

	public event Action<Player, BorderZone, float, bool> PlayerShotEvent
	{
		[CompilerGenerated]
		add
		{
			Action<Player, BorderZone, float, bool> action = _E003;
			Action<Player, BorderZone, float, bool> action2;
			do
			{
				action2 = action;
				Action<Player, BorderZone, float, bool> value2 = (Action<Player, BorderZone, float, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player, BorderZone, float, bool> action = _E003;
			Action<Player, BorderZone, float, bool> action2;
			do
			{
				action2 = action;
				Action<Player, BorderZone, float, bool> value2 = (Action<Player, BorderZone, float, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public virtual void Update()
	{
		if (Time.time < this.m__E000)
		{
			return;
		}
		for (int num = NotTargetedPlayers.Count - 1; num >= 0; num--)
		{
			Player player = NotTargetedPlayers[num];
			if (IsInTriggerZone(player.Position))
			{
				NotTargetedPlayers.RemoveAt(num);
				TargetedPlayers.Add(player);
				StartCoroutine(FireCoroutine(player));
			}
		}
		this.m__E000 = Time.time + 1f;
	}

	protected virtual IEnumerator FireCoroutine(Player player)
	{
		yield break;
	}

	public void RemoveAuthority()
	{
		base.enabled = false;
	}

	public virtual void ProcessIncomingPacket(Player player, bool willHit = true)
	{
		_E003?.Invoke(player, this, Delay, willHit);
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			NotTargetedPlayers.Add(playerByCollider);
			playerByCollider.OnPlayerDeadOrUnspawn += _E000;
		}
	}

	private void _E000(Player player)
	{
		_E001(player);
	}

	private void _E001(Player player)
	{
		player.OnPlayerDeadOrUnspawn -= _E000;
		TargetedPlayers.Remove(player);
		NotTargetedPlayers.Remove(player);
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			_E001(playerByCollider);
		}
	}

	protected void InvokePlayerShotEvent(Player player, float delay, bool willHit = true)
	{
		_E003?.Invoke(player, this, delay, willHit);
	}

	private void OnValidate()
	{
		if (Collider != null)
		{
			_extents = Collider.size / 2f;
		}
	}

	protected virtual bool IsInTriggerZone(Vector3 global)
	{
		Vector3 vector = base.transform.InverseTransformPoint(global);
		Vector3 lossyScale = base.transform.lossyScale;
		if ((_extents.x + vector.x) * lossyScale.x < _triggerZoneSettings.x)
		{
			return false;
		}
		if ((_extents.x - vector.x) * lossyScale.x < _triggerZoneSettings.y)
		{
			return false;
		}
		if ((vector.z + _extents.z) * lossyScale.z < _triggerZoneSettings.z)
		{
			return false;
		}
		if ((_extents.z - vector.z) * lossyScale.z < _triggerZoneSettings.w)
		{
			return false;
		}
		return true;
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
