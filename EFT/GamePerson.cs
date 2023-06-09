using System;
using System.Collections.Generic;
using EFT.Game.Spawning;
using UnityEngine;

namespace EFT;

public sealed class GamePerson : MonoBehaviour, _E5B4
{
	[SerializeField]
	private int _id;

	[SerializeField]
	private string _profileId;

	[SerializeField]
	private ESpawnCategory _category;

	[SerializeField]
	private EPlayerSide _side;

	[SerializeField]
	private string _groupId;

	[SerializeField]
	private string _teamId;

	[SerializeField]
	private string _infiltration;

	[SerializeField]
	private string _botZoneName;

	public int Id => _id;

	public string ProfileId => _profileId;

	public EPlayerSide Side
	{
		get
		{
			return _side;
		}
		set
		{
			_side = value;
		}
	}

	public bool IsAI => _category != ESpawnCategory.Player;

	public ESpawnCategory Category
	{
		get
		{
			return _category;
		}
		set
		{
			_category = value;
		}
	}

	public Vector3 Position => base.transform.position;

	public Vector3 LookDirection => Quaternion.Euler(base.transform.rotation.y, base.transform.rotation.x, 0f) * Vector3.forward;

	public string GroupId
	{
		get
		{
			return _groupId;
		}
		set
		{
			_groupId = value;
		}
	}

	public string TeamId
	{
		get
		{
			return _teamId;
		}
		set
		{
			_teamId = value;
		}
	}

	public string Infiltration => _infiltration;

	public BifacialTransform Transform => null;

	public BifacialTransform WeaponRoot => null;

	public _E9C4 HealthController => null;

	public Profile Profile => null;

	public Player GetPlayer => null;

	public _E279 AIData => null;

	public _E27C Loyalty => null;

	public Dictionary<BodyPartType, _E1FF> MainParts => null;

	public static GamePerson Create(Vector3 position, Quaternion rotation, int id, string profileId, ESpawnCategory category, EPlayerSide side, string name = null, string groupId = null, string infiltration = null, string botZoneName = null)
	{
		if (string.IsNullOrEmpty(name))
		{
			name = _ED3E._E000(105748);
		}
		GamePerson gamePerson = new GameObject(name).AddComponent<GamePerson>();
		gamePerson.transform.position = position;
		gamePerson.transform.rotation = rotation;
		gamePerson._id = id;
		gamePerson._profileId = profileId;
		gamePerson._category = category;
		gamePerson._side = side;
		gamePerson._groupId = ((groupId != "") ? groupId : null);
		gamePerson._infiltration = ((infiltration != "") ? infiltration : null);
		gamePerson._botZoneName = ((botZoneName != "") ? botZoneName : null);
		return gamePerson;
	}

	public static GamePerson Create(_E5B3 args)
	{
		return Create(args.Position, args.Rotation, args.Id, args.ProfileId, args.Category, args.Side, args.Name, args.GroupId, args.Infiltration);
	}

	public void OnDrawGizmos()
	{
		Color color2 = (Gizmos.color = Side switch
		{
			EPlayerSide.Usec => Color.red, 
			EPlayerSide.Bear => Color.green, 
			EPlayerSide.Savage => Color.blue, 
			_ => throw new ArgumentOutOfRangeException(), 
		});
		Vector3 direction = base.transform.TransformDirection(Vector3.up) * 20f;
		Gizmos.DrawRay(base.transform.position, direction);
		color2.a = 0.5f;
		Gizmos.color = color2;
		_E395.DrawCube(base.transform.position, base.transform.rotation, new Vector3(1f, 0.01f, 1f));
		switch (Category)
		{
		case ESpawnCategory.Player:
			Gizmos.color = new Color(1f, 0.35f, 0f);
			break;
		case ESpawnCategory.Bot:
			Gizmos.color = new Color(0.35f, 0f, 1f);
			break;
		case ESpawnCategory.Boss:
			Gizmos.color = new Color(0f, 1f, 0.35f);
			break;
		case ESpawnCategory.Coop:
		case ESpawnCategory.Group:
		case ESpawnCategory.Opposite:
			Gizmos.color = new Color(1f, 0.35f, 0f);
			break;
		}
		Gizmos.DrawSphere(base.transform.position + Vector3.up * 30f, 1f);
		Gizmos.DrawSphere(base.transform.position + Vector3.up * 28f, 2f);
		Gizmos.DrawSphere(base.transform.position + Vector3.up * 26f, 3f);
		Gizmos.color = Color.white;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawLine(Vector3.zero, Vector3.forward * 2f * 0.3f);
		Gizmos.DrawLine(Vector3.forward * 2f * 0.3f, Vector3.forward * 1.5f * 0.3f + Vector3.left * 0.5f * 0.3f);
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * 0.3f, Vector3.forward * 1.5f * 0.3f + Vector3.right * 0.5f * 0.3f);
	}
}
