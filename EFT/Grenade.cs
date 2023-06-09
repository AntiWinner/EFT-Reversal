using System.Collections;
using System.Runtime.CompilerServices;
using Systems.Effects;
using Comfort.Common;
using EFT.Ballistics;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

public class Grenade : Throwable
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Item originalWeaponItem;

		public Player playerWhoThrew;

		public Vector3 grenadePosition;

		internal _EC23 _E000()
		{
			return _E002(originalWeaponItem, playerWhoThrew, grenadePosition);
		}
	}

	public const float SHIFT_DISTANCE = 0.08f;

	private const int _E02A = 300;

	private static readonly Vector3 _E02B = new Vector3(0f, 0.08f, 0f);

	public _EC1E Calculator;

	[CompilerGenerated]
	private Player _E02C;

	[CompilerGenerated]
	private _EADF _E02D;

	public static _EC17 GrenadeRandoms;

	protected GrenadeSettings _grenadeSettings;

	private IEnumerator _E02E;

	private bool _E02F;

	private float _E030;

	private float _E031;

	private Collider _E032;

	private Transform _E033;

	private static int _E034;

	private SoundBank _E035;

	private readonly Vector3 _E036 = new Vector3(0f, 0.1f, 0f);

	private const float _E028 = 0.2f;

	private const float _E029 = 0.5f;

	public Player Player
	{
		[CompilerGenerated]
		get
		{
			return _E02C;
		}
		[CompilerGenerated]
		private set
		{
			_E02C = value;
		}
	}

	public _EADF WeaponSource
	{
		[CompilerGenerated]
		get
		{
			return _E02D;
		}
		[CompilerGenerated]
		private set
		{
			_E02D = value;
		}
	}

	public GrenadeSettings GrenadeSettings => _grenadeSettings;

	public Vector3 Offset => _grenadeSettings.Offset;

	public override int Id => WeaponSource?.Id.GetHashCode() ?? (-1);

	protected virtual float PhysicsQuality => 1f;

	protected static float PhysicsQualityForObserved => 0f;

	public override bool HasNetData => false;

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		_E032 = GetComponent<Collider>();
		_E320._E002.SupportRigidbody(Rigidbody, visibilityChecker: GetVisibilityChecker(), quality: PhysicsQuality);
	}

	protected virtual _E383 GetVisibilityChecker()
	{
		return null;
	}

	private void OnCollisionEnter(Collision collision)
	{
		float sqrMagnitude = collision.impulse.sqrMagnitude;
		if (sqrMagnitude > 0.5f && _E000(collision))
		{
			Physics.IgnoreCollision(collision.collider, _E032);
			Rigidbody.velocity = Velocity;
		}
		else if (!(sqrMagnitude < 0.2f) && !(Time.time < IgnoreCollisionTrackingTimer))
		{
			base.CollisionNumber++;
			ProcessContactExplodeCollision(collision.impulse.magnitude);
		}
	}

	protected virtual void ProcessContactExplodeCollision(float impulse)
	{
		if (!(WeaponSource.MinTimeToContactExplode < 0f) && !(_E030 < WeaponSource.MinTimeToContactExplode))
		{
			StopTimer();
			_E02F = true;
		}
	}

	public override void OnCollisionHandler()
	{
		Vector3 position = base.transform.position + _E036;
		Singleton<BetterAudio>.Instance.PlayAtPoint(position, _E035, _E8A8.Instance.Distance(position), 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		base.OnCollisionHandler();
	}

	private bool _E000(Collision collision)
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

	protected static _E383 GetVisibilityCheckerForObserved(Grenade grenade)
	{
		if (_E8A8.Instance.Camera != null)
		{
			return new _E383(_E8A8.Instance.Camera, grenade.gameObject, _E2B6.Config.Physics.CullingForGrenade);
		}
		return null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Calculator != null && Singleton<_E5CE>.Instantiated)
		{
			Singleton<_E5CE>.Instance.RemoveBallisticCalculator(WeaponSource);
		}
	}

	public virtual void SetThrowForce(Vector3 force)
	{
		Velocity = force;
		Rigidbody.AddForce(force, ForceMode.Impulse);
	}

	public virtual void Init(GrenadeSettings settings, Player player, _EADF throwWeap, float timeSpent, _EC1E calculator)
	{
		base.Init(settings);
		_grenadeSettings = settings;
		Player = player;
		if (Player != null)
		{
			_E320.IgnoreCollision(Player.CharacterControllerCommon.GetCollider(), _E032);
		}
		WeaponSource = throwWeap;
		Calculator = calculator;
		_E031 = timeSpent;
		WeaponSounds weaponSounds = Singleton<_ED0A>.Instance.InstantiateAsset<WeaponSounds>(_ED3E._E000(35307));
		switch (_grenadeSettings.CollisionSound)
		{
		case GrenadeSettings.CollisionSounds.frag:
			_E035 = weaponSounds.GrenadeDropSoundBank;
			break;
		case GrenadeSettings.CollisionSounds.smoke:
			_E035 = weaponSounds.SmokeGrenadeCollisions;
			break;
		case GrenadeSettings.CollisionSounds.stun:
			_E035 = weaponSounds.StunGrenadeCollisions;
			break;
		case GrenadeSettings.CollisionSounds.smokeM18:
			_E035 = weaponSounds.SmokeGrenadeM18Collisions;
			break;
		case GrenadeSettings.CollisionSounds.stunM7920:
			_E035 = weaponSounds.StunGrenadeM7920Collisions;
			break;
		}
		StartTimer();
		_E030 = 0f;
	}

	protected virtual void StartTimer()
	{
		_E02E = this.StartBehaviourTimer(WeaponSource.GetExplDelay - _E031, InvokeBlowUpEvent);
	}

	protected void StopTimer()
	{
		if (_E02E != null)
		{
			this.StopBehaviourTimer(ref _E02E);
		}
		_E02E = null;
	}

	protected void InvokeBlowUpEvent()
	{
		_E001();
		OnExplosion();
	}

	public virtual void OnExplosion()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	public void Attach(Transform t)
	{
		_E033 = t;
	}

	public virtual void LateUpdate()
	{
		_E030 += Time.deltaTime;
		if (_E02F)
		{
			InvokeBlowUpEvent();
		}
		else if (_E033 != null)
		{
			base.transform.rotation = _E033.rotation;
			base.transform.position = _E033.position + _E033.rotation * _grenadeSettings.Offset;
		}
	}

	private void _E001()
	{
		if (!string.IsNullOrEmpty(WeaponSource.ExplosionEffectType))
		{
			Singleton<Effects>.Instance.EmitGrenade(WeaponSource.ExplosionEffectType, base.transform.position, Vector3.up);
		}
		Explosion(this, WeaponSource, base.transform.position, Player, Calculator, WeaponSource, _E02B);
	}

	public static void Explosion(Grenade grenade, IExplosiveItem grenadeItem, Vector3 grenadePosition, Player playerWhoThrew, _EC1E grenadeBallisticsCalculator, Item originalWeaponItem, Vector3 shift)
	{
		if (Singleton<_E307>.Instantiated)
		{
			SmokeGrenade smokeGrenade = grenade as SmokeGrenade;
			Singleton<_E307>.Instance.GrenadeExplosion(grenadePosition, playerWhoThrew, smokeGrenade != null, smokeGrenade?.Radius ?? 0f, smokeGrenade?.LifeTime ?? 0f);
			grenadePosition += shift;
			grenadeItem.Explosion(grenadePosition, playerWhoThrew, grenadeBallisticsCalculator, originalWeaponItem, () => _E002(originalWeaponItem, playerWhoThrew, grenadePosition), 0f, 0f, null);
		}
	}

	private static _EC23 _E002(Item originalItemWeapon, Player playerWhoThrew, Vector3 explosionPosition)
	{
		_EC26 shot = _EC26.Create(originalItemWeapon, 0, 0, explosionPosition, Vector3.zero, 0f, 0f, 0.5f, 0.002f, 0f, 0f, 0f, 0f, 0f, 1f, 0, 0, null, null, 1f, playerWhoThrew, originalItemWeapon, -1, null);
		_EC23 result = new _EC23(EDamageType.GrenadeFragment, shot);
		_EC26.Release(shot);
		return result;
	}
}
