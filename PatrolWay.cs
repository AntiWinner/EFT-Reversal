using System;
using System.Collections.Generic;
using System.Text;
using EFT;
using UnityEngine;

[Serializable]
public class PatrolWay : MonoBehaviour
{
	public PatrolType PatrolType;

	public List<PatrolPoint> Points = new List<PatrolPoint>();

	public int MaxPersons;

	[HideInInspector]
	public WildSpawnType BlockRoles;

	private HashSet<BotOwner> _users = new HashSet<BotOwner>();

	public float CoefSubPoints = 1f;

	public WildSpawnType PosibleRoles { get; private set; }

	private Vector3 _E000
	{
		get
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < Points.Count; i++)
			{
				PatrolPoint patrolPoint = Points[i];
				zero += patrolPoint.position;
			}
			return zero / Points.Count;
		}
	}

	public void InitPoints()
	{
		WildSpawnType[] allTypes = _E620.AllTypes;
		WildSpawnType[] array = allTypes;
		foreach (WildSpawnType wildSpawnType in array)
		{
			if ((BlockRoles & wildSpawnType) != wildSpawnType)
			{
				PosibleRoles |= wildSpawnType;
			}
		}
		foreach (PatrolPoint point in Points)
		{
			point.SetWay(this);
		}
		if (!_E0FE.Instance.IsTraceEnable())
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		array = allTypes;
		for (int i = 0; i < array.Length; i++)
		{
			WildSpawnType wildSpawnType2 = array[i];
			if ((BlockRoles & wildSpawnType2) == wildSpawnType2)
			{
				stringBuilder.Append(wildSpawnType2.ToString() + _ED3E._E000(27331));
			}
		}
	}

	public void EditorInitPoints()
	{
		Points = new List<PatrolPoint>();
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.activeInHierarchy)
			{
				PatrolPoint component = item.GetComponent<PatrolPoint>();
				if (component != null)
				{
					_E000(component);
				}
			}
		}
		if (PatrolType == PatrolType.reserved)
		{
			_ = Points.Count;
			_ = 2;
		}
	}

	public bool CanBeUsedByRole(WildSpawnType role)
	{
		if (PosibleRoles == (WildSpawnType)0)
		{
			return true;
		}
		return (PosibleRoles & role) == role;
	}

	public void RemoveMe(BotOwner owner)
	{
		_users.Remove(owner);
	}

	public void AddUser(BotOwner owner)
	{
		if (!_users.Contains(owner))
		{
			_users.Add(owner);
		}
	}

	public bool HaveFreeSpace()
	{
		if (MaxPersons <= 0)
		{
			return true;
		}
		return _users.Count < MaxPersons;
	}

	public bool HaveFreeSpace(BotOwner exeptBot)
	{
		if (MaxPersons <= 0)
		{
			return true;
		}
		int num = ((!_users.Contains(exeptBot)) ? _users.Count : (_users.Count - 1));
		return num < MaxPersons;
	}

	public int UsersCount()
	{
		return _users.Count;
	}

	public string AllUsesrDebugIdInfo()
	{
		string text = "";
		foreach (BotOwner user in _users)
		{
			text = text + _ED3E._E000(18502) + user.Id;
		}
		return text;
	}

	public string AllUsesrDebugIdInfoRole()
	{
		string text = "";
		foreach (BotOwner user in _users)
		{
			text = text + _ED3E._E000(18502) + user.Profile.Info.Settings.Role;
		}
		return text;
	}

	public bool IsCloseToSelect(BotOwner owner, float closeToSelectReservWay)
	{
		float num = closeToSelectReservWay * closeToSelectReservWay;
		return (owner.Position - this._E000).sqrMagnitude < num;
	}

	public virtual string Suitabledata()
	{
		return _ED3E._E000(8257);
	}

	public virtual bool Suitable(BotOwner bot, _E301 data)
	{
		return false;
	}

	private void _E000(PatrolPoint pp)
	{
		if (pp.Id == 0)
		{
			pp.Id = Guid.NewGuid().GetHashCode();
		}
		Points.Add(pp);
	}
}
