using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EFT.Game.Spawning;

[ExecuteInEditMode]
public sealed class SpawnPointMarker : MonoBehaviour, ISpawnPointCollider, IPositionPoint
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SpawnPointParams @params;

		internal bool _E000(BotZone x)
		{
			return x.name == @params.BotZoneName;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _E001
	{
		public _E315 logger;

		public SpawnPointMarker _003C_003E4__this;
	}

	public Color Color = new Color(0f, 1f, 0f, 1f);

	[SerializeField]
	private SpawnPoint _spawnPoint;

	[SerializeField]
	private Collider _collider;

	private string m__E000;

	public Vector3 Position => base.transform.position;

	public ISpawnPoint SpawnPoint => _spawnPoint;

	public string Id
	{
		get
		{
			return _spawnPoint.Id;
		}
		set
		{
			_spawnPoint.Id = value;
		}
	}

	public BotZone BotZone
	{
		get
		{
			return _spawnPoint.BotZone;
		}
		set
		{
			_spawnPoint.BotZone = value;
		}
	}

	public EPlayerSideMask Sides
	{
		get
		{
			return _spawnPoint.Sides;
		}
		set
		{
			_spawnPoint.Sides = value;
		}
	}

	public static void TryCreate(SpawnPointParams[] parameters, Transform parent = null)
	{
		if (parameters == null)
		{
			return;
		}
		string[] source = (from marker in UnityEngine.Object.FindObjectsOfType<SpawnPointMarker>()
			select marker.SpawnPoint.Id).ToArray();
		for (int i = 0; i < parameters.Length; i++)
		{
			SpawnPointParams @params = parameters[i];
			if (!source.Contains(@params.Id))
			{
				SpawnPointMarker spawnPointMarker = Create(@params, parent);
				Debug.Log(_ED3E._E000(199819) + spawnPointMarker.SpawnPoint.Id + _ED3E._E000(199870) + spawnPointMarker.SpawnPoint.Name + _ED3E._E000(199861));
			}
		}
	}

	public static SpawnPointMarker Create(SpawnPointParams @params, Transform parent = null)
	{
		GameObject gameObject = new GameObject(@params.Id);
		if (parent != null)
		{
			gameObject.transform.parent = parent;
		}
		return Create(@params, gameObject);
	}

	public static SpawnPointMarker Create(SpawnPointParams @params, GameObject go)
	{
		go.layer = _E37B.TriggersLayer;
		go.transform.position = @params.Position;
		go.transform.rotation = Quaternion.Euler(0f, @params.Rotation, 0f);
		SpawnPointMarker spawnPointMarker = go.AddComponent<SpawnPointMarker>();
		spawnPointMarker._spawnPoint = new SpawnPoint
		{
			Id = @params.Id,
			Name = go.name,
			Position = go.transform.position,
			Rotation = go.transform.rotation,
			Sides = @params.Sides,
			Categories = @params.Categories,
			Infiltration = ((!string.IsNullOrWhiteSpace(@params.Infiltration)) ? @params.Infiltration : null),
			DelayToCanSpawnSec = @params.DelayToCanSpawnSec,
			Collider = spawnPointMarker
		};
		if (!string.IsNullOrEmpty(@params.BotZoneName))
		{
			BotZone botZone = _E3AA.FindUnityObjectsOfType<BotZone>().FirstOrDefault((BotZone x) => x.name == @params.BotZoneName);
			if (botZone != null)
			{
				botZone.SpawnPointMarkers.Add(spawnPointMarker);
			}
		}
		go.TryAddSpawnCollider(@params.ColliderParams);
		return spawnPointMarker;
	}

	public void OnDrawGizmos()
	{
		if (_E366.SpawnCategoryMask != 0)
		{
			ESpawnCategoryMask num = _spawnPoint.Categories & _E366.SpawnCategoryMask;
			EPlayerSideMask ePlayerSideMask = _spawnPoint.Sides & _E366.PlayerSideMask;
			if (num != 0 && ePlayerSideMask != 0)
			{
				_E000();
			}
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (_E366.SpawnCategoryMask == ESpawnCategoryMask.None)
		{
			_E000();
		}
		if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo, 50f))
		{
			Gizmos.DrawLine(base.transform.position, hitInfo.point);
		}
		_E001();
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawLine(Vector3.zero, Vector3.zero + Vector3.forward * 2f * 0.3f);
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * 0.3f, Vector3.zero + (Vector3.forward * 1.5f * 0.3f + Vector3.left * 0.5f * 0.3f));
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * 0.3f, Vector3.zero + (Vector3.forward * 1.5f * 0.3f + Vector3.right * 0.5f * 0.3f));
		Gizmos.color = Color;
		float num = 0.5f;
		Gizmos.DrawCube(Vector3.zero + Vector3.up * 2f, (Vector3.forward + Vector3.right) * num);
		Gizmos.DrawCube(Vector3.zero, (Vector3.forward + Vector3.right) * num);
		Vector3 vector = (Vector3.forward + Vector3.right) * num * 0.5f;
		Vector3 vector2 = (Vector3.forward + Vector3.left) * num * 0.5f;
		Vector3 vector3 = (Vector3.back + Vector3.right) * num * 0.5f;
		Vector3 vector4 = (Vector3.back + Vector3.left) * num * 0.5f;
		Vector3 vector5 = Vector3.up * 2f;
		Vector3 vector6 = Vector3.down * 0f;
		Gizmos.DrawLine(vector + vector5, vector + vector6);
		Gizmos.DrawLine(vector2 + vector5, vector2 + vector6);
		Gizmos.DrawLine(vector3 + vector5, vector3 + vector6);
		Gizmos.DrawLine(vector4 + vector5, vector4 + vector6);
	}

	private void _E000()
	{
		if (!Application.isPlaying)
		{
			switch (SpawnPoint.Sides)
			{
			case EPlayerSideMask.Usec:
				Gizmos.color = new Color(0.2f, 0.4f, 0.9f, 0.5f);
				break;
			case EPlayerSideMask.Bear:
				Gizmos.color = new Color(0.9f, 0.4f, 0.2f, 0.5f);
				break;
			case EPlayerSideMask.Savage:
				Gizmos.color = new Color(0.4f, 0.9f, 0.2f, 0.5f);
				break;
			case EPlayerSideMask.None:
				Gizmos.color = new Color(0f, 0f, 0f, 0.5f);
				break;
			default:
				Gizmos.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
				break;
			case EPlayerSideMask.All:
				break;
			}
			_E395.DrawCube(base.transform.position + Vector3.up * 120f * 0.5f, base.transform.rotation, new Vector3(1f, 120f, 1f));
		}
	}

	private void _E001()
	{
		if (!(_collider == null) && _collider.enabled)
		{
			Collider collider = _collider;
			BoxCollider boxCollider;
			if ((object)collider != null && (object)(boxCollider = collider as BoxCollider) == null && collider is SphereCollider sphereCollider)
			{
				SphereCollider sphereCollider2 = sphereCollider;
				Gizmos.DrawWireSphere(SpawnPoint.Position + sphereCollider2.center, sphereCollider2.radius);
			}
		}
	}

	public void Start()
	{
		if (_spawnPoint != null)
		{
			_spawnPoint.Name = base.gameObject.name;
			_spawnPoint.Collider = this;
		}
		if (_collider == null)
		{
			_collider = GetComponent<Collider>();
		}
		if (_collider == null)
		{
			Debug.LogError(_ED3E._E000(199854) + base.name + _ED3E._E000(199893));
		}
		else if (!_collider.enabled)
		{
			Debug.LogError(string.Format(_ED3E._E000(199873), base.name, _collider.GetType()));
		}
		this.m__E000 = _E002();
	}

	private string _E002()
	{
		try
		{
			Collider collider = _collider;
			if ((object)collider != null)
			{
				if (collider is SphereCollider sphereCollider)
				{
					SphereCollider sphereCollider2 = sphereCollider;
					return string.Format(_ED3E._E000(199954), SpawnPoint.Position, sphereCollider2.radius);
				}
				if (collider is BoxCollider boxCollider)
				{
					BoxCollider boxCollider2 = boxCollider;
					return string.Format(_ED3E._E000(199978), boxCollider2.bounds);
				}
			}
			return _ED3E._E000(200018);
		}
		catch (Exception)
		{
			return _ED3E._E000(200059);
		}
	}

	bool ISpawnPointCollider.Contains(Vector3 point)
	{
		if (_collider == null)
		{
			Debug.LogError(_ED3E._E000(200039) + base.name + _ED3E._E000(200073));
			return false;
		}
		if (!_collider.enabled)
		{
			Debug.LogError(_ED3E._E000(200039) + base.name + _ED3E._E000(200116));
			return false;
		}
		Collider collider = _collider;
		if ((object)collider != null)
		{
			if (collider is SphereCollider sphereCollider)
			{
				SphereCollider sphereCollider2 = sphereCollider;
				float sqrMagnitude = (point - SpawnPoint.Position).sqrMagnitude;
				float num = sphereCollider2.radius * sphereCollider2.radius;
				return sqrMagnitude <= num;
			}
			if (collider is BoxCollider boxCollider)
			{
				return boxCollider.bounds.Contains(point);
			}
		}
		Debug.LogError(_ED3E._E000(165519));
		return false;
	}

	public string DebugInfo()
	{
		return this.m__E000;
	}

	public SpawnPointParams CreateSpawnPointParams()
	{
		SpawnPointParams result = default(SpawnPointParams);
		result.Id = _spawnPoint.Id;
		result.Position = _spawnPoint.Position;
		result.Rotation = _spawnPoint.Rotation.eulerAngles.y;
		result.Sides = _spawnPoint.Sides;
		result.Categories = _spawnPoint.Categories;
		result.Infiltration = ((!string.IsNullOrWhiteSpace(_spawnPoint.Infiltration)) ? _spawnPoint.Infiltration : null);
		result.DelayToCanSpawnSec = _spawnPoint.DelayToCanSpawnSec;
		result.BotZoneName = _spawnPoint.BotZoneName;
		result.ColliderParams = base.gameObject.GetSpawnColliderParams();
		return result;
	}

	public bool FixParams(in SpawnPointParams @params, _E315 logger)
	{
		_E001 obj = default(_E001);
		obj.logger = logger;
		obj._003C_003E4__this = this;
		bool flag = false;
		if (@params.Categories != _spawnPoint.Categories)
		{
			_E003(string.Format(_ED3E._E000(200152), _spawnPoint.Categories, @params.Categories), ref obj);
			_spawnPoint.Categories = @params.Categories;
			flag = true;
		}
		if (@params.Sides != _spawnPoint.Sides)
		{
			_E003(string.Format(_ED3E._E000(200185), _spawnPoint.Sides, @params.Sides), ref obj);
			_spawnPoint.Sides = @params.Sides;
			flag = true;
		}
		if (Math.Abs(@params.DelayToCanSpawnSec - _spawnPoint.DelayToCanSpawnSec) > 0.001f)
		{
			_E003(string.Format(_ED3E._E000(200221), _spawnPoint.DelayToCanSpawnSec, @params.DelayToCanSpawnSec), ref obj);
			_spawnPoint.DelayToCanSpawnSec = @params.DelayToCanSpawnSec;
			flag = true;
		}
		string text = ((!string.IsNullOrWhiteSpace(@params.Infiltration)) ? @params.Infiltration : null);
		if (text != _spawnPoint.Infiltration)
		{
			string text2 = _ED3E._E000(200246) + _spawnPoint.Infiltration + _ED3E._E000(200227) + @params.Infiltration;
			if (text == null && string.IsNullOrEmpty(_spawnPoint.Infiltration))
			{
				obj.logger.LogInfo(_ED3E._E000(200276) + Id + _ED3E._E000(200257) + text2);
			}
			else
			{
				_E003(text2, ref obj);
				flag = true;
			}
			_spawnPoint.Infiltration = text;
		}
		if ((@params.Position.ToUnityVector3() - _spawnPoint.Position).magnitude > 0.001f)
		{
			_E003(string.Format(_ED3E._E000(200311), _spawnPoint.Position, @params.Position.ToUnityVector3()), ref obj);
			_spawnPoint.Position = @params.Position;
			flag = true;
		}
		if (Math.Abs(@params.Rotation - _spawnPoint.Rotation.eulerAngles.y) > 0.001f)
		{
			_E003(string.Format(_ED3E._E000(200311), _spawnPoint.Rotation.eulerAngles.y, @params.Rotation), ref obj);
			_spawnPoint.Rotation = Quaternion.Euler(0f, @params.Rotation, 0f);
			flag = true;
		}
		if (_EBD0.IsEqual(@params.ColliderParams, base.gameObject.GetSpawnColliderParams()))
		{
			return flag;
		}
		if (@params.ColliderParams != null)
		{
			bool flag2 = FixCollider(@params.ColliderParams);
			flag = flag || flag2;
			_E003(flag2 ? _ED3E._E000(200378) : _ED3E._E000(200342), ref obj);
		}
		else
		{
			_E003(_ED3E._E000(200354), ref obj);
		}
		return flag;
	}

	public bool FixCollider(ISpawnColliderParams @params)
	{
		if (@params == null)
		{
			return false;
		}
		if (_collider == null)
		{
			return base.gameObject.TryAddSpawnCollider(@params) != null;
		}
		if (@params != null)
		{
			if (@params is SpawnBoxParams spawnBoxParams)
			{
				SpawnBoxParams spawnBoxParams2 = spawnBoxParams;
				if (_collider is BoxCollider boxCollider)
				{
					bool result = false;
					if ((boxCollider.center - spawnBoxParams2.Center).magnitude > 0.001f)
					{
						boxCollider.center = spawnBoxParams2.Center;
						result = true;
					}
					if ((boxCollider.size - spawnBoxParams2.Size).magnitude > 0.001f)
					{
						boxCollider.size = spawnBoxParams2.Size;
						result = true;
					}
					if (!_collider.enabled)
					{
						_collider.enabled = true;
						result = true;
					}
					return result;
				}
				UnityEngine.Object.Destroy(_collider);
				return base.gameObject.TryAddSpawnCollider(@params) != null;
			}
			if (@params is SpawnSphereParams spawnSphereParams)
			{
				SpawnSphereParams spawnSphereParams2 = spawnSphereParams;
				if (_collider is SphereCollider sphereCollider)
				{
					bool result2 = false;
					if ((sphereCollider.center - spawnSphereParams2.Center).magnitude > 0.001f)
					{
						sphereCollider.center = spawnSphereParams2.Center;
						result2 = true;
					}
					if (Math.Abs(sphereCollider.radius - spawnSphereParams2.Radius) > 0.001f)
					{
						sphereCollider.radius = spawnSphereParams2.Radius;
						result2 = true;
					}
					if (!_collider.enabled)
					{
						_collider.enabled = true;
						result2 = true;
					}
					return result2;
				}
				UnityEngine.Object.Destroy(_collider);
				return base.gameObject.TryAddSpawnCollider(@params) != null;
			}
		}
		return false;
	}

	[CompilerGenerated]
	private void _E003(string message, ref _E001 P_1)
	{
		P_1.logger.LogError(_ED3E._E000(200276) + Id + _ED3E._E000(200257) + message);
	}
}
