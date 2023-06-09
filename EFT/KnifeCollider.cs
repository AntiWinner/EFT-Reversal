using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT;

public class KnifeCollider : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RaycastHit x;

		internal bool _E000(BodyPartCollider c)
		{
			return c.gameObject != x.collider.gameObject;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Collider x;

		internal bool _E000(BodyPartCollider c)
		{
			return c.gameObject != x.gameObject;
		}
	}

	public BoxCollider Collider;

	public Vector3 CastDirection;

	public RaycastHit[] hits = new RaycastHit[2];

	public Action<Player._E00D> OnHit;

	public float MaxDistance = 1f;

	internal Player.BaseKnifeController _E000;

	internal Player _E001;

	public int _hitMask;

	public int _spiritMask;

	private Collider[] m__E002 = new Collider[16];

	private bool m__E003;

	private Vector3 m__E004;

	private Vector3 m__E005;

	private Quaternion m__E006;

	private HashSet<Player> m__E007 = new HashSet<Player>();

	private Vector3 m__E008 = Vector3.one;

	[Header("Server setting")]
	[SerializeField]
	private float _weaponRootExtension = 0.7f;

	public void Awake()
	{
		Collider.enabled = false;
	}

	public void OnFire()
	{
	}

	public void OnFireEnd()
	{
		this.m__E007.Clear();
	}

	public void SetBotParameters(Vector3 colliderScaleMultiplier)
	{
		this.m__E008 = colliderScaleMultiplier;
	}

	public void ManualUpdate()
	{
		if (OnHit != null)
		{
			if (this._E001.IsAI)
			{
				_E001();
			}
			else
			{
				_E000();
			}
		}
	}

	private void _E000()
	{
		hits = new RaycastHit[2];
		Vector3 playerOrientation = this._E000.GetPlayerOrientation();
		Vector3 size = Collider.size;
		Quaternion rotation = base.transform.rotation;
		Vector3 position = base.transform.position;
		Physics.BoxCastNonAlloc(position + rotation * Collider.center - playerOrientation * size.y, new Vector3(size.x / 2f, size.y / 4f, size.z / 2f), this._E000.GetPlayerOrientation(), orientation: rotation, maxDistance: (MaxDistance >= float.Epsilon) ? MaxDistance : (Collider.size.y * 2.5f), layerMask: _hitMask, results: hits, queryTriggerInteraction: QueryTriggerInteraction.Collide);
		_E57F.DrawBox(position + rotation * Collider.center - playerOrientation * Collider.size.y, size / 2f, rotation, Color.cyan);
		_E57F.DrawBox(position + rotation * Collider.center - playerOrientation * Collider.size.y + playerOrientation * (size.y * 2f), size / 2f, rotation, Color.red);
		RaycastHit hit = hits.FirstOrDefault((RaycastHit x) => x.collider != null && this._E001.PlayerBones.BodyPartColliders.All((BodyPartCollider c) => c.gameObject != x.collider.gameObject));
		if (hit.collider != null)
		{
			OnHit(new Player._E00D(hit));
		}
	}

	private void _E001()
	{
		if (this._E001.IsAI && OnHit != null)
		{
			Vector3 position = this._E001.WeaponRoot.position;
			Vector3 lookDirection = this._E001.LookDirection;
			Vector3 size = _E38D.GetColliderBoundsWithoutRotation(Collider).size;
			size.x *= this.m__E008.x;
			size.z *= this.m__E008.z;
			size.y += _weaponRootExtension;
			_E002(position, lookDirection, size);
		}
	}

	private void _E002(Vector3 startPoint, Vector3 direction, Vector3 castSize)
	{
		castSize.y = _E006(startPoint, direction, castSize.y) * this.m__E008.y;
		Vector3 halfExtents = castSize * 0.5f;
		Physics.OverlapBoxNonAlloc(startPoint + direction * halfExtents.y, orientation: Quaternion.LookRotation(direction) * Quaternion.AngleAxis(90f, Vector3.left), halfExtents: halfExtents, results: this.m__E002, mask: _hitMask, queryTriggerInteraction: QueryTriggerInteraction.Collide);
		Collider collider = this.m__E002.FirstOrDefault((Collider x) => x != null && this._E001.PlayerBones.BodyPartColliders.All((BodyPartCollider c) => c.gameObject != x.gameObject));
		if (collider != null)
		{
			OnHit(new Player._E00D
			{
				collider = collider,
				point = collider.bounds.center,
				normal = Vector3.zero
			});
		}
	}

	private void _E003(PlayerSpirit victimSpirit)
	{
		if (!this.m__E007.Contains(victimSpirit._player))
		{
			this.m__E007.Add(victimSpirit._player);
		}
	}

	private void _E004(Vector3 startPoint, Vector3 direction, Player victimPlayer, Vector3 castSize)
	{
		int num = _E007(startPoint, direction, _EC20.SpiritHitMaskSecondPass, this.m__E002, castSize);
		Collider collider = null;
		for (int i = 0; i < num; i++)
		{
			Collider collider2 = this.m__E002[i];
			BodyPartCollider component = collider2.GetComponent<BodyPartCollider>();
			if (component != null && component.Player == victimPlayer)
			{
				collider = collider2;
				break;
			}
		}
		if (collider != null)
		{
			OnHit(new Player._E00D
			{
				collider = collider,
				point = collider.bounds.center,
				normal = Vector3.zero
			});
		}
	}

	private PlayerSpirit _E005(Vector3 startPoint, Vector3 direction, Vector3 castSize)
	{
		int num = _E007(startPoint, direction, _EC20.PlayerSpiritAuraLayerMask, this.m__E002, castSize);
		if (num > 0)
		{
			Collider collider = null;
			Collider collider2 = this._E001.Spirit.PlayerSpiritAura.GetCollider();
			for (int i = 0; i < num; i++)
			{
				Collider collider3 = this.m__E002[i];
				if (collider3 != collider2)
				{
					collider = collider3;
					break;
				}
			}
			if (collider != null)
			{
				Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(collider);
				if (playerByCollider == null || playerByCollider.Spirit == null || !playerByCollider.Spirit.IsActive)
				{
					return null;
				}
				return playerByCollider.Spirit;
			}
		}
		return null;
	}

	private float _E006(Vector3 startPoint, Vector3 direction, float maxDistance)
	{
		if (Physics.Raycast(new Ray(startPoint, direction), out var hitInfo, maxDistance, _EC20.StaticObjectsHitMask, QueryTriggerInteraction.Ignore))
		{
			return hitInfo.distance;
		}
		return maxDistance;
	}

	private int _E007(Vector3 startPoint, Vector3 direction, int mask, Collider[] colliders, Vector3 castSize)
	{
		Vector3 halfExtents = castSize * 0.5f;
		Vector3 vector = startPoint + direction * halfExtents.y;
		Quaternion quaternion = Quaternion.LookRotation(direction) * Quaternion.AngleAxis(90f, Vector3.left);
		int result = Physics.OverlapBoxNonAlloc(vector, halfExtents, colliders, quaternion, mask, QueryTriggerInteraction.Ignore);
		this.m__E003 = true;
		this.m__E004 = vector;
		this.m__E005 = castSize;
		this.m__E006 = quaternion;
		return result;
	}

	private void OnDrawGizmos()
	{
		if (this.m__E003)
		{
			Gizmos.matrix = Matrix4x4.TRS(this.m__E004, this.m__E006, Vector3.one);
			Gizmos.color = Color.red;
			Gizmos.DrawCube(Vector3.zero, this.m__E005);
			this.m__E003 = false;
		}
	}

	[CompilerGenerated]
	private bool _E008(RaycastHit x)
	{
		if (x.collider != null)
		{
			return this._E001.PlayerBones.BodyPartColliders.All((BodyPartCollider c) => c.gameObject != x.collider.gameObject);
		}
		return false;
	}

	[CompilerGenerated]
	private bool _E009(Collider x)
	{
		if (x != null)
		{
			return this._E001.PlayerBones.BodyPartColliders.All((BodyPartCollider c) => c.gameObject != x.gameObject);
		}
		return false;
	}
}
