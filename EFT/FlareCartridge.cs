using System.Collections.Generic;
using Systems.Effects;
using EFT.Ballistics;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.PrefabSettings;
using UnityEngine;

namespace EFT;

public class FlareCartridge : Throwable
{
	private const float _E012 = 3f;

	private const float _E013 = 10f;

	private readonly Vector3 _E014 = new Vector3(5f, 10f, 20f);

	private static PhysicMaterial _E015;

	private FlareColorType _E016;

	private FlareCartridgeSettings _E017;

	private GameObject _E018;

	private Player _E019;

	private _EA12 _E01A;

	private Weapon _E01B;

	private CapsuleCollider _E01C;

	private SphereCollider _E01D;

	private bool _E01E;

	private bool _E01F;

	private bool _E020;

	private float _E021;

	private Vector3 _E022;

	private GameObject _E023;

	private float _E024;

	private float _E025;

	private bool _E026;

	private bool _E027;

	private const float _E028 = 0.2f;

	private const float _E029 = 0.5f;

	private static PhysicMaterial _E000
	{
		get
		{
			if (_E015 == null)
			{
				_E015 = new PhysicMaterial(_ED3E._E000(138960));
				_E015.dynamicFriction = 0.4f;
				_E015.staticFriction = 0.4f;
				_E015.frictionCombine = PhysicMaterialCombine.Average;
				_E015.bounciness = 0.1f;
				_E015.bounceCombine = PhysicMaterialCombine.Average;
			}
			return _E015;
		}
	}

	public override int Id => _E000();

	public override bool HasNetData => false;

	public void Init(FlareCartridgeSettings flareCartridgeSettings, Player player, _EA12 flareCartridge, Weapon weapon)
	{
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
		base.Init(flareCartridgeSettings);
		_E017 = flareCartridgeSettings;
		_E024 = flareCartridgeSettings.SuccesfullHeight;
		_E016 = flareCartridgeSettings.FlareColorType;
		_E01A = flareCartridge;
		_E01B = weapon;
		_E019 = player;
		_E025 = base.transform.position.y;
		_E01E = false;
		_E020 = false;
		_E026 = false;
		_E01F = false;
		_E027 = false;
		_E021 = Time.time;
		base.CollisionNumber = 0;
		_E008();
		_E005();
		Rigidbody = base.gameObject.GetComponent<Rigidbody>();
		if (Rigidbody == null)
		{
			Rigidbody = base.gameObject.AddComponent<Rigidbody>();
		}
		_E320._E002.SupportRigidbody(Rigidbody);
		Rigidbody.isKinematic = false;
		Rigidbody.mass = flareCartridgeSettings.Weight;
		Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		Rigidbody.drag = 0f;
		_E023 = Object.Instantiate(flareCartridgeSettings.ProjectileAmmo, base.transform, worldPositionStays: false);
		_E023.transform.forward = base.transform.forward;
		_E017.SwitchMeshRenderersActive(value: false);
		if (flareCartridgeSettings.FlareEffectPrefab != null)
		{
			_E018 = Object.Instantiate(flareCartridgeSettings.FlareEffectPrefab, flareCartridgeSettings.transform);
			FlareShotEffectSelector component = _E018.GetComponent<FlareShotEffectSelector>();
			if (component != null)
			{
				component.SetFlareEffect(flareCartridgeSettings.FlareColorType, flareCartridgeSettings.FlareLifetime);
			}
			_E018.SetActive(value: false);
		}
		base.enabled = true;
	}

	private int _E000()
	{
		if (!_E01B.IsOneOff)
		{
			return _E01A.Id.GetHashCode();
		}
		return _E01B.Id.GetHashCode();
	}

