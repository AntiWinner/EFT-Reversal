using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace Audio.SpatialSystem;

[Serializable]
public sealed class AudioTriggerArea : MonoBehaviour, IPhysicsTrigger
{
	[Tooltip("Give an optional name for the Audio Trigger Area ")]
	public string triggerAreaName = "";

	[SerializeField]
	private BoxCollider areaCollider;

	private int _triggerCounter;

	private Dictionary<int, int> _playersTriggerCounter = new Dictionary<int, int>();

	public string Description { get; } = _ED3E._E000(73215);


	public event EventHandler<_E47F> OnTriggerArea;

	private void Awake()
	{
		base.gameObject.layer = _E37B.TriggersLayer;
		if ((object)areaCollider == null)
		{
			areaCollider = base.gameObject.GetComponent<BoxCollider>();
		}
	}

	public void OnTriggerExit(Collider coll)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(coll);
		if (!(playerByCollider != null) || !playerByCollider.HealthController.IsAlive)
		{
			return;
		}
		int id = playerByCollider.Id;
		if (_playersTriggerCounter.TryGetValue(id, out var value))
		{
			value--;
			_playersTriggerCounter[id] = value;
			if (value == 0)
			{
				_E001(playerByCollider);
				_playersTriggerCounter.Remove(id);
			}
		}
	}

	public void OnTriggerEnter(Collider coll)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(coll);
		if (!(playerByCollider != null) || !playerByCollider.HealthController.IsAlive)
		{
			return;
		}
		int id = playerByCollider.Id;
		if (_playersTriggerCounter.TryGetValue(id, out var value))
		{
			if (value == 0)
			{
				_E000(playerByCollider);
			}
			_playersTriggerCounter[id]++;
		}
		else
		{
			_playersTriggerCounter.Add(id, 1);
			_E000(playerByCollider);
		}
	}

	private void _E000(Player player)
	{
		_E47F e = new _E47F
		{
			TriggerEventType = _E47F.ETriggerEventType.TriggerEnter,
			Player = player
		};
		this.OnTriggerArea?.Invoke(this, e);
	}

	private void _E001(Player player)
	{
		_E47F e = new _E47F
		{
			TriggerEventType = _E47F.ETriggerEventType.TriggerExit,
			Player = player
		};
		this.OnTriggerArea?.Invoke(this, e);
	}

	public BoxCollider GetCollider()
	{
		if ((object)areaCollider == null)
		{
			areaCollider = base.gameObject.GetComponent<BoxCollider>();
			return areaCollider;
		}
		return areaCollider;
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
