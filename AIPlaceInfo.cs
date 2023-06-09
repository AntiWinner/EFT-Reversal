using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AIPlaceInfo : MonoBehaviour, IPhysicsTrigger
{
	public int AreaId;

	public string Id;

	public ThrowGrenadePlace[] GrenadePlaces;

	public BoxCollider Collider;

	public bool IsOnTrigger = true;

	public bool BlockGrenade;

	public bool IsDark;

	public bool IsInside;

	public bool AtrZone;

	private bool _E000;

	private readonly HashSet<Player> _E001 = new HashSet<Player>();

	[CompilerGenerated]
	private Action<Player> _E002;

	[CompilerGenerated]
	private Action<Player> _E003;

	public AIPlaceInfoLogic InfoLogicAllEnemy;

	public ECoverPointSpecial CoversSpecial;

	private _E620 _E004;

	[CompilerGenerated]
	private readonly string _E005 = _ED3E._E000(14982);

	public bool UseAsCoverGroupId = true;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
	}

	public event Action<Player> OnPlayerEnter
	{
		[CompilerGenerated]
		add
		{
			Action<Player> action = _E002;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player> action = _E002;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Player> OnPlayerExit
	{
		[CompilerGenerated]
		add
		{
			Action<Player> action = _E003;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Player> action = _E003;
			Action<Player> action2;
			do
			{
				action2 = action;
				Action<Player> value2 = (Action<Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		base.gameObject.layer = _E37B.TriggersLayer;
		Collider = GetComponent<BoxCollider>();
		if (Collider == null)
		{
			Debug.LogError(_ED3E._E000(15003), base.gameObject);
			IsOnTrigger = false;
		}
		else if (!Collider.enabled)
		{
			IsOnTrigger = false;
		}
	}

	public List<ThrowGrenadePlace> CheckAllStart()
	{
		List<ThrowGrenadePlace> list = new List<ThrowGrenadePlace>();
		for (int i = 0; i < GrenadePlaces.Length; i++)
		{
			ThrowGrenadePlace throwGrenadePlace = GrenadePlaces[i];
			Refresh();
			if (!throwGrenadePlace.IsValid(Id, withSubDoor: true))
			{
				list.Add(throwGrenadePlace);
			}
		}
		return list;
	}

	public int GetCount(Func<Player, bool> isGood)
	{
		return _E001.Count(isGood);
	}

	public IEnumerator<int> CheckAllPoints(bool shallRepeat)
	{
		for (int i = 0; i < GrenadePlaces.Length; i++)
		{
			ThrowGrenadePlace throwGrenadePlace = GrenadePlaces[i];
			Refresh();
			EDoorState? eDoorState = null;
			if (throwGrenadePlace.Door != null && throwGrenadePlace.Door.DoorState != EDoorState.Open)
			{
				eDoorState = throwGrenadePlace.Door.DoorState;
				throwGrenadePlace.Door.DoorState = EDoorState.Open;
				throwGrenadePlace.Door.OnValidate();
			}
			yield return 1;
			if (!throwGrenadePlace.IsValid(Id, withSubDoor: true) && !shallRepeat)
			{
				Debug.LogError(string.Concat(_ED3E._E000(15034), throwGrenadePlace.From, _ED3E._E000(15023), throwGrenadePlace.Target, _ED3E._E000(15014), base.gameObject.name));
			}
			yield return 1;
			if (throwGrenadePlace.Door != null && eDoorState.HasValue)
			{
				throwGrenadePlace.Door.DoorState = eDoorState.Value;
				throwGrenadePlace.Door.OnValidate();
			}
		}
		if (shallRepeat)
		{
			StartCoroutine(CheckAllPoints(shallRepeat: false));
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (GrenadePlaces == null)
		{
			return;
		}
		for (int i = 0; i < GrenadePlaces.Length; i++)
		{
			ThrowGrenadePlace throwGrenadePlace = GrenadePlaces[i];
			if (throwGrenadePlace != null)
			{
				throwGrenadePlace.DrawGizmos();
			}
		}
	}

	public void Refresh()
	{
		ThrowGrenadePlace[] componentsInChildren = GetComponentsInChildren<ThrowGrenadePlace>();
		GrenadePlaces = componentsInChildren.ToArray();
	}

	public void Init(IEnumerable<Door> allDoors, _E620 botsController)
	{
		if (!this._E000)
		{
			this._E000 = true;
			_E004 = botsController;
			ThrowGrenadePlace[] grenadePlaces = GrenadePlaces;
			for (int i = 0; i < grenadePlaces.Length; i++)
			{
				grenadePlaces[i].Init(allDoors);
			}
			if (InfoLogicAllEnemy != null)
			{
				InfoLogicAllEnemy.Init(this, botsController);
			}
		}
	}

	protected virtual void OnEnterPlace(Player player)
	{
		_E001.Add(player);
		_E002?.Invoke(player);
	}

	protected virtual void OnLeavePlace(Player player)
	{
		_E001.Remove(player);
		_E003?.Invoke(player);
	}

	void IPhysicsTrigger.OnTriggerExit(Collider col)
	{
		if (IsOnTrigger)
		{
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
			if (!(playerByCollider == null) && playerByCollider.AIData != null)
			{
				playerByCollider.AIData.LeavePlace(this);
				OnLeavePlace(playerByCollider);
			}
		}
	}

	void IPhysicsTrigger.OnTriggerEnter(Collider col)
	{
		if (IsOnTrigger)
		{
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
			if (!(playerByCollider == null) && playerByCollider.AIData != null)
			{
				playerByCollider.AIData.AddPlaceInfo(this);
				OnEnterPlace(playerByCollider);
			}
		}
	}

	private void OnDestroy()
	{
		if (InfoLogicAllEnemy != null)
		{
			InfoLogicAllEnemy.Dispose();
		}
		_E002 = null;
		_E003 = null;
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