	private bool _E001(Collider other, out bool contactWithOwnerPlayer)
	{
		contactWithOwnerPlayer = false;
		BodyPartCollider component = other.GetComponent<BodyPartCollider>();
		if (component == null || component.Player == null || !component.Player.HealthController.IsAlive)
		{
			return false;
		}
		if (component.Player == _E019)
		{
			contactWithOwnerPlayer = true;
			return true;
		}
		Player player = component.Player;
		RaycastHit[] array = new RaycastHit[1];
		int num = Physics.RaycastNonAlloc(new Ray(base.transform.position, base.transform.forward), array, 0.15f, _E37B.HitColliderMask);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				RaycastHit hit = array[i];
				BodyPartCollider component2 = array[i].collider.GetComponent<BodyPartCollider>();
				if (component2 != null && component2.Player == player)
				{
					_E002(component2, player, hit);
					return true;
				}
			}
		}
		return false;
	}

	private void _E002(BodyPartCollider bodyPart, Player player, RaycastHit hit)
	{
		float damage = _E003(bodyPart, hit);
		_EC23 obj = default(_EC23);
		obj.DamageType = EDamageType.Bullet;
		obj.Damage = damage;
		obj.ArmorDamage = _E01A.ArmorDamage;
		obj.PenetrationPower = _E01A.PenetrationPower;
		obj.Direction = Velocity.normalized;
		obj.HitCollider = bodyPart.Collider;
		obj.HitNormal = hit.normal;
		obj.HitPoint = hit.point;
		obj.HittedBallisticCollider = bodyPart;
		obj.Player = _E019;
		obj.Weapon = _E01B;
		obj.LightBleedingDelta = -10f;
		obj.HeavyBleedingDelta = -10f;
		_EC23 damageInfo = obj;
		if (bodyPart.BodyPartType == EBodyPart.Head && _E01E)
		{
			Dictionary<Player, _E6DF> dictionary = new Dictionary<Player, _E6DF>();
			List<Player> players = new List<Player> { player };
			_E6DF value = new _E6DF
			{
				Distance = Vector3.Distance(player.Position, base.transform.position),
				DirectionToEmitter = (base.transform.position - player.PlayerBones.Head.position).normalized,
				TryToApplyStun = false,
				TryToApplyBurnEyes = true,
				TryToApplyContusion = false
			};
			dictionary.Add(player, value);
			_E6E0.ApplyLightAndSoundHealthEffects(players, dictionary, base.transform.position, _E014);
		}
		player.ApplyShot(damageInfo, bodyPart.BodyPartType, _EC22.EMPTY_SHOT_ID);
	}

	private float _E003(BodyPartCollider bodyPart, RaycastHit hit)
	{
		if (bodyPart.IsHitToArmor(hit.point, hit.normal, base.transform.forward))
		{
			return 0f;
		}
		return Rigidbody.velocity.magnitude / _E017.InitialSpeed * (float)_E01A.Damage;
	}

	private bool _E004(Collision collision)
	{
		WindowBreaker componentInParent = collision.gameObject.GetComponentInParent<WindowBreaker>();
		if (componentInParent == null)
		{
			return false;
		}
		BallisticCollider ballisticCollider = (componentInParent.IsDamaged ? collision.gameObject.GetComponent<BallisticCollider>() : componentInParent.GlassBallisticCollider);
		if (ballisticCollider == null || ballisticCollider.TypeOfMaterial != MaterialType.Glass)
		{
			return false;
		}
		ballisticCollider.ApplyHit(new _EC23
		{
			DamageType = EDamageType.Blunt,
			HitPoint = collision.contacts[0].point,
			Direction = Velocity
		}, _EC22.EMPTY_SHOT_ID);
		return true;
	}

	public void Launch()
	{
		Vector3 velocity = (Velocity = _E017.InitialSpeed * base.transform.forward);
		Rigidbody.velocity = velocity;
		_E022 = _E019.Position;
	}

	private void _E005()
	{
	}

	private void Update()
	{
		_E005();
		if (!_E01E && !_E01F && Time.time > _E021 + _E017.FlareTimeAfterStart)
		{
			_E006();
			_E01E = true;
		}
		if (_E01E && !_E020 && base.CollisionNumber <= 0 && base.transform.position.y - _E025 > _E024)
		{
			_E020 = true;
			_E019.HandleFlareSuccessEvent(base.transform.position, _E017.FlareEventType);
			_EBAF.Instance.CreateCommonEvent<_EBBA>().Invoke(_E019, _E022, base.transform.position, _E017.FlareEventType);
		}
		if (_E01E && Time.time + 3f > _E021 + _E017.FlareLifetime)
		{
			_E01E = false;
			_E01F = true;
		}
		if (_E01F && Time.time > _E021 + _E017.FlareLifetime)
		{
			_E007();
		}
	}

	private void _E006()
	{
		Rigidbody.drag = _E017.RigidbodyDrag;
		if (_E023 != null)
		{
			_E023.SetActive(value: false);
		}
		if (_E018 != null)
		{
			_E018.SetActive(value: true);
		}
	}

	private void _E007()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	private void _E008()
	{
		_E01C = GetComponent<CapsuleCollider>();
		if (_E01C == null)
		{
			_E01C = base.gameObject.AddComponent<CapsuleCollider>();
		}
		_E01C.radius = 0.01f;
		_E01C.center = new Vector3(0f, 0f, -0.005f);
		_E01C.height = 0.05f;
		_E01C.direction = 2;
		_E01C.sharedMaterial = FlareCartridge._E000;
		if (_E019 != null)
		{
			_E320.IgnoreCollision(_E019.CharacterControllerCommon.GetCollider(), _E01C);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (base.CollisionNumber > 0 || _E01F)
		{
			return;
		}
		if (_E001(collision.collider, out var contactWithOwnerPlayer))
		{
			if (!contactWithOwnerPlayer)
			{
				base.CollisionNumber++;
			}
		}
		else if (collision.impulse.sqrMagnitude > 0.5f && _E004(collision))
		{
			Physics.IgnoreCollision(collision.collider, _E01C);
			Rigidbody.velocity = Velocity;
			base.CollisionNumber++;
		}
		else if (!(Time.time < IgnoreCollisionTrackingTimer))
		{
			base.CollisionNumber++;
		}
	}

	public override void OnCollisionHandler()
	{
		if (!_E027 && !(Rigidbody == null))
		{
			Rigidbody.drag = _E017.CollisionDrag;
			_E027 = true;
		}
	}
}
